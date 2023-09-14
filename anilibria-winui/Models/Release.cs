using System.Text.Json.Serialization;

namespace anilibria.Models
{
    public class Release
    {
        [JsonPropertyName("id")]
        public int Id { get; set; }
        [JsonPropertyName("code")]
        public string Code { get; set; }
        public string Title { get => Names.Ru; }
        [JsonPropertyName("names")]
        public Names Names { get; set; }
        [JsonPropertyName("description")]
        public string Description { get; set; }
        [JsonPropertyName("posters")]
        public Posters Posters { get; set; }
        public string PosterUrl { get => @"https://anilibriaqt.anilib.top" + Posters.Original.Url; }
        public string ThumbnailUrl { get => @"https://anilibriaqt.anilib.top" + Posters.Small.Url; }
        [JsonPropertyName("genres")]
        public string[] Genres { get; set; }
    }

    public class Names
    {
        [JsonPropertyName("ru")]
        public string Ru { get; set; }
        [JsonPropertyName("en")]
        public string En { get; set; }
        [JsonPropertyName("alternative")]
        public string Alternative { get; set; }
    }

    public class Posters
    {
        [JsonPropertyName("small")]
        public PosterImage Small { get; set; }
        [JsonPropertyName("medium")]
        public PosterImage Medium { get; set; }
        [JsonPropertyName("original")]
        public PosterImage Original { get; set; }
    }

    public class PosterImage
    {
        [JsonPropertyName("url")]
        public string Url { get; set; }
        [JsonPropertyName("raw_base64_file")]
        public string RawBase64File { get; set; }
    }
}
