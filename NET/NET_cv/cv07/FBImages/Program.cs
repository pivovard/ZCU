using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net;
using System.Diagnostics;

namespace Kiv.Net
{
    /// <summary>
    /// Program najde na FB nejmensi obrazek, ktery je vetsi nez zadane rozmery nebo nejvetsi z obrazku. 
    /// Url nalezeneho obrazku otevre ve vychozim programu.
    /// </summary>
    class FaceBookImages
    {
        /// <summary>
        /// FB App Id pro predmet kiv/net
        /// </summary>
        static string NetAppId = "237542193002558";

        /// <summary>
        /// FB App Secret pro predmet kiv/net
        /// </summary>
        static string NetAppSecret = "b21b1cd48f32ab3195b63b959903fc72";

        /// <summary>
        /// Pozadovana maximalni sirka obrazku
        /// </summary>
        static int Width = 580;

        /// <summary>
        /// Pozadovana maximalni vyska obrazku
        /// </summary>
        static int Height = 300;

        /// <summary>
        /// FB id obrazku
        /// </summary>
        static string ImageID = "552323464903980";

        /// <summary>
        /// Vstupni bod programu. 
        /// </summary>
        /// <param name="args">Nepouzivaji se</param>
        static void Main(string[] args)
        {
            FaceBookReader FBreader = new FaceBookReader(NetAppId, NetAppSecret);

            Console.WriteLine("Pozadovana minimalni velikost: ");
            Console.WriteLine($"{Width} x {Height}");

            Console.WriteLine("Stahuji data: ");
            dynamic jsonImages = (FBreader.GetData(ImageID).images);

            Console.WriteLine("Prevadim data: ");
            List<Image> images = new List<Image>();
            foreach (var image in jsonImages)
            {
                Console.WriteLine("{0} x {1}", image.width, image.height);
                images.Add(new Image() { Width = image.width, Height = image.height, Url = image.source });
            }

            images.Sort((a, b) => b.Width.CompareTo(a.Width));

            Image small = GetSmaller(Width, Height, images);
            Console.WriteLine("Vybral jsem: ");
            Console.WriteLine("{0} x {1}", small.Width, small.Height);

            Process.Start(small.Url);

            Console.ReadLine();
        }

        /// <summary>
        /// Metoda najde ze seznamu obrazku nejmensi obrazek, ktery ma vysku i sirku vetsi nez zadane 
        /// parametry height, width nebo nejvetsi z obrazku.
        /// </summary>
        /// <param name="width">Maximalni sirka obrazku</param>
        /// <param name="height">Maximalni vyska obrazku</param>
        /// <param name="images">Seznam obrazku setrizeny od nejvetsich po nejmensi</param>
        /// <returns>Vybrany obrazek</returns>
        static Image GetSmaller(int width, int height, List<Image> images)
        {
            Image last = images[0];
            double ratio = width/height;
            foreach(var image in images) {
                if(image.Width>=width && image.Height >= height) {
                    last = image;
                }
            }
            return last;
            throw new ApplicationException("Nenasel jsem mensi obrazek...");
        }
    }


    /// <summary>
    /// Struktura pro ulozeni obrazku.
    /// </summary>
    struct Image
    {
        /// <summary>
        /// Sirka
        /// </summary>
        public int Width { get; set; }

        /// <summary>
        /// Vyska
        /// </summary>
        public int Height { get; set; }        

        /// <summary>
        /// Adresa obrazku
        /// </summary>
        public string Url { get; set; }

        /// <summary>
        /// Pomer stran
        /// </summary>
        public double Ratio { get { return Width / Height;  } }
    }

    /// <summary>
    /// Jednoducha trida pro stahovani FB dat
    /// </summary>
    class FaceBookReader
    {
        /// <summary>
        /// FB App Id
        /// </summary>
        public string AppId { get; private set; }

        /// <summary>
        /// FB App Secret
        /// </summary>
        public string AppSecret { get; private set; }

        /// <summary>
        /// Vytvori instanci FB readeru a nastavi App Id a App Secret
        /// </summary>
        /// <param name="AppId">FB App Id</param>
        /// <param name="AppSecret">FB App Secret</param>
        public FaceBookReader(string AppID, string AppSecret)
        {
            this.AppId = AppID;
            this.AppSecret = AppSecret;            
        }

        /// <summary>
        /// Stahne data podle FB id
        /// </summary>
        /// <param name="id">FB id</param>
        /// <returns>Json</returns>
        public dynamic GetData(string id)
        {
            WebClient client = new WebClient();
            string http = string.Format("https://graph.facebook.com/v2.2/{0}?access_token={1}|{2}", 
                id, AppId, AppSecret);
            string jsonResult="";
            try
            {
                jsonResult = client.DownloadString(http);
            }
            catch
            {

            }
            var result = System.Web.Helpers.Json.Decode(jsonResult);
            return result;

        }
    }
}
