using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WizardBros.Model
{
    public class HUD // to do : lives
    {
        double baseHearts;
        double hearts;
        int score;
        public double Hearts { get { return hearts; } set { hearts = value; } }
        public int Score { get { return score; } }

        Personnage joueur;

        public HUD(Personnage joueur)
        {
            this.joueur = joueur;
            baseHearts = 5;
            hearts = joueur.getHealth() / 20;
            score = 0;
        }


        public void UpdateHearts()
        {
            hearts = joueur.getHealth() / 20.0;          
        }

        public void addScore(int nb)
        {
            score += nb;
        }
    }
}
