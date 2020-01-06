using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Media;
using Telegram.Bot;
using Telegram.Bot.Types;
using Telegram.Bot.Types.ReplyMarkups;

namespace WPF_SomeName200_bot
{
    public class TgMsgClient
    {
        #region Поля и свойства
        private SoundPlayer incMsgSound = new SoundPlayer("incMsg.wav");                    //звук входящего сообщения
        private MainWindow w;                                                               //объект главного окна
        public TelegramBotClient bot;                                                       //объект телеграм клиента
        public static List<int> idList = new List<int>();                                   //лист для id пользователей
        public ObservableCollection<MsgLog>[] BotMsgLog;                                    //коллекция для хранения информации о входящих сообщениях
        public ObservableCollection<MsgLog> LastMSG { get; set; }                           //Коллекция для хранения полседних сообщений пользователей для отображения их на ListBox главного окна
        public WeatherClient wc;                                                            //Объект с данными о погоде
        public static string weatherString = String.Empty;                                  //Строка для записи полученной информации о погоде
        public static Dictionary<long, int> usersDictionary = new Dictionary<long, int>();  //Словарь для поиска добавленных пользователей по id
        #endregion

        #region Конструктор
        public TgMsgClient(MainWindow w, string token)
        {
            wc = new WeatherClient(w);

            this.LastMSG = new ObservableCollection<MsgLog>();     //Коллекция для последних сообщений
            this.BotMsgLog = new ObservableCollection<MsgLog>[0];  //Коллекции для логгирования
            this.w = w;                                            //объект главного окнка
            bot = new TelegramBotClient(token);                    //клиент телеграм

            bot.OnCallbackQuery += Bot_OnCallbackQuery;            //Подписка на события нажатых клавиш
            bot.OnMessage += Bot_OnMessage;                        //подписка на входящие
            bot.StartReceiving();                                  //начать отслеживание
        }
        #endregion

