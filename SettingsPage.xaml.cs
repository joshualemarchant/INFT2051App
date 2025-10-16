namespace INFT2051App;

public partial class SettingsPage : ContentPage
{

   
    public SettingsPage()
	{
        InitializeComponent();
        BindingContext = this;
        DisplayPreferences();
    }
    //TODO: Add option for 24/7 prompting
    private void OnSaveClicked(object sender, EventArgs e)
    {
        if (UserEndTime.Time <= UserStartTime.Time)
        {
            DisplayAlert("Attention", "Invalid prompting hours selected", "Ok");
            return;
        }
        else
        {
            
            string start = UserStartTime.Time.ToString();
            string end = UserEndTime.Time.ToString();

            Preferences.Default.Set("HourStart", start);
            Preferences.Default.Set("HourEnd", end);

            DisplayAlert("Attention", 
                $"You will be prompted between the hours of {UserStartTime.Time} and {UserEndTime.Time}", 
                "ok");
        }
    }

    private void DisplayPreferences()
    {
        UserStartTime.Time = Utilities.ConvertToTimeSpan("HourStart", "9:00");
        UserEndTime.Time = Utilities.ConvertToTimeSpan("HourEnd", "17:00");
    }

}