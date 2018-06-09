using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using AudioWatermark.Models;
using System.IO;

namespace AudioWatermark.Controllers
{
    public class HomeController : Controller
    {
        private Function file;
        private audioSteg sh;
        private string message;
        [HttpGet]
        public ActionResult GetGoogleDriveFiles()
        {
            return View(GoogleDriveFilesRepository.GetDriveFiles());
        }
        public ActionResult Check()
        {
            return View();
        }
        [HttpPost]
        public ActionResult Checked(HttpPostedFileBase file)
        {
            string signature = GoogleDriveFilesRepository.checkWatermark(file);
            ViewBag.Message = signature;
            return View();
        }
        [HttpPost]
        public ActionResult DeleteFile(GoogleDriveFiles file)
        {
            GoogleDriveFilesRepository.DeleteFile(file);
            return RedirectToAction("GetGoogleDriveFiles");
        }

        [HttpPost]
        public ActionResult UploadFile(HttpPostedFileBase file)
        {
            GoogleDriveFilesRepository.FileUpload(file);
            return RedirectToAction("GetGoogleDriveFiles");
        }

        public void DownloadFile(string id)
        {
            string FilePath = GoogleDriveFilesRepository.DownloadGoogleFile(id);
            file = new Function(new FileStream(FilePath, FileMode.Open, FileAccess.Read));
            sh = new audioSteg(file);
            message = "Copyright by ThanhBinh " ;
            sh.waterMess(message);
            file.writeFile(FilePath);
            Response.ContentType = "application/zip";
            Response.AddHeader("Content-Disposition", "attachment; filename=" + Path.GetFileName(FilePath));
            Response.WriteFile(System.Web.HttpContext.Current.Server.MapPath("~/GoogleDriveFiles/" + Path.GetFileName(FilePath)));
            Response.End();
            Response.Flush();
            System.IO.File.Delete(FilePath);
        }
    }
}