// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

namespace CommunityToolkit.WinUI.Controls.NodeGraphRns;

/// <summary>
/// An example templated control.
/// </summary>
public class NodeGraph : Selector
{
    public const string NodeCanvasPartName = "NodeCanvas";

    /// <summary>
    /// Initializes a new instance of the <see cref="NodeGraph"/> class.
    /// </summary>
    public NodeGraph()
    {
        this.DefaultStyleKey = typeof(NodeGraph);

        // Allows directly using this control as the x:DataType in the template.
        this.DataContext = this;
    }
}
