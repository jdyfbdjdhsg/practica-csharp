using System.Windows;

namespace Day18
{
    public partial class App : Application
    {
        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);
            
            this.DispatcherUnhandledException += (sender, args) =>
            {
                MessageBox.Show("Произошла ошибка: " + args.Exception.Message, 
                    "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                args.Handled = true;
            };
        }
    }
}