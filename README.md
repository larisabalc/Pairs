# 🧠 Memory Tiles (Pairs) Game

## Overview
This C# and WPF (Windows Presentation Foundation) project implements the classic Pairs (Memory Tiles) game. Users can sign in, create new accounts, and play the memory game across multiple levels. The game includes features such as saving and loading game progress, viewing statistics, and deleting user accounts.

## Features
### 1. **User Authentication**
   - **Sign In:** A sign-in page where users can log in with an existing account or create a new one.
   - **User Creation:** Users can create new accounts and associate them with an avatar image.
   - **User Management:** Users can delete their accounts, which removes all associated data, including saved games and statistics.

### 2. **Gameplay**
   - **New Game:** Starts a new game with a random configuration of memory tiles.
   - **Tile Matching:** Players click on tiles to reveal images. Matching pairs are removed from the board, while non-matching pairs are flipped back.
   - **Levels:** The game consists of three levels. Winning all three levels consecutively counts as a victory.
   - **Customizable Board:** Players can choose between a standard 5x5 board or a custom-sized board (MxN).

### 3. **Saving and Loading Games**
   - **Save Game:** Players can save their current game state (tile configuration and level).
   - **Open Game:** Players can load previously saved games to continue playing from where they left off.

### 4. **Statistics**
   - **Game Stats:** Tracks games played and games won for each user.
   - **Statistics Display:** Shows statistics in a new window. Users can view either their own statistics or the statistics of all users.

## Technologies
- **C# and WPF:** Core technologies used for developing the application.
- **File Handling:** Saves and loads game data, user data, and statistics using convenient file formats (XML).
