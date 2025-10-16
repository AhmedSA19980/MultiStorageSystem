using FileSignatures;

namespace BlobStorage.Core.Global
{
    public  class FileType
    {


        public static string GetFileMimeTypeFromBytes(byte[] fileBytes)
        {
            // Only read the first 512 bytes (or the length of the array if smaller)
            int headerSize = Math.Min(fileBytes.Length, 512);
            byte[] headerBytes = new byte[headerSize];
            Array.Copy(fileBytes, headerBytes, headerSize);
         
            // 1. Convert the byte array to a Stream (required by the library)
            using var stream = new MemoryStream(headerBytes);
            stream.Position = 0;
            // 2. Create the inspector
            var inspector = new FileFormatInspector();

            // 3. Determine the file format
            var format = inspector.DetermineFileFormat(stream);

            if (format != null)
            {
                // Returns a FileFormat object that contains the file extension and MIME type.
                return format.MediaType; // e.g., "image/jpeg"
                                         // or return format.Extension; // e.g., ".jpg"
            }

            return "application/octet-stream"; // Default for unknown binary data

        
           
        }

    }
}

