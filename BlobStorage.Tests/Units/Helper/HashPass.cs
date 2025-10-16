using BlobStorage.Core.Global;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlobStorage.Tests.Units.Helper
{
    public class HashPassword
    {
        [Fact]
        public void HashPass_Returns64CharHexString()
        {
            string hash = HashPass.hashPassword("1234");

            Assert.NotNull(hash);
            Assert.Equal(64, hash.Length);
            Assert.Matches("^[a-f0-9]+$", hash); // ensures only lowercase hex chars
        }

    }
}
