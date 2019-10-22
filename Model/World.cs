using Microsoft.Xna.Framework;
using System.Collections.Generic;


namespace WizardBros.Model
{
    public class World
    {
        Background bg;
        Personnage joueur;
        HUD hud;
        int[] map; //equivalent de classe level
        List<CollisionObject> blocks;
        List<CollisionObject> projectiles;
        List<Enemy> enemies;

        float momentum = 12.0f; //float prend moinds de place
        float gravity = 1.0f;
        int jumpLength = 0;
        int charge = 0;
        public int jumpCharge { get { return charge; } set { charge = value; } }
        public bool spawnlock = true;
        public bool completed { get; private set; }

        public World(int screenW, int screenH, int niveau)
        {
            blocks = MapXml.getNiveau(niveau);
            projectiles = new List<CollisionObject>();
            enemies = new List<Enemy>();
            bg = new Background(1, 1, screenW, screenH);
            joueur = new Personnage((WizardBrosGame.width / 2) - 30, 60, 50, 60, 3);
            hud = new HUD(joueur);

            foreach (CollisionObject element in blocks)
            {
                if (element is Enemy)
                {
                    Enemy en = (Enemy)element;
                    enemies.Add(en);
                    //element.destroy();
                }
            }
            blocks.RemoveAll(x => x is Enemy);
        }

        //getters et reset
        public Personnage getJoueur()
        {
            return joueur;
        }
        public Background getBackground()
        {
            return bg;
        }
        public HUD getHUD()
        {
            return hud;
        }
        public List<CollisionObject> getBlocks()
        {
            return blocks;
        }
        public List<CollisionObject> getProjectiles()
        {
            return projectiles;
        }
        public List<Enemy> getEnemies()
        {
            return enemies;
        }

        public void resetMap()
        {
            foreach (CollisionObject element in blocks)
            {
                element.resetPos();
            }
            spawnlock = true;
            joueur.setPosY(60);
        }


        //déplacement de l'entité personnage

        public void moveRight()
        {
            foreach (CollisionObject element in blocks)
            {
                if (collisionRight(joueur, element))
                {
                    if (element.type == 4)
                        joueur.loseHealth(2);
                    return;
                }
            }

            if (bg.getSecondPosX() == 1)
            {
                bg.setPosX(bg.getLargeur());
            }
            else if (bg.getPosX() == 1)
            {
                bg.setSecondPosX(bg.getLargeur());
            }

            bg.setPosX(bg.getPosX() - 1);
            bg.setSecondPosX(bg.getSecondPosX() - 1);
            foreach (CollisionObject element in blocks)
            {
                element.setPosX(element.getPosX() - 3);
            }
            foreach (Projectile element in projectiles)
            {
                element.setPos(element.getPos() + new Vector2(-3,0));
            }
            foreach (CollisionObject element in enemies)
            {
                element.setPosX(element.getPosX() - 3);
            }
            joueur.posXAbsolue += 3;

        }

        public void moveLeft()
        {
            foreach (CollisionObject element in blocks)
            {
                if (collisionLeft(joueur, element))
                {
                    if (element.type == 5)
                        joueur.loseHealth(2);
                    return;
                }
            }
            if (bg.getSecondPosX() == 1)
            {
                bg.setPosX(-bg.getLargeur());
            }
            else if (bg.getPosX() == 1)
            {
                bg.setSecondPosX(-bg.getLargeur());
            }

            bg.setPosX(bg.getPosX() + 1);
            bg.setSecondPosX(bg.getSecondPosX() + 1);
            foreach (CollisionObject element in blocks)
            {
                element.setPosX(element.getPosX() + 3);
            }
            foreach (Projectile element in projectiles)
            {
                element.setPos(element.getPos() + new Vector2(3, 0));
            }
            foreach (CollisionObject element in enemies)
            {
                element.setPosX(element.getPosX() + 3);
            }
            joueur.posXAbsolue -= 3;
        }

        public void JumpSpeed()
        {
            jumpLength++;
            joueur.setPosY(joueur.getPosY() - (int)momentum); // l'accélération diminue au cours du saut et finit par prendre une valeur négative, ce qui amorce la chute (la soustraction devient une addition sur les X)

            foreach (CollisionObject element in blocks) //le mouvement est annulé si le perso passe en dessous du sol, comme pour la gravité
            {
                if (collisionBottom(joueur, element))
                {
                    joueur.setPosY(element.getPosY() - element.getH()); //réajustement du déplacement vertical en cas de sol
                    joueur.jumping = false;
                }

                if (collisionTop(joueur, element))
                {
                    if (element.type == 3)
                        joueur.loseHealth(20);
                    else if (element.type == 6)
                        completed = true;
                    joueur.setPosY(element.getPosY() + joueur.getH() - 15); //réajustement en cas de collision du dessus
                    joueur.jumping = false;
                }
            }

            if (charge > 20)
            {
                momentum -= 0.10f; //en début d'ascension
                if (jumpLength > 50 && momentum > 0) // pour éviter de trop planer en fin d'ascension
                {
                    momentum -= 0.25f;
                }
                if (momentum < 0)
                {
                    Gravity();
                }
            }
            else if (charge > 10)
            {
                momentum -= 0.25f;
                if (momentum < 0)
                {
                    Gravity();
                }
            }
            else momentum -= 0.5f;     // cas du saut minimal        
        }

