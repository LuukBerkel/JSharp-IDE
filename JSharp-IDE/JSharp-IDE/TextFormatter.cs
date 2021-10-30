using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Media;

namespace JSharp_IDE
{
    class TextFormatter
    {
        private static SolidColorBrush standardColor = Brushes.White;

        // Keywords
        // The color determines which color the regex matches get.
        private static SolidColorBrush keyWordColor = Brushes.Orange;
        private static string keywords = @"(\b(public|private|class|void|import|protected|static|final|enum|synchronized|super|this|boolean|while|for|;|true|false|case|break|if|switch|else|int|new|return|try|catch|finally|implements|extends|package)\b\s*)";
        private static string symbols = @"(;|,)";

        // Annotations
        // The color determines which color the regex matches get.
        private static SolidColorBrush annotationColor = Brushes.YellowGreen;
        private static string annotations = @"@[a-zA-Z]+";

        // Comments
        // The color determines which color the regex matches get.
        private static SolidColorBrush commentColor = Brushes.Green;
        private static string singleLineComment = @"//.*";
        private static string multiLineComment = @"(/\*\*|\*).*((\*/)?)";

        /// <summary>
        /// Event which will go through the currently edited block of text and checks for regex.
        /// </summary>
        /// <param name="rtb"></param>
        /// <returns>1 when finished.</returns>
        public static async Task<int> OnTextChanged(RichTextBox rtb)
        {
            await Task.Run(() =>
            {
                rtb.Dispatcher.Invoke(() =>
                {
                    TextPointer caretPos = rtb.CaretPosition;
                    //Find the current block (line in this case) where the caret (typing cursor) is at.
                    Block block = rtb.Document.Blocks.Where(x => x.ContentStart.CompareTo(caretPos) == -1 && x.ContentEnd.CompareTo(caretPos) == 1).FirstOrDefault();
                    if (block != null)
                    {
                        ChangeSelectedTextColor(new TextRange(block.ContentStart, block.ContentEnd), standardColor);
                        CheckSyntaxAtBlock(block);
                    }
                });
            });

            return 1;
        }


        

        public static async Task<int> OnTextPasted(RichTextBox rtb)
        {
            await Task.Run(() =>
            {
                //Filter on all the defined words and give them the correct color.
                rtb.Dispatcher.Invoke(() =>
                {
                    BlockCollection blocks = rtb.Document.Blocks;
                    if (blocks != null)
                    {
                        for (int i = 0; i < blocks.Count; i++)
                        {
                            // Check if the user is typing within this block.
                            var start = blocks.ElementAt(i).ContentStart;
                            TextRange range = new TextRange(blocks.ElementAt(i).ContentStart, blocks.ElementAt(i).ContentEnd.GetPositionAtOffset(-1));
                            ChangeSelectedTextColor(range, standardColor);
                            CheckSyntaxAtBlock(blocks.ElementAt(i));
                        }
                    }
                    else
                    {
                        Debug.WriteLine("OnTextPasted: block == null");
                    }
                });
            });
            return 1;
        }

        /// <summary>
        /// Goes through every word in the currently edited block of text.
        /// </summary>
        /// <param name="block">The block that is edited.</param>
        private static void CheckSyntaxAtBlock(Block block)
        {
            if (block != null)
            {
                var start = block.ContentStart;
                while (start != null && start.CompareTo(block.ContentEnd) < 0)
                {
                    if (start.GetPointerContext(LogicalDirection.Forward) == TextPointerContext.Text)
                    {
                        MatchRegexAndHighlight(start, new Regex(singleLineComment, RegexOptions.Compiled | RegexOptions.IgnoreCase), commentColor);
                        MatchRegexAndHighlight(start, new Regex(multiLineComment, RegexOptions.Compiled | RegexOptions.IgnoreCase), commentColor);
                        MatchRegexAndHighlight(start, new Regex(keywords), keyWordColor);
                        MatchRegexAndHighlight(start, new Regex(symbols), keyWordColor);
                        MatchRegexAndHighlight(start, new Regex(annotations, RegexOptions.Compiled | RegexOptions.IgnoreCase), annotationColor);
                    }
                    start = start.GetNextContextPosition(LogicalDirection.Forward);
                }
            } else
            {
                Debug.WriteLine("CheckSyntaxAtBlock: block == null");
            }
        }

        /// <summary>
        /// Searches a string of words for regex match and highlights it in the given color.
        /// </summary>
        /// <param name="start">Start of the string</param>
        /// <param name="rex">Regex to find matches with</param>
        /// <param name="color">Color that needs to be applied</param>
        /// <returns>A TextPointer whos position is at the end of this string.</returns>
        private static TextPointer MatchRegexAndHighlight(TextPointer start, Regex rex, SolidColorBrush color)
        {
            Match match = rex.Match(start.GetTextInRun(LogicalDirection.Forward));
            TextRange textRange = new TextRange(start.GetPositionAtOffset(match.Index, LogicalDirection.Forward), start.GetPositionAtOffset(match.Index + match.Length, LogicalDirection.Backward));
            ChangeSelectedTextColor(textRange, color);
            return textRange.End;
        }

        /// <summary>
        /// Changes the color of a range of text.
        /// </summary>
        /// <param name="textRange">String to be colored</param>
        /// <param name="color">The color that needs to be used.</param>
        private static void ChangeSelectedTextColor(TextRange textRange, SolidColorBrush color)
        {
            textRange.ApplyPropertyValue(TextElement.ForegroundProperty, color);
        }
    }
}
