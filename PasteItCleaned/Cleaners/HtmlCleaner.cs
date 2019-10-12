using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using System.IO;
using System.Text;

using HtmlAgilityPack;

using AngleSharp.Html;
using AngleSharp.Html.Parser;
using PasteItCleaned.Entities;

namespace PasteItCleaned.Cleaners
{
    public class HtmlCleaner : BaseCleaner
    {
        private string[] ValideAttributes = { "style", "colspan", "rowspan", "src" };

        //<font size="5"> becomes <h1> <font size="4"> becomes <h2> etc.)
        // replace images with inline image data, resize images if necessary
        //remove <a name=...> code as these are usually useless.

        public override SourceType GetSourceType()
        {
            return SourceType.Unknown;
        }

        public override string Clean(string content, Config config)
        {
            var cleaned = content;

            cleaned = base.Clean(cleaned, config);

            cleaned = base.SafeExec(this.RemoveComments, cleaned);
            cleaned = base.SafeExec(this.Compact, cleaned);
            cleaned = base.SafeExec(this.Indent, cleaned);

            return cleaned;
        }

        protected string Compact(string content)
        {
            content = content.Replace("  ", " ");
            content = content.Replace("\n", "");
            content = content.Replace("\t", "");
            content = content.Replace("\r", "");
            content = content.Trim();

            return content;
        }

        protected string Indent(string content)
        {
            var parser = new HtmlParser();
            var document = parser.ParseDocument(content);

            using (var writer = new StringWriter())
            {
                document.ToHtml(writer, new PrettyMarkupFormatter()
                {
                    Indentation = "\t",
                    NewLine = "\n"
                });

                return writer.ToString();
            }
        }


        protected string ParseWithHtmlAgilityPack(string content)
        {
            var htmlDoc = new HtmlDocument();

            htmlDoc.LoadHtml(content);

            return htmlDoc.DocumentNode.OuterHtml;
        }


        protected string RemoveUselessAttributes(string content)
        {
            var htmlDoc = new HtmlDocument();

            htmlDoc.LoadHtml(content);

            foreach (HtmlNode node in htmlDoc.DocumentNode.ChildNodes)
                RemoveUselessAttributesNode(node);

            return htmlDoc.DocumentNode.OuterHtml;
        }

        private void RemoveUselessAttributesNode(HtmlNode node)
        {
            foreach (HtmlNode n in node.ChildNodes)
            {
                if (n.HasChildNodes)
                    RemoveUselessAttributesNode(n);

                for (int i = n.Attributes.Count - 1; i >= 0; i--)
                {
                    var attr = n.Attributes[i];
                    var valid = false;

                    if (!string.IsNullOrWhiteSpace(attr.Value))
                        foreach (var att in ValideAttributes)
                            if (attr.Name.Trim().ToLower() == att)
                                valid = true;

                    if (!valid)
                        n.Attributes.Remove(attr);
                }
            }
        }


        protected string RemoveUselessStyles(string content)
        {
            var htmlDoc = new HtmlDocument();

            htmlDoc.LoadHtml(content);

            foreach (HtmlNode node in htmlDoc.DocumentNode.ChildNodes)
                RemoveUselessStylesNode(node);

            return htmlDoc.DocumentNode.OuterHtml;
        }

        private void RemoveUselessStylesNode(HtmlNode node)
        {
            foreach (HtmlNode n in node.ChildNodes)
            {
                if (n.HasChildNodes)
                    RemoveUselessStylesNode(n);

                for (int i = n.Attributes.Count - 1; i >= 0; i--)
                {
                    var attr = n.Attributes[i];

                    if (attr.Name.Trim().ToLower() == "style")
                    {
                        var newStyles = new StringBuilder();
                        var modifiers = attr.Value.Split(";");

                        foreach (string modifier in modifiers)
                        {
                            var modifierName = modifier.Split(":")[0];
                            var modifierValue = modifier.Split(":")[1];

                            if (IsModifierValid(modifierName, modifierValue))
                                newStyles.AppendFormat("{0}: {1}; ", modifierName, modifierValue);
                        }

                        attr.Value = newStyles.ToString();
                    }
                }
            }
        }


        protected string AddInlineStyles(string content)
        {
            var htmlDoc = new HtmlDocument();
            var styles = this.ParseCssClasses(content);

            htmlDoc.LoadHtml(content);

            foreach (HtmlNode node in htmlDoc.DocumentNode.ChildNodes)
                AddInlineStylesNode(styles, node);

            return htmlDoc.DocumentNode.OuterHtml;
        }

        protected void AddInlineStylesNode(Dictionary<string, Dictionary<string, string>> styles, HtmlNode node)
        {
            foreach (HtmlNode n in node.ChildNodes)
            {
                if (n.HasChildNodes)
                    AddInlineStylesNode(styles, n);

                var classAttr = this.FindOrCreateAttr(n, "class");

                if (classAttr != null)
                {
                    var styleAttr = this.FindOrCreateAttr(n, "style");

                    styleAttr.Value += this.GetInlineStylesForTag(styles, n.Name);
                    styleAttr.Value += this.GetInlineStylesForClass(styles, classAttr.Value);
                }
            }
        }


