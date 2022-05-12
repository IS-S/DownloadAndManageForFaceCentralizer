using System;
using System.IO;
using DP.DataProvider;

namespace DP.Core
{
    public class PhotoManager
    {
        public PhotoManager()
        {
        }

        public void SetData(string path)
        {
            PhotoDataProvider DP = new PhotoDataProvider();
            var names = DP.GetNames(path);

            int counter = 1;

            foreach (string name in names)
            {
                
                if (name.Contains(".jpeg"))
                {
                    DP.GetPhoto(name);
                    Console.WriteLine("Downloaded Photo #" + counter) ;
                    counter++;
                }
            }
            
        }
        public void Centralize()
        {
            PhotoDataProvider DP = new PhotoDataProvider();

            var targets = Directory.GetFiles("*path*/", "*.jpeg");
            Console.WriteLine("\nFound " + targets.Length + " files. Initializing centralizing process");

            int counter = 0, errCounter = 0;
            bool fail = false;


            foreach (string str in targets)
            {
                switch (DP.CentralizeCommand(str))
                {
                    case "OK":
                        counter++;
                        Console.WriteLine("Successfully centralized " + counter + " photo...");
                        break;

                    case "Face not found":

                        errCounter++;

                        Console.WriteLine("Photo " + str.Substring(str.LastIndexOf("/") + 1) + " cannot be centralized. Face has not been locked.");
                        DP.SaveFails(errCounter, str, "Face not found");

                        if (!fail)
                        {
                            fail = true;
                        }


                        break;

                    case "Fail":

                        errCounter++;

                        DP.SaveFails(errCounter, str.Substring(str.LastIndexOf("/") + 1), "Unknown error (CLIENT)");

                        if (!fail)
                        {
                            fail = true;
                        }

                        break;
                    case "Internal error":

                        errCounter++;

                        DP.SaveFails(errCounter, str.Substring(str.LastIndexOf("/") + 1), "Unknown error (SERVER)");

                        if (!fail)
                        {
                            fail = true;
                        }

                        break;
                }
            }
            if (fail)
            {
                Console.WriteLine("Successfully centralized " + counter + " photos. Check directory...");

            }
            else
            {
                Console.WriteLine("Successfully centralized all photos. Check directory...");
            }
        }
    }
}
