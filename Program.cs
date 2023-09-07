using System.Globalization;
using System.IO;
using System.Xml.Linq;
using WongmaneeB_Lab1CSharpReview;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace WongmaneeB_CSharpReview
{
    public class Program
    {
        private static string rootFolder = Directory.GetParent(Directory.GetCurrentDirectory()).Parent.Parent.ToString();
        private static string fileName;
        private static new List<VideoGame> vgList = new List<VideoGame>();
        public static void Main()
        {
            InputFile(); // Step 1.

            Console.WriteLine("Mash <ENTER> to view all games in videogames.csv.");
            Console.ReadKey();
            SortByTitle(); // Step 2.

            Console.WriteLine(Environment.NewLine);

            Console.WriteLine("Mash <ENTER> to view all Namco Bandai games.");
            Console.ReadKey();
            Console.Clear();
            NamcoBandaiGames(); // Steps 3 & 4.

            Console.WriteLine(Environment.NewLine);

            Console.WriteLine("Mash <ENTER> to view all RPGs.");
            Console.ReadKey();
            Console.Clear();
            RolePlayingGames(); // Steps 5 & 6.

            Console.WriteLine(Environment.NewLine);

            Console.WriteLine("Mash <ENTER> to view data based on a Publisher of your choice.");
            Console.ReadKey();
            Console.Clear();
            PublisherData(); // Step 7

            Console.WriteLine(Environment.NewLine);

            Console.WriteLine("Mash <ENTER> to view data based on a Genre of your choice.");
            Console.ReadKey();
            Console.Clear(); // Step 8
            GenreData();
        }







        // ======================== INPUT FILE ======================== //
        private static void InputFile()
        {
            // ========== DECLARE VARIABLES ========== //
            string filePath = $"{rootFolder}{Path.DirectorySeparatorChar}videogames.csv";

            fileName = Path.GetFileName(filePath);

            StreamReader sr = new StreamReader(filePath);

            List<string> lines = new List<string>();


            // ========== STREAMREADER ========== //
            using (sr)
            {
                sr.ReadLine(); // Cuts off the header

                while (!sr.EndOfStream)
                {
                    lines.Add(sr.ReadLine());
                }

                for (int i = 0; i < lines.Count; i++)
                {
                    // ========== BASIC PROPERTIES ========== //
                    string vgName = lines[i].Split(',')[0];
                    string vgPlatform = lines[i].Split(',')[1];
                    int vgYear = ParseInt(lines[i].Split(',')[2]);
                    string vgGenre = lines[i].Split(',')[3];
                    string vgPublisher = lines[i].Split(',')[4];

                    // ========== SALES PROPERTIES ========== //
                    decimal vgNASales = ParseDecimal(lines[i].Split(',')[5]);
                    decimal vgEUSales = ParseDecimal(lines[i].Split(',')[6]);
                    decimal vgJPSales = ParseDecimal(lines[i].Split(',')[7]);
                    decimal vgOtherSales = ParseDecimal(lines[i].Split(',')[8]);
                    decimal vgGlobalSales = ParseDecimal(lines[i].Split(',')[9]);

                    // ========== CREATE VG OBJECT ========== //
                    VideoGame vg = new VideoGame(vgName, vgPlatform, vgYear, vgGenre, vgPublisher,
                        vgNASales, vgEUSales, vgJPSales, vgOtherSales, vgGlobalSales);

                    vgList.Add(vg);
                }
            }

            sr.Close();
        }

        // ======================== PARSE METHODS ======================== //
        static int ParseInt(string valueAsString)
        {
            int valueAsInt;
            Int32.TryParse(valueAsString, out valueAsInt);
            return valueAsInt;
        }

        static decimal ParseDecimal(string valueAsString)
        {
            decimal valueAsDecimal;
            Decimal.TryParse(valueAsString, out valueAsDecimal);
            return valueAsDecimal;
        }







        // ======================== SORT BY TITLE ======================== //
        private static void SortByTitle()
        {
            vgList.Sort();
            foreach (VideoGame vg in vgList)
            {
                Console.WriteLine(vg);
            }
        }






        // ======================== NAMCO BANDAI GAMES ======================== //
        private static void NamcoBandaiGames()
        {
            string targetPublisher = "Namco Bandai Games";

            // ========== DISPLAY ========== //
            DisplayData(true, targetPublisher); // isPublisher == true.
        }

        // ======================== ROLE-PLAYING GAMES ======================== //
        private static void RolePlayingGames()
        {
            string targetGenre = "Role-Playing";

            // ========== DISPLAY ========== //
            DisplayData(false, targetGenre); // isPublisher == false.
        }






        // ======================== PUBLISHER DATA ======================== //
        private static void PublisherData()
        {
            DistinctSelect(true); // isPublisher == true.
        }

        // ======================== PUBLISHER DATA ======================== //
        private static void GenreData()
        {
            DistinctSelect(false); // isPublisher == false.
        }




        // ======================== DISPLAY ======================== //
        private static void DisplayData(bool isPublisher, string target)
        {
            List<VideoGame> targetVGList;

            if (isPublisher)
            {
                // ========== PUBLISHER LINQ FILTER & SORT ========== //
                targetVGList = vgList
                .Where(vg => vg.Publisher == target)
                .OrderBy(vg => vg.Name) // May be redundant if the entire list is already sorted A-Z.
                .ToList();
            }
            else
            {
                // ========== GENRE LINQ FILTER & SORT ========== //
                targetVGList = vgList
                .Where(vg => vg.Genre == target)
                .OrderBy(vg => vg.Name) // May be redundant if the entire list is already sorted A-Z.
                .ToList();
            }

            // ========== DISPLAY ========== //
            string header = $"======================== {target.ToUpper()} ========================";
            string footerText = $" END OF {target.ToUpper()} STREAM ";
            int borderPadding = (header.Length - footerText.Length) / 2;
            string footer = new string('=', borderPadding) + footerText + new string('=', borderPadding);

            Console.Write(Environment.NewLine);
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine(header);
            Console.ResetColor();
            Console.Write(Environment.NewLine);

            foreach (VideoGame vg in targetVGList)
            {
                Console.WriteLine(vg);
            }

            Console.Write(Environment.NewLine);
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine(footer);
            Console.ResetColor();

            Console.Write(Environment.NewLine);

            // ========== PERCENTAGE ========== //
            string percentage = (Math.Round(((decimal)targetVGList.Count / (decimal)vgList.Count) * 100, 2)).ToString("0.00");

            string singularPluralVerb = (targetVGList.Count == 1) ? "is" : "are";

            if (isPublisher)
            {
                Console.WriteLine($"Out of {vgList.Count} games, {targetVGList.Count} {singularPluralVerb} developed by {target}, which is {percentage}%.");
            }
            else
            {
                Console.WriteLine($"Out of {vgList.Count} games, {targetVGList.Count} are {target} games, which is {percentage}%.");
            }
        }






        // ======================== SELECT DISTINCT PUBLISHER/GENRE ======================== //
        private static void DistinctSelect(bool isPublisher)
        {
            List<string> allDistinct;

            if (isPublisher)
            {
                // ========== LINQ RETRIEVE PULBISHERS ========== //
                allDistinct = vgList
                    .Select(vg => vg.Publisher)
                    .Distinct()
                    .ToList();

                Console.WriteLine("Choose from one of the following PUBLISHERS:");
            }
            else
            {
                // ========== LINQ RETRIEVE GENRES ========== //
                allDistinct = vgList
                    .Select(vg => vg.Genre)
                    .Distinct()
                    .ToList();

                Console.WriteLine("Choose from one of the following GENRES:");
            }

            // ========== INPUT PROMPT ========== //
            
            Console.Write(Environment.NewLine);

            for (int i = 0; i < allDistinct.Count; i++)
            {
                Console.WriteLine($"[{i + 1}] {allDistinct[i]}");
            }

            Console.WriteLine(Environment.NewLine);
            Console.Write($"Input your option (1-{allDistinct.Count}) here: ");

            int selectedOption;

            // ========== INPUT VALIDATION ========== //
            while (!Int32.TryParse(Console.ReadLine(), out selectedOption)
                || (selectedOption < 1 || selectedOption > allDistinct.Count))
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.Write("ERROR: That was not a valid option. ");
                Console.ResetColor();
                Console.Write($"Please input an option (1-{allDistinct.Count}): ");
            }

            Console.Clear();

            // ========== DISPLAY ========== //
            if (isPublisher)
            {
                // ========== PUBLISHERDATA ========== //
                DisplayData(true, allDistinct[selectedOption - 1]); // isPublisher == true.
            }
            else
            {
                // ========== GENREDATA ========== //
                DisplayData(false, allDistinct[selectedOption - 1]); // isPublisher == false.
            }
        }
    }
}