using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;

namespace Object.Flattener.Claims
{
    public static class ClaimExtensions
    {
        public static List<string> ToClaimList<T>(this T input, string separator = ":")
        {
            var result = new List<string>();
            var json = JsonSerializer.Serialize(input);
            using (JsonDocument document = JsonDocument.Parse(json))
            {

                foreach (JsonProperty property in document.RootElement.EnumerateObject())
                {
                    result.AddRange(property.ToClaim(separator));
                }
            }
            return result;
        }
        public static IEnumerable<string> ToClaim(this JsonProperty property, string separator) =>
            property.Value.ValueKind == JsonValueKind.Array
            ? property.Value.ToChildClaimList(property, separator)
            : new string[]{$"{property.Name}{separator}{property.Value.ToString()}"};
        
        public static IEnumerable<string> ToChildClaimList(this JsonElement element, JsonProperty parentProperty, string separator)
        {
            var key = parentProperty.Name;
            return element.EnumerateArray().Select(it => $"{key}{separator}{it}");
        }
    }
}
