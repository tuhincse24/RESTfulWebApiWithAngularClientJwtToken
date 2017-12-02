using Microsoft.IdentityModel.Tokens;
using Microsoft.Owin.Security;
using Microsoft.Owin.Security.DataHandler.Encoder;
using RestServerApi.Entities;
using RestServerApi.InMemoryDataStores;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace RestServerApi.Formats
{
    public class JsonWebTokenDataFormat : ISecureDataFormat<AuthenticationTicket>
    {
        private const string AudiencePropertyKey = "audience";

        private readonly string _issuer = string.Empty;
        private readonly string _client_id = string.Empty;
        private readonly string _secretkey = string.Empty;

        public JsonWebTokenDataFormat()
        {
            _issuer = ConfigurationManager.AppSettings.Get("issuer");
            _client_id = ConfigurationManager.AppSettings.Get("client.id");
            _secretkey = ConfigurationManager.AppSettings.Get("secretkey"); 
        }

        public string Protect(AuthenticationTicket data)
        {
            if (data == null)
            {
                throw new ArgumentNullException("data");
            }

            string audienceId = data.Properties.Dictionary.ContainsKey(AudiencePropertyKey) ? data.Properties.Dictionary[AudiencePropertyKey] : null;

            if (string.IsNullOrWhiteSpace(audienceId)) throw new InvalidOperationException("AuthenticationTicket.Properties does not include audience");

            Audience audience = AudiencesStore.FindAudience(audienceId);

            string symmetricKeyAsBase64 = audience.Base64Secret;

            var keyByteArray = TextEncodings.Base64Url.Decode(symmetricKeyAsBase64);
            var securityKey = new SymmetricSecurityKey(TextEncodings.Base64Url.Decode(symmetricKeyAsBase64));
            var signingCredentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

            var issued = data.Properties.IssuedUtc;
            var expires = data.Properties.ExpiresUtc;

            var token = new JwtSecurityToken(_issuer, audienceId, data.Identity.Claims, issued.Value.UtcDateTime, expires.Value.UtcDateTime, signingCredentials);

            var handler = new JwtSecurityTokenHandler();

            var jsonWebToken = handler.WriteToken(token);

            return jsonWebToken;
        }

        public AuthenticationTicket Unprotect(string protectedText)
        {
            var secret = TextEncodings.Base64Url.Decode(_secretkey);
            if (string.IsNullOrWhiteSpace(protectedText))
            {
                throw new ArgumentNullException("protectedText");
            }

            var handler = new JwtSecurityTokenHandler();
            var token = handler.ReadToken(protectedText) as JwtSecurityToken;

            if (token == null)
            {
                throw new ArgumentOutOfRangeException("protectedText", "Invalid JWT Token");
            }
            ClaimsPrincipal claimsPrincipal;
            try
            {
                var validationParameters = new TokenValidationParameters { IssuerSigningKey = new SymmetricSecurityKey(secret), ValidateAudience = true, ValidAudiences = new[] { this._client_id }, ValidateIssuer = true, ValidIssuer = this._issuer, ValidateLifetime = true, ValidateIssuerSigningKey = true };

                var tokenHandler = new JwtSecurityTokenHandler();
                SecurityToken validatedToken = null;

                claimsPrincipal = tokenHandler.ValidateToken(protectedText, validationParameters, out validatedToken);
            }
            catch (Exception)
            {
                return null;
            }
            var claimsIdentity = (ClaimsIdentity)claimsPrincipal.Identity;

            var authenticationExtra = new AuthenticationProperties(new Dictionary<string, string>());

            var returnedIdentity = new ClaimsIdentity(claimsIdentity.Claims, "JWT");

            return new AuthenticationTicket(returnedIdentity, authenticationExtra);
        }
    }
}