using System.Collections.Generic;
using System.Text.RegularExpressions;
using System.IO;
using System.Text;

using HtmlAgilityPack;

using PasteItCleaned.Core.Entities;
using PasteItCleaned.Plugin.Helpers;
using PasteItCleaned.Core.Models;
using System;
using Vereyon.Web;
using PasteItCleaned.Plugin.Rtf;
using PasteItCleaned.Plugin.Rtf.NRtfTree.Core;
using PasteItCleaned.Core.Helpers;

namespace PasteItCleaned.Plugin.Cleaners
{
    public class HtmlCleaner : BaseCleaner
    {
        private string[] ValidAttributes = { "style", "colspan", "rowspan", "src", "class", "href", "target", "border", "cellspacing", "cellpadding", "span" };

        public override SourceType GetSourceType()
        {
            return SourceType.Other;
        }

        public override string Clean(string html, string rtf, Config config, bool keepStyles)
        {
            var htmlDoc = this.ParseWithHtmlAgilityPack(html);
            var rtfDoc = this.ParseRtf(rtf);

            htmlDoc = base.SafeExec(this.RemoveLocalAnchors, htmlDoc, rtfDoc, config);

            if (!keepStyles)
                htmlDoc = base.SafeExec(this.RemoveAllStyles, htmlDoc, rtfDoc, config);

            if (config.RemoveSpanTags)
                htmlDoc = base.SafeExec(this.RemoveSpanTags, htmlDoc, rtfDoc, config);

            htmlDoc = base.SafeExec(this.RemoveUselessStyles, htmlDoc, rtfDoc, config);
            htmlDoc = base.SafeExec(this.RemoveUselessAttributes, htmlDoc, rtfDoc, config);
            htmlDoc = base.SafeExec(this.RemoveEmptyAttributes, htmlDoc, rtfDoc, config);
            htmlDoc = base.SafeExec(this.RemoveInvisibleTags, htmlDoc, rtfDoc, config);
            htmlDoc = base.SafeExec(this.SanitizeHtmlWithHtmlSanitizer, htmlDoc, rtfDoc, config);

            var cleaned = this.GetOuterHtml(htmlDoc);

            if (!keepStyles) {
                cleaned = base.SafeExec(this.RemoveStyleTags, cleaned);
            }

            cleaned = base.SafeExec(this.CompactSpaces, cleaned);
            cleaned = base.SafeExec(this.RemoveComments, cleaned);
            cleaned = base.SafeExec(this.RemoveUselessTags, cleaned);
            cleaned = base.SafeExec(this.Compact, cleaned);
            cleaned = base.SafeExec(this.RemoveSurroundingTags, cleaned);

            cleaned = base.Clean(cleaned, rtf, config, keepStyles);

            return cleaned.Trim();
        }



        protected string Compact(string content)
        {
            //content = content.Replace("\n", " ");
            //content = content.Replace("\t", " ");
            //content = content.Replace("\r", " ");
            //content = content.Replace("  ", " ");
            content = content.Trim();

            return content;
        }

        protected string CompactSpaces(string content)
        {
            content = content.Replace("&#160;", " ");
            content = content.Replace("&nbsp;", " ");
            content = content.Replace("  ", " ");
            content = content.Replace("> <", "><");
            content = content.Trim();

            return content;
        }


        protected HtmlDocument ParseWithHtmlAgilityPack(string content)
        {
            var htmlDoc = new HtmlDocument()
            {
                OptionFixNestedTags = true,
                OptionCheckSyntax = true,
                OptionAutoCloseOnEnd = true
            };
            var parseableContent = content;

            //parseableContent = parseableContent.Replace("↵", Environment.NewLine);
            parseableContent = Regex.Replace(parseableContent, "\\<\\!DOCTYPE.+?\\>", "");
            parseableContent = Regex.Replace(parseableContent, "\\<meta.+?\\>", "");

            var styles = Regex.Matches(parseableContent, "style=\".+\"", RegexOptions.IgnoreCase | RegexOptions.Singleline | RegexOptions.Multiline);

            foreach (Match match in styles)
                parseableContent = parseableContent.Replace(match.Value, match.Value.Replace("&quot;", "'"));

            htmlDoc.LoadHtml(parseableContent);

            return htmlDoc;
        }

        protected HtmlDocument ParseRtf(string content)
        {
            var htmlDoc = new HtmlDocument();
            var html = ParseRtfUsingRtfPipe(content);

            //if (string.IsNullOrWhiteSpace(html))
                //html = ParseRtfUsingSautinSoft(content);

            if (string.IsNullOrWhiteSpace(html))
                html = ParseRtfUsingRtfTree(content);

            if (string.IsNullOrWhiteSpace(html))
                html = ParseRtfUsingRichTextStripper(content);

            htmlDoc.LoadHtml(html);

            return htmlDoc;
        }

        protected string ParseRtfUsingRtfPipe(string content)
        {
            if (!string.IsNullOrWhiteSpace(content))
            {
                try
                {
                    return RtfPipe.Rtf.ToHtml(new RtfPipe.RtfSource(new StringReader(content)));
                }
                catch (Exception ex)
                {
                    return string.Empty;
                }
            }

            return string.Empty;
        }

        protected string ParseRtfUsingSautinSoft(string content)
        {
            // Deactivated, it was a test, seems to work good but has some pitfalls also.

            /*if (!string.IsNullOrWhiteSpace(content))
            {
                try
                {
                    SautinSoft.RtfToHtml r = new SautinSoft.RtfToHtml();

                    var html = r.ConvertString(content);
                    var htmlWithoutTrial = html.Replace("<div style=\"text-align:center;\">The unlicensed version of &laquo;RTF to HTML .Net&raquo;.<br><a href=\"https://www.sautinsoft.com/products/rtf-to-html/order.php\">Get the full featured version!</a></div>", "").Replace("TRIAL", "");

                    return htmlWithoutTrial;
                }
                catch (Exception ex)
                {
                    return string.Empty;
                }
            }*/

            return string.Empty;
        }

        protected string ParseRtfUsingRichTextStripper(string content)
        {
            if (!string.IsNullOrWhiteSpace(content))
            {
                try
                {
                    return "<html><body>" + RichTextStripper.StripRichTextFormat(content) + "</body></html>";
                }
                catch (Exception ex)
                {
                    return string.Empty;
                }
            }

            return string.Empty;
        }

        protected string ParseRtfUsingRtfTree(string content)
        {
            if (!string.IsNullOrWhiteSpace(content))
            {
                try
                {
                    RtfTree arbol = new RtfTree();

                    arbol.LoadRtfText(content);

                    return "<html><body>" + arbol.Text + "</body></html>";
                }
                catch (Exception ex)
                {
                    return string.Empty;
                }
            }

            return string.Empty;
        }





        protected string GetOuterHtml(HtmlDocument doc)
        {
            return doc.DocumentNode.OuterHtml;
        }


