using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Security.Authentication;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading;
using System.Threading.Tasks;
using TrackZero.Abstract;
using TrackZero.DataTransfer;

namespace TrackZero
{
    /// <summary>
    /// Main class to use TrackZero
    /// </summary>
    public sealed class TrackZeroClient
    {
        private readonly IHttpClientFactory clientFactory;
        private readonly ILogger logger;
        private readonly bool throwExceptions;

        private const int BulkPageSize = 250;
        /// <summary>
        /// Creates a new Instance of TrackZeroClient
        /// </summary>
        /// <param name="clientFactory"></param>
        /// <param name="logger"></param>
        /// <param name="throwExceptions"></param>
        public TrackZeroClient(IServiceProvider serviceProvider, IHttpClientFactory clientFactory, bool throwExceptions)
        {
            this.clientFactory = clientFactory;
            var loggerFactory = serviceProvider.GetService<ILoggerFactory>();
            this.logger = loggerFactory?.CreateLogger<TrackZeroClient>();
            this.throwExceptions = throwExceptions;
        }

        /// <summary>
        /// Deletes an entity and all events it emitted.
        /// CAUTION : this action cannot be undone.
        /// </summary>
        /// <param name="type">The type of the entity to delete.</param>
        /// <param name="id">The id of the entity to delete.</param>
        /// <returns></returns>
        public async Task<TrackZeroOperationResult<EntityReference>> DeleteEntityAsync(string type, object id)
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
                    logger?.LogInformation("Deleting Entity Type = {type}, id = {id} successful.", type, id);
                    return new TrackZeroOperationResult<EntityReference>(new EntityReference(type, id));
                }

