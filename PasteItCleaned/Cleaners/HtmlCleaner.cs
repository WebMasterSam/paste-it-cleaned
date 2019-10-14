using System.Collections.Generic;
using System.Text.RegularExpressions;
using System.IO;
using System.Text;

using AngleSharp.Html;
using AngleSharp.Html.Parser;
using HtmlAgilityPack;

using PasteItCleaned.Entities;

namespace PasteItCleaned.Cleaners
{
    public class HtmlCleaner : BaseCleaner
    {
        private string[] ValideAttributes = { "style", "colspan", "rowspan", "src", "class", "href", "target", "border", "cellspacing", "cellpadding", "valign", "align" };

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

            //if (config.GetConfigValue("embedExternalImages", false))

            //if (config.GetConfigValue("removeEmptyTags", true))

            //if (config.GetConfigValue("removeSpanTags", true))

            //cleaned = base.SafeExec(this.RemoveEmptyParagraphs, cleaned);

            /* 
             embed external images (file:/// are always embed, but http:// are not embeded by default)
             ramener la config via objet json dans le callback, et traiter les images coté client
            */


            cleaned = base.SafeExec(this.RemoveUselessTags, cleaned);
            cleaned = base.SafeExec(this.RemoveComments, cleaned);
            cleaned = base.SafeExec(this.Compact, cleaned);
            cleaned = base.SafeExec(this.Indent, cleaned);
            cleaned = base.SafeExec(this.RemoveSurroundingTags, cleaned);

            return cleaned.Trim();
        }



