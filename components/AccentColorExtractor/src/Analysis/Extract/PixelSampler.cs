// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using Windows.Graphics.Imaging;
using Windows.Storage.Streams;
using Windows.UI;

namespace CommunityToolkit.WinUI.Controls.AccentColorExtractorRns.Analysis.Extract;

internal class PixelSampler
{
    private readonly BitmapDecoder _decoder;

    private PixelSampler(BitmapDecoder decoder)
    {
        _decoder = decoder;
    }

    public static async Task<PixelSampler> CreateAsync(IRandomAccessStream stream)
    {
        var decoder = await BitmapDecoder.CreateAsync(stream);
        return new PixelSampler(decoder);
    }

    public async Task<Color[]> SamplePixelsAsync(int count)
    {
        // Get pixel data
        var pixelData = await _decoder.GetPixelDataAsync();
        var pixels = pixelData.DetachPixelData();

        // Sample pixels
        var samples = new Color[count];
        var rand = new Random(0);
        for (int i = 0; i < count; i++)
        {
            // Select a random pixel and add it to the samples
            var bi = rand.Next(pixels.Length / 4) * 4; // Only sample on 4 byte boundaries
            var pixel = Color.FromArgb(pixels[bi+3], pixels[bi+2], pixels[bi+1], pixels[bi]);
            samples[i] = pixel;
        }

        return samples;
    }
}
