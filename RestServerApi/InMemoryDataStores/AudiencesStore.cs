using Microsoft.Owin.Security.DataHandler.Encoder;
using RestServerApi.Entities;
using System;
using System.Collections.Concurrent;
using System.Configuration;
using System.Security.Cryptography;

namespace RestServerApi.InMemoryDataStores
{
    public static class AudiencesStore
    {
        public static ConcurrentDictionary<string, Audience> AudiencesList = new ConcurrentDictionary<string, Audience>();

        static AudiencesStore()
        {
            var clientId = ConfigurationManager.AppSettings.Get("client.id");
            AudiencesList.TryAdd(clientId,
                                new Audience
                                {
                                    ClientId = clientId,
                                    Base64Secret = ConfigurationManager.AppSettings.Get("secretkey"),
                                    Name = ConfigurationManager.AppSettings.Get("issuer")
                                });
        }

        public static Audience AddAudience(string name)
        {
            var clientId = Guid.NewGuid().ToString("N");

            var key = new byte[32];
            RNGCryptoServiceProvider.Create().GetBytes(key);
            var base64Secret = TextEncodings.Base64Url.Encode(key);

            Audience newAudience = new Audience { ClientId = clientId, Base64Secret = base64Secret, Name = name };
            AudiencesList.TryAdd(clientId, newAudience);
            return newAudience;
        }

        public static Audience FindAudience(string clientId)
        {
            Audience audience = null;
            if (AudiencesList.TryGetValue(clientId, out audience))
            {
                return audience;
            }
            return null;
        }
    }
}