// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using CommunityToolkit.WinUI.Controls.AccentColorExtractorRns.Analysis.Cluster.Shape.Interfaces;
using Windows.UI;

namespace CommunityToolkit.WinUI.Controls.AccentColorExtractorRns.Analysis.Cluster.Shape;

internal class ColorSpace : ISpace<Color>
{
    /// <inheritdoc/>
    public Color Average(List<Color> points)
    {
        var a = 0;
        var r = 0;
        var g = 0;
        var b = 0;

        for (var i = 0; i < points.Count; i++)
        {
            var c = points[i];
            a += c.A;
            r += c.R;
            g += c.G;
            b += c.B;
        }

        a /= points.Count;
        r /= points.Count;
        g /= points.Count;
        b /= points.Count;

        return Color.FromArgb((byte)a, (byte)r, (byte)g, (byte)b);
    }
    
    /// <inheritdoc/>
    public double Distance(Color item1, Color item2)
    {
        var deltaA = item1.A - item2.A;
        var deltaR = item1.R - item2.R;
        var deltaG = item1.G - item2.G;
        var deltaB = item1.B - item2.B;

        deltaA *= deltaA;
        deltaR *= deltaR;
        deltaG *= deltaG;
        deltaB *= deltaB;

        return Math.Sqrt(deltaA + deltaR + deltaG + deltaB);
    }
}
