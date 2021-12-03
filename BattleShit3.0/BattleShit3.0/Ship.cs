using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BattleShit3._0
{
    class Ship
    {
        public List<MyPoint> ship;
        private bool isAlive;
        public Ship()
        {
            ship = new List<MyPoint>();
            isAlive = true;
        }
        public void AddPoint(MyPoint p)
        {
            ship.Add(p);
        }
        public void CheckLife()
        {
            int k = 0;
            foreach (MyPoint point in ship)
            {
                if (point.GetIsAlive())
                {
                    k++;
                }
            }

            isAlive = k != 0;
        }
        public bool GetIsAlive()
        {
            return isAlive;
        }
        public bool GetHit(int x, int y)
        {
            bool k = false;
            foreach (MyPoint p in ship)
            {
                if (p.point.Y == y && p.point.X == x)
                {
                    k = true;
                    p.SetIsAlive(false);
                }
            }
            return k;
        }
    }
}
