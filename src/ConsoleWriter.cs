#region Enbrea.Konsoli - Copyright (C) STÜBER SYSTEMS GmbH
/*    
 *    Enbrea.Konsoli 
 *    
 *    Copyright (C) STÜBER SYSTEMS GmbH
 *
 *    Licensed under the MIT License.
 * 
 */
#endregion

using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;

namespace Enbrea.Konsoli;

/// <summary>
/// A helper class for displaying progress in console apps.
/// </summary>
public class ConsoleWriter
{
    private readonly ILogger _logger = NullLogger<ConsoleWriter>.Instance;
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

        MaxLineWidth = !Console.IsOutputRedirected
            ? Console.BufferWidth - 1
            : int.MaxValue - 1;

        MaxProgressValue = progressValueUnit == ProgressUnit.Percent
            ? 100
            : long.MaxValue;

        MinProgressValue = 0;
        NoProgress = false;
        ProgressValueUnit = progressValueUnit;
        Strings = new ConsoleWriterStrings();
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
        _logger = logger ?? NullLogger<ConsoleWriter>.Instance;
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
            var statusLength = Math.Max(
                Strings.Ok.Length,
                Strings.Failed.Length);

            var lineWidth = !Console.IsOutputRedirected
                ? Math.Min(Console.BufferWidth, MaxLineWidth)
                : MaxLineWidth;

