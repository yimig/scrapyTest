using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace dbui.Data
{
    public static class Settings
    {
        public static string FileName = "settings.json";

        public static void WriteFavorite(List<int> favoriteList)
        {
            var json_str = JsonConvert.SerializeObject(favoriteList);
            FileStream fs=new FileStream(FileName,FileMode.Create);
            StreamWriter sw =new StreamWriter(fs);
            sw.Write(json_str);
            sw.Close();
            fs.Close();
        }

        public static List<int> ReadFavorite()
        {
            FileStream fs=new FileStream(FileName,FileMode.Open);
            StreamReader sr=new StreamReader(fs);
            var json_str = sr.ReadToEnd();
            sr.Close();
            fs.Close();
            return JsonConvert.DeserializeObject<List<int>>(json_str);
        }

        public static bool CheckSettingFile()
        {
            return File.Exists(FileName);
        }
    }
}
