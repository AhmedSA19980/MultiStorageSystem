

namespace BlobStorage.Providers.S3.AWS.Utills
{
    public  class AuthorizationHeader
    {
      public static string BuildAuthorizationHeader(string algorithm ,  string accessKey , string credentialScope ,string signedHeaders , string signature) =>
       $"{algorithm} Credential={accessKey}/{credentialScope}, SignedHeaders={signedHeaders}, Signature={signature}";

    }
}
