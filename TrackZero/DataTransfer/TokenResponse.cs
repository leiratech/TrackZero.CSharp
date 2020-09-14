using System;
using System.Collections.Generic;
using System.Text;

namespace TrackZero.DataTransfer
{
    internal class TokenResponse
    {
        public TokenResponse()
        {
            initTime = DateTime.UtcNow;
        }


        public string access_token { get; set; }
        public int expires_in { get; set; }
        public string token_type { get; set; }

        // token is considered not valid if 95% of it's lifetime has passed.
        public bool IsValid => DateTime.UtcNow < initTime.AddSeconds(expires_in * 0.95);
        private DateTime initTime { get; set; }
    }
}
