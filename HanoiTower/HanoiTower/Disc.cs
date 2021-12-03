using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Drawing;

namespace HanoiTower
{
    public class Disc
    {
        public Panel panel;
        private Point current;
        private Tower cur_tower;
        private List<Tower> towers;
        public Tower Cur_tower
        {
            get => cur_tower;
            set => cur_tower = value;
        }
        public List<Tower> Towers
        {
            get => towers;
            set => towers = value;
        }
        public Disc(int i, int countDiscs)
        {
            panel = new Panel();
            panel.Width = (countDiscs + 1 - i) * 10 * 2;
            panel.Location = new Point(255 - (countDiscs + 1 - i) * 10, 400 - i * 20);
            panel.BorderStyle = BorderStyle.FixedSingle;
            panel.Height = 20;
            panel.BackColor = Color.FromArgb(15 * i, 255 - i, 255 - i * 25);
            panel.MouseDown += new MouseEventHandler(MouseDownDisc);
            panel.MouseMove += new MouseEventHandler(MouseMoveDisc);
            panel.MouseUp += new MouseEventHandler(MouseUpDisc);

        }
        private void MouseDownDisc(object sender, MouseEventArgs e)
        {
            if (panel != cur_tower.towerStack.Peek().panel)
                MessageBox.Show("Incorrect action");
            current = new Point(e.X, e.Y);
        }
        private void MouseMoveDisc(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                Point newlocation = panel.Location;
                newlocation.X += e.X - current.X;
                newlocation.Y += e.Y - current.Y;
                panel.Location = newlocation;
            }
        }
        private void MouseUpDisc(object sender, MouseEventArgs e)
        {
            Panel pl = sender as Panel;
            Disc d = cur_tower.towerStack.Pop();
            int i = 0;
            foreach(Tower t in towers)
            {
                if (pl.Location.X < t.panel.Location.X + ((i == 2) ? 250 : 250/2) &&
                    pl.Location.X + pl.Width > t.panel.Location.X - ((i == 0) ? 250 : 250/2))
                {
                    if(t.towerStack.Count == 0 || d.panel.Width < t.towerStack.Peek().panel.Width )
                    {
                        pl.Location = new Point((t.panel.Location.X + t.panel.Width / 2) - panel.Width / 2, 
                            t.panel.Location.Y + t.panel.Height - pl.Height - t.towerStack.Count * 20);
                        t.towerStack.Push(d);
                        cur_tower = t;
                    }
                    else
                    {
                        pl.Location = new Point(cur_tower.towerStack.Peek().panel.Location.X + (cur_tower.towerStack.Peek().panel.Width - pl.Width) / 2,
                           cur_tower.towerStack.Peek().panel.Location.Y - pl.Height);
                        d.panel.Location = pl.Location;
                        cur_tower.towerStack.Push(d);
                    }
                }
                i++;
            }
        }
    }
}
