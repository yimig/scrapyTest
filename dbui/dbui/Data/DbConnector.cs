using System;
using System.Collections.Generic;
using System.ComponentModel.Design;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using dbui.Model;
using MySql.Data.MySqlClient;
using Newtonsoft.Json;

namespace dbui.Data
{
    public class DbConnector
    {
        private MySqlConnection conn;

        public void Close()
        {
            conn.Close();
        }

        public Dictionary<string, string> GetFilesDict(string jsonStr)
        {
            return JsonConvert.DeserializeObject<Dictionary<string, string>>(jsonStr);
        }

        public List<Article> GetArticles(string sql)
        {
            List<Article> articles = new List<Article>();
            try
            {
                conn.Open();
                MySqlCommand cmd = new MySqlCommand(sql, conn);
                MySqlDataReader rdr = cmd.ExecuteReader();
                while (rdr.Read())
                {
                    var article = new Article();
                    article.Id = Convert.ToInt32(rdr[0]);
                    article.Title = Convert.ToString(rdr[1]);
                    article.Content = Convert.ToString(rdr[2]);
                    article.Files = GetFilesDict(Convert.ToString(rdr[3]));
                    article.PublishDate = DateTime.Parse(Convert.ToString(rdr[4]));
                    article.IncomingYear = Convert.ToInt32(rdr[5]);
                    article.City = Convert.ToString(rdr[6]);
                    article.Area = Convert.ToString(rdr[7]);
                    article.Company = Convert.ToString(rdr[8]);
                    article.Job = Convert.ToString(rdr[9]);
                    article.EndDate = DateTime.Parse(Convert.ToString(rdr[10]));
                    articles.Add(article);
                }
                rdr.Close();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }
            Close();
            return articles;
        }

        public List<Article> GetAllArticle()
        {
            string sql = "SELECT * FROM article order by id desc";
            return GetArticles(sql);
        }


        public DbConnector()
        {
            string connStr = "server=localhost;user=shiyebian_user;database=shiyebian;port=3306;password=1320";
            conn = new MySqlConnection(connStr);
        }
    }
}
