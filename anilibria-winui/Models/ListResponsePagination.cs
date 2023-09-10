using System.Text.Json.Serialization;

namespace anilibria.Models
{
    class ListResponsePagination
    {
        [JsonPropertyName("pages")]
        public int Pages { get; set; }
        [JsonPropertyName("current_page")]
        public int CurrentPage { get; set; }
        [JsonPropertyName("items_per_page")]
        public int ItemsPerPage { get; set; }
        [JsonPropertyName("total_items")]
        public int TotalItems { get; set; }
    }
}
