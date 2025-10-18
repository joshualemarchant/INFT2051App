

namespace INFT2051App
{
    internal class Utilities
    {
        public static TimeSpan ConvertToTimeSpan(string key, string defaultValue)
        {
            string x = Preferences.Default.Get(key, defaultValue);

            TimeSpan y = TimeSpan.Parse(x);

            return y;
        }
    
     public static TimeSpan GetRandomTime(TimeSpan start, TimeSpan end)
        {
            if (end <= start)
                throw new ArgumentException("End time must be after start time");

            // Total seconds between start and end
            double totalSeconds = (end - start).TotalSeconds;

            // Generate a random offset in seconds
            Random random = new Random();
            double randomSeconds = random.NextDouble() * totalSeconds;

            // Return start + offset
            return start + TimeSpan.FromSeconds(randomSeconds);
        }
    }
}
