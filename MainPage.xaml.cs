
using System.Collections.ObjectModel;


namespace INFT2051App;

public partial class MainPage : ContentPage
{
    private ObservableCollection<UserQuestion> allQuestions { get; } = new(); // Collection for all questions in Database

    private ObservableCollection<UserQuestion> filteredQuestions = new(); // Collection for filtered questions
    public MainPage()
    {
        InitializeComponent();
        QuestionsCollection.ItemsSource = allQuestions; // Set Collection view item source to allquestions collection   
    }

    protected override async void OnAppearing()
    {
        base.OnAppearing();
        await LoadQuestionsAsync(); // Loads questions when page is visited
      
    }

    // Loading questions from DB 
    private async Task LoadQuestionsAsync()
    {
        var questions = await App.Database.GetItemsAsync<UserQuestion>(); // Get questions from DB
        allQuestions.Clear(); // Clear collection

        if (questions == null || !questions.Any())
        {
            NoQuestionsLabel.IsVisible = true; // Displays hint to user if no questions have been created
        }
        else
        {
            NoQuestionsLabel.IsVisible = false;
            foreach (var q in questions.OrderByDescending(q => q.CreatedAt))
            {
                allQuestions.Add(q); // Adds all questions to observable collection variable in order newest to oldest
                
            }
        }
    }
    // AI Generated method to assign text colour behaviour to a button. Used to alternate between showing and hiding answer in question card
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
            answerLabel.TextColor = Colors.Transparent;
            clickedButton.Text = "Show Answer";
        }
    }
    // Search bar method
    private void OnTextChanged(object sender, TextChangedEventArgs e)
    {
        string searchText = e.NewTextValue?.Trim() ?? string.Empty; // User Input value as variable

        filteredQuestions.Clear(); // Clear any old filtered results in collection

        if (string.IsNullOrWhiteSpace(searchText))
        {
            QuestionsCollection.ItemsSource = allQuestions; // set view source back to default collection if search bar is empty
        }
        else
        {
            // Add questions from default collection to filtered collection based on search bar query
            var matches = allQuestions.Where(q => q.Prompt.Contains(searchText, StringComparison.OrdinalIgnoreCase));

            foreach (var q in matches) 
                filteredQuestions.Add(q); 

            QuestionsCollection.ItemsSource = filteredQuestions; // set view to filtered questions collection to show filtered results to user
        }
    }

    // Navigate to edit question page with question with question ID data
    private async void OnEditQuestionClicked(object sender, EventArgs e)
    {
        if (sender is Button button && button.CommandParameter is UserQuestion question)
        {
            var questionId = question.ID;
            await Shell.Current.GoToAsync($"editquestionpage?QuestionId={questionId}");
        }
    }

    // Clears left over input in search bar if user leaves page
    protected override void OnNavigatedFrom(NavigatedFromEventArgs args)
    {
        base.OnNavigatedFrom(args);
        FilterSearchBar.Text = string.Empty;
    }
}