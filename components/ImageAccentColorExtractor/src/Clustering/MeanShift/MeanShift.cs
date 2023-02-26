// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using CommunityToolkit.Labs.WinUI.ImageAccentColorExtractorRns.Clustering.Interfaces;

namespace CommunityToolkit.Labs.WinUI.ImageAccentColorExtractorRns.Clustering.MeanShift;

// MeanShift finds clusters by moving clusters towards a convergence point.
// This initial position of a cluster is a clone of a corresponding point. If clusters share a position they can be merged into one.
// 
// Mathematically, the convergence point can be found by graphing the distribution from each point.
// After summing these distributions, the nearest local maxima to a cluster's initial
// position is that cluster's convergence point.
// 
// Clusters at 1, 4, 4.5 and 5. Overlaid
// 
//           *                             *    *    *
//         *   *                         *   **   **   *
//       *       *                     *    *  * *  *    *
//     *           *                 *    *    * *    *    *
//   *               *             *    *    *     *    *    *
// *                   *         *    *    *         *    *    *
// 0 - - - - 1 - - - - 2 - - - - 3 - - - - 4 - - - - 5 - - - - 6
//           ·                             ·    ·    ·
// 
// Clusters at 1, 4, 4.5 and 5. Summed
// 
//                                              *
//                                            *   *
//                                          *       *
//                                         *         *
//                                         *         *
//                                        *           *
//                                       *             *
//           *                          *               *
//         *   *                       *                 *
//       *       *                    *                   *
//     *           *                 *                     *
//   *               *             *                         *
// *                   *         *                             *
// 0 - - - - 1 - - - - 2 - - - - 3 - - - - 4 - - - - 5 - - - - 6
//           ·                             ·    ·    ·
// 
// The clusters would be 1 and 4.5, because those are all the local maximas.
// The clusters weighted would be (1, 1) and (4.5, 3) because 1 point went to the local max at 1 and 3 points went to the local max at 3.
// 
// 
// Programmatically, these clusters are found by continually shifting each cluster towards their convergence point.
// Each shift is performed by finding the cluster's distance from each point then weighting its effect on the cluster.
// These weights are then used to find a weighted average, the result of each is the new cluster position.
// 
// Floating point errors prevent the clusters from perfectly converging. As a result Connected Components will be used to merge
// similar clusters after convergence.

/// <summary>
/// A static class containing Mean Shift methods.
/// </summary>
internal static class MeanShift
{
    private const double ACCEPTED_ERROR = 0.000005;

    /// <summary>
    /// Clusters a set of points using MeanShift clustering.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <typeparam name="TShape"></typeparam>
    /// <typeparam name="TKernel"></typeparam>
    /// <param name="points"></param>
    /// <param name="field"></param>
    /// <param name="kernel"></param>
    /// <param name="shape"></param>
    /// <returns></returns>
    public static (T, double)[] Cluster<T, TShape, TKernel>(
        ReadOnlySpan<T> points,
        ReadOnlySpan<T> field,
        TKernel kernel,
        TShape shape = default)
        where T : unmanaged
        where TShape : struct, IClusterShape<T>
        where TKernel : struct, IKernel
    {
        var weightedPoints = MakeWeighted(points);
        var weightedField = MakeWeighted(field);

        return Cluster(weightedPoints, weightedField, kernel, shape);
    }

    public static unsafe (T, double)[] Cluster<T, TShape, TKernel>(
        ReadOnlySpan<(T, double)> points,
        ReadOnlySpan<(T, double)> field,
        TKernel kernel,
        TShape shape = default)
        where T : unmanaged
        where TShape : struct, IClusterShape<T>
        where TKernel : struct, IKernel
    {
        // Points will be cloned into a modifiable list of clusters
        var clusters = new (T, double)[points.Length];

        // This array will be reused on every iteration
        // However we allocate it here once to save on allocation time and space
        var fieldWeights = new (T, double)[field.Length];

        // Fix points in memory so it can be accessed as a pointer
        fixed ((T, double)* p = points)
        {
            // MeanShift each point
            for (var i = 0; i < clusters.Length; i++)
            {
                var point = points[i];
                var clusterPoint = MeanShiftPoint(point.Item1, p, points.Length, shape, kernel, fieldWeights);
                (T, double) cluster = (clusterPoint, point.Item2);
                clusters[i] = cluster;
            }
        }

        return PostProcess(clusters, kernel, shape);
    }

    private static unsafe T MeanShiftPoint<T, TShape, TKernel>(
        T cluster,
        (T, double)* field,
        int fieldSize,
        TShape shape,
        TKernel kernel,
        (T, double)[] fieldWeights)
        where T : unmanaged
        where TShape : struct, IClusterShape<T>
        where TKernel : struct, IKernel
    {
        var changed = true;

        // Shift point until it converges
        while (changed)
        {
            // Determine weight of all field points from the cluster's current position.
            for (var i = 0; i < fieldSize; i++)
            {
                var point = field[i];
                var distSqrd = shape.FindDistanceSquared(cluster, point.Item1);
                var weight = kernel.WeightDistance(distSqrd);
                fieldWeights[i] = (point.Item1, weight * point.Item2);
            }

            var newCluster = shape.WeightedAverage(fieldWeights);
            changed = shape.FindDistanceSquared(cluster, newCluster) > ACCEPTED_ERROR;
            cluster = newCluster;
        }

        return cluster;
    }

    private static (T, double)[] PostProcess<T, TShape, TKernel>(
        (T, double)[] clusters,
        TKernel kernel,
        TShape shape)
        where T : unmanaged
        where TShape : struct, IClusterShape<T>
        where TKernel : struct, IKernel
    {
        // Merge explicitly duplicate values
        Dictionary<T, double> mergeMap = new();
        foreach (var cluster in clusters)
        {
            if (mergeMap.ContainsKey(cluster.Item1))
            {
                mergeMap[cluster.Item1] += cluster.Item2;
            }
            else
            {
                mergeMap.Add(cluster.Item1, cluster.Item2);
            }
        }

        // Convert map back to array
        clusters = new (T, double)[mergeMap.Count];
        int i = 0;
        foreach (var cluster in mergeMap)
        {
            clusters[i] = (cluster.Key, cluster.Value);
            i++;
        }

        return ConnectedComponents.ConnectedComponents.Cluster<T, TShape>(clusters, kernel.WindowSize, shape).ToArray();
    }


    /// <summary>
    /// Merges a set of points into a list of weighted unique points.
    /// </summary>
    private static ReadOnlySpan<(T, double)> MakeWeighted<T>(ReadOnlySpan<T> points)
        where T : unmanaged
    {
        // Merge equal points
        Dictionary<T, double> mergeMap = new();
        foreach (var cluster in points)
        {
            if (mergeMap.ContainsKey(cluster))
            {
                mergeMap[cluster]++;
            }
            else
            {
                mergeMap.Add(cluster, 1);
            }
        }

        // Convert back to tuple array
        (T, double)[] weightedPoints = new (T, double)[mergeMap.Count];
        int i = 0;
        foreach (var entry in mergeMap)
        {
            weightedPoints[i] = (entry.Key, entry.Value);
            i++;
        }

        return weightedPoints;
    }
}
