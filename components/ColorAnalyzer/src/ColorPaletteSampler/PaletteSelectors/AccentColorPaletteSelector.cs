// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

namespace CommunityToolkit.WinUI.Helpers;

/// <summary>
/// A <see cref="ColorPaletteSelector"/> based on the three most "colorful" colors.
/// </summary>
public class AccentColorPaletteSelector : ColorPaletteSelector
{
    /// <inheritdoc/>
    protected override IEnumerable<PaletteColor> ApplySelector(IEnumerable<PaletteColor> palette)
    {
        // Select accent colors
        return palette.OrderByDescending(x => ColorExtensions.FindColorfulness(x.Color));
    }
}
