namespace MusicStoreShowcase.Models
{
    public class SongQuery
    {
        public string Region { get; set; } = "en-US";
        public long Seed { get; set; } = 12345;
        public double Likes { get; set; } = 3.5;
        public int Page { get; set; } = 1;
        public int PageSize { get; set; } = 20;
    }
}