using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;

namespace dbui.Model
{
    public class Article : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;
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


        private bool isFavorite;
        public bool IsFavorite
        {
            get => isFavorite;
            set
            {
                isFavorite = value;
                if (value)
                {
                    ItemColor = new SolidColorBrush(Color.FromRgb(134, 204, 142));
                }
                else
                {
                    ItemColor=new SolidColorBrush(Color.FromRgb(255, 255, 255));
                }
            }
        }

        private Brush itemColor;

        public Brush ItemColor
        {
            get => itemColor;
            set
            {
                itemColor = value;
                if (PropertyChanged != null)
                {
                    PropertyChanged(this, new PropertyChangedEventArgs("ItemColor"));
                }
            }
        }

        public void InitItemColor()
        {
            Color color = Color.FromRgb(255, 255, 255);
            if (DateTime.Now > EndDate) color = Color.FromRgb(255, 204, 204);
            else if ((EndDate - DateTime.Now).Days < 2) color = Color.FromRgb(246, 217, 111);
            ItemColor = new SolidColorBrush(color);
        }
    }
}
