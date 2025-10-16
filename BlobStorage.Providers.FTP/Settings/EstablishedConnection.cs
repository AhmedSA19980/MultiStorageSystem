using FluentFTP;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Runtime.InteropServices.JavaScript;
using System.Text;
using System.Threading.Tasks;

namespace BlobStorage.Providers.FTP.Settings
{
    public  class EstablishedConnection
    {
       public static async Task<AsyncFtpClient> ConnectAsyncFTP(string host  , int port ,  string username , string password)
        {
            var client = new AsyncFtpClient(host, port)
            {
          

                Credentials = new NetworkCredential(username, password),
           

            };
            
          
            await client.Connect();


            return client;
            
        }
    }
}
