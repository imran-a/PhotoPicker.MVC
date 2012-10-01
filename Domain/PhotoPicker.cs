using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace PhotoPicker.MVC.Domain
{
    public class Instance
    {
        public int Id { get; set; }

        [Required]
        public string Title { get; set; }

        [Required]
        public string ImagePathDirectory { get; set; }

        [Required]
        public int PageSize { get; set; }

        public virtual ICollection<User> Users { get; set; }
    }

    public class User
    {
        public int Id { get; set; }

        [Required]
        public string Name { get; set; }

        public int? InstanceId { get; set; }

        [Required]
        public virtual Instance Instance { get; set; }

        public virtual ICollection<SelectedPhoto> SelectedPhotos { get; set; }
    }

    public class SelectedPhoto
    {
        public int Id { get; set; }

        [Required]
        public string ImageName { get; set; }

        [Required]
        public int? UserId { get; set; }

        public virtual User User { get; set; }

    }

    public class Photo
    {
        public string ImageName { get; set; }
    }
}