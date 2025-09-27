using INFT2051App.Services.PartialMethods;

namespace INFT2051App;

public partial class AddQuestionPage : ContentPage
{
    public bool TestingModeOn = false;
    public DateTime TestscheduleTime= DateTime.Now.AddSeconds(10);
	public AddQuestionPage()
	{
		InitializeComponent();
	}

    private async void onSaveClicked(object sender, EventArgs e)
    {

        //TODO: Add error handling for empty text values and change IsDue to 'false' once testing is done

        if (string.IsNullOrWhiteSpace(PromptEntry.Text) && string.IsNullOrWhiteSpace(AnswerEditor.Text))
        {
            await DisplayAlert("Error", "Input fields cannot be blank!!", "OK");
            return;
        }

        // Question Creation
        var question = new UserQuestion
        {
            Prompt = PromptEntry.Text,
            Answer = AnswerEditor.Text,
            CreatedAt = DateTime.UtcNow,
            IsDue = false,
            IsAnswered = false
        };

        // Declare schedule time variable
        DateTime scheduleTime;

        if (TestingModeOn) // if test mode is on, schedule is set to 10 seconds after question creation
        {
            scheduleTime = TestscheduleTime;
            question.IsDue = true;
             
        } else
        {
            scheduleTime = MyDatePicker.Date;

        }

        NotificationService.SendNotification("You have a question ready!", PromptEntry.Text, scheduleTime);

        await App.Database.SaveItemAsync(question);

        await DisplayAlert("Saved", "Question saved successfully!", "OK");

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

   
}