using Microsoft.AspNetCore.Hosting;
using MusicStoreShowcase.Services;
using SixLabors.Fonts;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.Drawing;
using SixLabors.ImageSharp.Drawing.Processing;
using SixLabors.ImageSharp.Formats.Png;
using SixLabors.ImageSharp.PixelFormats;
using SixLabors.ImageSharp.Processing;
using System.Numerics;

namespace MusicStoreShowcase.Services
{
    public class CoverService
    {
        public byte[] GenerateCover(string songId, string title, string artist)
        {
            int seed = SeedHelper.ToStableInt($"cover:{songId}");
            var rng = new Random(seed);

            string[] files = Directory.GetFiles("wwwroot/assets/covers");
            string selectedFile = files[rng.Next(files.Length)];

            using var image = Image.Load<Rgba32>(selectedFile);

            image.Mutate(ctx =>
            {
                ctx.Resize(300, 300);

                ctx.Fill(Color.Black.WithAlpha(0.35f));

                ctx.Draw(
                    Color.White.WithAlpha(0.4f),
                    2,
                    new RectangularPolygon(15, 15, 270, 270)
                );
            });

            Font titleFont = SystemFonts.CreateFont("Arial", 26, FontStyle.Bold);
            Font artistFont = SystemFonts.CreateFont("Arial", 16, FontStyle.Regular);

            image.Mutate(ctx =>
            {
                ctx.DrawText(title, titleFont, Color.White, new PointF(20, 200));
                ctx.DrawText(artist, artistFont, Color.LightGray, new PointF(20, 240));
            });

            using var ms = new MemoryStream();
            image.Save(ms, new PngEncoder());
            return ms.ToArray();
        }
        private static Color RandomColor(Random rng)
        {
            return Color.FromRgb(
                (byte)rng.Next(40, 220),
                (byte)rng.Next(40, 220),
                (byte)rng.Next(40, 220)
            );
        }
    }
}