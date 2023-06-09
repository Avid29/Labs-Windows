// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

#if NET6_0_WINDOWS10_0_19041_0
using CommunityToolkit.WinUI.Controls.AccentColorExtractorRns.Analysis.Cluster;
using CommunityToolkit.WinUI.Controls.AccentColorExtractorRns.Analysis.Extract;
using Microsoft.UI.Xaml.Media.Imaging;
using Windows.Storage.Streams;
#else
using Windows.UI.Xaml.Media.Imaging;
#endif

using Windows.UI;

namespace CommunityToolkit.WinUI.Controls.Helpers;

public static class AccentColorExtractor
{
    // Use a different fallback color in release and debug
    #if DEBUG
    // Use magenta as fallback in debug for clear reporting
    private static readonly Color FALLBACK = Color.FromArgb(255,255,0,255);
    #else
    // Use a semi-transparent white fallback in release for minimal contrast error and confusion
    private static readonly Color FALLBACK = Color.FromArgb(200,255,255,255);
    #endif

    public static readonly DependencyProperty CalculateProperty =
        DependencyProperty.RegisterAttached(
            "Calculate",
            typeof(bool),
            typeof(AccentColorExtractor),
            new PropertyMetadata(false, OnCalculateAccentColorPropertyChanged));

    public static readonly DependencyProperty PrimaryAccentColorProperty =
        DependencyProperty.RegisterAttached(
            "PrimaryAccentColor",
            typeof(Brush),
            typeof(AccentColorExtractor),
            new PropertyMetadata(new SolidColorBrush(FALLBACK)));

    public static Brush GetPrimaryAccentColor(DependencyObject dependencyObject)
    {
        return (Brush) dependencyObject.GetValue(PrimaryAccentColorProperty);
    }

    public static void SetPrimaryAccentColor(DependencyObject dependencyObject, Brush value)
    {
        dependencyObject.SetValue(PrimaryAccentColorProperty, value);
    }
    
    public static bool GetCalculate(DependencyObject dependencyObject)
    {
        return (bool) dependencyObject.GetValue(CalculateProperty);
    }

    public static void SetCalculate(DependencyObject dependencyObject, bool value)
    {
        dependencyObject.SetValue(CalculateProperty, value);
    }

    private static async void OnCalculateAccentColorPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
    {
        if (d is Image {Source: BitmapImage imgSrc})
        {
            #if NET6_0_WINDOWS10_0_19041_0
            var stream = await RandomAccessStreamReference.CreateFromUri(imgSrc.UriSource).OpenReadAsync();
            var sampler = await PixelSampler.CreateAsync(stream);
            var sample = await sampler.SamplePixelsAsync(2500);

            var accents = KMeans.Cluster(sample, 3);
            var color = accents[0];
            #else

            // DEBUG: Temporary method of generating a unique color by image
            var hash = imgSrc.UriSource.GetHashCode();
            var bytes = BitConverter.GetBytes(hash);
            var color = Color.FromArgb(255, bytes[0], bytes[1], bytes[2]);

            #endif

            // Convert color to brush
            var brush = new SolidColorBrush(color);

            // Store brush in PrimaryAccentColor property
            SetPrimaryAccentColor(d, brush);
        }
    }
}
