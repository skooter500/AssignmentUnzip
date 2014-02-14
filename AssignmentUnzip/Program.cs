using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Diagnostics;

namespace AssignmentUnzip
{
    class Program
    {
        static void Main(string[] args)
        {
            string prefix = "";
            string path = "";

            if (args.Length != 2)
            {
                Console.WriteLine("Usage: AssignmentUnzip <prefix> <path>");
                return;
            }
            else
            {
                prefix = args[0];
                path = args[1];
                if (path.Substring(path.Length - 1, 1) != ("" + Path.DirectorySeparatorChar))
                {
                    path = path + Path.DirectorySeparatorChar;
                }
            }

            DirectoryInfo di = new DirectoryInfo(path);
            FileInfo[] fileInfo = di.GetFiles(prefix + "*.*");
            foreach(FileInfo file in fileInfo)
            {
                int nextUnderScore = file.Name.IndexOf("_", prefix.Length);
                int studentNumberLength = nextUnderScore - prefix.Length;
                string studentNumber = file.Name.Substring(prefix.Length, studentNumberLength);
                Directory.CreateDirectory(path + studentNumber);
                string[] zipExts = {"7z", "rar", "zip"};
                bool isZip = false;
                foreach (string ext in zipExts)
                {
                    if (file.Name.EndsWith(ext))
                    {
                        isZip = true;
                        break;
                    }
                }
                string dest = path + studentNumber + Path.DirectorySeparatorChar;
                if (isZip)
                {
                    string cmdLine = "x -o\"" + dest + "\" \"" + file.FullName + "\"";
                    Console.WriteLine(cmdLine);
                    ProcessStartInfo startInfo = new ProcessStartInfo();
                    startInfo.CreateNoWindow = false;
                    startInfo.UseShellExecute = true;
                    startInfo.FileName = "7z.exe";
                    startInfo.WindowStyle = ProcessWindowStyle.Normal;
                    startInfo.Arguments = cmdLine;

                    try
                    {
                        // Start the process with the info we specified.
                        // Call WaitForExit and then the using statement will close.
                        using (Process exeProcess = Process.Start(startInfo))
                        {
                            exeProcess.WaitForExit();
                        }
                    }
                    catch(Exception e)
                    {
                        Console.WriteLine(e.StackTrace);
                    }
                   
                }
                else
                {
                    string cmdLine = "/c copy \"" + file.FullName + "\"" + " \"" + dest + "\""; 
                    Process.Start("cmd.exe", cmdLine);
                    //File.Copy(file.FullName, dest);
                }
            }
        }
    }
}
