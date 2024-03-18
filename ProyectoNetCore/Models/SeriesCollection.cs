using Newtonsoft.Json;

namespace ProyectoNetCore.Models
{
    public class ChartViewModel
    {
        [JsonProperty("title")]
        public string Title { get; set; }
        [JsonProperty("subtitle")]
        public string Subtitle { get; set; }

        //[JsonProperty("dataTable")]
        //public VisualizationDataTable DataTable { get; set; }
    }
}
