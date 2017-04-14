using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace Approved.DatabaseConnection
{
    class DBConnect
    {
        private MySqlConnection connection;
        private string server;
        private string database;
        private string uid;
        private string password;

        //Constructor
        public DBConnect()
        {
            InitializeConnection();
        }

        //Initialize values
        private void InitializeConnection()
        {
            server = "31.220.20.83";
            database = "u717674429_app";
            uid = "u717674429_app";
            password = "Password1234";
            

            string connectionString;
            connectionString = "SERVER=" + server + ";DATABASE=" + database + ";UID=" + uid + ";PASSWORD=" + password + ";";

            connection = new MySqlConnection(connectionString);
        }

        //open connection to database
        public bool OpenConnection()
        {
            try
            {
                connection.Open();
                return true;
            }
            catch
            {
                return false;
            }
        }

        //Close connection
        public bool CloseConnection()
        {
            try
            {
                connection.Close();
                return true;
            }
            catch
            {
                return false;
            }
        }

        public List<productPreference> GetPreference(string productName)
        {
            List<productPreference> list = new List<productPreference>();

            //Open connection
            if (this.OpenConnection() == true)
            {

                //Create Command
                string query = "SELECT * FROM app_likesDislikes WHERE Product = '" + productName + "' ORDER BY 'Date' ASC ";

                MySqlCommand cmd = new MySqlCommand(query, connection);

                //Create a data reader and Execute the command
                MySqlDataReader dataReader = cmd.ExecuteReader();

                //Read the data and store them in the list
                while (dataReader.Read())
                {
                    string _id = dataReader["ID"].ToString();
                    string _user = dataReader["User"].ToString();
                    string _product = dataReader["Product"].ToString();
                    string _pref = dataReader["Preference"].ToString();
                    string _date = dataReader["Date"].ToString();

                    productPreference newProduct = new productPreference(_id, _user, _product, _pref, _date);
                    list.Add(newProduct);
                }

                //close Data Reader
                dataReader.Close();

                //close Connection
                this.CloseConnection();

                //return list to be displayed
                return list;

            }
            else
            {
                return list;
            }
        }

        public List<Products> GetEntries()
        {
            List<Products> list = new List<Products>();

            string query = "SELECT * FROM app_products";

            //Open connection
            if (this.OpenConnection() == true)
            {
                //Create Command
                MySqlCommand cmd = new MySqlCommand(query, connection);

                //Create a data reader and Execute the command
                MySqlDataReader dataReader = cmd.ExecuteReader();

                //Read the data and store them in the list
                while (dataReader.Read())
                {
                    string _id = dataReader["ID"].ToString();
                    string _poster = dataReader["Poster"].ToString();
                    string _title = dataReader["Prod_Title"].ToString();
                    string _desc = dataReader["Prod_Desc"].ToString();
                    string _pic = dataReader["Prod_Pic"].ToString();
                    string _active = dataReader["Prod_Active"].ToString();
                    string _published = dataReader["Prod_Published"].ToString();
                    string _views = dataReader["Prod_Views"].ToString();

                    Products newProduct = new Products(_id, _poster, _title, _desc, _pic, _active, _published, _views);
                    list.Add(newProduct);
                }

                //close Data Reader
                dataReader.Close();

                //close Connection
                this.CloseConnection();

                //return list to be displayed
                return list;
            }
            else
            {
                return list;
            }
        }

        public List<Products> GetEntry(string productName)
        {
            List<Products> list = new List<Products>();

            //Open connection
            if (this.OpenConnection() == true)
            {
                //Create Command

                string query = "SELECT * FROM app_products WHERE Prod_Title = '" + productName + "'";

                MySqlCommand cmd = new MySqlCommand(query, connection);

                //Create a data reader and Execute the command
                MySqlDataReader dataReader = cmd.ExecuteReader();

                //Read the data and store them in the list
                while (dataReader.Read())
                {
                    string _id = dataReader["ID"].ToString();
                    string _poster = dataReader["Poster"].ToString();
                    string _title = dataReader["Prod_Title"].ToString();
                    string _desc = dataReader["Prod_Desc"].ToString();
                    string _pic = dataReader["Prod_Pic"].ToString();
                    string _active = dataReader["Prod_Active"].ToString();
                    string _published = dataReader["Prod_Published"].ToString();
                    string _views = dataReader["Prod_Views"].ToString();

                    Products newProduct = new Products(_id, _poster, _title, _desc, _pic, _active, _published, _views);
                    list.Add(newProduct);
                }

                //close Data Reader
                dataReader.Close();

                //close Connection
                this.CloseConnection();

                //return list to be displayed
                return list;
            }
            else
            {
                return list;
            }
        }

        public bool SetProductDetails(string productName, string productDescription, string productImage, string productVideo)
        {
            //Open connection
            if (this.OpenConnection() == true)
            {
                try
                {
                    DateTime date = DateTime.Now;

                    //Create Command
                    string query = "INSERT INTO app_products (Poster, Prod_Title, Prod_Desc, Prod_Pic, Prod_Active, Prod_Published, Prod_Video_High) VALUES (@poster, @productName, @productDescription, @productPic, @productActive, @productPublished, @productVideo)";

                    MySqlCommand cmd = new MySqlCommand(query, connection);

                    cmd.Parameters.AddWithValue("@poster", "Admin");
                    cmd.Parameters.AddWithValue("@productName", productName);
                    cmd.Parameters.AddWithValue("@productDescription", productDescription);
                    cmd.Parameters.AddWithValue("@productPic", productImage);
                    cmd.Parameters.AddWithValue("@productActive", 1);
                    cmd.Parameters.AddWithValue("@productPublished", date);


                    cmd.Parameters.AddWithValue("@productVideo", productVideo);

                    cmd.ExecuteNonQuery();

                    connection.Close();

                    //close Connection
                    this.CloseConnection();

                    return true;
                }
                catch(Exception e)
                {
                    return false;
                }
                return true;
            }
            else
            {
                return false;
            }
        }

        public bool GetLogin(string username, string password)
        {
            string _username = null;
            string _password = null;

            //Open connection
            if (this.OpenConnection() == true)
            {
                password = MD5Hash(password);

                //Create Command
                string query = "SELECT * FROM app_users WHERE Username = '" + username + "'";

                MySqlCommand cmd = new MySqlCommand(query, connection);

                //Create a data reader and Execute the command
                MySqlDataReader dataReader = cmd.ExecuteReader();

                //Read the data and store them in the list
                while (dataReader.Read())
                {
                    _username = dataReader["Username"].ToString();
                    _password = dataReader["password"].ToString();
                }

                //close Data Reader & Connection
                dataReader.Close();
                this.CloseConnection();

                if (username.Equals(_username) && password.Equals(_password))
                {
                    Console.WriteLine("Username and Password match");
                    return true;
                }
            }
            else
            {
                return false;
            }

            return false;
        }

        public List<Comments> GetComments(string productName)
        {
            List<Comments> list = new List<Comments>();

            //Open connection
            if (this.OpenConnection() == true)
            {
                //Create Command

                string query = "SELECT * FROM app_comments WHERE ProductName = '" + productName + "'";

                MySqlCommand cmd = new MySqlCommand(query, connection);

                //Create a data reader and Execute the command
                MySqlDataReader dataReader = cmd.ExecuteReader();

                //Read the data and store them in the list
                while (dataReader.Read())
                {
                    string _id = dataReader["ID"].ToString();
                    string _product = dataReader["ProductName"].ToString();
                    string _user = dataReader["User"].ToString();
                    string _date = dataReader["Date"].ToString();
                    string _comment = dataReader["Comment"].ToString();


                    Comments newComment = new Comments(_id, _product, _user, _date, _comment);
                    list.Add(newComment);
                }

                //close Data Reader
                dataReader.Close();

                //close Connection
                this.CloseConnection();

                //return list to be displayed
                return list;
            }
            else
            {
                return list;
            }
        }

        public bool deleteComment(string productName, string comment)
        {

            //Open connection
            if (this.OpenConnection() == true)
            {
                try
                {
                    DateTime date = DateTime.Now;

                    //Create Command
                    string query = "DELETE FROM app_comments WHERE ProductName = '" + productName + "' AND Comment = '" + comment + "'; ";

                    MySqlCommand cmd = new MySqlCommand(query, connection);
                    cmd.ExecuteNonQuery();

                    connection.Close();

                    //close Connection
                    this.CloseConnection();

                    return true;
                }
                catch (Exception e)
                {
                    return false;
                }
                return false;
            }
            else { return false;  }
        }

        public static string MD5Hash(string input)
        {
            StringBuilder hash = new StringBuilder();
            MD5CryptoServiceProvider md5provider = new MD5CryptoServiceProvider();
            byte[] bytes = md5provider.ComputeHash(new UTF8Encoding().GetBytes(input));

            for (int i = 0; i < bytes.Length; i++)
            {
                hash.Append(bytes[i].ToString("x2"));
            }
            return hash.ToString();
        }
    }
}
