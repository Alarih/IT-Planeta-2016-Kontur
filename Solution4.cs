using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace itPlaneta2016
{
    public class Solution4
    {
        private static List<string> textList = new List<string>();
        private static List<string> queryList = new List<string>();

        static void Main(string[] args)
        {
            if (args.Length < 2)
            {
                Console.WriteLine("Пожалуйста, передайте два аргумента: 1 - Путь файла с произведением 2 -  Путь файла с поисковыми запросами");
                Console.ReadKey();
                return;
            }
            else
            {
                //считываем файл с текстом произведения
                string pathText = args[0];
                ReadFileToList(pathText, textList);

                //считываем файл с запросами
                string pathQuery = args[1];
                ReadFileToList(pathQuery, queryList);

                //начать обработку запросов 
                ProcessingQuery();

                Console.ReadKey();
            }

        }

        /// <summary>
        /// Обработать запросы из списка с запросами
        /// </summary>
        private static void ProcessingQuery()
        {
            foreach (string query in queryList)
            {
                string formattedQuery = query;

                int spaceIndex = query.IndexOf(' ');
                if (spaceIndex > 0)
                {
                    formattedQuery = query.Substring(0, spaceIndex);
                }
                
                CompareQueryWithText(formattedQuery);
            }

        }

        /// <summary>
        /// Сравнение запроса с каждой строкой текста
        /// </summary>
        /// <param name="query"></param>
        private static void CompareQueryWithText(string query)
        {
            int numLine = 0;
            StringBuilder reportLine = new StringBuilder();

            foreach (string textLine in textList)
            {
                if (FindAllOccurences(query, textLine))
                {
                    reportLine.Append(numLine + ",");
                }
                
                numLine++;

                if (numLine > 20)
                {
                    break;
                }
            }

            if (reportLine.Length > 0)
            {
                reportLine.Remove(reportLine.Length - 1, 1);
                Console.WriteLine(reportLine);
            }
            
        }

        /// <summary>
        /// Поиск повторений в строке
        /// </summary>
        /// <returns></returns>
        private static bool FindAllOccurences(string query, string textLine)
        {
            string[] splittedLine = textLine.Split(' ');
            int allOccurences = 0;

            foreach (string word in splittedLine)
            {
                if (word.Equals(query, StringComparison.OrdinalIgnoreCase) ||
                    word.Equals(query + '.', StringComparison.OrdinalIgnoreCase) ||
                    word.Equals(query + ',', StringComparison.OrdinalIgnoreCase)
                    )
                {
                    allOccurences++;
                }
            }
            
            return allOccurences > 0;

        }

        private static void ReadFileToList(string path, List<string> stringList)
        {
            using (StreamReader textReader = new StreamReader(path))
            {
                do
                {
                    string line = textReader.ReadLine();
                    if (!line.Equals(""))
                    {
                        stringList.Add(line);
                    }
                    

                } while (!textReader.EndOfStream);
            }

        }

    }

}
