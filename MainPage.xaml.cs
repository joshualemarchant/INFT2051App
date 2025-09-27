using INFT2051App.Services.PartialMethods;
using Microsoft.Maui.Controls;
using System.Collections.ObjectModel;
using System.Diagnostics;

namespace INFT2051App;

public partial class MainPage : ContentPage
{
    // Observable collection for UI binding
    private ObservableCollection<UserQuestion> Questions { get; } = new();
    public MainPage()
    {
        InitializeComponent();
        QuestionsCollection.ItemsSource = Questions;
    }

    protected override async void OnAppearing()
    {
        base.OnAppearing();
        await LoadQuestionsAsync();
      
    }

    private async Task LoadQuestionsAsync()
    {
        var questions = await App.Database.GetItemsAsync<UserQuestion>();
        Questions.Clear();

        if (questions == null || !questions.Any())
        {
            NoQuestionsLabel.IsVisible = true;
        }
        else
        {
            NoQuestionsLabel.IsVisible = false;
            foreach (var q in questions.OrderByDescending(q => q.CreatedAt))
                Questions.Add(q);
        }
    }


    private void OnShowAnswerClicked(object sender, EventArgs  e)
    {
        if (sender is Button btn && btn.Parent is VerticalStackLayout vstack)
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
    

    
}