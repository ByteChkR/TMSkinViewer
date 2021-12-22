using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

using CSharpImageLibrary.DDS;
using CSharpImageLibrary.Headers;

using UsefulThings;

namespace CSharpImageLibrary
{

    /// <summary>
    ///     Determines how alpha is handled.
    /// </summary>
    public enum AlphaSettings
    {

        /// <summary>
        ///     Keeps any existing alpha.
        /// </summary>
        KeepAlpha,

        /// <summary>
        ///     Premultiplies RBG and Alpha channels. Alpha remains.
        /// </summary>
        Premultiply,

        /// <summary>
        ///     Removes alpha channel.
        /// </summary>
        RemoveAlphaChannel,

    }

    /// <summary>
    ///     Determines how Mipmaps are handled.
    /// </summary>
    public enum MipHandling
    {

        /// <summary>
        ///     If mips are present, they are used, otherwise regenerated.
        /// </summary>
        [Description( "If mips are present, they are used, otherwise regenerated." )]
        Default,

        /// <summary>
        ///     Keeps existing mips if existing. Doesn't generate new ones either way.
        /// </summary>
        [Description( "Keeps existing mips if existing. Doesn't generate new ones either way." )]
        KeepExisting,

        /// <summary>
        ///     Removes old mips and generates new ones.
        /// </summary>
        [Description( "Removes old mips and generates new ones." )]
        GenerateNew,

        /// <summary>
        ///     Removes all but the top mip. Used for single mip formats.
        /// </summary>
        [Description( "Removes all but the top mip. Used for single mip formats." )]
        KeepTopOnly

    }

    /// <summary>
    ///     Provides main image functions
    /// </summary>
    public static class ImageEngine
    {

        /// <summary>
        ///     Indicates whether cancellation has been requested for
        /// </summary>
        public static bool IsCancellationRequested => cts.IsCancellationRequested;

        /// <summary>
        ///     True = Windows WIC Codecs are present (8+)
        /// </summary>
        public static bool WindowsWICCodecsAvailable { get; set; }

        /// <summary>
        ///     Enables threading of Loading and Saving operations to improve performance.
        /// </summary>
        public static bool EnableThreading { get; set; } = true;

        /// <summary>
        ///     CURRENTLY DISABLED. Didn't work :(
        ///     Enables GPU Accelerated encoding and decoding of all formats.
        ///     NOTE: WIC formats (jpg, bmp, png etc) probably already use GPU, but are not covered by this flag.
        /// </summary>
        public static bool EnableGPUAcceleration { get; set; } = false;

        /// <summary>
        ///     Determines how many threads to use. -1 is infinite.
        /// </summary>
        public static int NumThreads { get; set; } = -1;

        internal static CancellationTokenSource cts { get; set; } = new CancellationTokenSource();

        #region Public

        /// <summary>
        ///     Initiates a cancellation of currently running tasks.
        ///     Not guaranteed to cancel immediately.
        /// </summary>
        public static void Cancel()
        {
            if ( !cts.IsCancellationRequested )
            {
                cts.Cancel();
            }
        }

        /// <summary>
        ///     Gets pixels as a BGRA32 array regardless of their original format (float, short)
        /// </summary>
        /// <param name="width">Width of image.</param>
        /// <param name="height">Height of image.</param>
        /// <param name="pixels">Original pixels.</param>
        /// <param name="formatDetails">Details about format pixels array is currently in.</param>
        /// <returns>BGRA32 pixel array.</returns>
        public static byte[] GetPixelsAsBGRA32(
            int width,
            int height,
            byte[] pixels,
            ImageFormats.ImageEngineFormatDetails formatDetails )
        {
            if ( formatDetails.ComponentSize == 1 )
            {
                return pixels;
            }

            byte[] tempPixels = new byte[width * height * 4];

            Action < int > action = new Action < int >(
                                                       ind => tempPixels[ind] =
                                                                  formatDetails.ReadByte(
                                                                       pixels,
                                                                       ind * formatDetails.ComponentSize
                                                                      )
                                                      );

            if ( EnableThreading )
            {
                Parallel.For(
                             0,
                             tempPixels.Length,
                             new ParallelOptions { MaxDegreeOfParallelism = NumThreads },
                             ind => action( ind )
                            );
            }
            else
            {
                for ( int i = 0; i < tempPixels.Length; i++ )
                {
                    action( i );
                }
            }

            return tempPixels;
        }

