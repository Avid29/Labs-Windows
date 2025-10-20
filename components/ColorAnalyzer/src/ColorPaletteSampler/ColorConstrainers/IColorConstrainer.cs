// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using Windows.UI;

namespace CommunityToolkit.WinUI.Helpers;

/// <summary>
/// An <see langword="interface"/> for an object that defines and performs contraints on a color.
/// </summary>
public interface IColorConstrainer
{
    /// <summary>
    /// Applies the constraints defined by the <see cref="IColorConstrainer"/> on a color.
    /// </summary>
    /// <param name="color">The original color.</param>
    /// <returns>The constrained color.</returns>
    public Color Clamp(Color color);
}
