using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static WizardBros.WizardBrosGame;

//TODO : Options, bouton option, éventuellement Confirm,  hover

namespace WizardBros.Controller
{
    public class Menu
    {
        ContentManager content;
        SpriteFont font;
        Texture2D bgmenu;
        Texture2D start;
        Texture2D exit;
        Texture2D continuer;
        Texture2D menu;
        Texture2D newgame;
        Texture2D editor;
        Song menutrack;

        int timeroption;
        bool music = true;

        public Vector2 startPosition = new Vector2((width / 2) - 50, (height / 2) + 50);
        public Vector2 exitPosition = new Vector2((width / 2) - 50, (height / 2) + 100);
        public Vector2 mapEditorPosition = new Vector2((width / 2) - 50, (height / 2) + 200);

        public enum MenuType
        {
            Main,
            Options,
        }
        MenuType type; //gère les types de menu, indépendamment du gamestate 

        public Menu(ContentManager content)
        {
            this.content = content;
            type = 0;
        }

        public void LoadContent()
        {
            bgmenu = content.Load<Texture2D>("pixelartdemon");
            start = content.Load<Texture2D>("start");
            exit = content.Load<Texture2D>("exit");
            continuer = content.Load<Texture2D>("continue");
            menu = content.Load<Texture2D>("menu");
            newgame = content.Load<Texture2D>("newgame");
            editor = content.Load<Texture2D>("editor");

            font = content.Load<SpriteFont>("arial");
            menutrack = content.Load<Song>("LTdubstep");
            MediaPlayer.Play(menutrack);
            MediaPlayer.Volume = 0.8f;
        }

        public GameState Update(GameState currentState, ref MouseState oldMouseState, ref KeyboardState oldKeyboardState) 
        {
            KeyboardState kbstate = Keyboard.GetState();
            MouseState mstate = Mouse.GetState();
            if (currentState == GameState.Paused || currentState == GameState.Death) //la musique en question sera présente en dehors de la sélection de niveau et du jeu
                MediaPlayer.Resume();  

            if (oldMouseState.LeftButton == ButtonState.Released && mstate.LeftButton == ButtonState.Pressed) //gestion du placement des clics et réactions
            {
                currentState = MouseClick(mstate.X, mstate.Y, currentState);
                type = MenuType.Main;
            }

            if (currentState == GameState.Paused && oldKeyboardState.IsKeyUp(Keys.P) && kbstate.IsKeyDown(Keys.P)) //touche p = pause toggle
            {
                MediaPlayer.Pause();
                type = MenuType.Main;
                currentState = GameState.Playing;
            }

            timeroption++;
            if ((kbstate.IsKeyDown(Keys.O) || type==MenuType.Options) && timeroption%5==0)  //touche O = option toggle
            {
                currentState=Options(kbstate, currentState);
            }


            oldMouseState = mstate; //variable pour connaitre l'état de la souris au dernier update et n'enregistrer qu'un clic
            oldKeyboardState = kbstate;
 
            return currentState;
        }

        GameState MouseClick(int x, int y, GameState currentState) 
        {
            //rectangle étroit dans la zone du clic (x,y)
            Rectangle mouseClickRect = new Rectangle(x, y, 10, 10);
            Rectangle startButtonRect = new Rectangle((int)startPosition.X, (int)startPosition.Y, 100, 20); // les boutons 'continue', 'new game' sont au même endroit sur l'écran
            Rectangle exitButtonRect = new Rectangle((int)exitPosition.X, (int)exitPosition.Y, 100, 20);
            Rectangle editorButtonRect = new Rectangle((int)mapEditorPosition.X, (int)mapEditorPosition.Y, 100, 20);

            if (mouseClickRect.Intersects(startButtonRect))
            {
                MediaPlayer.Pause();
                if (currentState == GameState.GameOver || currentState == GameState.Start)
                    return GameState.Niveau;
                else
                {
                    type = MenuType.Main;
                    return GameState.Playing;
                }
            }
            else if (mouseClickRect.Intersects(exitButtonRect))
            {
                if (currentState != GameState.Start)
                {
                    MediaPlayer.Resume();
                    return GameState.Start;
                }
                else return GameState.Exit;
            } 
            else if(mouseClickRect.Intersects(editorButtonRect) && currentState == GameState.Start)
            {
                return GameState.MapEditor;
            }
            else return currentState;
        }

        public GameState Options(KeyboardState oldKeyboardState, GameState currentState)
        {
            type = MenuType.Options;
            KeyboardState kbstate = Keyboard.GetState();

            if (kbstate.IsKeyDown(Keys.K))
            {
                MainController.mouse = !(MainController.mouse);
            }
            if (kbstate.IsKeyDown(Keys.M))
            {
                if (music)
                    MediaPlayer.Stop();
                else
                    MediaPlayer.Play(menutrack); //gérer gametrack
                music = !music;
            }
            if (oldKeyboardState.IsKeyDown(Keys.Back))
            {
                type = MenuType.Main;
            }

            return currentState;
        }

        public void Draw(GameState currentState, SpriteBatch spriteBatch)
        {
            if (type == MenuType.Main)
            {
                if (currentState == GameState.Start)
                {
                    spriteBatch.Draw(bgmenu, new Rectangle(0, 0, width, height), Color.White);
                    spriteBatch.Draw(start, startPosition, Color.White);
                    spriteBatch.Draw(exit, exitPosition, Color.White);
                    spriteBatch.Draw(editor, mapEditorPosition, Color.White);
                    spriteBatch.DrawString(font, "Press O for options", new Vector2(200, 540), Color.White);
                }

                if (currentState == GameState.Paused)
                {
                    spriteBatch.Draw(bgmenu, new Rectangle(0, 0, width, height), Color.White);
                    spriteBatch.Draw(continuer, startPosition, Color.White);
                    spriteBatch.Draw(menu, exitPosition, Color.White);  // restera les confirm, bouton option
                    spriteBatch.DrawString(font, "Press O for options", new Vector2(200, 540), Color.White);
                }
                if (currentState == GameState.Death)
                {
                    spriteBatch.Draw(bgmenu, new Rectangle(0, 0, width, height), Color.White);
                    spriteBatch.Draw(continuer, startPosition, Color.White);
                    spriteBatch.Draw(menu, exitPosition, Color.White);
                    spriteBatch.DrawString(font, "I'm waiting...", new Vector2(235, 540), Color.White);
                }
                if (currentState == GameState.GameOver)
                {
                    spriteBatch.Draw(bgmenu, new Rectangle(0, 0, width, height), Color.White);
                    spriteBatch.Draw(newgame, startPosition, Color.White);
                    spriteBatch.Draw(menu, exitPosition, Color.White);
                    spriteBatch.DrawString(font, "?", new Vector2(300, 540), Color.White);
                }
            }
            if (type == MenuType.Options)
            {
                spriteBatch.Draw(bgmenu, new Rectangle(0, 0, width, height), Color.White);
                spriteBatch.Draw(continuer, startPosition, Color.White);
                spriteBatch.Draw(menu, exitPosition, Color.White);  
                if (MainController.mouse)
                    spriteBatch.DrawString(font, "Mode souris (Press K) : on" , new Vector2(145, 540), Color.White);
                else
                    spriteBatch.DrawString(font, " Mode souris (Press K) : off", new Vector2(145, 540), Color.White);
                if(music)
                    spriteBatch.DrawString(font, " Musique  (Press M) : on", new Vector2(145, 500), Color.White);
                else
                    spriteBatch.DrawString(font, " Musique (Press M) : off", new Vector2(145, 500), Color.White);
            }
        }
    }
}
