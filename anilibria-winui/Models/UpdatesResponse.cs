using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace anilibria.Models
{
    class UpdatesResponse
    {
        [JsonPropertyName("list")]
        public List<Release> List { get; set; }
        [JsonPropertyName("pagination")]
        public ListResponsePagination Pagination { get; set; }
    }
}
