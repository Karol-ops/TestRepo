using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebServiceAutomation.Model
{
    public class RestResponse
    {

        public RestResponse(int statusCode, string responseContent)
        {
            StatusCode = statusCode;
            ResponseContent = responseContent;
        }

        public int StatusCode { get; }
        public string ResponseContent { get; }

        public override string ToString() => $"StatusCode : {StatusCode} ResponseData : {ResponseContent}";
        
    }
}
