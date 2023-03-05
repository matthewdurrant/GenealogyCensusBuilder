using System.Runtime.CompilerServices;

namespace CensusBuilder.Extensions
{
    public static class StringExtensions
    {
        public static string[] GetRows(this string text)
        {
            string[] textLines = text.Split("\r\n");
            if (textLines.Length == 1)
            {
                //Try again on just \n
                textLines = text.Split("\n");
            }

            return textLines;
        }
    }
}
