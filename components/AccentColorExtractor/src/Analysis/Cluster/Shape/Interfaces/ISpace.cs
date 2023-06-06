// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

namespace CommunityToolkit.WinUI.Controls.AccentColorExtractorRns.Analysis.Cluster.Shape.Interfaces;

internal interface ISpace<T>
{
    /// <summary>
    /// Gets the distance between <paramref name="item1"/> and <paramref name="item2"/>.
    /// </summary>
    /// <param name="item1">The first point.</param>
    /// <param name="item2">The second point.</param>
    /// <returns>The distance between <paramref name="item1"/> and <paramref name="item2"/>.</returns>
    double Distance(T item1, T item2);

    /// <summary>
    /// Gets the average value of all points in <paramref name="points"/>.
    /// </summary>
    /// <param name="points">The points to average.</param>
    /// <returns>The average of all points in <paramref name="points"/>.</returns>
    T Average(List<T> points);
}
