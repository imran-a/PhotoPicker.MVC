using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using PhotoPicker.MVC.Domain;

namespace PhotoPicker.MVC.Models
{
    public class PhotoPickerContext : DbContext
    {
        public DbSet<Instance> Instances { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<SelectedPhoto> SelectedPhotos { get; set; }
    }
}