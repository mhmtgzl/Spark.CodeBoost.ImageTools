# ImageResizer for .NET

A lightweight image resizing utility for .NET using [ImageSharp](https://github.com/SixLabors/ImageSharp).  
Supports resizing images from base64 strings or `IFormFile` input, and returns optimized WebP images as base64 strings.

---

## Features

- ✅ Resize images while preserving aspect ratio  
- ✅ Accepts both base64 string and `IFormFile` inputs  
- ✅ Outputs base64-encoded WebP images  
- ✅ Uses high-performance ImageSharp library  
- ✅ Ideal for web and API scenarios  

---

## Installation

### 1. Install required NuGet packages:

```bash
dotnet add package SixLabors.ImageSharp
dotnet add package SixLabors.ImageSharp.Web

```
### 2. Add the necessary using statements:
```bash
using Spark.CodeBoost.ImageTools;

```
## Basic Usage
### Resize from Base64 String
```csharp
string resizedBase64 = await ImageResizer.ResizeImageAsync(
    base64Image: originalBase64,
    maxWidth: 800,
    maxHeight: 600
);

```
### Resize from IFormFile
string resizedBase64 = await ImageResizer.ResizeImageAsync(
    file: formFile,
    maxWidth: 1024,
    maxHeight: 768
);

### Output
```bash
Both methods return a base64-encoded WebP image
This can be safely embedded into HTML, stored in databases, or used in APIs.

```
## Internals
```bash
🔄 Automatically calculates dimensions to preserve original aspect ratio
🧠 Uses ResizeMode.Max to ensure the image fits within the given bounds
📦 Converts all output to WebP format using WebpEncoder
🚫 Input validation is the developer’s responsibility (e.g. null or corrupt files)

```
### Example: Base64 to Resized WebP
```bash
// Convert base64 PNG or JPEG to base64 WebP
string inputBase64 = await File.ReadAllTextAsync("original.txt");
string resizedWebPBase64 = await ImageResizer.ResizeImageAsync(inputBase64, 400, 400);
```
## License
```bash
MIT — Free for personal and commercial use.
