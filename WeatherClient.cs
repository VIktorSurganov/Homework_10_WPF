using Newtonsoft.Json;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using System.Windows.Media.Imaging;

namespace WPF_SomeName200_bot
{
    /// <summary>
    /// Класс для создания объекта на основе полученного ответа от сервера сайта погоды
    /// </summary>
    public class WeatherClient
    {
        #region Подклассы
        public class OpenWeather
        {
            public coord coord;
            public weather[] weather;
            [JsonProperty("base")]
            public string Base;
            public main main;
            public double visibility;
            public wind wind;
            public clouds clouds;
            public double dt;
            public sys sys;
            public double timezone;
            public int id;
            public string name;
            public double cod;
        }
        public class coord
        {
            public double lon;
            public double lat;
        }
        public class weather
        {
            public int id;
            public string main;
            public string description;
            public string icon;

            public Bitmap Icon { get { return new Bitmap(Image.FromFile($@"weather_icons\{icon}.png")); } }
            /// <summary>
            /// Метод для преобразования Bitmap в BitmapImage для размещения изображения на форме в качетсве Image.Source.
            /// </summary>
            /// <param name="bitmap"></param>
            /// <returns></returns>
            public static BitmapImage BitmapToImageSource(Bitmap bitmap)
            {
                using (MemoryStream memory = new MemoryStream())
                {
                    bitmap.Save(memory, System.Drawing.Imaging.ImageFormat.Bmp);
                    memory.Position = 0;
                    BitmapImage bitmapImage = new BitmapImage();
                    bitmapImage.BeginInit();
                    bitmapImage.StreamSource = memory;
                    bitmapImage.CacheOption = BitmapCacheOption.OnLoad;
                    bitmapImage.EndInit();
                    return bitmapImage;
                }
            }
        }
        public class main
        {
            private double temp;
            private double feels_like;
            private double temp_min;
            private double temp_max;
            private double pressure;
            public double humidity;


            public double Temp { get { return temp; } set { temp = value - 273.15; } }
            public double FeelsLike { get { return feels_like; } set { feels_like = value - 273.5; } }
            public double MinTemp { get { return temp_min; } set { temp_min = value - 273.15; } }
            public double MaxTemp { get { return temp_max; } set { temp_max = value - 273.15; } }
            public double Pressure { get { return pressure; } set { pressure = value / 1.3332239; } }


        }
        public class wind
        {
            public double speed;
            public double deg;
            public double gust;
        }
        public class clouds
        {
            public double all;
        }
        public class sys
        {
            public double type;
            public int id;
            public string country;
            public double sunrise;
            public double sunset;
        }
        #endregion

        #region Поля и свойства
        static string weatherKey;   //Ключ для доступа к серверу
        static string weatherUrl = "http://api.openweathermap.org/data/2.5/weather?q="; //Ссылка на запрос погоды без ключа и города
        MainWindow w;   //Объект главного окна
        public OpenWeather ow;  //Объект для записи полученных данных
        public List<string> citiesCollection;   //Список доступных для запроса городов
        public static string wKey //Свойство для получения ключа из файла
        {
            get
            {
                weatherKey = System.IO.File.ReadAllText(@"weatherKey.txt"); return weatherKey;
            }
            set
            {
                wKey = value;
            }
        }
        #endregion

        #region Конструктор
        public WeatherClient(MainWindow w)
        {
            citiesCollection = new List<string>();
            citiesCollection.AddRange(new string[] { "Moscow", "Tver", "Ryazan", "Nizhniy Novgorod", "Kirov", "Samara", "Ufa", "Saratov", "Kotlas", "Chelyabinsk", "Kurgan", "Rostov-na-Donu", "Kiev", "Kharkiv", "Syktyvkar", "Yekaterinburg", "Tyumen", "Saint Petersburg", "Petrozavodsk", "Novosibirsk", "Omsk", "Orenburg" });
            citiesCollection = citiesCollection.OrderBy(n => n).ToList();
            this.w = w;
        }
        #endregion

        /// <summary>
        /// Асинхронный метод получения ответа на запрос погоды по городу
        /// </summary>
        /// <param name="city"></param>
        /// <returns></returns>
        public static async Task<string> GetWeather(string city)
        {
            string targetRequest = $"{weatherUrl}{city}&APPID={wKey}";  //Строка для запроса с адресом сайта, городом и ключем            
            string answer = string.Empty; //Строка для ответа
            //Запрос и его свойства
            WebRequest request = WebRequest.Create(targetRequest);
            request.Method = "POST";
            request.ContentType = "application/x-www-urlencoded";
            WebResponse response = await request.GetResponseAsync(); //Получение ответа
            //Поток принимает ответ
            using (System.IO.Stream stream = response.GetResponseStream())
            {
                //Читаем поток ответа и вносим его в строку для овтета
                using (System.IO.StreamReader streamReader = new System.IO.StreamReader(stream))
                {
                    answer = await streamReader.ReadToEndAsync();
                }
            }
            response.Close();
            return answer;
        }
    }
}
