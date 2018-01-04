using System.Configuration;
using MattsWorld.Stations.Adapters.WebAppService;
using MattsWorld.Stations.Domain.Models.Commands.Stations;
using MattsWorld.Stations.Functions.Importing.Models;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Host;
using Newtonsoft.Json;

namespace MattsWorld.Stations.Functions.Importing
{
    public static class ImportStationRecord
    {
        [FunctionName("ImportStationRecord")]
        public static void Run([QueueTrigger("station-imports", Connection = "StationStorage")]string stationImportItem,
            TraceWriter log)
        {
            WebAppSettings settings = new WebAppSettings
            {
                CosmosEndpoint = ConfigurationManager.AppSettings[""],
                CosmosAuthKey = ConfigurationManager.AppSettings[""],
                DatabaseId = "",
                EventCollectionId = "",
                ViewCollectionId = ""
            };

            WebAppService app = new WebAppService(settings);

            StationImportModel stationImportModel = JsonConvert.DeserializeObject<StationImportModel>(stationImportItem);

            log.Info($"Importing station: {stationImportModel.StationName}");

            // todo create import command
            var importStation = new ImportStation(stationImportModel.StationName, stationImportModel.ThreeAlphaCode);

            app.Bus.Send(importStation);

            log.Info($"Imported station: {stationImportModel.StationName}");
        }
    }
}
