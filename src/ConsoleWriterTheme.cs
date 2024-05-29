#region ENBREA Konsoli - Copyright (C) STÜBER SYSTEMS GmbH
/*    
 *    ENBREA Konsoli 
 *    
 *    Copyright (C) STÜBER SYSTEMS GmbH
 *
 *    Licensed under the MIT License, Version 2.0. 
 * 
 */
#endregion

using System;

namespace Enbrea.Konsoli
{
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
            CaptionFormat = "{0}";
            CaptionTextColor = ConsoleColor.Yellow;
            DefaultBackgroundColor = Console.BackgroundColor;
            DefaultTextColor = Console.ForegroundColor;
            ErrorBackgroundColor = Console.BackgroundColor; 
            ErrorLabel = Strings.Error;
            ErrorLabelBackgroundColor = ConsoleColor.Red;
            ErrorLabelTextColor = ConsoleColor.White;
            ErrorTextColor = ConsoleColor.Red;
            ErrorTextFormat = "{0}";
            FailedLabel = Strings.Failed;
            FailedLabelBackgroundColor = ConsoleColor.Red;
            FailedLabelTextColor = ConsoleColor.White;
            InformationBackgroundColor = Console.BackgroundColor; 
            InformationLabel = Strings.Information;
            InformationLabelBackgroundColor = ConsoleColor.Blue;
            InformationLabelTextColor = ConsoleColor.White;
            InformationTextColor = ConsoleColor.White;
            InformationTextFormat = "{0}";
            MessageTextColor = Console.ForegroundColor;
            MessageTextFormat = "{0}";
            OkLabel = Strings.Ok;
            OkLabelBackgroundColor = ConsoleColor.Green;
            OkLabelColor = ConsoleColor.White;
            ProgressTextColor = Console.ForegroundColor;
            ProgressTextFormat = "{0}";
            ProgressValueColor = Console.ForegroundColor;
            SuccessBackgroundColor = Console.BackgroundColor;
            SuccessFormat = "{0}";
            SuccessLabel = Strings.Success;
            SuccessLabelBackgroundColor = ConsoleColor.Green;
            SuccessLabelTextColor = ConsoleColor.White;
            SuccessTextColor = ConsoleColor.Green;
            WarningBackgroundColor = Console.BackgroundColor;
            WarningFormat = "{0}";
            WarningLabel = Strings.Warning;
            WarningLabelBackgroundColor = ConsoleColor.DarkYellow;
            WarningLabelTextColor = ConsoleColor.Black;
            WarningTextColor = ConsoleColor.DarkYellow;
        }

        /// <summary>
        /// Caption text format string
        /// </summary>
        public string CaptionFormat { get; set; }

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
        /// Text for error label
        /// </summary>
        public string ErrorLabel { get; set; }

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
        /// Error text format string
        /// </summary>
        public string ErrorTextFormat { get; set; }

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
        /// Info label
        /// </summary>
        public string InformationLabel { get; set; }

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
        /// Info text format string
        /// </summary>
        public string InformationTextFormat { get; set; }

        /// <summary>
        /// Text color of messages
        /// </summary>
        public ConsoleColor MessageTextColor { get; set; }

        /// <summary>
        /// Message text format string
        /// </summary>
        public string MessageTextFormat { get; set; }

        /// <summary>
        /// Text of <see cref="ProgressResult.OK"/>
        /// </summary>
        public string OkLabel { get; set; }

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
        /// Progress text format string
        /// </summary>
        public string ProgressTextFormat { get; set; }

        /// <summary>
        /// Text color of progress values
        /// </summary>
        public ConsoleColor ProgressValueColor { get; set; }

        /// <summary>
        /// Background color of success text
        /// </summary>
        public ConsoleColor SuccessBackgroundColor { get; set; }

        /// <summary>
        /// Success text string format
        /// </summary>
        public string SuccessFormat { get; set; }

        /// <summary>
        /// Success label
        /// </summary>
        public string SuccessLabel { get; set; }

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
        /// Warning text string format
        /// </summary>
        public string WarningFormat { get; set; }

        /// <summary>
        /// Warning label 
        /// </summary>
        public string WarningLabel { get; set; }

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
}