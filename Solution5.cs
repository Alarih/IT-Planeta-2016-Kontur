using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Diagnostics;

namespace itPlaneta2016
{
    public class Solution5
    {
        //static Stopwatch sw = new Stopwatch();

        private static int[] input;

        static void Main(string[] args)
        {
            //sw.Start();

            if (args.Length != 1)
            {
                Console.WriteLine("Пожалуйста, передайте один аргумент: Путь файла с входными параметрами");
                Console.ReadKey();
                return;
            }
            else
            {
                //считываем файл с текстом произведения
                ReadInput(args[0]);
                //выводим множества
                WriteOutput();
            }

            //sw.Stop();

            //Console.WriteLine(sw.Elapsed);
            Console.ReadKey();

        }

        private static void WriteOutput()
        {
            //вычисляем общую сумму множества
            long sum = input.Sum();
            //получаем половину от суммы
            long halfSum = sum / 2;

            List<int> out1 = new List<int>();
            List<int> refused = new List<int>();

            int counter1 = 0;
            int counterRef = 0;

            //добавлять в первое множество элементы, пока общая сумма + следующий элемент
            //не будут превышать половину от суммы общего множества
            for (int i = 0; i < input.Length; i++)
            {
                if (out1.Sum() + input[i] < halfSum)
                {
                    out1.Add(input[i]);
                    counter1++;
                }
                else
                {
                    refused.Add(input[i]);
                    counterRef++;
                }
            }

            //если сумма элементов первого множества меньше половины,
            //то добавить наименьший пропущенный элемент из общего множества,
            //и вычесть его из второго
            if (out1.Sum() < halfSum)
            {
                out1.Add(refused.Min());
                refused.Remove(refused.Min());
            }
            
            //вывод результатов на консоль
            Console.WriteLine(out1.Count);
            foreach (int item in out1)
            {
                Console.WriteLine(item);
            }
            Console.WriteLine(refused.Count);
            foreach (int item in refused)
            {
                Console.WriteLine(item);
            }

        }

        private static void ReadInput(string path)
        {
            using (FileStream fs = File.Open(path, FileMode.Open, FileAccess.Read, FileShare.ReadWrite))
            using (BufferedStream bs = new BufferedStream(fs))
            using (StreamReader reader = new StreamReader(bs))
            {
                reader.ReadLine();

                input = new int[Convert.ToInt16(reader.ReadLine())];

                int counter = 0;
                do
                {
                    input[counter] = Convert.ToInt16(reader.ReadLine());
                    counter++;

                } while (!reader.EndOfStream);
            }
        }

    }
}
