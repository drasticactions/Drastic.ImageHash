// <copyright file="IImageHash.cs" company="Drastic Actions">
// Copyright (c) Drastic Actions. All rights reserved.
// </copyright>

using SkiaSharp;

namespace Drastic.ImageHash;

/// <summary>
/// Interface for perceptual image hashing algorithm.
/// </summary>
public interface IImageHash
{
    /// <summary>Hash the image using the algorithm.</summary>
    /// <param name="image">image to calculate hash from.</param>
    /// <returns>hash value of the image.</returns>
    /// <exception cref="ArgumentNullException">Thrown when <paramref name="image"/> is <c>null</c>.</exception>
    ulong Hash(SKBitmap bitmap);
}