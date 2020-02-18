using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System.Collections.Generic;
using System.Linq;

namespace MonoGameWindowsStarter
{
    /// <summary>
    /// This is the main type for your game.
    /// </summary>
    public class Game1 : Game
    {
        GraphicsDeviceManager graphics;
        SpriteFont spriteFont;
        SpriteBatch spriteBatch;
        SpriteSheet sheet;
        Player player;
        List<Platform> platforms;
        GhostEnemy ghost1;

        public Game1()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            platforms = new List<Platform>();
        }


        /// <summary>
        /// Allows the game to perform any initialization it needs to before starting to run.
        /// This is where it can query for any required services and load any non-graphic
        /// related content.  Calling base.Initialize will enumerate through any components
        /// and initialize them as well.
        /// </summary>
        protected override void Initialize()
        {
            // TODO: Add your initialization logic here
            graphics.PreferredBackBufferWidth = 1600;
            graphics.PreferredBackBufferHeight = 1000;
            graphics.ApplyChanges();

            base.Initialize();
        }

        /// <summary>
        /// LoadContent will be called once per game and is the place to load
        /// all of your content.
        /// </summary>
        protected override void LoadContent()
        {
            // Create a new SpriteBatch, which can be used to draw textures.
            spriteBatch = new SpriteBatch(GraphicsDevice);

            //load in spritefont
            spriteFont = Content.Load<SpriteFont>("File");

            // TODO: use this.Content to load your game content here
            var t = Content.Load<Texture2D>("spritesheet");
            sheet = new SpriteSheet(t, 21, 21, 1, 2);

            // Create the player with the corresponding frames from the spritesheet
            var playerFrames = from index in Enumerable.Range(139, 150) select sheet[index]; //19 is the 0 in player currentFrame
            player = new Player(playerFrames);

            var platFrames = from index in Enumerable.Range(19, 30) select sheet[index];

            var GhostFrames = from index in Enumerable.Range(445, 449) select sheet[index];
            ghost1 = new GhostEnemy(GhostFrames);
        }

        /// <summary>
        /// UnloadContent will be called once per game and is the place to unload
        /// game-specific content.
        /// </summary>
        protected override void UnloadContent()
        {
            // TODO: Unload any non ContentManager content here
        }

        /// <summary>
        /// Allows the game to run logic such as updating the world,
        /// checking for collisions, gathering input, and playing audio.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Update(GameTime gameTime)
        {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();

            player.Update(gameTime);
            ghost1.Update(gameTime);

            base.Update(gameTime);
        }

        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.Black);

            spriteBatch.Begin();
            player.Draw(spriteBatch);
            ghost1.Draw(spriteBatch);
            var j = 1;

            for (var i = 445; i < 500; i++)
            {
                j++;
                sheet[i].Draw(spriteBatch, new Vector2(j * 25, 100), Color.White);
                
            }

            if (player.gameState == 1)  //if you have won, draw the you win
            {
                //spriteBatch.Draw(YouWin, win, Color.White);
                spriteBatch.DrawString(spriteFont, "You Win! :)", new Vector2(630, 450), Color.Blue);
            }

            if (player.gameState == 2) //if you have lost, draw the you lose
            {
                //spriteBatch.Draw(YouLose, lose, Color.White);
                spriteBatch.DrawString(spriteFont, "You Lose! :(", new Vector2(630, 450), Color.Red);
            }

            spriteBatch.End();
            base.Draw(gameTime);
        }
    }
}
