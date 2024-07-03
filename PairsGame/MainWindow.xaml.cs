using System;
using System.Collections.Generic;
using System.IO;
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
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Xml;
using System.Xml.Linq;

namespace PairsGame
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public String pathToXmlFile = "../../XmlDoc.xml";

        public Dictionary<String, User> users = new Dictionary<string, User>();

        User selectedUser = null;

        public MainWindow()
        {
            InitializeComponent();
            InitializeUserList();
            btnDeleteUser1.Visibility = Visibility.Hidden;
            btnPlay.Visibility = Visibility.Hidden;
        }

        private void InitializeUserList()
        {
            XmlDocument xmlDoc = new XmlDocument();
            xmlDoc.Load(pathToXmlFile);
            XmlNodeList nodeList = xmlDoc.DocumentElement.SelectNodes("User");
            foreach (XmlNode node in nodeList)
            {
                User user = new User();
                user.SetPathToImage(node.SelectSingleNode("Image").InnerText);
                user.SetName(node.SelectSingleNode("Name").InnerText);
                usersList.Items.Add(user.GetName());
                userPhoto.Source = new BitmapImage(new Uri(user.GetPathToImage(), UriKind.Relative));
                users[user.GetName()] = user;
            }
        }

        private void DeleteFromXmlFile(String userName)
        {
            XmlDocument xmlDoc = new XmlDocument();
            xmlDoc.Load(pathToXmlFile);
            XmlNodeList nodeList = xmlDoc.DocumentElement.SelectNodes("User");
            foreach (XmlNode node in nodeList)
            {
                if(node.SelectSingleNode("Name").InnerText == userName)
                {
                    node.ParentNode.RemoveChild(node);
                    users.Remove(userName);
                }

            }
            xmlDoc.Save(pathToXmlFile);
        }

        private void btnPlay_Click(object sender, RoutedEventArgs e)
        {
            this.Hide();
            PlayMenu playMenu = new PlayMenu(selectedUser);
            playMenu.Show();
        }

        private void btnNewUser_Click(object sender, RoutedEventArgs e)
        {
            this.Hide();
            SignUp signUp = new SignUp();
            signUp.Show();
        }

        private void usersList_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

            object item = usersList.SelectedItem;
            if (item == null)
            {
                return;
            }
            else
            {
               btnDeleteUser1.Visibility = Visibility.Visible;
               btnPlay.Visibility = Visibility.Visible;
               userPhoto.Source = new BitmapImage(new Uri(users[item.ToString()].GetPathToImage(), UriKind.Relative));
               selectedUser = users[item.ToString()];
            }

        }

        private void btnDeleteUser1_Click(object sender, RoutedEventArgs e)
        {
            object item = usersList.SelectedItem;
            if (item == null)
            {
                return;
            }
            else
            {
                DeleteFromXmlFile(item.ToString());
                MessageBox.Show("User deleted!");

                usersList.Items.Clear();
                InitializeUserList();
            }
        }

        private void btnCancel_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }
    }
}
