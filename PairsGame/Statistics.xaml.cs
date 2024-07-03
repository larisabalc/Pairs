using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using System.Xml;

namespace PairsGame
{
    /// <summary>
    /// Interaction logic for Statistics.xaml
    /// </summary>
    public partial class Statistics : Window
    {

        public String pathToXmlFile = "../../XmlDoc.xml";

        public Statistics()
        {
            InitializeComponent();
            InitializeDataGrid();
        }

        private void InitializeDataGrid()
        {
            XmlDocument xmlDoc = new XmlDocument();
            xmlDoc.Load(pathToXmlFile);
            XmlNodeList nodeList = xmlDoc.DocumentElement.SelectNodes("User");

            DataTable dt = new DataTable();
            dt.Columns.Add("Name", typeof(string));
            dt.Columns.Add("WonGames", typeof(string));
            dt.Columns.Add("PlayedGames", typeof(string));

            foreach (XmlNode node in nodeList)
            {
                string name = node.SelectSingleNode("Name").InnerText;
                string wonGames = node.SelectSingleNode("WonGames").InnerText;
                string playedGames = node.SelectSingleNode("PlayedGames").InnerText;
                dt.Rows.Add(name,wonGames,playedGames);
            }

            statisticsData.ItemsSource = dt.DefaultView;
        }

    }
}