        public void Gravity()
        {
            joueur.setPosY(joueur.getPosY() + (int)gravity);
            if (gravity < 6.0f) //accélération de la chute jusqu'à une vitesse plafond
                gravity += 0.25f;

            foreach (CollisionObject element in blocks)
            {
                if (collisionBottom(joueur, element))
                {
                    spawnlock = false;
                    if (element.type == 2)
                        joueur.loseHealth(1);
                    else if (element.type == 6)
                        completed = true;
                        //reset de variables de saut et mouvement annulé
                    joueur.canJump = true; // on ne peut sauter que si on a retouché le sol (cas de la chute libre géré après la boucle)
                    joueur.jumping = false;
                    jumpLength = 0;
                    charge = 0;
                    momentum = 12.0f;
                    gravity = 1.0f;
                    joueur.setPosY(element.getPosY() - element.getH());
                    return;
                }
            }
            joueur.canJump = false; //n'est lue que si le joueur est en chute libre
        }

        //fonctions utilitaires de collision
        public bool collisionLeft(CollisionObject rect1, CollisionObject rect2) // ajouter une double vérification : si collision gauche elle doit venir du bord droit (etc)
        {
            Rectangle rect = rect2.getRectangle();
            Rectangle rectLeft = new Rectangle();
            if (rect1 is Personnage)
            {
                rectLeft = ((Personnage)rect1).getRectangleLeft();
            }
            if (rect1 is Projectile)
            {
                rectLeft = ((Projectile)rect1).getRectangleLeft();
                Rectangle inter = Rectangle.Intersect(rectLeft, rect);
                if (rect2 is Enemy)
                    return (!inter.IsEmpty);
                if (!inter.IsEmpty)
                    return inter.Height >= inter.Width && inter.Left > rect.Left && !(rect2.hasBlockToTheRight(blocks));
            }
            return (rectLeft.Intersects(rect));
        }

        public bool collisionRight(CollisionObject rect1, CollisionObject rect2)
        {
            Rectangle rect = rect2.getRectangle();
            Rectangle rectRight = new Rectangle();
            if (rect1 is Personnage)
            {
                rectRight = ((Personnage)rect1).getRectangleRight();
            }
            if (rect1 is Projectile)
            {
                rectRight = ((Projectile)rect1).getRectangleRight();
                Rectangle inter = Rectangle.Intersect(rectRight, rect);
                if (rect2 is Enemy)
                    return (!inter.IsEmpty);
                if (!inter.IsEmpty)
                    return inter.Height >= inter.Width && inter.Right < rect.Right && !(rect2.hasBlockToTheLeft(blocks));
            }

            return (rectRight.Intersects(rect));
        }

        public bool collisionBottom(CollisionObject rect1, CollisionObject rect2)
        {
            Rectangle rect = rect2.getRectangle();
            Rectangle rectBot = new Rectangle();
            if (rect1 is Personnage)
            {
                rectBot = ((Personnage)rect1).getRectangleBot();
            }
            if (rect1 is Projectile)
            {
                rectBot = ((Projectile)rect1).getRectangleBot();
                Rectangle inter = Rectangle.Intersect(rectBot, rect);
                if (rect2 is Enemy)
                    return (!inter.IsEmpty);
                if (!inter.IsEmpty)
                    return inter.Height <= inter.Width  && inter.Bottom < rect.Bottom && !(rect2.hasBlockOver(blocks));
            }

            return (rectBot.Intersects(rect));
        }

        public bool collisionTop(CollisionObject rect1, CollisionObject rect2)
        {
            Rectangle rect = rect2.getRectangle();
            Rectangle rectTop = new Rectangle();
            if (rect1 is Personnage)
            {
                rectTop = ((Personnage)rect1).getRectangleTop();
            }
            if (rect1 is Projectile)
            {
                rectTop = ((Projectile)rect1).getRectangleTop();
                Rectangle inter = Rectangle.Intersect(rectTop, rect);
                if (rect2 is Enemy)
                    return (!inter.IsEmpty);
                else if (!inter.IsEmpty)
                    return inter.Height <= inter.Width && inter.Top > rect.Top && !(rect2.hasBlockUnder(blocks));
            }

            return (rectTop.Intersects(rect));
        }


        //projectiles et ennemis

        public void fire(int mouseX, int mouseY)
        {
            projectiles.Add(new Projectile(joueur.getPosX() + 40, joueur.getPosY() + 30, 27, 35, new Vector2(mouseX, mouseY), 3, 2));
        }

        public void UpdateWorld()
        {
            foreach (Enemy monster in enemies)
            {
                monster.Behavior(joueur);
            }
            foreach (Projectile element in projectiles)
            {
                element.updatePos();
                foreach (CollisionObject bloc in blocks)
                {
                    if (element.getTimer() > 800)
                        element.destroy();

                    if (collisionBottom(element, bloc) || collisionTop(element, bloc))
                    {
                        if (element.getTimer() > 20)
                            element.bounceY();
                        else
                            element.destroy();
                    }

                    if (collisionLeft(element, bloc) || collisionRight(element, bloc))
                    {
                        if (element.getTimer() > 20)
                            element.bounceX();
                        else
                            element.destroy();
                    }
                }
                foreach (Enemy enemy in enemies)
                {
                    if (element.color == Color.Yellow && (collisionBottom(element, enemy) || collisionTop(element, enemy) || collisionLeft(element, enemy) || collisionRight(element, enemy)))
                    {
                        //test tankiness
                        enemy.destroy();
                        element.destroy();
                        hud.addScore(100);
                    }
                }
            }
            projectiles.RemoveAll(x => x.isdead());
            enemies.RemoveAll(x => x.isdead());
        }
    }
}



