// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using CommunityToolkit.Labs.WinUI.ImageAccentColorExtractorRns.Clustering.Interfaces;

namespace CommunityToolkit.Labs.WinUI.ImageAccentColorExtractorRns.Clustering.ConnectedComponents;

internal static class ConnectedComponents
{
    /// <summary>
    /// Clusters a set of points using connected components.
    /// </summary>
    /// <typeparam name="T">The type of points to cluster.</typeparam>
    /// <typeparam name="TShape">The type of shape to use on the points to cluster.</typeparam>
    /// <param name="points">The set of points to cluster.</param>
    /// <param name="epsilon">The max distance where two points are considered connected.</param>
    /// <param name="shape">The shape to use on the points to cluster.</param>
    /// <returns>A list of clusters.</returns>
    public static unsafe List<(T, double)> Cluster<T, TShape>(
        ReadOnlySpan<(T, double)> points,
        double epsilon,
        TShape shape = default)
        where T : unmanaged
        where TShape : struct, IClusterShape<T>
    {
        var clusters = new List<(T, double)>();

        fixed ((T, double)* p = points)
        {
            // Create a CCContext to avoid passing too many values between functions
            CCContext<T, TShape> context = new(epsilon, shape, p, points.Length);

            for (int i = 0; i < points.Length; i++)
            {
                // Create cluster if the point is unclassified
                // Otherwise the point is already classified
                if (context.ClusterIds[i] == 0)
                {
                    var size = FillCluster(i, context);
                    clusters.Add((points[i].Item1, size * points[i].Item2));
                }
            }
        }

        return clusters;
    }

    public static unsafe int FillCluster<T, TShape>(
        int i,
        CCContext<T, TShape> context)
        where T : unmanaged
        where TShape : struct, IClusterShape<T>
    {
        int id = ++context.CurrentClusterId;
        int size = 1;
        context.ClusterIds[i] = id;
        var iPoint = context.Points[i];
        for (int j = i+1; j < context.PointsLength; j++)
        {
            var jPoint = context.Points[j];
            if (context.Shape.FindDistanceSquared(iPoint.Item1, jPoint.Item1) < context.Episilon2)
            {
                context.ClusterIds[j] = id;
                size++;
            }
        }

        return size;
    }
}
