// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

namespace CommunityToolkit.WinUI.Controls;

/// <summary>
/// An enum for selecting the title bar display mode. 
/// </summary>
public enum TitleBarDisplayMode
{
    /// <summary>
    /// Sets the caption buttons height to 32px.
    /// </summary>
    Standard,

    /// <summary>
    /// Sets the caption buttons height to 32px.
    /// </summary>
    Tall,

    #if WINAPPSDK
    /// <summary>
    /// Removes caption buttons and min height from the title bar.
    /// </summary>
    Collapsed
    #endif
}
