using Plugin.LocalNotification;

namespace INFT2051App;

public partial class AddQuestionPage : ContentPage
{
    public bool TestingModeOn = false;
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
       
        if (TestingModeOn)
        {
            question.IsDue = true;
        }

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