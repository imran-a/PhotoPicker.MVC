using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using PhotoPicker.MVC.Domain;
using PhotoPicker.MVC.Services;

namespace PhotoPicker.MVC.Models
{
    public class AppViewModel
    {
        public AppViewModel(Instance instance)
        {
            Id = instance.Id;
            Title = instance.Title;
            ThumbImagePath = VirtualPathUtility.ToAbsolute(instance.ImagePathDirectory + "/thumb/");
            ImagePath = VirtualPathUtility.ToAbsolute(instance.ImagePathDirectory + "/detail/");
            Users = instance.Users.ToList();
            FinalCount = instance.FinalCount;
        }

        public int Id { get; set; }
        public string Title { get; set; }
        public List<User> Users { get; set; }
        public string ThumbImagePath { get; set; }
        public string ImagePath { get; set; }
        public int FinalCount { get; set; }
    }
}