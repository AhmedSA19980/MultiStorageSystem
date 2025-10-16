using BlobStorage.Core.Global;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlobStorage.Tests.Units.Helper
{
    public class verifyPass
    {
        [Fact]
        public void VerifyPassword_ReturnsTrue_WhenHashesMatch()
        {
            string password = "1234";

            string storedPass = "03ac674216f3e15c761ee1a5e255f067953623c8b388b4459e13f978d7c846f4";
            bool verifyHash = VerifyPass.VerifyPassword(storedPass, password);

            Assert.True(verifyHash);
        }



        [Fact]
        public void VerfiyPasswordTest_ReturnFasle_WhenHashesDoesNotMatch()
        {
            string inputPassword = "1234";
            string storedHash = HashPass.hashPassword(inputPassword);
            string password = "abcd";
            string storedHash2 = HashPass.hashPassword(password);

            bool verfiyhash = VerifyPass.VerifyPassword(storedHash2, storedHash);
            Assert.False(verfiyhash);
        }

    }
}
