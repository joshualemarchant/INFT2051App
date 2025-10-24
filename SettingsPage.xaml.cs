

namespace INFT2051App;

public partial class SettingsPage : ContentPage
{  
    public SettingsPage()
	{
        InitializeComponent();
        BindingContext = this;
        DisplayPreferences(); // auto-Sets input fields to current preference times
    }
    //TODO: Add option for 24/7 prompting
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
        if (e.Value) // Hides data selection box if on
        {                     
            OffOnLabel.Text = "On";
            
        }
        else
        {           
            OffOnLabel.Text = "Off";
        }
    }

    private void DisplayPreferences()
    {
        UserStartTime.Time = Utilities.ConvertToTimeSpan("HourStart", "9:00");
        UserEndTime.Time = Utilities.ConvertToTimeSpan("HourEnd", "17:00");
    }
}