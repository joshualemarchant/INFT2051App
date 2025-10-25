

using System.Threading.Tasks;

namespace INFT2051App;

public partial class SettingsPage : ContentPage
{
    
    public SettingsPage()
	{
        InitializeComponent();
        BindingContext = this;
        DisplayPreferences(); // auto-Sets input fields to current preference times
    }
    private void OnSaveClicked(object sender, EventArgs e)
    {
        if (UserEndTime.Time <= UserStartTime.Time)
        {
            DisplayAlert("Attention", "Invalid prompting hours selected", "Ok"); // Error handling
            return;
        }
        else
        {
            // Set variables with input Time picker values and parse to string to have them stored in preferences
            string start = UserStartTime.Time.ToString();
            string end = UserEndTime.Time.ToString();
           
            // Store in preferences
            Preferences.Default.Set("HourStart", start);
            Preferences.Default.Set("HourEnd", end);

            DisplayAlert("Attention",
                $"You will be prompted between the hours of {start} and {end}",
                "ok");
        }
    }

    private void RecurringQuestionsSwitchToggled(object sender, ToggledEventArgs e)
    {
        if (e.Value) 
        {                     
            OffOnLabel.Text = "On";
            Preferences.Default.Set("RecurringQuestionsIsOn", true);           
        }
        else
        {
            OffOnLabel.Text = "Off";
            Preferences.Default.Set("RecurringQuestionsIsOn", false);          
        }
    }

    private void DisplayPreferences()
    {
        UserStartTime.Time = Utilities.ConvertToTimeSpan("HourStart", "9:00");
        UserEndTime.Time = Utilities.ConvertToTimeSpan("HourEnd", "17:00");
        ToggleSwitch.IsToggled = Preferences.Default.Get("RecurringQuestionsIsOn", false);
    }

    private async void OnDeleteQuestionsClicked(object sender, EventArgs e)
    {
        bool answer = await DisplayAlert("Warning!", "You are about to delete every question you have created!. " +
            "Are you sure you want to do this?", "Yes", "No");
        if (answer)
        {
            var questions = await App.Database.GetItemsAsync<UserQuestion>();
            foreach (var q in questions)
            {
                await Utilities.ClearNotification(q.ID);
                await App.Database.DeleteItemAsync(q);
            }
            
        } 
        else
        {
            return;
        }
    }
}