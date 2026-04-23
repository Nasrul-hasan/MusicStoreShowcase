using Microsoft.AspNetCore.Hosting;
using SixLabors.Fonts;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.Drawing;
using SixLabors.ImageSharp.Drawing.Processing;
using SixLabors.ImageSharp.Formats.Png;
using SixLabors.ImageSharp.PixelFormats;
using SixLabors.ImageSharp.Processing;

namespace MusicStoreShowcase.Services
{
    public class CoverService
    {
        private readonly IWebHostEnvironment _environment;

        public CoverService(IWebHostEnvironment environment)
        {
            _environment = environment;
        }

        public byte[] GenerateCover(string songId, string title, string artist)
        {
            int seed = SeedHelper.ToStableInt($"cover:{songId}");
            var rng = new Random(seed);

            string coversPath = System.IO.Path.Combine(_environment.WebRootPath, "assets", "covers");

            if (!Directory.Exists(coversPath))
            {
                throw new DirectoryNotFoundException($"Covers folder not found: {coversPath}");
            }

            string[] files = Directory.GetFiles(coversPath)
                .Where(f =>
                    f.EndsWith(".jpg", StringComparison.OrdinalIgnoreCase) ||
                    f.EndsWith(".jpeg", StringComparison.OrdinalIgnoreCase) ||
                    f.EndsWith(".png", StringComparison.OrdinalIgnoreCase) ||
                    f.EndsWith(".webp", StringComparison.OrdinalIgnoreCase))
                .ToArray();

            if (files.Length == 0)
            {
                throw new FileNotFoundException($"No cover images found in: {coversPath}");
            }

            string selectedFile = files[rng.Next(files.Length)];

            using var image = Image.Load<Rgba32>(selectedFile);

            image.Mutate(ctx =>
            {
                ctx.Resize(new ResizeOptions
                {
                    Size = new Size(300, 300),
                    Mode = ResizeMode.Crop
                });

                ctx.Fill(Color.Black.WithAlpha(0.35f));

                ctx.Draw(
                    Color.White.WithAlpha(0.4f),
                    2,
                    new RectangularPolygon(15, 15, 270, 270)
                );
            });

            var fontFamily = SystemFonts.Collection.Families.First();
            Font titleFont = fontFamily.CreateFont(26, FontStyle.Bold);
            Font artistFont = fontFamily.CreateFont(16, FontStyle.Regular);

            image.Mutate(ctx =>
            {
                ctx.DrawText(title, titleFont, Color.White, new PointF(20, 200));
                ctx.DrawText(artist, artistFont, Color.LightGray, new PointF(20, 240));
            });

            using var ms = new MemoryStream();
            image.Save(ms, new PngEncoder());
            return ms.ToArray();
        }
    }
}