using System;
using System.Collections.Generic;
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

namespace PairsGame
{
    /// <summary>
    /// Interaction logic for CustomGameWindow.xaml
    /// </summary>
    public partial class CustomGameWindow : Window
    {
        public PlayMenu playMenu;

        public CustomGameWindow (PlayMenu playMenu) { this.playMenu = playMenu; InitializeComponent(); }

        public CustomGameWindow()
        {
            InitializeComponent();
        }

        private void GenerateCustom_Click(object sender, RoutedEventArgs e)
        {
            this.Hide();

            playMenu.rows = Convert.ToInt32(txtRowsCustom.Text);
            playMenu.cols = Convert.ToInt32(txtColsCustom.Text);

            this.playMenu.Show();
        }
    }
}
