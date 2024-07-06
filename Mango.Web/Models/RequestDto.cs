using Mango.Web.Utility;

namespace Mango.Web.Models
{
    public class RequestDto
    {
        public SD.ApiType ApiType { get; set; } = SD.ApiType.GET;
        public string ApiUrl { get; set; }
        public object Data { get; set; }
        public string AccessToken { get; set; }
    }
}
