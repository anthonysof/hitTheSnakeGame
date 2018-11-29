using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace atomiki1
{
    public partial class Form1 : Form
    {
        public int difficulty;
        public string PlayerName;
        bool found = false;
        string[] display = new string[5];
        private Form2 GameForm;
        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            if (!File.Exists("players.txt"))
            {
                File.Create("players.txt");
            }
            nameLabel.Text = "";
            scoreLabel.Text = "";
            groupBox1.Visible = false;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            found = false;
            Dictionary<string, int> playerDict = new Dictionary<string, int>();
            richTextBox2.Text = "";
            if (richTextBox1.Text != "")
            {
                if (loginButton.Text != "Logout")
                {
                    PlayerName = richTextBox1.Text;
                    string[] temp;
                    string line;

                    using (StreamReader readText = new StreamReader("players.txt"))
                    {
                        while ((line = readText.ReadLine()) != null)
                        {
                            temp = line.Split(' ');
                            if (temp[1] != "N/A")
                                playerDict.Add(temp[0], int.Parse(temp[1]));
                            else
                                playerDict.Add(temp[0], int.MinValue);
                            if (temp[0] == PlayerName)
                            {
                                //temp[2] = DateTime.Now.ToString();
                                found = true;
                                MessageBox.Show("Player: " + PlayerName + " Already registered!");
                                display = temp;

                            }
                        }
                        readText.Close();
                    }



                    if (!found)
                    {
                        using (StreamWriter writeText = File.AppendText("players.txt"))
                        {
                            string date = DateTime.Now.ToString();
                            writeText.WriteLine(PlayerName + " N/A " + "N/A N/A N/A");
                            writeText.Close();
                            display[0] = PlayerName;
                            display[1] = "N/A";
                            display[2] = "N/A";
                            //writeText.Flush();
                            writeText.Close();
                        }

                    }
                    richTextBox1.ReadOnly = true;
                    loginButton.Text = "Logout";
                    nameLabel.Text = display[0];
                    scoreLabel.Text = display[1];
                    groupBox1.Visible = true;
                    if(display[2] == "N/A")
                    {
                        lastplayedLabel.Text = "N/A";
                    }
                    else
                        lastplayedLabel.Text = display[2] + " " + display[3] + " " + display[4];
                    //var ordered = (from entry in playerDict orderby entry.Value descending select entry).ToDictionary(pair => pair.Key, pair => pair.Value).Take(3);
                    var ordered = playerDict.OrderBy(x => x.Value).Reverse<KeyValuePair<string, int>>();
                    //ordered = ordered.Reverse();
                    ordered = ordered.Take<KeyValuePair<string, int>>(3);
                    foreach (KeyValuePair<string, int> item in ordered)
                    {
                        richTextBox2.Text += item.Key + " " + item.Value + "\n";

                    }
                }
                else
                {
                    richTextBox1.ReadOnly = false;
                    PlayerName = "";
                    loginButton.Text = "Login";
                    groupBox1.Visible = false;
                }
            }
            else
                MessageBox.Show("Please enter a name!");
            }
        
        private void startButton_Click(object sender, EventArgs e)
        {

            MessageBox.Show("Starting game");
            GameForm = new Form2(PlayerName, display[1]);
            this.Hide();
            GameForm.ShowDialog();
            button1_Click(sender, e);
            this.Show();

                
            
        }

        private void scoreLabel_Click(object sender, EventArgs e)
        {

        }

        private void button1_Click_1(object sender, EventArgs e)
        {
            Application.Exit();
        }
    }
}


