using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;

namespace atomiki1
{
    public partial class Form2 : Form
    {
        string PlayerName;
        int highScore;
        int difficulty;
        int score = 0;
        int minX, minY;
        int offsetX = 110;
        int offsetY = 130;
        int offsetX2 = 139;
        int offsetY2 = 106;
        Random rnd = new Random();

        private void startButton_Click(object sender, EventArgs e)
        {
            countdownTimer.Start();
            gameTimer.Start();
            if (difficulty == 3)
                FriendlyFireTimer.Start();
            pictureBox1.Visible = true;
            button1.Enabled = false;
            button2.Enabled = false;
            button3.Enabled = false;
            score = 0;
            scoreLabel.Text = "0";
            if(difficulty == 3)
            {
                //pictureBox2.Location = new Point(rnd.Next(minX + offsetX2, minX + panel1.Size.Width - offsetX2), rnd.Next(minY + offsetY2, minY + panel1.Size.Height - offsetY2));
                pictureBox2.Location = new Point(minX, rnd.Next(minY + offsetY2, minY + panel1.Size.Height - offsetY2));
                pictureBox2.Show();
            }
        }

        private void updateDifficultyText()
        {
            if (difficulty == 1)
            {
                difficultyLabel.Text = "Easy, every hit is worth 10 points";
            }
            else if (difficulty == 2)
            {
                difficultyLabel.Text = "Medium, every hit is worth 20 points";
            }
            else if (difficulty == 3)
            {
                difficultyLabel.Text = "Hard, every hit is worth 30 points but hitting the sheep is -10!";
            }
        }

        private void gameTimer_Tick(object sender, EventArgs e)
        {
            pictureBox1.Show();
            pictureBox1.Location = new Point(rnd.Next(minX + offsetX, minX + panel1.Size.Width - offsetX), rnd.Next(minY + offsetY, minY + panel1.Size.Height - offsetY));
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            timeLeft--;
            timeLabel.Text = timeLeft.ToString();
            if (difficulty == 3)
            {
                gameTimer.Interval = rnd.Next(500, 1000);
                //FriendlyFireTimer.Interval = 5000;
            }
            else if (difficulty == 2)
            {
                gameTimer.Interval = 900;
            }
            else
            {
                gameTimer.Interval = 1000;
            }
            if (timeLeft == 0)
            {
                pictureBox1.Hide();
                pictureBox2.Hide();
                countdownTimer.Stop();
                gameTimer.Stop();
                FriendlyFireTimer.Stop();
                timeLeft = 30;
                MessageBox.Show("Game Over\nYour score is: " + score.ToString());
                timeLabel.Text = timeLeft.ToString();
                button1.Enabled = true;
                button2.Enabled = true;
                button3.Enabled = true;
                
                
                    FileStream fs = new FileStream("players.txt", FileMode.OpenOrCreate, FileAccess.ReadWrite, FileShare.None);
                    StreamReader readText = new StreamReader(fs);
                    StreamWriter writeText = new StreamWriter(fs);
                    string newText = readText.ReadToEnd();
                    string[] textpin = newText.Split('\n');
                    string[] date = DateTime.Now.ToString().Split(' ');
                    for(int i = 0; i < textpin.Length - 1; i++)
                    {
                    string[] subtext = textpin[i].Split(' ');
                    if(subtext[0] == PlayerName)
                    {
                        subtext[2] = date[0];
                        subtext[3] = date[1];
                        subtext[4] = date[2];


                        if (score > highScore)
                        {
                            subtext[1] = score.ToString();
                        }
                        textpin[i] = String.Join(" ", subtext);
                    }
                    
                        
                    }
                    newText = String.Join("\n", textpin);
                    
                    //newText = newText.Replace(PlayerName + " " + highScore, PlayerName + " " + score.ToString());
                    fs.SetLength(0);
                    writeText.Write(newText);
                    writeText.Flush();
                    fs.Close();
                    highScore = score;
                    
                
                highScoreLabel.Text = highScore.ToString();




            }
        }

        int timeLeft = 30;

        private void button1_Click(object sender, EventArgs e)
        {
            difficulty = 1;
            updateDifficultyText();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            difficulty = 2;
            updateDifficultyText();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            difficulty = 3;
            updateDifficultyText();
        }

        private void pictureBox1_Click(object sender, EventArgs e)
        {
            int scoregain;
            if (difficulty == 1)
            {
                scoregain = 10;
            }
            else if (difficulty == 2)
            {
                scoregain = 20;
            }
            else
                scoregain = 30;

            score += scoregain;
            scoreLabel.Text = score.ToString();
            pictureBox1.Hide();
            pictureBox1.Location = new Point(rnd.Next(minX + offsetX, minX + panel1.Size.Width - offsetX), rnd.Next(minY + offsetY, minY + panel1.Size.Height - offsetY));

        }

        public Form2(string PlayerName, string highScore)
        {
            if (highScore != "N/A")
                this.highScore = int.Parse(highScore);
            else
                this.highScore = int.MinValue;
            this.PlayerName = PlayerName;
            this.difficulty = 1;
            InitializeComponent();
        }

        private void logoutButton_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void button4_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void pictureBox2_Click(object sender, EventArgs e)
        {
            score += -10;
            scoreLabel.Text = score.ToString();
            pictureBox2.Hide();
            //pictureBox2.Location = new Point(rnd.Next(minX + offsetX2, minX + panel1.Size.Width - offsetX2), rnd.Next(minY + offsetY2, minY + panel1.Size.Height - offsetY2));
        }

        private void timer1_Tick_1(object sender, EventArgs e)
        {
            pictureBox2.Location = new Point(pictureBox2.Location.X + 5, pictureBox2.Location.Y);
            if (pictureBox2.Location.X > panel1.Size.Width)
            {
                pictureBox2.Show();
                pictureBox2.Location = new Point(minX, rnd.Next(minY + offsetY2, minY + panel1.Size.Height - offsetY2));
            }
        }

        private void Form2_Load(object sender, EventArgs e)
        {
            nameLabel.Text = PlayerName;
            FriendlyFireTimer.Interval = 1;
            if (highScore != int.MinValue)
                highScoreLabel.Text = highScore.ToString();
            else
                highScoreLabel.Text = "N/A";
            minX = panel1.Location.X;
            minY = panel1.Location.Y;
            pictureBox1.Visible = false;
            pictureBox2.Visible = false;
            updateDifficultyText();
        }
    }
}
