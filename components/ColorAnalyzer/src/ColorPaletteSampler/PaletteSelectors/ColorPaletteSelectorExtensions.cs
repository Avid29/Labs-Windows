// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using Windows.UI;

namespace CommunityToolkit.WinUI.Helpers;

/// <summary>
/// Extension methods for <see cref="ColorPaletteSelector"/>.
/// </summary>
public static class ColorPaletteSelectorExtensions
{
    /// <summary>
    /// Extends the list of colors to ensure it meets the minimum count by repeating the <paramref name="index"/>th color.
    /// </summary>
    /// <param name="colors">The list of colors to extend</param>
    /// <param name="minCount">The minimum number of colors required</param>
    /// <param name="index">The index of the item to repeat</param>
    public static IEnumerable<Color> EnsureMinColorCount(this IEnumerable<Color> colors, int minCount, int index = 0)
    {
        int i = 0;
        Color fallback = Colors.Transparent;
        foreach (var color in colors)
        {
            if (i == index)
            {
                fallback = color;
            }

            i++;
            yield return color;
        }

        if (i < minCount)
        {
            i++;
            yield return fallback;
        }
    }
}
