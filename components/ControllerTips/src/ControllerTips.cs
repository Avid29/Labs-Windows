// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

namespace CommunityToolkit.WinUI.Controls.ControllerTipsRns;

public class ControllerTips : Control
{
    private static readonly DependencyProperty ItemsProperty =
        DependencyProperty.Register(nameof(Items), typeof(IList<ControllerTipItem>), typeof(ControllerTips), new PropertyMetadata(null));
    
    public ControllerTips()
    {
        this.DefaultStyleKey = typeof(ControllerTips);
    }

    /// <summary>
    /// Gets or sets the collection of controller tips to display.
    /// </summary>
    public IList<ControllerTipItem>? Items
    {
        get => (IList<ControllerTipItem>?)GetValue(ItemsProperty);
        set => SetValue(ItemsProperty, value);
    }
}
