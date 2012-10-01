using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using PhotoPicker.MVC.Domain;
using PhotoPicker.MVC.Models;
using PhotoPicker.MVC.Services;

namespace PhotoPicker.MVC.Controllers
{
    public class AppController : Controller
    {

        PhotoPickerContext db = new PhotoPickerContext();
        PhotoService photoService = new PhotoService();

        // List all
        public ActionResult Index()
        {
            return View();
        }

        // View instance
        public ActionResult View(int id)
        {
            var view = new AppViewModel(db.Instances.FirstOrDefault(i => i.Id == id));
            return View(view);
        }

        // get users liked photos
        public JsonResult GetSelectedPhotosForUser(int userId)
        {
            return Json(
                db.SelectedPhotos.Where(s => s.UserId == userId).Select(s => s.Id).ToList(),
                JsonRequestBehavior.AllowGet
                );
        }

        [HttpPost]
        public JsonResult UserSelectPhoto(int userId, string imageName)
        {
            var photo = db.SelectedPhotos.Where(s => s.ImageName.ToLower() == imageName.ToLower() && s.UserId == userId).FirstOrDefault();
            if (photo == null)
            {
                photo = new SelectedPhoto() { ImageName = imageName.ToLower(), UserId = userId };
                db.SelectedPhotos.Add(photo);
                db.SaveChanges();
            }

            return Json(photo.Id);
        }

        [HttpPost]
        public JsonResult UserDeselectPhoto(int userId, int selectedPhotoId) 
        {
            var photo = db.SelectedPhotos.Where(s => s.Id == selectedPhotoId && s.UserId == userId).FirstOrDefault();
            if (photo != null)
            {
                db.SelectedPhotos.Remove(photo);
                db.SaveChanges();
            }

            return Json(true);
        }

        [HttpGet]
        public JsonResult GetAllPhotos(int appId, int userId, int page = 1)
        {
            var view = new PhotosViewModel();
            
            var app = db.Instances.Where(i => i.Id == appId).First();
            if (app != null)
            {
                int skip = (page - 1) * app.PageSize;

                var images = photoService.GetPhotos(app.ImagePathDirectory + "/thumb/", app.PageSize, page)
                                .Skip(skip).Take(app.PageSize).ToList();
                
                // for this set of images, which ones has the user selected
                var userSelectedPhotos = db.SelectedPhotos.Where(s => s.UserId == userId).ToList();
                PhotosViewModel.Photo photo;
                foreach (var image in images)
                {
                    photo = new PhotosViewModel.Photo() { ImageName = image };

                    var selected = userSelectedPhotos.Where(s => s.ImageName == image.ToLower()).FirstOrDefault();
                    if (selected != null)
                        photo.SelectedPhotoId = selected.Id;

                    view.Photos.Add(photo);
                }

                view.CurrentPage = page;
                view.PageSize = app.PageSize;
                view.TotalPhotos = photoService.GetPhotos(app.ImagePathDirectory + "/thumb/", app.PageSize, page).Count();
                view.Pages = (int)Math.Ceiling(view.TotalPhotos / (double)app.PageSize);
            }

            return Json(view, JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetUserSelectedPhotos(int appId, int userId, int page = 1)
        {
            var view = new PhotosViewModel();
            var app = db.Instances.Where(i => i.Id == appId).First();
            
            if (app != null)
            {
                view.Photos = db.SelectedPhotos.Where(s => s.UserId == userId).OrderBy(s => s.ImageName)
                                    .Select(s => new PhotosViewModel.Photo() { ImageName = s.ImageName, SelectedPhotoId = s.Id })
                                    .ToList();
                
                view.CurrentPage = page;
                view.PageSize = app.PageSize;
                view.TotalPhotos = photoService.GetPhotos(app.ImagePathDirectory + "/thumb/", app.PageSize, page).Count();
                view.Pages = (int)Math.Ceiling(view.TotalPhotos / (double)app.PageSize);
            }

            return Json(view, JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetAllLikedPhotos(int appId, int userId, int page = 1)
        {
            var view = new PhotosAllSelectedViewModel();
            var app = db.Instances.Where(i => i.Id == appId).First();

            if (app != null)
            {
                var all = db.SelectedPhotos.Where(s => s.User.InstanceId == appId).ToList();
                var distinctImageNames = all.Select(s => s.ImageName).Distinct();

                PhotosAllSelectedViewModel.Photo photo;
                foreach (string imageName in distinctImageNames)
                {
                    photo = new PhotosAllSelectedViewModel.Photo() { ImageName = imageName };

                    var totalSelected = all.Where(s => s.ImageName == imageName).Count();
                    var selectedByUser = all.Where(s => s.ImageName == imageName && s.UserId == userId).FirstOrDefault();
                    if (selectedByUser != null)
                    {
                        photo.SelectedPhotoId = selectedByUser.Id;
                        if (totalSelected > 1) photo.Others = true;
                    }
                    else
                    {
                        if (totalSelected > 0) photo.Others = true;
                    }

                    view.Photos.Add(photo);
                }

                view.CurrentPage = page;
                view.PageSize = app.PageSize;
                view.TotalPhotos = photoService.GetPhotos(app.ImagePathDirectory + "/thumb/", app.PageSize, page).Count();
                view.Pages = (int)Math.Ceiling(view.TotalPhotos / (double)app.PageSize);
            }

            return Json(view, JsonRequestBehavior.AllowGet);
        }

    }
}
