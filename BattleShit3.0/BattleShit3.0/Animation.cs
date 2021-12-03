using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace BattleShit3._0
{
    class Animation
    {
        public List<Image> frames = new List<Image>();
        public Animation()
        {
            string path = @"C:\Users\Михаил\source\repos\BattleShit3.0\BattleShit3.0\bin\Debug\Animation\";
            for (int i = 1; i < 5; i++)
            {
                string imgPath = path + i.ToString() + ".jpg";
                FileStream fs0 = new FileStream(imgPath, FileMode.Open);
                Image img = Image.FromStream(fs0);
                Console.WriteLine(imgPath);
                fs0.Close();
                frames.Add(img);
            }
        }

        public async void Test(Button pressedButton)
        {
            foreach (Image frame in frames)
            {
                pressedButton.Image = frame;
                await Task.Delay(100);
            }
        }
    }
}
