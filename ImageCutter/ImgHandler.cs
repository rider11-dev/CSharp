using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Web;

namespace ImageCutter
{
    public class ImgHandler
    {
        /// <summary>
        /// 剪裁头像图片
        /// 1、根据客户端图片大小对源图缩放（因为客户端可能已经进行了缩放，所以，裁剪前要以客户端图片尺寸为准）
        /// 2、根据客户端图片裁剪参数，截取缩放后的源图
        /// </summary>
        /// <param name="srcImgPath"></param>
        /// <param name="posX"></param>
        /// <param name="posY"></param>
        /// <param name="width"></param>
        /// <param name="height"></param>
        /// <returns></returns>
        public static bool CutAvatar(string srcImgPath, int posX = 0, int posY = 0, int width = 0, int height = 0)
        {
            if (string.IsNullOrEmpty(srcImgPath) || !File.Exists(srcImgPath))
            {
                throw new ArgumentNullException("srcImgPath", "源图路径为空或指定源图路径不存在");
            }

            if (width <= 0 || height <= 0)
            {
                throw new ArgumentNullException("width,height", "裁剪宽度、高度应大于零");
            }

            System.Drawing.Bitmap bitmap = null;   //按截图区域生成Bitmap
            System.Drawing.Image srcImg = null;  //被截图 
            System.Drawing.Graphics gps = null;    //存绘图对象   
            System.Drawing.Image avatarImg = null;  //头像图片
            try
            {
                int avatarWidth = 180;
                int avatarHeight = 180;
                bitmap = new System.Drawing.Bitmap(width, height);
                srcImg = System.Drawing.Image.FromFile(srcImgPath);
                gps = System.Drawing.Graphics.FromImage(bitmap);      //读到绘图对象
                //1、获得原始图片的截图
                gps.DrawImage(srcImg, new Rectangle(0, 0, width, height), new Rectangle(posX, posY, width, height), GraphicsUnit.Pixel);
                bitmap.Save("d:/bitmap.jpg");
                //2、截图缩放
                avatarImg = GetThumbNailImage(bitmap, avatarWidth, avatarHeight);

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

                string avatarPath = srcImgPath.Replace("src", "avatar");
                string avatarPathDir = avatarPath.Substring(0, avatarPath.LastIndexOf("\\"));

                if (!Directory.Exists(avatarPathDir))
                {
                    Directory.CreateDirectory(avatarPathDir);
                }

                if (jpegICIinfo != null)
                {
                    avatarImg.Save(avatarPath, jpegICIinfo, ep);
                }
                else
                {
                    avatarImg.Save(avatarPath);
                }

                return true;
            }
            catch (Exception ex)
            {
                throw new Exception("剪裁头像图片失败", ex);
            }
            finally
            {
                if (bitmap != null)
                {
                    bitmap.Dispose();
                }
                if (srcImg != null)
                {
                    srcImg.Dispose();
                }
                if (gps != null)
                {
                    gps.Dispose();
                }
                if (avatarImg != null)
                {
                    avatarImg.Dispose();
                }
                GC.Collect();
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

        public static Image GetZoomedImage(Image srcImage, int newWidth, int newHeight)
        {
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