using System.Windows;

namespace WPF_SomeName200_bot
{
    public partial class App : Application
    {

        private void Application_DispatcherUnhandledException(object sender, System.Windows.Threading.DispatcherUnhandledExceptionEventArgs e)
        {
            MessageBox.Show("An unhandled " + e.Exception.GetType().ToString() + " excrption was caught and ignored.");
            e.Handled = true;
        }
        private void OnStartup(object sender, StartupEventArgs e)
        {

        }

    }
}
