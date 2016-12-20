using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Web;

namespace ImageCutter
{
    public class ImageHelper
    {
        /// <summary>
        /// 剪裁头像图片
        /// 1、根据客户端图片大小对源图缩放（因为客户端可能已经进行了缩放，所以，裁剪前要以客户端图片尺寸为准）
        /// 2、根据客户端图片裁剪参数，截取缩放后的源图
        /// 3、截图缩放到固定大小
        /// </summary>
        /// <param name="cutParams"></param>
        /// <param name="avatarFixedWidth">缩略图固定宽度</param>
        /// <param name="avatarFixedHeight">缩略图固定高度</param>
        /// <returns></returns>
        public static bool CutAvatar(CutAvatarParams cutParams, int avatarFixedWidth, int avatarFixedHeight)
        {
            bool result = false;
            if (cutParams == null)
            {
                throw new ArgumentNullException("cutParams", "裁剪参数不能为空");
            }
            if (string.IsNullOrEmpty(cutParams.imgSrcRealPath) || !File.Exists(cutParams.imgSrcRealPath))
            {
                throw new ArgumentNullException("cutParams.imgSrcPath", "源图路径为空或指定源图路径不存在");
            }

            if (cutParams.w <= 0 || cutParams.h <= 0)
            {
                throw new ArgumentNullException("cutParams.w,cutParams.h", "裁剪宽度、高度应大于0");
            }

            if (avatarFixedWidth <= 0 || avatarFixedWidth <= 0)
            {
                throw new ArgumentNullException("avatarFixedWidth,avatarFixedWidth", "缩略图固定宽度、高度应大于0");
            }

            Image imgSrc = null, imgSrcThumbNail = null, imgAvatar = null, imgFinal = null;
            Graphics g = null;
            try
            {
                //1、缩放源图
                imgSrc = Image.FromFile(cutParams.imgSrcRealPath);
                imgSrcThumbNail = imgSrc;
                if (cutParams.srcClientHeight > 0 && cutParams.srcClientWidth > 0)
                {
                    imgSrcThumbNail = GetZoomedImage(imgSrc, cutParams.srcClientWidth, cutParams.srcClientHeight);
                }
                //2、裁剪
                imgAvatar = new Bitmap(cutParams.w, cutParams.h);
                g = Graphics.FromImage(imgAvatar);
                g.DrawImage(imgSrcThumbNail, new Rectangle(0, 0, cutParams.w, cutParams.h), new Rectangle(cutParams.x, cutParams.y, cutParams.w, cutParams.h), GraphicsUnit.Pixel);

                //3、缩略图缩放
                imgFinal = GetThumbNailImage(imgAvatar, avatarFixedWidth, avatarFixedHeight);

                //以下代码为保存图片时，设置压缩质量  
                EncoderParameters ep = new EncoderParameters();
                long[] qy = new long[1];
                qy[0] = 80;//设置压缩的比例1-100  
                EncoderParameter eParam = new EncoderParameter(System.Drawing.Imaging.Encoder.Quality, qy);
                ep.Param[0] = eParam;

                ImageCodecInfo[] arrayICI = ImageCodecInfo.GetImageEncoders();
                ImageCodecInfo jpegICIinfo = null;
                for (int x = 0; x < arrayICI.Length; x++)
                {
                    if (arrayICI[x].FormatDescription.Equals("JPEG"))
                    {
                        jpegICIinfo = arrayICI[x];
                        break;
                    }
                }

                string avatarPath = cutParams.imgSrcRealPath.Replace("src", "avatar");
                string avatarPathDir = avatarPath.Substring(0, avatarPath.LastIndexOf("\\"));

                if (!Directory.Exists(avatarPathDir))
                {
                    Directory.CreateDirectory(avatarPathDir);
                }

                if (jpegICIinfo != null)
                {
                    imgAvatar.Save(avatarPath, jpegICIinfo, ep);
                }
                else
                {
                    imgAvatar.Save(avatarPath);
                }
                result = true;
            }
            catch (Exception ex)
            {
                throw new Exception("头像裁剪失败", ex);
            }
            finally
            {
                Dispose(imgSrc, imgSrcThumbNail, imgAvatar, imgFinal, g);
                GC.Collect();
            }
            return result;
        }

