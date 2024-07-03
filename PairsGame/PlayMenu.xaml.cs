using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
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
    /// Interaction logic for PlayMenu.xaml
    /// </summary>
    public partial class PlayMenu : Window
    {
        public static List<String> pathToImages = new List<String>
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

        private User currentUser = null;

        private int level = 1;

        public String pathToXmlFile = "../../XmlDoc.xml";

        public int rows { get; set; } = 5;
        public int cols { get; set; } = 5;

        private Button firstGuess = null;
        private Button secondGuess = null;
        private Button previousGuess = null;

        private bool allowClick = false;

        private int indexFirstGuess, indexSecondGuess;

        public String[,] board;

        public bool[,] guessesBoard;

        public PlayMenu(User user)
        {
            currentUser = user;
            InitializeComponent();
            currentUserName.Content = currentUser.GetName();
            currentUserLevel.Content = "";
            photoCurrentUser.Source = new BitmapImage(new Uri(currentUser.GetPathToImage(), UriKind.Relative));
            InitializeNrGames();
        }

        private void InitializeNrGames()
        {
            XmlDocument xmlDoc = new XmlDocument();
            xmlDoc.Load(pathToXmlFile);
            XmlNodeList nodeList = xmlDoc.DocumentElement.SelectNodes("User");
            foreach (XmlNode node in nodeList)
            {
                if (node.SelectSingleNode("Name").InnerText == currentUser.GetName())
                {
                    currentUser.SetWonGame(int.Parse(node.SelectSingleNode("WonGames").InnerText));
                    currentUser.SetPlayedGame(int.Parse(node.SelectSingleNode("PlayedGames").InnerText));
                }
            }
        }

        public int CountGuesses()
        {
            int count = 0;
            for (int i = 0; i < rows; i++)
            {
                for (int j = 0; j < cols; j++)
                {
                    if (guessesBoard[i, j] == true)
                    {
                        count++;
                    }
                }
            }
            return count;
        }

        public void InitializeGuessesBoard()
        {
            guessesBoard = new bool[rows, cols];
            for (int i = 0; i < rows; i++)
            {
                for (int j = 0; j < cols; j++)
                {
                    guessesBoard[i, j] = false;
                }
            }
        }

        private List<Tuple<int, int>> GetEmptyPositions()
        {
            List<Tuple<int, int>> emptyPositions = new List<Tuple<int, int>>();
            for (int i = 0; i < rows; i++)
            {
                for (int j = 0; j < cols; j++)
                {
                    if (board[i, j] == "")
                    {
                        emptyPositions.Add(new Tuple<int, int>(i, j));
                    }
                }
            }
            return emptyPositions;
        }

        public void InitializeBoard()
        {
            board = new String[rows, cols];
            for (int i = 0; i < rows; i++)
            {
                for (int j = 0; j < cols; j++)
                {
                    board[i, j] = "";
                }
            }

            Random random = new Random();
            while (GetEmptyPositions().Count > 1)
            {
                List<Tuple<int, int>> emptyPositions = GetEmptyPositions();
                int image = random.Next(0, 7);
                int position1 = random.Next(0, emptyPositions.Count);
                board[emptyPositions[position1].Item1, emptyPositions[position1].Item2] = pathToImages[image];
                emptyPositions = GetEmptyPositions();
                int position2 = random.Next(0, emptyPositions.Count);
                board[emptyPositions[position2].Item1, emptyPositions[position2].Item2] = pathToImages[image];
            }

            if (GetEmptyPositions().Count == 1)
            {
                List<Tuple<int, int>> emptyPositions = GetEmptyPositions();
                int image = random.Next(0, 7);
                int position1 = random.Next(0, emptyPositions.Count);
                board[emptyPositions[position1].Item1, emptyPositions[position1].Item2] = pathToImages[image];
            }

        }

        private void CreateBoard()
        {
            currentUserName.Content = currentUser.GetName();
            currentUserLevel.Content = "Level: " + level.ToString();
            photoCurrentUser.Source = new BitmapImage(new Uri(currentUser.GetPathToImage(), UriKind.Relative));

            matrix.HorizontalAlignment = HorizontalAlignment.Stretch;
            matrix.VerticalAlignment = VerticalAlignment.Stretch;

            matrix.RowDefinitions.Clear();
            matrix.ColumnDefinitions.Clear();

            double rowHeight = matrix.ActualHeight / rows;
            double colWidth = matrix.ActualWidth / cols;

            for (int i = 0; i < rows; i++)
            {
                var rowDefinition = new RowDefinition();
                rowDefinition.Height = new GridLength(rowHeight);
                matrix.RowDefinitions.Add(rowDefinition);
            }

            for (int i = 0; i < cols; i++)
            {
                var columnDefinition = new ColumnDefinition();
                columnDefinition.Width = new GridLength(colWidth);
                matrix.ColumnDefinitions.Add(columnDefinition);
            }

            for (int i = 0; i < rows; i++)
            {
                for (int j = 0; j < cols; j++)
                {
                    var button = new Button();
                    if (guessesBoard[i,j] == false)
                       button.Content = (i * cols) + j + 1;
                    else
                    {
                        BitmapImage btm = new BitmapImage(new Uri(board[i,j], UriKind.Relative));
                        Image image = new Image();
                        image.Source = btm;
                        button.Content = image;
                    }
                    button.HorizontalAlignment = HorizontalAlignment.Stretch;
                    button.VerticalAlignment = VerticalAlignment.Stretch;

                    Grid.SetColumn(button, j);
                    Grid.SetRow(button, i);
                    matrix.Children.Add(button);
                    button.Click += Button_Click;
                }
            }
        }

        private void Play_Click(object sender, RoutedEventArgs e)
        {
            currentUser.newPlayedGame();
            ChangeStistics();
            InitializeBoard();
            InitializeGuessesBoard();
            CreateBoard();
        }

        private void Exit_Click(object sender, RoutedEventArgs e)
        {
            this.Hide();
            MainWindow window = new MainWindow();
            window.Show();
        }

        private void Custom_Click(object sender, RoutedEventArgs e)
        {
            this.Hide();
            CustomGameWindow customGameWindow = new CustomGameWindow(this);
            customGameWindow.Show();
        }

        private void About_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
            AboutWindow aboutWindow = new AboutWindow();
            aboutWindow.Show();
        }

        private void ChangeStistics()
        {
            XmlDocument xmlDoc = new XmlDocument();
            xmlDoc.Load(pathToXmlFile);
            XmlNodeList nodeList = xmlDoc.DocumentElement.SelectNodes("User");
            foreach (XmlNode node in nodeList)
            {
                if (node.SelectSingleNode("Name").InnerText == currentUser.GetName())
                {
                    XmlNode wonGames = node.SelectSingleNode("WonGames");
                    if (wonGames != null)
                    {
                        wonGames.ParentNode.RemoveChild(wonGames);
                    }

                    XmlNode playedGames = node.SelectSingleNode("PlayedGames");
                    if (playedGames != null)
                    {
                        playedGames.ParentNode.RemoveChild(playedGames);
                    }
                    node.AppendChild(xmlDoc.CreateNode(XmlNodeType.Element, "WonGames", "")).InnerText = currentUser.GetWonGame().ToString();
                    node.AppendChild(xmlDoc.CreateNode(XmlNodeType.Element, "PlayedGames", "")).InnerText = currentUser.GetPlayedGame().ToString();
                }
            }
            xmlDoc.Save(pathToXmlFile);
        }

        private void Statistics_Click(object sender, RoutedEventArgs e)
        {
            Statistics statistics = new Statistics();
            statistics.ShowDialog();
        }

        private void OpenGame_Click(object sender, RoutedEventArgs e)
        {
            XmlDocument xmlDoc = new XmlDocument();
            xmlDoc.Load(pathToXmlFile);

            XmlNodeList userList = xmlDoc.GetElementsByTagName("User");
            foreach (XmlNode userNode in userList)
            {
                if (userNode.SelectSingleNode("Name").InnerText == currentUser.GetName())
                {
                    rows = int.Parse(userNode.SelectSingleNode("Rows").InnerText);
                    cols = int.Parse(userNode.SelectSingleNode("Cols").InnerText);

                    XmlNode matrixNode = userNode.SelectSingleNode("Matrix");

                    board = new string[rows, cols];

                    XmlNodeList rowList = matrixNode.SelectNodes("Row");
                    for (int i = 0; i < rows; i++)
                    {
                        XmlNode rowNode = rowList.Item(i);
                        XmlNodeList valueList = rowNode.SelectNodes("Value");
                        for (int j = 0; j < cols; j++)
                        {
                            XmlNode valueNode = valueList.Item(j);
                            string value = valueNode.InnerText;
                            board[i, j] = value;
                        }
                    }

                    matrixNode = userNode.SelectSingleNode("GuessesMatrix");

                    guessesBoard = new bool[rows, cols];

                    rowList = matrixNode.SelectNodes("Row");
                    for (int i = 0; i < rows; i++)
                    {
                        XmlNode rowNode = rowList.Item(i);
                        XmlNodeList valueList = rowNode.SelectNodes("Value");
                        for (int j = 0; j < cols; j++)
                        {
                            XmlNode valueNode = valueList.Item(j);
                            bool value = bool.Parse(valueNode.InnerText.ToLower());
                            guessesBoard[i, j] = value;
                        }
                    }
                    CreateBoard();
                    break;
                }
            }
        }

        private void SaveGame_Click(object sender, RoutedEventArgs e)
        {
            XmlDocument xmlDoc = new XmlDocument();
            xmlDoc.Load(pathToXmlFile);
            XmlNodeList nodeList = xmlDoc.DocumentElement.SelectNodes("User");
            foreach (XmlNode node in nodeList)
            {
                if (node.SelectSingleNode("Name").InnerText == currentUser.GetName())
                {
                    XmlNode matrixNode = node.SelectSingleNode("Matrix");
                    if (matrixNode != null)
                    {
                        matrixNode.ParentNode.RemoveChild(matrixNode);
                    }

                    XmlNode guessesMatrixNode = node.SelectSingleNode("GuessesMatrix");
                    if (guessesMatrixNode != null)
                    {
                        guessesMatrixNode.ParentNode.RemoveChild(guessesMatrixNode);
                    }

                    XmlNode levelNode = node.SelectSingleNode("Level");
                    if (levelNode != null)
                    {
                        levelNode.ParentNode.RemoveChild(levelNode);
                    }

                    XmlNode rowsNode = node.SelectSingleNode("Rows");
                    if (rowsNode != null)
                    {
                        rowsNode.ParentNode.RemoveChild(rowsNode);
                    }

                    XmlNode colsNode = node.SelectSingleNode("Cols");
                    if (colsNode != null)
                    {
                        colsNode.ParentNode.RemoveChild(colsNode);
                    }

                    node.AppendChild(xmlDoc.CreateNode(XmlNodeType.Element, "Level", "")).InnerText = level.ToString();
                    node.AppendChild(xmlDoc.CreateNode(XmlNodeType.Element, "Rows", "")).InnerText = rows.ToString();
                    node.AppendChild(xmlDoc.CreateNode(XmlNodeType.Element, "Cols", "")).InnerText = cols.ToString();

                    XmlElement matrixElem = xmlDoc.CreateElement("Matrix");
                    for (int i = 0; i < board.GetLength(0); i++)
                    {
                        XmlElement rowElem = xmlDoc.CreateElement("Row");

                        for (int j = 0; j < board.GetLength(1); j++)
                        {
                            XmlElement valueElem = xmlDoc.CreateElement("Value");
                            valueElem.InnerText = board[i, j].ToString();
                            rowElem.AppendChild(valueElem);
                        }
                        matrixElem.AppendChild(rowElem);
                    }
                    node.AppendChild(matrixElem);

                    matrixElem = xmlDoc.CreateElement("GuessesMatrix");
                    for (int i = 0; i < guessesBoard.GetLength(0); i++)
                    {
                        XmlElement rowElem = xmlDoc.CreateElement("Row");

                        for (int j = 0; j < guessesBoard.GetLength(1); j++)
                        {
                            XmlElement valueElem = xmlDoc.CreateElement("Value");
                            valueElem.InnerText = guessesBoard[i, j].ToString();
                            rowElem.AppendChild(valueElem);
                        }
                        matrixElem.AppendChild(rowElem);
                    }
                    node.AppendChild(matrixElem);

                    MessageBox.Show("Game Saved!");

                    matrix.Children.Clear();
                    currentUserLevel.Content = "";

                    break;
                }
            }
            xmlDoc.Save(pathToXmlFile);
        }

        private async void Button_Click(object sender, RoutedEventArgs e)
        {
            if (firstGuess == null)
            {
                firstGuess = new Button();
                firstGuess = (Button)sender;
                try
                {
                    indexFirstGuess = Convert.ToUInt16(firstGuess.Content) - 1;
                }
                catch {
                    firstGuess = null;
                    return;
                }
                  
                if (guessesBoard[indexFirstGuess / cols, indexFirstGuess % cols] == false)
                {
                    if (firstGuess != previousGuess)
                    {
                        allowClick = true;
                        BitmapImage btm = new BitmapImage(new Uri(board[indexFirstGuess / cols, indexFirstGuess % cols], UriKind.Relative));
                        Image image = new Image();
                        image.Stretch = Stretch.Uniform;
                        image.Source = btm;
                        image.Width = firstGuess.ActualWidth;
                        image.Height = firstGuess.ActualHeight;
                        firstGuess.Content = image;

                    }
                    else
                    {
                        allowClick = false;
                        firstGuess = null;
                    }
                }

            }
            else
            {
                    secondGuess = new Button();
                    secondGuess = (Button)sender;
                    previousGuess = secondGuess;
                    try
                    {
                        indexSecondGuess = Convert.ToUInt16(secondGuess.Content) - 1;
                    }
                    catch
                    {
                    secondGuess = null;
                        return;
                    }

                        if (guessesBoard[indexSecondGuess / cols, indexSecondGuess % cols] == false)
                        {
                            if (allowClick == true)
                            {
                                BitmapImage btm = new BitmapImage(new Uri(board[indexSecondGuess / cols, indexSecondGuess % cols], UriKind.Relative));
                                Image image = new Image();
                                image.Stretch = Stretch.Uniform;
                                image.Source = btm;
                                image.Width = secondGuess.ActualWidth;
                                image.Height = secondGuess.ActualHeight;
                                secondGuess.Content = image;
                            }
                        }

            }

            if (firstGuess != null && secondGuess != null)
            {
                if (board[indexFirstGuess / cols, indexFirstGuess % cols] != board[indexSecondGuess / cols, indexSecondGuess % cols] &&
                    guessesBoard[indexFirstGuess / cols, indexFirstGuess % cols] == false && guessesBoard[indexSecondGuess / cols, indexSecondGuess % cols] == false)
                {
                    Button clickedButton = (Button)sender;
                    clickedButton.IsEnabled = false;

                    await Task.Delay(TimeSpan.FromSeconds(0.5));

                    clickedButton.IsEnabled = true;

                    firstGuess.Content = indexFirstGuess + 1;
                    secondGuess.Content = indexSecondGuess + 1;
                }
                else
                {
                    guessesBoard[indexFirstGuess / cols, indexFirstGuess % cols] = true;
                    guessesBoard[indexSecondGuess / cols, indexSecondGuess % cols] = true;
                }

                if (level <= 3)
                {
                    if (CountGuesses() == rows * cols - rows * cols % 2)
                    {
                        level++;
                        if (level == 4)
                        {
                            MessageBox.Show("WON!");
                            currentUser.newWonGame();
                            ChangeStistics();

                            matrix.Children.Clear();
                            currentUserLevel.Content = "";

                            level = 0;
                        }
                        else
                        {
                            MessageBox.Show("New Level Loading ...");
                            Button clickedButton = (Button)sender;
                            clickedButton.IsEnabled = false;

                            await Task.Delay(TimeSpan.FromSeconds(0.5));

                            clickedButton.IsEnabled = true;

                            ChangeStistics();
                            InitializeBoard();
                            InitializeGuessesBoard();
                            CreateBoard();
                        }

                    }
                }

                firstGuess = null;
                secondGuess = null;
            }
        }
    }
}
