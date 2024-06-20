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
using System.Windows.Shapes;

namespace KrestikiNoliki
{
    /// <summary>
    /// Логика взаимодействия для StartWindow.xaml
    /// </summary>
    public partial class StartWindow : Window
    {
        public StartWindow()
        {
            InitializeComponent();
        }

        private void CheckBox_Checked(object sender, RoutedEventArgs e)
        {
            ComboBoxWhoStart.Items.Clear();
            ComboBoxWhoStart.Items.Add("Игрок");
            ComboBoxWhoStart.Items.Add("Компьютер");
            ComboBoxWhoStart.SelectedIndex = 0;
            TB_Name2.Visibility = Visibility.Collapsed;
            TB_Name2.Text = null;
        }

        private void CheckBox_Unchecked(object sender, RoutedEventArgs e)
        {
            ComboBoxWhoStart.Items.Clear();
            ComboBoxWhoStart.Items.Add("Первый игрок");
            ComboBoxWhoStart.Items.Add("Второй игрок");
            ComboBoxWhoStart.SelectedIndex = 0;
            TB_Name2.Visibility = Visibility.Visible;
        }

        private void BtnStart_Click(object sender, RoutedEventArgs e)
        {
            if (!string.IsNullOrWhiteSpace(TB_Name1.Text))
            {
                var selected = ComboBoxWhoStart.Text;
                if (!string.IsNullOrEmpty(TB_Name2.Text) && TB_Name2.Text.Length <= 10)
                {
                    Manager.SetSettings(TB_Name1.Text.Trim(), TB_Name2.Text.Trim(), selected);
                    MainWindow mainWindow = new MainWindow();
                    mainWindow.Show();
                    this.Close();
                }
                else if (TB_Name2.Visibility == Visibility.Collapsed)
                {
                    Manager.SetSettings(TB_Name1.Text.Trim(), TB_Name2.Text.Trim(), selected);
                    MainWindow mainWindow = new MainWindow();
                    mainWindow.Show();
                    this.Close();
                }
                else
                {
                    TB_Errors.Text = "Имя не может быть пустым";
                    TB_Errors.Margin = new Thickness(0, 10, 0, 0);
                }

            }
            else
            {
                TB_Errors.Text = "Имя не может быть пустым";
                TB_Errors.Margin = new Thickness(0, 10, 0, 0);
            }
        }
    }
}
