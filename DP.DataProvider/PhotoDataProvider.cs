using System.IO;
using System.Collections.Generic;
using System.Net;
using System.Drawing;
using System.Drawing.Imaging;

namespace DP.DataProvider
{
    public class PhotoDataProvider
    {
        public void GetPhoto(string name)
        {
            //ссылка
            byte[] photobytes = null;
            using (WebClient client = new WebClient())
            {
                photobytes = client.DownloadData("*url*/" + name);

                using (MemoryStream mem = new MemoryStream(photobytes))
                {
#pragma warning disable CA1416 // Validate platform compatibility
                    using (var yourImage = Image.FromStream(mem))
                    {


                        yourImage.Save("*path*/" + name, ImageFormat.Jpeg);

#pragma warning restore CA1416 // Validate platform compatibility
                    }
                }
            }
        }

        public List<string> GetNames(string csvPath)
        {
            List<string> data = new List<string>();

            using (StreamReader sr = new StreamReader(csvPath))
            {
                string line;
                while ((line = sr.ReadLine()) != null)
                {
                    data.Add(line);
                }
            }

            return data;
        }

        public string CentralizeCommand(string name)
        {
            WebRequest reqPOST = WebRequest.Create(@"https://localhost:44358/centralize_photo/post_data?path=" + name);
            reqPOST.Method = "POST";
            reqPOST.Timeout = 120000;
            reqPOST.ContentType = "application/json";

            try
            {
                HttpWebResponse response = reqPOST.GetResponse() as HttpWebResponse;

                if(response.StatusCode == HttpStatusCode.OK)
                {
                    return "OK";
                }
                else
                {
                    return "Fail";
                }
            }

            catch (WebException webExcp)
            {
                int status = 0;
                if (webExcp.Status == WebExceptionStatus.ProtocolError)
                {
                    // Get HttpWebResponse so that you can check the HTTP status code.  
                    HttpWebResponse httpResponse = (HttpWebResponse)webExcp.Response;
                    status = (int)httpResponse.StatusCode;
                }
                switch (status)
                {
                    case 404:
                        return "Face not found";
                    case 500:
                        return "Internal error";
                    default:
                        return "Fail";
                }
            }
        }
        public void SaveFails(int num, string name, string reason)
        {
            //Pass the filepath and filename to the StreamWriter Constructor
            StreamWriter sw = new StreamWriter("*path*/fails3.txt", true);
            //Write a line of text
            sw.WriteLine(num.ToString() + ". " + name + " : " + reason);
            //Close the file
            sw.Close();
        }
    }

}
