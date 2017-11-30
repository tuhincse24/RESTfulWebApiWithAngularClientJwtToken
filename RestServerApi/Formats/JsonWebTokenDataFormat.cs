﻿using Microsoft.IdentityModel.Tokens;
using Microsoft.Owin.Security;
using Microsoft.Owin.Security.DataHandler.Encoder;
using RestServerApi.Entities;
using RestServerApi.InMemoryDataStores;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
//using Thinktecture.IdentityModel.Tokens;

namespace RestServerApi.Formats
{
    public class JsonWebTokenDataFormat : ISecureDataFormat<AuthenticationTicket>
    {
        private const string AudiencePropertyKey = "audience";

        private readonly string _issuer = string.Empty;

        public JsonWebTokenDataFormat(string issuer)
        {
            _issuer = issuer;
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

            //var signingKey = new HmacSigningCredentials(keyByteArray);

            //const string sec = "401b09eab3c013d4ca54922bb802bec8fd5318192b0a75f201d8b3727429090fb337591abd3e44453b954555b7a0812e1081c39b740293f765eae731f5a65ed1";
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
            var issuer = "http://www.RestServerApi.com.bd";
            var audience = "IAmTheFirstClient";
            var secret = TextEncodings.Base64Url.Decode("IxrAjDoa2FqElO7IhrSrUJELhUckePEPVpaePlS_Xaw");
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

            var validationParameters = new TokenValidationParameters { IssuerSigningKey = new SymmetricSecurityKey(secret), ValidAudiences = new[] { audience }, ValidateIssuer = true, ValidIssuer = this._issuer, ValidateLifetime = true, ValidateAudience = true, ValidateIssuerSigningKey = true };

            var tokenHandler = new JwtSecurityTokenHandler();
            SecurityToken validatedToken = null;

            ClaimsPrincipal claimsPrincipal = tokenHandler.ValidateToken(protectedText, validationParameters, out validatedToken);
            var claimsIdentity = (ClaimsIdentity)claimsPrincipal.Identity;

            var authenticationExtra = new AuthenticationProperties(new Dictionary<string, string>());
            //if (claimsIdentity.Claims.Any(c => c.Type == ExpiryClaimName))
            //{
            //    string expiryClaim = (from c in claimsIdentity.Claims where c.Type == ExpiryClaimName select c.Value).Single();
            //    authenticationExtra.ExpiresUtc = _epoch.AddSeconds(Convert.ToInt64(expiryClaim, CultureInfo.InvariantCulture));
            //}

            //if (claimsIdentity.Claims.Any(c => c.Type == IssuedAtClaimName))
            //{
            //    string issued = (from c in claimsIdentity.Claims where c.Type == IssuedAtClaimName select c.Value).Single();
            //    authenticationExtra.IssuedUtc = _epoch.AddSeconds(Convert.ToInt64(issued, CultureInfo.InvariantCulture));
            //}

            var returnedIdentity = new ClaimsIdentity(claimsIdentity.Claims, "JWT");

            return new AuthenticationTicket(returnedIdentity, authenticationExtra);
        }
    }
}