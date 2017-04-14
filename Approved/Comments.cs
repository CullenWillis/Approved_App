using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Approved
{
    class Comments
    {
        public string CommentID { get; set; }
        public string CommentTitle { get; set; }
        public string CommentUser { get; set; }
        public string CommentDate { get; set; }
        public string CommentText { get; set; }

        public Comments(string id, string title, string user, string date, string text)
        {
            CommentID = id;
            CommentTitle = title;
            CommentUser = user;
            CommentDate = date;
            CommentText = text;
        }
    }
}
