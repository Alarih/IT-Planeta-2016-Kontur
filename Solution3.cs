using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

using System.Diagnostics;

namespace itPlaneta2016
{

    public struct Dispatches
    {
        public int Id;
        public int WareId;
        public DateTime StartTime;
        public DateTime EndTime;

        //продолжительность обработки товара в минутах
        public double Time { get { return (EndTime - StartTime).Hours * 60 + (EndTime - StartTime).Minutes; } } 

    }

    public struct Wares
    {
        public int WareId;
        public string WareName;

    }

    public struct Report
    {
        public int Id;
        public string WareName;

        /// <summary>
        /// Среднее значение
        /// </summary>
        public double Mean;

        /// <summary>
        /// Среднеквадратичное отклонение
        /// </summary>
        public double Deviation;

        /// <summary>
        /// Максимум времени обработки
        /// </summary>
        public double Max;
    }

    public class CVSHandler
    {
        private static string currentDirectory = Path.GetDirectoryName(Assembly.GetEntryAssembly().Location);

        private const string DISPATCHES = "\\dispatches.txt";
        private const string DISPATCHES_HEADER = "id,ware_id,start_time,end_time";
        private static List<Dispatches> dispList = new List<Dispatches>();

        private const string WARES = "\\wares.txt";
        private const string WARES_HEADER = "ware_id,ware_name";
        private static List<Wares> waresList = new List<Wares>();

        private const string REPORT = "\\report.txt";
        private const string REPORT_HEADER = "id,ware_name,mean,deviation,max";
        private static List<Report> reportList = new List<Report>();

        static void Main(string[] args)
        {
            GenerateReport();
            Console.WriteLine("Отчет был успешно сгенерирован в файл {0}", REPORT);
            Console.ReadKey();

        }

        public static void GenerateReport()
        {
            ReadDispatches();
            ReadWares();

            foreach (Wares item in waresList)
            {
                Report repElement = new Report();

                repElement.Id = item.WareId;
                repElement.WareName = item.WareName;

                //вычисление Среднего значения
                repElement.Mean = dispList.Where(x => x.WareId == item.WareId).Average(x => x.Time);
                //вычисление Среднеквадратичного отклонения
                repElement.Deviation = StandardDeviation(dispList.Where(x => x.WareId == item.WareId).Select(x => x.Time));
                //вычисление Максимума времени обработки
                repElement.Max = dispList.Where(x => x.WareId == item.WareId).Max(x => x.Time);

                reportList.Add(repElement);

            }

            NumberFormatInfo dotDoubleFormat = new NumberFormatInfo();
            dotDoubleFormat.NumberDecimalSeparator = ".";

            using (StreamWriter reportStream = new StreamWriter(File.Open(currentDirectory + REPORT, FileMode.Create), Encoding.UTF8))
            {
                reportStream.WriteLine(REPORT_HEADER);

                foreach (Report item in reportList)
                {
                    reportStream.WriteLine("{0},{1},{2},{3},{4}", item.Id.ToString(), item.WareName, FormatDouble(item.Mean), FormatDouble(item.Deviation), FormatDouble(item.Max));
                }
            }
        }

        private static string FormatDouble(double num)
        {
            string formated = String.Format("{0:0.000}", Math.Round(num, 3));
            return formated.Replace(',', '.');
        }

        private static double StandardDeviation(IEnumerable<double> values)
        {
            double avg = values.Average();
            return Math.Sqrt(values.Average(v => Math.Pow(v - avg, 2)));

        }

        /// <summary>
        /// Чтение файла dispatches.txt
        /// </summary>
        private static void ReadDispatches()
        {
            Stopwatch sw = new Stopwatch();
            sw.Start();

            using (FileStream fs = File.Open(currentDirectory + DISPATCHES, FileMode.Open, FileAccess.Read, FileShare.ReadWrite))
            using (BufferedStream bs = new BufferedStream(fs))
            using (StreamReader dispStream = new StreamReader(bs))
            {
                do
                {
                    string line = dispStream.ReadLine();

                    //пропуск заголовка
                    if (!line.Equals(DISPATCHES_HEADER))
                    {
                        //парсинг строки
                        ParseDispathes(line);

                    }

                } while (!dispStream.EndOfStream);
                
            }

            sw.Stop();
            Console.WriteLine(sw.Elapsed);
        }

        /// <summary>
        /// Чтение файла wares.txt
        /// </summary>
        private static void ReadWares()
        {
            using (StreamReader waresStream = new StreamReader(currentDirectory + WARES))
            {
                do
                {
                    string line = waresStream.ReadLine();

                    //пропуск заголовка
                    if (!line.Equals(WARES_HEADER))
                    {
                        //парсинг строки
                        ParseWares(line);

                    }

                } while (!waresStream.EndOfStream);

            }
        }

        private static void ParseDispathes(string line)
        {           
            string[] words = line.Split(',');

            Dispatches newLine = new Dispatches();

            newLine.Id = TryParseToInt(words[0]);
            newLine.WareId = TryParseToInt(words[1]);
            newLine.StartTime = TryParseToDateTime(words[2]);
            newLine.EndTime = TryParseToDateTime(words[3]);

            dispList.Add(newLine);
        }

        private static void ParseWares(string line)
        {
            string[] words = line.Split(',');

            Wares newLine = new Wares();

            newLine.WareId = TryParseToInt(words[0]);
            newLine.WareName = words[1];

            waresList.Add(newLine); 
        }

        private static int TryParseToInt(string word)
        {
            int outputInt;
            int.TryParse(word, out outputInt);
            return outputInt;

        }

        private static DateTime TryParseToDateTime(string word)
        {
            DateTime outputDateTime;
            DateTime.TryParseExact(word, "dd.MM.yyyy HH:mm:ss.fff", CultureInfo.InvariantCulture, DateTimeStyles.None, out outputDateTime);
            return outputDateTime;

        }

    }
}
