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

using Microsoft.Extensions.Logging;
using System;

namespace Enbrea.Konsoli
{
    /// <summary>
    /// A helper class for displaying progress in console apps.
    /// </summary>
    public class ConsoleWriter
    {
        private readonly ILogger _logger;
        private string _cachedMessage;
        private string _cachedProgressValue;

        /// <summary>
        /// Initializes a new instance of the <see cref="ConsoleWriter"/> class.
        /// </summary>
        /// <param name="progressValueUnit">Progress value unit</param>
        public ConsoleWriter(ProgressUnit progressValueUnit)
        {
            CurrentProgressMessage = null;
            CurrentProgressValue = 0;
            InProgress = false;
            MaxLineWidth = !Console.IsOutputRedirected ? Console.BufferWidth : int.MaxValue;
            MaxProgressValue = progressValueUnit == ProgressUnit.Percent ? 100 : long.MaxValue;
            MinProgressValue = 0;
            NoProgress = false;
            ProgressValueUnit = progressValueUnit;
            Theme = new ConsoleWriterTheme();
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ConsoleWriter"/> class.
        /// </summary>
        /// <param name="progressValueUnit">Progress value unit</param>
        /// <param name="logger">A logging implementation</param>
        public ConsoleWriter(ProgressUnit progressValueUnit, ILogger logger)
            : this(progressValueUnit)
        {
            _logger = logger;
        }

        /// <summary>
        /// The current custom progress message 
        /// </summary>
        public string CurrentCustomProgressValue { get; internal set; }

        /// <summary>
        /// The current progress message 
        /// </summary>
        public string CurrentProgressMessage { get; internal set; }

        /// <summary>
        /// The current progress value 
        /// </summary>
        public long CurrentProgressValue { get; internal set; }

        /// <summary>
        /// Is in progress?
        /// </summary>
        public bool InProgress { get; internal set; }

        /// <summary>
        /// Maximum line width. Default is <see cref="Console.BufferWidth"/>
        /// </summary>
        public int MaxLineWidth { get; set; }

        /// <summary>
        /// Maximum message width (excluding status)
        /// </summary>
        public int MaxMessageLength
        {
            get
            {
                if (!Console.IsOutputRedirected)
                {
                    return Math.Max(0, Math.Min(Console.BufferWidth, MaxLineWidth) - 1 - Math.Max(Theme.OkLabel.Length, Theme.FailedLabel.Length));
                }
                else 
                {
                    return Math.Max(0, MaxLineWidth - 1 - Math.Max(Theme.OkLabel.Length, Theme.FailedLabel.Length));
                }
            }
        }

        /// <summary>
        /// Maximum progress value (used for <see cref="ProgressUnit.Percent"/>)
        /// </summary>
        public long MaxProgressValue { get; set; }

        /// <summary>
        /// Minimum progress value (used for <see cref="ProgressUnit.Percent"/>)
        /// </summary>
        public long MinProgressValue { get; set; }

        /// <summary>
        /// Show no progress?
        /// </summary>
        public bool NoProgress { get; set; }

        /// <summary>
        /// Progress value unit
        /// </summary>
        public ProgressUnit ProgressValueUnit { get; set; }

        /// <summary>
        /// Theme (colors, labels, formats)
        /// </summary>
        public ConsoleWriterTheme Theme { get; set; }

        /// <summary>
        /// Cancels the current progress report by writing the current progress value, the status 
        /// <see cref="ProgressResult.Failed"/> and the line terminator to the standard output stream.
        /// </summary>
        public ConsoleWriter CancelProgress()
        {
            InProgress = false;
            WriteStatus(ProgressResult.Failed);

            if (!Console.IsOutputRedirected)
            {
                Console.CursorVisible = true;
            }

            LogFailedProgress(_cachedMessage, _cachedProgressValue);

            ClearInternalCache();

            return NewLine();
        }

        /// <summary>
        /// Writes caption to the standard output stream.
        /// </summary>
        /// <param name="text">The caption text</param>
        public ConsoleWriter Caption(string text)
        {
            WriteMessage(string.Format(Theme.CaptionFormat, text), Theme.CaptionTextColor, Theme.DefaultBackgroundColor);

            LogInformation(_cachedMessage);

            ClearInternalCache();

            return NewLine();
        }

        /// <summary>
        /// Continues progress by incrementing the progress value and writing it to the standard 
        /// output stream. 
        /// </summary>
        /// <param name="newProgressValue">New progress value</param>
        public ConsoleWriter ContinueProgress(long newProgressValue)
        {
            InProgress = true;

            if (newProgressValue > CurrentProgressValue)
            {
                WriteProgressValue(newProgressValue);
                CurrentProgressValue = newProgressValue;
            }
            return this;
        }

        /// <summary>
        /// Continues progress by incrementing the progress value and writing it to the standard 
        /// output stream. 
        /// </summary>
        /// <param name="newCustomProgressValue">New custom progress value</param>
        public ConsoleWriter ContinueProgress(string newCustomProgressValue)
        {
            InProgress = true;

            if (newCustomProgressValue != CurrentCustomProgressValue)
            {
                WriteProgressValue(newCustomProgressValue);
                CurrentCustomProgressValue = newCustomProgressValue;
            }
            return this;
        }

        /// <summary>
        /// Continues progress by incrementing the progress value and writing it to the standard 
        /// output stream. 
        /// </summary>
        /// <param name="newCustomProgressValueFormat">New custom progress value format string</param>
        /// <param name="args">An object array that contains zero or more objects to format</param>
        public ConsoleWriter ContinueProgress(string newCustomProgressValueFormat, params object[] args)
        {
            return ContinueProgress(string.Format(newCustomProgressValueFormat, args));
        }

        /// <summary>
        /// Writes an error label and text and the line terminator to the standard output stream.
        /// </summary>
        /// <param name="text">The error text</param>
        public ConsoleWriter Error(string text)
        {
            return Error(Theme.ErrorLabel, text);
        }

        /// <summary>
        /// Writes an error label and text and the line terminator to the standard output stream.
        /// </summary>
        /// <param name="label">The error label</param>
        /// <param name="text">The error text</param>
        public ConsoleWriter Error(string label, string text)
        {
            WriteLabel(label, Theme.ErrorLabelTextColor, Theme.ErrorLabelBackgroundColor);
            WriteMessage(string.Format(Theme.ErrorTextFormat, text), Theme.ErrorTextColor, Theme.ErrorBackgroundColor);

            LogError(_cachedMessage);

            ClearInternalCache();

            return NewLine();
        }

        /// <summary>
        /// Finsishs the current progress report by writing the current progress value, the status 
        /// <see cref="ProgressResult.OK"/> and the line terminator to the standard output stream. 
        /// </summary>
        public ConsoleWriter FinishProgress()
        {
            if (CurrentCustomProgressValue != null) FinishProgress(CurrentCustomProgressValue);
            else FinishProgress(CurrentProgressValue);
            return this;
        }

        /// <summary>
        /// Finsishs the current progress report by writing the given progress value, the status
        /// <see cref="ProgressResult.OK"/> and the line terminator to the standard output stream.
        /// </summary>
        /// <param name="newProgressValue">The final progress value</param>
        public ConsoleWriter FinishProgress(long newProgressValue)
        {
            if (InProgress)
            {
                WriteProgressValue(newProgressValue);
                InProgress = false;
            }

            WriteStatus(ProgressResult.OK);

            if (!Console.IsOutputRedirected)
            {
                Console.CursorVisible = true;
            }

            LogCompletion(_cachedMessage, _cachedProgressValue);

            ClearInternalCache();

            return NewLine();
        }

        /// <summary>
        /// Finsishs the current progress report by writing the given progress value, the status 
        /// <see cref="ProgressResult.OK"/> and the line terminator to the standard output stream. 
        /// </summary>
        /// <param name="newCustomProgressValue">The final custom progress value</param>
        public ConsoleWriter FinishProgress(string newCustomProgressValue)
        {
            if (InProgress)
            {
                WriteProgressValue(newCustomProgressValue);
                InProgress = false;
            }

            WriteStatus(ProgressResult.OK);

            if (!Console.IsOutputRedirected)
            {
                Console.CursorVisible = true;
            }

            LogCompletion(_cachedMessage, _cachedProgressValue);

            ClearInternalCache();

            return NewLine();
        }

        /// <summary>
        /// Finsishs the current progress report by writing the given progress value, the status 
        /// <see cref="ProgressResult.OK"/> and the line terminator to the standard output stream. 
        /// </summary>
        /// <param name="newCustomProgressValueFormat">The final custom progress value format string</param>
        /// <param name="args">An object array that contains zero or more objects to format</param>
        public ConsoleWriter FinishProgress(string newCustomProgressValueFormat, params object[] args)
        {
            return FinishProgress(string.Format(newCustomProgressValueFormat, args));
        }

        /// <summary>
        /// Writes a information label and text and the line terminator to the standard output stream.
        /// </summary>
        /// <param name="text">The information text</param>
        public ConsoleWriter Information(string text)
        {
            return Information(Theme.InformationLabel, text);
        }

        /// <summary>
        /// Writes an information label and text and the line terminator to the standard output stream.
        /// </summary>
        /// <param name="label">The information label</param>
        /// <param name="text">The information text</param>
        public ConsoleWriter Information(string label, string text)
        {
            WriteLabel(label, Theme.InformationLabelTextColor, Theme.InformationLabelBackgroundColor);
            WriteMessage(string.Format(Theme.InformationTextFormat, text), Theme.InformationTextColor, Theme.InformationBackgroundColor);

            LogInformation(_cachedMessage);

            ClearInternalCache();

            return NewLine();
        }

        /// <summary>
        /// Writes a message and the line terminator to the standard output stream.
        /// </summary>
        /// <param name="text">The message text</param>
        public ConsoleWriter Message(string text)
        {
            WriteMessage(string.Format(Theme.MessageTextFormat, text), Theme.MessageTextColor, Theme.DefaultBackgroundColor);

            LogInformation(_cachedMessage);

            ClearInternalCache();

            return NewLine();
        }

        /// <summary>
        /// Writes the line terminator to the standard output stream.
        /// </summary>
        public ConsoleWriter NewLine()
        {
            Console.WriteLine();
            return this;
        }

        /// <summary>
        /// Writes the progress message to the standard output stream.
        /// </summary>
        /// <param name="text">The progress message text</param>
        public ConsoleWriter StartProgress(string text)
        {
            InProgress = false;

            CurrentProgressMessage = string.Format(Theme.ProgressTextFormat, text);
            CurrentProgressValue = 0;
            CurrentCustomProgressValue = null;

            WriteProgressMessage();

            if (!Console.IsOutputRedirected)
            {
                Console.CursorVisible = false;
            }

            return this;
        }

        /// <summary>
        /// Writes the progress message to the standard output stream.
        /// </summary>
        /// <param name="text">The progress message text</param>
        /// <param name="progressValue">The progress value with which to start</param>
        public ConsoleWriter StartProgress(string text, long progressValue)
        {
            InProgress = true;

            CurrentProgressMessage = string.Format(Theme.ProgressTextFormat, text);
            CurrentProgressValue = progressValue;
            CurrentCustomProgressValue = null;

            WriteProgressMessage();
            WriteProgressValue(progressValue);

            if (!Console.IsOutputRedirected)
            {
                Console.CursorVisible = false;
            }

            return this;
        }

        /// <summary>
        /// Writes the progress message to the standard output stream.
        /// </summary>
        /// <param name="text">The progress message text</param>
        /// <param name="customProgressValueFormat">The custom progress value with which to start</param>
        public ConsoleWriter StartProgress(string text, string customProgressValue)
        {
            InProgress = true;

            CurrentProgressMessage = string.Format(Theme.ProgressTextFormat, text);
            CurrentProgressValue = 0;
            CurrentCustomProgressValue = customProgressValue;

            WriteProgressMessage();
            WriteProgressValue(customProgressValue);

            if (!Console.IsOutputRedirected)
            {
                Console.CursorVisible = false;
            }

            return this;
        }

        /// <summary>
        /// Writes a success label and text and the line terminator to the standard output stream.
        /// </summary>
        /// <param name="text">The success text</param>
        public ConsoleWriter Success(string text)
        {
            return Success(Theme.SuccessLabel, text);
        }

        /// <summary>
        /// Writes a success label and text and the line terminator to the standard output stream.
        /// </summary>
        /// <param name="label">The success label</param>
        /// <param name="text">The success text</param>
        public ConsoleWriter Success(string label, string text)
        {
            WriteLabel(label, Theme.SuccessLabelTextColor, Theme.SuccessLabelBackgroundColor);
            WriteMessage(string.Format(Theme.SuccessFormat, text), Theme.SuccessTextColor, Theme.SuccessBackgroundColor);

            LogInformation(_cachedMessage);

            ClearInternalCache();

            return NewLine();
        }

        /// <summary>
        /// Writes a warning label and text and the line terminator to the standard output stream.
        /// </summary>
        /// <param name="text">The warning text</param>
        public ConsoleWriter Warning(string text)
        {
            return Warning(Theme.WarningLabel, text);
        }

        /// <summary>
        /// Writes a warning label and text and the line terminator to the standard output stream.
        /// </summary>
        /// <param name="label">The warning label</param>
        /// <param name="text">The warning text</param>
        public ConsoleWriter Warning(string label, string text)
        {
            WriteLabel(label, Theme.WarningLabelTextColor, Theme.WarningLabelBackgroundColor);
            WriteMessage(string.Format(Theme.WarningFormat, text), Theme.WarningTextColor, Theme.WarningBackgroundColor);

            LogWarning(_cachedMessage);

            ClearInternalCache();

            return NewLine();
        }

        /// <summary>
        /// Deletes internal data for logging purposes
        /// </summary>
        private void ClearInternalCache()
        {
            _cachedMessage = null;
            _cachedProgressValue = null;
        }

        /// <summary>
        /// Get string representastion of a progress value
        /// </summary>
        /// <param name="progressValue">The progress value</param>
        /// <returns>The string representastion of the progress value</returns>
        private string GetProgressValueStr(long progressValue)
        {
            switch (ProgressValueUnit)
            {
                case ProgressUnit.Percent:
                    return string.Format("{0,3:##0}%", (double)((progressValue - MinProgressValue) / (double)(MaxProgressValue - MinProgressValue)) * 100);

                case ProgressUnit.Count:
                    return (MaxProgressValue < long.MaxValue) ? string.Format("{0}/{1}", progressValue, MaxProgressValue) : progressValue.ToString();

                case ProgressUnit.FileSize:
                    var fileSize = new FileSize(progressValue);
                    if (MaxProgressValue < long.MaxValue)
                    {
                        var percentValue = (double)((progressValue - MinProgressValue) / (double)(MaxProgressValue - MinProgressValue)) * 100;
                        if (fileSize.Value > 1024)
                        {
                            return string.Format("{0} ({1}), {2,3:##0}%", fileSize.ToString(FileSizeUnit.BytesOnly), fileSize.ToString(), percentValue);
                        }
                        else
                        {
                            return string.Format("{0}, {1,3:##0}%", fileSize.ToString(FileSizeUnit.BytesOnly), percentValue);
                        }
                    }
                    else
                    {
                        if (fileSize.Value > 1024)
                        {
                            return string.Format("{0} ({1})", fileSize.ToString(FileSizeUnit.BytesOnly), fileSize.ToString());
                        }
                        else
                        {
                            return string.Format("{0}", fileSize.ToString(FileSizeUnit.BytesOnly));
                        }
                    }

                default:
                    return string.Empty;
            }
        }

        /// <summary>
        /// Logs the successful completion to abstract log
        /// </summary>
        /// <param name="message">The message</param>
        private void LogCompletion(string message, string value)
        {
            if (value != null)
            {
                LogInformation($"[{Strings.Ok}] {message} {value}");
            }
            else
            {
                LogInformation($"[{Strings.Ok}] {message}");
            }
        }

        /// <summary>
        /// Logs error message to abstract log
        /// </summary>
        /// <param name="message">The message</param>
        private void LogError(string message)
        {
            _logger?.LogError(message);
        }

        /// <summary>
        /// Logs failed progress to abstract log
        /// </summary>
        /// <param name="message">The message</param>
        private void LogFailedProgress(string message, string value)
        {
            if (value != null)
            {
                LogError($"[{Strings.Failed}] {message} {value}");
            }
            else
            {
                LogError($"[{Strings.Failed}] {message}");
            }
        }

        /// <summary>
        /// Logs info message to abstract log
        /// </summary>
        /// <param name="message">The message</param>
        private void LogInformation(string message)
        {
            _logger?.LogInformation(message);
        }

        /// <summary>
        /// Logs warning message to abstract log
        /// </summary>
        /// <param name="message">The message</param>
        private void LogWarning(string message)
        {
            _logger?.LogWarning(message);
        }

        /// <summary>
        /// Writes a label to the standard output stream.
        /// </summary>
        /// <param name="text">The label text</param>
        /// <param name="textColor">Text color</param>
        /// <param name="backgroundColor">Background color</param>
        private void WriteLabel(string text, ConsoleColor textColor, ConsoleColor backgroundColor)
        {
            if (!string.IsNullOrWhiteSpace(text))
            {
                Console.ForegroundColor = textColor;
                Console.BackgroundColor = backgroundColor;
                Console.Write(text);
                Console.ForegroundColor = Theme.DefaultTextColor;
                Console.BackgroundColor = Theme.DefaultBackgroundColor;
                Console.Write(' ');
            }
        }

        /// <summary>
        /// Writes a message to the standard output stream.
        /// </summary>
        /// <param name="text">The message text</param>
        /// <param name="textColor">Text color</param>
        /// <param name="backgroundColor">Background color</param>
        private void WriteMessage(string text, ConsoleColor textColor, ConsoleColor backgroundColor)
        {
            if (!string.IsNullOrWhiteSpace(text))
            {
                _cachedMessage = text;
                Console.ForegroundColor = textColor;
                Console.BackgroundColor = backgroundColor;
                Console.Write(_cachedMessage);
                Console.ForegroundColor = Theme.DefaultTextColor;
                Console.BackgroundColor = Theme.DefaultBackgroundColor;
            }
        }

        /// <summary>
        /// Writes the current progress message to the standard output stream.
        /// </summary>
        private void WriteProgressMessage()
        {
            if (!string.IsNullOrWhiteSpace(CurrentProgressMessage))
            {
                _cachedMessage = CurrentProgressMessage;
                Console.ForegroundColor = Theme.ProgressTextColor;
                Console.BackgroundColor = Theme.DefaultBackgroundColor;
                Console.Write(!Console.IsOutputRedirected ? StringExtensions.CutOrPadRight(_cachedMessage, MaxMessageLength) : _cachedMessage);
                Console.ForegroundColor = Theme.DefaultTextColor;
                Console.Write(' ');
            }
        }

        /// <summary>
        /// Overrides the current progress value with the given progress value as text to the 
        /// standard output stream.
        /// </summary>
        /// <param name="progressValue">The progress value</param>
        private void WriteProgressValue(long progressValue)
        {
            WriteProgressValue(GetProgressValueStr(progressValue));
        }

        /// <summary>
        /// Overrides the current progress value with the given progress value as text to the 
        /// standard output stream.
        /// </summary>
        /// <param name="progressValueStr">The progress value as string</param>
        private void WriteProgressValue(string customProgressValue)
        {
            if ((!NoProgress) && (!Console.IsOutputRedirected))
            {
                var progressMessageLength = MaxMessageLength - customProgressValue.Length - 1;
                var progressValueLength = MaxMessageLength - progressMessageLength - 1;

                if ((progressMessageLength + progressValueLength) >= (customProgressValue.Length + customProgressValue.Length))
                {
                    _cachedProgressValue = customProgressValue;
                    Console.Write(string.Empty.PadLeft(progressMessageLength + progressValueLength + 2, '\b'));
                    Console.ForegroundColor = Theme.ProgressTextColor;
                    Console.Write(StringExtensions.CutOrPadRight(CurrentProgressMessage, progressMessageLength));
                    Console.ForegroundColor = Theme.ProgressValueColor;
                    Console.Write(' ');
                    Console.Write(customProgressValue.PadLeft(progressValueLength));
                    Console.Write(' ');
                }
            }
        }

        /// <summary>
        /// Writes the given status as text to the standard output stream.
        /// </summary>
        /// <param name="status">The status value</param>
        private void WriteStatus(ProgressResult status)
        {
            if (status == ProgressResult.OK)
            {
                Console.ForegroundColor = Theme.OkLabelColor;
                Console.BackgroundColor = Theme.OkLabelBackgroundColor;
                Console.Write(Strings.Ok);
                Console.ForegroundColor = Theme.DefaultTextColor;
                Console.BackgroundColor = Theme.DefaultBackgroundColor;
            }
            else
            {
                Console.ForegroundColor = Theme.FailedLabelTextColor;
                Console.BackgroundColor = Theme.FailedLabelBackgroundColor;
                Console.Write(Strings.Failed);
                Console.ForegroundColor = Theme.DefaultTextColor;
                Console.BackgroundColor = Theme.DefaultBackgroundColor;
            }
        }
    }
}