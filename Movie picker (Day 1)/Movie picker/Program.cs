using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;
using System.IO;

namespace Movie_picker
{
    class Program
    {
        static Random rand = new Random();

        static void Main(string[] args)
        {
            Console.Title = "Movie Picker";

            int TotalMovies = 0;

            List<string> MovieList = new List<string>()
            {

            };

            if (!File.Exists("movies.txt"))
            {
                Console.WriteLine("movies.txt doesn't exist");
                goto end;
            }

            string[] MovieNames = File.ReadAllLines("movies.txt");

            foreach (var a in MovieNames)
            {
                if (!a.ToLower().Contains("finished"))
                {
                    MovieList.Add(a);
                    TotalMovies++;
                }
            }

            foreach(var Cur in MovieList)
            {
                Console.Clear();
                Console.Write(MovieList[rand.Next(0, TotalMovies)]);
                Thread.Sleep(50);
            }

            end:

            Console.ReadKey();
        }
    }
}
