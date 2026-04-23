namespace MusicStoreShowcase.Models
{
    public class SongItemVm
    {
        public string Id { get; set; } = "";
        public int Index { get; set; }
        public string Title { get; set; } = "";
        public string Artist { get; set; } = "";
        public string Album { get; set; } = "";
        public string Genre { get; set; } = "";
        public int Likes { get; set; }
    }
}