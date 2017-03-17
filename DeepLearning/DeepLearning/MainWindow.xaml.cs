using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using DeepLearning.MachineLearning;
using DeepLearning.MachineLearning.RBM;
using System.Collections.ObjectModel;
using DeepLearning.MNIST;
using System.Drawing;
using DeepLearning.Common.Draw;
using MahApps.Metro.Controls;

namespace DeepLearning
{
    /// <summary>
    /// MainWindow.xaml の相互作用ロジック
    /// </summary>
    public partial class MainWindow : MetroWindow
    {
        public MainWindow()
        {
            InitializeComponent();
        }
    }
}

