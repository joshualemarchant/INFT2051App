using INFT2051App.Pages;

namespace INFT2051App
{
    public partial class AppShell : Shell
    {
        public AppShell()
        {
            InitializeComponent();

            // Route registration for shell navigation methods
            Routing.RegisterRoute("answerpage", typeof(AnswerQuestionPage));
            Routing.RegisterRoute("editquestionpage", typeof(EditQuestionPage));

        }
    }
}
