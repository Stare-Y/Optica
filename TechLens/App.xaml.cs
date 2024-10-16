namespace TechLens
{
    public partial class App : Microsoft.Maui.Controls.Application
    {
        public App()
        {
            InitializeComponent();

            MainPage = new AppShell();
        }

        public static object IUsuarioRepo { get; internal set; }
    }
}
