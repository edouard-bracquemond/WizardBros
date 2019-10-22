using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;
using System;
using System.Collections.Generic;
using WizardBros.Model;
using static WizardBros.WizardBrosGame;


namespace WizardBros.Controller
{
    public class NiveauxController
    {

        List<Rectangle> listePo = new List<Rectangle>();
        public static int page = 0;


        public GameState Update(GameState currentState, ref KeyboardState oldKeyboardState, int nbNiveaux)
        {
            KeyboardState kbstate = Keyboard.GetState();

            if (kbstate.IsKeyDown(Keys.Back) || kbstate.IsKeyDown(Keys.Escape))
            {
                currentState = GameState.Start;
            }
            else if (kbstate.IsKeyDown(Keys.Down) && oldKeyboardState.IsKeyUp(Keys.Down))
            {
                if (WizardBrosGame.level + 1 < nbNiveaux)
                {
                    WizardBrosGame.level++;


                }
            }
            else if (kbstate.IsKeyDown(Keys.Up) && oldKeyboardState.IsKeyUp(Keys.Up))
            {
                if (WizardBrosGame.level - 1 >= 0)
                {

                    WizardBrosGame.level--;
                }
            }
            else if (kbstate.IsKeyDown(Keys.X))
            {
                WizardBrosGame.niveau = new World(600, 600, level);
                currentState = GameState.Playing;
            }

            oldKeyboardState = kbstate;
            return currentState;
        }

    }
}
