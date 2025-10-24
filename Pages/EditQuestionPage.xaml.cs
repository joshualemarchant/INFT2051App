
using Plugin.LocalNotification;

namespace INFT2051App.Pages;

[QueryProperty(nameof(QuestionId), "QuestionId")]
public partial class EditQuestionPage : ContentPage
{
    public int QuestionId { get; set; }
    private UserQuestion currentQuestion;

    public EditQuestionPage()
	{
		InitializeComponent();
        BindingContext = this;
    }
    protected override async void OnAppearing()
    {
        base.OnAppearing();

        // Get question from DB
        currentQuestion = await App.Database.GetItemAsync<UserQuestion>(QuestionId); 

        // Populate fields with current Question and Answer
        NewQuestion.Text = currentQuestion.Prompt;
        NewAnswer.Text = currentQuestion.Answer;
    }

    // Save new question details method
    private async void OnSaveClicked(object sender, EventArgs e)
    {
        // Check if input fields are filled
        if (string.IsNullOrWhiteSpace(NewQuestion.Text) || string.IsNullOrWhiteSpace(NewAnswer.Text))
        {
            await DisplayAlert("Attention", "Fields cannot be empty!", "Ok");
            return;
        } 
        else
        {
            // Save new question details
            currentQuestion.Prompt = NewQuestion.Text;
            currentQuestion.Answer = NewAnswer.Text;

            await App.Database.SaveItemAsync(currentQuestion);

            await DisplayAlert("Saved", "Question updated successfully!", "OK");
            await Shell.Current.GoToAsync(".."); 

        }
    }

    // Delete question method
    private async void OnDeleteAnswerClicked(object sender, EventArgs e)
    {
        bool answer = await DisplayAlert("Attention", "Are you sure you want to delete this question?", "Yes", "No");
        
        if (answer)
        {
            await Utilities.ClearNotification(currentQuestion.ID); // Clears pending notification for question thats being deleted

            await App.Database.DeleteItemAsync(currentQuestion); // Delete question from db

            await DisplayAlert("Attention", "Question deleted!", "OK");

            await Shell.Current.GoToAsync(".."); // Navigate back
        }
        else
        {
            return;
        }
        
    }
   
}