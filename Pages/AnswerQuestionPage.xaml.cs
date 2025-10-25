
using Plugin.LocalNotification;

namespace INFT2051App.Pages;
[QueryProperty(nameof(QuestionId), "QuestionId")]


public partial class AnswerQuestionPage : ContentPage
{
    public int QuestionId { get; set; }
    public UserQuestion Question { get; set; }
    public string UserAnswer { get; set; }

    public bool RecurringQuestionsIsOn; // If true, answered questions will be automatically rescheduled
    public AnswerQuestionPage()
    {
        InitializeComponent();
        BindingContext = this;
    }

    protected override async void OnAppearing()
    {
        base.OnAppearing();

        Question = await App.Database.GetItemAsync<UserQuestion>(QuestionId); // Get question from DB

        // Error handling for edge case in which question was deleted after notification has fired
        if (Question == null)
        {            
            await DisplayAlert("Error", "This question no longer exists", "Ok");
            await Navigation.PopAsync();
        }
        else
        {
            OnPropertyChanged(nameof(Question)); // Set local variable to question fetched from DB
            RecurringQuestionsIsOn = Preferences.Default.Get("RecurringQuestionsIsOn", false); // Set Recurring questions value
        }
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
            await DisplayAlert("Result", "Correct!", "Ok");
            Question.IsAnswered = true;
            Question.IsDue = false;
            await App.Database.SaveItemAsync(Question);
        }
        else
        {
            await DisplayAlert("Result", "Incorrect!", "Try Again");
        }

        // Recurring question scheduling
        if (Question.IsAnswered)
        {
            if(RecurringQuestionsIsOn)
            {               
                var scheduledNotification = new NotificationRequest
                {
                    NotificationId = Question.ID,
                    Title = "You have a question ready!",
                    Description = Question.Prompt,
                    Schedule =
                {
                    NotifyTime = DateTime.Now.AddSeconds(15) // Defaults to 15 seconds for app prototype purposes
                },
                    ReturningData = Question.ID.ToString() // Parse ID to string so it can be stored in notification returning data
                };

                await LocalNotificationCenter.Current.Show(scheduledNotification);
                await Navigation.PopAsync();
            }
            else
            {
                await App.Database.DeleteItemAsync(Question);
                await Navigation.PopAsync();
            }
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