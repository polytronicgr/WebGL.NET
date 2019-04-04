﻿using System;
using System.Diagnostics;
using System.Threading.Tasks;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.PixelFormats;
using WebGLDotNET;

namespace Samples
{
    public class TexturedCubeFromAssets : BaseTexturedCube
    {
        protected override async void LoadImage()
        {
            var file = "Assets/spongebob.bmp";
            var img = await GetImageFromAssets(file);
            var colors = GetRGBAColors(img);

            var imageData = new ImageData(colors, img.Width, img.Height);

            gl.BindTexture(WebGLRenderingContextBase.TEXTURE_2D, texture);

            gl.TexParameteri(
                WebGLRenderingContextBase.TEXTURE_2D,
                WebGLRenderingContextBase.TEXTURE_WRAP_S,
                (int)WebGLRenderingContextBase.CLAMP_TO_EDGE);
            gl.TexParameteri(
                WebGLRenderingContextBase.TEXTURE_2D,
                WebGLRenderingContextBase.TEXTURE_WRAP_T,
                (int)WebGLRenderingContextBase.CLAMP_TO_EDGE);
            gl.TexParameteri(
                WebGLRenderingContextBase.TEXTURE_2D,
                WebGLRenderingContextBase.TEXTURE_MIN_FILTER,
                (int)WebGLRenderingContextBase.NEAREST);
            gl.TexParameteri(
                WebGLRenderingContextBase.TEXTURE_2D,
                WebGLRenderingContextBase.TEXTURE_MAG_FILTER,
                (int)WebGLRenderingContextBase.NEAREST);

            gl.TexImage2D(
                WebGLRenderingContextBase.TEXTURE_2D,
                0,
                WebGLRenderingContextBase.RGB,
                WebGLRenderingContextBase.RGB,
                WebGLRenderingContextBase.UNSIGNED_BYTE,
                imageData);
        }

        private async Task<Image<Rgba32>> GetImageFromAssets(string filename)
        {
            var content = await WasmResourceLoader.LoadResource(
                filename,
                WasmResourceLoader.GetBaseAddress());

            var stopwatch = Stopwatch.StartNew();
            var img = SixLabors.ImageSharp.Image.Load(content);
            stopwatch.Stop();

#if DEBUG
            Console.WriteLine($"Image.Load elapsed: {stopwatch.Elapsed}");
#endif

            return img;
        }

        private byte[] GetRGBAColors(Image<Rgba32> img)
        {
            var numBytes = img.Width * img.Height * 4;
            var colors = new byte[numBytes];
            var colorIndex = 0;

            for (int i = 0; i < img.Width; i++)
            {
                for (int j = 0; j < img.Height; j++)
                {
                    var pixel = img[i, j];

                    colors[colorIndex++] = pixel.R;
                    colors[colorIndex++] = pixel.G;
                    colors[colorIndex++] = pixel.B;
                    colors[colorIndex++] = pixel.A;
                }
            }

            return colors;
        }
    }
}
