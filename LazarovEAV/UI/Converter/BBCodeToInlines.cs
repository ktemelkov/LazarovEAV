using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Documents;
using CodeKicker.BBCode;
using CodeKicker.BBCode.SyntaxTree;
using System.Windows.Media;


namespace LazarovEAV.UI.Util
{
    class BBCodeToInlines
    {
        class BBCodeMapEntry
        {
            public DependencyProperty Prop;
            public object Value;
        }

        static Dictionary<string, BBCodeMapEntry> bbCodeMap = new Dictionary<string, BBCodeMapEntry>() { 
            {"b", new BBCodeMapEntry(){ Prop = Inline.FontWeightProperty, Value = FontWeights.Bold }},
            {"i", new BBCodeMapEntry(){ Prop = Inline.FontStyleProperty, Value = FontStyles.Italic }},
            {"u", new BBCodeMapEntry(){ Prop = Inline.TextDecorationsProperty, Value = TextDecorations.Underline }},
            {"l", new BBCodeMapEntry(){ Prop = Inline.ForegroundProperty, Value = new SolidColorBrush(Color.FromRgb(0, 200, 0)) }},
            {"c", new BBCodeMapEntry(){ Prop = Inline.ForegroundProperty, Value = new SolidColorBrush(Color.FromRgb(200, 0, 0)) }},
            {"s", new BBCodeMapEntry(){ Prop = Inline.ForegroundProperty, Value = new SolidColorBrush(Color.FromRgb(200, 0, 0)) }},
        };



        /// <summary>
        /// 
        /// </summary>
        /// <param name="bbCode"></param>
        /// <returns></returns>
        public static Span Convert(string bbCode)
        {
            Span root = new Span();

            var bbTree = (new BBCodeParser((from e in bbCodeMap select new BBTag(e.Key, "", "", false, false)).ToArray()))
                            .ParseSyntaxTree(bbCode);

            SyntaxTreeToInlines(bbTree.SubNodes, root);

            return root;
        }



        /// <summary>
        /// 
        /// </summary>
        /// <param name="syntaxTreeNodeCollection"></param>
        /// <returns></returns>
        private static void SyntaxTreeToInlines(ISyntaxTreeNodeCollection bbNodes, Span parent)
        {
            if (bbNodes == null || bbNodes.Count == 0)
                return;

            for (int i = 0; i < bbNodes.Count; i++)
            {
                var node = bbNodes.ElementAt(i);

                if (node is TagNode)
                {
                    Span sp = new Span();
                    parent.Inlines.Add(MapTagNameToObjectProperties((TagNode)node, sp));

                    SyntaxTreeToInlines(node.SubNodes, sp);
                }
                else
                {
                    parent.Inlines.Add(new Run(node.ToText()));
                }
            }
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="tagName"></param>
        /// <param name="sp"></param>
        private static Inline MapTagNameToObjectProperties(TagNode tagNode, Inline sp)
        {
            string tagName = tagNode.Tag.Name;

            if (bbCodeMap.ContainsKey(tagName))
                sp.SetValue(bbCodeMap[tagName].Prop, bbCodeMap[tagName].Value);

            return sp;
        }
    }
}
