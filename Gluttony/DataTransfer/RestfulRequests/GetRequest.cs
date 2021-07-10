﻿using Gluttony.Abstracts;
using System;
using System.Globalization;
using System.Net.Http;
using System.Threading.Tasks;

namespace Gluttony.DataTransfer.RestfulRequests
{
    public class GetRequest<T> : IRestfulRequest<T> where T:Response
    {
        public GetRequest(Func<HttpResponseMessage, Task<T>> success) : base(success, HttpVerb.GET) { }
        
        public GetRequest(Func<HttpResponseMessage, Task<T>> success, Func<HttpResponseMessage, Task<T>> error) : base(success, error, HttpVerb.GET) { }

        public GetRequest(Func<HttpResponseMessage, Task<T>> success, CultureInfo culture) : base(success, culture, HttpVerb.GET) { }

        public GetRequest(Func<HttpResponseMessage, Task<T>> success, Func<HttpResponseMessage, Task<T>> error, CultureInfo culture) : base(success, error, culture, HttpVerb.GET) { }

        public GetRequest(AuthenticationType authType, string token, Func<HttpResponseMessage, Task<T>> success) : base(authType, token, success, HttpVerb.GET) { }

        public GetRequest(AuthenticationType authType, string token, Func<HttpResponseMessage, Task<T>> success, CultureInfo culture) : base(authType, token, success, culture, HttpVerb.GET) { }
        
        public GetRequest(AuthenticationType authType, string token, Func<HttpResponseMessage, Task<T>> success, Func<HttpResponseMessage, Task<T>> error) : base(authType, token, success, error, HttpVerb.GET) { }
        
        public GetRequest(AuthenticationType authType, string token, Func<HttpResponseMessage, Task<T>> success, Func<HttpResponseMessage, Task<T>> error, CultureInfo culture) : base(authType, token, success, error, culture, HttpVerb.GET) { }
    }
}
