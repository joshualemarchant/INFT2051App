namespace INFT2051App.Pages;
[QueryProperty(nameof(QuestionId), "QuestionId")]

// TODO: Add more comments
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
        Question = await App.Database.GetItemAsync<UserQuestion>(QuestionId);
        OnPropertyChanged(nameof(Question));
    }

    private async void OnEnterClicked(object sender, EventArgs e)
    
    {
        if (string.IsNullOrWhiteSpace(UserAnswer))
        {
            await DisplayAlert("Error", "Please enter an answer before submitting.", "OK");
            return;
        }

        if (Question.Answer.ToLower().Trim() == UserAnswer.ToLower().Trim())
        {
            Question.IsAnswered = true;
            await DisplayAlert("Result", "Correct!", "Ok");
        }
        else
        {
            await DisplayAlert("Result", "Incorrect!", "Try Again");
        }

        if (Question.IsAnswered)
        {
            await App.Database.DeleteItemAsync(Question);
            await Navigation.PopAsync();
        }
    }

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