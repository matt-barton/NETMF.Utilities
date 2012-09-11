using System;

namespace MattBarton.NETMF.Utilities.Data
{
    class HttpResponseComponents
    {
        public int StatusCode { get; set; }
        public string StatusDescriptor{ get; set; }
        public string[] Headers { get; set; }
        public string Content { get; set; }
    }
}
