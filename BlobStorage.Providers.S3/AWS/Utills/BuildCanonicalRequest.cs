using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlobStorage.Providers.S3.AWS.Utills
{
    public  class CanonicalRequest
    {
      public static string BuildCanonicalRequest(string method, string canonicalUri, string canonicalQuery, string canonicalHeaders, string signedHeaders, string payloadHash) =>
                $"{method}\n{canonicalUri}\n{canonicalQuery}\n{canonicalHeaders}\n{signedHeaders}\n{payloadHash}";

    }
}