        /// <summary>
        ///     Resets cancellation token source given an external source.
        /// </summary>
        /// <param name="yourCTS">External CTS to use.</param>
        public static void ResetCancellation( CancellationTokenSource yourCTS )
        {
            cts = yourCTS;
        }

        /// <summary>
        ///     Resets cancellation token source.
        /// </summary>
        public static void ResetCancellation()
        {
            cts = new CancellationTokenSource();
        }

        /// <summary>
        ///     Destroys mipmaps. Expects at least one mipmap in given list.
        /// </summary>
        /// <param name="MipMaps">List of Mipmaps.</param>
        /// <param name="mipToSave">Index of mipmap to save.</param>
        /// <returns>Number of mips present.</returns>
        internal static int DestroyMipMaps( List < MipMap > MipMaps, int mipToSave = 0 )
        {
            if ( MipMaps.Count != 1 )
            {
                MipMaps.RemoveRange(
                                    mipToSave + 1,
                                    MipMaps.Count - 1
                                   ); // +1 because mipToSave is 0 based and we want to keep it
            }

            return 1;
        }

        internal static AbstractHeader LoadHeader( Stream stream )
        {
            stream.Seek( 0, SeekOrigin.Begin );

            // Determine type of image
            ImageFormats.SupportedExtensions ext = ImageFormats.DetermineImageType( stream );

            // Parse header
            AbstractHeader header = null;

            switch ( ext )
            {
                case ImageFormats.SupportedExtensions.BMP:
                    header = new BMP_Header( stream );

                    break;

                case ImageFormats.SupportedExtensions.DDS:
                    header = new DDS_Header( stream );

                    break;

                case ImageFormats.SupportedExtensions.JPG:
                    header = new JPG_Header( stream );

                    break;

                case ImageFormats.SupportedExtensions.PNG:
                    header = new PNG_Header( stream );

                    break;

                case ImageFormats.SupportedExtensions.TGA:
                    header = new TGA_Header( stream );

                    break;

                case ImageFormats.SupportedExtensions.GIF:
                    header = new GIF_Header( stream );

                    break;

                case ImageFormats.SupportedExtensions.TIF:
                    header = new TIFF_Header( stream );

                    break;

                default:
                    throw new NotSupportedException( "Image type unknown." );
            }

            return header;
        }

        internal static List < MipMap > LoadImage(
            Stream imageStream,
            AbstractHeader header,
            int maxDimension,
            double scale,
            ImageFormats.ImageEngineFormatDetails formatDetails )
        {
            imageStream.Seek( 0, SeekOrigin.Begin );
            List < MipMap > MipMaps = null;

            int decodeWidth = header.Width > header.Height ? maxDimension : 0;
            int decodeHeight = header.Width < header.Height ? maxDimension : 0;

            switch ( header.Format )
            {
                case ImageEngineFormat.DDS_DXT1:
                case ImageEngineFormat.DDS_DXT2:
                case ImageEngineFormat.DDS_DXT3:
                case ImageEngineFormat.DDS_DXT4:
                case ImageEngineFormat.DDS_DXT5:
                    MipMaps = DDSGeneral.LoadDDS(
                                                 ( MemoryStream )imageStream,
                                                 ( DDS_Header )header,
                                                 maxDimension,
                                                 formatDetails
                                                );

                    break;

                case ImageEngineFormat.DDS_G8_L8:
                case ImageEngineFormat.DDS_ARGB_4:
                case ImageEngineFormat.DDS_RGB_8:
                case ImageEngineFormat.DDS_V8U8:
                case ImageEngineFormat.DDS_A8L8:
                case ImageEngineFormat.DDS_ARGB_8:
                case ImageEngineFormat.DDS_ARGB_32F:
                case ImageEngineFormat.DDS_ABGR_8:
                case ImageEngineFormat.DDS_G16_R16:
                case ImageEngineFormat.DDS_R5G6B5:
                case ImageEngineFormat.DDS_ATI1:
                case ImageEngineFormat.DDS_ATI2_3Dc:
                case ImageEngineFormat.DDS_CUSTOM:
                case ImageEngineFormat.DDS_DX10:
                    MipMaps = DDSGeneral.LoadDDS(
                                                 ( MemoryStream )imageStream,
                                                 ( DDS_Header )header,
                                                 maxDimension,
                                                 formatDetails
                                                );

                    break;

                case ImageEngineFormat.GIF:
                case ImageEngineFormat.JPG:
                case ImageEngineFormat.PNG:
                case ImageEngineFormat.BMP:
                case ImageEngineFormat.TIF:
                    throw new NotSupportedException();

                    // MipMaps = WIC_Codecs.LoadWithCodecs(
                    //                                     imageStream,
                    //                                     decodeWidth,
                    //                                     decodeHeight,
                    //                                     scale,
                    //                                     false,
                    //                                     formatDetails
                    //                                    );

                    break;

                case ImageEngineFormat.TGA:
                    using ( TargaImage tga = new TargaImage( imageStream, ( ( TGA_Header )header ).header ) )
                    {
                        MipMaps = new List < MipMap >()
                                  {
                                      new MipMap( tga.ImageData, tga.Header.Width, tga.Header.Height, formatDetails )
                                  };
                    }

                    break;

                default:
                    throw new FormatException( $"Format unknown: {header.Format}." );
            }

            return MipMaps;
        }

