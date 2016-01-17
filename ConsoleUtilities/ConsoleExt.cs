using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleUtilities
{
    static class ConsoleExt
    {
        public static void WriteLine(string line, int newLineIndent = 0, int firstLineIndent = 0)
        {
            ConsoleExt.Write(line + Environment.NewLine, newLineIndent: newLineIndent, firstLineIndent: firstLineIndent);
        }

        /// <summary>
        /// Write the given text to the console, indenting the first line by a
        /// given value and the 
        /// </summary>
        /// <param name="text"></param>
        /// <param name="newLineIndent"></param>
        /// <param name="firstLineIndent"></param>
        public static void Write(string text, int newLineIndent = 0, int firstLineIndent = 0)
        {
            var lines = new List<string> { new string(' ', firstLineIndent) };

            int i = 0;
            foreach (string word in text.Split(' '))
            {
                if (word.Length + lines[i].Length + 1 > 80)
                {
                    lines.Add(new string(' ', newLineIndent));
                    i++;
                }

                lines[i] += string.IsNullOrWhiteSpace(lines[i]) ? word : $" {word}";
            }

            Console.Write(string.Join(Environment.NewLine, lines));
        }

        /// <summary>
        /// Print each list item, indenting the subsequent lines to line up
        /// with the level of the first.
        /// </summary>
        /// <example>
        /// Example output:
        ///  - This is a really long line of text that will wrap once the line reaches 80
        ///    characters.
        ///  - This is another really long line of text that will wrap once the line reaches
        ///    80 characters.
        /// </example>
        /// <param name="listItems"></param>
        public static void WriteList(List<string> listItems)
        {
            foreach (var listItem in listItems)
            {
                ConsoleExt.WriteLine($"- {listItem}", newLineIndent: 3, firstLineIndent: 1);
            }
        }
    }
}
