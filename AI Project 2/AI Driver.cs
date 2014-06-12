using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;
using Microsoft.Xna.Framework.Net;
using Microsoft.Xna.Framework.Storage;

namespace AI_Project_2
{
    /// <summary>
    /// This is the main type for your game
    /// </summary>
    public class Game1 : Microsoft.Xna.Framework.Game
    {
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;
        World world;
        Texture2D agentSprite, nodeSprite, wallSprite, uiSprite, buttonSprite;
        mouseHandler mousehandler;
        SpriteFont font, displayFont;
        KeyboardState state, prevState;

        public Game1()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
        }

        /// <summary>
        /// Allows the game to perform any initialization it needs to before starting to run.
        /// This is where it can query for any required services and load any non-graphic
        /// related content.  Calling base.Initialize will enumerate through any components
        /// and initialize them as well.
        /// </summary>
        protected override void Initialize()
        {
            world = new World();
            graphics.PreferredBackBufferWidth = 800;
            graphics.PreferredBackBufferHeight = 600;
            graphics.IsFullScreen = false;
            graphics.ApplyChanges();
            Window.Title = "Pathfinding Testing";
            base.Initialize();
            
        }

        /// <summary>
        /// LoadContent will be called once per game and is the place to load
        /// all of your content.
        /// </summary>
        protected override void LoadContent()
        {
            // Create a new SpriteBatch, which can be used to draw textures.
            mousehandler = new mouseHandler(Content.Load<Texture2D>("Art\\World\\cursor"));
            spriteBatch = new SpriteBatch(GraphicsDevice);
            agentSprite = Content.Load<Texture2D>("Art\\Agent\\agent");
            nodeSprite = Content.Load<Texture2D>("Art\\Node\\stationaryAgent");
            wallSprite = Content.Load<Texture2D>("Art\\World\\wall");
            uiSprite = Content.Load<Texture2D>("Art\\World\\vPane");
            font = Content.Load<SpriteFont>("Art\\World\\consoleFont");
            displayFont = Content.Load<SpriteFont>("Art\\World\\displayFont");
            buttonSprite = Content.Load<Texture2D>("Art\\World\\button");
            
            world.initializeWorld(agentSprite, nodeSprite, wallSprite, uiSprite, buttonSprite, font, displayFont, ref mousehandler);

            

            // TODO: use this.Content to load your game content here
        }

        /// <summary>
        /// UnloadContent will be called once per game and is the place to unload
        /// all content.
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
            // Allows the game to exit
            prevState = state;
            state = Keyboard.GetState();

            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed)
                this.Exit();
            if (state.IsKeyDown(Keys.Down))
            {
                world.activeAgent.MoveAgent(-1);

            }
            if (state.IsKeyDown(Keys.Up))
            {
                world.activeAgent.MoveAgent(1);

            }
            if (state.IsKeyDown(Keys.Right))
                world.activeAgent.rotateAgent(1);
            if (state.IsKeyDown(Keys.Left))
                world.activeAgent.rotateAgent(-1);
            if (state.IsKeyDown(Keys.OemTilde) && prevState.IsKeyUp(Keys.OemTilde))
                world.showDebug = !world.showDebug;
            if (state.IsKeyDown(Keys.Tab) && prevState.IsKeyUp(Keys.Tab))
                world.showUI = !world.showUI;
            if (state.IsKeyDown(Keys.Space) && prevState.IsKeyUp(Keys.Space))
                world.pauseMove = false;
            if (state.IsKeyDown(Keys.Home) && prevState.IsKeyUp(Keys.Home))
                world.reinitAgent();
            world.updateWorld();
            base.Update(gameTime);
        }

        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.White);

            spriteBatch.Begin();
            world.Draw(spriteBatch);
            spriteBatch.End();
            base.Draw(gameTime);
            
        }
    }
}
