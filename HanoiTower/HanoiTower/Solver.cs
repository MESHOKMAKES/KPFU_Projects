using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;
using System.Threading;

namespace HanoiTower
{
    class Solver
    {
        List<Tower> towers;
        public List<Tower> Towers
        {
            get => towers;
            set => towers = value;
        }
        public Solver(List<Tower> towers)
        {
            this.Towers = towers;
            Transfer(towers[0].towerStack.Count, 0, 1);
            Console.WriteLine(towers[1].towerStack.Count().ToString());
        }
        void Transfer(int n, int i, int k)
        {
            if (n == 1)
            {
                Animation(i, k);
            }
            else
            {
                int tmp = 3 - i - k;
                Transfer(n - 1, i, tmp);
                Animation(i, k);
                Transfer(n - 1, tmp, k);
            }
        }
        void Animation(int i, int k)
        {
            Point p;
            for (int y = towers[i].towerStack.Peek().panel.Location.Y; y >= 70; y--)
            {
                p = new Point(towers[i].towerStack.Peek().panel.Location.X, y);
                towers[i].towerStack.Peek().panel.Location = p;
            }
            Thread.Sleep(2);

            for (int x = towers[i].towerStack.Peek().panel.Location.X; (i < k) ?
                x <= towers[k].panel.Location.X + 5 - towers[i].towerStack.Peek().panel.Width / 2 :
                x >= towers[k].panel.Location.X + 5 - towers[i].towerStack.Peek().panel.Width / 2; x = (i < k) ? x + 1 : x - 1)
            {
                p = new Point(x, towers[i].towerStack.Peek().panel.Location.Y);
                towers[i].towerStack.Peek().panel.Location = p;
            }
            Thread.Sleep(2);
            for (int y = towers[i].towerStack.Peek().panel.Location.Y;
                y <= towers[k].panel.Location.Y + towers[k].panel.Height -
                towers[k].towerStack.Count * ((towers[k].towerStack.Count == 0) ? 0 : towers[k].towerStack.Peek().panel.Height) -
                towers[i].towerStack.Peek().panel.Height; y++)
            {
                p = new Point(towers[i].towerStack.Peek().panel.Location.X, y);
                towers[i].towerStack.Peek().panel.Location = p;
            }
            towers[k].towerStack.Push(towers[i].towerStack.Pop());
            Thread.Sleep(2);
        }
    }
}
