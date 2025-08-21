using Microsoft.Maui.Controls;

namespace INFT2051App;

public partial class MainPage : ContentPage
{
    public MainPage()
    {
        InitializeComponent();
    }

    private void OnQuestionClicked(object sender, EventArgs e)
    {
        if (sender is Button button)
        {
            // Get the parent StackLayout that contains both button and answer
            if (button.Parent is StackLayout parentStack)
            {
                // Find the answer StackLayout (should be the second child)
                var answerStack = parentStack.Children.OfType<StackLayout>().FirstOrDefault(x => x != button.Parent);

                if (answerStack != null)
                {
                    // Toggle visibility
                    answerStack.IsVisible = !answerStack.IsVisible;

                    // Update button text to show expand/collapse state
                    if (answerStack.IsVisible)
                    {
                        button.Text = button.Text.Replace("▶", "▼");
                    }
                    else
                    {
                        button.Text = button.Text.Replace("▼", "▶");
                    }
                }
            }
        }
    }
}