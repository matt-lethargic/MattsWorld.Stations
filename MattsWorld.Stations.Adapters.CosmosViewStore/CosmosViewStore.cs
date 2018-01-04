using System;
using System.Linq;
using System.Threading.Tasks;
using MattsWorld.Stations.Domain.Models.Views;
using MattsWorld.Stations.Domain.Ports;
using Microsoft.Azure.Documents.Client;

namespace MattsWorld.Stations.Adapters.CosmosViewStore
{
    public class CosmosStore : IViewStore
    {
        private readonly DocumentClient _client;
        private readonly Uri _collectionUri;

        public CosmosStore(string endpoint, string authkey, string databaseId, string collectionId)
        {
            _client = new DocumentClient(new Uri(endpoint), authkey);
            _collectionUri = UriFactory.CreateDocumentCollectionUri(databaseId, collectionId);
        }

        public T Get<T>(Guid id) where T : View
        {
            var doctype = typeof(T).Name;
            var viewId = $"{id}-{doctype}";

            ViewDocument<T> view = _client.CreateDocumentQuery<ViewDocument<T>>(_collectionUri)
                .Where(db => db.Id == viewId && db.Type == doctype)
                .ToArray()
                .FirstOrDefault();

            return view?.Data;
        }

        public async Task Add<T>(T view) where T : View
        {
            var viewDoc = new ViewDocument<T>(view);
            await _client.CreateDocumentAsync(_collectionUri, viewDoc);
        }

        public async Task Update<T>(T view) where T : View
        {
            var viewDoc = new ViewDocument<T>(view);
            await _client.UpsertDocumentAsync(_collectionUri, viewDoc);
        }
    }
}
