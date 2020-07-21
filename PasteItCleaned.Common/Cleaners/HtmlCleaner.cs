using System.Collections.Generic;
using System.Text.RegularExpressions;
using System.IO;
using System.Text;

using HtmlAgilityPack;

using PasteItCleaned.Core.Entities;
using PasteItCleaned.Plugin.Helpers;
using PasteItCleaned.Core.Models;
using System;

namespace PasteItCleaned.Plugin.Cleaners
{
    public class HtmlCleaner : BaseCleaner
    {
        private string[] ValideAttributes = { "style", "colspan", "rowspan", "src", "class", "href", "target", "border", "cellspacing", "cellpadding", "valign", "align" };

        public override SourceType GetSourceType()
        {
            return SourceType.Other;
        }

        public override string Clean(string html, string rtf, Config config, bool keepStyles)
        {
            var htmlDoc = this.ParseWithHtmlAgilityPack(html);
            var rtfDoc = this.ParseWithRtfPipe(rtf);

            htmlDoc = base.SafeExec(this.RemoveLocalAnchors, htmlDoc, rtfDoc);

            if (!keepStyles)
                htmlDoc = base.SafeExec(this.RemoveAllStyles, htmlDoc, rtfDoc);

            //if (config.GetConfigValue("removeEmptyTags", true))

            //if (config.GetConfigValue("removeSpanTags", true))

            var cleaned = this.GetOuterHtml(htmlDoc);

            if (!keepStyles) {
                cleaned = base.SafeExec(this.RemoveStyleTags, cleaned);
                cleaned = base.SafeExec(this.CompactSpaces, cleaned);
            }

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
            content = content.Replace("&nbsp;", " ");
            content = content.Replace("  ", " ");
            content = content.Trim();

            return content;
        }


        protected HtmlDocument ParseWithHtmlAgilityPack(string content)
        {
            var htmlDoc = new HtmlDocument();
            var parseableContent = content;

            //parseableContent = parseableContent.Replace("↵", Environment.NewLine);
            parseableContent = Regex.Replace(parseableContent, "\\<\\!DOCTYPE.+?\\>", "");
            parseableContent = Regex.Replace(parseableContent, "\\<meta.+?\\>", "");

            htmlDoc.LoadHtml(parseableContent);

            return htmlDoc;
        }

        protected HtmlDocument ParseWithRtfPipe(string content)
        {
            var htmlDoc = new HtmlDocument();

            if (!string.IsNullOrWhiteSpace(content))
            {
                try
                {
                    var html = RtfPipe.Rtf.ToHtml(new RtfPipe.RtfSource(new StringReader(content)));

                    htmlDoc.LoadHtml(html);
                }
                catch
                {
                    SautinSoft.RtfToHtml r = new SautinSoft.RtfToHtml();
                    var html = r.ConvertString(content);
                    var htmlWithoutTrial = html.Replace("</div><div style=\"text-align:center;\">The unlicensed version of &laquo;RTF to HTML .Net&raquo;.<br><a href=\"https://www.sautinsoft.com/products/rtf-to-html/order.php\">Get the full featured version!</a></div>", "").Replace("TRIAL", "");

                    htmlDoc.LoadHtml(htmlWithoutTrial);
                }
            }

            return htmlDoc;
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
                        foreach (var att in ValideAttributes)
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
                }
            }
        }


        protected HtmlDocument RemoveUselessTags(HtmlDocs docs)
        {
            foreach (HtmlNode node in docs.Html.DocumentNode.ChildNodes)
                RemoveUselessTagsNode(node);

            return docs.Html;
        }

        private void RemoveUselessTagsNode(HtmlNode node)
        {
            foreach (HtmlNode n in node.ChildNodes)
            {
                if (n.HasChildNodes)
                    RemoveUselessTagsNode(n);

                if (n.Name.ToLower() == "style")
                    n.ParentNode.ReplaceChild(HtmlNode.CreateNode(""), n);

                if (n.Name.ToLower() == "head")
                    n.ParentNode.ReplaceChild(HtmlNode.CreateNode(""), n);
            }
        }


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

            foreach (HtmlNode node in docs.Html.DocumentNode.ChildNodes)
                CleanBulletListsNode(node);

            return docs.Html;
        }

        private void ConvertBulletListsNodeConvertP(HtmlNode node)
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


        protected string RemoveIframes(string content)
        {
            var pattern = @"<(iframe|!\[)[^>]*?>";

            return Regex.Replace(content, pattern, "", RegexOptions.Singleline);
        }

        protected string RemoveUselessTags(string content)
        {
            var pattern = @"<(meta|link|/?o:|/?v:|/?style|/?title|/?div|/?std|/?head|/?html|/?body|/?script|/?col|/?colgroup|/?form|/?input|/?textarea|/?select|/?button|/?temp|/?picture|/?def|!\[)[^>]*?>";

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
