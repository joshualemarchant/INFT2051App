
namespace INFT2051App.Pages;

[QueryProperty(nameof(QuestionId), "QuestionId")]

// TODO: Add more comments
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

        currentQuestion = await App.Database.GetItemAsync<UserQuestion>(QuestionId);

        NewQuestion.Text = currentQuestion.Prompt;
        NewAnswer.Text = currentQuestion.Answer;
    }

    private async void OnSaveClicked(object sender, EventArgs e)
    {
        currentQuestion.Prompt = NewQuestion.Text;
        currentQuestion.Answer = NewAnswer.Text;

        await App.Database.SaveItemAsync(currentQuestion);

        await DisplayAlert("Saved", "Question updated successfully!", "OK");
        await Shell.Current.GoToAsync(".."); 

    }

    private async void OnDeleteAnswerClicked(object sender, EventArgs e)
    {
        bool answer = await DisplayAlert("Attention", "Are you sure you want to delete this question?", "Yes", "No");

        if (answer)
        {
            await App.Database.DeleteItemAsync(currentQuestion);
            await DisplayAlert("Attention", "Question deleted!", "OK");
            await Shell.Current.GoToAsync("..");
        }
        else
        {
            return;
        }
        
    }


}