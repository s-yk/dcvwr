using DocViewer.Model;
using DocViewer.ViewModel;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;

namespace DocViewer
{
    /// <summary>
    /// App.xaml の相互作用ロジック
    /// </summary>
    public partial class App : Application
    {
        private void Application_Startup(object sender, StartupEventArgs e)
        {
            var vm = new MainWindowViewModel(new ViewerModel());
            var window = new MainWindow();
            window.DataContext = vm;
            window.Show();
        }
    }
}
