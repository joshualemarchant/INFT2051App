

using Microsoft.Maui.Controls;
using System.Collections.ObjectModel;
using System.Diagnostics;

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


    private void OnShowAnswerClicked(object sender, EventArgs  e)
    {
        if (sender is Button btn && btn.Parent is StackLayout vstack)
        {
            // Find the Answer StackLayout inside this question item
            var answerStack = vstack.Children.OfType<StackLayout>().FirstOrDefault();
            if (answerStack != null)
            {
                answerStack.IsVisible = !answerStack.IsVisible;
                btn.Text = answerStack.IsVisible ? "Hide Answer" : "Show Answer";
            }

        }
    }

    private async void OnDeleteAnswerClicked(object sender, EventArgs e)
    {
        if (sender is Button btn && btn.BindingContext is UserQuestion question)
        {
                await App.Database.DeleteItemAsync(question);
                await LoadQuestionsAsync();
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

    protected override void OnNavigatedFrom(NavigatedFromEventArgs args)
    {
        base.OnNavigatedFrom(args);
        FilterSearchBar.Text = string.Empty;
    }
}