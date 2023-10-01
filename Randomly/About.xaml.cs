using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace Randomly
{
    /// <summary>
    /// About.xaml 的交互逻辑
    /// </summary>
    public partial class About : Window
    {
        public About()
        {
            InitializeComponent();
        }

        private void window_Loaded(object sender, RoutedEventArgs e)
        {
            Storyboard storyboard = Resources["backgroundOpacity"] as Storyboard;

            storyboard.SetValue(Storyboard.TargetNameProperty, "About");

            // 开始动画
            storyboard.Begin();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            string openSourceUrl = "https://github.com/yangnuozhen/Randomly";

            Process.Start(new ProcessStartInfo
            {
                FileName = openSourceUrl,
                UseShellExecute = true
            });
        }
    }
}
