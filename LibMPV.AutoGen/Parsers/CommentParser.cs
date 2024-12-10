using CppSharp;
using CppSharp.AST;
using System;
using System.CodeDom.Compiler;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace LibMPV.AutoGen.Parsers
{
    internal sealed class CommentParser
    {
        public static void Parse(RawComment comment, IndentedTextWriter writer)
        {
            if (comment == null)
            {
                return;
            }
            Parse(comment.FullComment, writer, false);
        }

        private static void Parse(Comment comment, IndentedTextWriter writer, bool newLine)
        {
            switch (comment.Kind)
            {
                case DocumentationCommentKind.FullComment:
                    var fullComment = (FullComment)comment;
                    ParseFullComment(fullComment, writer);
                    break;
                case DocumentationCommentKind.BlockCommandComment:
                    var blockCommand = (BlockCommandComment)comment;
                    ParseBlockCommand(blockCommand, writer);
                    break;
                case DocumentationCommentKind.ParamCommandComment:
                    var paramCommand = (ParamCommandComment)comment;
                    ParseParamCommand(paramCommand, writer);
                    break;
                case DocumentationCommentKind.ParagraphComment:
                    var paragraphComment = (ParagraphComment)comment;
                    ParseParagraphComment(paragraphComment, writer);
                    break;
                case DocumentationCommentKind.TextComment:
                    var textComment = (TextComment)comment;
                    ParseTextComment(textComment, writer, newLine);
                    break;
                case DocumentationCommentKind.InlineCommandComment:
                    break;
                case DocumentationCommentKind.VerbatimBlockLineComment:
                    break;
                default:
                    break;
            }
        }

        private static void ParseFullComment(FullComment comment, IndentedTextWriter writer)
        {
            var first = true;
            var summaryEnded = false;
            foreach (var item in comment.Blocks)
            {
                if (!first && item is not ParagraphComment && !summaryEnded)
                {
                    writer.WriteLine("/// </summary>");
                    summaryEnded = true;
                }

                if (item is ParagraphComment && first)
                {
                    writer.WriteLine("/// <summary>");

                    first = false;
                }

                Parse(item, writer, false);
            }

            if (!summaryEnded)
            {
                writer.WriteLine("/// </summary>");
            }
        }

        private static void ParseBlockCommand(BlockCommandComment comment, IndentedTextWriter writer)
        {
            var kind = comment.CommandKind.ToString().ToLowerInvariant();
            writer.Write($"/// <{kind}>");

            var newLine = comment.ParagraphComment.Content.Count > 1;

            if (newLine)
            {
                writer.WriteLine();
            }

            foreach (var item in comment.ParagraphComment.Content)
            {
                Parse(item, writer, newLine);
                newLine = item.HasTrailingNewline;
            }

            if (comment.ParagraphComment.Content.Count == 1)
            {
                writer.WriteLine($"</{kind}>");
            }
            else
            {
                writer.WriteLine($"/// </{kind}>");
            }
        }

        private static void ParseParamCommand(ParamCommandComment comment, IndentedTextWriter writer)
        {
            writer.WriteLine($"/// <param name='{comment.Arguments[0].Text}'>");
            Parse(comment.ParagraphComment, writer, true);
            writer.WriteLine($"/// </param>");
        }

        private static void ParseTextComment(TextComment textComment, IndentedTextWriter writer, bool newLine)
        {
            if (string.IsNullOrEmpty(textComment.Text) || string.IsNullOrWhiteSpace(textComment.Text))
            {
                return;
            }
            if (newLine)
            {
                writer.WriteLine("/// <para>{0}</para>", textComment.Text.Trim());
            }
            else
            {
                writer.Write(textComment.Text.Trim());

                if (textComment.HasTrailingNewline)
                {
                    writer.WriteLine();
                }
            }
        }

        private static void ParseParagraphComment(ParagraphComment comment, IndentedTextWriter writer)
        {
            foreach (var item in comment.Content)
            {
                Parse(item, writer, true);
            }
        }
    }
}
