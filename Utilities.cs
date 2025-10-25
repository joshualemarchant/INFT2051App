using Plugin.LocalNotification;

namespace INFT2051App
{
    // I put these methods in a Utilities class to keep some files a bit cleaner
    internal class Utilities
    {
        // This method is used to get the prompting hour values stored in preferences as a string and return them as TimeSpan values
        public static TimeSpan ConvertToTimeSpan(string key, string defaultValue)
        {
            string x = Preferences.Default.Get(key, defaultValue);

            TimeSpan y = TimeSpan.Parse(x);

            return y;
        }
    
        // AI generated method that returns a random TimeSpan value
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

        // Clear pending notification method 
        public static async Task ClearNotification(int questionID)
        {
            // Get all pending notifications
            var pending = await LocalNotificationCenter.Current.GetPendingNotificationList();

            // Clear notification by ID
            foreach (var notification in pending)
            {
                if (notification.NotificationId == questionID)
                {
                    LocalNotificationCenter.Current.Cancel(notification.NotificationId);
                }
            }
        }
    }
}
