// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using Windows.UI;

namespace CommunityToolkit.WinUI.Helpers;

/// <summary>
/// An <see cref="IColorConstrainer"/> that restricts 
/// </summary>
public class HsvColorConstrainer : IColorConstrainer
{
    /// <summary>
    /// Gets or sets the minimum saturation.
    /// </summary>
    public double MinimumSaturation { get; set; } = 0;

    /// <summary>
    /// Gets or sets the maximum saturation.
    /// </summary>
    public double MaximumSaturation { get; set; } = 1;

    /// <summary>
    /// Gets or sets the minimum value.
    /// </summary>
    public double MinimumValue { get; set; } = 0;

    /// <summary>
    /// Gets or sets the maximum value.
    /// </summary>
    public double MaximumValue { get; set; } = 1;

    /// <inheritdoc/>
    public Color Clamp(Color color)
    {
        HsvColor hsv = color.ToHsv();
        hsv.S = Math.Clamp(hsv.S, MinimumSaturation, MaximumSaturation);
        hsv.V = Math.Clamp(hsv.V, MinimumValue, MaximumValue);
        return ColorHelper.FromHsv(hsv.H, hsv.S, hsv.V);
    }
}
