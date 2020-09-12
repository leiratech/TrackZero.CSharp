using System;
using System.ComponentModel;
using System.Net.Http;

namespace TrackZero
{
    public sealed class TrackZeroClient
    {
        private readonly IHttpClientFactory clientFactory;
        private Uri baseUri;
        private string projectId;
        private string projectSecret;

        public TrackZeroClient(IHttpClientFactory clientFactory, string connectionString)
        {
            this.clientFactory = clientFactory;
            initializeFromConnectionString(connectionString);
        }

        public TrackZeroClient(IHttpClientFactory clientFactory, Uri baseUri, string projectId, string projectSecret)
        {
            this.clientFactory = clientFactory;
            this.baseUri = baseUri;
            this.projectId = projectId;
            this.projectSecret = projectSecret;
        }

        private void initializeFromConnectionString(string connectionString)
        {
            var connectionArray = connectionString.Split(';');
            if (connectionArray.Length< 3)
                throw new InvalidEnumArgumentException("Invalid Connection String");

            foreach (var configLine in connectionArray)
            {
                var configItem = configLine.Split('=');
                if (configItem[0].ToUpperInvariant() == "server".ToUpperInvariant())
                {
                    if (!Uri.TryCreate(configItem[1], UriKind.Absolute, out baseUri))
                    {
                        throw new InvalidEnumArgumentException("Invalid Connection String");
                    }
                }

                if (configItem[0].ToUpperInvariant() == "projectId".ToUpperInvariant())
                {
                    if (!Guid.TryParse(configItem[1], out Guid projectGuid))
                    {
                        throw new InvalidEnumArgumentException("Invalid Connection String");
                    }
                    else
                        projectId = projectGuid.ToString();
                }

                if (configItem[0].ToUpperInvariant() == "projectsecret".ToUpperInvariant())
                {
                    projectSecret = configItem[1];
                }
            }
        }



    }
}
