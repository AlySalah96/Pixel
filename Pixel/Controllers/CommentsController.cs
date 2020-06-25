using Pixel.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web;
using System.Web.Http;

namespace Pixel.Controllers
{
    public class CommentsController : ApiController
    {
        private PixelContext db = new PixelContext();

        // POST: api/Comment

        public IHttpActionResult PostComment()
        {
            Comment comment = new Comment();
            var httpRequest = HttpContext.Current.Request;
            comment.Content = httpRequest["Content"];
            comment.ArticleId = int.Parse(httpRequest["ArticleId"]);
            comment.UserId = httpRequest["UserId"];





            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.Comments.Add(comment);
            db.SaveChanges();

            return Ok(comment);//CreatedAtRoute("DefaultApi", new { id = article.Id }, article);
        }

        [Route("api/comments/GetCommentsByArticle/{id}")]

        public IQueryable GetCommentsByArticle(int id)
        {
            var comments = db.Comments.Where(c => c.ArticleId == id);
            return comments;

        }
    }
}
