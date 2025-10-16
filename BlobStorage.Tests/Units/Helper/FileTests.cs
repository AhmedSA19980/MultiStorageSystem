using BlobStorage.Core.Global;
using Microsoft.VisualStudio.TestPlatform.Utilities.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlobStorage.Tests.Units.Helper
{
    public class FileTests
    {
        [Fact]
        public void GetFileMimeTypeFromBytes_ReturnsJpeg_ForJpegHeader()
        {
            // Arrange: minimal valid JPEG header
            byte[] jpegBytes = new byte[] { 0xFF, 0xD8, 0xFF, 0xDB };

         
            string mimeType = FileType.GetFileMimeTypeFromBytes(jpegBytes);

         
            Assert.Equal("image/jpeg", mimeType);
        }

        [Fact]
        public void GetFileMimeTypeFromBytes_ReturnsPng_ForPngHeader()
        {
            // Arrange: minimal valid PNG header
            byte[] pngBytes = new byte[] { 0x89, 0x50, 0x4E, 0x47, 0x0D, 0x0A, 0x1A, 0x0A };

        
            string mimeType = FileType.GetFileMimeTypeFromBytes(pngBytes);

        
            Assert.Equal("image/png", mimeType);
        }

        [Fact]
        public void GetFileMimeTypeFromBytes_ReturnsPdf_ForPdfHeader()
        {
            
            byte[] pdfBytes = System.Text.Encoding.ASCII.GetBytes("%PDF-1.4");

        
            string mimeType = FileType.GetFileMimeTypeFromBytes(pdfBytes);

        
            Assert.Equal("application/pdf", mimeType);
        }

        [Fact]
        public void GetFileMimeTypeFromBytes_ReturnsDefault_ForRandomBytes()
        {
          
            byte[] randomBytes = new byte[] { 0x11, 0x22, 0x33, 0x44 };

           
            string mimeType = FileType.GetFileMimeTypeFromBytes(randomBytes);

        
            Assert.Equal("application/octet-stream", mimeType);
        }
    }
}
