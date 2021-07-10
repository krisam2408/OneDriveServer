using Gluttony.Abstracts;
using System;
using System.Globalization;
using System.Net.Http;
using System.Threading.Tasks;

namespace Gluttony.DataTransfer.RestfulRequests
{
    public class PatchRequest <T> : IRestfulRequest<T> where T : Response
    {
        public PatchRequest(Func<HttpResponseMessage, Task<T>> success) : base(success, HttpVerb.PATCH) { }

        public PatchRequest(Func<HttpResponseMessage, Task<T>> success, Func<HttpResponseMessage, Task<T>> error) : base(success, error, HttpVerb.PATCH) { }

        public PatchRequest(Func<HttpResponseMessage, Task<T>> success, CultureInfo culture) : base(success, culture, HttpVerb.PATCH) { }

        public PatchRequest(Func<HttpResponseMessage, Task<T>> success, Func<HttpResponseMessage, Task<T>> error, CultureInfo culture) : base(success, error, culture, HttpVerb.PATCH) { }

        public PatchRequest(AuthenticationType authType, string token, Func<HttpResponseMessage, Task<T>> success) : base(authType, token, success, HttpVerb.PATCH) { }

        public PatchRequest(AuthenticationType authType, string token, Func<HttpResponseMessage, Task<T>> success, CultureInfo culture) : base(authType, token, success, culture, HttpVerb.PATCH) { }

        public PatchRequest(AuthenticationType authType, string token, Func<HttpResponseMessage, Task<T>> success, Func<HttpResponseMessage, Task<T>> error) : base(authType, token, success, error, HttpVerb.PATCH) { }
        
        public PatchRequest(AuthenticationType authType, string token, Func<HttpResponseMessage, Task<T>> success, Func<HttpResponseMessage, Task<T>> error, CultureInfo culture) : base(authType, token, success, error, culture, HttpVerb.PATCH) { }
    }
}
