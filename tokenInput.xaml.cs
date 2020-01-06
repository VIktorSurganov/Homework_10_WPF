using Microsoft.Win32;
using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using Telegram.Bot;

namespace WPF_SomeName200_bot
{
    /// <summary>
    /// Класс окна ввода токена
    /// </summary>
    public partial class tokenInput : Window
    {
        #region Поля
        public static string token { get; set; }    //Токен для взаимодействия с API телеграм
        static bool bottest = false;    //Флаг проверки отклика бота
        #endregion
        #region Конструктор
        public tokenInput() //Конструктор инициализирует окно и размещает его по центру экрана
        {
            this.WindowStartupLocation = WindowStartupLocation.CenterScreen;
            InitializeComponent();
        }
        #endregion
        #region Обработка событий 
        //Открытие поиска
        private void searchBTN_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog open = new OpenFileDialog();
            open.Filter = "Text Files|*.txt";
            if (open.ShowDialog() == true)
            {
                forTokenTbox.Text = System.IO.File.ReadAllText(open.FileName);
            }
        }

        //Клавиша для ввода токена
        private void okBTN_Click(object sender, RoutedEventArgs e)
        {
            //Если поле для ввода не пусто и колличестов символов = 45, пробуем получить ответ от бота
            if (forTokenTbox.Text != "" && forTokenTbox.Text.Length == 45)
            {
                try
                {
                    TelegramBotClient testClient = new TelegramBotClient(token);
                    var me = testClient.GetMeAsync().Result;
                    bottest = true;
                }
                catch (Exception e1)
                {
                    MessageBox.Show($"Не удается подключиться к боту с текущим токеном\nПроверьте правильность написания токена!\nFROM SYSTEM: {e1.Message.ToString()}");
                    bottest = false;
                    forTokenTbox.Text = "";
                }
                //Если ответ не получен, то показываем форму заново
                if (!bottest) Show();
                //Иначе открываем главное окно, это закрываем
                else
                {
                    token = forTokenTbox.Text;
                    MainWindow mainWindow = new MainWindow(token);
                    mainWindow.Show();
                    this.Close();
                }
            }
            else { MessageBox.Show("Необходимо ввести токен!"); }
        }

        //Прямоугольник для перетаскивания окна в альтернативу стандартному
        private void Rectangle_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (e.ChangedButton == MouseButton.Left)
                DragMove();
        }

        //Алтернативная кнопка закрытия
        private void Button_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        //Проверка на корректность токена (длина), с отображением на countTB
        private void forTokenTbox_TextChanged(object sender, TextChangedEventArgs e)
        {
            countTB.Foreground = new SolidColorBrush(Colors.Black);
            if (forTokenTbox.Text.Length == 45)
            {
                countTB.Foreground = new SolidColorBrush(Colors.White);
            }
            countTB.Text = forTokenTbox.Text.Length.ToString();
            if (forTokenTbox.Text.Length > 45)
            {
                MessageBox.Show("Длина токена не должна превышать 45 символов!");
                string g = forTokenTbox.Text.Remove(44, forTokenTbox.Text.Length - 45);
                forTokenTbox.Text = g;
            }
            token = forTokenTbox.Text;
        }

        //Альтернатива нажатию кнопки ОК, нажатием ENTER
        private void Window_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                if (forTokenTbox.Text != "" && forTokenTbox.Text.Length == 45)
                {
                    try
                    {
                        TelegramBotClient testClient = new TelegramBotClient(token);
                        var me = testClient.GetMeAsync().Result;
                        bottest = true;
                    }
                    catch (Exception e1)
                    {
                        MessageBox.Show($"Не удается подключиться к боту с текущим токеном\nПроверьте правильность написания токена!\nFROM SYSTEM: {e1.Message.ToString()}");
                        bottest = false;
                        forTokenTbox.Text = "";
                    }
                    if (!bottest) Show();
                    else
                    {
                        token = forTokenTbox.Text;
                        MainWindow mainWindow = new MainWindow(token);
                        mainWindow.Show();
                        this.Close();
                    }
                }
                else { MessageBox.Show("Необходимо ввести токен!"); }
            }
        }
        #endregion
    }
}
