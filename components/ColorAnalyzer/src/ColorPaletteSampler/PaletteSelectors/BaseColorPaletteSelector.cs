// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

namespace CommunityToolkit.WinUI.Helpers;

/// <summary>
/// A <see cref="ColorPaletteSelector"/> based on the least "colorful" color.
/// </summary>
public class BaseColorPaletteSelector : ColorPaletteSelector
{
    /// <inheritdoc/>
    protected override IEnumerable<PaletteColor> ApplySelector(IEnumerable<PaletteColor> palettes)
    {
        // Get base color
        return palettes.OrderBy(x => ColorExtensions.FindColorfulness(x.Color));
    }
}
