using System.Runtime.CompilerServices;

namespace CensusMaui.Extensions
{
    public static class StringExtensions
    {
        public static List<string> GetRows(this string text)
        {
            string[] textLines = text.Split("\r\n");
            if (textLines.Length == 1)
            {
                //Try again on just \n
                textLines = text.Split("\n");
            }

            return textLines.ToList();
        }
    }
}
