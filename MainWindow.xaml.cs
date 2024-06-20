using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Interop;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Windows.Threading;
using static System.Net.Mime.MediaTypeNames;

namespace KrestikiNoliki
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        string name1 = string.Empty;
        string name2 = string.Empty;

        private static int player;
        private static bool computerPlay;
        private static int counter = 0;

        public static int[,] winCombination = new int[8, 3]
        {
            { 0, 1, 2},{ 3, 4, 5 },{ 6, 7, 8 }, { 0, 3, 6}, { 1, 4, 7}, { 2, 5, 8}, { 0, 4, 8}, { 2, 4, 6}
        };

        public static Button[] btns = new Button[9];


        public MainWindow()
        {
            InitializeComponent();
            name1 = Manager.name1;
            name2 = Manager.name2;
            WhoStart(Manager.selecteg);
            counter = 0;
            SetField();
            if(Manager.selecteg == "Компьютер")
            {
                ComputerStep();
            }
        }


        static string text = "";
        bool win = false;

        public void CheckWinner()
        {

            if (counter == 9)
            {
                text = "Ничья";
                dispatcherTimer = new DispatcherTimer();
                dispatcherTimer.Interval = TimeSpan.FromSeconds(2);
                dispatcherTimer.Tick += DispatcherTimer_Tick;
                dispatcherTimer.Start();
                return;

            }

            int rows = winCombination.GetUpperBound(0) + 1;
            int columns = winCombination.Length / rows;

            for (int i = 0; i < rows; i++)
            {
                for (int j = 0; j < columns; j++)
                {
                    //берём значение из комбинации, смотрим контент этой кнопки
                    var btn = (btns[winCombination[i, j]]);


                    if (btn.Content != null)
                    {
                        if (btn.Content.ToString() == CurrentPease())
                        {
                            win = true;
                            if (j == 2) break;
                        }
                        else
                        {
                            win = false;
                            break;
                        }
                    }
                    else
                    {
                        win = false;
                        break;
                    }
                    //берём кнопку по индексу комбинации

                }
                if (win)
                {
                    if (computerPlay)
                    {
                        if (player == 0)
                        {
                            text = $"Победа за {name1}";
                        }
                        else
                        {
                            text = $"Победа за компьютером";
                        }
                    }
                    else
                    {
                        if (player == 0)
                        {
                            text = $"Победа за {name1}";
                        }
                        else
                        {
                            text = $"Победа за {name2}";
                        }
                    }
                    foreach (var b in btns)
                    {
                        b.IsHitTestVisible = false;
                    }
                    dispatcherTimer = new DispatcherTimer();
                    dispatcherTimer.Interval = TimeSpan.FromSeconds(2);
                    dispatcherTimer.Tick += DispatcherTimer_Tick;
                    dispatcherTimer.Start();
                    return;
                }
            }
        }

        private void DispatcherTimer_Tick(object sender, EventArgs e)
        {
            dispatcherTimer.Stop();
            ResultWindow resultWindow = new ResultWindow(text);
            resultWindow.Show();
            this.Close();
        }

        DispatcherTimer dispatcherTimer = new DispatcherTimer();
        Button btnComp;

        public void ComputerStep()
        {
            while (true)
            {
                Random rand = new Random();
                int ind = rand.Next(0, 9);
                Button clickedButton = btns[ind];
                btnComp = clickedButton;

                dispatcherTimer.Interval = TimeSpan.FromSeconds(1);
                dispatcherTimer.Tick += Tick;



                if (btnComp.Content != null)
                {
                    dispatcherTimer = new DispatcherTimer();
                    continue;
                }
                else
                {
                    dispatcherTimer.Start();
                    break;
                }
            }

        }


        void Tick(object sender, EventArgs e)
        {
            dispatcherTimer.Stop();
            dispatcherTimer = new DispatcherTimer();
            btnComp.IsEnabled = false;
            btnComp.Content = CurrentPease();
            CheckWinner();
            NextPlayer();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            Button clickedButton = sender as Button;
            //лпределение игра с компьютером или нет
            if (computerPlay == true)
            {
                if (player == 0)
                {
                    clickedButton.IsEnabled = false;
                    clickedButton.Content = CurrentPease();
                    foreach (var b in btns)
                    {
                        b.IsHitTestVisible = false;
                    }
                    counter++;
                    CheckWinner();
                    if (!win)
                    {
                        NextPlayer();
                        ComputerStep();
                        foreach (var b in btns)
                        {
                            if (b.Content == null)
                            {
                                b.IsHitTestVisible = true;
                            }
                        }
                    }

                }
            }
            else
            {
                clickedButton.IsEnabled = false;
                clickedButton.Content = CurrentPease();
                counter++;
                CheckWinner();

                if (!win)
                    NextPlayer();
            }
        }

        public void NextPlayer()
        {
            if (computerPlay != true)
            {
                if (player == 0)
                {
                    player = 1;
                    curPlayer.Text = $"Ходит: {name2}";
                }
                else if (player == 1)
                {
                    player = 0;
                    curPlayer.Text = $"Ходит: {name1}";
                }
            }
            //если с компом
            else
            {
                //ходил человек
                if (player == 0)
                {
                    player = 1;
                    curPlayer.Text = $"Ходит: Компьютер";
                }
                //комп
                else if (player == 1)
                {
                    player = 0;
                    curPlayer.Text = $"Ходит: {name1}";
                    //ComputerStep();
                }
            }

        }

        public string CurrentPease()
        {
            if (player == 0)
            {
                return "X";
            }
            else
            {
                return "O";
            }
        }

        //задание кто начинает и отбражение в ТБ
        public void WhoStart(string name)
        {
            computerPlay = false;
            if (name2 != "")
            {
                switch (name)
                {
                    case "Второй игрок":
                        player = 1;
                        curPlayer.Text = $"Ходит: {name2}";
                        break;

                    case "Первый игрок":
                        player = 0;
                        curPlayer.Text = $"Ходит: {name1}";
                        break;
                }
            }
            else
            {
                computerPlay = true;
                switch (name)
                {
                    case "Игрок":
                        player = 0;
                        curPlayer.Text = $"Ходит: {name1}";
                        break;

                    case "Компьютер":
                        player = 1;
                        curPlayer.Text = $"Ходит: компьютер";
                        break;
                }
            }
        }

        //Заполнение панели кнопками
        public void SetField()
        {
            for (int i = 0; i < 9; i++)
            {
                var button = new Button
                {
                    Name = $"btn{i}",
                    Height = 100,
                    Width = 100,
                    FontSize = 70,
                    HorizontalContentAlignment = HorizontalAlignment.Center,
                    VerticalContentAlignment = VerticalAlignment.Center,
                    FontWeight = FontWeights.Bold,
                };
                button.Click += Button_Click;
                btns[i] = button;
                field.Children.Add(button);
            }
        }

        private void restartBtn_Click(object sender, RoutedEventArgs e)
        {
            MainWindow mainWindow = new MainWindow();
            mainWindow.Show();
            this.Close();
        }

        private void menuBtn_Click(object sender, RoutedEventArgs e)
        {
            StartWindow startWindow = new StartWindow();
            startWindow.Show();
            this.Close();
        }
    }
}
