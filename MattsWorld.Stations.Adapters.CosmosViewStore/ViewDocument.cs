using MattsWorld.Stations.Domain.Models.Views;
using Newtonsoft.Json;

namespace MattsWorld.Stations.Adapters.CosmosViewStore
{
    public class ViewDocument<T> where T : View
    {
        [JsonProperty(PropertyName = "id")]
        public string Id { get; set; }

        public T Data { get; set; }

        public string Type { get; set; }

        public ViewDocument(T data)
        {
            Data = data;
            Type = data.GetType().Name;
            Id = $"{data.Id}-{Type}";
        }
    }
}