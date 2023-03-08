using System.Text.Json.Nodes;

namespace CensusMaui.Extensions
{
    public static class JsonObjectExtensions
    {
        public static JsonObject AddTextToJsonObject(this JsonObject obj, string line, string separator)
        {
            string[] keyValuePair = line.Split(separator);

            if (keyValuePair.Length == 2)
            {
                keyValuePair[0] = keyValuePair[0].Replace(" ", string.Empty);
                keyValuePair[0] = keyValuePair[0].Replace("/", string.Empty);
                keyValuePair[0] = keyValuePair[0].Replace("(", string.Empty);
                keyValuePair[0] = keyValuePair[0].Replace(")", string.Empty);
                keyValuePair[0] = keyValuePair[0].Trim();
                keyValuePair[1] = keyValuePair[1].Trim();

                if (int.TryParse(keyValuePair[1], out int value))
                    obj.Add(keyValuePair[0], value);
                else
                    obj.Add(keyValuePair[0], keyValuePair[1]);
            }

            return obj;
        }
    }
}
