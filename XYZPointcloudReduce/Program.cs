using System;
using System.IO;
using System.Globalization;
using System.Collections.Generic;
using System.Linq;
using System.Text;


namespace XYZPointcloudReduce
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("XYZ Pointcloud reducer");
            Console.WriteLine("---------------------");
            Console.WriteLine("Creates a copy of a text pointcloud file with reduced precision of the floating point to decrease the file size");
            Console.WriteLine("http://id144.org");
            Console.WriteLine("");
            Console.WriteLine("Usage:");
            Console.WriteLine("XYZPointcloudReduce [-n:0..9] input_file output_file");
            Console.WriteLine("Mandatory arguments:");
            Console.WriteLine("");
            Console.WriteLine(" input_file     Input point-cloud file containing space separated values as strings. Supported formats: ASC, XYZ, etc. ");
            Console.WriteLine(" input_file     Output point-cloud file containing space separated values as strings.");

            Console.WriteLine("Optional arguments:");
            Console.WriteLine(" -n:0..9     Specify the count of digits after decimal point, lower number results in lower file size. Default value is 3.");
            Console.WriteLine("");
                        
            if (args.Length<2)
            {

                Console.WriteLine("No command line arguments specified.");

                Environment.Exit(0);

            }
            string outputFile = args[args.Length - 1];
            string inputFile = args[args.Length - 2];

            if (!File.Exists(inputFile))
            {
                Console.WriteLine("File {0} not found.", inputFile);

                Environment.Exit(0);
            }
            FileInfo inputFileInfo = new FileInfo(inputFile);
            double inputFileSize = inputFileInfo.Length;
            if (inputFileSize < 10) 
            {
                Console.WriteLine("Input file {0} size is too small. File is probably invalid.", inputFile);
                Environment.Exit(0);
            }

            if (File.Exists(outputFile))
            {
                ConsoleKey response;
                do
                {
                    Console.WriteLine("Output file {0} already exists. owerwrite?", outputFile);
                    response = Console.ReadKey(false).Key;   
                    if (response != ConsoleKey.Enter)
                        Console.WriteLine();

                } while (response != ConsoleKey.Y && response != ConsoleKey.N);

                if (response == ConsoleKey.N) Environment.Exit(0);
            }


            string stringFormat = "{0:0.";
            int reductionTo;
            var reductionArg = args.SingleOrDefault(arg => arg.Contains("-n:"));
            if (string.IsNullOrEmpty(reductionArg))
            {
                reductionArg = "";
            }
            reductionArg = reductionArg.Replace("-n:", "");
            reductionTo = Int32.TryParse(reductionArg, out reductionTo)? reductionTo : 2;
            
            for (int i = 0; i < reductionTo; i++)
            {
                stringFormat += "0";
            }           

            stringFormat += "}";
            int lineIndex = 0;

            using (System.IO.StreamWriter fileWrite =
                new System.IO.StreamWriter(outputFile))
            using (System.IO.StreamReader fileRead =
                new System.IO.StreamReader(inputFile))
            {
                Console.WriteLine("Processing file {0}", inputFile);
                Console.WriteLine("Writing to file {0}", outputFile);                
                while (fileRead.Peek() >= 0)
                {

                    lineIndex++;
                    if (lineIndex%30000==0)
                    {
                        Console.Write(".");
                    }
                    string line = (fileRead.ReadLine());

                    string[] tokens = line.Split(' ');
                    double[] values = null;
                    try
                    {
                        values = Array.ConvertAll(tokens, double.Parse);
                    }
                    catch (System.FormatException ex)
                    {

                    }
                    if (values == null)
                    {
                        //line contained no values, just copy it
                        fileWrite.Write(line);
                    }
                    else
                    {

                        for (int i = 0; i < values.Length; i++)
                        {
                            if ((values[i] - Math.Truncate(values[i])) == 0)
                            {
                                    fileWrite.Write(
                                    String.Format(CultureInfo.InvariantCulture,
                                    "{0:0}", values[i])); 
                            }
                            else
                            {
                                 fileWrite.Write(
                                    String.Format(CultureInfo.InvariantCulture,
                                     stringFormat, values[i])); 

                            }
                            if (i != values.Length - 1)
                            {
                                fileWrite.Write(" ");
                            }
                        }
                    }
                    fileWrite.WriteLine();
                }
            }
            FileInfo outputFileInfo = new FileInfo(outputFile);
            double outputFileSize = outputFileInfo.Length;

            double reductionPercentage = 100 - 100 * (outputFileSize / inputFileSize) ;
            Console.WriteLine("");
            Console.WriteLine("Input file {0} size: {1}", inputFile, inputFileSize);
            Console.WriteLine("Output file {0} size: {1}", outputFile, outputFileSize);
            Console.WriteLine("Reduced by " + String.Format(CultureInfo.InvariantCulture,"{0:0.00}",reductionPercentage) + "%");

        }
        
    }
}
