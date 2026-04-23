using System.Security.Cryptography;
using System.Text;

namespace MusicStoreShowcase.Services
{
    public static class SeedHelper
    {
        public static int ToStableInt(string input)
        {
            var bytes = SHA256.HashData(Encoding.UTF8.GetBytes(input));
            return BitConverter.ToInt32(bytes, 0) & int.MaxValue;
        }

        public static string MakeCoreKey(string region, long seed, int page, int index)
        {
            return $"core:{region}:{seed}:{page}:{index}";
        }

        public static string MakeLikesKey(string region, long seed, int page, int index)
        {
            return $"likes:{region}:{seed}:{page}:{index}";
        }
    }
}