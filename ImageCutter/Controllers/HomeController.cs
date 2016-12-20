using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.IO;

namespace ImageCutter.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }

        public JsonResult UploadFile()
        {
            ResultEntity rst = new ResultEntity();

            try
            {
                HttpPostedFileBase file = HttpContext.Request.Files["input_uploadFile"];
                string folder = HttpContext.Request["folder"];
                string virtualFolder = Request.ApplicationPath + "/upload/" + (string.IsNullOrEmpty(folder) ? "unknown" : folder);//虚拟路径
                string realFolder = Server.MapPath(virtualFolder);

                if (file == null)
                {
                    rst.code = 0;
                    rst.msg = "上传文件为空";
                    return new JsonResult { Data = rst, JsonRequestBehavior = JsonRequestBehavior.AllowGet };
                }

                if (!Directory.Exists(realFolder))
                {
                    Directory.CreateDirectory(realFolder);
                }

                //这里可以校验文件后缀（只允许指定类型文件上传）
                //保存
                string fileName = Guid.NewGuid().ToString("N") + Path.GetExtension(file.FileName);

                file.SaveAs(realFolder + "/" + fileName);

                rst.code = 1;
                rst.msg = "上传成功";
                rst.data = new
                {
                    folder = folder,
                    filepath = virtualFolder + "/" + fileName
                };
            }
            catch (Exception ex)
            {
                rst.code = 0;
                rst.msg = ex.Message;
            }

            return new JsonResult { Data = rst, JsonRequestBehavior = JsonRequestBehavior.AllowGet };
        }

        public JsonResult CutImage(CutAvatarParams cutInfo)
        {
            ResultEntity rst = new ResultEntity();
            var jsonRst = new JsonResult { Data = rst, JsonRequestBehavior = JsonRequestBehavior.AllowGet };
            if (cutInfo == null)
            {
                rst.code = 0;
                rst.msg = "图片裁剪参数不能为空";
                return jsonRst;
            }

            cutInfo.imgSrcRealPath = Server.MapPath(cutInfo.imgSrcPath);

            try
            {
                ImageHelper.CutAvatar(cutInfo, 100, 100);
                rst.code = 1;
                rst.msg = "图片裁剪成功";
            }
            catch (Exception ex)
            {
                rst.code = 0;
                rst.msg = ex.Message;
            }

            return jsonRst;
        }
    }
}