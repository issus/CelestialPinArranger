using System;
using System.Diagnostics;
using System.IO;
using System.Text.RegularExpressions;

namespace RegexVerifier
{
    class Program
    {

        static Regex rex = new Regex("\\[[Cc]omponent\\]\\s+(?<Component>[^\\r\\n]+)[ \\t]*(?:[\\r\\n]{1,2}[ \\t]*(?:[^\\[\\r\\n][^\\r\\n]*)?)*(?:(?:(?:[\\r\\n]{1,2}\\[[Mm]anufacturer\\]\\s+(?<Manufacturer>[^\\r\\n]+)[ \\t]*)(?:[\\r\\n]{1,2}[ \\t]*(?:[^\\[\\r\\n][^\\r\\n]*)?)*)|(?:[\\r\\n]{1,2}\\[[Pp]ackage\\][^\\r\\n]*(?:[\\r\\n]{1,2}[ \\t]*(?:[^\\[\\r\\n][^\\r\\n]*)?)*)+|(?:[\\r\\n]{1,2}\\[Package Model\\][^\\r\\n]*)?)+[\\r\\n]{1,2}\\[[Pp][Ii][Nn]]\\s*signal_name\\s+model_name[^\\r\\n]*(?:[\\r\\n]{1,2}[ \\t]*(?:[^\\w\\r\\n][^\\r\\n]*)?)*(?:[\\r\\n]{1,2}[ \\t]*(?:\\|[^\\r\\n]*)?|(?<PinDef>(?<Pin>\\w+)\\s+(?<Signal>\\S+)[^\\r\\n]*))*(?:[\\r\\n]{1,2}[ \\t]*(?:[^\\[\\r\\n][^\\r\\n]*)?)*(?:[\\r\\n]{1,2}[ \\t]*\\[)", RegexOptions.Compiled);

        static void Main(string[] args)
        {
            AppDomain domain = AppDomain.CurrentDomain;
            // Set a timeout interval of 1 seconds.
            domain.SetData("REGEX_DEFAULT_MATCH_TIMEOUT", TimeSpan.FromSeconds(1));

            string[] paths = new string[]
            {
                @"X:\Symbols\onsemi\IBS",
                @"X:\Symbols\Microchip IBIS",
                @"X:\Symbols\ad-ibis"
            };

            WriteMessage($"Verifying RegEx across {paths.Length} folders.");

            foreach (var path in paths)
            {
                if (!Directory.Exists(path))
                {
                    WriteMessage($"Could not find folder {path}", ConsoleColor.Red);
                    continue;
                }

                var files = Directory.GetFiles(path, "*.ibs");

                WriteMessage($"{path} has {files.Length} .ibs files to verify against.", ConsoleColor.Cyan);

                int fileCount = 0;
                int missCount = 0;
                Stopwatch folderRunTime = new Stopwatch();
                folderRunTime.Start();

                foreach (var file in files)
                {
                    fileCount++;
                    string fileContents = "";
                    try
                    {
                        fileContents = File.ReadAllText(file);

                        // truncate file a bit, to make regex faster
                        if (fileContents.Contains("[Model]"))
                        {
                            fileContents = fileContents.Substring(0, fileContents.IndexOf("[Model]") + 8);
                        }
                    }
                    catch (Exception err)
                    {

                        WriteMessage($"Failed to read {file}: {err.Message}", ConsoleColor.Red);
                        continue;
                    }

                    if (string.IsNullOrWhiteSpace(fileContents))
                    {
                        WriteMessage($"{file} is empty!", ConsoleColor.Red);
                        continue;
                    }

                    try
                    {
                        var matches = rex.Matches(fileContents);
                        if (matches.Count == 0)
                        {
                            missCount++;
                            WriteMessage($"Match failed on {file}", ConsoleColor.Red);

                            continue;
                        }

                        //Debug.WriteLine($"{fileCount}] {matches.Count} matches in {file}");


                        foreach (Match match in matches)
                        {
                            //Debug.WriteLine($"{match.Groups["Component"].Value} has {match.Groups["Pin"].Captures.Count} pins.");

                            if (match.Groups["Pin"].Captures.Count < 2)
                            {
                                WriteMessage($"{match.Groups["Component"].Value} doesn't have enough ({match.Groups["Pin"].Captures.Count}) pins in {file}", ConsoleColor.Red);
                            }
                        }
                        
                    }
                    catch (Exception err)
                    {
                        WriteMessage($"Error on {file}: {err.Message}", ConsoleColor.Red);
                    }
                }

                folderRunTime.Stop();
                WriteMessage($"Processed {fileCount} files in {folderRunTime.Elapsed:g}. Average of {folderRunTime.ElapsedMilliseconds/fileCount:0}ms per file.", ConsoleColor.Green);
                if (missCount > 0)
                {
                    WriteMessage($"Could not match on {missCount} files!", ConsoleColor.Red);
                }
            }
        }

        static void WriteMessage(string message, ConsoleColor colour = ConsoleColor.White) 
        {
            Console.ForegroundColor = colour;
            Console.WriteLine(message);
            Console.ForegroundColor = ConsoleColor.White;
        }

    }
}
