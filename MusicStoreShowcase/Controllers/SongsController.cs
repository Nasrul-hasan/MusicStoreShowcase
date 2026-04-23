using Microsoft.AspNetCore.Mvc;
using MusicStoreShowcase.Models;
using MusicStoreShowcase.Services;

namespace MusicStoreShowcase.Controllers
{
    [Route("songs")]
    public class SongsController : Controller
    {
        private readonly CoverService _coverService;
        private readonly LocaleService _localeService;
        private readonly AudioPreviewService _audioPreviewService;

        public SongsController(
            CoverService coverService,
            LocaleService localeService,
            AudioPreviewService audioPreviewService)
        {
            _coverService = coverService;
            _localeService = localeService;
            _audioPreviewService = audioPreviewService;
        }

        [HttpGet("list")]
        public IActionResult GetSongs([FromQuery] SongQuery query)
        {
            if (query.Page < 1)
                query.Page = 1;

            if (query.PageSize < 1)
                query.PageSize = 20;

            if (query.PageSize > 100)
                query.PageSize = 100;

            var locale = _localeService.GetLocaleData(query.Region);
            var list = new List<SongItemVm>();

            for (int i = 0; i < query.PageSize; i++)
            {
                int itemIndex = i + 1;
                int sequenceIndex = (query.Page - 1) * query.PageSize + itemIndex;

                string coreKey = SeedHelper.MakeCoreKey(query.Region, query.Seed, query.Page, itemIndex);
                int coreSeed = SeedHelper.ToStableInt(coreKey);
                var coreRandom = new Random(coreSeed);

                string title = $"{Pick(locale.TitleWords, coreRandom)} {Pick(locale.TitleWords, coreRandom)}";
                string artist = $"{Pick(locale.ArtistWords, coreRandom)} {Pick(locale.ArtistWords, coreRandom)}";
                string album = coreRandom.NextDouble() < 0.3
                    ? locale.SingleLabel
                    : $"{Pick(locale.AlbumWords, coreRandom)} {Pick(locale.AlbumWords, coreRandom)}";
                string genre = Pick(locale.Genres, coreRandom);

                string likesKey = SeedHelper.MakeLikesKey(query.Region, query.Seed, query.Page, itemIndex);
                int likesSeed = SeedHelper.ToStableInt(likesKey);
                int likes = LikesHelper.GenerateLikes(query.Likes, likesSeed);

                string id = $"{query.Region}_{query.Seed}_{query.Page}_{itemIndex}";

                list.Add(new SongItemVm
                {
                    Id = id,
                    Index = sequenceIndex,
                    Title = title,
                    Artist = artist,
                    Album = album,
                    Genre = genre,
                    Likes = likes
                });
            }

            return Json(list);
        }

        [HttpGet("details")]
        public IActionResult GetDetails(string id, string title = "", string artist = "", string region = "en-US")
        {
            var locale = _localeService.GetLocaleData(region);
            var seed = SeedHelper.ToStableInt($"review:{id}");
            var rng = new Random(seed);

            var phrase = locale.ReviewPhrases.Count > 0
                ? locale.ReviewPhrases[rng.Next(locale.ReviewPhrases.Count)]
                : "Generated review.";

            var details = new SongDetailsVm
            {
                Id = id,
                Review = $"\"{title}\" — {phrase}",
                CoverUrl = $"/songs/cover?id={Uri.EscapeDataString(id)}&title={Uri.EscapeDataString(title)}&artist={Uri.EscapeDataString(artist)}",
                PreviewUrl = $"/songs/preview?id={Uri.EscapeDataString(id)}"
            };

            return Json(details);
        }

        [HttpGet("cover")]
        public IActionResult GetCover(string id, string title = "Unknown Title", string artist = "Unknown Artist")
        {
            var bytes = _coverService.GenerateCover(id, title, artist);
            return File(bytes, "image/png");
        }

        [HttpGet("preview")]
        public IActionResult GetPreview(string id)
        {
            var bytes = _audioPreviewService.GeneratePreview(id);
            return File(bytes, "audio/wav");
        }

        private static string Pick(List<string> arr, Random rng)
        {
            if (arr == null || arr.Count == 0)
                return "Unknown";

            return arr[rng.Next(arr.Count)];
        }
    }
}