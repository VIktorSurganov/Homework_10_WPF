using Microsoft.Win32;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace WPF_SomeName200_bot
{
    public partial class MainWindow : Window
    {
        #region Поля и свойства
        public static TgMsgClient client;                           //Объект телеграм клиента
        public static WeatherClient clientWeather;                  //Объект клиента погоды
        //public static long userId = 0;
        public static FileInfo fi;                                  //Объект для работы с файлами для отправки
        public static string weatherString = string.Empty;          //Строка для информации о погоде
        public static List<long> ChatWindowOpen = new List<long>(); //Список id пользователей с которыми открыты окна для переписки
        public static bool IsClosed { get; set; } = false;          //Флаг закрытия окна
        #endregion

        #region Конструктор
        public MainWindow(string tokenForBot)
        {
            InitializeComponent();                                  //Инициализация окна
            App.Current.MainWindow.Title = "SomeName2000_bot";      //Заголовок окна
            client = new TgMsgClient(this, tokenForBot);            //Создание объекта клиента бота
            clientWeather = new WeatherClient(this);                //Объект для работы с погодными данными
            ForMsgLB.ItemsSource = client.LastMSG;                  //Привязка объекта коллекции логов к элементу ЛистБокс (ForMsgLB) на форме
            citiesCB.ItemsSource = clientWeather.citiesCollection;  //ComboBox заполняется городами из списка
        }
        #endregion

        #region Обработка событий
        /// <summary>
        /// Октрытие проводника для поиска музыки для отправки
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void SndMusicButton_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog open = new OpenFileDialog
            {
                Filter = "Music|*.mp3"
            };
            if (open.ShowDialog() == true)
            {
                fi = new System.IO.FileInfo(open.FileName);
                //Внесение имени музыки в текстбокс
                pathToFile.Text = fi.Name;
            }
        }

        /// <summary>
        /// Нажатие кнопки отправки погды
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private async void SndweatherButton_Click(object sender, RoutedEventArgs e)
        {
            //Если город выбран
            if (citiesCB.SelectedItem != null)
            {
                //Запрос погоды по выбранному городу
                weatherString = await WeatherClient.GetWeather(citiesCB.SelectedItem.ToString());
                //Создание объекта погоды
                clientWeather.ow = JsonConvert.DeserializeObject<WeatherClient.OpenWeather>(weatherString);
                var weather = clientWeather.ow;
                //Строка для отправки пользователю
                string weatherMessage =
                                $"City: {citiesCB.SelectedItem.ToString()}\n" +
                                $"Temp: {weather.main.Temp.ToString("0.##")} ℃\n" +
                                $"{weather.weather[0].description}\n" +
                                $"Humidity: {weather.main.humidity.ToString()}\n" +
                                $"Pressure: {((int)weather.main.Pressure).ToString()}\n" +
                                $"WindSpeed: {weather.wind.speed.ToString()}\n" +
                                $"WindDeg: {weather.wind.deg.ToString()}";

                Dispatcher.Invoke(() => client.SendMessage(weatherMessage, forIdTB.Text));
            }
            else
            {
                MessageBox.Show("Сперва выберите город.");
            }
        }

        /// <summary>
        /// Открытие проводника для поиска и отправки изображения
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void SndPicButton_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog open = new OpenFileDialog
            {
                Filter = "Picture|*.jpg"
            };
            if (open.ShowDialog() == true)
            {
                fi = new FileInfo(open.FileName);
                //Имя файла в текстбоксе
                pathToFile.Text = fi.FullName;
                //Изображение для отправки помещается в imgToSend
                imgToSend.Source = new BitmapImage(new Uri(@$"{fi.FullName}"));
            }
        }

        /// <summary>
        /// Обработчик события нажатия на кнопку отправки сообщения
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private async void OnSendMsgBtnClick(object sender, RoutedEventArgs e)
        {
            //Если пользователь выбран
            if (forIdTB.Text != "")
            {
                //Если есть прикрепленный файл и нет сообщения
                if (pathToFile.Text != "" && ForInputmsgTBox.Text == "")
                {
                    //В зависимости от типа файла вызывается соответсвующий метод
                    if (fi.FullName.Contains("jpg"))
                    {
                        using (var s = File.OpenRead($@"{fi.FullName}"))
                        {
                            var fileToSend = new Telegram.Bot.Types.InputFiles.InputOnlineFile(s);
                            await client.bot.SendPhotoAsync(Convert.ToInt64(forIdTB.Text), fileToSend).ConfigureAwait(false);
                        }
                    }
                    if (fi.FullName.Contains("mp3"))
                    {
                        using (var s = File.OpenRead($@"{fi.FullName}"))
                        {
                            var fileToSend = new Telegram.Bot.Types.InputFiles.InputOnlineFile(s);
                            await client.bot.SendAudioAsync(Convert.ToInt64(forIdTB.Text), fileToSend).ConfigureAwait(false);
                        }
                    }

                    //Очистка формы
                    Dispatcher.Invoke(() => pathToFile.Text = "");

                    Dispatcher.Invoke(() => imgToSend.Source = null);
                }
                //Если нет файла для отправки и есть сообщение
                else if (pathToFile.Text == "" && ForInputmsgTBox.Text != "")
                {
                    //Строка сообщения из данных текстбокса
                    string message = ForInputmsgTBox.Text;
                    //Отправка и добавление в лог сообщения, очистка формы
                    Dispatcher.Invoke(() => client.SendMessage(ForInputmsgTBox.Text, forIdTB.Text));

                    Dispatcher.Invoke(() => client.BotMsgLog[TgMsgClient.usersDictionary[long.Parse(forIdTB.Text)] - 1].Add(new MsgLog(message, "СомНэйм", client.bot.BotId)));

                    Dispatcher.Invoke(() => pathToFile.Text = "");

                    Dispatcher.Invoke(() => ForInputmsgTBox.Clear());

                    Dispatcher.Invoke(() => imgToSend.Source = null);
                }
                //Если есть и сообщение и файл, то выполняется совокупность действий двух предыдущих if
                else if (pathToFile.Text != "" && ForInputmsgTBox.Text != "")
                {
                    string message = ForInputmsgTBox.Text;

                    if (pathToFile.Text != "" && !pathToFile.Text.Contains("\"coord\""))
                    {

                        if (fi.FullName.Contains("jpg"))
                        {
                            using (var s = File.OpenRead($@"{fi.FullName}"))
                            {
                                var fileToSend = new Telegram.Bot.Types.InputFiles.InputOnlineFile(s);
                                await client.bot.SendPhotoAsync(Convert.ToInt64(forIdTB.Text), fileToSend).ConfigureAwait(false);
                            }
                        }
                        if (fi.FullName.Contains("mp3"))
                        {
                            using (var s = File.OpenRead($@"{fi.FullName}"))
                            {
                                var fileToSend = new Telegram.Bot.Types.InputFiles.InputOnlineFile(s);
                                await client.bot.SendAudioAsync(Convert.ToInt64(forIdTB.Text), fileToSend).ConfigureAwait(false);
                            }
                        }

                    }

                    Dispatcher.Invoke(() => client.SendMessage(ForInputmsgTBox.Text, forIdTB.Text));

                    Dispatcher.Invoke(() => client.BotMsgLog[TgMsgClient.usersDictionary[long.Parse(forIdTB.Text)] - 1].Add(new MsgLog(message, "СомНэйм", client.bot.BotId)));

                    Dispatcher.Invoke(() => pathToFile.Text = "");

                    Dispatcher.Invoke(() => ForInputmsgTBox.Clear());

                    Dispatcher.Invoke(() => imgToSend.Source = null);
                }                
                else
                {
                    MessageBox.Show("Нет данных для отправки.");
                }
            }
            else
            {
                MessageBox.Show("Выберете получателя.");
            }
        }

        /// <summary>
        /// Метод обработки события нажатия кнопки "отправить всем". Алгоритмы и методы аналогичны кнопке отправки сообщения.
        /// Только здесь отправка происходит всем пользователям из списка TgMsgClient.idList посредством foreach
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private async void OnSendMsgToAllBtnClick(object sender, RoutedEventArgs e)
        {
            if (TgMsgClient.idList.Count != 0)
            {
                if (pathToFile.Text != "" && ForInputmsgTBox.Text == "")
                {
                    if (pathToFile.Text != "" && !pathToFile.Text.Contains("\"coord\""))
                    {

                        if (fi.FullName.Contains("jpg"))
                        {
                            foreach (int i in TgMsgClient.idList)
                            {
                                using (var s = File.OpenRead($@"{fi.FullName}"))
                                {
                                    var fileToSend = new Telegram.Bot.Types.InputFiles.InputOnlineFile(s);

                                    long id = Convert.ToInt64(i);
                                    await client.bot.SendPhotoAsync(id, fileToSend);
                                }
                            }
                        }
                        if (fi.FullName.Contains("mp3"))
                        {
                            foreach (int i in TgMsgClient.idList)
                            {
                                using (var s = File.OpenRead($@"{fi.FullName}"))
                                {
                                    var fileToSend = new Telegram.Bot.Types.InputFiles.InputOnlineFile(s);

                                    long id = Convert.ToInt64(i);
                                    await client.bot.SendAudioAsync(id, fileToSend);
                                }
                            }
                        }
                    }
                    Dispatcher.Invoke(() => pathToFile.Text = "");

                    Dispatcher.Invoke(() => imgToSend.Source = null);
                }
                else if (pathToFile.Text == "" && ForInputmsgTBox.Text != "")
                {
                    string message = ForInputmsgTBox.Text;
                    foreach (int i in TgMsgClient.idList)
                    {
                        long id = Convert.ToInt64(i);
                        await client.bot.SendTextMessageAsync(id, ForInputmsgTBox.Text);
                    }

                    foreach (int i in TgMsgClient.idList)
                    {
                        long id = Convert.ToInt64(i);
                        Dispatcher.Invoke(() => client.BotMsgLog[TgMsgClient.usersDictionary[id] - 1].Add(new MsgLog(message, "СомНэйм", client.bot.BotId)));
                    }

                    Dispatcher.Invoke(() => ForInputmsgTBox.Clear());
                }
                else if (pathToFile.Text != "" && ForInputmsgTBox.Text != "")
                {
                    if (pathToFile.Text != "" && !pathToFile.Text.Contains("\"coord\""))
                    {

                        if (fi.FullName.Contains("jpg"))
                        {
                            foreach (int i in TgMsgClient.idList)
                            {
                                using (var s = File.OpenRead($@"{fi.FullName}"))
                                {
                                    var fileToSend = new Telegram.Bot.Types.InputFiles.InputOnlineFile(s);

                                    long id = Convert.ToInt64(i);
                                    await client.bot.SendPhotoAsync(id, fileToSend);
                                }
                            }
                        }
                        if (fi.FullName.Contains("mp3"))
                        {
                            foreach (int i in TgMsgClient.idList)
                            {
                                using (var s = File.OpenRead($@"{fi.FullName}"))
                                {
                                    var fileToSend = new Telegram.Bot.Types.InputFiles.InputOnlineFile(s);

                                    long id = Convert.ToInt64(i);
                                    await client.bot.SendAudioAsync(id, fileToSend);
                                }
                            }
                        }
                    }

                    string message = ForInputmsgTBox.Text;
                    foreach (int i in TgMsgClient.idList)
                    {
                        long id = Convert.ToInt64(i);
                        await client.bot.SendTextMessageAsync(id, ForInputmsgTBox.Text);
                    }

                    foreach (int i in TgMsgClient.idList)
                    {
                        long id = Convert.ToInt64(i);
                        Dispatcher.Invoke(() => client.BotMsgLog[TgMsgClient.usersDictionary[id] - 1].Add(new MsgLog(message, "СомНэйм", client.bot.BotId)));
                    }

                    Dispatcher.Invoke(() => pathToFile.Text = "");

                    Dispatcher.Invoke(() => ForInputmsgTBox.Clear());

                    Dispatcher.Invoke(() => imgToSend.Source = null);
                }
                else
                {
                    MessageBox.Show("Нет данных для отправки.");
                }
            }
            else
            {
                MessageBox.Show("Не найдено ни одного получателя.");
            }
        }

        /// <summary>
        /// Открытие окна с чатом с конркетным пользователем
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ForMsgLB_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            //Новый объект лога
            MsgLog lMsg = (MsgLog)ForMsgLB.SelectedItem;
            //Если пользователя нет в списке открытых окон с чатом
            if (!ChatWindowOpen.Contains(lMsg.ID))
            {
                //Создание объекта окна чата
                ChatWindow chatWindow = new ChatWindow(lMsg.Name, lMsg.ID);
                //Добавление ползователя в список открытых окон с чатом 
                ChatWindowOpen.Add(lMsg.ID);
                //Открытие окна с чатом
                chatWindow.Show();
            }
            else
            {
                MessageBox.Show("Переписка с данным пользователем уже открыта");
            }
        }

        /// <summary>
        /// Обработчик события загрузки окна
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            Application.Current.MainWindow = this;
        }

        /// <summary>
        /// Выбор города в ComboBox
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void CitiesCB_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            //Создание потока для обновления данных погоды на форме
            var thread = new System.Threading.Thread(() =>
            {
                //Пока окно не закрыто
                while (!IsClosed)
                {
                    //Запрос данных о погоде каждые 5 секунд и обновление элементов окна новыми данными
                    Dispatcher.Invoke(new Action(async () =>
                    {
                        weatherString = await WeatherClient.GetWeather(citiesCB.SelectedItem.ToString());
                        //pathToFile.Text = weatherString;
                        clientWeather.ow = JsonConvert.DeserializeObject<WeatherClient.OpenWeather>(weatherString);
                        var weather = clientWeather.ow;

                        tempLBL.Content = $"Temp: {weather.main.Temp.ToString("0.##")} ℃";
                        tempDescriptLBL.Content = $"{weather.weather[0].description}";
                        temphumidityLBL.Content = $"Humidity: {weather.main.humidity.ToString()}";
                        temppressureLBL.Content = $"Pressure: {((int)weather.main.Pressure).ToString()}";
                        tempwindspeedLBL.Content = $"WindSpeed: {weather.wind.speed.ToString()}";
                        tempwinddegLBL.Content = $"WindDeg: {weather.wind.deg.ToString()}";
                        tempImage.Source = WeatherClient.weather.BitmapToImageSource(weather.weather[0].Icon);

                    }));
                    System.Threading.Thread.Sleep(5000);
                }
            });
            thread.Start();
        }        

        /// <summary>
        /// При закрытии - активация флага
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Window_Closing(object sender, CancelEventArgs e)
        {
            IsClosed = true;
        }

        /// <summary>
        /// Прямоугольник для перетаскивания
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Rectangle_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (e.ChangedButton == MouseButton.Left)
                DragMove();
        }

        /// <summary>
        /// Кнопка для закрытия
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void CloseMWbtn_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        /// <summary>
        /// Нажатие Enter аналогичные действия отправки сообщения пользователю
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private async void Window_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                //Если пользователь выбран
                if (forIdTB.Text != "")
                {
                    //Если есть прикрепленный файл и нет сообщения
                    if (pathToFile.Text != "" && ForInputmsgTBox.Text == "")
                    {
                        //В зависимости от типа файла вызывается соответсвующий метод
                        if (fi.FullName.Contains("jpg"))
                        {
                            using (var s = File.OpenRead($@"{fi.FullName}"))
                            {
                                var fileToSend = new Telegram.Bot.Types.InputFiles.InputOnlineFile(s);
                                await client.bot.SendPhotoAsync(Convert.ToInt64(forIdTB.Text), fileToSend).ConfigureAwait(false);
                            }
                        }
                        if (fi.FullName.Contains("mp3"))
                        {
                            using (var s = File.OpenRead($@"{fi.FullName}"))
                            {
                                var fileToSend = new Telegram.Bot.Types.InputFiles.InputOnlineFile(s);
                                await client.bot.SendAudioAsync(Convert.ToInt64(forIdTB.Text), fileToSend).ConfigureAwait(false);
                            }
                        }

                        //Очистка формы
                        Dispatcher.Invoke(() => pathToFile.Text = "");

                        Dispatcher.Invoke(() => imgToSend.Source = null);
                    }
                    //Если нет файла для отправки и есть сообщение
                    else if (pathToFile.Text == "" && ForInputmsgTBox.Text != "")
                    {
                        //Строка сообщения из данных текстбокса
                        string message = ForInputmsgTBox.Text;
                        //Отправка и добавление в лог сообщения, очистка формы
                        Dispatcher.Invoke(() => client.SendMessage(ForInputmsgTBox.Text, forIdTB.Text));

                        Dispatcher.Invoke(() => client.BotMsgLog[TgMsgClient.usersDictionary[long.Parse(forIdTB.Text)] - 1].Add(new MsgLog(message, "СомНэйм", client.bot.BotId)));

                        Dispatcher.Invoke(() => pathToFile.Text = "");

                        Dispatcher.Invoke(() => ForInputmsgTBox.Clear());

                        Dispatcher.Invoke(() => imgToSend.Source = null);
                    }
                    //Если есть и сообщение и файл, то выполняется совокупность действий двух предыдущих if
                    else if (pathToFile.Text != "" && ForInputmsgTBox.Text != "")
                    {
                        string message = ForInputmsgTBox.Text;

                        if (pathToFile.Text != "" && !pathToFile.Text.Contains("\"coord\""))
                        {

                            if (fi.FullName.Contains("jpg"))
                            {
                                using (var s = File.OpenRead($@"{fi.FullName}"))
                                {
                                    var fileToSend = new Telegram.Bot.Types.InputFiles.InputOnlineFile(s);
                                    await client.bot.SendPhotoAsync(Convert.ToInt64(forIdTB.Text), fileToSend).ConfigureAwait(false);
                                }
                            }
                            if (fi.FullName.Contains("mp3"))
                            {
                                using (var s = File.OpenRead($@"{fi.FullName}"))
                                {
                                    var fileToSend = new Telegram.Bot.Types.InputFiles.InputOnlineFile(s);
                                    await client.bot.SendAudioAsync(Convert.ToInt64(forIdTB.Text), fileToSend).ConfigureAwait(false);
                                }
                            }

                        }

                        Dispatcher.Invoke(() => client.SendMessage(ForInputmsgTBox.Text, forIdTB.Text));

                        Dispatcher.Invoke(() => client.BotMsgLog[TgMsgClient.usersDictionary[long.Parse(forIdTB.Text)] - 1].Add(new MsgLog(message, "СомНэйм", client.bot.BotId)));

                        Dispatcher.Invoke(() => pathToFile.Text = "");

                        Dispatcher.Invoke(() => ForInputmsgTBox.Clear());

                        Dispatcher.Invoke(() => imgToSend.Source = null);
                    }
                    else
                    {
                        MessageBox.Show("Нет данных для отправки.");
                    }
                }
                else
                {
                    MessageBox.Show("Выберете получателя.");
                }
            }
        }
        #endregion
    }
}
