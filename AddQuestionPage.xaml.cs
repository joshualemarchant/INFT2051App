using Plugin.LocalNotification;

namespace INFT2051App;

public partial class AddQuestionPage : ContentPage
{
    // Testing mode automatically schedules Question prompt/notification prompt to be fired 10 seconds after it was created
    public bool TestingModeOn = false;

    // Variables for user's question prompting hours that are stored in preferences.
    public TimeSpan TimeStart;
    public TimeSpan TimeEnd;
    public DateTime CurrentDateTime { get; set; } = DateTime.Now; // I used this to set the minimum date in DatePicker component 
    public AddQuestionPage()
	{
		InitializeComponent();
        BindingContext = this;
        AskPermissions(); // requests notification permissions from user
	}

    // Sets variables to preference value. Called on every page visit in case of preferences being updated
    protected override void OnAppearing()
    {
        base.OnAppearing();
        TimeStart = Utilities.ConvertToTimeSpan("HourStart", "5:00");
        TimeEnd = Utilities.ConvertToTimeSpan("HourEnd", "17:00");      
    }

    private async Task<PermissionStatus> AskPermissions() // Method for requesting notification permission
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

    // Save button behaviour method
    private async void onSaveClicked(object sender, EventArgs e)
    {
        // Error handling for invalid input
        if (string.IsNullOrWhiteSpace(PromptEntry.Text) || string.IsNullOrWhiteSpace(AnswerEditor.Text)) 
        {
            await DisplayAlert("Error", "Input fields cannot be blank!!", "OK");
            return;
        }

        // Question object creation 
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
                NotifyTime = SetSchedule() // Method handles scheduling logic
            },
            ReturningData = question.ID.ToString() 
        };

        await LocalNotificationCenter.Current.Show(scheduledNotification);

        // I put this here for testing scheduled notifications 

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

    // Test switch behaviour
    private void TestSwitchToggled (object sender, ToggledEventArgs e)
    {
        if (e.Value) // Hides data selection box if on
        {
            TestingModeOn = true;
            DateSelectionBox.IsVisible = false;
            OffOnLabel.Text = "On";
        }
        else 
        {
            TestingModeOn = false;
            DateSelectionBox.IsVisible = true;
            OffOnLabel.Text = "Off";
        }
    }

    // Scheduling method
    private DateTime SetSchedule()
    {
        TimeSpan randomTime = Utilities.GetRandomTime(TimeStart, TimeEnd); // Get a random TimeSpan value in between user preference times
       
        if (TestingModeOn) // if test mode is on, schedule is set to 10 seconds after question creation
            return DateTime.Now.AddSeconds(10);
        else
        {
            return MyDatePicker.Date + randomTime; // Return selected date from date picker and random time
        }
    }
}

   
