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
        hsv.S = LoopClamp(hsv.S, MinimumSaturation, MaximumSaturation);
        hsv.V = LoopClamp(hsv.V, MinimumValue, MaximumValue);
        return ColorHelper.FromHsv(hsv.H, hsv.S, hsv.V);
    }

    /// <summary>
    /// LoopClamp allows a clamp region to be inverted, rounding to the nearest valid value if in the dead-zone above max and below min.
    /// </summary>
    private static double LoopClamp(double value, double min, double max)
    {
        // Min is less than max.
        // Apply regular clamp
        if (min < max)
        {
            return Math.Clamp(value, min, max);
        }

        // Value is less than max or greater than min
        if (value < max || value > min)
        {
            return value;
        }

        // The value is in the deadzone, round to nearest valid value
        return value - max < min - value ? max : min;
    }
}
