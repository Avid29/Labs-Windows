// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using CommunityToolkit.WinUI.Controls.NodeGraphRns.Interfaces;

namespace CommunityToolkit.WinUI.Controls.NodeGraphRns;

public class NodeCanvas : Panel
{
    /// <inheritdoc/>
    protected override Size ArrangeOverride(Size finalSize)
    {
        foreach (var child in Children)
        {
            if (child is not INodeCanvasChild item)
            {
                // TODO: Improve exception
                throw new Exception();
            }

            child.Arrange(item.Bounds);
        }

        return finalSize;
    }

    /// <inheritdoc/>
    protected override Size MeasureOverride(Size availableSize)
    {
        foreach (var child in Children)
        {
            child.Measure(availableSize);
        }

        return availableSize;
    }
}
