using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BattleShit3._0
{
    class MyPoint
    {
        public Point point;
        private bool isAlive;
        public MyPoint()
        {
            isAlive = true;
        }

        public MyPoint(Point point)
        {
            this.point = point;
            isAlive = true;
        }

        public bool GetIsAlive()
        {
            return isAlive;
        }

        public void SetIsAlive(bool flag)
        {
            isAlive = flag;
        }
    }
}