        internal static MipMap Resize( MipMap mipMap, double scale )
        {
            return Resize( mipMap, scale, scale ); // Could be either scale dimension, doesn't matter.
        }

        internal static MipMap Resize( MipMap mipMap, double xScale, double yScale )
        {
            return mipMap; //Disable Resizing!??!?
            // WriteableBitmap baseBMP = Images.CreateWriteableBitmap( mipMap.Pixels, mipMap.Width, mipMap.Height );
            // baseBMP.Freeze();
            //
            // //return Resize(baseBMP, xScale, yScale, mipMap.Width, mipMap.Height, mipMap.LoadedFormatDetails);
            //
            // #region Old code, but want to keep not only for posterity, but I'm not certain the above works in the context below.
            //
            // // KFreon: Only do the alpha bit if there is any alpha. Git #444 (https://github.com/ME3Explorer/ME3Explorer/issues/444) exposed the issue where if there isn't alpha, it overruns the buffer.
            // bool alphaPresent = mipMap.IsAlphaPresent;
            //
            // WriteableBitmap alpha = new WriteableBitmap(
            //                                             mipMap.Width,
            //                                             mipMap.Height,
            //                                             96,
            //                                             96,
            //                                             PixelFormats.Bgr32,
            //                                             null
            //                                            );
            //
            // if ( alphaPresent ) // && !mergeAlpha)
            // {
            //     // Pull out alpha since scaling with alpha doesn't work properly for some reason
            //     try
            //     {
            //         unsafe
            //         {
            //             alpha.Lock();
            //             int index = 3;
            //             byte* alphaPtr = ( byte* )alpha.BackBuffer.ToPointer();
            //
            //             for ( int i = 0; i < mipMap.Width * mipMap.Height * 4; i += 4 )
            //             {
            //                 // Set all pixels in alpha to value of alpha from original image - otherwise scaling will interpolate colours
            //                 alphaPtr[i] = mipMap.Pixels[index];
            //                 alphaPtr[i + 1] = mipMap.Pixels[index];
            //                 alphaPtr[i + 2] = mipMap.Pixels[index];
            //                 alphaPtr[i + 3] = mipMap.Pixels[index];
            //                 index += 4;
            //             }
            //
            //             alpha.Unlock();
            //         }
            //     }
            //     catch ( Exception e )
            //     {
            //         Debug.WriteLine( e.ToString() );
            //
            //         throw;
            //     }
            // }
            //
            // WriteableBitmap bmp = Images.CreateWriteableBitmap( mipMap.Pixels, mipMap.Width, mipMap.Height );
            // FormatConvertedBitmap main = new FormatConvertedBitmap( bmp, PixelFormats.Bgr32, null, 0 );
            //
            // // Scale RGB
            // ScaleTransform scaletransform = new ScaleTransform( xScale, yScale );
            // TransformedBitmap scaledMain = new TransformedBitmap( main, scaletransform );
            //
            // int newWidth = ( int )( mipMap.Width * xScale );
            // int newHeight = ( int )( mipMap.Height * yScale );
            // int newStride = ( int )( newWidth * 4 );
            //
            // // Put alpha back in
            // FormatConvertedBitmap newConv = new FormatConvertedBitmap( scaledMain, PixelFormats.Bgra32, null, 0 );
            // WriteableBitmap resized = new WriteableBitmap( newConv );
            //
            // if ( alphaPresent ) // && !mergeAlpha)
            // {
            //     TransformedBitmap scaledAlpha = new TransformedBitmap( alpha, scaletransform );
            //     WriteableBitmap newAlpha = new WriteableBitmap( scaledAlpha );
            //
            //     try
            //     {
            //         unsafe
            //         {
            //             resized.Lock();
            //             newAlpha.Lock();
            //             byte* resizedPtr = ( byte* )resized.BackBuffer.ToPointer();
            //             byte* alphaPtr = ( byte* )newAlpha.BackBuffer.ToPointer();
            //
            //             for ( int i = 3; i < newStride * newHeight; i += 4 )
            //             {
            //                 resizedPtr[i] = alphaPtr[i];
            //             }
            //
            //             resized.Unlock();
            //             newAlpha.Unlock();
            //         }
            //     }
            //     catch ( Exception e )
            //     {
            //         Debug.WriteLine( e.ToString() );
            //
            //         throw;
            //     }
            // }
            //
            // return new MipMap( resized.GetPixelsAsBGRA32(), newWidth, newHeight, mipMap.LoadedFormatDetails );
            //
            // #endregion Old code
        }

