using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ImageCutter
{
    public class CutAvatarParams
    {
        public int x { get; set; }
        public int y { get; set; }
        public int w { get; set; }
        public int h { get; set; }
        public string imgSrcPath { get; set; }
        public string imgSrcRealPath { get; set; }
        /// <summary>
        /// 源图展现尺寸宽度（客户端展现源图时，可能有缩放）
        /// </summary>
        public int srcClientWidth { get; set; }
        public int srcClientHeight { get; set; }
    }
}