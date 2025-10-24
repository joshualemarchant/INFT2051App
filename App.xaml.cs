
using INFT2051App.Pages;
using Microsoft.Maui.Graphics.Text;
using Plugin.LocalNotification;
using Plugin.LocalNotification.EventArgs;

namespace INFT2051App
{
    public partial class App : Application
    {
        private static AppDatabase? database;

        // Create new database if none
        public static AppDatabase Database
        {
            get
            {
                if (database == null)
                    database = new AppDatabase();
                return database;
            }
        }
        public App()
        {
            InitializeComponent();

            // Code to subscribe from notification events from Plugin documentation at: https://github.com/thudugala/Plugin.LocalNotification/wiki/1.-Usage-10.0.0--.Net-MAUI 
            LocalNotificationCenter.Current.NotificationReceived += OnNotificationReceived;
            LocalNotificationCenter.Current.NotificationActionTapped += OnNotificationTapped;
        }

        protected override Window CreateWindow(IActivationState? activationState)
        {

            return new Window(new AppShell());

        }
        
        // Notification fired / received behaviour (updates question IsDue status to 'true')
        private async void OnNotificationReceived(NotificationEventArgs e)
        {
            int questionId = int.Parse(e.Request.ReturningData); // question ID string converted to Int
            var question = await Database.GetItemAsync<UserQuestion>(questionId); // gets question in DB via ID
            if (question != null)
            {
                question.IsDue = true; // updates is Due field to true
                await Database.SaveItemAsync(question); // saves question with updated IsDue Field
            }
            
        }

        // Notification tapped behaviour (takes user directly to answer question page)
        private async void OnNotificationTapped(NotificationActionEventArgs e)
        {
            if (e.IsDismissed)
            {
                return; // return if dismissed
            } 
            if (e.IsTapped)
            {
                await Task.Delay(500); // allow app shell to build if launching app from notification tap to avoid crash
                int questionId = int.Parse(e.Request.ReturningData); // question ID string converted to Int                                                                 
                await Shell.Current.GoToAsync($"answerpage?QuestionId={questionId}"); // Navigates to corresponding answer question page with fetched ID            
            }
        }
    }
}