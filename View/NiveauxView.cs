using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WizardBros.Controller;
using WizardBros.Model;
using static WizardBros.WizardBrosGame;

namespace WizardBros.View
{
    public class NiveauxView
    {
        ContentManager content;
        SpriteFont font;
        Texture2D bgmenu;
        
        public static bool[] completed;

        public NiveauxView(ContentManager content)
        {
            this.content = content;
            completed = new bool[100];
        }

        public void LoadContent()
        {
            bgmenu = content.Load<Texture2D>("pixelartdemon");
            font = content.Load<SpriteFont>("arial");
        }

        public void Draw(List<int> niveaux, SpriteBatch spriteBatch)
        {


            spriteBatch.Draw(bgmenu, new Rectangle(0, 0, width, height), Color.White);
            int decalage = 300;
            //spriteBatch.DrawString(font, WizardBrosGame.level.ToString(), new Vector2(250, 500), Color.White);
            if (level >= 0 && level < niveaux.Count)
            {
                if (niveaux[level] < NiveauxController.page * 5)
                {
                    NiveauxController.page = NiveauxController.page - 1;

                }
                else if (niveaux[level] >= NiveauxController.page * 5 + 5)
                {
                    NiveauxController.page = NiveauxController.page + 1;
                }
                else if (niveaux[level] >= NiveauxController.page * 5 && niveaux[level] < NiveauxController.page * 5 + 5)
                {
                    for (int y = NiveauxController.page * 5; y < NiveauxController.page * 5 + 5; y++)
                    {
                        if (y < niveaux.Count)
                        {
                            if (niveaux[y] == WizardBrosGame.level)
                            {

                                    spriteBatch.DrawString(font, "Niveau " + niveaux[y].ToString(), new Vector2(250, decalage), Color.Red);
                            }
                            else
                            {
                                if (completed[y])
                                    spriteBatch.DrawString(font, "Niveau " + niveaux[y].ToString(), new Vector2(250, decalage), Color.Green);
                                else
                                {
                                    spriteBatch.DrawString(font, "Niveau " + niveaux[y].ToString(), new Vector2(250, decalage), Color.White);
                                }
                            }
                            decalage = decalage + 60;
                        }


                    }
                }

            }

        }
    }
}
