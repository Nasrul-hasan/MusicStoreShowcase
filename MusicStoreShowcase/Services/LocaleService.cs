using System.Text.Json;
using MusicStoreShowcase.Models;

namespace MusicStoreShowcase.Services
{
    public class LocaleService
    {
        private readonly IWebHostEnvironment _environment;

        public LocaleService(IWebHostEnvironment environment)
        {
            _environment = environment;
        }

        public LocaleData GetLocaleData(string region)
        {
            var fileName = region switch
            {
                "de-DE" => "de-DE.json",
                _ => "en-US.json"
            };

            var filePath = Path.Combine(_environment.ContentRootPath, "Resources", "Locales", fileName);

            if (!System.IO.File.Exists(filePath))
            {
                throw new FileNotFoundException($"Locale file not found: {filePath}");
            }

            var json = System.IO.File.ReadAllText(filePath);
            var data = JsonSerializer.Deserialize<LocaleData>(json, new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            });

            return data ?? new LocaleData();
        }
    }
}