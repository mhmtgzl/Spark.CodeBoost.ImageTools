using SixLabors.ImageSharp;
using SixLabors.ImageSharp.Formats.Webp;
using SixLabors.ImageSharp.Processing;
using Microsoft.AspNetCore.Http;
using System.IO;

namespace Spark.CodeBoost.ImageTools;

/// <summary>
/// Provides functionality for resizing images from various sources and converting them to base64 WebP format.
/// </summary>
public class ImageResizer
{
    /// <summary>
    /// Resizes a base64-encoded image while preserving its aspect ratio,
    /// and returns the result as a base64-encoded WebP string.
    /// </summary>
    /// <param name="base64Image">The base64 string representation of the image.</param>
    /// <param name="maxWidth">Maximum width for the resized image.</param>
    /// <param name="maxHeight">Maximum height for the resized image.</param>
    /// <returns>Base64-encoded resized image in WebP format.</returns>
    public static async Task<string> ResizeImageAsync(string base64Image, int maxWidth, int maxHeight)
    {
        var image = await Base64ToImageAsync(base64Image);
        return await ResizeAndConvertToBase64Async(image, maxWidth, maxHeight);
    }

    /// <summary>
    /// Resizes an uploaded image file while preserving its aspect ratio,
    /// and returns the result as a base64-encoded WebP string.
    /// </summary>
    /// <param name="file">The image file to resize.</param>
    /// <param name="maxWidth">Maximum width for the resized image.</param>
    /// <param name="maxHeight">Maximum height for the resized image.</param>
    /// <returns>Base64-encoded resized image in WebP format.</returns>
    public static async Task<string> ResizeImageAsync(IFormFile file, int maxWidth, int maxHeight)
    {
        using var stream = new MemoryStream();
        await file.CopyToAsync(stream);
        stream.Position = 0;
        var image = await Image.LoadAsync(stream);
        return await ResizeAndConvertToBase64Async(image, maxWidth, maxHeight);
    }

    /// <summary>
    /// Resizes the image and converts it to base64-encoded WebP format.
    /// </summary>
    /// <param name="image">Image to resize.</param>
    /// <param name="maxWidth">Maximum width for resizing.</param>
    /// <param name="maxHeight">Maximum height for resizing.</param>
    /// <returns>Base64-encoded WebP string.</returns>
    private static async Task<string> ResizeAndConvertToBase64Async(Image image, int maxWidth, int maxHeight)
    {
        var resizedImage = ResizeImage(image, maxWidth, maxHeight);
        return await ImageToBase64Async(resizedImage);
    }

    /// <summary>
    /// Converts a base64 string into an ImageSharp <see cref="Image"/> object.
    /// </summary>
    /// <param name="base64String">The base64-encoded image string.</param>
    /// <returns>An Image object loaded from the base64 string.</returns>
    private static async Task<Image> Base64ToImageAsync(string base64String)
    {
        var imageBytes = Convert.FromBase64String(base64String);
        using var ms = new MemoryStream(imageBytes);
        return await Image.LoadAsync(ms);
    }

    /// <summary>
    /// Converts an <see cref="Image"/> to a base64-encoded WebP string.
    /// </summary>
    /// <param name="image">The image to convert.</param>
    /// <returns>Base64 string of the image in WebP format.</returns>
    private static async Task<string> ImageToBase64Async(Image image)
    {
        using var ms = new MemoryStream();
        await image.SaveAsync(ms, new WebpEncoder());
        return Convert.ToBase64String(ms.ToArray());
    }

    /// <summary>
    /// Resizes the given image to fit within the specified dimensions while preserving its aspect ratio.
    /// </summary>
    /// <param name="image">The image to resize.</param>
    /// <param name="maxWidth">Maximum width constraint.</param>
    /// <param name="maxHeight">Maximum height constraint.</param>
    /// <returns>The resized image.</returns>
    private static Image ResizeImage(Image image, int maxWidth, int maxHeight)
    {
        var options = new ResizeOptions
        {
            Size = new Size(maxWidth, maxHeight),
            Mode = ResizeMode.Max
        };

        image.Mutate(x => x.Resize(options));
        return image;
    }
}