        /// <summary>
        ///     Save mipmaps as given format to stream.
        /// </summary>
        /// <param name="MipMaps">List of Mips to save.</param>
        /// <param name="mipChoice">Determines how to handle mipmaps.</param>
        /// <param name="maxDimension">Maximum value for either image dimension.</param>
        /// <param name="alphaSetting">Determines how to handle alpha.</param>
        /// <param name="mipToSave">0 based index on which mipmap to make top of saved image.</param>
        /// <param name="destFormatDetails">Details about the destination format.</param>
        /// <returns>True on success.</returns>
        internal static byte[] Save(
            List < MipMap > MipMaps,
            ImageFormats.ImageEngineFormatDetails destFormatDetails,
            MipHandling mipChoice,
            AlphaSettings alphaSetting,
            int maxDimension = 0,
            int mipToSave = 0 )
        {
            List < MipMap > newMips = new List < MipMap >( MipMaps );

            int width = newMips[0].Width;
            int height = newMips[0].Height;

            if ( destFormatDetails.IsMippable && mipChoice == MipHandling.GenerateNew ||
                 destFormatDetails.IsMippable && newMips.Count == 1 && mipChoice == MipHandling.Default )
            {
                DDSGeneral.BuildMipMaps( newMips );
            }

            // KFreon: Resize if asked
            if ( maxDimension != 0 && maxDimension < width && maxDimension < height )
            {
                if ( !General.IsPowerOfTwo( maxDimension ) )
                {
                    throw new ArgumentException(
                                                $"{nameof( maxDimension )} must be a power of 2. Got {nameof( maxDimension )} = {maxDimension}"
                                               );
                }

                // KFreon: Check if there's a mipmap suitable, removes all larger mipmaps
                IEnumerable < MipMap > validMipmap = newMips.Where(
                                                                   img => img.Width == maxDimension &&
                                                                          img.Height <= maxDimension ||
                                                                          img.Height == maxDimension &&
                                                                          img.Width <= maxDimension
                                                                  ); // Check if a mip dimension is maxDimension and that the other dimension is equal or smaller

                if ( validMipmap?.Count() != 0 )
                {
                    int index = newMips.IndexOf( validMipmap.First() );
                    newMips.RemoveRange( 0, index );
                }
                else
                {
                    // KFreon: Get the amount the image needs to be scaled. Find largest dimension and get it's scale.
                    double scale = maxDimension * 1d / ( width > height ? width : height );

                    // KFreon: No mip. Resize.
                    newMips[0] = Resize( newMips[0], scale );
                }
            }

            // KFreon: Ensure we have a power of two for dimensions FOR DDS ONLY
            TestDDSMipSize(
                           newMips,
                           destFormatDetails,
                           width,
                           height,
                           out double fixXScale,
                           out double fixYScale,
                           mipChoice
                          );

            if ( fixXScale != 0 || fixYScale != 0 || mipChoice == MipHandling.KeepTopOnly )
            {
                DestroyMipMaps( newMips, mipToSave );
            }

            if ( ( fixXScale != 0 || fixXScale != 0 ) &&
                 destFormatDetails.IsMippable &&
                 mipChoice != MipHandling.KeepTopOnly )
            {
                DDSGeneral.BuildMipMaps( newMips );
            }

            byte[] destination = null;

            if ( destFormatDetails.IsDDS )
            {
                destination = DDSGeneral.Save( newMips, destFormatDetails, alphaSetting );
            }
            else
            {
                throw new NotSupportedException();

                // KFreon: Try saving with built in codecs
                MipMap mip = newMips[0];

                // Fix formatting
                byte[] newPixels = new byte[mip.Width * mip.Height * 4];

                for ( int i = 0, j = 0; i < newPixels.Length; i++, j += mip.LoadedFormatDetails.ComponentSize )
                {
                    newPixels[i] = mip.LoadedFormatDetails.ReadByte( mip.Pixels, j );
                }

                // destination = WIC_Codecs.SaveWithCodecs(
                //                                         newPixels,
                //                                         destFormatDetails.Format,
                //                                         mip.Width,
                //                                         mip.Height,
                //                                         alphaSetting
                //                                        );
            }

            return destination;
        }

