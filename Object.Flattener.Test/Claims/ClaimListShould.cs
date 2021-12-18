using Object.Flattener.Claims;
using Object.Flattener.Test.TestModels;
using System;
using Xunit;

namespace Object.Flattener.Test.Claims
{
    public class ClaimListShould
    {
        [Theory]
        [InlineData("AreaAcces:false")]
        [InlineData("EditorAccess:false")]
        public void Contain_First_Level_Properties(string expected)
        {
            var user = new User();
            var claimList = user.ToClaimList();
            Assert.Contains(claimList, 
                item => string.Equals(item, expected, StringComparison.OrdinalIgnoreCase));
        }

        [Theory]
        [InlineData("FirstPage:r")]
        [InlineData("FirstPage:w")]
        [InlineData("SecondPage:r")]
        [InlineData("SecondPage:w")]
        [InlineData("ThirdPage:r")]
        [InlineData("ThirdPage:w")]
        public void Contain_List_Properties(string expected)
        {
            var pageAcess = new PageAccess
            {
                FirstPage = new string[] { "r", "w" },
                SecondPage = new string[] { "r", "w" },
                ThirdPage = new string[] { "r", "w" },
            };
            var claimList = pageAcess.ToClaimList();
            Assert.Contains(claimList,
                item => string.Equals(item, expected, StringComparison.OrdinalIgnoreCase));
        }
    }
    
}
