namespace INFT2051App;

public partial class AddQuestionPage : ContentPage
{
	public AddQuestionPage()
	{
		InitializeComponent();
	}

	private async void onSaveClicked(object sender, EventArgs e)
	{
		var question = new UserQuestion
		{
			Prompt = PromptEntry.Text,
			Answer = AnswerEditor.Text,
			CreatedAt = DateTime.UtcNow,
			IsDue = true,
			IsAnswered = false
		};
        await App.Database.SaveItemAsync(question);

        await DisplayAlert("Saved", "Question saved successfully!", "OK");

        // Clear input
        PromptEntry.Text = string.Empty;
        AnswerEditor.Text = string.Empty;
    }
}