        protected HtmlDocument RemoveUselessAttributes(HtmlDocs docs)
        {
            foreach (HtmlNode node in docs.Html.DocumentNode.ChildNodes)
                RemoveUselessAttributesNode(node);

            return docs.Html;
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
                        foreach (var att in ValidAttributes)
                            if (attr.Name.Trim().ToLower() == att)
                                valid = true;

                    if (!valid)
                        n.Attributes.Remove(attr);
                }
            }
        }


        protected HtmlDocument RemoveEmptyAttributes(HtmlDocs docs)
        {
            foreach (HtmlNode node in docs.Html.DocumentNode.ChildNodes)
                RemoveEmptyAttributesNode(node);

            return docs.Html;
        }

        private void RemoveEmptyAttributesNode(HtmlNode node)
        {
            foreach (HtmlNode n in node.ChildNodes)
            {
                if (n.HasChildNodes)
                    RemoveEmptyAttributesNode(n);

                for (int i = n.Attributes.Count - 1; i >= 0; i--)
                {
                    var attr = n.Attributes[i];
                    var valid = false;

                    if (!string.IsNullOrWhiteSpace(attr.Value))
                        valid = true;

                    if (!valid)
                        n.Attributes.Remove(attr);
                    else
                        attr.Value = attr.Value.Trim();
                }
            }
        }


        //protected HtmlDocument RemoveUselessTags(HtmlDocs docs)
        //{
        //    foreach (HtmlNode node in docs.Html.DocumentNode.ChildNodes)
        //        RemoveUselessTagsNode(node);

        //    return docs.Html;
        //}

        //private void RemoveUselessTagsNode(HtmlNode node)
        //{
        //    foreach (HtmlNode n in node.ChildNodes)
        //    {
        //        if (n.HasChildNodes)
        //            RemoveUselessTagsNode(n);

        //        if (n.Name.ToLower() == "style")
        //            n.ParentNode.ReplaceChild(HtmlNode.CreateNode(""), n);

        //        if (n.Name.ToLower() == "head")
        //            n.ParentNode.ReplaceChild(HtmlNode.CreateNode(""), n);
        //    }
        //}


        protected HtmlDocument RemoveClassAttributes(HtmlDocs docs)
        {
            foreach (HtmlNode node in docs.Html.DocumentNode.ChildNodes)
                RemoveClassAttributesNode(node);

            return docs.Html;
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

                    if (attr.Name.Trim().ToLower() == "class")
                        valid = false;

                    if (!valid)
                        n.Attributes.Remove(attr);
                }
            }
        }


        protected HtmlDocument RemoveUselessStyles(HtmlDocs docs)
        {
            foreach (HtmlNode node in docs.Html.DocumentNode.ChildNodes)
                RemoveUselessStylesNode(node);

            return docs.Html;
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
                        var modifiers = new List<string>(attr.Value.Replace("&quot;", "").Split(";"));
                        var addedModifiers = new List<string>();
                        var keptModifiers = new List<string>();

                        modifiers.Reverse();

                        foreach (string modifier in modifiers)
                        {
                            if (!string.IsNullOrWhiteSpace(modifier))
                            {
                                var modifierName = modifier.Split(":")[0].Trim();
                                var modifierValue = modifier.Split(":")[1].Trim();
                                var completeModifier = string.Format("{0}: {1}; ", modifierName, modifierValue.TrimEnd(','));

                                if (IsModifierValid(modifierName, modifierValue))
                                {
                                    if (!addedModifiers.Contains(modifierName.Trim().ToLower()))
                                    {
                                        addedModifiers.Add(modifierName.Trim().ToLower());
                                        keptModifiers.Add(completeModifier);
                                    }
                                }
                            }
                        }

                        keptModifiers.Reverse();

                        foreach (string completeModifier in keptModifiers)
                            newStyles.Append(completeModifier);

                        attr.Value = newStyles.ToString().Trim();
                    }
                }
            }
        }


        protected HtmlDocument RemoveAllStyles(HtmlDocs docs)
        {
            foreach (HtmlNode node in docs.Html.DocumentNode.ChildNodes)
                RemoveAllStylesNode(node);

            return docs.Html;
        }

        private void RemoveAllStylesNode(HtmlNode node)
        {
            foreach (HtmlNode n in node.ChildNodes)
            {
                if (n.HasChildNodes)
                    RemoveAllStylesNode(n);

                for (int i = n.Attributes.Count - 1; i >= 0; i--)
                {
                    var attr = n.Attributes[i];

                    if (attr.Name.Trim().ToLower() == "style")
                    {
                        var attrValue = attr.Value;

                        attr.Value = "";

                        if (n.Name.ToLower() == "td" || n.Name.ToLower() == "table" || n.Name.ToLower() == "tr")
                        {
                            var modifiers = this.ParseCssModifiers(attrValue);
                            var width = this.GetValue(modifiers, "width", "");
                            var height = this.GetValue(modifiers, "height", "");

                            if (!string.IsNullOrWhiteSpace(width))
                                attr.Value += string.Format("width: {0};", width);

                            if (!string.IsNullOrWhiteSpace(height))
                                attr.Value += string.Format("height: {0};", height);
                        }
                    }
                }
            }
        }


        protected HtmlDocument AddDefaultOpenAndLibreOfficeStyles(HtmlDocs docs)
        {
            var content = this.GetOuterHtml(docs.Html);
            var styles = this.ParseCssClasses(content);

            foreach (HtmlNode node in docs.Html.DocumentNode.ChildNodes)
            {
                AddDefaultOpenOfficeStylesNode(styles, node);
                RewriteSomeStylesNode(node);
            }

            return docs.Html;
        }

        private void AddDefaultOpenOfficeStylesNode(Dictionary<string, Dictionary<string, string>> styles, HtmlNode node)
        {
            var validTags = new List<string>(new string[] { "p", "h1", "h2", "h3", "h4", "h5", "h6", "label", "div" });

            if (node.HasChildNodes)
                foreach (HtmlNode n in node.ChildNodes)
                    AddDefaultOpenOfficeStylesNode(styles, n);

            var nodeStyleAttr = this.FindOrCreateAttr(node, "style");

            // Adding FONT-FAMILY

            var current = node;
            var hasDefaultFont = false;

            while (current.Name != "#document")
            {
                var styleAttr = this.FindOrCreateAttr(current, "style");

                if (current != node && styleAttr != null && styleAttr.Value.Contains("font-family"))
                {
                    hasDefaultFont = true;
                    break;
                }

                current = current.ParentNode;
            }

            if (!hasDefaultFont && nodeStyleAttr != null && validTags.Contains(node.Name.ToLower()) && !nodeStyleAttr.Value.ToLower().Contains("font-family"))
                nodeStyleAttr.Value = "font-family: Times New Roman, serif; " + nodeStyleAttr.Value;

            // Adding FONT-SIZE

            /*current = node;
            hasDefaultFont = false;

            while (current.Name != "#document")
            {
                var styleAttr = this.FindOrCreateAttr(current, "style");

                if (current != node && styleAttr != null && styleAttr.Value.Contains("font-size"))
                {
                    hasDefaultFont = true;
                    break;
                }

                current = current.ParentNode;
            }

            if (!hasDefaultFont && nodeStyleAttr != null && !nodeStyleAttr.Value.ToLower().Contains("font-size"))
                nodeStyleAttr.Value = "font-size: 12pt; " + nodeStyleAttr.Value;*/
        }



        protected HtmlDocument RemoveDefaultLibreOfficeStyles(HtmlDocs docs)
        {
            var content = this.GetOuterHtml(docs.Html);
            var styles = this.ParseCssClasses(content);

            foreach (HtmlNode node in docs.Html.DocumentNode.ChildNodes)
            {
                RemoveDefaultLibreOfficeStylesNode(styles, node);
            }

            return docs.Html;
        }

        private void RemoveDefaultLibreOfficeStylesNode(Dictionary<string, Dictionary<string, string>> styles, HtmlNode node)
        {
            if (node.HasChildNodes)
                foreach (HtmlNode n in node.ChildNodes)
                    RemoveDefaultLibreOfficeStylesNode(styles, n);

            var nodeStyleAttr = this.FindOrCreateAttr(node, "style");

            // Removing FONT-SIZE=x-small

            if (nodeStyleAttr != null && nodeStyleAttr.Value.ToLower().Contains("font-size: x-small;"))
                nodeStyleAttr.Value = nodeStyleAttr.Value.Replace("font-size: x-small;", "");
        }


        protected HtmlDocument AddInlineStyles(HtmlDocs docs)
        {
            var content = this.GetOuterHtml(docs.Html);
            var styles = this.ParseCssClasses(content);

            foreach (HtmlNode node in docs.Html.DocumentNode.ChildNodes)
            {
                AddInlineStylesNode(styles, node);
                RewriteSomeStylesNode(node);
            }

            return docs.Html;
        }

        private void AddInlineStylesNode(Dictionary<string, Dictionary<string, string>> styles, HtmlNode node)
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

                    styleAttr.Value += this.GetInlineStylesForTagClass(styles, n.Name, classAttr.Value);
                    styleAttr.Value += this.GetInlineStylesForClass(styles, classAttr.Value);
                    styleAttr.Value += this.GetInlineStylesForTag(styles, n.Name);

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


        protected HtmlDocument RewriteSomeStyles(HtmlDocs docs)
        {
            foreach (HtmlNode node in docs.Html.DocumentNode.ChildNodes)
                RewriteSomeStylesNode(node);

            return docs.Html;
        }

        private void RewriteSomeStylesNode(HtmlNode node)
        {
            foreach (HtmlNode n in node.ChildNodes)
            {
                if (n.HasChildNodes)
                    RewriteSomeStylesNode(n);

                var styleAttr = this.FindOrCreateAttr(n, "style");

                if (styleAttr != null)
                {
                    var modifiers = this.ParseCssModifiers(styleAttr.Value);

                    // Handle margin styles
                    var margin = this.GetValue(modifiers, "margin", "").Split(' ');
                    var marginTop = this.GetValue(modifiers, "margin-top", "");
                    var marginRight = this.GetValue(modifiers, "margin-right", "");
                    var marginBottom = this.GetValue(modifiers, "margin-bottom", "");
                    var marginLeft = this.GetValue(modifiers, "margin-left", "");

                    var newMarginTop = !string.IsNullOrWhiteSpace(marginTop) ? marginTop : margin.Length >= 1 && !string.IsNullOrWhiteSpace(margin[0]) ? margin[0] : "initial";
                    var newMarginRight = !string.IsNullOrWhiteSpace(marginRight) ? marginRight : margin.Length >= 2 && !string.IsNullOrWhiteSpace(margin[1]) ? margin[1] : margin.Length >= 1 && !string.IsNullOrWhiteSpace(margin[0]) ? margin[0] : "initial";
                    var newMarginBottom = !string.IsNullOrWhiteSpace(marginBottom) ? marginBottom : margin.Length >= 3 && !string.IsNullOrWhiteSpace(margin[2]) ? margin[2] : margin.Length >= 1 && !string.IsNullOrWhiteSpace(margin[0]) ? margin[0] : "initial";
                    var newMarginLeft = !string.IsNullOrWhiteSpace(marginLeft) ? marginLeft : margin.Length >= 4 && !string.IsNullOrWhiteSpace(margin[3]) ? margin[3] : margin.Length >= 2 && !string.IsNullOrWhiteSpace(margin[1]) ? margin[1] : margin.Length >= 1 && !string.IsNullOrWhiteSpace(margin[0]) ? margin[0] : "initial";
                    var newMargin = string.Format("{0} {1} {2} {3}", newMarginTop, newMarginRight, newMarginBottom, newMarginLeft);

                    if (newMargin == "initial initial initial initial")
                        newMargin = "";

                    this.SetValue(modifiers, "margin", newMargin);
                    this.SetValue(modifiers, "margin-top", "");
                    this.SetValue(modifiers, "margin-right", "");
                    this.SetValue(modifiers, "margin-bottom", "");
                    this.SetValue(modifiers, "margin-left", "");

                    styleAttr.Value = this.GetInlineStyles(modifiers);
                }
            }
        }


        protected HtmlDocument ConvertAttributesToStyles(HtmlDocs docs)
        {
            foreach (HtmlNode node in docs.Html.DocumentNode.ChildNodes)
                ConvertAttributesToStylesNode(node);

            return docs.Html;
        }

        private void ConvertAttributesToStylesNode(HtmlNode node)
        {
            foreach (HtmlNode n in node.ChildNodes)
            {
                if (n.HasChildNodes)
                    ConvertAttributesToStylesNode(n);

                // We add the size attributes to objects other than cells everywhere.
                // There is another method to apply size styles everywhere (no avoiding cells).

                var widthAttr = this.FindOrCreateAttr(n, "width");
                var heightAttr = this.FindOrCreateAttr(n, "height");

                if (widthAttr != null && heightAttr != null)
                {
                    var styleAttr = this.FindOrCreateAttr(n, "style");

                    if (n.Name.ToLower() != "td")
                    {
                        if (!string.IsNullOrWhiteSpace(widthAttr.Value))
                            styleAttr.Value += string.Format("width: {0}; ", this.GetSize(widthAttr.Value));

                        if (!string.IsNullOrWhiteSpace(heightAttr.Value))
                            styleAttr.Value += string.Format("height: {0}; ", this.GetSize(heightAttr.Value));
                    }
                }

                // We apply different attributes to styles everytime we see them

                var alignAttr = this.FindOrCreateAttr(n, "align");

                if (alignAttr != null)
                {
                    var styleAttr = this.FindOrCreateAttr(n, "style");

                    if (!string.IsNullOrWhiteSpace(alignAttr.Value))
                        styleAttr.Value += string.Format("text-align: {0}; ", alignAttr.Value);
                }

                var valignAttr = this.FindOrCreateAttr(n, "valign");

                if (valignAttr != null)
                {
                    var styleAttr = this.FindOrCreateAttr(n, "style");

                    if (!string.IsNullOrWhiteSpace(valignAttr.Value))
                        styleAttr.Value += string.Format("vertical-align: {0}; ", valignAttr.Value);
                }

                var bgColorAttr = this.FindOrCreateAttr(n, "bgcolor");

                if (bgColorAttr != null)
                {
                    var styleAttr = this.FindOrCreateAttr(n, "style");

                    if (!string.IsNullOrWhiteSpace(bgColorAttr.Value))
                        styleAttr.Value += string.Format("background-color: {0}; ", bgColorAttr.Value);
                }

                var faceAttr = this.FindOrCreateAttr(n, "face");

                if (faceAttr != null)
                {
                    var styleAttr = this.FindOrCreateAttr(n, "style");

                    if (!string.IsNullOrWhiteSpace(faceAttr.Value))
                        styleAttr.Value += string.Format("font-family: {0}; ", faceAttr.Value);
                }

                var colorAttr = this.FindOrCreateAttr(n, "color");

                if (colorAttr != null)
                {
                    var styleAttr = this.FindOrCreateAttr(n, "style");

                    if (!string.IsNullOrWhiteSpace(colorAttr.Value))
                        styleAttr.Value += string.Format("color: {0}; ", colorAttr.Value);
                }
            }
        }


        protected HtmlDocument ConvertAttributesSizeToStyles(HtmlDocs docs)
        {
            foreach (HtmlNode node in docs.Html.DocumentNode.ChildNodes)
                ConvertAttributesSizeToStylesNode(node);

            return docs.Html;
        }

        private void ConvertAttributesSizeToStylesNode(HtmlNode node)
        {
            foreach (HtmlNode n in node.ChildNodes)
            {
                if (n.HasChildNodes)
                    ConvertAttributesSizeToStylesNode(n);

                var widthAttr = this.FindOrCreateAttr(n, "width");
                var heightAttr = this.FindOrCreateAttr(n, "height");

                if (widthAttr != null && heightAttr != null)
                {
                    var styleAttr = this.FindOrCreateAttr(n, "style");

                    if (!string.IsNullOrWhiteSpace(widthAttr.Value))
                        styleAttr.Value += string.Format("width: {0}; ", this.GetSize(widthAttr.Value));

                    if (!string.IsNullOrWhiteSpace(heightAttr.Value))
                        styleAttr.Value += string.Format("height: {0}; ", this.GetSize(heightAttr.Value));
                }
            }
        }


        protected HtmlDocument ConvertFontFamilies(HtmlDocs docs)
        {
            foreach (HtmlNode node in docs.Html.DocumentNode.ChildNodes)
                ConvertFontFamiliesNode(node);

            return docs.Html;
        }

        private void ConvertFontFamiliesNode(HtmlNode node)
        {
            foreach (HtmlNode n in node.ChildNodes)
            {
                if (n.HasChildNodes)
                    ConvertFontFamiliesNode(n);

                var styleAttr = this.FindOrCreateAttr(n, "style");

                if (styleAttr != null)
                {
                    var modifiers = this.ParseCssModifiers(styleAttr.Value);
                    var fontFamily = this.GetValue(modifiers, "font-family", "").ToLower();

                    if (fontFamily.Contains("courier"))
                        this.SetValue(modifiers, "font-family", "Courier New");
                    else if (fontFamily.Contains("times"))
                        this.SetValue(modifiers, "font-family", "Times New Roman");
                    else if (fontFamily.Contains("helvetica"))
                        this.SetValue(modifiers, "font-family", "Helvetica");
                    else if (fontFamily.Contains("symbol"))
                        this.SetValue(modifiers, "font-family", "Symbol");
                    else if (fontFamily.Contains("garamond"))
                        this.SetValue(modifiers, "font-family", "Garamond");
                    else if (fontFamily.Contains("lucida"))
                        this.SetValue(modifiers, "font-family", "Lucida Console");
                    else if (fontFamily.Contains("comic"))
                        this.SetValue(modifiers, "font-family", "Comic Sans MS");
                    else if (fontFamily.Contains("roboto"))
                        this.SetValue(modifiers, "font-family", "Roboto");
                    else if (fontFamily.Contains("verdana"))
                        this.SetValue(modifiers, "font-family", "Verdana");
                    else if (fontFamily.Contains("georgia"))
                        this.SetValue(modifiers, "font-family", "Georgia");
                    else if (fontFamily.Contains("palatino"))
                        this.SetValue(modifiers, "font-family", "Palatino");
                    else if (fontFamily.Contains("impact"))
                        this.SetValue(modifiers, "font-family", "Impact");
                    else if (!string.IsNullOrWhiteSpace(fontFamily))
                        this.SetValue(modifiers, "font-family", "Arial");

                    if (fontFamily.Contains("bold"))
                        this.SetValue(modifiers, "font-weight", "bold");

                    if (fontFamily.Contains("italic"))
                        this.SetValue(modifiers, "font-style", "italic");

                    if (fontFamily.Contains("oblique"))
                        this.SetValue(modifiers, "font-style", "oblique");

                    styleAttr.Value = this.GetInlineStyles(modifiers);
                }
            }
        }


        protected HtmlDocument RemoveMarginStylesAttr(HtmlDocs docs)
        {
            foreach (HtmlNode node in docs.Html.DocumentNode.ChildNodes)
                RemoveTheseStylesAttrNode(node, new string[] { "margin" });

            return docs.Html;
        }

        protected HtmlDocument RemoveTheseStylesAttr(HtmlDocs docs, string[] styles)
        {
            foreach (HtmlNode node in docs.Html.DocumentNode.ChildNodes)
                RemoveTheseStylesAttrNode(node, styles);

            return docs.Html;
        }

        private void RemoveTheseStylesAttrNode(HtmlNode node, string[] styles)
        {
            foreach (HtmlNode n in node.ChildNodes)
            {
                if (n.HasChildNodes)
                    RemoveTheseStylesAttrNode(n, styles);

                var styleAttr = this.FindOrCreateAttr(n, "style");

                if (styleAttr != null)
                {
                    var modifiers = this.ParseCssModifiers(styleAttr.Value);

                    foreach (var style in styles)
                        this.SetValue(modifiers, style, string.Empty);

                    styleAttr.Value = this.GetInlineStyles(modifiers);
                }
            }
        }


        protected HtmlDocument AddVShapesTags(HtmlDocs docs)
        {
            var vShapes = this.GetVShapesNodes(docs.Html);

            foreach (HtmlNode node in docs.Html.DocumentNode.ChildNodes)
                AddVShapesTagsNode(vShapes, node);

            return docs.Html;
        }

        private void AddVShapesTagsNode(Dictionary<string, HtmlNode> vShapes, HtmlNode node)
        {
            foreach (HtmlNode n in node.ChildNodes)
            {
                if (n.HasChildNodes)
                    AddVShapesTagsNode(vShapes, n);

                if (n.Name.ToLower().Trim() == "img")
                {
                    var vShapesAttr = this.FindOrCreateAttr(n, "v:shapes");

                    if (!string.IsNullOrWhiteSpace(vShapesAttr.Value))
                    {
                        if (vShapes.ContainsKey(vShapesAttr.Value))
                        {
                            var vShapeNode = vShapes[vShapesAttr.Value];

                            if (vShapeNode.ChildNodes.Count > 0)
                            {
                                foreach (var no in vShapeNode.ChildNodes)
                                {
                                    if (no.Name.ToLower().Trim() == "v:imagedata")
                                    {
                                        var oHrefAttr = this.FindOrCreateAttr(no, "o:href");

                                        if (!string.IsNullOrWhiteSpace(oHrefAttr.Value))
                                        {
                                            var srcAttr = this.FindOrCreateAttr(n, "src");

                                            srcAttr.Value = oHrefAttr.Value;
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
            }
        }


        protected HtmlDocument EmbedInternalImages(HtmlDocs docs)
        {
            var rtfImages = this.GetRtfImages(docs.Rtf);
            var imgIndex = 0;

            foreach (HtmlNode node in docs.Html.DocumentNode.ChildNodes)
                EmbedInternalImagesNode(rtfImages, node, ref imgIndex);

            return docs.Html;
        }

        private void EmbedInternalImagesNode(List<string> rtfImages, HtmlNode node, ref int imgIndex)
        {
            foreach (HtmlNode n in node.ChildNodes)
            {
                if (n.HasChildNodes)
                    EmbedInternalImagesNode(rtfImages, n, ref imgIndex);

                if (n.Name.ToLower().Trim() == "img")
                {
                    var srcAttr = this.FindOrCreateAttr(n, "src");
                    var width = this.GetNodeWidthValue(n);
                    var height = this.GetNodeHeightValue(n);

                    if (srcAttr.Value.ToLower().StartsWith("file://"))
                        if (rtfImages.Count >= imgIndex + 1)
                            srcAttr.Value = ImageHelper.ConvertToPngDataUri(rtfImages[imgIndex++], width, height);
                }
            }
        }


        protected HtmlDocument EmbedExternalImages(HtmlDocs docs)
        {
            foreach (HtmlNode node in docs.Html.DocumentNode.ChildNodes)
                EmbedExternalImagesNode(node);

            return docs.Html;
        }

        private void EmbedExternalImagesNode(HtmlNode node)
        {
            foreach (HtmlNode n in node.ChildNodes)
            {
                if (n.HasChildNodes)
                    EmbedExternalImagesNode(n);

                if (n.Name.ToLower().Trim() == "img")
                {
                    var srcAttr = this.FindOrCreateAttr(n, "src");
                    var width = this.GetNodeWidthValue(n);
                    var height = this.GetNodeHeightValue(n);

                    if (srcAttr.Value.ToLower().StartsWith("http://") || srcAttr.Value.ToLower().StartsWith("https://"))
                        srcAttr.Value = ImageHelper.GetExternalDataUri(srcAttr.Value, width, height);
                }
            }
        }


        protected HtmlDocument ConvertBulletLists(HtmlDocs docs)
        {
            foreach (HtmlNode node in docs.Html.DocumentNode.ChildNodes)
                ConvertBulletListsNodeConvertP(node);

            for (int i = 0; i < 10; i++)
                foreach (HtmlNode node in docs.Html.DocumentNode.ChildNodes)
                    ConvertBulletListsNodeCreateUlOl(node);

            /*foreach (HtmlNode node in docs.Html.DocumentNode.ChildNodes)
                CleanBulletListsNode(node);*/

            return docs.Html;
        }

        private void ConvertBulletListsNodeConvertP(HtmlNode node)
        {
            var childNodes = new List<HtmlNode>();

            foreach (HtmlNode n in node.ChildNodes)
                childNodes.Add(n);

            /*
                <p class="MsoNormal" style='text-indent: -18pt; mso-list: l0 level1 lfo1; background: white; mso-style-unhide: no; mso-style-qformat: yes; margin: 11pt 0cm 0cm 36pt; line-height: 115%; mso-pagination: widow-orphan; font-size: 11pt; font-family: arial,sans-serif; mso-fareast-font-family: arial; mso-ansi-language: #000c; mso-level-number-format: bullet; mso-level-text: -; mso-level-tab-stop: none; mso-level-number-position: left; text-decoration: none; text-underline: none;'>
                    <![if !supportLists]>
                    <span lang="EN-CA" style='font-family: calibri,sans-serif; mso-fareast-font-family: calibri; mso-ansi-language: en-ca;' class="">
                        <span style='mso-list: ignore; margin: initial initial initial 0px;' class="">
                            -<span style='font: 7pt times new roman;' class="">&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;</span>
                        </span>
                    </span>
                    <span lang="EN-CA" style='font-family: calibri,sans-serif; mso-fareast-font-family: calibri; color: black; mso-color-alt: windowtext; mso-ansi-language: en-ca;' class="">
                        Creating and maintaining the Software;
                    </span>
                    <span lang="EN-CA" style='font-family: calibri,sans-serif; mso-fareast-font-family: calibri; mso-ansi-language: en-ca;' class="">
                        <o:p class="" style=""></o:p>
                    </span>
                </p>
            */

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

                        if (liNode.InnerHtml.StartsWith("<![if !supportLists]>"))
                        {
                            liNode.ChildNodes.RemoveAt(0);
                            liNode.ChildNodes.RemoveAt(0);
                        }

                        n.ParentNode.ReplaceChild(liNode, n);
                    }
                }
            }
        }

        private void ConvertBulletListsNodeCreateUlOl(HtmlNode node)
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

        private void CleanBulletListsNode(HtmlNode node)
        {
            var childNodes = new List<HtmlNode>();

            foreach (HtmlNode n in node.ChildNodes)
                childNodes.Add(n);

            for (var i = 0; i < childNodes.Count; i++)
            {
                HtmlNode n = childNodes[i];

                if (n.HasChildNodes)
                    CleanBulletListsNode(n);

                if (n.Name.ToLower() == "li")
                {
                    var liHtml = n.InnerHtml;

                    liHtml = Regex.Replace(liHtml, @"\<span.+\<\/span\>", "", RegexOptions.Singleline | RegexOptions.IgnoreCase);

                    n.InnerHtml = liHtml;
                }
            }
        }



        protected HtmlDocument SanitizeHtmlWithHtmlSanitizer(HtmlDocs docs)
        {
            var sanitizer = HtmlSanitizer.SimpleHtml5DocumentSanitizer();

            sanitizer.RemoveComments = false;

            sanitizer.Tag("em").Rename("i");
            sanitizer.Tag("font").AllowAttributes("style").Rename("span");
            sanitizer.Tag("cite").AllowAttributes("style").Rename("span");
            sanitizer.Tag("strong").Rename("b");
            sanitizer.Tag("strike").Rename("s");
            sanitizer.Tag("center").Rename("p").SetAttribute("align", "center");

            sanitizer.Tag("br").AllowAttributes("style").AllowAttributes("class");
            sanitizer.Tag("hr").AllowAttributes("style").AllowAttributes("class");

            sanitizer.Tag("colgroup").AllowAttributes("style").AllowAttributes("class").AllowAttributes("span");
            sanitizer.Tag("col").AllowAttributes("style").AllowAttributes("class").AllowAttributes("span");

            sanitizer.Tag("head").Remove();
            sanitizer.Tag("style").Remove();
            sanitizer.Tag("script").Remove();

            sanitizer.Tag("meta").Remove();
            sanitizer.Tag("link").Remove();
            sanitizer.Tag("title").Remove();
            sanitizer.Tag("std").Remove();
            sanitizer.Tag("form").Remove();
            sanitizer.Tag("input").Remove();
            sanitizer.Tag("textarea").Remove();
            sanitizer.Tag("select").Remove();
            sanitizer.Tag("button").Remove();
            sanitizer.Tag("temp").Remove();
            sanitizer.Tag("picture").Remove();
            sanitizer.Tag("def").Remove();

            sanitizer.Tag("temp").RemoveEmpty().NoAttributes(SanitizerOperation.FlattenTag);

            if (docs.Config.RemoveIframes)
                sanitizer.Tag("iframe").Remove();

            if (docs.Config.RemoveEmptyTags && docs.Config.RemoveTagAttributes)
            {
                sanitizer.Tag("code").AllowAttributes("style").AllowAttributes("class").RemoveEmpty();
                sanitizer.Tag("big").AllowAttributes("style").AllowAttributes("class").RemoveEmpty();
                sanitizer.Tag("small").AllowAttributes("style").AllowAttributes("class").RemoveEmpty();
                sanitizer.Tag("p").AllowAttributes("style").AllowAttributes("class").RemoveEmpty();
                sanitizer.Tag("b").AllowAttributes("style").AllowAttributes("class").RemoveEmpty();
                sanitizer.Tag("s").AllowAttributes("style").AllowAttributes("class").RemoveEmpty();
                sanitizer.Tag("i").AllowAttributes("style").AllowAttributes("class").RemoveEmpty();
                sanitizer.Tag("u").AllowAttributes("style").AllowAttributes("class").RemoveEmpty();
                sanitizer.Tag("a").AllowAttributes("style").AllowAttributes("class").AllowAttributes("href").AllowAttributes("target").RemoveEmpty();

                if (docs.Config.RemoveSpanTags)
                    sanitizer.Tag("span").AllowAttributes("style").AllowAttributes("class").RemoveEmpty().NoAttributes(SanitizerOperation.FlattenTag);
                else
                    sanitizer.Tag("span").AllowAttributes("style").AllowAttributes("class").RemoveEmpty();

                sanitizer.Tag("div").AllowAttributes("style").AllowAttributes("class");//.RemoveEmpty();
                sanitizer.Tag("blockquote").AllowAttributes("style").AllowAttributes("class").RemoveEmpty();
                sanitizer.Tag("pre").AllowAttributes("style").AllowAttributes("class").RemoveEmpty();
                sanitizer.Tag("sub").AllowAttributes("style").AllowAttributes("class").RemoveEmpty();
                sanitizer.Tag("sup").AllowAttributes("style").AllowAttributes("class").RemoveEmpty();
                sanitizer.Tag("label").AllowAttributes("style").AllowAttributes("class").RemoveEmpty();

                sanitizer.Tag("img").AllowAttributes("style").AllowAttributes("class").AllowAttributes("src").RemoveEmpty();

                if (!docs.Config.RemoveIframes)
                    sanitizer.Tag("iframe").AllowAttributes("style").AllowAttributes("class").AllowAttributes("src").RemoveEmpty();

                sanitizer.Tag("h1").AllowAttributes("style").AllowAttributes("class").RemoveEmpty();
                sanitizer.Tag("h2").AllowAttributes("style").AllowAttributes("class").RemoveEmpty();
                sanitizer.Tag("h3").AllowAttributes("style").AllowAttributes("class").RemoveEmpty();
                sanitizer.Tag("h4").AllowAttributes("style").AllowAttributes("class").RemoveEmpty();
                sanitizer.Tag("h5").AllowAttributes("style").AllowAttributes("class").RemoveEmpty();
                sanitizer.Tag("h6").AllowAttributes("style").AllowAttributes("class").RemoveEmpty();

                sanitizer.Tag("table").AllowAttributes("style").AllowAttributes("class").AllowAttributes("border").AllowAttributes("cellspacing").AllowAttributes("cellpadding").RemoveEmpty();
                sanitizer.Tag("thead").AllowAttributes("style").AllowAttributes("class").RemoveEmpty();
                sanitizer.Tag("tbody").AllowAttributes("style").AllowAttributes("class").RemoveEmpty();
                sanitizer.Tag("tfoot").AllowAttributes("style").AllowAttributes("class").RemoveEmpty();
                sanitizer.Tag("th").AllowAttributes("style").AllowAttributes("class").RemoveEmpty();
                sanitizer.Tag("tr").AllowAttributes("style").AllowAttributes("class").RemoveEmpty();
                sanitizer.Tag("td").AllowAttributes("style").AllowAttributes("class").AllowAttributes("colspan").AllowAttributes("rowspan").RemoveEmpty();

                sanitizer.Tag("ol").AllowAttributes("style").AllowAttributes("class").RemoveEmpty();
                sanitizer.Tag("ul").AllowAttributes("style").AllowAttributes("class").RemoveEmpty();
                sanitizer.Tag("li").AllowAttributes("style").AllowAttributes("class").RemoveEmpty();
            }
            else if (docs.Config.RemoveEmptyTags && !docs.Config.RemoveTagAttributes)
            {
                sanitizer.Tag("code").RemoveEmpty();
                sanitizer.Tag("big").RemoveEmpty();
                sanitizer.Tag("small").RemoveEmpty();
                sanitizer.Tag("p").RemoveEmpty();
                sanitizer.Tag("b").RemoveEmpty();
                sanitizer.Tag("s").RemoveEmpty();
                sanitizer.Tag("i").RemoveEmpty();
                sanitizer.Tag("u").RemoveEmpty();
                sanitizer.Tag("a").RemoveEmpty();

                if (docs.Config.RemoveSpanTags)
                    sanitizer.Tag("span").RemoveEmpty().NoAttributes(SanitizerOperation.FlattenTag);
                else
                    sanitizer.Tag("span").RemoveEmpty();

                sanitizer.Tag("div");//.RemoveEmpty();
                sanitizer.Tag("blockquote").RemoveEmpty();
                sanitizer.Tag("pre").RemoveEmpty();
                sanitizer.Tag("sub").RemoveEmpty();
                sanitizer.Tag("sup").RemoveEmpty();
                sanitizer.Tag("label").RemoveEmpty();

                sanitizer.Tag("img").RemoveEmpty();

                if (!docs.Config.RemoveIframes)
                    sanitizer.Tag("iframe").RemoveEmpty();

                sanitizer.Tag("h1").RemoveEmpty();
                sanitizer.Tag("h2").RemoveEmpty();
                sanitizer.Tag("h3").RemoveEmpty();
                sanitizer.Tag("h4").RemoveEmpty();
                sanitizer.Tag("h5").RemoveEmpty();
                sanitizer.Tag("h6").RemoveEmpty();

                sanitizer.Tag("table").RemoveEmpty();
                sanitizer.Tag("thead").RemoveEmpty();
                sanitizer.Tag("tbody").RemoveEmpty();
                sanitizer.Tag("tfoot").RemoveEmpty();
                sanitizer.Tag("th").RemoveEmpty();
                sanitizer.Tag("tr").RemoveEmpty();
                sanitizer.Tag("td").RemoveEmpty();

                sanitizer.Tag("ol").RemoveEmpty();
                sanitizer.Tag("ul").RemoveEmpty();
                sanitizer.Tag("li").RemoveEmpty();
            }
            else
            {
                sanitizer.Tag("b").Rename("strong");

                sanitizer.Tag("code").AllowAttributes("style").AllowAttributes("class");
                sanitizer.Tag("big").AllowAttributes("style").AllowAttributes("class");
                sanitizer.Tag("small").AllowAttributes("style").AllowAttributes("class");
                sanitizer.Tag("p").AllowAttributes("style").AllowAttributes("class");
                sanitizer.Tag("b").AllowAttributes("style").AllowAttributes("class");
                sanitizer.Tag("s").AllowAttributes("style").AllowAttributes("class");
                sanitizer.Tag("i").AllowAttributes("style").AllowAttributes("class");
                sanitizer.Tag("u").AllowAttributes("style").AllowAttributes("class");
                sanitizer.Tag("a").AllowAttributes("style").AllowAttributes("class").AllowAttributes("href").AllowAttributes("target");

                if (docs.Config.RemoveSpanTags)
                    sanitizer.Tag("span").AllowAttributes("style").AllowAttributes("class").NoAttributes(SanitizerOperation.FlattenTag);
                else
                    sanitizer.Tag("span").AllowAttributes("style").AllowAttributes("class");

                sanitizer.Tag("div").AllowAttributes("style").AllowAttributes("class");
                sanitizer.Tag("blockquote").AllowAttributes("style").AllowAttributes("class");
                sanitizer.Tag("pre").AllowAttributes("style").AllowAttributes("class");
                sanitizer.Tag("sub").AllowAttributes("style").AllowAttributes("class");
                sanitizer.Tag("sup").AllowAttributes("style").AllowAttributes("class");
                sanitizer.Tag("label").AllowAttributes("style").AllowAttributes("class");

                sanitizer.Tag("img").AllowAttributes("style").AllowAttributes("class").AllowAttributes("src");

                if (!docs.Config.RemoveIframes)
                    sanitizer.Tag("iframe").AllowAttributes("style").AllowAttributes("class").AllowAttributes("src");

                sanitizer.Tag("h1").AllowAttributes("style").AllowAttributes("class");
                sanitizer.Tag("h2").AllowAttributes("style").AllowAttributes("class");
                sanitizer.Tag("h3").AllowAttributes("style").AllowAttributes("class");
                sanitizer.Tag("h4").AllowAttributes("style").AllowAttributes("class");
                sanitizer.Tag("h5").AllowAttributes("style").AllowAttributes("class");
                sanitizer.Tag("h6").AllowAttributes("style").AllowAttributes("class");

                sanitizer.Tag("table").AllowAttributes("style").AllowAttributes("class").AllowAttributes("border").AllowAttributes("cellspacing").AllowAttributes("cellpadding");
                sanitizer.Tag("thead").AllowAttributes("style").AllowAttributes("class");
                sanitizer.Tag("tbody").AllowAttributes("style").AllowAttributes("class");
                sanitizer.Tag("tfoot").AllowAttributes("style").AllowAttributes("class");
                sanitizer.Tag("th").AllowAttributes("style").AllowAttributes("class");
                sanitizer.Tag("tr").AllowAttributes("style").AllowAttributes("class");
                sanitizer.Tag("td").AllowAttributes("style").AllowAttributes("class").AllowAttributes("colspan").AllowAttributes("rowspan");

                sanitizer.Tag("ol").AllowAttributes("style").AllowAttributes("class");
                sanitizer.Tag("ul").AllowAttributes("style").AllowAttributes("class");
                sanitizer.Tag("li").AllowAttributes("style").AllowAttributes("class");
            }

            var content = this.GetOuterHtml(docs.Html);

            content = content.Replace("&#160;", " ");
            content = content.Replace("&nbsp;", " ");
            content = content.Replace("  ", " ");
            content = content.Replace("> <", "><");

            string sanitized = sanitizer.Sanitize(content);
            
            return this.ParseWithHtmlAgilityPack(sanitized);
        }



        protected HtmlDocument ConvertFontHeaders(HtmlDocs docs)
        {
            foreach (HtmlNode node in docs.Html.DocumentNode.ChildNodes)
                ConvertFontHeadersNode(node);

            return docs.Html;
        }

        private void ConvertFontHeadersNode(HtmlNode node)
        {
            var childNodes = new List<HtmlNode>();

            foreach (HtmlNode n in node.ChildNodes)
                childNodes.Add(n);

            foreach (HtmlNode n in childNodes)
            {
                if (n.HasChildNodes)
                    ConvertFontHeadersNode(n);

                if (n.Name.ToLower() == "font")
                {
                    var sizeAttr = this.FindOrCreateAttr(n, "size");

                    if (sizeAttr.Value == "5")
                        n.ParentNode.ReplaceChild(HtmlNode.CreateNode(n.OuterHtml.Replace("<font", "<h1").Replace("</font>", "</h1>")), n);

                    if (sizeAttr.Value == "4")
                        n.ParentNode.ReplaceChild(HtmlNode.CreateNode(n.OuterHtml.Replace("<font", "<h2").Replace("</font>", "</h2>")), n);

                    if (sizeAttr.Value == "3")
                        n.ParentNode.ReplaceChild(HtmlNode.CreateNode(n.OuterHtml.Replace("<font", "<h3").Replace("</font>", "</h3>")), n);

                    if (sizeAttr.Value == "2")
                        n.ParentNode.ReplaceChild(HtmlNode.CreateNode(n.OuterHtml.Replace("<font", "<h4").Replace("</font>", "</h4>")), n);

                    if (sizeAttr.Value == "1")
                        n.ParentNode.ReplaceChild(HtmlNode.CreateNode(n.OuterHtml.Replace("<font", "<h5").Replace("</font>", "</h5>")), n);
                }
            }
        }



        protected HtmlDocument ConvertFontHeadersForOpenOffice(HtmlDocs docs)
        {
            foreach (HtmlNode node in docs.Html.DocumentNode.ChildNodes)
                ConvertFontHeadersForOpenOfficeNode(node);

            return docs.Html;
        }

        private void ConvertFontHeadersForOpenOfficeNode(HtmlNode node)
        {
            var childNodes = new List<HtmlNode>();

            foreach (HtmlNode n in node.ChildNodes)
                childNodes.Add(n);

            foreach (HtmlNode n in childNodes)
            {
                if (n.HasChildNodes)
                    ConvertFontHeadersForOpenOfficeNode(n);

                if (n.Name.ToLower() == "font")
                {
                    var sizeAttr = this.FindOrCreateAttr(n, "size");
                    var styleAttr = this.FindOrCreateAttr(n, "style");

                    if (sizeAttr.Value == "1")
                        styleAttr.Value += "font-size: 6pt;";
                    else if (sizeAttr.Value == "2")
                        styleAttr.Value += "font-size: 10pt;";
                    else if (sizeAttr.Value == "3")
                        styleAttr.Value += "font-size: 12pt;";
                    else if (sizeAttr.Value == "4")
                        styleAttr.Value += "font-size: 14pt;";
                    else if (sizeAttr.Value == "5")
                        styleAttr.Value += "font-size: 18pt;";
                    else if (sizeAttr.Value == "6")
                        styleAttr.Value += "font-size: 22pt;";
                    else if (sizeAttr.Value == "7")
                        styleAttr.Value += "font-size: 26pt;";
                    else if (sizeAttr.Value == "8")
                        styleAttr.Value += "font-size: 30pt;";

                    n.ParentNode.ReplaceChild(HtmlNode.CreateNode(n.OuterHtml.Replace("<font", "<span").Replace("</font>", "</span>")), n);
                }
            }
        }



        protected HtmlDocument UnifyHeaders(HtmlDocs docs)
        {
            foreach (HtmlNode node in docs.Html.DocumentNode.ChildNodes)
                UnifyHeadersNode(node);

            return docs.Html;
        }

        private void UnifyHeadersNode(HtmlNode node)
        {
            var childNodes = new List<HtmlNode>();
            var headerTags = new List<string>(new string[] { "h1", "h2", "h3", "h4", "h5", "h6" });

            if (headerTags.Contains(node.Name.ToLower()))
            {
                var nodeStyleAttr = this.FindOrCreateAttr(node, "style");

                var current = node;
                var newStyles = "";

                while (current.Name != "#document")
                {
                    var styleAttr = this.FindOrCreateAttr(current, "style");

                    // Adding child styles to new styles
                    newStyles = styleAttr.Value + newStyles;

                    if (current != node && headerTags.Contains(current.Name.ToLower()))
                    {
                        var allStyles = styleAttr.Value + newStyles;

                        nodeStyleAttr.Value = allStyles;
                        node.Name = current.Name;

                        current.ParentNode.ReplaceChild(HtmlNode.CreateNode(node.OuterHtml), current);
                        break;
                    }

                    current = current.ParentNode;
                }
            }

            foreach (HtmlNode n in node.ChildNodes)
                childNodes.Add(n);

            foreach (HtmlNode n in childNodes)
                UnifyHeadersNode(n);
        }



        //protected HtmlDocument RemoveUselessNestedTextNodes(HtmlDocs docs)
        //{
        //    var finalTopTags = new List<string>(new string[] { "p" });

        //    foreach (string tag in finalTopTags)
        //        foreach (HtmlNode node in docs.Html.DocumentNode.ChildNodes)
        //            RemoveUselessNestedTextNodesNode(node, tag);

        //    return docs.Html;
        //}

        //private void RemoveUselessNestedTextNodesNode(HtmlNode node, string tag)
        //{
        //    var childNodes = new List<HtmlNode>();

        //    if (node.Name.ToLower() == tag.ToLower())
        //    {
        //        if (node.HasChildNodes && node.ChildNodes.Count == 1)
        //        {
        //            var nodeStyleAttr = this.FindOrCreateAttr(node, "style");
        //        }

        //        /*

        //        var current = node;
        //        var newStyles = "";

        //        while (current.Name != "#document")
        //        {
        //            var styleAttr = this.FindOrCreateAttr(current, "style");

        //            // Adding child styles to new styles
        //            newStyles = styleAttr.Value + newStyles;

        //            if (current != node && current.Name.ToLower() == tag)
        //            {
        //                var allStyles = styleAttr.Value + newStyles;

        //                nodeStyleAttr.Value = allStyles;
        //                node.Name = current.Name;

        //                current.ParentNode.ReplaceChild(HtmlNode.CreateNode(node.OuterHtml), current);
        //                break;
        //            }

        //            current = current.ParentNode;
        //        }*/
        //    }

        //    foreach (HtmlNode n in node.ChildNodes)
        //        childNodes.Add(n);

        //    foreach (HtmlNode n in childNodes)
        //        RemoveUselessNestedTextNodesNode(n, tag);
        //}



        protected HtmlDocument RemoveSpanTags(HtmlDocs docs)
        {
            foreach (HtmlNode node in docs.Html.DocumentNode.ChildNodes)
                RemoveSpanTagsNode(node);

            return docs.Html;
        }

        private void RemoveSpanTagsNode(HtmlNode node)
        {
            var childNodes = new List<HtmlNode>();
            var removableTags = new List<string>(new string[] { "span" });

            if (removableTags.Contains(node.Name.ToLower()))
            {
                if (node.ParentNode.ChildNodes.Count == 1)
                {
                    var styleAttr = this.FindOrCreateAttr(node, "style");
                    var parentStyleAttr = this.FindOrCreateAttr(node.ParentNode, "style");

                    parentStyleAttr.Value += styleAttr.Value;
                    styleAttr.Remove();
                }
            }

            foreach (HtmlNode n in node.ChildNodes)
                childNodes.Add(n);

            foreach (HtmlNode n in childNodes)
                RemoveSpanTagsNode(n);
        }




        protected HtmlDocument RemoveInvisibleTags(HtmlDocs docs)
        {
            foreach (HtmlNode node in docs.Html.DocumentNode.ChildNodes)
                RemoveInvisibleTagsNode(node);

            return docs.Html;
        }

        private void RemoveInvisibleTagsNode(HtmlNode node)
        {
            var childNodes = new List<HtmlNode>();
            var styleAttr = this.FindOrCreateAttr(node, "style");

            if (styleAttr.Value.ToLower().Contains("visibility: hidden;"))
                node.Remove();
            else if (styleAttr.Value.ToLower().Contains("display: none;"))
                node.Remove();
            else
            {
                foreach (HtmlNode n in node.ChildNodes)
                    childNodes.Add(n);

                foreach (HtmlNode n in childNodes)
                    RemoveInvisibleTagsNode(n);
            }
        }


        protected HtmlDocument RemoveLocalAnchors(HtmlDocs docs)
        {
            foreach (HtmlNode node in docs.Html.DocumentNode.ChildNodes)
                RemoveLocalAnchorsNode(node);

            return docs.Html;
        }

        private void RemoveLocalAnchorsNode(HtmlNode node)
        {
            var childNodes = new List<HtmlNode>();

            foreach (HtmlNode n in node.ChildNodes)
                childNodes.Add(n);

            foreach (HtmlNode n in childNodes)
            {
                if (n.HasChildNodes)
                    RemoveLocalAnchorsNode(n);

                if (n.Name.ToLower() == "a")
                {
                    var nameAttr = this.FindOrCreateAttr(n, "name");

                    if (!string.IsNullOrWhiteSpace(nameAttr.Value))
                        n.ParentNode.ReplaceChild(HtmlNode.CreateNode(n.InnerHtml), n);
                }
            }
        }


        //protected string RemoveIframes(string content)
        //{
        //    var pattern = @"<(iframe|!\[)[^>]*?>";

        //    return Regex.Replace(content, pattern, "", RegexOptions.Singleline);
        //}

        protected string RemoveUselessTags(string content)
        {
            var pattern = @"<(meta|link|/?o:|/?v:|/?style|/?title|/?std|/?head|/?html|/?body|/?script|/?form|/?input|/?textarea|/?select|/?button|/?temp|/?picture|/?def|!\[)[^>]*?>";

            content = content.Replace("<font", "<span");
            content = content.Replace("</font>", "</span>");

            return Regex.Replace(content, pattern, "", RegexOptions.Singleline);
        }

        protected string RemoveStyleTags(string content)
        {
            var pattern = @"<(/?i|/?b|/?u|/?strong|/?em|/?span|/?font|/?pre|/?code|/?blockquote)(^>|[\s\t].+?)*?/?>";

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

        protected string RemoveVmlComments(string content)
        {
            content = content.Replace("<!--[if gte vml 1]>", "");
            content = content.Replace("<![endif]--><![if !vml]>", "<![if !vml]>");
            content = content.Replace("<![if !vml]>", "");
            content = content.Replace("<![endif]>", "");

            return content;
        }


        private List<string> GetRtfImages(HtmlDocument htmlDoc)
        {
            var rtfImages = new List<string>();

            foreach (HtmlNode n in htmlDoc.DocumentNode.ChildNodes)
            {
                var nodes = this.GetRtfImages(n);

                foreach (var no in nodes)
                    rtfImages.Add(no);
            }

            return rtfImages;
        }

        private List<string> GetRtfImages(HtmlNode node)
        {
            var rtfImages = new List<string>();

            if (node.HasChildNodes)
            {
                foreach (HtmlNode n in node.ChildNodes)
                {
                    var nodes = this.GetRtfImages(n);

                    foreach (var no in nodes)
                        rtfImages.Add(no);
                }
            }

            if (node.Name.Trim().ToLower() == "img")
                rtfImages.Add(this.FindOrCreateAttr(node, "src").Value);

            return rtfImages;
        }

        private string GetSize(string size)
        {
            if (!string.IsNullOrWhiteSpace(size))
            {
                var numerics = new List<char>("0123456789".ToCharArray());

                if (numerics.Contains(size.ToCharArray()[size.Length - 1]))
                    return string.Format("{0}px", size);
            }

            return size;
        }


        private string GetNodeWidthValue(HtmlNode n)
        {
            var styleAttr = this.FindOrCreateAttr(n, "style");
            var widthAttr = this.FindOrCreateAttr(n, "width");

            if (!string.IsNullOrWhiteSpace(widthAttr.Value))
                return widthAttr.Value;

            var modifiers = this.ParseCssModifiers(styleAttr.Value);

            foreach (var modifier in modifiers)
                if (modifier.Key.ToLower().Trim() == "width")
                    return modifier.Value;

            return "";
        }

        private string GetNodeHeightValue(HtmlNode n)
        {
            var styleAttr = this.FindOrCreateAttr(n, "style");
            var heightAttr = this.FindOrCreateAttr(n, "height");

            if (!string.IsNullOrWhiteSpace(heightAttr.Value))
                return heightAttr.Value;

            var modifiers = this.ParseCssModifiers(styleAttr.Value);

            foreach (var modifier in modifiers)
                if (modifier.Key.ToLower().Trim() == "height")
                    return modifier.Value;

            return "";
        }

        private Dictionary<string, HtmlNode> GetVShapesNodes(HtmlDocument htmlDoc)
        {
            var vShapes = new Dictionary<string, HtmlNode>();

            foreach (HtmlNode n in htmlDoc.DocumentNode.ChildNodes)
            {
                var nodes = this.GetVShapesNodes(n);

                foreach (var no in nodes)
                    vShapes.Add(no.Key, no.Value);
            }

            return vShapes;
        }

        private Dictionary<string, HtmlNode> GetVShapesNodes(HtmlNode node)
        {
            var vShapes = new Dictionary<string, HtmlNode>();

            if (node.HasChildNodes)
            {
                foreach (HtmlNode n in node.ChildNodes)
                {
                    var nodes = this.GetVShapesNodes(n);

                    foreach (var no in nodes)
                        vShapes.Add(no.Key, no.Value);
                }
            }

            if (node.Name.Trim().ToLower() == "v:shape")
                vShapes.Add(node.Attributes["id"].Value.ToLower().Trim(), node);

            return vShapes;
        }

        private HtmlNode FindNearestParent(HtmlNode node, string tagName)
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

        private string GetInlineStyles(Dictionary<string, string> styles)
        {
            var inlineStyles = new StringBuilder();

            foreach (var modifier in styles)
                if (!string.IsNullOrWhiteSpace(modifier.Value))
                    inlineStyles.AppendFormat("{0}: {1}; ", modifier.Key, modifier.Value);

            return inlineStyles.ToString().Trim();
        }

        private bool IsModifierValid(string name, string value)
        {
            var stripFontFamily = ConfigHelper.GetAppSetting<bool>("Behavior.StripFontFamily");
            var hardClean = ConfigHelper.GetAppSetting<bool>("Behavior.HardClean");
            var defaultFontSize = ConfigHelper.GetAppSetting("Behavior.DefaultFontSize");

            // Remove webkit and other browser specific styles
            if (name.ToLower().StartsWith("-")) return false;

            // Remove any MS specific styles (Word)
            if (name.ToLower().StartsWith("mso-")) return false;

            // Remove styles we don't want to see in the final results
            if (name.ToLower() == "color" && value.ToLower() == "windowtext") return false;
            if (name.ToLower().StartsWith("page-break")) return false;
            if (name.ToLower().StartsWith("tab-stops")) return false;
            if (name.ToLower().StartsWith("letter-spacing")) return false;
            if (name.ToLower().StartsWith("text-indent")) return false;

            // Remove absolute/relative positionning
            if (name.ToLower().Equals("z-index")) return false;
            if (name.ToLower().Equals("position")) return false;
            if (name.ToLower().Equals("top")) return false;
            if (name.ToLower().Equals("right")) return false;
            if (name.ToLower().Equals("bottom")) return false;
            if (name.ToLower().Equals("left")) return false;

            // Remove Font Family if necessary (behavior in appSettings)
            if (stripFontFamily && name.ToLower().Equals("font-family")) return false;

            // Remove useless styles (hard clean)
            if (hardClean && name.ToLower().Equals("background") && value.ToLower().Equals("transparent")) return false;
            if (hardClean && name.ToLower().Equals("color") && value.ToLower().Equals("#000000")) return false;

            if (hardClean && name.ToLower().Equals("font-style") && value.ToLower().Equals("normal")) return false;
            if (hardClean && name.ToLower().Equals("font-variant-ligatures") && value.ToLower().Equals("normal")) return false;
            if (hardClean && name.ToLower().Equals("font-variant-caps") && value.ToLower().Equals("normal")) return false;
            if (hardClean && name.ToLower().Equals("font-weight") && value.ToLower().Equals("400")) return false;

            if (hardClean && name.ToLower().Equals("text-align") && value.ToLower().Equals("start")) return false;
            if (hardClean && name.ToLower().Equals("text-align") && value.ToLower().Equals("left")) return false;
            if (hardClean && name.ToLower().Equals("text-transform") && value.ToLower().Equals("none")) return false;

            if (hardClean && name.ToLower().Equals("white-space") && value.ToLower().Equals("normal")) return false;
            if (hardClean && name.ToLower().Equals("word-spacing") && value.ToLower().Equals("0px")) return false;
            if (hardClean && name.ToLower().Equals("ine-height")) return false;

            if (hardClean && name.ToLower().Equals("outline") && value.ToLower().Equals("0px")) return false;
            if (hardClean && name.ToLower().Equals("padding") && value.ToLower().Equals("0px")) return false;
            if (hardClean && name.ToLower().Equals("vertical-align") && value.ToLower().Equals("baseline")) return false;

            if (hardClean && name.ToLower().Equals("margin") && value.ToLower().Equals("0px 0px 0px 0px")) return false;
            if (hardClean && name.ToLower().Equals("border") && value.ToLower().Equals("0px")) return false;
            if (hardClean && name.ToLower().Equals("border-collapse") && value.ToLower().Equals("collapse")) return false;
            if (hardClean && name.ToLower().Equals("border-spacing") && value.ToLower().Equals("0px")) return false;

            if (hardClean && name.ToLower().Equals("orphans")) return false;
            if (hardClean && name.ToLower().Equals("widows")) return false;

            // Remove defaults
            if (name.ToLower().Equals("font-size") && value.ToLower().Equals(defaultFontSize)) return false;

            // Remove styles that doesn't change nothing
            if (value.ToLower() == "initial") return false;
            if (value.ToLower() == "inherit") return false;

            return true;
        }

        private Dictionary<string, Dictionary<string, string>> ParseCssClasses(string content)
        {
            content = Regex.Replace(content, "\\/\\*.+?\\*\\/", "", RegexOptions.Multiline | RegexOptions.Singleline | RegexOptions.IgnoreCase);

            var allClasses = new Dictionary<string, Dictionary<string, string>>();
            var patternStyles = @"<style.*?>(?<styles>.*?)</style>";
            var patternClass = @"(?<name>[a-zA-Z0-9-@#\. ,:]+?)[\s\t]*?{(?<class>.*?)}";
            var patternStyle = @"(?<modifier>[a-zA-Z0-9-]+?)[\s\t]*?:[\s\t]*?(?<value>.*?);";
            var comments = Regex.Matches(content, patternStyles, RegexOptions.IgnoreCase | RegexOptions.Singleline | RegexOptions.Multiline);

            foreach (Match comment in comments)
            {
                var styles = comment.Groups["styles"].Value.Replace("<!--", "").Replace("-->", "");
                var classes = Regex.Matches(styles, patternClass, RegexOptions.IgnoreCase | RegexOptions.Singleline | RegexOptions.Multiline);

                foreach (Match classe in classes)
                {
                    var className = classe.Groups["name"].Value;
                    var classContent = classe.Groups["class"].Value;

                    if (!string.IsNullOrWhiteSpace(classContent))
                        classContent = classContent.Trim();
                    else
                        classContent = "";

                    if (!classContent.EndsWith(";"))
                        classContent = classContent + ";";

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
            modifierValue = modifierValue.Replace("\"", "");
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

        private void SetValue(Dictionary<string, string> allClasses, string key, string value)
        {
            if (allClasses.ContainsKey(key.ToLower()))
                allClasses[key] = value;
            else
                allClasses.Add(key, value);
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
