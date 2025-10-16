using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlobStorage.Core.Global
{
    public  class FromStrToDatetime
    {
        public static DateTimeOffset ConvertStringToDatetimeOffest(string date)
        {
            if (DateTimeOffset.TryParse(date, out DateTimeOffset dateTimeOffset))
            {

                return dateTimeOffset;
            }
            else throw new Exception("Invalid input datetime string format must be like {yyyy-MM-ddTHH:mm:ssZ} ");
        }
    }
}
