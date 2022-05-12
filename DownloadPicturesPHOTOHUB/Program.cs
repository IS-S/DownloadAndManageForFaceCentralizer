using System;
using DP.Core;
using DP.DataProvider;

namespace DownloadPicturesPHOTOHUB
{
    class Program
    {
        static void Main(string[] args)
        {

            bool cont = true;

            Console.WriteLine("Select option: \n1. Download photos to new folder\n2. Centralize downloaded photos");
            string choice = Console.ReadLine();
            PhotoManager PM = new PhotoManager();

            while (cont)
            {
                switch (choice)
                {
                    case "1":
                        
                        Console.WriteLine("Start successful");
                        PM.SetData("C:/Users/i.sumin/Pictures/list.csv");
                        Console.WriteLine("Download finished...");
                        cont = false;
                        break;
                    case "2":
                        Console.WriteLine("\nCentralizing...");
                        PM.Centralize();
                        cont = false;
                        break;
                    default:
                        Console.WriteLine("Unprocessable input");
                        break;
                }
            }

            
        }
    }
}
