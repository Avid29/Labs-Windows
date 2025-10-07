// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

namespace ColorAnalyzerExperiment.Samples;

/// <summary>
/// An example sample page of a custom control inheriting from Panel.
/// </summary>
[ToolkitSample(id: nameof(ContrastHelperSample), "ContrastAnalyzer helper", description: $"A sample for showing how the contrast analyzer can be used.")]
public sealed partial class ContrastHelperSample : Page
{
    public ContrastHelperSample()
    {
        this.InitializeComponent();
    }
}
