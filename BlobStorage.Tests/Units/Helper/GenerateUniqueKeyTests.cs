using BlobStorage.Core.Global;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlobStorage.Tests.Units.Helper
{
    public class GenerateUniqueKeyTests
    {
        [Fact]
        public void GenerateUniqueKey_ReturnsNonEmptyGuid()
        {
            // Act
            Guid key = UniKey.GenerateUniqueKey();

            // Assert
            Assert.NotEqual(Guid.Empty, key);
        }
    }

}
