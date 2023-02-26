// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

namespace CommunityToolkit.Labs.WinUI.ImageAccentColorExtractorRns.Clustering.Interfaces;

internal interface IClusterShape<T>
{
    /// <summary>
    /// Gets the distance between <paramref name="it1"/> and <paramref name="it2"/>.
    /// </summary>
    /// <param name="it1">Point A.</param>
    /// <param name="it2">Point B.</param>
    /// <returns>The distance between point A and point B.</returns>
    double FindDistanceSquared(T it1, T it2);

    /// <summary>
    /// Gets the weighted average value of a list of (T, double) by point and weight.
    /// </summary>
    /// <param name="items">A weighted list of points.</param>
    /// <returns>The weighted center of the points.</returns>
    T WeightedAverage((T, double)[] items);
}
