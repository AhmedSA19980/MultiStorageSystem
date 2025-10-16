using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlobStorage.Core.Global
{
    public  class Folder
    {

        public static bool CreateFolderIfDoesNotExist(string FolderPath)
        {

            if (string.IsNullOrWhiteSpace(FolderPath)) return false;
        
            if (!Directory.Exists(FolderPath))
            {
                try
                {
                   
                    Directory.CreateDirectory(FolderPath);
                    return true;
                }
                catch (Exception ex)
                {
                    Console.WriteLine("Error creating folder: " + ex.Message);
                    return false;
                }
            }

            return true;

        }
    }
}
