namespace MusicStoreShowcase.Services
{
    public static class LikesHelper
    {
        public static int GenerateLikes(double averageLikes, int stableSeed)
        {
            var integerPart = (int)Math.Floor(averageLikes);
            var fractionalPart = averageLikes - integerPart;

            var rng = new Random(stableSeed);
            var extraLike = rng.NextDouble() < fractionalPart ? 1 : 0;

            return integerPart + extraLike;
        }
    }
}