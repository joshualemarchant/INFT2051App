using INFT2051App.Pages;
using System.Collections.ObjectModel;

namespace INFT2051App;
public partial class NotificationsPage : ContentPage
{
    private ObservableCollection<UserQuestion> DueQuestions { get; } = new(); // Collection to store questions that are due
    public NotificationsPage()
    {
        InitializeComponent();
        DueQuestionsCollection.ItemsSource = DueQuestions; // Set collection view to due questions
    }
    protected override async void OnAppearing()
    {
        base.OnAppearing();
        await LoadDueQuestionsAsync(); // Load questions on page visit

    }

    // Load questions method
    private async Task LoadDueQuestionsAsync()
    {       
        var questions = await App.Database.GetItemsAsync<UserQuestion>(); // Get all questions from database

        var dueQuestions = questions.Where(q => q.IsDue).OrderByDescending(q => q.CreatedAt); // Create variable to store due questions in descending order

        // Clear collection to remove old notifications and add due questions
        DueQuestions.Clear(); 
        foreach (var q in dueQuestions)
            DueQuestions.Add(q); 
    }

    // Navigate to answer page method
    private async void OnGoToAnswerQuestionPageClicked(object sender, EventArgs e)
    {
        if (sender is Button button && button.CommandParameter is UserQuestion question)
        {           
            var questionId = question.ID;           
            await Shell.Current.GoToAsync($"answerpage?QuestionId={questionId}"); // pass Question ID in argument to render question object on corresponding answer page
        }
    }

    // Refresh method
    private async void OnRefresh(object sender, EventArgs e)
    {
        await LoadDueQuestionsAsync(); // reload due questions
        Refresh.IsRefreshing = false; // Stop spinner
    }
}
    
