using Fusillade;
using ModernHttpClient;
using Refit;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;

namespace SearchPeople.Services
{
    public class ApiService<T> : IApiService<T>
    {
        Func<HttpMessageHandler, T> createClient;
        public ApiService(string apiBaseAddress)
        {
            createClient = messageHandler =>
            {
                var client = new HttpClient(messageHandler)
                {
                    BaseAddress = new Uri(apiBaseAddress)
                };

                return RestService.For<T>(client);
            };

        }

        private T BackGround
        {
            get
            {
                return new Lazy<T>(() => createClient(new RateLimitedHttpMessageHandler(new NativeMessageHandler(), Priority.Background))).Value;
            }
        }

        private T UserInitialized
        {
            get
            {
                return new Lazy<T>(() => createClient(new RateLimitedHttpMessageHandler(new NativeMessageHandler(), Priority.UserInitiated))).Value;

            }
        }

        private T Speculative
        {
            get
            {
                return new Lazy<T>(() => createClient(new RateLimitedHttpMessageHandler(new NativeMessageHandler(), Priority.Speculative))).Value;

            }
        }

        public T GetApi(Priority priority)
        {
            switch (priority)
            {
                case Priority.Background:
                    return BackGround;
                case Priority.UserInitiated:
                    return UserInitialized;
                case Priority.Speculative:
                    return Speculative;
                default:
                    return UserInitialized;
            }
        }
    }
}
