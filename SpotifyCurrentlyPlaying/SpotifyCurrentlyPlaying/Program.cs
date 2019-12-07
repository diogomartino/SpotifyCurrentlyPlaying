using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace SpotifyCurrentlyPlaying
{
    class Program
    {
        private static string musicName = string.Empty;
        private static HttpListener listener;
        private static string url = "http://localhost:5454/";

        private static void Main(string[] args)
        {
            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine("Choose an option:");
            Console.WriteLine("(1) Web Server");
            Console.WriteLine("(2) Text File");

            int op = int.Parse(Console.ReadLine());

            if(op == 1) {
                if (!File.Exists("display.html"))
                {
                    Console.WriteLine("display.html not found");
                    Console.ReadKey();
                    return;
                }

                Console.Clear();
                Console.WriteLine("Listening on: " + url);
                Thread t = new Thread(RunServer);
                t.Start();
            }
            else
            {
                Console.WriteLine("Enter the update time (in seconds). Leave blank for default 3 seconds.");
                int secs = 0;

                try
                {
                    secs = int.Parse(Console.ReadLine());
                } 
                catch
                {
                    secs = 3;
                }

                while (true)
                {
                    musicName = GetMusicName();
                    Console.Clear();
                    Console.WriteLine("Currently playing: " + musicName);
                    File.WriteAllText("playing.txt", musicName);
                    Thread.Sleep(secs * 1000);
                }
            }
        }

        public static void RunServer()
        {
            listener = new HttpListener();
            listener.Prefixes.Add(url);
            listener.Start();

            Task listenTask = HandleIncomingConnections();
            listenTask.GetAwaiter().GetResult();

            listener.Close();
        }

        public static string GetMusicName()
        {
            string temp = "";

            try
            {
                foreach (Process p in Process.GetProcessesByName("spotify"))
                {
                    string title = p.MainWindowTitle.Trim();

                    if (!string.IsNullOrEmpty(title))
                    {
                        if (title == "Spotify Premium" || title == "Spotify")
                        {
                            temp = "";
                        }
                        else
                        {
                            temp = title;
                        }
                        break;
                    }
                }
            }
            catch
            {
                temp = "";
            }

            return temp;
        }

        public static async Task HandleIncomingConnections()
        {
            while (true)
            {
                HttpListenerContext ctx = await listener.GetContextAsync();
                HttpListenerResponse resp = ctx.Response;

                musicName = GetMusicName();

                string page = string.Empty;

                if(string.IsNullOrEmpty(musicName))
                {
                    page = File.ReadAllText("default.html");
                }
                else
                {
                    page = File.ReadAllText("display.html").Replace("{MUSIC_NAME}", musicName);
                }

                byte[] data = Encoding.UTF8.GetBytes(page);
                resp.ContentType = "text/html";
                resp.ContentEncoding = Encoding.UTF8;
                resp.ContentLength64 = data.LongLength;

                await resp.OutputStream.WriteAsync(data, 0, data.Length);
                resp.Close();
            }
        }
    }
}
