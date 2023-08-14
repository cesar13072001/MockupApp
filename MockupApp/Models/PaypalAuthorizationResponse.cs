using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MockupApp.Models
{
    public class PaypalAuthorizationResponse
    {
        [JsonProperty("scope")]
        public Uri Scope { get; set; }

        [JsonProperty("access_token")]
        public string AccessToken { get; set; }

        [JsonProperty("token_type")]
        public string TokenType { get; set; }

        [JsonProperty("app_id")]
        public string AppId { get; set; }

        [JsonProperty("expires_in")]
        public long ExpiresIn { get; set; }

        [JsonProperty("supported_authn_schemes")]
        public List<string> SupportedAuthnSchemes { get; set; }

        [JsonProperty("nonce")]
        public string Nonce { get; set; }

        [JsonProperty("client_metadata")]
        public ClientMetadata ClientMetadata { get; set; }
    }

    public class ClientMetadata
    {
        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("display_name")]
        public string DisplayName { get; set; }

        [JsonProperty("logo_uri")]
        public string LogoUri { get; set; }

        [JsonProperty("scopes")]
        public List<string> Scopes { get; set; }

        [JsonProperty("ui_type")]
        public string UiType { get; set; }
    }
}