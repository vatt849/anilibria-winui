using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace anilibria.Models.Request
{
    public class PaginatedReleasesResponse
    {
        [JsonPropertyName("list")]
        public List<Release> List { get; set; }
        [JsonPropertyName("pagination")]
        public ResponsePagination Pagination { get; set; }
    }
}
