using Microsoft.Win32;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace GeoTunes
{
    public class ETSConnection
    {
        private string url;

        public bool isShuttingDown = false;

        TelemetryData data;


        public ETSConnection(string url)
        {
            this.url = url;
        }

        public List<City> GetCities()
        {
            string baseDirectory = AppDomain.CurrentDomain.BaseDirectory;
            string targetDir = Directory.GetParent(baseDirectory)?.Parent?.Parent?.FullName;
            string citiesJson = File.ReadAllText(targetDir + "\\cities.json");
            JObject jObject = JObject.Parse(citiesJson);
            return jObject["citiesList"].ToObject<List<City>>();
            
        }

        public string GetCoordinatesForJson()
        {
            return "\"gameName\": \"\",\r\n      " +
                "\"realName\": \"\",\r\n      " +
                "\"country\": \"\",\r\n      " +
                "\"x\": \"" + data.truck.placement.x.ToString().Replace(",", ".") + "\",\r\n      " +
                "\"y\": \"" + data.truck.placement.y.ToString().Replace(",", ".") + "\",\r\n      " +
                "\"z\": \"" + data.truck.placement.z.ToString().Replace(",", ".") + "\"";
        }

        public async Task<Placement> GetPlacement()
        {
            await RefreshTelemetry();
            return data.truck.placement;
        }

        private async Task RefreshTelemetry()
        {
            string json = await FetchUrlAsync();
            if(isShuttingDown)
            {
                Application.Current.Shutdown();
            }
            data = JsonConvert.DeserializeObject<TelemetryData>(json);
        }

        private async Task<string> FetchUrlAsync()
        {
            using (HttpClient client = new HttpClient())
            {
                try
                {
                    return await client.GetStringAsync(this.url);
                }
                catch (Exception ex)
                {
                    if(!isShuttingDown)
                    {
                        isShuttingDown = true;
                        MessageBox.Show($"Error: {ex.Message}");
                    }
                    return string.Empty;
                }
            }
        }
    }
}

