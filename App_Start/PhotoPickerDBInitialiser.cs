using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Validation;
using System.Diagnostics;
using System.Linq;
using System.Web;
using PhotoPicker.MVC.Domain;
using PhotoPicker.MVC.Models;

namespace PhotoPicker.MVC
{
    public class PhotoPickerDBInitialiser : CreateDatabaseIfNotExists<PhotoPickerContext>
    {
        protected override void Seed(PhotoPickerContext context)
        {
            var instances = new List<Instance>()
            {
                new Instance() 
                {
                    Title = "Wedding Pictures",
                    ImagePathDirectory = "~/content/images/1/",
                    PageSize = 50,
                    Users = new List<User> 
                    {
                        new User 
                        { 
                            Name = "Imran"
                        },
                        new User 
                        {
                            Name = "Emmi"
                        }
                    }
                }
            };

            try
            {
                instances.ForEach(a => context.Instances.Add(a));
                context.SaveChanges();
                base.Seed(context);
            }
            catch (DbEntityValidationException dbEx)
            {
                foreach (var validationErrors in dbEx.EntityValidationErrors)
                {
                    foreach (var validationError in validationErrors.ValidationErrors)
                    {
                        Trace.TraceInformation("Property: {0} Error: {1}", validationError.PropertyName, validationError.ErrorMessage);
                    }
                }
            }
        }
    }
}