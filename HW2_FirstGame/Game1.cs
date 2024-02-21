using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Schema;

namespace HW2_FirstGame
{
    public enum GameState
    {
        Menu,
        Game,
        GameOver
    }

    public class Game1 : Game
    {
        private GraphicsDeviceManager _graphics;
        private SpriteBatch _spriteBatch;

        private Player player;    // represents the player
        private List<Collectible> collectibles;

        private int currentLevel;
        private double timer;
        private GameState currentGameState;

        private KeyboardState prevKbState;   // checking for single key presses
        private KeyboardState currentKbState;

        private Random rng;   // determining Collectible initial positions
        private Texture2D playerTexture;
        private Texture2D collectiblesTexture;
 
        private int windowWidth;
        private int windowHeight;

        // Font
        private SpriteFont arial15;


        public Game1()
        {
            _graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            IsMouseVisible = true;

            _graphics.PreferredBackBufferWidth = 3000;  // set this value to the desired width of your window
            _graphics.PreferredBackBufferHeight = 3000;   // set this value to the desired height of your window
            _graphics.ApplyChanges();
        }

        protected override void Initialize()
        {
            // TODO: Add your initialization logic here
            windowWidth = _graphics.GraphicsDevice.Viewport.Width;
            windowHeight = _graphics.GraphicsDevice.Viewport.Height;

            rng = new Random();

            collectibles = new List<Collectible>();

            currentGameState = GameState.Menu;

            timer = 10.0;
            currentLevel = 0;

            base.Initialize();
        }

        protected override void LoadContent()
        {
            _spriteBatch = new SpriteBatch(GraphicsDevice);

            // TODO: use this.Content to load your game content here
            playerTexture = Content.Load<Texture2D>("PlayerImageCat");
            collectiblesTexture = Content.Load<Texture2D>("RedFish");
            arial15 = Content.Load<SpriteFont>("File");

            player = new Player(playerTexture, new Rectangle(1000, 1000, playerTexture.Width, playerTexture.Height), windowWidth, windowHeight);
        }

        protected override void Update(GameTime gameTime)
        {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();

            // TODO: Add your update logic here

            // FSM here
            // Get keyboard state
            currentKbState = Keyboard.GetState();

            switch (currentGameState)
            {
                case GameState.Menu:
                    // Check for a single press of the Enter
                    if (SingleKeyPress(Keys.Enter, currentKbState, prevKbState))
                    {
                        ResetGame();
                        currentGameState = GameState.Game;
                    }
                    break;

                case GameState.Game:
                    timer -= gameTime.ElapsedGameTime.TotalSeconds;

                    player.Update(gameTime);

                    for (int i = 0; i < collectibles.Count; i++)
                    {
                        if (collectibles[i].CheckCollision(player))
                        {
                            player.LevelScore += 10; // Reward the player
                            player.TotalScore += 10;
                            collectibles.Remove(collectibles[i]); // Remove the collected items from the collectibles list
                            i--;
                        }
                    }

                    // If time run out of time
                    if (timer <= 0)
                    {
                        player.TotalScore += player.LevelScore;
                        currentGameState = GameState.GameOver;
                    }
                    // If all collectibles been caught
                    else if (collectibles.Count == 0)
                    {
                        NextLevel();
                    }
                    break;

                case GameState.GameOver:
                    if (SingleKeyPress(Keys.Enter, currentKbState, prevKbState))
                    {
                        currentGameState = GameState.Menu;
                    }
                    break;
            }
            
            prevKbState = currentKbState;
            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);

            // TODO: Add your drawing code here
            _spriteBatch.Begin();

            switch (currentGameState)
            {
                case GameState.Menu:
                    string instruction = "Welcome to The Pond!\n" +
                                         "Press ENTER to start the game.\n" +
                                         "Then use arrow keys to help the kitten catch the fish!\n";
                    _spriteBatch.DrawString(
                        arial15,
                        instruction,
                        new Vector2(500, 500),
                        Color.AliceBlue
                         );
                    break;

                case GameState.Game:
                    foreach (Collectible collectible in collectibles)
                    {
                        collectible.Draw(_spriteBatch);
                    }
                    player.Draw(_spriteBatch);

                    string levelText = $"Level {currentLevel}: {player.LevelScore}";
                    string totalScoreText = $"Total Score: {player.TotalScore}";
                    string timeText = $"Time Left: {timer:0.00}";

                    _spriteBatch.DrawString(arial15, levelText, new Vector2(10,10), Color.White);
                    _spriteBatch.DrawString(arial15, totalScoreText, new Vector2(10, 40), Color.White);
                    _spriteBatch.DrawString(arial15, timeText, new Vector2(10, 70), Color.White);

                    break;

                case GameState.GameOver:
                    string gameOverText = "Game Over";
                    string lastLevelText = $"Last Level Reached: {currentLevel}";
                    string finalScoreText = $"Final Score: {player.TotalScore}";
                    string returnText = "Press Enter to return to the Menu";

                    _spriteBatch.DrawString(arial15, gameOverText, new Vector2(10, 10), Color.White);
                    _spriteBatch.DrawString(arial15, lastLevelText, new Vector2(10, 30), Color.White);
                    _spriteBatch.DrawString(arial15, finalScoreText, new Vector2(10, 50), Color.White);
                    _spriteBatch.DrawString(arial15, returnText, new Vector2(10, 70), Color.White);
                    break;

            }

            _spriteBatch.End();

            base.Draw(gameTime);
        }

        private void NextLevel()
        {
            // Increment the current level by one
            currentLevel++;
            // Add 1s every level
            timer += 1;

            // Update the player’s level score back to 0
            player.LevelScore = 0;
            // re-center the player in the game window
            player.Center();

            // Clear the list of collectibles
            collectibles.Clear();

            // Calculate the number of collectibles the level should contain
            int numberOfCollectibles = 5 + (currentLevel - 1) * 2;

            rng = new Random();
            for (int i = 0; i < numberOfCollectibles; i++)
            {
                // Random position and always fit in the screen
                int x = rng.Next(0, windowWidth - collectiblesTexture.Width);
                int y = rng.Next(0, windowHeight - collectiblesTexture.Height);

                // Instantiate every collectible
                Collectible newCollectible = 
                    new Collectible(
                        collectiblesTexture,
                        new Rectangle 
                        (x, y, 
                        collectiblesTexture.Width, 
                        collectiblesTexture.Height)
                    );

                // Add each to the list
                collectibles.Add(newCollectible);


            }
        }

        /// <summary>
        /// Set up the initial values for the game.
        /// </summary>
        private void ResetGame()
        {
            // Set the current level to zero
            currentLevel = 0;

            // Reset total score
            player.TotalScore = 0;
            timer = 10.0;

            NextLevel();
        }

        /// <summary>
        /// Check wheather this is the first frame that key was pressed.
        /// </summary>
        /// <param name="key">the key to check</param>
        /// <returns>Wheather this is the first frame that key was pressed</returns>
        private bool SingleKeyPress(Keys key, KeyboardState currentState, KeyboardState previousState)
        {
            return 
                currentState.IsKeyDown(key) && previousState.IsKeyUp(key);
        }
    }
}
