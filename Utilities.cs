

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
    }
}
