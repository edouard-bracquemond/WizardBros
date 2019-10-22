using Microsoft.Xna.Framework;

namespace WizardBros.Model
{
    public class Enemy : CollisionObject
    {
        int strength;
        int tanking;
        int typeEnemy;
        int compteur = 0;
        int delay = 0;

        static string[] nameTable = {"placeholder", "bat" };
        static int[,] stats = { {0,0,0 }, {7,20,1} };

        public Enemy (int x, int y, int h, int w, int type, int str, int tank) : base(x, y, h, w, type+10)
        {
            strength = str;
            tanking = tank;
            typeEnemy = type;
        }

        public int getType()
        {
            return typeEnemy;
        }

        public override Rectangle getRectangle()
        {
            if (typeEnemy == 17)
                return new Rectangle(posX, posY, w, (2/3)*h);
            return base.getRectangle();
        }

        public void Behavior(Personnage joueur)
        {
            if (typeEnemy == 17)
            {
                if (compteur<50)
                    compteur++;
                if (compteur == 50)
                    compteur = -50;
                if (compteur >= 0)
                    posX = posX - 3;
                else
                    posX = posX + 3;
            }

            if (joueur.getRectangle().Intersects(this.getRectangle()) && delay == 0) {
                joueur.loseHealth(strength);
                delay = 20;
            }

            if (delay > 0)
                delay--;
        }

    }
}
