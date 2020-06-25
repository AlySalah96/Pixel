using Microsoft.AspNet.Identity.EntityFramework;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;

namespace Pixel.Models
{
    public class PixelContext: IdentityDbContext
    {
        public PixelContext():base("name=PixelContext")
            {}
             public virtual DbSet<Category> Categories { get; set; }
        public virtual DbSet<Article> Articles { get; set; }
        public virtual DbSet<Comment> Comments { get; set; }


        object placeHolderVariable;
     
    }
}