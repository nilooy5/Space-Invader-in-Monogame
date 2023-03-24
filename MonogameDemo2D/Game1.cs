﻿using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using RC_Framework;
using System.IO;

namespace MonogameDemo2D
{
    public class Game1 : Game
    {
        private GraphicsDeviceManager _graphics;
        private SpriteBatch _spriteBatch;
        int gameWindowWidth = 800;
        int gameWindowHeight = 600;
        float bottomLimit = 0;

        float xx = 100;
        float yy = 600/2 - 50/2;

        float defaultXspeed = 6;
        float ssXSpeed = 6;
        float ssYSpeed = 4f;

        Texture2D texBack;
        Texture2D texSpaceShip;
        Texture2D texMountain;
        Texture2D texMissile;

        Sprite3 spaceship = null;
        Sprite3 mountain = null;
        Sprite3 missile = null;

        ImageBackground back1 = null;

        bool showBB = false;

        KeyboardState prevK;
        KeyboardState k;

        string dir = @"C:\Users\fazal_ix0ll8n\source\repos\MonogameDemo2D\MonogameDemo2D\images\";

        public Game1()
        {
            _graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            _graphics.PreferredBackBufferHeight = gameWindowHeight;
            _graphics.PreferredBackBufferWidth = gameWindowWidth;
            IsMouseVisible = true;
        }

        protected override void Initialize()
        {
            base.Initialize();
        }

        protected override void LoadContent()
        {
            _spriteBatch = new SpriteBatch(GraphicsDevice);
            LineBatch.init(GraphicsDevice);

            texBack = Util.texFromFile(GraphicsDevice, dir + "Back1.png");
            texSpaceShip = Util.texFromFile(GraphicsDevice, dir + "Spaceship3a.png");
            texMountain = Util.texFromFile(GraphicsDevice, dir + "Mountain2.png");
            texMissile = Util.texFromFile(GraphicsDevice, dir + "Missile.png");

            back1 = new ImageBackground(texBack, Color.White, GraphicsDevice);

            spaceship = new Sprite3(true, texSpaceShip, xx, yy);
            setupSpaceship(spaceship);

            missile = new Sprite3(true, texMissile, 100, 0); //535x83
            setupMissile(missile);

            mountain = new Sprite3(true, texMountain, 700, 0);
            mountain.setPosY(gameWindowHeight - mountain.getHeight());
        }
        private void setupMissile(Sprite3 missile)
        {
            missile.setWidthHeightOfTex(535, 83);
            missile.setXframes(3);
            missile.setYframes(1);
            missile.setWidthHeight(535 / 3, 83);
            Vector2[] anim = new Vector2[8]; // arrays start at 0 REMEMBER
            anim[0].X = 0; anim[0].Y = 0;
            anim[1].X = 1; anim[1].Y = 0;
            anim[2].X = 2; anim[2].Y = 0;
            missile.setAnimationSequence(anim, 0, 2, 17);
            missile.setAnimFinished(0); // this is the default but - explicit for the tutorial
            missile.setPos(500, 100);
            missile.animationStart();
        }

        protected override void Update(GameTime gameTime)
        {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();

            prevK = k;
            k = Keyboard.GetState();

            handleMovement(k);

            if (k.IsKeyDown(Keys.B) && prevK.IsKeyUp(Keys.B)) showBB = !showBB;

            // updating horizontal speed
            if (k.IsKeyDown(Keys.Left)) ssXSpeed = 3f;
            if (prevK.IsKeyUp(Keys.Left)) ssXSpeed = defaultXspeed;


            // updating obstacles
            if (mountain.getPosX() < -mountain.getWidth())
            {
                mountain.setPosX(gameWindowWidth);
            }
            else mountain.setPosX(mountain.getPosX() - ssXSpeed);
            missile.animationTick(gameTime);

            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);

            _spriteBatch.Begin();

            back1.Draw(_spriteBatch);
            spaceship.Draw(_spriteBatch);
            mountain.Draw(_spriteBatch);
            missile.Draw(_spriteBatch);

            if (showBB)
            {
                renderBoundingBoxes();
            }

            _spriteBatch.End();

            base.Draw(gameTime);
        }

        private void setupSpaceship(Sprite3 spaceship)
        {
            spaceship.setHeight(50);
            spaceship.setWidth(100);
            spaceship.setBBToTexture();
            bottomLimit = gameWindowHeight - spaceship.getHeight();
        }

        private void handleMovement(KeyboardState k)
        {
            if (k.IsKeyDown(Keys.Up))
            {
                if (spaceship.getPosY() >= 0) spaceship.setPosY(spaceship.getPosY() - ssYSpeed);
            }
            if (k.IsKeyDown(Keys.Down))
            {
                if (spaceship.getPosY() < bottomLimit) spaceship.setPosY(spaceship.getPosY() + ssYSpeed);
            }
        }

        private void renderBoundingBoxes()
        {
            spaceship.drawBB(_spriteBatch, Color.Black);
            spaceship.drawHS(_spriteBatch, Color.Green);
            mountain.drawBB(_spriteBatch, Color.Black);
            mountain.drawHS(_spriteBatch, Color.Green);
            missile.drawBB(_spriteBatch, Color.Black);
            missile.drawHS(_spriteBatch, Color.Green);
        }
    }
}