        private static void Dispose(params IDisposable[] objs)
        {
            if (objs != null && objs.Length > 0)
            {
                foreach (var o in objs)
                {
                    o.Dispose();
                }
            }
        }

        ///<summary>
        /// 对给定的一个图片（Image对象）生成一个指定大小的缩略图。
        ///</summary>
        ///<param name="srcImage">原始图片</param>
        ///<param name="thumMaxWidth">缩略图的宽度</param>
        ///<param name="thumMaxHeight">缩略图的高度</param>
        ///<returns>返回缩略图的Image对象</returns>
        public static Image GetThumbNailImage(Image srcImage, int thumMaxWidth, int thumMaxHeight)
        {
            Size thumRealSize = GetNewSize(thumMaxWidth, thumMaxHeight, srcImage.Width, srcImage.Height);
            return GetZoomedImage(srcImage, thumRealSize.Width, thumRealSize.Height);
        }

        /// <summary>
        /// 缩放图片
        /// </summary>
        /// <param name="srcImage"></param>
        /// <param name="newWidth"></param>
        /// <param name="newHeight"></param>
        /// <returns></returns>
        public static Image GetZoomedImage(Image srcImage, int newWidth, int newHeight)
        {
            if (srcImage == null)
            {
                throw new ArgumentNullException("srcImage", "图片缩放失败，源图不能为空");
            }
            if (newWidth <= 0 || newHeight <= 0)
            {
                throw new ArgumentNullException("newWidth,newHeight", "图片缩放失败，缩放大小应大于0");
            }
            Image newImage = srcImage;
            Graphics graphics = null;
            try
            {
                newImage = new System.Drawing.Bitmap(newWidth, newHeight);
                graphics = Graphics.FromImage(newImage);
                graphics.DrawImage(srcImage, new System.Drawing.Rectangle(0, 0, newWidth, newHeight), new Rectangle(0, 0, srcImage.Width, srcImage.Height), GraphicsUnit.Pixel);
            }
            catch (Exception ex)
            {
                throw new Exception("图片缩放失败", ex);
            }
            finally
            {
                if (graphics != null)
                {
                    graphics.Dispose();
                    graphics = null;
                }
            }
            return newImage;
        }

        ///<summary>
        /// 获取一个图片按等比例缩小后的大小。
        ///</summary>
        ///<param name="maxWidth">需要缩小到的宽度</param>
        ///<param name="maxHeight">需要缩小到的高度</param>
        ///<param name="imageOriginalWidth">图片的原始宽度</param>
        ///<param name="imageOriginalHeight">图片的原始高度</param>
        ///<returns>返回图片按等比例缩小后的实际大小</returns>
        public static System.Drawing.Size GetNewSize(int maxWidth, int maxHeight, int imageOriginalWidth, int imageOriginalHeight)
        {
            double w = 0.0;
            double h = 0.0;
            double src_w = Convert.ToDouble(imageOriginalWidth);
            double src_h = Convert.ToDouble(imageOriginalHeight);
            double max_w = Convert.ToDouble(maxWidth);
            double max_h = Convert.ToDouble(maxHeight);
            if (src_w < max_w && src_h < max_h)
            {
                w = src_w;
                h = src_h;
            }
            else if ((src_w / src_h) > (max_w / max_h))
            {
                w = maxWidth;
                h = (w * src_h) / src_w;
            }
            else
            {
                h = maxHeight;
                w = (h * src_w) / src_h;
            }
            return new System.Drawing.Size(Convert.ToInt32(w), Convert.ToInt32(h));
        }
    }
}