        protected string Compact(string content)
        {
            content = content.Replace("\n", " ");
            content = content.Replace("\t", " ");
            content = content.Replace("\r", " ");
            content = content.Replace("  ", " ");
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


        protected string RemoveClassAttributes(string content)
        {
            var htmlDoc = new HtmlDocument();

            htmlDoc.LoadHtml(content);

            foreach (HtmlNode node in htmlDoc.DocumentNode.ChildNodes)
                RemoveClassAttributesNode(node);

            return htmlDoc.DocumentNode.OuterHtml;
        }

        private void RemoveClassAttributesNode(HtmlNode node)
        {
            foreach (HtmlNode n in node.ChildNodes)
            {
                if (n.HasChildNodes)
                    RemoveClassAttributesNode(n);

                for (int i = n.Attributes.Count - 1; i >= 0; i--)
                {
                    var attr = n.Attributes[i];
                    var valid = true;

                    if (!string.IsNullOrWhiteSpace(attr.Value))
                        if (attr.Name.Trim().ToLower() == "class")
                            valid = false;

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
                        var modifiers = attr.Value.Replace("&quot;", "\"").Split(";");

                        foreach (string modifier in modifiers)
                        {
                            if (!string.IsNullOrWhiteSpace(modifier))
                            {
                                var modifierName = modifier.Split(":")[0].Trim();
                                var modifierValue = modifier.Split(":")[1].Trim();

                                if (IsModifierValid(modifierName, modifierValue))
                                    newStyles.AppendFormat("{0}: {1}; ", modifierName, modifierValue);
                            }
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
                    var modifiers = this.ParseCssModifiers(styleAttr.Value);
                    var msoList = this.GetValue(modifiers, "mso-list", "");

                    if (!styleAttr.Value.Trim().EndsWith(";"))
                        styleAttr.Value = styleAttr.Value.Trim() + ";";

                    styleAttr.Value += this.GetInlineStylesForTag(styles, n.Name);
                    styleAttr.Value += this.GetInlineStylesForClass(styles, classAttr.Value);
                    styleAttr.Value += this.GetInlineStylesForTagClass(styles, n.Name, classAttr.Value);

                    if (!string.IsNullOrWhiteSpace(msoList))
                    {
                        styleAttr.Value += this.GetInlineStylesForList(styles, msoList.Replace("level1", "level2"));
                        styleAttr.Value += this.GetInlineStylesForList(styles, msoList);
                        styleAttr.Value += "margin-left: 0px; ";
                    }

                    styleAttr.Value = styleAttr.Value.TrimStart(';');
                }
            }
        }


        protected string ConvertBulletLists(string content)
        {
            var htmlDoc = new HtmlDocument();

            htmlDoc.LoadHtml(content);

            foreach (HtmlNode node in htmlDoc.DocumentNode.ChildNodes)
                ConvertBulletListsNodeConvertP(node);

            for (int i = 0; i < 10; i++)
                foreach (HtmlNode node in htmlDoc.DocumentNode.ChildNodes)
                    ConvertBulletListsNodeCreateUlOl(node);

            return htmlDoc.DocumentNode.OuterHtml;
        }

        protected void ConvertBulletListsNodeConvertP(HtmlNode node)
        {
            var childNodes = new List<HtmlNode>();

            foreach (HtmlNode n in node.ChildNodes)
                childNodes.Add(n);

            foreach (HtmlNode n in childNodes)
            {
                if (n.HasChildNodes)
                    ConvertBulletListsNodeConvertP(n);

                if (n.Name.ToLower() == "p")
                {
                    var styleAttr = this.FindOrCreateAttr(n, "style");
                    var modifiers = this.ParseCssModifiers(styleAttr.Value);
                    var msoList = this.GetValue(modifiers, "mso-list", "");

                    if (!string.IsNullOrWhiteSpace(msoList))
                    {
                        var patternLevel = @"level(?<level>[0-9])";
                        var patternSupportLists = @"<\!\[if \!supportLists\]>(?<comment>.+?)<\!\[endif\]>";
                        var patternText = @"<\!\[endif\]>(?<text>.+)";
                        var matchLevel = Regex.Match(msoList, patternLevel, RegexOptions.IgnoreCase | RegexOptions.Singleline | RegexOptions.Multiline);
                        var matchSupportLists = Regex.Match(n.InnerHtml, patternSupportLists, RegexOptions.IgnoreCase | RegexOptions.Singleline | RegexOptions.Multiline);
                        var matchText = Regex.Match(n.InnerHtml, patternText, RegexOptions.IgnoreCase | RegexOptions.Singleline | RegexOptions.Multiline);
                        var level = (matchLevel.Success && matchLevel.Groups["level"] != null) ? matchLevel.Groups["level"].Value : "1";
                        var supportLists = (matchSupportLists.Success && matchSupportLists.Groups["comment"] != null) ? matchSupportLists.Groups["comment"].Value.ToLower() : "";
                        var text = (matchText.Success && matchText.Groups["text"] != null) ? matchText.Groups["text"].Value : n.InnerHtml;
                        var listStyleType = "disc";
                        var listType = "ul";
                        var msoLevelText = this.GetValue(modifiers, "mso-level-text", "");
                        var msoLevelNumberFormat = this.GetValue(modifiers, "mso-level-number-format", "");

                        if (supportLists.ToLower().Contains("symbol") && supportLists.Contains("·")) { listStyleType = "disc"; listType = "ul"; }
                        if (supportLists.Contains(">o<")) { listStyleType = "circle"; listType = "ul"; }
                        if (supportLists.ToLower().Contains("wingdings") && supportLists.Contains("§")) { listStyleType = "square"; listType = "ul"; }

                        if (msoLevelText.StartsWith("\"%1\\") || msoLevelText.StartsWith("\"%2\\") || msoLevelText.StartsWith("\"%3\\") || msoLevelText.StartsWith("\"%4\\") || msoLevelText.StartsWith("\"%5\\") || msoLevelText.StartsWith("\"%6\\") || msoLevelText.StartsWith("\"%7\\") || msoLevelText.StartsWith("\"%8\\") || msoLevelText.StartsWith("\"%9\\")) { listStyleType = "decimal"; listType = "ol"; }
                        if (msoLevelNumberFormat.Contains("roman-lower")) { listStyleType = "lower-roman"; listType = "ol"; }
                        if (msoLevelNumberFormat.Contains("roman-upper")) { listStyleType = "upper-roman"; listType = "ol"; }
                        if (msoLevelNumberFormat.Contains("greek-lower")) { listStyleType = "lower-greek"; listType = "ol"; }
                        if (msoLevelNumberFormat.Contains("greek-upper")) { listStyleType = "upper-greek"; listType = "ol"; }
                        if (msoLevelNumberFormat.Contains("latin-lower")) { listStyleType = "lower-latin"; listType = "ol"; }
                        if (msoLevelNumberFormat.Contains("latin-upper")) { listStyleType = "upper-latin"; listType = "ol"; }
                        if (msoLevelNumberFormat.Contains("alpha-lower")) { listStyleType = "lower-alpha"; listType = "ol"; }
                        if (msoLevelNumberFormat.Contains("alpha-upper")) { listStyleType = "upper-alpha"; listType = "ol"; }
                        
                        var liNodeHtml = string.Format("<li temp-level='{0}' temp-list-style-type='{1}' temp-list-type='{2}' style='{3}'>{4}</li>", level, listStyleType, listType, styleAttr.Value, text);
                        var liNode = HtmlNode.CreateNode(liNodeHtml);

                        n.ParentNode.ReplaceChild(liNode, n);
                    }
                }
            }
        }

        protected void ConvertBulletListsNodeCreateUlOl(HtmlNode node)
        {
            var childNodes = new List<HtmlNode>();
            
            foreach (HtmlNode n in node.ChildNodes)
                childNodes.Add(n);

            for (var i = 0; i < childNodes.Count; i++)
            {
                HtmlNode n = childNodes[i];

                if (n.HasChildNodes)
                    ConvertBulletListsNodeCreateUlOl(n);

                if (n.Name.ToLower() == "li" && n.ParentNode != null && n.PreviousSibling != null)
                {
                    if (n.ParentNode.Name.ToLower() != "ol" && n.ParentNode.Name.ToLower() != "ul" && n.PreviousSibling.Name.ToLower() != "ol" && n.PreviousSibling.Name.ToLower() != "ul")
                    {
                        var allLis = new List<HtmlNode>();
                        var list = new StringBuilder();
                        var openedStack = new Stack<string>();

                        for (var j = i; j < childNodes.Count; j++)
                        {
                            HtmlNode li = childNodes[j];

                            if (li.Name.ToLower() == "li")
                                allLis.Add(li);
                        }

                        var previousLevel = "0";

                        list.Append("<temp>");

                        foreach (var li in allLis)
                        {
                            var currentListType = li.Attributes["temp-list-type"].Value;
                            var currentStyleType = li.Attributes["temp-list-style-type"].Value;
                            var currentLevel = li.Attributes["temp-level"].Value;
                            var currentStyles = li.Attributes["style"].Value;
                            var currentText = li.InnerHtml;

                            if (previousLevel != currentLevel)
                            {
                                if (int.Parse(previousLevel) > int.Parse(currentLevel))
                                    for (int l = 0; l < int.Parse(previousLevel) - int.Parse(currentLevel); l++)
                                        list.AppendFormat("</li></{0}>", openedStack.Pop());

                                if (int.Parse(currentLevel) > int.Parse(previousLevel))
                                {
                                    for (int l = 0; l < int.Parse(currentLevel) - int.Parse(previousLevel); l++)
                                    {
                                        if (int.Parse(previousLevel) > 0)
                                            if (l > 0 && l <= (int.Parse(currentLevel) - int.Parse(previousLevel) - 1))
                                                list.AppendFormat("<li style='{0}'>", currentStyles);

                                        if (l == (int.Parse(currentLevel) - int.Parse(previousLevel) - 1))
                                            list.AppendFormat("<{0} style='list-style-type: {1}; {2}'>", currentListType, currentStyleType, currentStyles);
                                        else
                                            list.AppendFormat("<{0} style='list-style-type: none; {1}'>", currentListType, currentStyles);

                                        openedStack.Push(currentListType);
                                    }
                                }
                            }
                            else
                                list.Append("</li>");

                            list.AppendFormat("<li style='{0}'>{1}", currentStyles, currentText);

                            previousLevel = currentLevel;
                        }

                        list.Append("</temp>");

                        var listObj = HtmlNode.CreateNode(list.ToString());
                        var parentNode = n.ParentNode;

                        parentNode.InsertBefore(listObj, n);

                        foreach (var li in allLis)
                            parentNode.RemoveChild(li);

                        return;
                    }
                }
            }
        }


        //protected string AddInlineStyleMarginTableP(string content)
        //{
        //    var htmlDoc = new HtmlDocument();

        //    htmlDoc.LoadHtml(content);

        //    foreach (HtmlNode node in htmlDoc.DocumentNode.ChildNodes)
        //        AddInlineStyleMarginTablePNode(node);

        //    return htmlDoc.DocumentNode.OuterHtml;
        //}

        //protected void AddInlineStyleMarginTablePNode(HtmlNode node)
        //{
        //    foreach (HtmlNode n in node.ChildNodes)
        //    {
        //        if (n.HasChildNodes)
        //            AddInlineStyleMarginTablePNode(n);

        //        if (n.Name.ToLower() == "p")
        //        {
        //            var styleAttr = this.FindOrCreateAttr(n, "style");
        //            var nearestTable = this.FindNearestParent(n, "table");

        //            if (nearestTable != null)
        //            {
        //                var nearestTableStyles = this.FindOrCreateAttr(nearestTable, "style");
        //                var modifiers = this.ParseCssModifiers(nearestTableStyles.Value);
        //                var marginTop = this.GetValue(modifiers, "mso-para-margin-top", this.GetValue(modifiers, "mso-para-margin", "inherit"));
        //                var marginLeft = this.GetValue(modifiers, "mso-para-margin-left", this.GetValue(modifiers, "mso-para-margin", "inherit"));
        //                var marginRight = this.GetValue(modifiers, "mso-para-margin-right", this.GetValue(modifiers, "mso-para-margin", "inherit"));
        //                var marginBottom = this.GetValue(modifiers, "mso-para-margin-bottom", this.GetValue(modifiers, "mso-para-margin", "inherit"));
        //                var margin = string.Format("{0} {1} {2} {3}", marginTop, marginRight, marginBottom, marginLeft);

        //                styleAttr.Value = string.Format("margin: {0}; {1}", margin, styleAttr.Value);

        //                break;
        //            }
        //        }
        //    }
        //}


        protected string RemoveIframes(string content)
        {
            var pattern = @"<(iframe|!\[)[^>]*?>";

            return Regex.Replace(content, pattern, "", RegexOptions.Singleline);
        }

        protected string RemoveUselessTags(string content)
        {
            var pattern = @"<(meta|link|/?o:|/?v:|/?style|/?title|/?div|/?std|/?head|/?html|/?body|/?script|/?col|/?colgroup|/?form|/?input|/?textarea|/?select|/?temp|!\[)[^>]*?>";

            content = content.Replace("<font", "<span");
            content = content.Replace("</font>", "</span>");

            return Regex.Replace(content, pattern, "", RegexOptions.Singleline);
        }

        protected string RemoveSurroundingTags(string content)
        {
            var pattern = @"<(/?head|/?html|/?body|!\[)[^>]*?>";

            return Regex.Replace(content, pattern, "", RegexOptions.Singleline);
        }

        protected string RemoveComments(string content)
        {
            var pattern = "<!--.*?-->";

            return Regex.Replace(content, pattern, "", RegexOptions.Singleline);
        }




        protected HtmlNode FindNearestParent(HtmlNode node, string tagName)
        {
            if (node.Name.ToLower() == tagName.ToLower())
                return node;

            if (node.ParentNode == null)
                return null;

            return this.FindNearestParent(node.ParentNode, tagName);
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

        private string GetInlineStylesForList(Dictionary<string, Dictionary<string, string>> styles, string msoList)
        {
            var inlineStyles = new StringBuilder();
            var arr = msoList.Split(" ");
            var listId = arr.Length > 0 ? arr[0] : "";
            var listLevel = arr.Length > 1 ? arr[1] : "";
            var cssSelector = string.Format("@list {0}:{1}", listId, listLevel);

            foreach (var classeComplete in styles)
                foreach (var classe in classeComplete.Key.Split(','))
                    if (classe.ToLower().Trim() == cssSelector.ToLower().Trim())
                        foreach (var modifier in classeComplete.Value)
                            if (modifier.Key.ToLower() != "font-family")
                                inlineStyles.AppendFormat("{0}: {1}; ", modifier.Key, modifier.Value);

            return inlineStyles.ToString().Trim();
        }

        private string GetInlineStylesForTagClass(Dictionary<string, Dictionary<string, string>> styles, string tagName, string className)
        {
            var inlineStyles = new StringBuilder();

            foreach (var classeComplete in styles)
                foreach (var classe in classeComplete.Key.Split(','))
                    if (classe.ToLower().Trim() == tagName.ToLower().Trim() + "." + className.ToLower().Trim())
                        foreach (var modifier in classeComplete.Value)
                            inlineStyles.AppendFormat("{0}: {1}; ", modifier.Key, modifier.Value);

            return inlineStyles.ToString().Trim();
        }

        private string GetInlineStylesForClass(Dictionary<string, Dictionary<string, string>> styles, string className)
        {
            var inlineStyles = new StringBuilder();

            foreach (var classeComplete in styles)
                foreach (var classe in classeComplete.Key.Split(','))
                    if (classe.ToLower().Trim() == "." + className.ToLower().Trim())
                        foreach (var modifier in classeComplete.Value)
                            inlineStyles.AppendFormat("{0}: {1}; ", modifier.Key, modifier.Value);

            return inlineStyles.ToString().Trim();
        }

        private string GetInlineStylesForTag(Dictionary<string, Dictionary<string, string>> styles, string tagName)
        {
            var inlineStyles = new StringBuilder();

            foreach (var classeComplete in styles)
                foreach (var classe in classeComplete.Key.Split(','))
                    if (classe.ToLower().Trim() == tagName.ToLower().Trim())
                        foreach (var modifier in classeComplete.Value)
                            inlineStyles.AppendFormat("{0}: {1}; ", modifier.Key, modifier.Value);

            return inlineStyles.ToString().Trim();
        }

        private bool IsModifierValid(string name, string value)
        {
            if (name.ToLower().StartsWith("mso-")) return false;
            if (name.ToLower() == "color" && value.ToLower() == "windowtext") return false;
            if (name.ToLower().StartsWith("page-break")) return false;
            if (name.ToLower().StartsWith("tab-stops")) return false;
            if (name.ToLower().StartsWith("letter-spacing")) return false;
            if (name.ToLower().StartsWith("text-indent")) return false;

            return true;
        }

        private Dictionary<string, Dictionary<string, string>> ParseCssClasses(string content)
        {
            var allClasses = new Dictionary<string, Dictionary<string, string>>();
            var patternStyles = @"<style.*?>.*?<\!--(?<styles>.*?)-->.*?</style>";
            var patternClass = @"(?<name>[a-zA-Z0-9-@#\. ,:]+?)[\s\t]*?{(?<class>.*?)}";
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

                        AddClassToList(allClasses, className, modifierName, CleanModifierValue(modifierValue));
                    }
                }
            }

            return allClasses;
        }

