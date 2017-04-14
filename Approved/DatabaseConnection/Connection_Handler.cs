using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Approved.DatabaseConnection
{
    class Connection_Handler
    {
        DBConnect database;

        public Connection_Handler()
        {
            database = new DBConnect();
        }

        public List<productPreference> GetPreference(string productName)
        {
            List<productPreference> list = new List<productPreference>();
            list = database.GetPreference(productName);

            return list;
        }

        public List<Products> GetEntries()
        {
            List<Products> list = new List<Products>();
            list = database.GetEntries();

            return list;
        }

        public List<Products> GetEntry(string productName)
        {
            List<Products> list = new List<Products>();
            list = database.GetEntry(productName);

            return list;
        }

        public List<Comments> GetComments(string productName)
        {
            List<Comments> list = new List<Comments>();
            list = database.GetComments(productName);

            return list;
        }

        public bool SetProductDetails(string productName, string productDescription, string productImage, string productVideo)
        {
            if (database.SetProductDetails(productName, productDescription, productImage, productVideo))
                return true;
            else
                return false;
        }

        public bool deleteComment(string productName, string comment)
        {
            if (database.deleteComment(productName, comment))
                return true;
            else
                return false;
        }

        public bool GetLogin(string username, string password)
        {
            if (database.GetLogin(username, password))
                return true;
            else
                return false;
        }
    }
}
