using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Text.RegularExpressions;
using System.IO;
using HtmlAgilityPack;
using System.Xml;
using AngleSharp.Html;
using AngleSharp.Html.Parser;
using System.Text;

namespace PasteItCleaned.Cleaners
{
    public class HtmlCleaner : BaseCleaner
    {
        private string[] ValideAttributes = { "style", "colspan", "rowspan" };

        //<font size="5"> becomes <h1> <font size="4"> becomes <h2> etc.)
        // replace images with inline image data, resize images if necessary
        //remove <a name=...> code as these are usually useless.

        public override SourceType GetSourceType()
        {
            return SourceType.Unknown;
        }

        public override string Clean(string content)
        {
            var cleaned = content;

            cleaned = base.Clean(cleaned);

            cleaned = base.SafeExec(this.RemoveComments, cleaned);
            //cleaned = base.SafeExec(this.RemoveScriptTags, cleaned);
            cleaned = base.SafeExec(this.RemoveUselessAttributes, cleaned);
            //cleaned = base.SafeExec(this.RemoveSpecialCharacters, cleaned);
            //cleaned = base.SafeExec(this.CreateRealLineReturns, cleaned);
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
                {
                    RemoveUselessAttributesNode(n);
                }

                for (int i = n.Attributes.Count - 1; i >= 0; i--)
                {
                    var attr = n.Attributes[i];
                    var valid = false;

                    foreach (var att in ValideAttributes)
                        if (attr.Name.Trim().ToLower() == att) valid = true;

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
            var patternStyle = @"(?<modifier>[a-zA-Z0-9-]+?)[\s\t]*?:[\s\t]*?(?<value>.*?);";

            foreach (HtmlNode n in node.ChildNodes)
            {
                if (n.HasChildNodes)
                    RemoveUselessStylesNode(n);

                for (int i = n.Attributes.Count - 1; i >= 0; i--)
                {
                    var attr = n.Attributes[i];
                    var valid = false;

                    foreach (var att in ValideAttributes)
                    {
                        if (attr.Name.Trim().ToLower() == "style")
                        {
                            var newStyles = new StringBuilder();
                            var modifiers = Regex.Matches(attr.Value, patternStyle, RegexOptions.IgnoreCase | RegexOptions.Singleline | RegexOptions.Multiline);

                            foreach (Match modifier in modifiers)
                            {
                                var modifierName = modifier.Groups["modifier"].Value;
                                var modifierValue = modifier.Groups["value"].Value;

                                if (IsModifierValid(modifierName, modifierValue))
                                    newStyles.AppendFormat("{0}: {1}; ", modifierName, modifierValue);
                            }

                            attr.Value = newStyles.ToString();
                        }
                    }

                    if (!valid)
                        n.Attributes.Remove(attr);
                }
            }
        }




        /*protected string RemoveSpecialCharacters(string content)
        {
            content = content.Replace("&rsquo;", "'");
            content = content.Replace("&lsquo;", "'");
            content = content.Replace("&ndash;", "--");
            content = content.Replace("&mdash;", "--");
            content = content.Replace("&hellip;", "...");
            content = content.Replace("&quot;", "\"");
            content = content.Replace("&ldquo;", "\"");
            content = content.Replace("&rdquo;", "\"");
            content = content.Replace("&bull;", "");
            content = content.Replace("", "");

            return content;
        }*/

        /*protected string RemoveClassnames(string content)
        {
            var pattern = @"\s?class=\w+";

            return Regex.Replace(content, pattern, "");
        }*/

        protected string RemoveUselessTags(string content)
        {
            var pattern = @"<(meta|link|/?o:|/?style|/?title|/?div|/?std|/?head|/?html|/?body|/?font|/?span|/?script|!\[)[^>]*?>";

            return Regex.Replace(content, pattern, "", RegexOptions.Singleline);
        }

        protected string RemoveEmptyParagraphs(string content)
        {
            var pattern = @"(<[^>]+>)+(&nbsp;|<br>|<br/>|<br />)(</\w+>)+";

            return Regex.Replace(content, pattern, "<br />", RegexOptions.Singleline);
        }

        protected string RemoveComments(string content)
        {
            var pattern = "<!--.*?-->";

            return Regex.Replace(content, pattern, "", RegexOptions.Singleline);
        }

        /*protected string RemoveScriptTags(string content)
        {
            var pattern = "<script.*?>.*?</script>";

            return Regex.Replace(content, pattern, "", RegexOptions.Singleline);
        }*/


        protected string AddInlineStyles(string content)
        {
            var styles = this.ParseCssClasses(content);
            var patternClassnames = "class=['\"]?(?<name>[\\.a-zA-Z0-9-_]+?)['\"]?[\\s>]";
            var classNames = Regex.Matches(content, patternClassnames, RegexOptions.IgnoreCase | RegexOptions.Singleline | RegexOptions.Multiline);

            foreach (Match className in classNames)
                content = content.Replace(className.Value, this.GetInlineStyles(styles, className.Groups["name"].Value));

            return content;
        }


        private string GetInlineStyles(Dictionary<string, Dictionary<string, string>> styles, string className)
        {
            var inlineStyles = new StringBuilder();

            foreach (var classe in styles)
                if (classe.Key == "." + className.ToLower().Trim())
                    foreach (var modifier in classe.Value)
                        inlineStyles.AppendFormat("{0}: {1}; ", modifier.Key, modifier.Value);

            return string.Format("style=\"{0}\"", inlineStyles.ToString().Trim());
        }

        private bool IsModifierValid(string name, string value)
        {
            if (name.ToLower().StartsWith("mso-")) return false;
            if (name.ToLower() == "color" && value.ToLower() == "windowtext") return false;

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
