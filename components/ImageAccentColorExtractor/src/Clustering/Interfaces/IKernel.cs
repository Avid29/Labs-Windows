// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

namespace CommunityToolkit.Labs.WinUI.ImageAccentColorExtractorRns.Clustering.Interfaces;

internal interface IKernel
{
    /// <summary>
    /// Gets or sets the windows size of the kernel.
    /// </summary>
    double WindowSize { get; set; }

    /// <summary>
    /// Gets the weighted relevance of a point at sqrt(<paramref name="distanceSquared"/>) away.
    /// </summary>
    /// <param name="distanceSquared">The distance^2 of the point to be weighted.</param>
    /// <returns>The weight of a point at sqrt(<paramref name="distanceSquared"/>) away.</returns>
    double WeightDistance(double distanceSquared);
}
