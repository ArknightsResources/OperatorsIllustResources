using SixLabors.ImageSharp;
using SixLabors.ImageSharp.PixelFormats;
using System;
using System.Collections.Generic;
using System.Text;

namespace ArknightsResources.Operators.Resources
{
    internal class ImageHelper
    {
        public static byte[] ProcessImage(Image<Bgra32> rgb, Image<Bgra32> alpha)
        {
            //Image<Rgba32> image = new Image<Rgba32>(rgbImage.Width, rgbImage.Height);

            return new byte[] { 0, 0, 0, 0 };
        }


    }
}
