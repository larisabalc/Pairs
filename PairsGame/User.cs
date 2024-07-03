using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace PairsGame
{
   public class User
    {
        private String _name;
        private String _pathToImage;
        private int wonGames;
        private int playedGames;

        public User()
        {
        }

        public String GetName()
        {
            return _name;
        }
        public void SetName(String name)
        {
            _name = name;
        }
        public String GetPathToImage()
        {
            return _pathToImage;
        }
        public void SetPathToImage(String pathToImage)
        {
            _pathToImage = pathToImage;
        }
        public void newWonGame()
        {
            wonGames++;
        }
        public void newPlayedGame()
        {
            playedGames++;
        }
        public int GetWonGame()
        {
            return wonGames;
        }
        public int GetPlayedGame()
        {
            return playedGames;
        }
        public void SetWonGame(int nrGames)
        {
            wonGames = nrGames;
        }
        public void SetPlayedGame(int nrGames)
        {
            playedGames = nrGames;
        }
    }


}
