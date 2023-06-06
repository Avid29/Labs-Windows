// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using CommunityToolkit.WinUI.Controls.AccentColorExtractorRns.Analysis.Cluster.Shape.Interfaces;
using System.Runtime.InteropServices;

namespace CommunityToolkit.WinUI.Controls.AccentColorExtractorRns.Analysis.Cluster;

internal static class KMeans
{
    internal static T[] Cluster<T>(List<T> points, int k, ISpace<T> space)
    {
        if (k < 1)
        {
            throw new ArgumentOutOfRangeException(nameof(k), k, $"{nameof(k)} must be greater than or equal to one (1).");
        }

        if (k == 1)
        {
            var avg = space.Average(points);
            return new T[] { avg };
        }

        // Split the points arbitrarily into initial clusters
        var clusters = Split(points, k, space);

        // Iterate shifting every point until convergence
        var changed = true;
        while (changed)
        {
            for (var i = 0; i < clusters.Length; i++)
            {
                var cluster = clusters[i];
                for (var pointIndex = 0; pointIndex < cluster.Points.Count; pointIndex++)
                {
                    var point = cluster.Points[pointIndex];

                    var nearestClusterIndex = FindNearestClusterIndex(point, clusters, space);

                    // Already in nearest cluster
                    if (nearestClusterIndex == i)
                        continue;

                    // Cluster cannot be made empty
                    if (cluster.Points.Count <= 1)
                        continue;

                    // Move point to nearer cluster
                    cluster.Points.RemoveAt(pointIndex);
                    clusters[nearestClusterIndex].Points.Add(point);
                    changed = true;
                }
            }
        }

        // Get the centroid of every cluster
        return clusters.Select(x => x.Centroid).ToArray();
    }

    private static int FindNearestClusterIndex<T>(T point, KCluster<T>[] clusters, ISpace<T> space)
    {
        var minDist = double.PositiveInfinity;
        var nearestIndex = -1;

        for (var k = 0; k < clusters.Length; k++)
        {
            var dist = space.Distance(point, clusters[k].Centroid);

            if (dist < minDist)
            {
                minDist = dist;
                nearestIndex = k;
            }
        }

        return nearestIndex;
    }

    private static KCluster<T>[] Split<T>(List<T> points, int k, ISpace<T> space)
    {
        var clusters = new KCluster<T>[k];
        var clusterSize = points.Count / k;

        for (var i = 0; i < k; i++)
        {
            var cluster = new KCluster<T>(space);

            var start = i * clusterSize;
            var end = Math.Min(points.Count - 1, (i + 1) * clusterSize);

            for (var j = start; j < end; j++)
            {
                var p = points[j];
                cluster.Points.Add(p);
            }

            clusters[i] = cluster;
        }

        return clusters;
    }

    private class KCluster<T>
    {
        private ISpace<T> _space;

        public KCluster(ISpace<T> space)
        {
            Points = new List<T>();
            _space = space;
        }

        public List<T> Points { get; }

        public T Centroid => _space.Average(Points);
    }
}
