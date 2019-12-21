using Xamarin.Forms;

namespace MyCustomPicker
{
    public partial class App : Application
    {
        public App()
        {
            InitializeComponent();

            MainPage = new Views.MonkeyView();
        }
    }
}
