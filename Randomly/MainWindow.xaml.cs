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

namespace Randomly
{
    /// <summary>
    /// MainWindow.xaml 的交互逻辑
    /// </summary>
    public partial class MainWindow : Window
    {
        private Random randomSeed = new Random();
        List<int> randomNumbers = new List<int>();

        public MainWindow()
        {
            InitializeComponent();
        }

        private void btnStart_Click(object sender, RoutedEventArgs e)
        {
            randomNumbers.Clear();
            if (int.TryParse(tbStartNum.Text, out int minValue) &&
                int.TryParse(tbEndNum.Text, out int maxValue) &&
                int.TryParse(tbNum.Text, out int quantity))
            {
                
                while (randomNumbers.Count < quantity)
                {
                    int randomNumber = getRandom(minValue, maxValue);
                    if(cbAntiRepeat.IsChecked == true)
                    {
                        if (quantity < maxValue - minValue + 1 && !randomNumbers.Contains(randomNumber))
                        {
                            randomNumbers.Add(randomNumber);
                        }else if(quantity > maxValue - minValue + 1)
                        {
                            //取得值比能取到不重复的值大，显示错误
                            ErrorPage errorPage = new ErrorPage("你开启了避免重复，但是你取的值太大啦 ＞﹏＜");
                            errorPage.Show();
                            break;
                        }
                    }
                    else
                    {
                        randomNumbers.Add(randomNumber);
                    }
                    
                }
                lbRandomList.ItemsSource = null; //刷新显示
                lbRandomList.ItemsSource = randomNumbers;
            }
            else
            {
                // 输入的值无效，显示错误
                ErrorPage errorPage = new ErrorPage("好像出了点问题。。。一定不是Randomly的问题，一定不是!");
                errorPage.Show();
            }

        }

        private void btnDelete_Click(object sender, RoutedEventArgs e)
        {
            if (lbRandomList.SelectedItem != null)
            {
                int selectedValue = (int)lbRandomList.SelectedItem;
                randomNumbers.Remove(selectedValue);
                lbRandomList.ItemsSource = null; //刷新显示
                lbRandomList.ItemsSource = randomNumbers;
            }
        }

        private void btnAbout_Click(object sender, RoutedEventArgs e)
        {

        }
        
        private int getRandom(int minValue, int maxValue) 
        {
            if(minValue <= maxValue)
            {
                int randomNumber = randomSeed.Next(minValue, maxValue);

                return randomNumber;
            }
            else
            {
                return 0;
            }
        }

        private void lbRandomList_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (lbRandomList.SelectedItem != null)
            {
                btnDelete.IsEnabled = true;
            }
            else
            {
                btnDelete.IsEnabled = false;
            }
        }

    }
}
