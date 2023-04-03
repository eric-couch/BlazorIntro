using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlazorIntro.Shared.Wrappers
{
    public class Response
    {
        public Response()
        {
            Errors = new Dictionary<string, string[]>();
        }

        public Response(bool succeeded, string message)
        {
            Errors = new Dictionary<string, string[]>();
            Succeeded = succeeded;
            Message = message;
        }

        public Response(string message)
        {
            Errors = new Dictionary<string, string[]>();
            Succeeded = false;
            Message = message;
        }

        public bool Succeeded { get; set; }
        public string Message { get; set; }
        public Dictionary<string, string[]> Errors { get; set; }
    }
}