            return Math.Max(0, lineWidth - statusLength - 1);
        }
    }

    /// <summary>
    /// Maximum progress value
    /// </summary>
    public long MaxProgressValue { get; set; }

    /// <summary>
    /// Minimum progress value
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
    /// Strings (for translation purpose)
    /// </summary>
    public ConsoleWriterStrings Strings { get; set; }

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
        RestoreCursor();

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
        WriteMessage(string.Format(Strings.CaptionFormat, text), Theme.CaptionTextColor, Theme.DefaultBackgroundColor);

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
            CurrentProgressValue = newProgressValue;
            CurrentCustomProgressValue = null;

            WriteProgressValue(newProgressValue);
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
            CurrentCustomProgressValue = newCustomProgressValue;

            WriteProgressValue(newCustomProgressValue);
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
        return Error(Strings.ErrorLabel, text);
    }

    /// <summary>
    /// Writes an error label and text and the line terminator to the standard output stream.
    /// </summary>
    /// <param name="label">The error label</param>
    /// <param name="text">The error text</param>
    public ConsoleWriter Error(string label, string text)
    {
        WriteLabel(label, Theme.ErrorLabelTextColor, Theme.ErrorLabelBackgroundColor);
        WriteMessage(string.Format(Strings.ErrorFormat, text), Theme.ErrorTextColor, Theme.ErrorBackgroundColor);

        LogError(_cachedMessage);
        ClearInternalCache();

        return NewLine();
    }

    /// <summary>
    /// Finishes the current progress report by writing the current progress value, the status 
    /// <see cref="ProgressResult.OK"/> and the line terminator to the standard output stream. 
    /// </summary>
    public ConsoleWriter FinishProgress()
    {
        return CurrentCustomProgressValue != null
           ? FinishProgress(CurrentCustomProgressValue)
           : FinishProgress(CurrentProgressValue);
    }

    /// <summary>
    /// Finishes the current progress report by writing the given progress value, the status
    /// <see cref="ProgressResult.OK"/> and the line terminator to the standard output stream.
    /// </summary>
    /// <param name="newProgressValue">The final progress value</param>
    public ConsoleWriter FinishProgress(long newProgressValue)
    {
        CurrentProgressValue = newProgressValue;
        CurrentCustomProgressValue = null;

        if (InProgress)
        {
            WriteProgressValue(newProgressValue);
            InProgress = false;
        }

        WriteStatus(ProgressResult.OK);
        RestoreCursor();

        LogCompletion(_cachedMessage, _cachedProgressValue);
        ClearInternalCache();

        return NewLine();
    }

    /// <summary>
    /// Finishes the current progress report by writing the given progress value, the status 
    /// <see cref="ProgressResult.OK"/> and the line terminator to the standard output stream. 
    /// </summary>
    /// <param name="newCustomProgressValue">The final custom progress value</param>
    public ConsoleWriter FinishProgress(string newCustomProgressValue)
    {
        CurrentCustomProgressValue = newCustomProgressValue;

        if (InProgress)
        {
            WriteProgressValue(newCustomProgressValue);
            InProgress = false;
        }

        WriteStatus(ProgressResult.OK);
        RestoreCursor();

        LogCompletion(_cachedMessage, _cachedProgressValue);
        ClearInternalCache();

        return NewLine();
    }

    /// <summary>
    /// Finishes the current progress report by writing the given progress value, the status 
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
        return Information(Strings.Information, text);
    }

    /// <summary>
    /// Writes an information label and text and the line terminator to the standard output stream.
    /// </summary>
    /// <param name="label">The information label</param>
    /// <param name="text">The information text</param>
    public ConsoleWriter Information(string label, string text)
    {
        WriteLabel(label, Theme.InformationLabelTextColor, Theme.InformationLabelBackgroundColor);
        WriteMessage(string.Format(Strings.InformationTextFormat, text), Theme.InformationTextColor, Theme.InformationBackgroundColor);

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
        WriteMessage(string.Format(Strings.MessageTextFormat, text), Theme.MessageTextColor, Theme.DefaultBackgroundColor);

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

        CurrentProgressMessage = string.Format(Strings.ProgressTextFormat, text);
        CurrentProgressValue = 0;
        CurrentCustomProgressValue = null;

        WriteProgressMessage();

        HideCursor();

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

        CurrentProgressMessage = string.Format(Strings.ProgressTextFormat, text);
        CurrentProgressValue = progressValue;
        CurrentCustomProgressValue = null;

        WriteProgressMessage();
        WriteProgressValue(progressValue);

        HideCursor();

        return this;
    }

    /// <summary>
    /// Writes the progress message to the standard output stream.
    /// </summary>
    /// <param name="text">The progress message text</param>
    /// <param name="customProgressValue">The custom progress value with which to start</param>
    public ConsoleWriter StartProgress(string text, string customProgressValue)
    {
        InProgress = true;

        CurrentProgressMessage = string.Format(Strings.ProgressTextFormat, text);
        CurrentProgressValue = 0;
        CurrentCustomProgressValue = customProgressValue;

        WriteProgressMessage();
        WriteProgressValue(customProgressValue);

        HideCursor();

        return this;
    }

    /// <summary>
    /// Writes a success label and text and the line terminator to the standard output stream.
    /// </summary>
    /// <param name="text">The success text</param>
    public ConsoleWriter Success(string text)
    {
        return Success(Strings.SuccessLabel, text);
    }

    /// <summary>
    /// Writes a success label and text and the line terminator to the standard output stream.
    /// </summary>
    /// <param name="label">The success label</param>
    /// <param name="text">The success text</param>
    public ConsoleWriter Success(string label, string text)
    {
        WriteLabel(label, Theme.SuccessLabelTextColor, Theme.SuccessLabelBackgroundColor);
        WriteMessage(string.Format(Strings.SuccessFormat, text), Theme.SuccessTextColor, Theme.SuccessBackgroundColor);

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
        return Warning(Strings.WarningLabel, text);
    }

    /// <summary>
    /// Writes a warning label and text and the line terminator to the standard output stream.
    /// </summary>
    /// <param name="label">The warning label</param>
    /// <param name="text">The warning text</param>
    public ConsoleWriter Warning(string label, string text)
    {
        WriteLabel(label, Theme.WarningLabelTextColor, Theme.WarningLabelBackgroundColor);
        WriteMessage(string.Format(Strings.WarningFormat, text), Theme.WarningTextColor, Theme.WarningBackgroundColor);

        LogWarning(_cachedMessage);
        ClearInternalCache();

        return NewLine();
    }

    /// <summary>
    /// Restores the cursor after dynamic progress output.
    /// </summary>
    private static void RestoreCursor()
    {
        if (!Console.IsOutputRedirected)
        {
            Console.CursorVisible = true;
        }
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
    /// Calculates the progress percentage based on the given progress value, minimum progress value, and maximum progress value.
    /// </summary>
    private double GetProgressPercentage(long progressValue)
    {
        if (MaxProgressValue <= MinProgressValue)
        {
            throw new InvalidOperationException(
                $"{nameof(MaxProgressValue)} must be greater than {nameof(MinProgressValue)}.");
        }

        var value = (double)progressValue - MinProgressValue;
        var range = (double)MaxProgressValue - MinProgressValue;

        return value / range * 100d;
    }

    /// <summary>
    /// Get string representation of a progress value
    /// </summary>
    private string GetProgressValueStr(long progressValue)
    {
        switch (ProgressValueUnit)
        {
            case ProgressUnit.Percent:
                return $"{GetProgressPercentage(progressValue),3:##0}%";

            case ProgressUnit.Count:
                return MaxProgressValue < long.MaxValue
                    ? $"{progressValue}/{MaxProgressValue}"
                    : progressValue.ToString();

            case ProgressUnit.FileSize:
                var fileSize = new FileSize(progressValue);

                if (MaxProgressValue < long.MaxValue)
                {
                    var percentValue = GetProgressPercentage(progressValue);

                    if (fileSize.Value >= 1024)
                    {
                        return $"{fileSize.ToString(FileSizeUnit.BytesOnly)} ({fileSize}), {percentValue,3:##0}%";
                    }
                    else
                    {
                        return $"{fileSize.ToString(FileSizeUnit.BytesOnly)}, {percentValue,3:##0}%";
                    }
                }
                else
                {
                    if (fileSize.Value >= 1024)
                    {
                        return $"{fileSize.ToString(FileSizeUnit.BytesOnly)} ({fileSize})";
                    }
                    else
                    {
                        return fileSize.ToString(FileSizeUnit.BytesOnly);
                    }
                }

            default:
                return string.Empty;
        }
    }

    /// <summary>
    /// Hides the cursor during dynamic progress output.
    /// </summary>
    private void HideCursor()
    {
        if (!NoProgress && !Console.IsOutputRedirected)
        {
            Console.CursorVisible = false;
        }
    }

    /// <summary>
    /// Logs the successful completion to abstract log
    /// </summary>
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
        if (_logger.IsEnabled(LogLevel.Error))
        {
            _logger.LogError("{Message}", message);
        }
    }

    /// <summary>
    /// Logs failed progress to abstract log
    /// </summary>
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
    private void LogInformation(string message)
    {
        if (_logger.IsEnabled(LogLevel.Information))
        {
            _logger.LogInformation("{Message}", message);
        }
    }

    /// <summary>
    /// Logs warning message to abstract log
    /// </summary>
    private void LogWarning(string message)
    {
        if (_logger.IsEnabled(LogLevel.Warning))
        {
            _logger.LogWarning("{Message}", message);
        }
    }
    /// <summary>
    /// Writes a label to the standard output stream.
    /// </summary>
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
    private void WriteProgressValue(long progressValue)
    {
        WriteProgressValue(GetProgressValueStr(progressValue));
    }

    /// <summary>
    /// Overrides the current progress value with the given progress value as text to the 
    /// standard output stream.
    /// </summary>
    private void WriteProgressValue(string customProgressValue)
    {
        _cachedProgressValue = customProgressValue;

        if (NoProgress || Console.IsOutputRedirected)
        {
            return;
        }

        var progressValueLength = customProgressValue.Length;
        var progressMessageLength = MaxMessageLength - progressValueLength - 1;

        if (progressMessageLength <= 0)
        {
            return;
        }

        Console.Write(new string('\b', MaxMessageLength + 1));

        Console.ForegroundColor = Theme.ProgressTextColor;

        Console.Write(StringExtensions.CutOrPadRight(CurrentProgressMessage, progressMessageLength));
        Console.Write(' ');

        Console.ForegroundColor = Theme.ProgressValueColor;

        Console.Write(customProgressValue);
        Console.Write(' ');

        Console.ForegroundColor = Theme.DefaultTextColor;
        Console.BackgroundColor = Theme.DefaultBackgroundColor;
    }
    /// <summary>
    /// Writes the given status as text to the standard output stream.
    /// </summary>
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
