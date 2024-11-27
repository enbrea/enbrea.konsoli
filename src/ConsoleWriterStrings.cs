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

namespace Enbrea.Konsoli
{
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
            Error = Strings.Bytes;
            Failed = Strings.Bytes;
            GB = Strings.Bytes;
            KB = Strings.Bytes;
            MB = Strings.Bytes;
            Ok = Strings.Bytes;
            Success = Strings.Bytes;
            TB = Strings.Bytes;
            Warning = Strings.Bytes;
        }

        public string Bytes { get; set; }
        public string Error { get; set; }
        public string Failed { get; set; }
        public string GB { get; set; }
        public string Information { get; set; }
        public string KB { get; set; }
        public string MB { get; set; }
        public string Ok { get; set; }
        public string Success { get; set; }
        public string TB { get; set; }
        public string Warning { get; set; }
    }
}