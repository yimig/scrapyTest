using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;

namespace dbui.Model
{
    public class Article
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public DateTime PublishDate { get; set; }
        public string Content { get; set; }
        public Dictionary<string,string> Files { get; set; }
        public int IncomingYear { get; set; }
        public string City { get; set; }
        public string  Area { get; set; }
        public string Company { get; set; }
        public string Job { get; set; }
        public DateTime EndDate { get; set; }

        public string PublishDateString => PublishDate.ToString("yyyy年MM月dd日");
        public string EndDateString => EndDate.ToString("yyyy年MM月dd日");

        public Brush ItemColor
        {
            get
            {
                Color color = Color.FromRgb(255, 255, 255);
                if(DateTime.Now>EndDate)color=Color.FromRgb(255, 204, 204);
                else if((EndDate-DateTime.Now).Days<2)color=Color.FromRgb(246,217,111);
                return new SolidColorBrush(color);
            }
        }
    }
}
