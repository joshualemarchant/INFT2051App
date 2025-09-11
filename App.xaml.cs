namespace INFT2051App
{
    public partial class App : Application
    {
        private static AppDatabase? database;
        public static AppDatabase Database
        {
            get
            {
                if (database == null)
                    database = new AppDatabase();
                return database;
            }
        }
        public App()
        {
            InitializeComponent();

        }

        protected override Window CreateWindow(IActivationState? activationState)
        {
            return new Window(new AppShell());
        }
    }
}