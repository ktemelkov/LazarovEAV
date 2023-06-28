using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LazarovEAV.Model;

namespace DbConverter
{
    class Program
    {
        static void Main(string[] args)
        {
            using (var ctx = new EntityManager())
            {
                ConvertTestResultData<TestResultLeft>(ctx);
                ConvertTestResultData<TestResultRight>(ctx);

                Console.WriteLine("\r\nDone.");
            }
        }

        static void ConvertTestResultData<TEntity>(EntityManager ctx) where TEntity : TestResult
        {
            Console.WriteLine("Loading database ...");

            var results = ctx.loadItems<TEntity>();
            int progress = 0;

            Console.WriteLine("Processing records ...");

            foreach (var res in results)
            {
                if (res.Data.StartsWith("[{"))
                {
                    res.Data = Compress(res.Data);
                }

                progress++;

                if (progress % 100 == 0)
                {
                    Console.Write(".");
                }
            }

            Console.WriteLine("\r\nSaving records ...");
            ctx.saveChanges();
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        public static string Compress(string json)
        {
            byte[] data = Encoding.ASCII.GetBytes(json);

            using (MemoryStream output = new MemoryStream())
            {
                using (DeflateStream dstream = new DeflateStream(output, CompressionLevel.Optimal))
                {
                    dstream.Write(data, 0, data.Length);
                }

                return System.Convert.ToBase64String(output.ToArray());
            }
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        public static string Decompress(string base64data)
        {
            byte[] data = System.Convert.FromBase64String(base64data);

            using (MemoryStream input = new MemoryStream(data))
            {
                using (MemoryStream output = new MemoryStream())
                {
                    using (DeflateStream dstream = new DeflateStream(input, CompressionMode.Decompress))
                    {
                        dstream.CopyTo(output);
                    }

                    return Encoding.ASCII.GetString(output.ToArray());
                }
            }
        }
    }
}
