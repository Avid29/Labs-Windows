// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using CommunityToolkit.WinUI.Controls.NodeGraphRns.Interfaces;

namespace CommunityToolkit.WinUI.Controls.NodeGraphRns.Nodes;

public sealed partial class Node : Control, INodeCanvasChild
{
    public Node()
    {
        this.DefaultStyleKey = typeof(Node);
    }

    public Point Location { get; }

    public Size Size { get; }

    public Rect Bounds => new(Location, Size);
}
