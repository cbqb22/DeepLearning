using DeepLearning.VewModel;
using GalaSoft.MvvmLight.Ioc;
using GalaSoft.MvvmLight.Threading;
using Microsoft.Practices.ServiceLocation;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;

namespace DeepLearning
{
    /// <summary>
    /// App.xaml の相互作用ロジック
    /// </summary>
    public partial class App : Application
    {
        static App()
        {
            DispatcherHelper.Initialize();

            ServiceLocator.SetLocatorProvider(() => SimpleIoc.Default);
            ViewModelLocator.RegisterServices();
        }

        private void Application_Startup(object sender, StartupEventArgs e)
        {
            
        }
    }
}
