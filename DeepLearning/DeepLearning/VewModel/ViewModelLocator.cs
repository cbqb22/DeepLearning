using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Practices.ServiceLocation;
using DeepLearning;
using GalaSoft.MvvmLight.Ioc;

namespace DeepLearning.VewModel
{
    public class ViewModelLocator
    {

        public static void RegisterServices()
        {
            SimpleIoc.Default.Register<MainWindowViewModel>();
        }

        public static MainWindowViewModel Main
        {
            get
            {
                return ServiceLocator.Current.GetInstance<MainWindowViewModel>();
            }
        }

    }
}
