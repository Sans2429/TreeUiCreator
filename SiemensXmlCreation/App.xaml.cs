using System;
using System.Diagnostics;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Threading;

namespace SiemensXmlCreation
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        public App()
        {
            AppDomain.CurrentDomain.UnhandledException += CurrentDomain_UnhandledException;
            DispatcherUnhandledException += App_DispatcherUnhandledException;
            TaskScheduler.UnobservedTaskException += TaskScheduler_UnobservedTaskException;
        }
        private void App_DispatcherUnhandledException(object sender, DispatcherUnhandledExceptionEventArgs e)
        {
            if (Debugger.IsAttached)
                MessageBox.Show("WPF UI Thread Exception: " + e.Exception.Message);

            e.Handled = true; // Prevent crash
        }
        private void CurrentDomain_UnhandledException(object sender, UnhandledExceptionEventArgs e)
        {
            Exception ex = (Exception)e.ExceptionObject;
            if (Debugger.IsAttached)
                MessageBox.Show("AppDomain Exception: " + ex.Message);

            // Optionally log here
        }

        private void TaskScheduler_UnobservedTaskException(object sender, UnobservedTaskExceptionEventArgs e)
        {
            if (Debugger.IsAttached)
                MessageBox.Show("UnHandled exception: " + e.Exception.Message);

            e.SetObserved(); // Prevent process crash
        }
    }
}
