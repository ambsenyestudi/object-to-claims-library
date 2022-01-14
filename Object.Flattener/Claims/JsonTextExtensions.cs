using System.Collections.Generic;
using System.Linq;
using System.Text.Json;

namespace Object.Flattener.Claims
{
    public static class JsonTextExtensions
    {
        
        public static Dictionary<string, JsonElement> AsFlattenPropertyDictionary<T>(this T obj, string separator = ":") =>
            JsonSerializer.Serialize(obj).AsFlattenPropertyDictionary(separator);

        public static Dictionary<string, JsonElement> AsFlattenPropertyDictionary(this string json, string separator = ":") =>
            GetFlatJsonElementDictionry(json, separator);

        private static Dictionary<string, JsonElement> GetFlatJsonElementDictionry(string json, string separator = ":")
        {
            using (JsonDocument document = JsonDocument.Parse(json)) // Optional JsonDocumentOptions options
                return document.RootElement.EnumerateObject()
                    .SelectMany(p => p.GetLeaves(null, separator))
                    .ToDictionary(k => k.Path, v => v.P.Value.Clone()); //Clone so that we can use the values outside of using
        }

        public static IEnumerable<(string Path, JsonProperty P)> GetLeaves(this JsonProperty p, string path,  string separator = ":")
        {
            if (p.Value.ValueKind == JsonValueKind.Array)
            {
                return p.FlattenArray(path, separator);
            }
            if (p.Value.ValueKind != JsonValueKind.Object)
            {
                return new[] { (Path: path.ConcatPath(p.Name, separator), p) };
            }
            return p.Value.EnumerateObject().SelectMany(child => child.GetLeaves(path.ConcatPath(p.Name, separator), separator));
        }

        public static string ConcatPath(this string path, string name, string separator = ":") =>
            string.IsNullOrWhiteSpace(path)
            ? name
            : path + separator + name;


    }
}
