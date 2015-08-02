using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO; //file

namespace FuncCompiler
{
        
    class Program
    {
        static void Main(string[] args)
        {
            #region ArgCheck
            //var for command-line flags
            bool argErr = false;
            bool flagInterpret = false;
            bool flagCompile = false;
            string inputFile;
            string input = null;
            
            //Command-line flags check.
            if (args.Count() == 0 || args.Count() > 4)
            {
                argErr = true;
                Console.WriteLine("Error on arguments." + (args.Count() == 0 ? "Too few" : "Too many") + " args.\n");
            }
            else
            {
                for (int i = 0; i < args.Length; i++)
                {
                    switch (args[i])
                    {
                        case "-i":
                            flagInterpret = true;
                            break;

                        case "-c":
                            flagCompile = true;
                            break;

                        case "-f":
                            if (i + 1 < args.Length)
                            {
                                inputFile = args[++i];

                                try
                                {
                                    string path = Directory.GetCurrentDirectory();
                                    string filePath = Path.Combine(path, inputFile);
                                    input = System.IO.File.ReadAllText(@inputFile);
                                }
                                catch (Exception e)
                                {
                                    Console.WriteLine("Error on input file: "+ e.Message);
                                    argErr = true;
                                }

                            }
                            else
                            {
                                argErr = true;
                                Console.WriteLine("Error on arguments. Input file name not specified");
                            }
                            break;

                        default:
                            argErr = true;
                            Console.WriteLine("Error on arguments. Flag " + args[i] + " not valid");
                            break;
                    }
                }
            }

            //I check here so I can list all invalid flag on the switch
            if (argErr)
            {
                Console.WriteLine("\n\nAllowed command-line flag:");
                Console.WriteLine("\t -f fileName: use the file with fileName in the current working directory as input.");
                Console.WriteLine("\t -i: causes the program to interpret the Boolean expressions to produce the corresponding boolean value on the standard output stream.");
                Console.WriteLine("\t -c: causes the program to create a file named \"result.c\" in the current working directory. That file contain C code to perform the computation specified by the input string.");

                Console.WriteLine("\n\nPress any key to exit.");
                System.Console.ReadKey();
                Environment.Exit(1);
            }

            #endregion
            
            Tokenizer tokenizer = new Tokenizer();
            tokenizer.setInput(input);

            Parser parser = new Parser(tokenizer);
            Console.WriteLine("Input file parsed successfully.");
            Node root = parser.parse();


            if (flagInterpret)
            {
                Console.WriteLine("Evaluating:\n\t" + input);
                try
                {
                    Console.WriteLine("Result: \t" + root.eval().ToString());
                }
                catch (Exception e)
                {
                    Console.WriteLine("Error evaluating input file. Evaluation interrupted.\n\t" + e.Message);
                    Environment.Exit(1);
                }
            }

            if (flagCompile)
            {
                         
                string path = Directory.GetCurrentDirectory();
                string fileName = Path.Combine(path, "result.c");

                Compiler compiler = new Compiler(input, root);

                string compiledCode = compiler.compile();

                try
                {
                    System.IO.File.WriteAllText(fileName, compiledCode);
                }
                catch (Exception e)
                {
                    Console.WriteLine("Error writing \"result.c\"\n\r" + e.Message);
                    Environment.Exit(1);
                }

                Console.WriteLine("Compilation successful.");
            }
            
            // Keep the console window open in debug mode.
            //Console.WriteLine("Press any key to exit.");
            //System.Console.ReadKey();
        }
    }
}
