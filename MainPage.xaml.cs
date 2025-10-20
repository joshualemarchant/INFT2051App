

using Microsoft.Maui.Controls;
using Microsoft.Maui.Controls.Internals;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Net.Security;

namespace INFT2051App;

public partial class MainPage : ContentPage
{
    // Observable collection for UI binding
    private ObservableCollection<UserQuestion> allQuestions { get; } = new();

    private ObservableCollection<UserQuestion> filteredQuestions = new();
    public MainPage()
    {
        InitializeComponent();
        QuestionsCollection.ItemsSource = allQuestions;
        
    }

    protected override async void OnAppearing()
    {
        base.OnAppearing();
        await LoadQuestionsAsync();
      
    }

    private async Task LoadQuestionsAsync()
    {
        var questions = await App.Database.GetItemsAsync<UserQuestion>();
        allQuestions.Clear();

        if (questions == null || !questions.Any())
        {
            NoQuestionsLabel.IsVisible = true;
        }
        else
        {
            NoQuestionsLabel.IsVisible = false;
            foreach (var q in questions.OrderByDescending(q => q.CreatedAt))
            {
                allQuestions.Add(q);
                
            }
        }
    }
    private void OnShowAnswerClicked(object sender, EventArgs e)
    {
        
        var clickedButton = sender as Button;
        if (clickedButton == null) return;

        
        if (clickedButton.Parent is not HorizontalStackLayout horizontalStack) return;
        if (horizontalStack.Parent is not VerticalStackLayout verticalStack) return;

       
        if (verticalStack.Children.Count <= 1 || verticalStack.Children[1] is not Layout answerContainer) return;

        
        if (answerContainer.Children.Count <= 0 || answerContainer.Children[0] is not Label answerLabel) return;

       
        if (answerLabel.TextColor.ToHex().Equals(Colors.Transparent.ToHex(), StringComparison.OrdinalIgnoreCase))
        {
            
            answerLabel.TextColor = Colors.White;
            clickedButton.Text = "Hide Answer";
        }
        else
        {
            // Currently visible (White), so hide it.
            answerLabel.TextColor = Colors.Transparent;
            clickedButton.Text = "Show Answer";
        }
    }
    private void OnTextChanged(object sender, TextChangedEventArgs e)
    {
        string searchText = e.NewTextValue?.Trim() ?? string.Empty;

        filteredQuestions.Clear();

        if (string.IsNullOrWhiteSpace(searchText))
        {
            QuestionsCollection.ItemsSource = allQuestions;
        }
        else
        {
            var matches = allQuestions
             .Where(q => q.Prompt.Contains(searchText, StringComparison.OrdinalIgnoreCase));

            foreach (var q in matches)
                filteredQuestions.Add(q);

            QuestionsCollection.ItemsSource = filteredQuestions;
        }
    }

    private async void OnEditQuestionClicked(object sender, EventArgs e)
    {
        if (sender is Button button && button.CommandParameter is UserQuestion question)
        {

            var questionId = question.ID;


            await Shell.Current.GoToAsync($"editquestionpage?QuestionId={questionId}");
        }
    }

    protected override void OnNavigatedFrom(NavigatedFromEventArgs args)
    {
        base.OnNavigatedFrom(args);
        FilterSearchBar.Text = string.Empty;
    }
}