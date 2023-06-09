// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using Windows.UI;

namespace CommunityToolkit.WinUI.Controls.AccentColorExtractorRns.Analysis.Cluster;

internal static class KMeans
{
    private const int MAX_ITERS = 50;

    internal static Color[] Cluster(Color[] points, int k)
    {
        if (k <= 1)
        {
            throw new ArgumentOutOfRangeException(nameof(k), k, $"{nameof(k)} must be greater than or equal to two (2).");
        }

        // Split the points arbitrarily into initial clusters
        var clusters = Split(points, k);

        // Iterate shifting every point until convergence
        var changed = true;
        int iters = 0;
        while (changed && iters < MAX_ITERS)
        {
            changed = false;

            for (var i = 0; i < clusters.Length; i++)
            {
                var cluster = clusters[i];
                for (var pointIndex = 0; pointIndex < cluster.Count; pointIndex++)
                {
                    var point = cluster[pointIndex];

                    var nearestClusterIndex = FindNearestClusterIndex(point, clusters);

                    // Already in nearest cluster
                    if (nearestClusterIndex == i)
                    {
                        continue;
                    }

                    // Cluster cannot be made empty
                    if (cluster.Count <= 1)
                    {
                        continue;
                    }

                    // Move point to nearer cluster
                    cluster.RemoveAt(pointIndex);
                    clusters[nearestClusterIndex].Add(point);
                    changed = true;
                }
            }

            iters++;
        }

        // Get the centroid of every cluster
        return clusters.Select(x => x.Centroid).ToArray();
    }

    private static int FindNearestClusterIndex(Color point, KCluster[] clusters)
    {
        var minDist = double.PositiveInfinity;
        var nearestIndex = -1;

        for (var k = 0; k < clusters.Length; k++)
        {
            var dist = DistanceSquared(point, clusters[k].Centroid);

            if (dist < minDist)
            {
                minDist = dist;
                nearestIndex = k;
            }
        }

        return nearestIndex;
    }

    private static double DistanceSquared(Color item1, Color item2)
    {
        // Get delta for each component
        var dR = item1.R - item2.R;
        var dG = item1.G - item2.G;
        var dB = item1.B - item2.B;
        // Ignore alpha

        return dR * dR + dG * dG + dB * dB;
    }

    private static KCluster[] Split(Color[] points, int k)
    {
        var clusters = new KCluster[k];
        var clusterSize = points.Length / k;

        for (var i = 0; i < k; i++)
        {
            var cluster = new KCluster();

            var start = i * clusterSize;
            var end = Math.Min(points.Length - 1, (i + 1) * clusterSize);

            for (var j = start; j < end; j++)
            {
                var p = points[j];
                cluster.Add(p);
            }

            clusters[i] = cluster;
        }

        return clusters;
    }

    private class KCluster
    {
        private readonly List<Color> _points;
        private int sumR;
        private int sumG;
        private int sumB;

        public KCluster()
        {
            this._points = new List<Color>();
        }

        public Color Centroid
        {
            get
            {
                var r = (byte)(sumR / Count);
                var g = (byte)(sumG / Count);
                var b = (byte)(sumB / Count);
                return Color.FromArgb(255, r, g, b);
            }
        }

        public int Count => _points.Count;

        public Color this[int index] => _points[index];

        public void Add(Color color)
        {
            _points.Add(color);
            sumR += color.R;
            sumG += color.G;
            sumB += color.B;
        }

        public void RemoveAt(int index)
        {
            var color = this._points[index];
            _points.RemoveAt(index);
            sumR -= color.R;
            sumG -= color.G;
            sumB -= color.B;
        }
    }
}
