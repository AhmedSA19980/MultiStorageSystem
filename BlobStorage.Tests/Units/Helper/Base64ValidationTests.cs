using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using BlobStorage.Core.Global;

namespace BlobStorage.Tests.Units.Helper
{
    public class Base64ValidationTests
    {

            [Theory]
            [InlineData("SGVsbG8gV29ybGQh", "Hello World!")] // valid Base64
            public void Decode_ValidBase64_ReturnsExpectedBytes(string base64, string expectedText)
            {
                // Arrange
                var expectedBytes = System.Text.Encoding.UTF8.GetBytes(expectedText);

                // Act
                var result = DecodeBase64.Decode(base64);

                // Assert
                Assert.Equal(expectedBytes, result);
            }

            [Theory]
            [InlineData("Invalid@@@")] // invalid Base64 string
            public void Decode_InvalidBase64_ThrowsArgumentException(string invalidBase64)
            {
                // Act & Assert
                var ex = Assert.Throws<ArgumentException>(() => DecodeBase64.Decode(invalidBase64));
                Assert.Equal($"Invalid Base64 string.{nameof(invalidBase64)}" , $"Invalid Base64 string.{nameof(invalidBase64)}");
            }
    }



}
