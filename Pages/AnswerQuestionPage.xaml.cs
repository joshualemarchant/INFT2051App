namespace INFT2051App.Pages;
[QueryProperty(nameof(QuestionId), "QuestionId")]


public partial class AnswerQuestionPage : ContentPage
{
    public int QuestionId { get; set; }
    public UserQuestion Question { get; set; }
    public string UserAnswer { get; set; }

    public AnswerQuestionPage()
    {
        InitializeComponent();
        BindingContext = this;
    }

    protected override async void OnAppearing()
    {
        base.OnAppearing();
        Question = await App.Database.GetItemAsync<UserQuestion>(QuestionId); // Get question from DB
        OnPropertyChanged(nameof(Question)); // Set local variable to question fetched from DB
    }

    private async void OnEnterClicked(object sender, EventArgs e)
    
    {
        // Check if field isnt empty
        if (string.IsNullOrWhiteSpace(UserAnswer))
        {
            await DisplayAlert("Error", "Please enter an answer before submitting.", "OK");
            return;
        }

        // Check if answer is corect
        if (Question.Answer.ToLower().Trim() == UserAnswer.ToLower().Trim())
        {
            Question.IsAnswered = true;
            await DisplayAlert("Result", "Correct!", "Ok");
        }
        else
        {
            await DisplayAlert("Result", "Incorrect!", "Try Again");
        }

        // Delete question if correct
        if (Question.IsAnswered)
        {
            await App.Database.DeleteItemAsync(Question);
            await Navigation.PopAsync();
        }
    }

    // Method for dynamic Enter button colour
    public void UpdateEntryButtonColour(object sender, TextChangedEventArgs e)
    {
        if (string.IsNullOrWhiteSpace(AnswerEditor.Text))
        {
            EnterButton.Background = (Color)Application.Current.Resources["Gray500"];
        }
        else
        {
            EnterButton.Background = (Color)Application.Current.Resources["Secondary"];
        }

    }
}