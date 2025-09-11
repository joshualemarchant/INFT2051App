using INFT2051App.Pages;
using System.Collections.ObjectModel;

namespace INFT2051App;

public partial class NotificationsPage : ContentPage
{
    private ObservableCollection<UserQuestion> DueQuestions { get; } = new();
    public NotificationsPage()
	{
		InitializeComponent();
        DueQuestionsCollection.ItemsSource = DueQuestions;
	}
    protected override async void OnAppearing()
    {
        base.OnAppearing();
        await LoadDueQuestionsAsync();

    }

    private async Task LoadDueQuestionsAsync()
    {
        // Get all questions from database
        var questions = await App.Database.GetItemsAsync<UserQuestion>();

        // Filter: only those where IsDue == true
        var dueQuestions = questions
            .Where(q => q.IsDue)
            .OrderByDescending(q => q.CreatedAt);

        // Refresh collection
        DueQuestions.Clear();
        foreach (var q in dueQuestions)
            DueQuestions.Add(q);
    }

    private async void OnGoToAnswerQuestionPageClicked(object sender, EventArgs e)
    {
        if (sender is Button button && button.CommandParameter is UserQuestion question)
        {
            await Navigation.PushAsync(new AnswerQuestionPage(question));
        }
    }
}