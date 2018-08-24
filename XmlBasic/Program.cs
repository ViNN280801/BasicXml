using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Xml;

namespace XmlBasic
{
    class Program
    {
        static void Main(string[] args)
        {
            foreach (News item in GetDataFromXmlDocument())
                Console.WriteLine(item);

            XmlDocument doc = CreatingAndFillingXmlDocument(GetDataFromXmlDocument());
            doc.Save("HabrText.xml");
        }

        static List<News> GetDataFromXmlDocument()
        {
            //Creating Xml document
            XmlDocument document = new XmlDocument();

            //Loading Xml data from link
            document.Load("https://habr.com/rss/interesting/");

            var items = document.SelectNodes("rss/channel/item");

            //Creating collection and writing data into Xml file
            List<News> News_ = new List<News>();

            foreach (XmlNode item in items)
            {
                News news = new News();
                news.Title = item.SelectSingleNode("title").InnerText;
                news.Title = item.SelectSingleNode("link").InnerText;
                news.Title = item.SelectSingleNode("description").InnerText;

                DateTime pub_date = news.PubDate;
                string str = item.SelectSingleNode("pubDate").InnerText;
                DateTime.TryParse(str, out pub_date);

                string[] createText = { str };

                File.WriteAllLines("XmlBasic.xml", createText, Encoding.UTF8);

                News_.Add(news);
            }

            
           

            return News_;

            #region useless_code
            //Returning root of Xml document
            XmlElement root = document.DocumentElement;

            //Getting all children(attributes) in Xml
            foreach (XmlNode children in root.ChildNodes)
            {
                foreach (XmlNode element in children.ChildNodes)
                {
                    foreach (XmlNode chelem in element.ChildNodes)
                    {
                        if (chelem.Name == "title" || chelem.Name == "link" || (chelem.Name == "description"))
                            Console.WriteLine(children.InnerText);

                    }
                    Console.WriteLine("- - - - - - - - - - - - - - - - - - - - ");
                }
            }
            #endregion 

        }

        static XmlDocument CreatingAndFillingXmlDocument(List<News> news_)
        {
            XmlDocument doc = new XmlDocument();
            XmlElement root = doc.CreateElement("News");

            foreach (News item in news_)
            {
                XmlElement news = doc.CreateElement("news");

                XmlElement title = doc.CreateElement("title");
                title.InnerText = item.Title;
                news.AppendChild(title);
            
                XmlElement description = doc.CreateElement("description");
                description.InnerText = item.Description;
                news.AppendChild(description);
               
                XmlElement link = doc.CreateElement("link");
                link.InnerText = item.Link;
                news.AppendChild(link);
               
                XmlElement PD = doc.CreateElement("pubDate");
                PD.InnerText = item.PubDate.ToString();
                news.AppendChild(PD);

                root.AppendChild(news);
            }
            doc.AppendChild(root);

            return doc;
        }
    }
    public class News
    {
        public string Title { get; set; }
        public string Description { get; set; }
        public string Link { get; set; }
        public DateTime PubDate { get; set; }
        public override string ToString()
        {
            string str = string.Format(">>{0}\n{1}\n[Description]{2}\n{3}\n\n", Title, Link, Description, PubDate);

            return str;
        }
    }
}