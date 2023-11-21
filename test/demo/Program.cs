#region ENBREA Konsoli - Copyright (C) 2023 STÜBER SYSTEMS GmbH
/*    
 *    ENBREA Konsoli
 *    
 *    Copyright (C) 2023 STÜBER SYSTEMS GmbH
 *
 *    Licensed under the MIT License, Version 2.0. 
 * 
 */
#endregion

using Serilog;
using Serilog.Extensions.Logging;
using System;
using System.IO;
using System.Reflection;
using System.Threading;

namespace Enbrea.Konsoli.Demo
{
    class Program
    {
        static Microsoft.Extensions.Logging.ILogger CreateLogger()
        {
            // Create a new Serilog instance
            var seriLog = new LoggerConfiguration()
                .WriteTo.File(Path.Combine(Path.GetDirectoryName(Assembly.GetEntryAssembly().Location), "logs", "log.txt"))
                .CreateLogger();

            // Creates a new Microsoft logging instance out of Serilog.
            return new SerilogLoggerFactory(seriLog).CreateLogger(null);
        }

        static void DoSomething(int millisecondsTimeout)
        {
            Thread.Sleep(millisecondsTimeout);
        }

        static void Main()
        {
            try
            {
                Console.WriteLine("PROGRESS DEMO");
                Console.WriteLine();
                
                // Creates a logger
                var logger = CreateLogger();

                // Count Demo
                var consoleWriter = new ConsoleWriter(ProgressUnit.Count, logger)
                {
                    Theme = new ConsoleWriterTheme()
                    {
                        ProgressTextFormat = "> {0}"
                    },
                    MaxProgressValue = 100
                };

                consoleWriter.Caption("Callouts");

                consoleWriter.Information("This is an information.");
                consoleWriter.Success("This is ok.");
                consoleWriter.Warning("This is a warning!");
                consoleWriter.Error("This is an error!!!").NewLine();

                consoleWriter.Caption("Count Demo");

                consoleWriter.StartProgress("Open File");
                DoSomething(5);
                consoleWriter.FinishProgress();

                consoleWriter.StartProgress("Load Data (and some dummy text which is very, very, very, very long and tries to cover the complete text line)");
                for (var j = 1; j < 100; ++j)
                {
                    DoSomething(10);
                    consoleWriter.ContinueProgress(j);
                }
                consoleWriter.FinishProgress(100);

                consoleWriter.StartProgress("Close File");
                DoSomething(20);
                consoleWriter.FinishProgress();

                consoleWriter.Message("100 steps counted").NewLine();

                // Percent Demo
                consoleWriter = new ConsoleWriter(ProgressUnit.Percent, logger)
                {
                    Theme = new ConsoleWriterTheme()
                    {
                        ProgressTextFormat = "> {0}"
                    }
                };

                consoleWriter.Caption("Percent Demo");

                consoleWriter.StartProgress("Open File");
                DoSomething(20);
                consoleWriter.FinishProgress();

                consoleWriter.StartProgress("Load Data A");
                for (var i = 1; i <= 100; ++i)
                {
                    DoSomething(10);
                    consoleWriter.ContinueProgress(i);
                }
                consoleWriter.FinishProgress(100);

                consoleWriter.StartProgress("Load Data B");
                for (var i = 1; i <= 100; ++i)
                {
                    DoSomething(20);
                    consoleWriter.ContinueProgress(i);
                }
                consoleWriter.FinishProgress(100);

                consoleWriter.Message("Data A and B loaded").NewLine();

                // Custom Value Demo
                consoleWriter = new ConsoleWriter(ProgressUnit.Percent, logger)
                {
                    Theme = new ConsoleWriterTheme()
                    {
                        ProgressTextFormat = "> {0}"
                    }
                };

                consoleWriter.Caption("Custom Value Demo");

                consoleWriter.StartProgress("Open File");
                DoSomething(20);
                consoleWriter.FinishProgress();

                consoleWriter.StartProgress("Load Data A");
                for (var i = 1; i <= 100; ++i)
                {
                    DoSomething(10);
                    consoleWriter.ContinueProgress("{0}/{1}", i, i*10);
                }
                consoleWriter.FinishProgress();

                consoleWriter.StartProgress("Load Data B");
                for (var i = 1; i <= 100; ++i)
                {
                    DoSomething(20);
                    consoleWriter.ContinueProgress("{0}/{1}", i, i * 2);
                }
                consoleWriter.FinishProgress("{0}/{1}", 100, 200);

                consoleWriter.Message("Data A and B loaded").NewLine();

                // Failed Demo
                consoleWriter = new ConsoleWriter(ProgressUnit.Percent, logger)
                {
                    Theme = new ConsoleWriterTheme()
                    {
                        ProgressTextFormat = "> {0}"
                    }
                };

                consoleWriter.Caption("Failed Demo");

                consoleWriter.StartProgress("Open File");
                DoSomething(20);
                consoleWriter.FinishProgress();

                var Failed = false;

                consoleWriter.StartProgress("Load Data");
                try
                {
                    for (var i = 1; i <= 100; ++i)
                    {
                        DoSomething(10);
                        consoleWriter.ContinueProgress(i);
                        if (i == 50) throw new Exception("Error!");
                    }
                    consoleWriter.FinishProgress();
                }
                catch
                {
                    consoleWriter.CancelProgress().NewLine();
                    Failed = true;
                }

                if (!Failed)
                {
                    consoleWriter.StartProgress("Close File");
                    DoSomething(5);
                    consoleWriter.FinishProgress();

                    consoleWriter.Message("100 steps counted").NewLine();
                }

                // File Donwload Demo
                consoleWriter = new ConsoleWriter(ProgressUnit.FileSize, logger)
                {
                    Theme = new ConsoleWriterTheme()
                    {
                        CaptionFormat = "** {0} **",
                        CaptionTextColor = ConsoleColor.Blue,
                        MessageTextColor = ConsoleColor.Green,
                        ProgressTextFormat = "> {0}"
                    },
                    MaxProgressValue = 2048
                };

                consoleWriter.Caption("File Download Demo");

                consoleWriter.StartProgress("Connect");
                DoSomething(5);
                consoleWriter.FinishProgress();

                consoleWriter.StartProgress("Download File");
                for (var j = 1; j <= 2048; ++j)
                {
                    DoSomething(5);
                    consoleWriter.ContinueProgress(j);
                }
                consoleWriter.FinishProgress();

                consoleWriter.StartProgress("Disconnect");
                DoSomething(5);
                consoleWriter.FinishProgress();

                consoleWriter.Success(">", "2048 Bytes downloaded").NewLine();

            }
            catch (Exception ex)
            {
                Console.WriteLine();
                Console.WriteLine(ex.ToString()); 
            }
        }
    }
}