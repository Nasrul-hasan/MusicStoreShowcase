namespace MusicStoreShowcase.Models
{
    public class LocaleData
    {
        public string SingleLabel { get; set; } = "Single";
        public List<string> TitleWords { get; set; } = new();
        public List<string> ArtistWords { get; set; } = new();
        public List<string> AlbumWords { get; set; } = new();
        public List<string> Genres { get; set; } = new();
        public List<string> ReviewPhrases { get; set; } = new();
    }
}