        internal static void TestDDSMipSize(
            List < MipMap > newMips,
            ImageFormats.ImageEngineFormatDetails destFormatDetails,
            int width,
            int height,
            out double fixXScale,
            out double fixYScale,
            MipHandling mipChoice )
        {
            fixXScale = 0;
            fixYScale = 0;

            if ( destFormatDetails.IsBlockCompressed &&
                 ( !General.IsPowerOfTwo( width ) || !General.IsPowerOfTwo( height ) ) )
            {
                // If only keeping top mip, and that mip is divisible by 4, it's ok.
                if ( ( mipChoice == MipHandling.KeepTopOnly || mipChoice == MipHandling.KeepExisting ) &&
                     DDSGeneral.CheckSize_DXT( width, height ) )
                {
                    return;
                }

                double newWidth = 0;
                double newHeight = 0;

                // Takes into account aspect ratio (a little bit)
                double aspect = width / height;

                if ( aspect > 1 )
                {
                    newWidth = General.RoundToNearestPowerOfTwo( width );
                    double tempScale = newWidth / width;
                    newHeight = General.RoundToNearestPowerOfTwo( ( int )( height * tempScale ) );
                }
                else
                {
                    newHeight = General.RoundToNearestPowerOfTwo( height );
                    double tempScale = newHeight / height;
                    newWidth = General.RoundToNearestPowerOfTwo( ( int )( width * tempScale ) );
                }

                // Little extra bit to allow integer cast from Double with the correct answer. Occasionally dimensions * scale would be 511.99999999998 instead of 512, so adding a little allows the integer cast to return correct value.
                fixXScale = 1d * newWidth / width + 0.001;
                fixYScale = 1d * newHeight / height + 0.001;
                newMips[0] = Resize( newMips[0], fixXScale, fixYScale );
            }
        }

        #endregion

        #region Private

        /// <summary>
        ///     Constructor. Checks WIC status before any other operation.
        /// </summary>
        static ImageEngine()
        {
            WindowsWICCodecsAvailable = false;

            // Set NumThreads to be more sensible
            NumThreads = Environment.ProcessorCount - 1;

            if ( NumThreads == 0 ) // Single core...
            {
                NumThreads = 1;
            }

            // Enable GPU Acceleration by default
            /*if (GPU.IsGPUAvailable)
                EnableGPUAcceleration = false;*/
        }

        private static byte[] BuildGrayscaleFromChannel( byte[] pixels, int channel )
        {
            byte[] destination = new byte[pixels.Length / 4];

            for ( int i = channel, count = 0; i < pixels.Length; i += 4, count++ )
            {
                destination[count] = pixels[i];
            }

            return destination;
        }

        #endregion

