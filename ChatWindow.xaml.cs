using System.Windows;
using System.Windows.Input;

namespace WPF_SomeName200_bot
{
    public partial class ChatWindow : Window
    {
        public long UserId { get; set; }

        #region Конструктор
        /// <summary>
        /// Перед созданием окна создается объект MsgLog и передаются поля Name и Id для заголовка
        /// </summary>
        /// <param name="name"></param>
        /// <param name="id"></param>
        public ChatWindow(string name, long id)
        {
            UserId = id;
            InitializeComponent();
            forChatLB.ItemsSource = MainWindow.client.BotMsgLog[TgMsgClient.usersDictionary[id] - 1];
            ChatWithLBL.Content = $"Chat with: {name} ({id})";
        }
        #endregion

        #region Обработка событий
        //Событие кнопки отправки сообщения
        private void SendMsg_Click(object sender, RoutedEventArgs e)
        {
            //Отправка
            MainWindow.client.SendMessage(forMsgTextBox.Text, UserId.ToString());
            //Добавление в лог
            MainWindow.client.BotMsgLog[TgMsgClient.usersDictionary[UserId] - 1].Add(new MsgLog(forMsgTextBox.Text, "СомНэйм", MainWindow.client.bot.BotId));
            //Очистка текстбокса
            forMsgTextBox.Clear();
        }
        //Прямоугольник для перетаскивания
        private void Rectangle_MouseDown(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            if (e.ChangedButton == MouseButton.Left)
                this.DragMove();
        }
        //Закрыте
        private void CloseChatWindowBTN_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
        //Удаление id из списка открытых окон
        private void Window_Closed(object sender, System.EventArgs e)
        {
            MainWindow.ChatWindowOpen.Remove(this.UserId);
        }
        #endregion
    }
}
