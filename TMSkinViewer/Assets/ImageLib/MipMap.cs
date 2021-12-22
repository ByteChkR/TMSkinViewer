﻿using UnityEngine;

namespace CSharpImageLibrary
{

    /// <summary>
    ///     Represents a mipmap of an image.
    /// </summary>
    public class MipMap
    {

        /// <summary>
        ///     Pixels in bitmap image.
        /// </summary>
        public byte[] Pixels { get; set; }

        /// <summary>
        ///     Size of mipmap in memory.
        /// </summary>
        public int UncompressedSize { get; private set; }

        /// <summary>
        ///     Details of the format that this mipmap was created from.
        /// </summary>
        public ImageFormats.ImageEngineFormatDetails LoadedFormatDetails { get; private set; }

        /// <summary>
        ///     Mipmap width.
        /// </summary>
        public int Width { get; set; }

        /// <summary>
        ///     Mipmap height.
        /// </summary>
        public int Height { get; set; }

        /// <summary>
        ///     Indicates if there is any alpha in image.
        /// </summary>
        public bool IsAlphaPresent
        {
            get
            {
                if ( Pixels?.Length != 0 )
                {
                    for ( int i = 3; i < Pixels.Length; i += 4 ) // TODO: ComponentSize
                    {
                        if ( Pixels[i] != 0 )
                        {
                            return true;
                        }
                    }
                }

                return false;
            }
        }

        #region Public

        /// <summary>
        ///     Creates a Mipmap object from a WPF image.
        /// </summary>
        public MipMap( byte[] pixels, int width, int height, ImageFormats.ImageEngineFormatDetails details )
        {
            Pixels = pixels;
            Width = width;
            Height = height;
            LoadedFormatDetails = details;

            UncompressedSize = ImageFormats.GetUncompressedSize( width, height, details.MaxNumberOfChannels, false );
        }

        /// <summary>
        ///     Creates a WPF image from this mipmap.
        /// </summary>
        /// <returns>WriteableBitmap of image.</returns>
        public Texture2D ToImage()
        {
            byte[] tempPixels = ImageEngine.GetPixelsAsBGRA32( Width, Height, Pixels, LoadedFormatDetails );
            Texture2D image = new Texture2D( Width, Height );
            image.SetPixelData( tempPixels, 0, 0 );

            image.Apply();

            return image;
        }

        #endregion

    }

}
