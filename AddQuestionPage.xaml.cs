using Plugin.LocalNotification;




namespace INFT2051App;

public partial class AddQuestionPage : ContentPage
{
    public bool TestingModeOn = false;
    public TimeSpan TimeStart = Utilities.ConvertToTimeSpan("HourStart", "5:00");
    public TimeSpan TimeEnd = Utilities.ConvertToTimeSpan("HourEnd", "17:00");

    
    public DateTime CurrentDateTime { get; set; } = DateTime.Now;
    

    public AddQuestionPage()
	{
		InitializeComponent();
        BindingContext = this;
        AskPermissions();
	}


    private async Task<PermissionStatus> AskPermissions()
    {
        PermissionStatus status = await Permissions.RequestAsync<Permissions.PostNotifications>();

        if (status == PermissionStatus.Granted)
            return status;

        if (status == PermissionStatus.Denied && DeviceInfo.Platform == DevicePlatform.Android)
        {
            await DisplayAlert(
                "Enable Notifications",
                "EverFloat needs notifications to be activated in order to prompt you. Please turn them on in settings.",
                "OK");
        }
        return status;
    }

    private async void onSaveClicked(object sender, EventArgs e)
    {
        if (string.IsNullOrWhiteSpace(PromptEntry.Text) || string.IsNullOrWhiteSpace(AnswerEditor.Text))
        {
            await DisplayAlert("Error", "Input fields cannot be blank!!", "OK");
            return;
        }

        // Question Creation
        var question = new UserQuestion
        {
            Prompt = PromptEntry.Text,
            Answer = AnswerEditor.Text,
            CreatedAt = DateTime.Now,
            IsDue = false,
            IsAnswered = false
        };

        // Save question
        await App.Database.SaveItemAsync(question);

        await DisplayAlert("Saved", "Question saved successfully!", "OK");

        

        // Schedule Notification
        var scheduledNotification = new NotificationRequest
        {
            NotificationId = question.ID,
            Title = "You have a question ready!",
            Description = PromptEntry.Text,
            Schedule =
            {
                NotifyTime = SetSchedule()
            },
            ReturningData = question.ID.ToString()
        };


        await LocalNotificationCenter.Current.Show(scheduledNotification);

        Console.WriteLine($"Notification scheduled:");
        Console.WriteLine($"ID: {scheduledNotification.NotificationId}");
        Console.WriteLine($"Title: {scheduledNotification.Title}");
        Console.WriteLine($"Description: {scheduledNotification.Description}");
        Console.WriteLine($"NotifyTime: {scheduledNotification.Schedule.NotifyTime}");
        Console.WriteLine($"ReturningData: {scheduledNotification.ReturningData}");

        // Clear input
        PromptEntry.Text = string.Empty;
        AnswerEditor.Text = string.Empty;
    }

    private void TestSwitchToggled (object sender, ToggledEventArgs e)
    {
        if (e.Value) // Switch is ON
        {
            TestingModeOn = true;
            DateSelectionBox.IsVisible = false;
            OffOnLabel.Text = "On";

        }
        else // Switch is OFF
        {
            TestingModeOn = false;
            DateSelectionBox.IsVisible = true;
            OffOnLabel.Text = "Off";

        }
    }
    private DateTime SetSchedule()
    {
        TimeSpan randomTime = Utilities.GetRandomTime(TimeStart, TimeEnd);
        

        if (TestingModeOn) // if test mode is on, schedule is set to 10 seconds after question creation
            return DateTime.Now.AddSeconds(10);
        else
        {
            return MyDatePicker.Date + randomTime;
        }
    }
}

   
