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

namespace Enbrea.Konsoli;

/// <summary>
/// Theming for <see cref="ConsoleWriter"/>.
/// </summary>
public class ConsoleWriterTheme
{
    /// <summary>
    /// Initializes a new instance of the <see cref="ConsoleWriterTheme"/> class.
    /// </summary>
    public ConsoleWriterTheme()
    {
        CaptionTextColor = ConsoleColor.Yellow;
        DefaultBackgroundColor = Console.BackgroundColor;
        DefaultTextColor = Console.ForegroundColor;
        ErrorBackgroundColor = Console.BackgroundColor; 
        ErrorLabelBackgroundColor = ConsoleColor.Red;
        ErrorLabelTextColor = ConsoleColor.White;
        ErrorTextColor = ConsoleColor.Red;
        FailedLabel = Strings.Failed;
        FailedLabelBackgroundColor = ConsoleColor.Red;
        FailedLabelTextColor = ConsoleColor.White;
        InformationBackgroundColor = Console.BackgroundColor; 
        InformationLabelBackgroundColor = ConsoleColor.Blue;
        InformationLabelTextColor = ConsoleColor.White;
        InformationTextColor = ConsoleColor.White;
        MessageTextColor = Console.ForegroundColor;
        OkLabelBackgroundColor = ConsoleColor.Green;
        OkLabelColor = ConsoleColor.White;
        ProgressTextColor = Console.ForegroundColor;
        ProgressValueColor = Console.ForegroundColor;
        SuccessBackgroundColor = Console.BackgroundColor;
        SuccessLabelBackgroundColor = ConsoleColor.Green;
        SuccessLabelTextColor = ConsoleColor.White;
        SuccessTextColor = ConsoleColor.Green;
        WarningBackgroundColor = Console.BackgroundColor;
        WarningLabelBackgroundColor = ConsoleColor.DarkYellow;
        WarningLabelTextColor = ConsoleColor.Black;
        WarningTextColor = ConsoleColor.DarkYellow;
    }

    /// <summary>
    /// Text color of caption 
    /// </summary>
    public ConsoleColor CaptionTextColor { get; set; }
    
    /// <summary>
    /// Default background color
    /// </summary>
    public ConsoleColor DefaultBackgroundColor { get; set; }

    /// <summary>
    /// Default text color
    /// </summary>
    public ConsoleColor DefaultTextColor { get; set; }

    /// <summary>
    /// Background color of error text
    /// </summary>
    public ConsoleColor ErrorBackgroundColor { get; set; }

    /// <summary>
    /// Background color of error label 
    /// </summary>
    public ConsoleColor ErrorLabelBackgroundColor { get; set; }

    /// <summary>
    /// Text color of error label
    /// </summary>
    public ConsoleColor ErrorLabelTextColor { get; set; }

    /// <summary>
    /// Text color of error text
    /// </summary>
    public ConsoleColor ErrorTextColor { get; set; }

    /// <summary>
    /// Text of <see cref="ProgressResult.Failed"/>
    /// </summary>
    public string FailedLabel { get; set; }

    /// <summary>
    /// Background color of <see cref="ProgressResult.Failed"/>
    /// </summary>
    public ConsoleColor FailedLabelBackgroundColor { get; set; }
    /// <summary>
    /// Text color of <see cref="ProgressResult.Failed"/>
    /// </summary>
    public ConsoleColor FailedLabelTextColor { get; set; }

    /// <summary>
    /// Background color of info text
    /// </summary>
    public ConsoleColor InformationBackgroundColor { get; set; }

    /// <summary>
    /// Background color of info label
    /// </summary>
    public ConsoleColor InformationLabelBackgroundColor { get; set; }

    /// <summary>
    /// Text color of info label
    /// </summary>
    public ConsoleColor InformationLabelTextColor { get; set; }

    /// <summary>
    /// Text color of info text
    /// </summary>
    public ConsoleColor InformationTextColor { get; set; }

    /// <summary>
    /// Text color of messages
    /// </summary>
    public ConsoleColor MessageTextColor { get; set; }

    /// <summary>
    /// Background color of <see cref="ProgressResult.OK"/>
    /// </summary>
    public ConsoleColor OkLabelBackgroundColor { get; set; }

    /// <summary>
    /// Text color of <see cref="ProgressResult.OK"/>
    /// </summary>
    public ConsoleColor OkLabelColor { get; set; }

    /// <summary>
    /// Text color of progress text
    /// </summary>
    public ConsoleColor ProgressTextColor { get; set; }

    /// <summary>
    /// Text color of progress values
    /// </summary>
    public ConsoleColor ProgressValueColor { get; set; }

    /// <summary>
    /// Background color of success text
    /// </summary>
    public ConsoleColor SuccessBackgroundColor { get; set; }

    /// <summary>
    /// Background color of success label
    /// </summary>
    public ConsoleColor SuccessLabelBackgroundColor { get; set; }

    /// <summary>
    /// Text color of success label
    /// </summary>
    public ConsoleColor SuccessLabelTextColor { get; set; }

    /// <summary>
    /// Text color of success text
    /// </summary>
    public ConsoleColor SuccessTextColor { get; set; }

    /// <summary>
    /// Background color of warning text
    /// </summary>
    public ConsoleColor WarningBackgroundColor { get; set; }

    /// <summary>
    /// Background color of warning label
    /// </summary>
    public ConsoleColor WarningLabelBackgroundColor { get; set; }

    /// <summary>
    /// Text color of warning label 
    /// </summary>
    public ConsoleColor WarningLabelTextColor { get; set; }

    /// <summary>
    /// Text color of warning text
    /// </summary>
    public ConsoleColor WarningTextColor { get; set; }
}