        private Dictionary<string, string> ParseCssModifiers(string classContent)
        {
            var allClasses = new Dictionary<string, string>();
            var patternStyle = @"(?<modifier>[a-zA-Z0-9-]+?)[\s\t]*?:[\s\t]*?(?<value>.*?);";
            var modifiers = Regex.Matches(classContent.Trim().TrimEnd(';') + ";", patternStyle, RegexOptions.IgnoreCase | RegexOptions.Singleline | RegexOptions.Multiline);

            foreach (Match modifier in modifiers)
            {
                var modifierName = modifier.Groups["modifier"].Value;
                var modifierValue = modifier.Groups["value"].Value;

                AddModifiersToList(allClasses, modifierName, CleanModifierValue(modifierValue));
            }

            return allClasses;
        }

        private string CleanModifierValue(string modifierValue)
        {
            modifierValue = modifierValue.Replace("&quot;", "\"");
            modifierValue = modifierValue.Replace(".0%", "%");
            modifierValue = modifierValue.Replace(".0ch", "ch");
            modifierValue = modifierValue.Replace(".0pt", "pt");
            modifierValue = modifierValue.Replace(".0pc", "pc");
            modifierValue = modifierValue.Replace(".0px", "px");
            modifierValue = modifierValue.Replace(".0em", "em");
            modifierValue = modifierValue.Replace(".0ex", "ex");
            modifierValue = modifierValue.Replace(".0rm", "rm");
            modifierValue = modifierValue.Replace(".0cm", "cm");
            modifierValue = modifierValue.Replace(".0mm", "mm");
            modifierValue = modifierValue.Replace(".0in", "in");
            modifierValue = modifierValue.Replace(".0vw", "vw");
            modifierValue = modifierValue.Replace(".0vh", "vh");
            modifierValue = modifierValue.Replace(".0vmin", "vmin");
            modifierValue = modifierValue.Replace(".0vmax", "vmax");
            modifierValue = modifierValue.Replace(".0rem", "rem");

            return modifierValue;
        }

        private string GetValue(Dictionary<string, string> allClasses, string key, string defaultValue)
        {
            if (allClasses.ContainsKey(key.ToLower()))
                return allClasses[key];

            return defaultValue;
        }

        private void AddClassToList(Dictionary<string, Dictionary<string, string>> allClasses, string className, string modifierName, string modifierValue)
        {
            if (!allClasses.ContainsKey(className.ToLower().Trim()))
                allClasses.Add(className.ToLower().Trim(), new Dictionary<string, string>());

            if (!allClasses[className.ToLower().Trim()].ContainsKey(modifierName.ToLower().Trim()))
                allClasses[className.ToLower().Trim()].Add(modifierName.ToLower().Trim(), modifierValue.ToLower().Trim());

            allClasses[className.ToLower().Trim()][modifierName.ToLower().Trim()] = modifierValue.ToLower().Trim();
        }

        private void AddModifiersToList(Dictionary<string, string> allClasses, string modifierName, string modifierValue)
        {
            if (!allClasses.ContainsKey(modifierName.ToLower().Trim()))
                allClasses.Add(modifierName.ToLower().Trim(), modifierValue.ToLower().Trim());
        }
    }
}
