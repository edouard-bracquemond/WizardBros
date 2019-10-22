using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using WizardBros.Model;
using Microsoft.Xna.Framework.Input;

namespace WizardBros.View
    {
        public class EditorMapView
        {
            private ContentManager content;
            SpriteFont font;


            Texture2D forestTexture;
            Texture2D stoneTexture;
            Rectangle stoneRectangle;
            Rectangle forestRectangle;
            Texture2D spikeTTexture;
            Texture2D spikeBTexture;
            Texture2D spikeLTexture;
            Texture2D spikeRTexture;
            Rectangle enemyRectangle;
            Texture2D enemyTexture;
            Texture2D victoryTexture;

            public EditorMapView(ContentManager content)
            {
                this.content = content;
            }

            public void LoadContent(EditorMap editor)
            {
                forestRectangle = new Rectangle(1, 1, editor.getBackground().getLargeur(), editor.getBackground().getHauteur());
                stoneRectangle = new Rectangle(1, 1, 60, 60);
                enemyRectangle = new Rectangle(1, 1, 120, 60);
                forestTexture = content.Load<Texture2D>("forest");
                stoneTexture = content.Load<Texture2D>("stone");
                spikeTTexture = content.Load<Texture2D>("spikeT");
                spikeBTexture = content.Load<Texture2D>("spikeB");
                spikeLTexture = content.Load<Texture2D>("spikeL");
                spikeRTexture = content.Load<Texture2D>("spikeR");
                enemyTexture = content.Load<Texture2D>("bat");
                victoryTexture = content.Load<Texture2D>("BlocVictory");


            }

            public void Draw(SpriteBatch spriteBatch, EditorMap editor)
        {
            MouseState mstate = Mouse.GetState();
            int x = mstate.X;
            int y = mstate.Y;

            Color couleur;
            spriteBatch.Draw(forestTexture, new Vector2(editor.getBackground().getPosX(), editor.getBackground().getPosY()), forestRectangle, Color.White);
            spriteBatch.Draw(forestTexture, new Vector2(editor.getBackground().getSecondPosX(), editor.getBackground().getPosY()), forestRectangle, Color.White);



            foreach (CollisionObject element in editor.getBlocks())
            {
                if (element.getPosX() < 600 && element.getPosX() > -120)
                {

                    switch (element.type)
                    {

                        case (1):
                            spriteBatch.Draw(stoneTexture, new Vector2(element.getPosX(), element.getPosY()), stoneRectangle, Color.White);
                            break;
                        case (2):
                            spriteBatch.Draw(spikeTTexture, new Vector2(element.getPosX(), element.getPosY()), stoneRectangle, Color.White);
                            break;
                        case (3):
                            spriteBatch.Draw(spikeBTexture, new Vector2(element.getPosX(), element.getPosY()), stoneRectangle, Color.White);
                            break;
                        case (4):
                            spriteBatch.Draw(spikeLTexture, new Vector2(element.getPosX(), element.getPosY()), stoneRectangle, Color.White);
                            break;
                        case (5):
                            spriteBatch.Draw(spikeRTexture, new Vector2(element.getPosX(), element.getPosY()), stoneRectangle, Color.White);
                            break;
                        case (6):
                            spriteBatch.Draw(victoryTexture, new Vector2(element.getPosX(), element.getPosY()), stoneRectangle, Color.White);
                            break;
                        default:
                            spriteBatch.Draw(enemyTexture, new Vector2(element.getPosX(), element.getPosY()), enemyRectangle, Color.White);
                            break;
                    }
                }
            }

            if (editor.droitAdd(editor.previsualiser(x, y)))
            {
                couleur = Color.Gray;
            }
            else
            {
                couleur = Color.White;
            }
            switch (editor.typeSelect)
            {

                case (1):
                    spriteBatch.Draw(stoneTexture, editor.previsualiser(x, y), stoneRectangle, couleur);
                    break;
                case (2):
                    spriteBatch.Draw(spikeTTexture, editor.previsualiser(x, y), stoneRectangle, couleur);
                    break;
                case (3):
                    spriteBatch.Draw(spikeBTexture, editor.previsualiser(x, y), stoneRectangle, couleur);
                    break;
                case (4):
                    spriteBatch.Draw(spikeLTexture, editor.previsualiser(x, y), stoneRectangle, couleur);
                    break;
                case (5):
                    spriteBatch.Draw(spikeRTexture, editor.previsualiser(x, y), stoneRectangle, couleur);
                    break;
                case (6):
                    spriteBatch.Draw(victoryTexture, editor.previsualiser(x, y), stoneRectangle, couleur);
                    break;
                default:
                    spriteBatch.Draw(enemyTexture, editor.previsualiser(x, y), enemyRectangle, couleur);
                    break;
            }
        }
    }
}