        // private static async Task < ConcurrentBag < string > > DoBulkParallel(
        //     IEnumerable < string > files,
        //     ImageFormats.ImageEngineFormatDetails destFormatDetails,
        //     string saveFolder,
        //     MipHandling saveMipType = MipHandling.Default,
        //     bool useSourceAsDestination = false,
        //     bool removeAlpha = false,
        //     IProgress < int > progressReporter = null )
        // {
        //     ConcurrentBag < string > failures = new ConcurrentBag < string >();
        //
        //     BufferBlock < string > fileNameStore = new BufferBlock < string >();
        //
        //     int maxParallelism = NumThreads == 1 ? 1 :
        //                          NumThreads == -1 ? Environment.ProcessorCount : NumThreads;
        //
        //     // Define block to perform each conversion
        //     TransformBlock < string, Tuple < byte[], string > > encoder =
        //         new TransformBlock < string, Tuple < byte[], string > >(
        //                                                                 file =>
        //                                                                 {
        //                                                                     byte[] data = null;
        //
        //                                                                     string filename =
        //                                                                         Path.GetFileNameWithoutExtension(
        //                                                                              file
        //                                                                             ) +
        //                                                                         "." +
        //                                                                         destFormatDetails.Extension;
        //
        //                                                                     string path =
        //                                                                         Path.Combine(
        //                                                                              useSourceAsDestination
        //                                                                                  ? Path.GetDirectoryName( file )
        //                                                                                  : saveFolder,
        //                                                                              filename
        //                                                                             );
        //
        //                                                                     path = General.FindValidNewFileName( path );
        //
        //                                                                     using ( ImageEngineImage img =
        //                                                                         new ImageEngineImage( file ) )
        //                                                                     {
        //                                                                         try
        //                                                                         {
        //                                                                             data = img.Save(
        //                                                                                  destFormatDetails,
        //                                                                                  saveMipType,
        //                                                                                  removeAlpha: removeAlpha
        //                                                                                 );
        //                                                                         }
        //                                                                         catch ( Exception e )
        //                                                                         {
        //                                                                             failures.Add(
        //                                                                                  path +
        //                                                                                  "  Reason: " +
        //                                                                                  e.ToString()
        //                                                                                 );
        //                                                                         }
        //                                                                     }
        //
        //                                                                     progressReporter.
        //                                                                         Report( 1 ); // Value not relevent.
        //
        //                                                                     return new Tuple < byte[], string >(
        //                                                                          data,
        //                                                                          path
        //                                                                         );
        //                                                                 },
        //                                                                 new ExecutionDataflowBlockOptions
        //                                                                 {
        //                                                                     MaxDegreeOfParallelism = maxParallelism,
        //                                                                     BoundedCapacity = maxParallelism
        //                                                                 }
        //                                                                );
        //
        //     // Define block to write converted data to disk
        //     ActionBlock < Tuple < byte[], string > > diskWriter = new ActionBlock < Tuple < byte[], string > >(
        //          tuple =>
        //          {
        //              string path = General.FindValidNewFileName( tuple.Item2 );
        //
        //              try
        //              {
        //                  File.WriteAllBytes( path, tuple.Item1 );
        //              }
        //              catch ( Exception e )
        //              {
        //                  failures.Add( path + "  Reason: " + e.ToString() );
        //              }
        //          },
        //          new ExecutionDataflowBlockOptions
        //          {
        //              MaxDegreeOfParallelism = 2,
        //              BoundedCapacity = maxParallelism
        //          }
        //         ); // Limit to 2 disk write operations at a time, but allow many to be stored in it's buffer.
        //
        //     // Link blocks together
        //     fileNameStore.LinkTo( encoder, new DataflowLinkOptions { PropagateCompletion = true } );
        //     encoder.LinkTo( diskWriter, new DataflowLinkOptions { PropagateCompletion = true } );
        //
        //     // Begin production
        //     new Action(
        //                async () =>
        //                {
        //                    foreach ( string file in files )
        //                    {
        //                        await fileNameStore.SendAsync( file );
        //                    }
        //
        //                    fileNameStore.Complete();
        //                }
        //               ).Invoke();
        //
        //     await diskWriter.Completion;
        //
        //     return failures;
        // }

    }

}
