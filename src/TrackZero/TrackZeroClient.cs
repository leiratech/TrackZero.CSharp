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
    /// <summary>
    /// Main class to use TrackZero
    /// </summary>
    public sealed class TrackZeroClient
    {
        private readonly IHttpClientFactory clientFactory;
        private readonly string projectApiKey;

        /// <summary>
        /// Creates a new Instance of TrackZeroClient
        /// </summary>
        /// <param name="clientFactory"></param>
        public TrackZeroClient(IHttpClientFactory clientFactory)
        {
            this.clientFactory = clientFactory;
        }

        /// <summary>
        /// Deletes an entity and all events it emitted.
        /// CAUTION : this action cannot be undone.
        /// </summary>
        /// <param name="type"></param>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task DeleteEntityAsync(string type, object id)
        {
            HttpClient httpClient = clientFactory.CreateClient("TrackZero");
            try
            {
                
                var request = new HttpRequestMessage
                {
                    Method = HttpMethod.Delete,
                    RequestUri = new Uri("/tracking/entities", UriKind.Relative),
                    Content = new StringContent(JsonConvert.SerializeObject(new EntityReference(type, id)), Encoding.UTF8, "application/json"),
                    
                };
                var response = await httpClient.SendAsync(request).ConfigureAwait(false);
                if (response.IsSuccessStatusCode)
                {
                    return;
                }

                throw new Exception(await response.Content.ReadAsStringAsync().ConfigureAwait(false));
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

        public async Task DeleteEntityAsync(EntityReference entityReference)
        {
            await DeleteEntityAsync(entityReference.Type, entityReference.Id).ConfigureAwait(false);
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
                entity.ValidateAndCorrect();
                var response = await httpClient.PostAsync("/tracking/entities", new StringContent(JsonConvert.SerializeObject(entity), Encoding.UTF8, "application/json")).ConfigureAwait(false);
                if (response.IsSuccessStatusCode)
                {
                    return entity;
                }

                throw new Exception(await response.Content.ReadAsStringAsync().ConfigureAwait(false));
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
                foreach (var e in entities)
                    e.ValidateAndCorrect();

                var response = await httpClient.PostAsync("tracking/entities/bulk", new StringContent(JsonConvert.SerializeObject(entities), Encoding.UTF8, "application/json")).ConfigureAwait(false);
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
        /// Deletes an event.
        /// CAUTION : this action cannot be undone.
        /// </summary>
        /// <param name="type"></param>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task DeleteEventAsync(string type, object id)
        {
            HttpClient httpClient = clientFactory.CreateClient("TrackZero");
            try
            {

                var request = new HttpRequestMessage
                {
                    Method = HttpMethod.Delete,
                    RequestUri = new Uri("tracking/events", UriKind.Relative),
                    Content = new StringContent(JsonConvert.SerializeObject(new EntityReference(type, id)), Encoding.UTF8, "application/json")
                };
                var response = await httpClient.SendAsync(request).ConfigureAwait(false);
                if (response.IsSuccessStatusCode)
                {
                    return;
                }

                throw new Exception(await response.Content.ReadAsStringAsync().ConfigureAwait(false));
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

        public async Task DeleteEventAsync(EntityReference entityReference)
        {
            await DeleteEntityAsync(entityReference.Type, entityReference.Id).ConfigureAwait(false);
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
                @event.ValidateAndCorrect();
                var response = await httpClient.PostAsync("tracking/events", new StringContent(JsonConvert.SerializeObject(@event), Encoding.UTF8, "application/json")).ConfigureAwait(false);
                if (response.IsSuccessStatusCode)
                {
                    return @event;
                }

                throw new Exception(await response.Content.ReadAsStringAsync().ConfigureAwait(false));
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
                foreach (var e in events)
                    e.ValidateAndCorrect();

                var response = await httpClient.PostAsync("tracking/events/bulk", new StringContent(JsonConvert.SerializeObject(events), Encoding.UTF8, "application/json")).ConfigureAwait(false);
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
