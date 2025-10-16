using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlobStorage.Providers.S3.AWS.Utills
{
    public class PayloadHash
    {
        public static string CreatePayloadHash(byte[] data) => AwsV4Signer.HashHex(data);
    }
}
