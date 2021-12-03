using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace HanoiTower
{
    public partial class Form1 : Form
    {
        Solver solver;
        ComboBox comboBox;
        Button startGame;
        Button reStart;
        Button solverButton;
        List<Tower> towers;
        
        public Form1()
        {
            InitializeComponent();
            CreateMap();
            StartGameButton();
            RestartGameButton();
            SolverButton();
        }
        public void CreateMap()
        {
            this.Size = new Size(1000, 450);
            this.MaximumSize = new Size(1000, 450);
            this.MinimumSize = new Size(1000, 450);
            comboBox = new ComboBox();
            comboBox.Location = new Point(10, 10);
            comboBox.Size = new Size(50, 40);
            for(int i = 0; i < 8; i++)
            {
                comboBox.Items.Add(i + 3);
            }
            this.Controls.Add(comboBox);
        }
        public void StartGameButton()
        {
            startGame = new Button();
            startGame.Location = new Point(comboBox.Width + comboBox.Location.X + 10, comboBox.Location.Y);
            startGame.Size = new Size(40, comboBox.Height);
            startGame.Text = "Ok";
            startGame.Click += new EventHandler(CreateTowers);
            this.Controls.Add(startGame);
        }
        public void CreateTowers(object sender, EventArgs e)
        {
            towers = new List<Tower>();
            startGame.Enabled = false;
            for (int i = 0; i < 3; i++)
            {
                Tower t = new Tower(i + 1, Convert.ToInt32(comboBox.SelectedItem));
                this.Controls.Add(t.panel);
                if(i == 0)
                    foreach(Disc d in t.towerStack)
                    {
                        this.Controls.Add(d.panel);
                    }
                towers.Add(t);
            }
            foreach(Tower tower in towers)
            {
                foreach(Disc d in tower.towerStack)
                {
                    d.Cur_tower = tower;
                    d.Towers = towers;
                }
            }
        }
        public void RestartGameButton()
        {
            reStart = new Button();
            reStart.Location = new Point(startGame.Width + startGame.Location.X + 10, startGame.Location.Y);
            reStart.Size = new Size(50, startGame.Height);
            reStart.Text = "Restart";
            reStart.Click += new EventHandler(RestartGameClick);
            this.Controls.Add(reStart);
        }
        public void SolverButton()
        {
            solverButton = new Button();
            solverButton.Location = new Point(reStart.Width + reStart.Location.X + 10, reStart.Location.Y);
            solverButton.Size = new Size(50, reStart.Height);
            solverButton.Text = "Solver";
            solverButton.Click += new EventHandler(SolverClick);
            this.Controls.Add(solverButton);
        }
        public void SolverClick(object sender, EventArgs e)
        {
            solverButton.Enabled = false;
            solver = new Solver(towers);
        }
        public void RestartGameClick(object sender, EventArgs e)
        {
            this.Controls.Clear();
            CreateMap();
            StartGameButton();
            RestartGameButton();
            SolverButton();
        }
        private void Form1_Load(object sender, EventArgs e)
        {

        }
    }
}
