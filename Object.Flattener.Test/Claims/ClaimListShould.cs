using Object.Flattener.Claims;
using Object.Flattener.Test.TestModels;
using System.Linq;
using System.Security.Claims;
using System.Text.Json;
using Xunit;

namespace Object.Flattener.Test.Claims
{
    public class ClaimListShould
    {
        [Theory]
        [InlineData("AreaAcces False")]
        [InlineData("EditorAccess False")]
        public void Contain_First_Level_Properties(string expected)
        {
            var exptectedClaim = ToClaim(expected);
            var user = new User();
            var claimList = user.ToClaimList();
            
            Assert.Contains(claimList,
                item => item.Type == exptectedClaim.Type && item.Value == exptectedClaim.Value);

        }

        [Theory]
        [InlineData("FirstPage:r True")]
        [InlineData("FirstPage:w True")]
        [InlineData("SecondPage:r True")]
        [InlineData("SecondPage:w True")]
        [InlineData("ThirdPage:r True")]
        [InlineData("ThirdPage:w True")]
        public void Contain_List_Properties(string expected)
        {
            var exptectedClaim = ToClaim(expected);
            var pageAcess = new PageAccess
            {
                FirstPage = new string[] { "r", "w" },
                SecondPage = new string[] { "r", "w" },
                ThirdPage = new string[] { "r", "w" },
            };
            var claimList = pageAcess.ToClaimList();
            
            Assert.Contains(claimList,
                item => item.Type == exptectedClaim.Type && item.Value == exptectedClaim.Value);
            
        }

        [Theory]
        [InlineData("name test")]
        [InlineData("father:name test2")]
        [InlineData("age 13")]
        [InlineData("dog:breed:name golden retriver")]
        [InlineData("dog:breed:friendly True")]
        [InlineData("dog:breed:small False")]
        [InlineData("dog:color brown")]
        public void Contain_Many_Sorts_Of_Properties(string expected)
        {
            var exptectedClaim = ToClaim(expected);
            var json = JsonSerializer.Serialize(
                new
                {
                    name = "test",
                    father = new
                    { 
                        name = "test2"
                    },
                    age = 13,
                    dog = new
                    {
                        breed = new 
                        { 
                            name = "golden retriver",
                            friendly = true,
                            small = false
                        },
                        color = "brown"
                    }
                });
            var dic = json.AsFlattenPropertyDictionary();
            
            Assert.Contains(dic,
                item => item.Key == exptectedClaim.Type && item.Value.ToString() == exptectedClaim.Value);
        }
        [Theory]
        [InlineData("name John")]
        [InlineData("parents:type:father True")]
        [InlineData("parents:name:Kyle True")]
        [InlineData("parents:type:mother True")]
        [InlineData("parents:name:Sarah True")]
        public void ContainObjectArrays(string expected)
        {
            var exptectedClaim = ToClaim(expected);
            var json = JsonSerializer.Serialize(
                new
                {
                    name = "John",
                    parents = new[]
                    {
                        new
                        {
                            type = "father",
                            name = "Kyle"
                        },
                        new
                        {
                            type = "mother",
                            name = "Sarah"
                        }
                    }
                });
            var dic = json.AsFlattenPropertyDictionary();
            Assert.Contains(dic,
                item => item.Key == exptectedClaim.Type);
        }

        private Claim ToClaim(string claimRaw)
        {
            var claimType = claimRaw.Split(" ").First();
            return new Claim(claimType, claimRaw.Substring(claimType.Length).Trim());
        }
    }
    
}
