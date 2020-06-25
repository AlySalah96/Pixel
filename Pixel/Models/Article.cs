using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace Pixel.Models
{
    public class Article
    {
        public int Id { get; set; }
        [Required]
        public string Title { get; set; }

        [Required]
        public string Body { get; set; }

        [Required]
        public DateTime Date { get; set; }

        public int CatId { get; set; }

        [JsonIgnore]
        [ForeignKey("CatId")]
        public virtual Category Category { get; set; }

        [JsonIgnore]
        public virtual ICollection<Comment> Comments{ get; set; }



    }
}