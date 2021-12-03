using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace BattleShit3._0
{
    class Bot
    {
        public int[,] myMap = new int[Form1.mapSize, Form1.mapSize]; // bot's map
        public int[,] enemyMap = new int[Form1.mapSize, Form1.mapSize]; // player's map
        public Button[,] myButtons = new Button[Form1.mapSize, Form1.mapSize];
        public Button[,] enemyButtons = new Button[Form1.mapSize, Form1.mapSize];
        public Dictionary<int, int> deckShipsBot = new Dictionary<int, int>();
        public int allCountShips = 20;
        public List<Ship> shipsBot = new List<Ship>();
        public List<Ship> shipsPlayer = new List<Ship>();
        public Animation animation = new Animation();
        public Bot(int[,] myMap, int[,] enemyMap, Button[,] myButtons, Button[,] enemyButtons, List<Ship> shipsPlayer)
        {
            this.myMap = myMap;
            this.enemyMap = enemyMap;
            this.myButtons = myButtons;
            this.enemyButtons = enemyButtons;
            this.shipsPlayer = shipsPlayer;
            deckShipsBot.Add(4, 1);
            deckShipsBot.Add(1, 4);
            deckShipsBot.Add(3, 2);
            deckShipsBot.Add(2, 3);
        }

        public int[,] ConfigureBotsShips()
        {
            Random random = new Random();
            int posX = 0;
            int posY = 0;
            int[] deck1 = new int[] { 4, 3, 2, 1 };
            int posInMap = 0;
            for (int i = 0; i < 4; i++)
            {
                while (deckShipsBot[deck1[i]] > 0)
                {
                    posInMap = random.Next(0, 10);
                    bool check = true;
                    int k = 0;
                    posX = random.Next(1, Form1.mapSize);
                    posY = random.Next(1, Form1.mapSize);
                    Ship ship = new Ship();
                    if (posInMap <= 5) // вертикально
                    {
                        if (posY + deck1[i] <= Form1.mapSize)
                        {
                            for (int cur1 = posX - 1; cur1 <= posX + 1; cur1++)
                            {
                                if (cur1 >= Form1.mapSize)
                                    break;
                                for (int cur2 = posY - 1; cur2 <= posY + deck1[i]; cur2++)
                                {
                                    if (cur2 >= Form1.mapSize)
                                        break;
                                    if (myMap[cur1, cur2] == 1)
                                    {
                                        check = false;
                                        break;
                                    }
                                }
                                if (!check)
                                    break;
                            }

                            if (check == true)
                            {

                                while (k < deck1[i])
                                {
                                    MyPoint point = new MyPoint();
                                    point.point.X = posX;
                                    point.point.Y = posY + k;
                                    ship.AddPoint(point);
                                    myButtons[posX, posY + k].BackColor = Color.Red;
                                    myMap[posX, posY + k] = 1;
                                    k++;
                                }
                                deckShipsBot[deck1[i]]--;
                                shipsBot.Add(ship);
                            }
                        }
                    }
                    if (posInMap > 5) // горизонтально
                    {
                        if (posX + deck1[i] <= Form1.mapSize)
                        {
                            for (int cur1 = posX - 1; cur1 <= posX + deck1[i]; cur1++)
                            {
                                if (cur1 >= Form1.mapSize)
                                    break;
                                for (int cur2 = posY - 1; cur2 <= posY + 1; cur2++)
                                {
                                    if (cur2 >= Form1.mapSize)
                                        break;
                                    if (myMap[cur1, cur2] == 1)
                                    {
                                        check = false;
                                        break;
                                    }
                                }
                                if (!check)
                                    break;
                            }
                            if (check == true)
                            {
                                while (k < deck1[i])
                                {
                                    MyPoint point = new MyPoint();
                                    point.point.X = posX + k;
                                    point.point.Y = posY;
                                    myButtons[posX + k, posY].BackColor = Color.Red;
                                    myMap[posX + k, posY] = 1;
                                    k++;
                                    ship.AddPoint(point);
                                }
                                deckShipsBot[deck1[i]]--;
                                shipsBot.Add(ship);
                            }
                        }
                    }
                }
            }

            return myMap;
        }
        public void Painting(Ship ship)
        {
            for (int i = ship.ship[0].point.X - 1; i <= ship.ship[ship.ship.Count() - 1].point.X + 1; i++)
            {
                for (int j = ship.ship[0].point.Y - 1; j <= ship.ship[ship.ship.Count() - 1].point.Y + 1; j++)
                {
                    if (i < Form1.mapSize && j < Form1.mapSize && i != 0 && j != 0)
                    {
                        if (enemyMap[i, j] != -2)
                        {
                            enemyButtons[i, j].BackColor = Color.Blue;
                            enemyButtons[i, j].Enabled = false;

                        }
                    }
                }
            }
        }
        public bool Shoot()
        {
            bool hit = false;
            Ship ship = new Ship();
            Random random = new Random();
            int posX = 0;
            int posY = 0;
            posX = random.Next(1, Form1.mapSize);
            posY = random.Next(1, Form1.mapSize);
            while (true)
            {
                if (enemyButtons[posX, posY].BackColor == Color.Blue || enemyMap[posX, posY] == -2)
                {
                    posX = random.Next(1, Form1.mapSize);
                    posY = random.Next(1, Form1.mapSize);
                }
                else break;
            }
            if (enemyMap[posX, posY] == 1)
            {
                //enemyButtons[posX, posY].BackColor = Color.Green;
                //enemyButtons[posX, posY].Text = "X";
                animation.Test(enemyButtons[posX, posY]);
                enemyMap[posX, posY] = -2;
                //enemyButtons[posX, posY].Enabled = false;
                hit = true;
                foreach (Ship shipS in shipsPlayer)
                {
                    if (shipS.GetHit(posX, posY))
                        ship = shipS;
                }
                ship.CheckLife();
                if (!ship.GetIsAlive())
                {
                    Painting(ship);
                }
                allCountShips--;
            }
            else
            {
                hit = false;
                enemyButtons[posX, posY].BackColor = Color.Blue;
            }
            if (hit == true && allCountShips != 0)
            {
                ShootModern(ship);
            }
            if (allCountShips == 0)
            {
                MessageBox.Show("Победа Бота!");
            }
            return hit;
        }
        public void ShootModern(Ship ship)
        {
            ship.CheckLife();
            if (!ship.GetIsAlive())
            {
                Painting(ship);
                Shoot();
            }
            else
            {
                Random random = new Random();
                double rd = random.Next(0, 1);
                if (rd <= 0.25)
                {
                    foreach (MyPoint point in ship.ship)
                    {
                        if (point.GetIsAlive())
                        {
                            //enemyButtons[point.point.X, point.point.Y].BackColor = Color.Green;
                            //enemyButtons[point.point.X, point.point.Y].Text = "X";
                            animation.Test(enemyButtons[point.point.X, point.point.Y]);
                            enemyMap[point.point.X, point.point.Y] = -2;
                            //enemyButtons[point.point.X, point.point.Y].Enabled = false;
                            point.SetIsAlive(false);
                            allCountShips--;
                            if (allCountShips == 0)
                            {
                                MessageBox.Show("Bot win!");
                            }
                            break;
                        }
                    }
                    if (allCountShips == 0)
                    {
                        MessageBox.Show("Bot win!");
                    }
                    ShootModern(ship);
                }
                else
                {
                    Shoot();
                }
            }
        }
    }
}
