// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System.Windows.Input;
using Windows.System;

namespace CommunityToolkit.WinUI.Controls.ControllerTipsRns;

public class ControllerTipItem : Control
{
    private static readonly DependencyProperty KeyProperty =
        DependencyProperty.Register(nameof(Key), typeof(VirtualKey), typeof(ControllerTipItem), new PropertyMetadata(null));

    private static readonly DependencyProperty CommandProperty =
        DependencyProperty.Register(nameof(Command), typeof(ICommand), typeof(ControllerTipItem), new PropertyMetadata(null));

    private static readonly DependencyProperty CommandParameterProperty =
        DependencyProperty.Register(nameof(CommandParameter), typeof(object), typeof(ControllerTipItem), new PropertyMetadata(null));

    public ControllerTipItem()
    {
        this.DefaultStyleKey = typeof(ControllerTipItem);
    }

    /// <summary>
    /// Gets or sets the controller tip key action.
    /// </summary>
    /// <remarks>
    /// Must be a gamepad key.
    /// </remarks>
    public VirtualKey Key
    {
        get => (VirtualKey)GetValue(KeyProperty);
        set
        {
            // All game pad key's values are between GamepadA and GamepadRightThumbstickLeft.
            // Any other virtual keys are invalid here. 
            if (value is < VirtualKey.GamepadA or > VirtualKey.GamepadRightThumbstickLeft)
            {
                throw new InvalidEnumArgumentException("Key must be a gamepad virtual key.");
            }

            SetValue(KeyProperty, value);
        }
    }

    /// <summary>
    /// Gets the sets the command.
    /// </summary>
    public ICommand? Command 
    {
        get => (ICommand?)GetValue(CommandProperty);
        set => SetValue(CommandProperty, value);
    }

    /// <summary>
    /// Gets the sets the command parameter.
    /// </summary>
    public object? CommandParameter
    {
        get => (object?)GetValue(CommandParameterProperty);
        set => SetValue(CommandParameterProperty, value);
    }

    protected override void OnApplyTemplate()
    {
        base.OnApplyTemplate();

        // Bind to events
        KeyDown += this.ControllerTipItem_KeyDown;
    }

    private void ControllerTipItem_KeyDown(object sender, KeyRoutedEventArgs e)
    {
        if (e.OriginalKey != Key)
        {
            return;
        }
        
        Command?.Execute(CommandParameter);
    }
}
