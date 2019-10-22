using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System;
using WizardBros.Model;


namespace WizardBros.View
{
    public class MainView
    {
        private ContentManager content;
        SpriteFont font;
    
        Texture2D playerTexture;
        Texture2D forestTexture;
        Texture2D stoneTexture;
        Texture2D spikeTTexture;
        Texture2D spikeBTexture;
        Texture2D spikeLTexture;
        Texture2D spikeRTexture;
        Texture2D victoryTexture;

        Texture2D fullheart;
        Texture2D halfheart;

        Texture2D proj1;
        Texture2D bat;

        Rectangle playerRectangle;
        Rectangle stoneRectangle;
        Rectangle forestRectangle;
        Rectangle batRectangle;


        public MainView(ContentManager content)
        {
            this.content = content;
        }

        public void LoadContent(World niveau)
        {
            forestTexture = content.Load<Texture2D>("forest");
            playerTexture = content.Load<Texture2D>("heros");
            stoneTexture = content.Load<Texture2D>("stone");
            spikeTTexture = content.Load<Texture2D>("spikeT");
            spikeBTexture = content.Load<Texture2D>("spikeB");
            spikeLTexture = content.Load<Texture2D>("spikeL");
            spikeRTexture = content.Load<Texture2D>("spikeR");
            victoryTexture = content.Load<Texture2D>("BlocVictory");
            proj1 = content.Load<Texture2D>("proj1");
            bat = content.Load<Texture2D>("bat");

            fullheart = content.Load<Texture2D>("fullheart");
            halfheart = content.Load<Texture2D>("halfheart");

            font = content.Load<SpriteFont>("arial");
        }

        public void Draw(SpriteBatch spriteBatch, World world)
        {
            playerRectangle = new Rectangle(1, 4, world.getJoueur().getH(), world.getJoueur().getW()); // vous avez inversé length et width on doit faire avec maintenant
            forestRectangle = new Rectangle(1, 1, world.getBackground().getLargeur(), world.getBackground().getHauteur());
            stoneRectangle = new Rectangle(1, 1, 60, 60);
            batRectangle = new Rectangle(1, 1, 120, 39);

            //background et joueur
            spriteBatch.Draw(forestTexture, new Vector2(world.getBackground().getPosX(), world.getBackground().getPosY()), forestRectangle, Color.White);
            spriteBatch.Draw(forestTexture, new Vector2(world.getBackground().getSecondPosX(), world.getBackground().getPosY()), forestRectangle, Color.White);

            spriteBatch.Draw(playerTexture, new Vector2(world.getJoueur().getPosX(), world.getJoueur().getPosY()), new Rectangle(1+50*world.getJoueur().getFrame(), 4, world.getJoueur().getH(), world.getJoueur().getW()),  Color.White);

            //blocs, ennemis, sorts
            foreach (CollisionObject element in world.getBlocks())
            {
                if (element.getPosX() < 600 && element.getPosX() > -120)
                {
                    if (element.type == 1)
                        spriteBatch.Draw(stoneTexture, new Vector2(element.getPosX(), element.getPosY()), stoneRectangle, Color.White);
                    if (element.type == 2)
                        spriteBatch.Draw(spikeTTexture, new Vector2(element.getPosX(), element.getPosY()), stoneRectangle, Color.White);
                    if (element.type == 3)
                        spriteBatch.Draw(spikeBTexture, new Vector2(element.getPosX(), element.getPosY()), stoneRectangle, Color.White);
                    if (element.type == 4)
                        spriteBatch.Draw(spikeLTexture, new Vector2(element.getPosX(), element.getPosY()), stoneRectangle, Color.White);
                    if (element.type == 5)
                        spriteBatch.Draw(spikeRTexture, new Vector2(element.getPosX(), element.getPosY()), stoneRectangle, Color.White);
                    if (element.type == 6)
                        spriteBatch.Draw(victoryTexture, new Vector2(element.getPosX(), element.getPosY()), stoneRectangle, Color.White);
                    if (element.type > 10)
                        spriteBatch.Draw(bat, new Vector2(element.getPosX(), element.getPosY()), batRectangle, Color.White);
                }
            }

            foreach (Projectile element in world.getProjectiles())
            {
                spriteBatch.Draw(proj1, element.getPos(), new Rectangle(0, 0, 35, 27), element.getColor(), element.Angle, new Vector2(0, 14), 1.0f, SpriteEffects.None, 1.0f);
            }

            foreach (Enemy element in world.getEnemies())
            {
                if (element.getType() == 17)
                {
                    spriteBatch.DrawString(font, element.getType().ToString(), new Vector2(550, 200), Color.White);
                    spriteBatch.Draw(bat, new Vector2(element.getPosX(), element.getPosY()), batRectangle, Color.White);
                }
            }

            //hud
            int heartcursor = 70;
            for (int i = 0; i < Math.Truncate(Math.Round(world.getHUD().Hearts * 2) / 2); i++) //arrondis à 0.5
            {
                spriteBatch.Draw(fullheart, new Vector2(heartcursor, 40), Color.White);
                heartcursor += 22;
            }
            if (Math.Round(world.getHUD().Hearts * 2) % 2 == 1 || heartcursor == 70)  //s'il faut modéliser un demi coeur (coeurs*2 impair)
            {
                spriteBatch.Draw(halfheart, new Vector2(heartcursor, 40), Color.White);
            }
            spriteBatch.DrawString(font, "Vies : " + world.getJoueur().getLives().ToString(), new Vector2(520, 10), Color.White);
            spriteBatch.DrawString(font, world.getHUD().Score.ToString(), new Vector2(550, 30), Color.White);
        }
    }
}