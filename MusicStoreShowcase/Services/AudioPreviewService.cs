using System.Text;

namespace MusicStoreShowcase.Services
{
    public class AudioPreviewService
    {
        public byte[] GeneratePreview(string songId)
        {
            int seed = SeedHelper.ToStableInt($"audio:{songId}");
            var rng = new Random(seed);

            const int sampleRate = 22050;
            const short bitsPerSample = 16;
            const short channels = 1;
            const int durationSeconds = 6;

            int totalSamples = sampleRate * durationSeconds;
            short[] samples = new short[totalSamples];

            double[] scale = new double[]
            {
                261.63, // C4
                293.66, // D4
                329.63, // E4
                349.23, // F4
                392.00, // G4
                440.00, // A4
                493.88  // B4
            };

            int notesCount = 12;
            int samplesPerNote = totalSamples / notesCount;

            for (int noteIndex = 0; noteIndex < notesCount; noteIndex++)
            {
                double freq = scale[rng.Next(scale.Length)];

                for (int i = 0; i < samplesPerNote; i++)
                {
                    int sampleIndex = noteIndex * samplesPerNote + i;
                    if (sampleIndex >= totalSamples) break;

                    double t = (double)sampleIndex / sampleRate;
                    double localT = (double)i / samplesPerNote;

                    double envelope = Math.Exp(-3 * localT);
                    double value =
                        0.6 * Math.Sin(2 * Math.PI * freq * t) +
                        0.2 * Math.Sin(2 * Math.PI * freq * 2 * t);

                    value *= envelope;

                    samples[sampleIndex] = (short)(value * short.MaxValue * 0.5);
                }
            }

            return BuildWav(samples, sampleRate, bitsPerSample, channels);
        }

        private byte[] BuildWav(short[] samples, int sampleRate, short bitsPerSample, short channels)
        {
            using var stream = new MemoryStream();
            using var writer = new BinaryWriter(stream, Encoding.UTF8, true);

            int subChunk2Size = samples.Length * sizeof(short);
            int chunkSize = 36 + subChunk2Size;
            short blockAlign = (short)(channels * bitsPerSample / 8);
            int byteRate = sampleRate * blockAlign;

            writer.Write(Encoding.ASCII.GetBytes("RIFF"));
            writer.Write(chunkSize);
            writer.Write(Encoding.ASCII.GetBytes("WAVE"));

            writer.Write(Encoding.ASCII.GetBytes("fmt "));
            writer.Write(16);
            writer.Write((short)1); // PCM
            writer.Write(channels);
            writer.Write(sampleRate);
            writer.Write(byteRate);
            writer.Write(blockAlign);
            writer.Write(bitsPerSample);

            writer.Write(Encoding.ASCII.GetBytes("data"));
            writer.Write(subChunk2Size);

            foreach (var sample in samples)
            {
                writer.Write(sample);
            }

            writer.Flush();
            return stream.ToArray();
        }
    }
}