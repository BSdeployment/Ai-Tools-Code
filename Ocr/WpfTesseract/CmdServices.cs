using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WpfOCR
{
    public class CmdServices
    {
       
            public static void Run(string command, out string output, out string error, string directory = null)
            {
                using Process process = new Process
                {
                    StartInfo = new ProcessStartInfo
                    {
                        FileName = "cmd.exe",
                        UseShellExecute = false,
                        RedirectStandardOutput = true,
                        RedirectStandardError = true,
                        RedirectStandardInput = true,
                        Arguments = "/c " + command,
                        CreateNoWindow = true,
                        WorkingDirectory = directory ?? string.Empty,
                        StandardOutputEncoding = Encoding.UTF8, // הגדרת קידוד UTF-8
                        StandardErrorEncoding = Encoding.UTF8,
                    }
                };
                process.Start();
                process.WaitForExit();
                output = process.StandardOutput.ReadToEnd();
                error = process.StandardError.ReadToEnd();
            }
        

    }
}
