using BlobStorage.Core.Global;
using Microsoft.VisualStudio.TestPlatform.ObjectModel.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlobStorage.Tests.Units.Helper
{
    public class FolderTests
    {
        [Fact]
        public void CreateFolderIfDoesNotExist_ReturnsFalse_WhenPathIsNullOrWhitespace()
        {
          
            string nullPath = null;
            string emptyPath = "   ";

        
            Assert.False(Folder.CreateFolderIfDoesNotExist(nullPath));
            Assert.False(Folder.CreateFolderIfDoesNotExist(emptyPath));
        }

        [Fact]
        public void CreateFolderIfDoesNotExist_CreatesFolder_WhenNotExists()
        {
           
            string testFolder = Path.Combine(Path.GetTempPath(), "TestFolder_" + Guid.NewGuid());

            try
            {
                
                if (Directory.Exists(testFolder))
                    Directory.Delete(testFolder, true);

           
                bool result = Folder.CreateFolderIfDoesNotExist(testFolder);

             
                Assert.True(result);
                Assert.True(Directory.Exists(testFolder));
            }
            finally
            {
              
                if (Directory.Exists(testFolder))
                    Directory.Delete(testFolder, true);
            }
        }

        [Fact]
        public void CreateFolderIfDoesNotExist_ReturnsTrue_WhenAlreadyExists()
        {
         
            string existingFolder = Path.Combine(Path.GetTempPath(), "ExistingFolder_" + Guid.NewGuid());
            Directory.CreateDirectory(existingFolder);

            try
            {
            
                bool result = Folder.CreateFolderIfDoesNotExist(existingFolder);

             
                Assert.True(result);
                Assert.True(Directory.Exists(existingFolder));
            }
            finally
            {
             
                if (Directory.Exists(existingFolder))
                    Directory.Delete(existingFolder, true);
            }
        }
    }
}
