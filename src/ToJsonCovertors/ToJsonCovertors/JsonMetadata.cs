using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ToJsonCovertors
{
    internal class JsonMetadata
    {
        internal static readonly string[] IllegalJsonNameChars =
        {
            "\"", "\\"
        };

        internal static string StringCleanup(string source)
        {
            foreach (var ijc in IllegalJsonNameChars)
                source = source.Replace(ijc, string.Empty);
            return source;
        }

        internal static IEnumerable<string> ToCamelCase(IEnumerable<object> rowCells)
        {
            var sb = new StringBuilder();
            var totalCells = rowCells.Count();
            var result = new string[totalCells];
            for (var i = 0; i < totalCells; i++)
            {
                var curCell = rowCells.ElementAt(i);
                var words = curCell.ToString().Split(new[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);
                sb.Append(char.ToLower(words[0][0]) + words[0].Substring(1).ToLower());

                for (var j = 1; j < words.Length; j++)
                    sb.Append(char.ToUpper(words[j][0]) + words[j].Substring(1).ToLower());

                result[i] = sb.ToString();
                sb.Clear();
            }
            return result;
        }
    }
}