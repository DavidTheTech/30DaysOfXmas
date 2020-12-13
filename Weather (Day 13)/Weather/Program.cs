using System;
using System.Collections.Generic;
using System.Threading;
using System.Net;
using Newtonsoft.Json;

namespace art
{
    class Program
    {
        static void Main(string[] args)
        {
            double Temp = 0;
            int Humidity = 0;
            double Wind = 0;
            string Country = "";
            string Name = "";
            int Clouds = 0;

            using (WebClient wc = new WebClient())
            {
                //Get a new api key
                var json = wc.DownloadString("http://api.openweathermap.org/data/2.5/weather?q=London,uk&appid=APIKEYGOESHERE&mode=json");
                var obj = JsonConvert.DeserializeObject<OpenWeatherMap.Root>(json);

                Temp = obj.main.temp - 273.16;
                Humidity = obj.main.humidity;
                Wind = obj.wind.speed;
                Country = obj.sys.country;
                Name = obj.name;
                Clouds = obj.clouds.all;
            }
            
            DateTime now = DateTime.Now;
            writeSlow("Hello", 50);
            writeSlow("\n\n\n", 50);
            writeSlow("Time : " + now, 50);
            Console.WriteLine();
            Console.WriteLine();
            writeSlow("To Days Weather Forecast", 50);
            Console.WriteLine();
            writeSlow("Country : " + Name + ", " + Country, 50);
            Console.WriteLine();
            writeSlow("Temperature : " + Temp.ToString("N2") + " Celcius", 50);
            Console.WriteLine();
            writeSlow("Humidity : " + Humidity + "%", 50);
            Console.WriteLine();
            writeSlow("Wind : " + Wind, 50);
            Console.WriteLine();
            writeSlow("Clouds : " + Clouds, 50);
            Console.ReadKey();
        }

        private static void writeSlow(string text, int speed)
        {
            char[] pieces = text.ToCharArray();
            foreach (char single in pieces)
            {
                Console.Write(single);
                Thread.Sleep(speed);
            }
        }

        public class OpenWeatherMap
        {
            public class Coord
            {
                public double lon { get; set; }
                public double lat { get; set; }
            }

            public class Sys
            {
                public int type { get; set; }
                public int id { get; set; }
                public double message { get; set; }
                public string country { get; set; }
                public int sunrise { get; set; }
                public int sunset { get; set; }
            }

            public class Weather
            {
                public int id { get; set; }
                public string main { get; set; }
                public string description { get; set; }
                public string icon { get; set; }
            }

            public class Main
            {
                public double temp { get; set; }
                public int humidity { get; set; }
                public double pressure { get; set; }
                public double temp_min { get; set; }
                public double temp_max { get; set; }
            }

            public class Wind
            {
                public double speed { get; set; }
                public double gust { get; set; }
                public int deg { get; set; }
            }

            public class Clouds
            {
                public int all { get; set; }
            }

            public class Root
            {
                public Coord coord { get; set; }
                public Sys sys { get; set; }
                public List<Weather> weather { get; set; }
                public string @base { get; set; }
                public Main main { get; set; }
                public Wind wind { get; set; }
                public Dictionary<string, double> rain { get; set; }
                public Clouds clouds { get; set; }
                public int dt { get; set; }
                public int id { get; set; }
                public string name { get; set; }
                public int cod { get; set; }
            }
        }
    }
}
