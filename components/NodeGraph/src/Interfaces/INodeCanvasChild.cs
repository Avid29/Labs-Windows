// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

namespace CommunityToolkit.WinUI.Controls.NodeGraphRns.Interfaces;

public interface INodeCanvasChild
{
    /// <summary>
    /// Gets the item location.
    /// </summary>
    Point Location { get; }

    /// <summary>
    /// Gets the item size.
    /// </summary>
    Size Size { get; }

    /// <summary>
    /// Gets the item bounds.
    /// </summary>
    Rect Bounds { get; }
}
