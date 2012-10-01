using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PhotoPicker.MVC.Models
{
    public class PhotosViewModel
    {
        public PhotosViewModel()
        {
            Photos = new List<Photo>();
        }

        public int CurrentPage { get; set; }
        public int PageSize { get; set; }
        public int Pages { get; set; }
        public int TotalPhotos { get; set; }
        public List<Photo> Photos { get; set; }

        public class Photo
        {
            public string ImageName { get; set; }
            public int SelectedPhotoId { get; set; }
        }

    }

    public class PhotosAllSelectedViewModel
    {
        public PhotosAllSelectedViewModel()
        {
            Photos = new List<Photo>();
        }

        public int CurrentPage { get; set; }
        public int PageSize { get; set; }
        public int Pages { get; set; }
        public int TotalPhotos { get; set; }
        public List<Photo> Photos { get; set; }

        public class Photo
        {
            public string ImageName { get; set; }
            public int SelectedPhotoId { get; set; }
            public bool Others { get; set; }
        }

    }
}