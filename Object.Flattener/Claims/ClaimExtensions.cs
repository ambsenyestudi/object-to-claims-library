using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;

namespace Object.Flattener.Claims
{
    public static class ClaimExtensions
    {
        public static List<Claim> ToClaimList<T>(this T input, string separator = ":")
        {
            var result = new List<string>();
            var flatDic = input.AsFlattenPropertyDictionary(separator);
            return flatDic.Select(x=> new Claim(x.Key, x.Value.ToString())).ToList();
        }
    }
}
