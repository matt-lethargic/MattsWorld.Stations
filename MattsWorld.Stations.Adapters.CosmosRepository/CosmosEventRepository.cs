using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MattsWorld.Stations.Adapters.CosmosRepository.Converters;
using MattsWorld.Stations.Adapters.CosmosRepository.Models;
using MattsWorld.Stations.Domain.Interfaces;
using MattsWorld.Stations.Domain.Models;
using MattsWorld.Stations.Domain.Models.Entities;
using MattsWorld.Stations.Domain.Ports;
using Microsoft.Azure.Documents;
using Microsoft.Azure.Documents.Client;
using Newtonsoft.Json;

namespace MattsWorld.Stations.Adapters.CosmosRepository
{
    public class CosmosEventRepository<T> : IRepository<T> where T : AggregateRoot, new()
    {
        private readonly IEventPublisher _publisher;
        private readonly DocumentClient _client;
        private readonly Uri _collectionUri;
        private readonly Uri _databaseUri;
        private readonly string _collectionId;

        public CosmosEventRepository(string endpoint, string authkey, string databaseId, string collectionId, IEventPublisher publisher)
        {
            if (string.IsNullOrEmpty(endpoint)) throw new ArgumentException(nameof(endpoint));
            if (string.IsNullOrEmpty(authkey)) throw new ArgumentException(nameof(authkey));
            if (string.IsNullOrEmpty(databaseId)) throw new ArgumentException(nameof(databaseId));
            if (string.IsNullOrEmpty(collectionId)) throw new ArgumentException(nameof(collectionId));

            JsonSerializerSettings jsonSettings = new JsonSerializerSettings
            {
                Converters = new List<JsonConverter>
                {
                    new EventDescriptionConverter()
                }
            };

            _publisher = publisher ?? throw new ArgumentNullException(nameof(publisher));
            _client = new DocumentClient(new Uri(endpoint), authkey, jsonSettings);
            _collectionId = collectionId;
            _collectionUri = UriFactory.CreateDocumentCollectionUri(databaseId, collectionId);
            _databaseUri = UriFactory.CreateDatabaseUri(databaseId);
        }

        public async Task<T> GetById(Guid id)
        {
            var obj = new T(); // lots of ways to do this
            var e = await GetEventsForAggregate(id);
            obj.LoadFromHistory(e);
            return obj;
        }

        public async Task Save(AggregateRoot aggregate, int expectedVersion)
        {
            await SaveEvents(aggregate.Id, aggregate.GetUncomittedChanges(), expectedVersion);
        }


        private async Task<List<Event>> GetEventsForAggregate(Guid aggregateId)
        {
            await _client.CreateDocumentCollectionIfNotExistsAsync(_databaseUri, new DocumentCollection { Id = _collectionId });

            var eventDescriptors = _client.CreateDocumentQuery<EventDescriptor>(_collectionUri)
                .Where(e => e.AggregateId == aggregateId)
                .OrderByDescending(e => e.Version)
                .AsEnumerable()
                .ToList();

            return eventDescriptors.Select(desc => desc.EventData).ToList();
        }

        private async Task SaveEvents(Guid aggregateId, IEnumerable<Event> events, int expectedVerion)
        {
            await _client.CreateDocumentCollectionIfNotExistsAsync(_databaseUri, new DocumentCollection { Id = _collectionId });

            var eventDescriptors = _client.CreateDocumentQuery<EventDescriptor>(_collectionUri)
                .Where(e => e.AggregateId == aggregateId)
                .OrderByDescending(e => e.Version)
                .AsEnumerable()
                .ToList();

            if (!eventDescriptors.Any())
            {

            }
            else if (eventDescriptors[eventDescriptors.Count - 1].Version != expectedVerion && expectedVerion != -1)
            {
                throw new ConcurrencyException();
            }

            var i = expectedVerion;

            foreach (var @event in events)
            {
                i++;
                @event.Version = i;

                await _client.CreateDocumentAsync(_collectionUri, new EventDescriptor(aggregateId, @event, @event.GetType().FullName, i));

                _publisher.Publish(@event);
            }
        }
    }
}
