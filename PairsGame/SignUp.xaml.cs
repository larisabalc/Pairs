using System;
using System.Collections.Generic;
using System.Drawing;
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
using static System.Net.Mime.MediaTypeNames;

namespace PairsGame
{
    /// <summary>
    /// Interaction logic for SignUp.xaml
    /// </summary>
    public partial class SignUp : Window
    {
        public static List<String> pathToImage = new List<String>
        {
            "Images/raven1.jpg",
            "Images/raven2.jpg",
            "Images/raven3.jpg",
            "Images/raven4.jpg",
            "Images/raven5.jpg",
            "Images/raven6.jpg",
            "Images/raven7.jpg",
            "Images/raven8.jpg"
        };
        
        public String pathToXmlFile = "../../XmlDoc.xml";

        public static int indexListPathToImage = 0;

        public SignUp()
        {
            InitializeComponent();
            characters.Source = new BitmapImage(new Uri(pathToImage[indexListPathToImage], UriKind.Relative));
        }

        private void btnSaveNewUser_Click(object sender, RoutedEventArgs e)
        {
            string name = txtBoxNumeSignUp.Text.Trim();
            if (string.IsNullOrEmpty(name))
            {
                MessageBox.Show("Please enter a name.");
                return;
            }

            XmlDocument xmlDoc = new XmlDocument();
            xmlDoc.Load(pathToXmlFile);
            XmlNode rootNode = xmlDoc.SelectSingleNode("Users");

            foreach (XmlNode user in rootNode.ChildNodes)
            {
                string existingName = user.SelectSingleNode("Name").InnerText;
                if (name == existingName)
                {
                    MessageBox.Show("A user with the same name already exists.");
                    return;
                }
            }

            XmlNode userNode = rootNode.AppendChild(xmlDoc.CreateNode(XmlNodeType.Element, "User", ""));
            userNode.AppendChild(xmlDoc.CreateNode(XmlNodeType.Element, "Name", "")).InnerText = name;
            userNode.AppendChild(xmlDoc.CreateNode(XmlNodeType.Element, "Image", "")).InnerText = ((BitmapImage)characters.Source).UriSource.ToString();
            userNode.AppendChild(xmlDoc.CreateNode(XmlNodeType.Element, "WonGames", "")).InnerText = "0";
            userNode.AppendChild(xmlDoc.CreateNode(XmlNodeType.Element, "PlayedGames", "")).InnerText = "0";

            MessageBox.Show("User saved!");
            txtBoxNumeSignUp.Clear();

            xmlDoc.Save(pathToXmlFile);
        }

        private void btnPreviousImage_Click(object sender, RoutedEventArgs e)
        {
            indexListPathToImage--;
            if (indexListPathToImage < 0)
            {
                indexListPathToImage++;
                MessageBox.Show("Reached end of list!");
            }
            else
            {
                characters.Source = new BitmapImage(new Uri(pathToImage[indexListPathToImage], UriKind.Relative));
            }
        }

        private void btnNextImage_Click(object sender, RoutedEventArgs e)
        {
            indexListPathToImage++;
            if (indexListPathToImage >= pathToImage.Count)
            {
                indexListPathToImage--;
                MessageBox.Show("Reached end of list!");
            }
            else
            {
                characters.Source = new BitmapImage(new Uri(pathToImage[indexListPathToImage], UriKind.Relative));
            }
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            this.Hide();
            MainWindow window = new MainWindow();
            window.Show();
        }
    }
}
