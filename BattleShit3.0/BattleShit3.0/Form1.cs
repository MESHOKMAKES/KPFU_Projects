using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace BattleShit3._0
{
    public partial class Form1 : Form
    {
        public const int mapSize = 11;
        public const int cellSize = 30;
        public int[,] myMap = new int[mapSize, mapSize];
        public int[,] enemyMap = new int[mapSize, mapSize];
        public string alphabet_Rus = "АБВГДЕЖЗИК";
        public bool startGame = false;
        public Button[,] myButtons = new Button[mapSize, mapSize];
        public Button[,] enemyButtons = new Button[mapSize, mapSize];
        public CheckedListBox checkedListBox1 = new CheckedListBox();
        public CheckedListBox checkedListBox2 = new CheckedListBox();
        Dictionary<int, int> countShips1 = new Dictionary<int, int>();
        Bot bot;
        public int allCountShips = 20;
        List<Ship> shipsPlayer;
        Animation animation = new Animation();
        //public PictureBox picture = new PictureBox();
        public Form1()
        {
            InitializeComponent();
            Init();
        }

        public void Init()
        {
            shipsPlayer = new List<Ship>();
            startGame = false;
            CreateM();
            
            bot = new Bot(enemyMap, myMap, enemyButtons, myButtons, shipsPlayer);
            enemyMap = bot.ConfigureBotsShips();
            //picture.Size = new Size(30, 30);

        }

        

        public void CreateM()
        {
            this.Width = mapSize * 2 * cellSize + 90;
            this.Height = (mapSize + 3) * cellSize + 70;
            for (int i = 0; i < mapSize; i++)
            {
                for (int j = 0; j < mapSize; j++)
                {
                    myMap[i, j] = 0;
                    Button button = new Button();
                    button.Location = new Point(i * cellSize, j * cellSize);
                    button.Size = new Size(cellSize, cellSize);
                    button.BackColor = Color.White;
                    button.Click += new EventHandler(CreateShips);
                    if (i == 0 || j == 0)
                    {
                        button.BackColor = Color.Gray;
                        button.Enabled = false;
                        if (i == 0 && j > 0)
                            button.Text = j.ToString();
                        if (j == 0 && i > 0)
                            button.Text = alphabet_Rus[i - 1].ToString();
                    }
                    myButtons[i, j] = button;
                    this.Controls.Add(button);
                }
            }
            for (int i = 0; i < mapSize; i++)
            {
                for (int j = 0; j < mapSize; j++)
                {
                    enemyMap[i, j] = 0;
                    Button button = new Button();
                    button.Location = new Point(i * cellSize + 350, j * cellSize);
                    button.Size = new Size(cellSize, cellSize);
                    button.BackColor = Color.White;
                    if (i == 0 || j == 0)
                    {
                        button.BackColor = Color.Gray;
                        button.Enabled = false;
                        if (i == 0 && j > 0)
                            button.Text = j.ToString();
                        if (j == 0 && i > 0)
                            button.Text = alphabet_Rus[i - 1].ToString();
                    }
                    else
                    {
                        button.Click += new EventHandler(PlayerShoot);
                    }
                    enemyButtons[i, j] = button;
                    this.Controls.Add(button);
                }
            }
            Button start = new Button();
            start.Text = "Погнали";
            start.Click += new EventHandler(Start);
            start.Location = new Point(0, mapSize * cellSize + 10);
            this.Controls.Add(start);
            string[] words1 = { "4", "3", "2", "1" };
            checkedListBox1.Location = new Point(100, mapSize * cellSize + 20);
            for (int i = 0; i < words1.Length; i++)
                checkedListBox1.Items.Add(words1[i]);
            checkedListBox1.SelectionMode = SelectionMode.One;
            this.Controls.Add(checkedListBox1);

            string[] words2 = { "Вертикально", "Горизонтально" };
            checkedListBox2.Location = new Point(250, mapSize * cellSize + 20);
            for (int i = 0; i < words2.Length; i++)
                checkedListBox2.Items.Add(words2[i]);
            checkedListBox2.SelectionMode = SelectionMode.One;
            this.Controls.Add(checkedListBox2);
            //picture.Location = new Point(400, mapSize * cellSize + 20);
            //picture.SizeMode = PictureBoxSizeMode.StretchImage;
            //picture.BackColor = Color.White;
            //this.Controls.Add(picture);


            countShips1.Add(4, 1);
            countShips1.Add(3, 2);
            countShips1.Add(2, 3);
            countShips1.Add(1, 4);

        }

        public void Start(object sender, EventArgs e)
        {
            if (countShips1[4] == 0 && countShips1[3] == 0 && countShips1[2] == 0 && countShips1[1] == 0)
            {
                startGame = true;
            }
            else
            {
                MessageBox.Show("Нельзя");
            }
        }

        public void PlayerShoot(object sender, EventArgs e)
        {
            Button pressedButton = sender as Button;
            if (startGame)
            {
                bool playerTurn = Shoot(enemyMap, pressedButton);
                if (!playerTurn)
                    bot.Shoot();
            }

        }

        public void CreateShips(object sender, EventArgs e)
        {
            Button button = sender as Button;
            Ship ship = new Ship();
            if (!startGame)
            {
                int positionInCheckedListBoxOne = Convert.ToInt32(checkedListBox1.SelectedItem);
                string positionInCheckedListBoxTwo = Convert.ToString(checkedListBox2.SelectedItem);

                for (int i = 1; i < mapSize; i++)
                {
                    for (int j = 1; j < mapSize; j++)
                    {
                        if (myButtons[i, j] == button)
                        {
                            if (positionInCheckedListBoxTwo == "Вертикально")
                            {
                                int k = 0;
                                bool n = true;
                                if (countShips1[positionInCheckedListBoxOne] > 0)
                                {
                                    if (j + positionInCheckedListBoxOne <= mapSize)
                                    {
                                        for (int cur1 = i - 1; cur1 <= i + 1; cur1++)
                                        {
                                            if (cur1 >= mapSize)
                                                break;
                                            for (int cur2 = j - 1; cur2 <= j + positionInCheckedListBoxOne; cur2++)
                                            {
                                                if (cur2 >= mapSize)
                                                    break;
                                                if (myMap[myButtons[cur1, cur2].Location.X / cellSize, myButtons[cur1, cur2].Location.Y / cellSize] == 1)
                                                {
                                                    n = false;
                                                    MessageBox.Show("Нельзя");
                                                    break;
                                                }
                                            }
                                            if (!n)
                                                break;
                                        }
                                        if (n == true)
                                        {
                                            while (k < positionInCheckedListBoxOne)
                                            {
                                                MyPoint point = new MyPoint();
                                                point.point.X = myButtons[i, j + k].Location.X / cellSize;
                                                point.point.Y = myButtons[i, j + k].Location.Y / cellSize;
                                                myButtons[i, j + k].BackColor = Color.Red;
                                                myMap[myButtons[i, j + k].Location.X / cellSize, myButtons[i, j + k].Location.Y / cellSize] = 1;
                                                ship.AddPoint(point);
                                                k++;
                                            }
                                            countShips1[positionInCheckedListBoxOne]--;
                                            shipsPlayer.Add(ship);
                                        }
                                    }
                                    else
                                    {
                                        MessageBox.Show("Нельзя");
                                        break;
                                    }
                                }
                                else
                                {
                                    MessageBox.Show("Нельзя");
                                    break;
                                }
                            }
                            if (positionInCheckedListBoxTwo == "Горизонтально")
                            {
                                int k = 0;
                                bool n = true;
                                if (countShips1[positionInCheckedListBoxOne] > 0)
                                {
                                    if (i + positionInCheckedListBoxOne <= mapSize)
                                    {
                                        for (int cur1 = i - 1; cur1 <= i + positionInCheckedListBoxOne; cur1++)
                                        {
                                            if (cur1 >= mapSize)
                                                break;
                                            for (int cur2 = j - 1; cur2 <= j + 1; cur2++)
                                            {
                                                if (cur2 >= mapSize)
                                                    break;
                                                if (myMap[myButtons[cur1, cur2].Location.X / cellSize, myButtons[cur1, cur2].Location.Y / cellSize] == 1)
                                                {
                                                    n = false;
                                                    MessageBox.Show("Нельзя");
                                                    break;
                                                }
                                            }
                                            if (!n)
                                                break;
                                        }
                                        if (n == true)
                                        {
                                            while (k < positionInCheckedListBoxOne)
                                            {
                                                MyPoint point = new MyPoint();
                                                point.point.X = myButtons[i + k, j].Location.X / cellSize;
                                                point.point.Y = myButtons[i + k, j].Location.Y / cellSize;
                                                ship.AddPoint(point);
                                                myButtons[i + k, j].BackColor = Color.Red;
                                                myMap[myButtons[i + k, j].Location.X / cellSize, myButtons[i + k, j].Location.Y / cellSize] = 1;
                                                k++;
                                            }
                                            countShips1[positionInCheckedListBoxOne]--;
                                            shipsPlayer.Add(ship);
                                        }
                                    }
                                    else
                                    {
                                        MessageBox.Show("Нельзя");
                                        break;
                                    }
                                }
                                else
                                {
                                    MessageBox.Show("Нельзя");
                                    break;
                                }
                            }
                        }
                    }

                }
            }
        }

        public bool Shoot(int[,] map, Button pressedButton)
        {
            bool hit = false;
          
            
            if (startGame)
            {
                int delta = 0;
                if (pressedButton.Location.X > 350)
                    delta = 350;
                if (map[(pressedButton.Location.X - delta) / cellSize, pressedButton.Location.Y / cellSize] == 1)
                {
                    animation.Test(pressedButton);
                    hit = true;

                    Ship ship = new Ship();
                    foreach (Ship shipS in bot.shipsBot)
                    {
                        if (shipS.GetHit((pressedButton.Location.X - delta) / cellSize, pressedButton.Location.Y / cellSize))
                            ship = shipS;
                    }
                    ship.CheckLife();
                    map[(pressedButton.Location.X - delta) / cellSize, pressedButton.Location.Y / cellSize] = -2;
                    pressedButton.BackColor = Color.Green;
                    //pressedButton.Text = "X";
                    if (!ship.GetIsAlive())
                    {
                        for (int i = ship.ship[0].point.X - 1; i <= ship.ship[ship.ship.Count() - 1].point.X + 1; i++)
                        {
                            for (int j = ship.ship[0].point.Y - 1; j <= ship.ship[ship.ship.Count() - 1].point.Y + 1; j++)
                            {
                                if (i < mapSize && j < mapSize && i != 0 && j != 0)
                                {
                                    if (enemyMap[i, j] != -2)
                                    {
                                        enemyButtons[i, j].BackColor = Color.Blue;
                                       

                                    }
                                }
                            }
                        }
            
                    }
                    
          
                    allCountShips--;
                }
                else
                {
                    hit = false;
                    pressedButton.BackColor = Color.Blue;
                }
           
                if (allCountShips == 0)
                {
                    for (int i = 0; i < mapSize; i++)
                    {
                        for (int j = 0; j < mapSize; j++)
                        {
                            enemyButtons[i, j].Enabled = false;
                            myButtons[i, j].Enabled = false;
                        }
                    }
                    MessageBox.Show("Вы выиграли!");
                }
            }
            //pressedButton.Enabled = false;
            return hit;
        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }
    }
}
