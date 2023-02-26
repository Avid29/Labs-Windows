// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using CommunityToolkit.Labs.WinUI.ImageAccentColorExtractorRns.Clustering.Interfaces;

namespace CommunityToolkit.Labs.WinUI.ImageAccentColorExtractorRns.Clustering.ConnectedComponents;

/// <summary>
/// A struct used to wrap the scope for a connected components.
/// </summary>
/// <typeparam name="T">The type of points to cluster.</typeparam>
/// <typeparam name="TShape">The type of shape to use on the points to cluster.</typeparam>
internal unsafe ref struct CCContext<T, TShape>
    where T : unmanaged
    where TShape : struct, IClusterShape<T>
{
    public CCContext(double epsilon, TShape shape, (T, double)* points, int pointsLength)
    {
        CurrentClusterId = 0;
        ClusterIds = new int[pointsLength];
        Episilon2 = epsilon * epsilon;
        Points = points;
        PointsLength = pointsLength;
        Shape = shape;
    }

    /// <summary>
    /// Gets or sets the next clusterId.
    /// </summary>
    public int CurrentClusterId { get; set; }

    /// <summary>
    /// Gets an array containing the cluster ids of each point in <see cref="Points"/>.
    /// </summary>
    public int[] ClusterIds { get; }

    /// <summary>
    /// Gets the epsilon squared. The max distance squared to consider two points connected.
    /// </summary>
    public double Episilon2 { get; }

    /// <summary>
    /// Gets a pointer to a span containing all the points being clustered.
    /// </summary>
    public (T, double)* Points { get; }

    /// <summary>
    /// Gets the number of items in the <see cref="Points"/>.
    /// </summary>
    public int PointsLength { get; }

    /// <summary>
    /// Gets or sets the shape to use on the points to cluster. 
    /// </summary>
    public TShape Shape { get; }
}
