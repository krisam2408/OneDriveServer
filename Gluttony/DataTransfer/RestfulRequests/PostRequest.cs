using Gluttony.Abstracts;
using System;
using System.Globalization;
using System.Net.Http;
using System.Threading.Tasks;

namespace Gluttony.DataTransfer.RestfulRequests
{
    public class PostRequest <T> : IRestfulRequest<T> where T : Response
    {
        public PostRequest(Func<HttpResponseMessage, Task<T>> success) : base(success, HttpVerb.POST) { }

        public PostRequest(Func<HttpResponseMessage, Task<T>> success, Func<HttpResponseMessage, Task<T>> error) : base(success, error, HttpVerb.POST) { }

        public PostRequest(Func<HttpResponseMessage, Task<T>> success, CultureInfo culture) : base(success, culture, HttpVerb.POST) { }

        public PostRequest(Func<HttpResponseMessage, Task<T>> success, Func<HttpResponseMessage, Task<T>> error, CultureInfo culture) : base(success, error, culture, HttpVerb.POST) { }

        public PostRequest(AuthenticationType authType, string token, Func<HttpResponseMessage, Task<T>> success) : base(authType, token, success, HttpVerb.POST) { }

        public PostRequest(AuthenticationType authType, string token, Func<HttpResponseMessage, Task<T>> success, CultureInfo culture) : base(authType, token, success, culture, HttpVerb.POST) { }

        public PostRequest(AuthenticationType authType, string token, Func<HttpResponseMessage, Task<T>> success, Func<HttpResponseMessage, Task<T>> error) : base(authType, token, success, error, HttpVerb.POST) { }
        
        public PostRequest(AuthenticationType authType, string token, Func<HttpResponseMessage, Task<T>> success, Func<HttpResponseMessage, Task<T>> error, CultureInfo culture) : base(authType, token, success, error, culture, HttpVerb.POST) { }
    }
}
