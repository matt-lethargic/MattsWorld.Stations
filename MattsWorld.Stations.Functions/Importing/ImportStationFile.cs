using System;
using System.IO;
using System.Text;
using FileHelpers;
using MattsWorld.Stations.Functions.Importing.Models;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Host;
using Microsoft.WindowsAzure.Storage.Queue;
using Newtonsoft.Json;

namespace MattsWorld.Stations.Functions.Importing
{
    public static class ImportStationFile
    {
        [FunctionName("ImportStationFile")]
        public static void Run(
            [BlobTrigger("stations/import/{name}", Connection = "StationStorage")]Stream importBlob, 
            string name,
            [Queue("station-imports", Connection = "StationStorage")]CloudQueue outputQueue,
            TraceWriter log)
        {
            log.Info($"C# Blob trigger function Processed blob\n Name:{name} \n Size: {importBlob.Length} Bytes");

            //read file
            var file = new StreamReader(importBlob);

            StringBuilder sb = new StringBuilder();
            string fileLine;
            while ((fileLine = file.ReadLine()) != null)
            {
                if (!fileLine.ToLower().StartsWith("a") || fileLine.ToLower().Contains("file-spec"))
                {
                    continue;
                }

                sb.AppendLine(fileLine);
            }

            file.Close();

            // parse lines
            var engine = new FixedFileEngine<StationImportModel>();
            StationImportModel[] stationImportModels = engine.ReadString(sb.ToString());
            log.Info($"Parsed {stationImportModels.Length} stations.");

            // add to queue
            foreach (var stationImportModel in stationImportModels)
            {
                outputQueue.AddMessage(new CloudQueueMessage(JsonConvert.SerializeObject(stationImportModel)), TimeSpan.FromMinutes(10));
                log.Info($"Station {stationImportModel.StationName} added to queue.");
            }
        }
    }
}
