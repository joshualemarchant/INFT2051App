
using INFT2051App.Pages;
using Plugin.LocalNotification;
using Plugin.LocalNotification.EventArgs;

namespace INFT2051App
{
    public partial class App : Application
    {
        private static AppDatabase? database;
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
            if (int.TryParse(e.Request.ReturningData, out int questionId)) // question ID string converted to Int
            {
                var question = await Database.GetItemAsync<UserQuestion>(questionId); // gets question in DB via ID
                if (question != null)
                {
                    question.IsDue = true; // updates
                    await Database.SaveItemAsync(question); // saves 
                }
            }
        }
        
        // Notification tapped behaviour (takes user directly to answer question page)
        private async void OnNotificationTapped(NotificationActionEventArgs e)
        {
            if (int.TryParse(e.Request.ReturningData, out int questionId))
            {
                var question = await Database.GetItemAsync<UserQuestion>(questionId);
                if (question != null)
                {
                    await Current.MainPage.Navigation.PushAsync(new AnswerQuestionPage(question));
                }
            }
        }

    }
}