using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using System.Linq;
using System.Text.RegularExpressions;
using System.Reflection;

namespace Anagram
{
    class Program
    {
        private static List<string> allResults;
        static void Main(string[] args)
        {
            Console.WriteLine("Welcome!");
            Start();
        }

        private static void Start()
        {
            allResults = new List<string>();
            List<string> listOfWords = ReadFile();
            Console.WriteLine("Enter the desired word or phrase:");
            string input = Console.ReadLine().ToLower().Replace(" ", string.Empty);

            if (IsValid(input))
            {
                Compare(input, listOfWords);
            }
            else
            {
                Console.WriteLine("Invalid word or phrase.");
            }
        }

        private static List<string> ReadFile()
        {
            string path = Path.GetDirectoryName(Assembly.GetEntryAssembly().Location.Substring(0, Assembly.GetEntryAssembly().Location.IndexOf("bin\\"))) + @"\File\palavras.txt";
            return File.ReadLines(path).ToList();
        }

        private static bool IsValid(string str)
        {
            return Regex.IsMatch(str, @"^[a-z]+$");
        }

        private static void Compare(string input, List<string> listOfWords)
        {
            Console.WriteLine("----------------- START -----------------");
            Combinations("", input, listOfWords);
            Console.WriteLine("------------------ END ------------------");
            Start();
        }

        private static void Combinations(string newWord, string input, List<string> listOfWords)
        {
            int n = input.Length;
            if (n == 0)
            {
                Verify(newWord, listOfWords);
            }
            else
            {
                for (int i = 0; i < n; i++)
                {
                    Combinations(newWord + input[i], input.Substring(0, i) + input.Substring(i + 1, n - (i + 1)), listOfWords);
                }
            }
        }

        private static void Verify(string word, List<string> listOfWords)
        {
            var list = listOfWords.Where(x => word.IndexOf(x.ToLower()) >= 0).OrderByDescending(x => x.Length).ToList();

            List<string> result = new List<string>();
            int length = word.Length;
            list.ForEach(item =>
            {
                if (word.IndexOf(item.ToLower()) >= 0)
                {
                    word = word.Remove(word.IndexOf(item.ToLower()), item.Length);
                    result.Add(item);
                }
            });

            if (string.Join(string.Empty, result).Length == length) 
            {
                if (!allResults.Contains(string.Join(string.Empty, result)))
                {
                    allResults.Add(string.Join(string.Empty, result));
                    Console.WriteLine(string.Join(' ', result));
                }
            }
        }
    }
}
