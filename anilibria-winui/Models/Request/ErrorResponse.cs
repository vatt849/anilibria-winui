using System.Text.Json.Serialization;

namespace anilibria.Models.Request
{
    class ErrorResponse
    {
        [JsonPropertyName("error")]
        public Error Error { get; set; }
    }

    class Error
    {
        [JsonPropertyName("code")]
        public int Code { get; set; }
        [JsonPropertyName("message")]
        public string Message { get; set; }
    }
}
