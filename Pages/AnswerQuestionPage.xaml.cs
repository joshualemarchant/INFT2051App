namespace INFT2051App.Pages;
[QueryProperty(nameof(QuestionId), "QuestionId")]
public partial class AnswerQuestionPage : ContentPage
{
    private int _questionId;
    public int QuestionId
    {
        get => _questionId;
        set
        {
            _questionId = value;
            LoadQuestion(value); // fetch from DB
        }
    }

    public UserQuestion Question { get; set; }
    public string UserAnswer { get; set; }

    public AnswerQuestionPage()
    {
        InitializeComponent();
        BindingContext = this;
    }

    private async void LoadQuestion(int id)
    {
        Question = await App.Database.GetItemAsync<UserQuestion>(id);
        OnPropertyChanged(nameof(Question));
    }

    private async void OnEnterClicked(object sender, EventArgs e)
        //TODO more error handling
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
}