using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Windows.Forms;
using System.IO;

namespace AnalLyzatore
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }
        public string TopLongWords() // + 
        {
            string res = "";
            string text = richTextBox1.Text as string;
            
            Regex rx = new Regex(@"\w+", RegexOptions.IgnoreCase);
            MatchCollection match = rx.Matches(text);
            List<string> toplong = new List<string>();
            foreach(Match key in match)
            {
                string name = key.Value;
                toplong.Add(name);
            }
            toplong.Sort((x,y) =>y.Length.CompareTo(x.Length));
            int i = 0;
            foreach(string key in toplong)
            {
                if(i < 10)
                {
                    res += key + "\n";
                    i++;
                }
            }
            
            return res;

        }

        public string UnicWords() // Подсчёт уникальных слов + 
        {
            string text = richTextBox1.Text as string;
            Regex rx = new Regex(@"\w+", RegexOptions.IgnoreCase);
            MatchCollection matches = rx.Matches(text);
            Dictionary<string, int> wordCount = new Dictionary<string, int>();
            foreach (Match m in matches) //получение словаря слово -> количество повторений
            {
                if (!wordCount.ContainsKey(m.Value.ToLower()))
                {
                    wordCount.Add(m.Value.ToLower(), 1);
                }
                else
                {
                    wordCount[m.Value.ToLower()]++;
                }
            }
            int num = 0;
            foreach (int freq in wordCount.Values)
            {
                if (freq == 1)
                    num++;
            }
            
            return "Кол-во уникальных слов: " + num;

        }

        private string CountWords() // Подсчёт кол-ва слов + 
        {
            int words = Regex.Split(richTextBox1.Text, @"\w+").Count() - 1;
            return "Кол-во слов: " + words;
        } 

        private string TopIdenticalWords() // Одинаковые слова + 
        {
            string res = "";
            string text = richTextBox1.Text as string;
            Regex rx = new Regex(@"\w+", RegexOptions.IgnoreCase);
            MatchCollection matches = rx.Matches(text);
            Dictionary<string, int> wordCount = new Dictionary<string, int>();
            foreach (Match m in matches) //получение словаря слово -> количество повторений
            {
                if (!wordCount.ContainsKey(m.Value.ToLower()))
                {
                    wordCount.Add(m.Value.ToLower(), 1);
                }
                else
                {
                    wordCount[m.Value.ToLower()]++;
                }
            }
            Dictionary<string, int> top10Words = new Dictionary<string, int>();
            while (top10Words.Count != wordCount.Count && top10Words.Count < 10)
            {
                int max = 0; string word = "";
                foreach (string key in wordCount.Keys)
                {
                    if (max < wordCount[key] && !top10Words.ContainsKey(key))
                    {
                        max = wordCount[key];
                        word = key;
                        res += word + ": " + wordCount[key] + "\n";
                    }
                }
                top10Words.Add(word, max);
            }
            return res;
        }

        private void button1_Click(object sender, EventArgs e) // Analizatore
        {
            richTextBox2.Text = CountWords();
            richTextBox5.Text = CountLetters();
            richTextBox3.Text = TopIdenticalWords();
            richTextBox4.Text = UnicWords();
            richTextBox6.Text = TopLongWords();
        }

        private void button2_Click(object sender, EventArgs e) // Delete + 
        {
            richTextBox1.Clear();
        }

        private void button3_Click(object sender, EventArgs e) // File + 
        {
            OpenFileDialog open = new OpenFileDialog();
            open.Filter = "Text files (*.txt)|*.txt|All files (*.*)|*.*";
            if (open.ShowDialog() == DialogResult.OK)
            {
                richTextBox1.LoadFile(open.FileName, RichTextBoxStreamType.PlainText);
            }
        }

        private void button4_Click(object sender, EventArgs e) // Save
        {
            File.WriteAllText(@"D:\\txt.txt", richTextBox2.Text + "\n" + richTextBox3.Text + richTextBox4 + "\n" + richTextBox5.Text + richTextBox6.Text);
        }

        private void richTextBox2_TextChanged(object sender, EventArgs e) // Output кол-ва слов + 
        {

        }

        private void richTextBox1_TextChanged(object sender, EventArgs e) // Input + 
        {

        }

        private string CountLetters() // Кол-во букв с % + 
        {
            int letters = Regex.Split(richTextBox1.Text, @"\p{L}").Count() - 1;
            string Alphabet_RUS = "абвгдеёжзийклмнопрстуфхцчшщъыьэюя";
            string res = "";
            
            for (int i = 0; i < Alphabet_RUS.Length; i++)
            {
                string pattern = "" + Alphabet_RUS[i];
                double tmp = Regex.Split(richTextBox1.Text, pattern, RegexOptions.IgnoreCase).Count() - 1;
                res += Alphabet_RUS[i] +": " + (tmp / letters*100) + "%\n";
            }
            string Alphabet_ENG = "abcdefghijklmnopqrstuvwxyz";
            for (int i = 0; i < Alphabet_ENG.Length; i++)
            {
                string pattern = "" + Alphabet_ENG[i];
                double tmp = Regex.Split(richTextBox1.Text, pattern, RegexOptions.IgnoreCase).Count() - 1;
                res += Alphabet_ENG[i] + ": " + (tmp / letters * 100) + "%\n";
            }
            return res;
        }

        private void richTextBox3_TextChanged(object sender, EventArgs e) // Output топ 10 одинаковых слов + 
        {

        }

        private void richTextBox4_TextChanged(object sender, EventArgs e) // Output кол-ва уникальных слов + 
        {

        }

        private void richTextBox6_TextChanged(object sender, EventArgs e)
        {

        }
    }
}
