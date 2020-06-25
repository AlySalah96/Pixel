using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Pixel.Models
{
    public class Category
    {
        public int Id { get; set; }


        [Required]
        public string Name { get; set; }

        [JsonIgnore]

        public virtual ICollection<Article> Articles { get; set; }
    }
}