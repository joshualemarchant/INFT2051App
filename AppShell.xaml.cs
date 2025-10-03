using INFT2051App.Pages;

namespace INFT2051App
{
    public partial class AppShell : Shell
    {
        public AppShell()
        {
            InitializeComponent();
            Routing.RegisterRoute("answerpage", typeof(AnswerQuestionPage));

        }
    }
}
