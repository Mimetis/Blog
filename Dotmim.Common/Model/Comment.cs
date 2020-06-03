using System;
using System.Collections.Generic;
using System.Text;

namespace Dotmim.Common.Model
{
    public class Comment
    {
        public Guid ID { get; set; }

        public Guid PostID { get; set; }

        public string Author { get; set; }

        public string Email { get; set; }

        public string CommentContent { get; set; }

        public DateTime? PubDate { get; set; }

        public bool? IsAdmin { get; set; }


    }
}
