using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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
        ObservableCollection<int> randomNumbers = new ObservableCollection<int>();
        private bool isAscending = true;
        public MainWindow()
        {
            InitializeComponent();
        }
        private async Task GetAndSetRandomListAsync(int minValue, int maxValue, int quantity, bool noRepeat)
        {
            int randomNumber;
            if (noRepeat == true)
            {
                do
                {
                    randomNumber = getRandom(minValue, maxValue);
                } while (randomNumbers.Contains(randomNumber));
            }
            else
            {
                randomNumber = getRandom(minValue, maxValue);
            }
            //确保在主线程上进行集合修改
            await Application.Current.Dispatcher.InvokeAsync(() =>
               {
                   randomNumbers.Add(randomNumber);
                   pbProgress.Value = ((double)randomNumbers.Count / (double)quantity) * 100;
               });

            if (randomNumbers.Count == quantity) {
                await Application.Current.Dispatcher.InvokeAsync(() =>
                {
                    Storyboard storyboard = Resources["addItem"] as Storyboard;

                    storyboard.SetValue(Storyboard.TargetNameProperty, "MainWindow");

                    // 开始动画
                    
                    storyboard.Begin();
                    pbProgress.Value = 100;
                    btnStart.IsEnabled = true;
                    RandomlyIcon.IsEnabled = true;
                });

            }
        }
        private async Task GenerateRandomNumbersAsync(int minValue, int maxValue, int quantity, bool noRepeat)
        {
            await Application.Current.Dispatcher.InvokeAsync(() =>
            {
                if (quantity > 500)
                {
                    pbProgress.Visibility = Visibility.Visible;
                }
                else
                {
                    pbProgress.Visibility = Visibility.Hidden;
                }
            });
            await Task.Run(async () =>
            {
                for (int i = 0; i < quantity; i++)
                {
                    await GetAndSetRandomListAsync(minValue, maxValue, quantity,noRepeat);
                }
            });
        }
        private async void btnStart_Click(object sender, RoutedEventArgs e)
        {
            
            randomNumbers.Clear();
            lbRandomList.ItemsSource = null;
            lbRandomList.ItemsSource = randomNumbers;
            btnStart.IsEnabled = false;
            RandomlyIcon.IsEnabled = false;
            if (int.TryParse(tbStartNum.Text, out int minValue) &&
                int.TryParse(tbEndNum.Text, out int maxValue) &&
                int.TryParse(tbNum.Text, out int quantity))
            {
                if (minValue < maxValue)
                {
                    if (quantity > maxValue - minValue + 1)
                    {
                        if ((bool)cbAntiRepeat.IsChecked)
                        {

                            ErrorPage errorPage = new ErrorPage("你开启了避免重复，但是你取的值太大啦 ＞﹏＜");
                            errorPage.Show();
                            btnStart.IsEnabled = true;
                            RandomlyIcon.IsEnabled = true;
                        }
                        else
                        {
                            // 特殊情况: 取数数量超过可取数量，只允许重复
                            await GenerateRandomNumbersAsync(minValue, maxValue, quantity,false);
                        }
                    }
                    else
                    {
                        // 通常情况: 允许重复 + 不允许重复
                        await GenerateRandomNumbersAsync(minValue, maxValue, quantity,(bool)cbAntiRepeat.IsChecked);
                    }

                }
                else
                {
                    // 输入的值无效，显示错误
                    ErrorPage errorPage = new ErrorPage("为什么最小值比最大值还大啊喂 (#`O′)");
                    errorPage.Show();
                    btnStart.IsEnabled = true;
                    RandomlyIcon.IsEnabled = true;
                }


            }
            else
            {
                // 输入的值无效，显示错误
                ErrorPage errorPage = new ErrorPage("好像出了点问题。。。一定不是Randomly的问题，一定不是!\n 无法将输入值转化为int类型。");
                errorPage.Show();
                btnStart.IsEnabled = true;
                RandomlyIcon.IsEnabled = true;
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
            List<int> randomNumbersList = new List<int>(randomNumbers);
            if (isAscending)
            {
                randomNumbersList.Sort((a, b) => a.CompareTo(b)); // 从小到大排序
                btnSort.Content = "从大到小排序";
            }
            else
            {
                randomNumbersList.Sort((a, b) => b.CompareTo(a)); // 从大到小排序
                btnSort.Content = "从小到大排序";
            }

            isAscending = !isAscending;
            randomNumbers = new ObservableCollection<int>(randomNumbersList);
            lbRandomList.ItemsSource = null; //刷新显示
            lbRandomList.ItemsSource = randomNumbers;
        }

        private async void RandomlyIcon_Click(object sender, RoutedEventArgs e)
        {
            randomNumbers.Clear();
            lbRandomList.ItemsSource = null;
            lbRandomList.ItemsSource = randomNumbers;
            btnStart.IsEnabled = false;
            RandomlyIcon.IsEnabled = false;
            if (int.TryParse(tbStartNum.Text, out int minValue) &&
                int.TryParse(tbEndNum.Text, out int maxValue) &&
                int.TryParse(tbNum.Text, out int quantity))
            {
                if (minValue < maxValue)
                {
                    if (quantity > maxValue - minValue + 1)
                    {
                        if ((bool)cbAntiRepeat.IsChecked)
                        {

                            ErrorPage errorPage = new ErrorPage("你开启了避免重复，但是你取的值太大啦 ＞﹏＜");
                            errorPage.Show();
                            btnStart.IsEnabled = true;
                            RandomlyIcon.IsEnabled = true;
                        }
                        else
                        {
                            // 特殊情况: 取数数量超过可取数量，只允许重复
                            await GenerateRandomNumbersAsync(minValue, maxValue, quantity, false);
                        }
                    }
                    else
                    {
                        // 通常情况: 允许重复 + 不允许重复
                        await GenerateRandomNumbersAsync(minValue, maxValue, quantity, (bool)cbAntiRepeat.IsChecked);
                    }

                }
                else
                {
                    // 输入的值无效，显示错误
                    ErrorPage errorPage = new ErrorPage("为什么最小值比最大值还大啊喂 (#`O′)");
                    errorPage.Show();
                    btnStart.IsEnabled = true;
                    RandomlyIcon.IsEnabled = true;
                }


            }
            else
            {
                // 输入的值无效，显示错误
                ErrorPage errorPage = new ErrorPage("好像出了点问题。。。一定不是Randomly的问题，一定不是!\n 无法将输入值转化为int类型。");
                errorPage.Show();
                btnStart.IsEnabled = true;
                RandomlyIcon.IsEnabled = true;
            }



        }

    }
}