                logger?.LogError("Deleting Entity Type = {type}, id = {id} failed with status code {code}.", type, id, response.StatusCode);
                return TrackZeroOperationResult<EntityReference>.GenericFailure;
            }
            catch (Exception ex)
            {
                logger?.LogCritical(ex, "Deleting Entity Type = {type}, id = {id} threw an exception.", type, id);

                if (throwExceptions)
                    throw;

                return new TrackZeroOperationResult<EntityReference>(ex);
            }
            finally
            {
                httpClient.Dispose();
            }
        }

        /// <summary>
        /// Deletes an entity and all events it emitted.
        /// CAUTION : this action cannot be undone.
        /// </summary>
        /// <param name="entityReference">The entity reference to delete.</param>
        /// <returns></returns>
        public async Task<TrackZeroOperationResult<EntityReference>> DeleteEntityAsync(EntityReference entityReference)
        {
            return await DeleteEntityAsync(entityReference.Type, entityReference.Id).ConfigureAwait(false);
        }

        /// <summary>
        /// Adds a new entity if it doesn't exist (based on Id and Type) or updates existing one if it exists.
        /// </summary>
        /// <param name="entity">Entity to create. Any EntityReference in CustomAttributes will automatically be created if do not exist.</param>
        /// <returns></returns>
        public async Task<TrackZeroOperationResult<Entity>> UpsertEntityAsync(Entity entity)
        {
            HttpClient httpClient = clientFactory.CreateClient("TrackZero");
            try
            {
                entity.ValidateAndCorrect();

                var response = await httpClient.PostAsync("/tracking/entities", new StringContent(JsonConvert.SerializeObject(entity), Encoding.UTF8, "application/json")).ConfigureAwait(false);
                if (response.IsSuccessStatusCode)
                {
                    logger?.LogInformation("Upsert Entity Type = {type}, id = {id} successful.", entity.Type, entity.Id);
                    return new TrackZeroOperationResult<Entity>(entity);
                }

                logger?.LogError("Upsert Entity Type = {type}, id = {id} failed with status code {code}.", entity.Type, entity.Id, response.StatusCode);
                return TrackZeroOperationResult<Entity>.GenericFailure;
            }
            catch (Exception ex)
            {
                logger?.LogCritical(ex, "Upsert Entity Type = {type}, id = {id} threw an exception.", entity.Type, entity.Id);

                if (throwExceptions)
                    throw;

                return new TrackZeroOperationResult<Entity>(ex);
            }
            finally
            {
                httpClient.Dispose();
            }
        }


        public async Task<TrackZeroOperationResult<IEnumerable<Entity>>> UpsertEntityAsync(IEnumerable<Entity> entities)
        {
            HttpClient httpClient = clientFactory.CreateClient("TrackZero");
            try
            {
                foreach (var e in entities)
                    e.ValidateAndCorrect();

                var response = await httpClient.PostAsync("tracking/entities/bulk", new StringContent(JsonConvert.SerializeObject(entities), Encoding.UTF8, "application/json")).ConfigureAwait(false);
                if (response.IsSuccessStatusCode)
                {
                    logger?.LogInformation("Bulk Upsert of {count} Entities successfull.", entities.Count());
                    return new TrackZeroOperationResult<IEnumerable<Entity>>(entities);
                }

                logger?.LogError("Bulk Upsert of {count} Entities failed with status code {code}.", entities.Count(), response.StatusCode);
                return TrackZeroOperationResult<IEnumerable<Entity>>.GenericFailure;
            }
            catch (Exception ex)
            {
                logger?.LogCritical(ex, "Bulk Upsert of {count} Entities threw an exception.", entities.Count());

                if (throwExceptions)
                    throw;

                return new TrackZeroOperationResult<IEnumerable<Entity>>(ex);
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
        /// <param name="type">The type of the event to delete.</param>
        /// <param name="id">The id of the event to delete.</param>
        /// <returns></returns>
        public async Task<TrackZeroOperationResult<EntityReference>> DeleteEventAsync(string type, object id)
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
                    logger?.LogInformation("Deleting Event Type = {type}, id = {id} successful.", type, id);
                    return new TrackZeroOperationResult<EntityReference>(new EntityReference(type, id));
                }

                logger?.LogError("Deleting Event Type = {type}, id = {id} failed with status code {code}.", type, id, response.StatusCode);
                return TrackZeroOperationResult<EntityReference>.GenericFailure;
            }
            catch (Exception ex)
            {
                logger?.LogCritical(ex, "Deleting Event Type = {type}, id = {id} threw an exception.", type, id);

                if (throwExceptions)
                    throw;

                return new TrackZeroOperationResult<EntityReference>(ex);
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
        /// <param name="entityReference">The event reference to delete.</param>
        /// <returns></returns>
        public async Task<TrackZeroOperationResult<EntityReference>> DeleteEventAsync(EntityReference entityReference)
        {
            return await DeleteEntityAsync(entityReference.Type, entityReference.Id).ConfigureAwait(false);
        }

        /// <summary>
        /// Adds a new event.
        /// </summary>
        /// <param name="event">Event to create. Any EntityReference in CustomAttributes, Emitter and Targets will automatically be created if do not exist.</param>
        /// <returns></returns>
        public async Task<TrackZeroOperationResult<Event>> UpsertEventAsync(Event @event)
        {
            HttpClient httpClient = clientFactory.CreateClient("TrackZero");
            try
            {
                @event.ValidateAndCorrect();
                var response = await httpClient.PostAsync("tracking/events", new StringContent(JsonConvert.SerializeObject(@event), Encoding.UTF8, "application/json")).ConfigureAwait(false);
                if (response.IsSuccessStatusCode)
                {
                    logger?.LogInformation("Upsert Event Name = {type}, id = {id} successful.", @event.Name, @event.Id);
                    return new TrackZeroOperationResult<Event>(@event);
                }

                logger?.LogError("Upsert Event Name = {type}, id = {id} failed with status code {code}.", @event.Name, @event.Id, response.StatusCode);
                return TrackZeroOperationResult<Event>.GenericFailure;
            }
            catch (Exception ex)
            {
                logger?.LogCritical(ex, "Upsert Event Name = {type}, id = {id} threw an exception.", @event.Name, @event.Id);

                if (throwExceptions)
                    throw;

                return new TrackZeroOperationResult<Event>(ex);
            }
            finally
            {
                httpClient.Dispose();
            }
        }

        [Obsolete]
        public async Task<TrackZeroOperationResult<Event>> TrackEventAsync(Event @event)
        {
            logger?.LogWarning("You are using TrackEventAsync which is obselete, please use UpsertEventAsync instead.");
            return await UpsertEventAsync(@event).ConfigureAwait(false);
        }


        public async Task<TrackZeroOperationResult<IEnumerable<Event>>> UpsertEventAsync(IEnumerable<Event> events)
        {
            HttpClient httpClient = clientFactory.CreateClient("TrackZero");
            try
            {
                foreach (var e in events)
                    e.ValidateAndCorrect();

                var response = await httpClient.PostAsync("tracking/events/bulk", new StringContent(JsonConvert.SerializeObject(events), Encoding.UTF8, "application/json")).ConfigureAwait(false);
                if (response.IsSuccessStatusCode)
                {
                    logger?.LogInformation("Bulk Upsert of {count} Events successfull.", events.Count());
                    return new TrackZeroOperationResult<IEnumerable<Event>>(events);
                }

                logger?.LogError("Bulk Upsert of {count} Events failed with status code {code}.", events.Count(), response.StatusCode);
                return TrackZeroOperationResult<IEnumerable<Event>>.GenericFailure;
            }
            catch (Exception ex)
            {
                logger?.LogCritical(ex, "Bulk Upsert of {count} Entities threw an exception.", events.Count());

                if (throwExceptions)
                    throw;

                return new TrackZeroOperationResult<IEnumerable<Event>>(ex);

            }
            finally
            {
                httpClient.Dispose();
            }
        }

        [Obsolete]
        public async Task<TrackZeroOperationResult<IEnumerable<Event>>> TrackEventAsync(IEnumerable<Event> events)
        {
            logger?.LogWarning("You are using TrackEventAsync which is obselete, please use UpsertEventAsync instead.");
            return await UpsertEventAsync(events).ConfigureAwait(false);
        }
    }
}
