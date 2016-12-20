using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AES
{
    class Program
    {
        static void Main(string[] args)
        {
            string tmp_msg = "Hello world!";
            string tmp_key = "Key";
            char mode;
            char input;

            Console.Write("Mode (e - encrypt, d - decrypt, x - exit): ");
            mode = Console.ReadKey().KeyChar;
            Console.Write("\nMessage (c - console input, f - file input): ");
            input = Console.ReadKey().KeyChar;
            Console.Write("\nMessage: ");
            tmp_msg = Console.ReadLine();
            Console.Write("Key: ");
            tmp_key = Console.ReadLine();

            while(tmp_key.Count() < 4)
            {
                tmp_key += 0;
            }

            if(input == 'f')
            {
                using(StreamReader reader = new StreamReader(new FileStream(tmp_msg, FileMode.Open), Encoding.Default))
                {
                    tmp_msg = reader.ReadToEnd();
                    Console.WriteLine("Message: " + tmp_msg);
                }
            }

            Console.WriteLine();

            byte[] msg = Encoding.Default.GetBytes(tmp_msg);
            byte[] key = Encoding.Default.GetBytes(tmp_key);

            byte[] result = { };

            /*result = Rijndael.encrypt(msg, key);
            Console.WriteLine(Encoding.Default.GetString(result));

            result = Rijndael2.encrypt(msg, key);
            Console.WriteLine(Encoding.Default.GetString(result));

            result = DRijndael.decrypt(result, key);
            Console.WriteLine(Encoding.Default.GetString(result));*/

            switch (mode)
            {
                case 'e':
                    result = Rijndael2.encrypt(msg, key);
                    Console.WriteLine(Encoding.Default.GetString(result));
                    break;
                case 'd':
                    result = DRijndael.decrypt(msg, key);
                    Console.WriteLine(Encoding.Default.GetString(result));
                    break;
                case 'x': return;
            }

            using (StreamWriter writer = new StreamWriter(new FileStream("result.dat", FileMode.Create), Encoding.Default))
            {
                writer.Write(Encoding.Default.GetString(result));
            }

            Console.ReadKey();
        }
    }
}
