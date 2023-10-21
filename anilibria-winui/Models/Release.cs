using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace anilibria.Models
{
    public class Release
    {
        [JsonPropertyName("id")]
        public int Id { get; set; }
        [JsonPropertyName("code")]
        public string Code { get; set; }
        [JsonPropertyName("names")]
        public Names Names { get; set; }
        public string Title { get => Names.Ru; }
        [JsonPropertyName("description")]
        public string Description { get; set; }
        [JsonPropertyName("posters")]
        public Posters Posters { get; set; }
        public string PosterUrl { get => @"https://anilibriaqt.anilib.top" + Posters.Original.Url; }
        public string ThumbnailUrl { get => @"https://anilibriaqt.anilib.top" + Posters.Small.Url; }
        [JsonPropertyName("genres")]
        public string[] Genres { get; set; }
        [JsonPropertyName("type")]
        public ReleaseType Type { get; set; }
        [JsonPropertyName("status")]
        public Status Status { get; set; }
        [JsonPropertyName("season")]
        public Season Season { get; set; }
        public int Year { get => Season.Year; }
        [JsonPropertyName("in_favorites")]
        public int InFavorites { get; set; }
        public string InFavStr { get => InFavorites > 1000 ? $"{InFavorites / 1000.0}K" : $"{InFavorites}"; }
        [JsonPropertyName("updated")]
        public int UpdatedTimestamp { get; set; }
        public DateTimeOffset Updated { get => DateTimeOffset.FromUnixTimeSeconds(UpdatedTimestamp).UtcDateTime.ToLocalTime(); }
        [JsonPropertyName("last_change")]
        public int LastChangeTimestamp { get; set; }
        public DateTime LastChange { get => DateTimeOffset.FromUnixTimeSeconds(LastChangeTimestamp).UtcDateTime.ToLocalTime(); }
        [JsonPropertyName("player")]
        public Player Player { get; set; }
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

    public class Season
    {
        [JsonPropertyName("string")]
        public string String { get; set; }
        [JsonPropertyName("code")]
        public int Code { get; set; }
        [JsonPropertyName("year")]
        public int Year { get; set; }
        [JsonPropertyName("week_day")]
        public int WeekDay { get; set; }
    }

    public class Status
    {
        [JsonPropertyName("string")]
        public string String { get; set; }
        [JsonPropertyName("code")]
        public int Code { get; set; }
    }

    public class ReleaseType
    {
        [JsonPropertyName("full_string")]
        public string FullString { get; set; }
        [JsonPropertyName("code")]
        public int Code { get; set; }
        [JsonPropertyName("string")]
        public string String { get; set; }
        [JsonPropertyName("episodes")]
        public int? Episodes { get; set; }
        [JsonPropertyName("length")]
        public int? Length { get; set; }
    }

    public class Player
    {
        [JsonPropertyName("host")]
        public string Host { get; set; }
        [JsonPropertyName("is_rutube")]
        public bool IsRutube { get; set; }
        [JsonPropertyName("episodes")]
        public PlayerEpisodes Episodes { get; set; }
        [JsonPropertyName("list")]
        public List<Episode> List { get; set; }
    }

    public class PlayerEpisodes
    {
        [JsonPropertyName("first")]
        public int First { get; set; }
        [JsonPropertyName("last")]
        public int Last { get; set; }
        [JsonPropertyName("string")]
        public string String { get; set; }
    }

    public class Episode
    {
        [JsonPropertyName("episode")]
        public int EpisodeNum { get; set; }
        [JsonPropertyName("name")]
        public string Name { get; set; }
        public string Title { get => $"Серия {EpisodeNum} ({Created.ToShortDateString()}) {(Name != "" ? "" + Name : "")}"; }
        [JsonPropertyName("uuid")]
        public string Uuid { get; set; }
        [JsonPropertyName("created_timestamp")]
        public int CreatedTimestamp { get; set; }
        public DateTime Created { get => DateTimeOffset.FromUnixTimeSeconds(CreatedTimestamp).UtcDateTime.ToLocalTime(); }
        [JsonPropertyName("hls")]
        public EpisodeHLS HLS { get; set; }
        public string HLSDescr { get => $"{(HLS.FHD != "" ? "1080" : "")} {(HLS.HD != "" ? "720" : "")} {(HLS.SD != "" ? "480" : "")}"; }
    }

    public class EpisodeHLS
    {
        [JsonPropertyName("fhd")]
        public string FHD { get; set; }
        [JsonPropertyName("hd")]
        public string HD { get; set; }
        [JsonPropertyName("sd")]
        public string SD { get; set; }
    }
}
