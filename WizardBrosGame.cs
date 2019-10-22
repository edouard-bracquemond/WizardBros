using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using WizardBros.Model;
using WizardBros.Controller;
using WizardBros.View;
using System.Collections.Generic;
using System;
using Microsoft.Xna.Framework.Media;

namespace WizardBros
{
    /// <summary>
    /// This is the main type for your game.
    /// </summary>
    public class WizardBrosGame : Game
    {
        List<int> niveaux = new List<int>();
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;
        static public int height { get; private set; }
        static public int width { get; private set; }
        EditorMap editor;
        public static World niveau;
        MainController mController;
        NiveauxController nController;
        NiveauxView nView;
        MainView mView;
        Menu menu;
        EditorMapView viewEditor;
        MouseState oldMouseState; //car les updates sont très rapides
        public static KeyboardState oldKeyboardState;
        EditorMapController controllerEditor;
        public static int level = 0;
        public enum GameState  //aide les controlleurs à gérer le cours du jeu
        {
            Start,
            Niveau,
            Playing,
            Paused,
            Death,
            GameOver,
            MapEditor,
            Exit
        };
        private GameState currentState;

        public WizardBrosGame()
        {
            graphics = new GraphicsDeviceManager(this); 
            height = width = 600;
            graphics.PreferredBackBufferHeight = height;
            graphics.PreferredBackBufferWidth = width;

            this.Window.Title = "Little Gandalf";
            IsMouseVisible = true;
            Content.RootDirectory = "Content";
        }


        protected override void Initialize()
        {
            
            niveaux = MapXml.getListNiveauxName();
            currentState = GameState.Start;
            nView = new NiveauxView(Content);
            nController = new NiveauxController();

            mController = new MainController();
            mView = new MainView(Content);
            menu = new Menu(Content);
            editor = new EditorMap(600, 600);
            viewEditor = new EditorMapView(Content);
            controllerEditor = new EditorMapController();
            base.Initialize();
        }

        protected override void LoadContent()
        {
            spriteBatch = new SpriteBatch(GraphicsDevice);
            mView.LoadContent(niveau); 
            nView.LoadContent();
            viewEditor.LoadContent(editor);
            menu.LoadContent();
        }

        /// <summary>
        /// UnloadContent will be called once per game and is the place to unload
        /// game-specific content.
        /// </summary>
        protected override void UnloadContent()
        {
            // TODO: Unload any non ContentManager content here
        }

        protected override void Update(GameTime gameTime)
        {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape) || currentState==GameState.Exit) //bug chez certains ordis
                Exit();

            // Appel du contrôleur approprié

            if (currentState == GameState.MapEditor)
            {
                currentState = controllerEditor.Update(editor, currentState, ref oldMouseState, ref oldKeyboardState);
            }

            //controleur des menus, par élimination
            else if (currentState != GameState.Playing)
            {
                if (currentState == GameState.Niveau)
                {
                    niveaux = MapXml.getListNiveauxName();
                    currentState = nController.Update(currentState, ref oldKeyboardState, niveaux.Count);
                }

                if (currentState == GameState.Death) //la seule différence avec gameover, en attente de niveaux, est le reset du score
                {
                    niveau.getJoueur().resetHealth();
                    niveau.getJoueur().ResetPos();
                    niveau.resetMap();                    
                }
                
                currentState = menu.Update(currentState, ref oldMouseState, ref oldKeyboardState);
            }
            //controleur jeu
            else
            {
                currentState = mController.Update(niveau, currentState, ref oldMouseState, ref oldKeyboardState, niveau.getJoueur().getPosX(), niveau.getJoueur().getPosY());
            }

            // TODO: Add your update logic here
            base.Update(gameTime);
        }

        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {
            spriteBatch.Begin(); //appel de l'afficheur approprié

            if (currentState == GameState.Playing)
                mView.Draw(spriteBatch, niveau);
            else if (currentState == GameState.MapEditor)
                viewEditor.Draw(spriteBatch, editor);
            else if (currentState == GameState.Niveau)
                nView.Draw(niveaux, spriteBatch);
            else
                menu.Draw(currentState, spriteBatch);

            spriteBatch.End();
            // TODO: Add your drawing code here
            base.Draw(gameTime);

        }



    }
}
