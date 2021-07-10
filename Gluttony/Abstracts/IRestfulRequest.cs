using Gluttony.DataTransfer;
using System;
using System.Globalization;
using System.Net.Http;
using System.Threading.Tasks;

namespace Gluttony.Abstracts
{
    public abstract class IRestfulRequest<T> : IRequest<T> where T : Response
    {
        public HttpVerb Verb { get; init; }
        public AuthenticationType AuthenticationType { get; init; }
        public string Token { get; init; }

        protected IRestfulRequest(Func<HttpResponseMessage, Task<T>> success, HttpVerb verb) : base(success)
        {
            Verb = verb;
            Token = null;
            AuthenticationType = AuthenticationType.None;
        }

        protected IRestfulRequest(Func<HttpResponseMessage, Task<T>> success, Func<HttpResponseMessage, Task<T>> error, HttpVerb verb) : base(success, error)
        {
            Verb = verb;
            Token = null;
            AuthenticationType = AuthenticationType.None;
        }

        protected IRestfulRequest(Func<HttpResponseMessage, Task<T>> success, CultureInfo culture, HttpVerb verb) : base(success, culture)
        {
            Verb = verb;
            Token = null;
            AuthenticationType = AuthenticationType.None;
        }

        protected IRestfulRequest(Func<HttpResponseMessage, Task<T>> success, Func<HttpResponseMessage, Task<T>> error, CultureInfo culture, HttpVerb verb) : base(success, error, culture)
        {
            Verb = verb;
            Token = null;
            AuthenticationType = AuthenticationType.None;
        }

        protected IRestfulRequest(AuthenticationType authType, string token, Func<HttpResponseMessage, Task<T>> success, HttpVerb verb):base(success)
        {
            Verb = verb;
            Token = token;
            AuthenticationType = authType;
        }

        protected IRestfulRequest(AuthenticationType authType, string token, Func<HttpResponseMessage, Task<T>> success, Func<HttpResponseMessage, Task<T>> error, HttpVerb verb) : base(success, error)
        {
            Verb = verb;
            Token = token;
            AuthenticationType = authType;
        }
        
        protected IRestfulRequest(AuthenticationType authType, string token, Func<HttpResponseMessage, Task<T>> success, CultureInfo culture, HttpVerb verb):base(success, culture)
        {
            Verb = verb;
            Token = token;
            AuthenticationType = authType;
        }
        
        protected IRestfulRequest(AuthenticationType authType, string token, Func<HttpResponseMessage, Task<T>> success, Func<HttpResponseMessage, Task<T>> error, CultureInfo culture, HttpVerb verb):base(success, error, culture)
        {
            Verb = verb;
            Token = token;
            AuthenticationType = authType;
        }
    }
}
