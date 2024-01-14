using anilibria.Models;

namespace anilibria.Pages.Helpers
{
    internal class PlayerData
    {
        public Release Release;
        public Episode Episode;
        public string HLS;
        public int Timecode;

        public bool IsEpisodeLast { get { return Release.Player.Episodes.Last == Episode.EpisodeNum; } }
        public bool IsEpisodeFirst { get { return Release.Player.Episodes.First == Episode.EpisodeNum; } }

        public bool IsEpisodeNotLast { get { return Release.Player.Episodes.Last != Episode.EpisodeNum; } }
        public bool IsEpisodeNotFirst { get { return Release.Player.Episodes.First != Episode.EpisodeNum; } }
    }
}
