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
/// Progress results
/// </summary>
public enum ProgressResult
{
	/// <summary>
	/// Operation was successful
	/// </summary>
	OK,

    /// <summary>
    /// Operation was canceled
    /// </summary>
    Canceled,

    /// <summary>
    /// Operation failed
    /// </summary>
    Failed
}