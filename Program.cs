using System;
using System.IO;
using System.Diagnostics;
using System.Xml;
using System.Linq;

namespace DD
{
    class Program
    {
        static void Main(string[] args)
        {
            if (args.Length == 2)
            {
                if (Directory.Exists(args[0]))
                {

                    Process p = new Process();
                    p.StartInfo.FileName = "srcML";
                    p.StartInfo.Arguments = " --position " + args[0] + " -o " + args[1];
                    p.Start();
                    p.WaitForExit();
                    p.Dispose();

                    if (File.Exists(args[1]))
                    {
                        DefectScanner ds = new DefectScanner(args[1]);
                        ds.Detect();
                    }
                    else
                    {
                        Console.WriteLine("Error: srcML file not created");
                    }
                }
                else
                {
                    Console.WriteLine("Error: folder with sources not exists");
                }


            }
            else
            {
                Console.WriteLine("Usage:  DD path-to-source-code-folder xml-file-name");
            }



        }
    }
}
