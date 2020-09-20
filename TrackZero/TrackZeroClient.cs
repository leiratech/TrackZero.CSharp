using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Net;
using System.Net.Http;
using System.Security.Authentication;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading;
using System.Threading.Tasks;
using TrackZero.DataTransfer;

namespace TrackZero
{
    public sealed class TrackZeroClient
    {
        private readonly IHttpClientFactory clientFactory;
        private Uri baseUri;
        private string projectId;
        private string projectSecret;
        TokenResponse tokenResponse;
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
            if (connectionArray.Length < 3)
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

        AutoResetEvent tokenAquisitionLock = new AutoResetEvent(true);
        private async Task<string> getTokenAsync()
        {

            if (tokenResponse?.IsValid ?? false)
            {
                return tokenResponse.access_token;
            }

            tokenAquisitionLock.WaitOne();

            if (tokenResponse?.IsValid ?? false)
            {
                tokenAquisitionLock.Set();
                return tokenResponse.access_token;
            }

            HttpClient httpClient = clientFactory.CreateClient("TrackZero");

            try
            {
                FormUrlEncodedContent formUrlEncodedContent = new FormUrlEncodedContent(new Dictionary<string, string>()
                {
                    { "grant_type", "client_credentials" },
                    { "client_id", projectId },
                    { "client_secret", projectSecret }
                });

                var response = await httpClient.PostAsync("/connect/token", formUrlEncodedContent).ConfigureAwait(false);
                //var str = await response.Content.ReadAsStringAsync().ConfigureAwait(false);
                if (response.StatusCode == System.Net.HttpStatusCode.BadRequest)
                {
                    throw new InvalidCredentialException("Invalid Project Id or Project Secret");
                }

                if (response.IsSuccessStatusCode)
                {
                    var jsonResponse = await response.Content.ReadAsStringAsync().ConfigureAwait(false);
                    tokenResponse = JsonConvert.DeserializeObject<TokenResponse>(jsonResponse);
                    return tokenResponse.access_token;
                }

                throw new Exception("Unable to obtain token, check internet connectivity and try again.");
            }
            catch (Exception ex)
            {
                throw;
            }
            finally
            {
                httpClient.Dispose();
                tokenAquisitionLock.Set();
            }
        }

        /// <summary>
        /// Adds a new entity if it doesn't exist (based on Id and Type) or updates existing one if it exists.
        /// </summary>
        /// <param name="entity">Entity to create. Any EntityReference in CustomAttributes will automatically be created if do not exist.</param>
        /// <returns></returns>
        public async Task<Entity> UpsertEntityAsync(Entity entity)
        {
            HttpClient httpClient = clientFactory.CreateClient("TrackZero");
            try
            {
                string token = await getTokenAsync().ConfigureAwait(false);
                httpClient.DefaultRequestHeaders.Add("Authorization", $"Bearer {token}");
                var response = await httpClient.PutAsync("/v1.0/Tracking/entities", new StringContent(JsonConvert.SerializeObject(entity), Encoding.UTF8, "application/json")).ConfigureAwait(false);
                if (response.IsSuccessStatusCode)
                {
                    return entity;
                }

                throw new Exception("Unknown Error Occured");
            }
            catch (Exception ex)
            {
                throw;
            }
            finally
            {
                httpClient.Dispose();
            }
        }

        public async Task<IEnumerable<Entity>> UpsertEntityAsync(IEnumerable<Entity> entities)
        {
            HttpClient httpClient = clientFactory.CreateClient("TrackZero");
            try
            {
                string token = await getTokenAsync().ConfigureAwait(false);
                httpClient.DefaultRequestHeaders.Add("Authorization", $"Bearer {token}");
                var response = await httpClient.PutAsync("/v1.0/Tracking/entities/bulk", new StringContent(JsonConvert.SerializeObject(entities), Encoding.UTF8, "application/json")).ConfigureAwait(false);
                if (response.IsSuccessStatusCode)
                {
                    return entities;
                }

                throw new Exception("Unknown Error Occured");
            }
            catch (Exception ex)
            {
                throw;
            }
            finally
            {
                httpClient.Dispose();
            }
        }
        /// <summary>
        /// Adds a new event.
        /// </summary>
        /// <param name="event">Event to create. Any EntityReference in CustomAttributes, Emitter and Targets will automatically be created if do not exist.</param>
        /// <returns></returns>
        public async Task<Event> TrackEventAsync(Event @event)
        {
            HttpClient httpClient = clientFactory.CreateClient("TrackZero");
            try
            {
                string token = await getTokenAsync().ConfigureAwait(false);
                httpClient.DefaultRequestHeaders.Add("Authorization", $"Bearer {token}");
                var response = await httpClient.PutAsync("/v1.0/Tracking/events", new StringContent(JsonConvert.SerializeObject(@event), Encoding.UTF8, "application/json")).ConfigureAwait(false);
                if (response.IsSuccessStatusCode)
                {
                    return @event;
                }

                throw new Exception("Unknown Error Occured");
            }
            catch (Exception ex)
            {
                throw;
            }
            finally
            {
                httpClient.Dispose();
            }
        }

        public async Task<IEnumerable<Event>> TrackEventAsync(IEnumerable<Event> events)
        {
            HttpClient httpClient = clientFactory.CreateClient("TrackZero");
            try
            {
                string token = await getTokenAsync().ConfigureAwait(false);
                httpClient.DefaultRequestHeaders.Add("Authorization", $"Bearer {token}");
                var response = await httpClient.PutAsync("/v1.0/Tracking/events/bulk", new StringContent(JsonConvert.SerializeObject(events), Encoding.UTF8, "application/json")).ConfigureAwait(false);
                if (response.IsSuccessStatusCode)
                {
                    return events;
                }

                throw new Exception("Unknown Error Occured");
            }
            catch (Exception ex)
            {
                throw;
            }
            finally
            {
                httpClient.Dispose();
            }
        }
    }
}