        protected string RemoveIframes(string content)
        {
            var pattern = @"<(iframe|!\[)[^>]*?>";

            return Regex.Replace(content, pattern, "", RegexOptions.Singleline);
        }

        protected string RemoveUselessTags(string content)
        {
            var pattern = @"<(meta|link|/?o:|/?v:|/?style|/?title|/?div|/?std|/?head|/?html|/?body|/?script|/?col|/?colgroup|!\[)[^>]*?>";

            content = content.Replace("<font", "<span");
            content = content.Replace("</font>", "</span>");

            return Regex.Replace(content, pattern, "", RegexOptions.Singleline);
        }

        protected string RemoveComments(string content)
        {
            var pattern = "<!--.*?-->";

            return Regex.Replace(content, pattern, "", RegexOptions.Singleline);
        }




        private HtmlAttribute FindOrCreateAttr(HtmlNode n, string name)
        {
            if (!(n.Name.StartsWith("#")))
            {
                for (int i = n.Attributes.Count - 1; i >= 0; i--)
                {
                    var attr = n.Attributes[i];

                    if (attr.Name.Trim().ToLower() == name.Trim().ToLower())
                        return attr;
                }

                n.Attributes.Add(name, "");

                return this.FindOrCreateAttr(n, name);
            }

            return null;
        }

        private string GetInlineStylesForClass(Dictionary<string, Dictionary<string, string>> styles, string className)
        {
            var inlineStyles = new StringBuilder();

            foreach (var classe in styles)
                if (classe.Key == "." + className.ToLower().Trim())
                    foreach (var modifier in classe.Value)
                        inlineStyles.AppendFormat("{0}: {1}; ", modifier.Key, modifier.Value);

            return inlineStyles.ToString().Trim();
        }

        private string GetInlineStylesForTag(Dictionary<string, Dictionary<string, string>> styles, string tagName)
        {
            var inlineStyles = new StringBuilder();

            foreach (var classe in styles)
                if (classe.Key == tagName.ToLower().Trim())
                    foreach (var modifier in classe.Value)
                        inlineStyles.AppendFormat("{0}: {1}; ", modifier.Key, modifier.Value);

            return inlineStyles.ToString().Trim();
        }

        private bool IsModifierValid(string name, string value)
        {
            if (name.ToLower().StartsWith("mso-")) return false;
            if (name.ToLower() == "color" && value.ToLower() == "windowtext") return false;
            if (name.ToLower().StartsWith("margin")) return false;
            if (name.ToLower().StartsWith("page-break")) return false;
            if (name.ToLower().StartsWith("tab-stops")) return false;

            return true;
        }

        private Dictionary<string, Dictionary<string, string>> ParseCssClasses(string content)
        {
            var allClasses = new Dictionary<string, Dictionary<string, string>>();
            var patternStyles = @"<style>.*?<\!--(?<styles>.*?)-->.*?</style>";
            var patternClass = @"(?<name>[a-zA-Z0-9-@#\.]+?)[\s\t]*?{(?<class>.*?)}";
            var patternStyle = @"(?<modifier>[a-zA-Z0-9-]+?)[\s\t]*?:[\s\t]*?(?<value>.*?);";
            var comments = Regex.Matches(content, patternStyles, RegexOptions.IgnoreCase | RegexOptions.Singleline | RegexOptions.Multiline);

            foreach (Match comment in comments)
            {
                var styles = comment.Groups["styles"].Value;
                var classes = Regex.Matches(styles, patternClass, RegexOptions.IgnoreCase | RegexOptions.Singleline | RegexOptions.Multiline);

                foreach (Match classe in classes)
                {
                    var className = classe.Groups["name"].Value;
                    var classContent = classe.Groups["class"].Value;
                    var modifiers = Regex.Matches(classContent, patternStyle, RegexOptions.IgnoreCase | RegexOptions.Singleline | RegexOptions.Multiline);

                    foreach (Match modifier in modifiers)
                    {
                        var modifierName = modifier.Groups["modifier"].Value;
                        var modifierValue = modifier.Groups["value"].Value;

                        AddClassToList(allClasses, className, modifierName, modifierValue);
                    }
                }
            }

            return allClasses;
        }

        private void AddClassToList(Dictionary<string, Dictionary<string, string>> allClasses, string className, string modifierName, string modifierValue)
        {
            if (this.IsModifierValid(modifierName, modifierValue))
            {
                if (!allClasses.ContainsKey(className.ToLower().Trim()))
                    allClasses.Add(className.ToLower().Trim(), new Dictionary<string, string>());

                if (!allClasses[className.ToLower().Trim()].ContainsKey(modifierName.ToLower().Trim()))
                    allClasses[className.ToLower().Trim()].Add(modifierName.ToLower().Trim(), modifierValue.ToLower().Trim());

                allClasses[className.ToLower().Trim()][modifierName.ToLower().Trim()] = modifierValue.ToLower().Trim();
            }
        }
    }
}
