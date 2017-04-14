using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Approved
{
    class Products
    {
        public string ProductID { get; set; }
        public string ProductPoster { get; set; }
        public string ProductTitle { get; set; }
        public string ProductDescription { get; set; }
        public string ProductPicture { get; set; }
        public string ProductActive { get; set; }
        public string ProductPublished { get; set; }
        public string ProductViews { get; set; }

        public Products(string id, string poster, string title, string desc, string pic, string active, string published, string views)
        {
            ProductID = id;
            ProductPoster = poster;
            ProductTitle = title;
            ProductDescription = desc;
            ProductPicture = pic;
            ProductActive = active;
            ProductPublished = published;
            ProductViews = views;
        }
    }
}
