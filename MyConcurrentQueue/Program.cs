using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace MyConcurrentQueue
{
    class Program
    {
        private static volatile bool readingIsPossible = true;

        static void Main(string[] args)
        {
            string str = "1234567890";

            char[] chars1 = str.ToCharArray();

            str = "abcdefghijk";

            char[] chars2 = str.ToCharArray();

            str = "ABCDEFGHIJK";

            char[] chars3 = str.ToCharArray();

            IQueue<char> charsQueue = new MultiThreadedQueue<char>();

            //Parallel.Invoke( 
            // () => AddCharsToQueue(chars1, charsQueue), 
            // () => AddCharsToQueue(chars2, charsQueue), 
            // () => AddCharsToQueue(chars3, charsQueue) 
            // ); 

            Task.Run(() => AddCharsToQueue(chars1, charsQueue));
            Task.Run(() => AddCharsToQueue(chars2, charsQueue));
            Task.Run(() => AddCharsToQueue(chars3, charsQueue));
            Task.Run(() => ReadCharsAndDisplay(charsQueue));
            Task.Run(() => ReadCharsAndDisplay(charsQueue));
            Task.Run(() => ReadCharsAndDisplay(charsQueue));

            // Чтобы остановить чтение в холостую, нажмите любую клавишу 
            Console.WriteLine("Press any key to stop reading");
            Console.WriteLine();
            Console.ReadKey();
            Console.WriteLine();

            readingIsPossible = false;

            //string newStr = new string(charsQueue.ToArray()); 
            //Console.WriteLine(newStr); Console.ReadKey(); 

            Console.ReadKey();
        }

        static void AddCharsToQueue(char[] chars, IQueue<char> charsQueue)
        {
            foreach (var item in chars)
            {
                charsQueue.Push(item);

                // Приостановка для наглядности, чтобы значения были перемешанными 
                Thread.Sleep(250);
            }
        }

        static void ReadCharsAndDisplay(IQueue<char> charsQueue)
        {
            while (readingIsPossible)
            {
                char symbol;

                try
                {
                    symbol = charsQueue.Pop();
                    Console.Write("{0} ", symbol);
                }
                catch (TimeoutException ex)
                {
                    // TODO: Здесь могло быть логирование 
                    //Console.WriteLine(ex.Message); 
                }
            }
        }
    }
}
