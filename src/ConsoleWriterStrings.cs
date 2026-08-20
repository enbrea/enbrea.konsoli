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
public class ConsoleWriterStrings
{
    /// <summary>
    /// Initializes a new instance of the <see cref="ConsoleWriterStrings"/> class.
    /// </summary>
    public ConsoleWriterStrings()
    {
        Bytes = Strings.Bytes;
        CaptionFormat = "{0}";
        ErrorFormat = "{0}";
        ErrorLabel = Strings.Error;
        Failed = Strings.Failed;
        GB = Strings.GB;
        InformationFormat = "{0}";
        InformationLabel = Strings.Information;
        KB = Strings.KB;
        MB = Strings.MB;
        MessageFormat = "{0}";
        Ok = Strings.Ok;
        Canceled = Strings.Canceled;
        ProgressFormat = "{0}";
        SuccessFormat = "{0}";
        SuccessLabel = Strings.Success;
        TB = Strings.TB;
        WarningFormat = "{0}";
        WarningLabel = Strings.Warning;
    }

    public string Bytes { get; set; }
    public string CaptionFormat { get; set; }
    public string ErrorFormat { get; set; }
    public string ErrorLabel { get; set; }
    public string Failed { get; set; }
    public string GB { get; set; }
    public string InformationFormat { get; set; }
    public string InformationLabel { get; set; }
    public string KB { get; set; }
    public string MB { get; set; }
    public string MessageFormat { get; set; }
    public string Ok { get; set; }
    public string Canceled { get; set; }
    public string ProgressFormat { get; set; }
    public string SuccessFormat { get; set; }
    public string SuccessLabel { get; set; }
    public string TB { get; set; }
    public string WarningFormat { get; set; }
    public string WarningLabel { get; set; }
}