        #region Обработка событий
        /// <summary>
        /// Обработчик телеграм кнопок
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Bot_OnCallbackQuery(object sender, Telegram.Bot.Args.CallbackQueryEventArgs e)
        {
            var m = e.CallbackQuery;    //аргументы события
            //Если в списке городов есть совпадение с сообщением польозвателя
            if (wc.citiesCollection.Contains(m.Data))
            {
                w.Dispatcher.Invoke(new Action(async () =>
                {
                    //Получение погоду
                    weatherString = await WeatherClient.GetWeather(m.Data);
                    //Создание объекта
                    wc.ow = JsonConvert.DeserializeObject<WeatherClient.OpenWeather>(weatherString);
                    var weather = wc.ow;
                    //Составление строки из данных объекта погоды для отправки
                    string weatherMessage =
                        $"City: {m.Data}\n" +
                        $"Temp: {weather.main.Temp.ToString("0.##")} ℃\n" +
                        $"{weather.weather[0].description}\n" +
                        $"Humidity: {weather.main.humidity.ToString()}\n" +
                        $"Pressure: {((int)weather.main.Pressure).ToString()}\n" +
                        $"WindSpeed: {weather.wind.speed.ToString()}\n" +
                        $"WindDeg: {weather.wind.deg.ToString()}";
                    //Отправка сообщения посредством SendPhotoAsync т.к. присутствует изображение погоды
                    using (var s = System.IO.File.OpenRead($@"weather_icons\{weather.weather[0].icon}.png"))
                    {
                        await bot.SendTextMessageAsync(m.From.Id, "Подождите немного, загружаю...");
                        await bot.SendChatActionAsync(m.From.Id, Telegram.Bot.Types.Enums.ChatAction.Typing);
                        try
                        {
                            await bot.SendPhotoAsync(m.From.Id, new Telegram.Bot.Types.InputFiles.InputOnlineFile(s), weatherMessage);
                        }
                        catch (Exception ex)
                        {
                            Console.WriteLine(ex.Message);
                            await bot.SendTextMessageAsync(m.From.Id, "Извините, не удлаось прислать вам картинку, повторите попытку позже");
                        }
                    }
                }));
            }
        }
        /// <summary>
        /// Обработчик события входящего на бот сообщения
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Bot_OnMessage(object sender, Telegram.Bot.Args.MessageEventArgs e)
        {

            var WeatherKey = new InlineKeyboardMarkup(new[]
           {
                new [] {InlineKeyboardButton.WithCallbackData("Moscow", "Moscow"),},
                new [] {InlineKeyboardButton.WithCallbackData("Tver", "Tver"),},
                new [] {InlineKeyboardButton.WithCallbackData("Ryazan", "Ryazan"),},
                new [] {InlineKeyboardButton.WithCallbackData("Nizhniy Novgorod", "Nizhniy Novgorod"),},
                new [] {InlineKeyboardButton.WithCallbackData("Kirov", "Kirov"),},
                new [] {InlineKeyboardButton.WithCallbackData("Samara", "Samara"),},
                new [] {InlineKeyboardButton.WithCallbackData("Ufa", "Ufa"),},
                new [] {InlineKeyboardButton.WithCallbackData("Saratov", "Saratov"),},
                new [] {InlineKeyboardButton.WithCallbackData("Kotlas", "Kotlas"),},
                new [] {InlineKeyboardButton.WithCallbackData("Chelyabinsk", "Chelyabinsk"),},
                new [] {InlineKeyboardButton.WithCallbackData("Kurgan", "Kurgan"),},
                new [] {InlineKeyboardButton.WithCallbackData("Rostov-na-Donu", "Rostov-na-Donu"),},
                new [] {InlineKeyboardButton.WithCallbackData("Kiev", "Kiev"),},
                new [] {InlineKeyboardButton.WithCallbackData("Kharkiv", "Kharkiv"),},
                new [] {InlineKeyboardButton.WithCallbackData("Syktyvkar", "Syktyvkar"),},
                new [] {InlineKeyboardButton.WithCallbackData("Yekaterinburg", "Yekaterinburg"),},
                new [] {InlineKeyboardButton.WithCallbackData("Tyumen", "Tyumen"),},
                new [] {InlineKeyboardButton.WithCallbackData("Saint Petersburg", "Saint Petersburg"),},
                new [] {InlineKeyboardButton.WithCallbackData("Petrozavodsk", "Petrozavodsk"),},
                new [] {InlineKeyboardButton.WithCallbackData("Novosibirsk", "Novosibirsk"),},
                new [] {InlineKeyboardButton.WithCallbackData("Omsk", "Omsk"),},
                new [] {InlineKeyboardButton.WithCallbackData("Orenburg", "Orenburg"),},
            }); //Кнопки с городами для запроса погоды

            incMsgSound.Play();                 //Оповещение о входящем
            Message msg = e.Message;            //Объект с данными входящего сообщения

            if (msg.Text == null) return;       //Если текста нет, возврат  


            if (!idList.Contains(msg.From.Id))//Приветсвенное сообщение, если пользователь новый
            {
                bot.SendTextMessageAsync(msg.From.Id, "Привет, " + msg.From.FirstName + "!" + "\n\"weather\" для запроса погоды");
            }
            w.Dispatcher.Invoke(() =>
            {


                if (!idList.Contains(Convert.ToInt32(msg.From.Id))) //Если пользователя не существует, то создается экземпляр MsgLog
                                                                    //и добавляется в коллекцию LastMSG
                {
                    idList.Add(msg.From.Id);
                    LastMSG.Add(new MsgLog(msg.Text, msg.Chat.FirstName, msg.Chat.Id));  //Увеличение массива логов

                    Array.Resize(ref BotMsgLog, BotMsgLog.Length + 1);
                    int index = BotMsgLog.Length;

                    BotMsgLog[index - 1] = new ObservableCollection<MsgLog>();  //Добавление ннового лога

                    BotMsgLog[index - 1].Add(new MsgLog(msg.Text, msg.Chat.FirstName, msg.Chat.Id));    //Инициализаия нового лога новым пользователем


                    usersDictionary.Add(msg.Chat.Id, index);    //Добавляю в словарь пару (индекс, присвоенный написавшему : его Id)
                                                                //чтобы потом его найти
                }
                //Если пользователь пишет не первый раз, то редактируется его экземпляр MsgLog
                //в коллекции LastMSG для отображения в ListBox последнего сообщения в главном окне
                else
                {

                    LastMSG[Convert.ToInt32(LastMSG.IndexOf(LastMSG.First(x => x.ID == msg.Chat.Id)))].Msg = msg.Text;  //Поиск совпадения по id и замена текста

                    w.ForMsgLB.ItemsSource = null;  //Обновление измененных данных на форме
                    w.ForMsgLB.ItemsSource = LastMSG;

                    //Поиск совпадения по ключу Id в словаре, чтобы знать в какую коллекцию внести лог                    
                    BotMsgLog[usersDictionary[msg.Chat.Id] - 1].Add(new MsgLog(msg.Text, msg.Chat.FirstName, msg.Chat.Id));
                }
            });

            if (msg.Text.Contains("weather"))   //Показ клавишь с городами, если пользователь ввел weather
            {
                bot.SendTextMessageAsync(msg.From.Id, text: "Выбирете категорию", replyMarkup: WeatherKey);
            }

        }
        /// <summary>
        /// Отправка сообщения конкретному пользователю
        /// </summary>
        /// <param name="text">Текст сообщения</param>
        /// <param name="Id">Id пользователя</param>
        public void SendMessage(string text, string Id)
        {
            if (Id != "")
            {
                long id = Convert.ToInt64(Id);
                bot.SendTextMessageAsync(id, text);
                w.ForInputmsgTBox.Clear();
            }
            else { System.Windows.MessageBox.Show("Выбирите получателя!"); }
        }
        #endregion
    }
}
