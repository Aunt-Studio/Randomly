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
using System.Windows.Media.Animation;
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
        private bool isAscending = true;
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
                if (minValue <= maxValue)
                {
                    while (randomNumbers.Count < quantity)
                    {
                        int randomNumber = getRandom(minValue, maxValue);
                        if (cbAntiRepeat.IsChecked == true)
                        {
                            if (quantity <= maxValue - minValue + 1 && !randomNumbers.Contains(randomNumber))
                            {
                                randomNumbers.Add(randomNumber);
                            }
                            else if (quantity > maxValue - minValue + 1)
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
                    Storyboard storyboard = Resources["addItem"] as Storyboard;

                    storyboard.SetValue(Storyboard.TargetNameProperty, "MainWindow");

                    // 开始动画
                    storyboard.Begin();
                }
                else
                {
                    // 输入的值无效，显示错误
                    ErrorPage errorPage = new ErrorPage("为什么最小值比最大值还大啊喂 (#`O′)");
                    errorPage.Show();
                }
                

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
            About aboutPage = new About();
            aboutPage.Show();
        }
        
        private int getRandom(int minValue, int maxValue) 
        {
            if(minValue <= maxValue)
            {
                int randomNumber = randomSeed.Next(minValue, maxValue + 1);

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

        private void btnSort_Click(object sender, RoutedEventArgs e)
        {
            if (isAscending)
            {
                randomNumbers.Sort((a, b) => a.CompareTo(b)); // 从小到大排序
                btnSort.Content = "从大到小排序";
            }
            else
            {
                randomNumbers.Sort((a, b) => b.CompareTo(a)); // 从大到小排序
                btnSort.Content = "从小到大排序";
            }

            isAscending = !isAscending;
            
            lbRandomList.ItemsSource = null; //刷新显示
            lbRandomList.ItemsSource = randomNumbers;
        }

    }
}
