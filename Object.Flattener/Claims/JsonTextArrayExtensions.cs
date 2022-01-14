using System.Collections.Generic;
using System.Linq;
using System.Text.Json;

namespace Object.Flattener.Claims
{
    public static class JsonTextArrayExtensions
    {
        private static readonly JsonProperty TRUE_PROPERTY = JsonDocument.Parse("{\"Main\":true}").RootElement.EnumerateObject().Select(x => x).First();
        public static IEnumerable<(string Path, JsonProperty P)> FlattenArray(this JsonProperty jsonProperty, string path, string separator = ":")
        {
            //refacotr this
            var result = new List<(string Path, JsonProperty P)>();
            foreach (var jsonElement in jsonProperty.Value.EnumerateArray())
            {
                if (jsonElement.ValueKind != JsonValueKind.Object)
                {
                    result.Add(jsonElement.ProcessPrimitive(jsonProperty, path, separator));
                }
                else
                {
                    result.AddRange(jsonElement.ProcessObject(jsonProperty, path, separator));
                }
            }
            return result;
        }

        private static (string Path, JsonProperty P) ProcessPrimitive(this JsonElement element, JsonProperty property, string path, string separator = ":") =>
            (path.ConcatPath(property.Name, separator).ConcatPath(element.GetString(), separator),
            TRUE_PROPERTY);

        private static IEnumerable<(string Path, JsonProperty P)> ProcessObject(this JsonElement element, JsonProperty property, string path, string separator = ":") =>
            element.EnumerateObject()
            .SelectMany(child => child.GetLeaves(path.ConcatPath(property.Name, separator)))
            .Select(child=> (child.Path.ConcatPath(child.P.Value.ToString(), separator), TRUE_PROPERTY));
        
    }
}
