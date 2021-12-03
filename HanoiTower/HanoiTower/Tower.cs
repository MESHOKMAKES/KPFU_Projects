using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Drawing;

namespace HanoiTower
{
    public class Tower
    {
        public Stack<Disc> towerStack;
        public Panel panel;
        public Tower(int i, int countDiscs)
        {
            CreateTowerPanel(i);
            towerStack = new Stack<Disc>();
            if (i == 1)
                CreateDiscs(countDiscs);
        }
        private void CreateTowerPanel(int i)
        {
            panel = new Panel();
            panel.Location = new Point(i * 250, 100);
            panel.Width = 10;
            panel.Height = 300;
            panel.BackColor = Color.Gray;
        }
        private void CreateDiscs(int countDiscs)
        {
            for(int i = 1; i <= countDiscs; i++)
            {
                Disc disc = new Disc(i, countDiscs);
                towerStack.Push(disc);
                disc.Cur_tower = this;
            }
        }
    }
}
