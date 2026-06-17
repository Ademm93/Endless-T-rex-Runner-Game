using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Endless_Runner_Game
{
    public partial class Game : Form
    {
        bool jumping = false;
        int jumpSpeed = 12;
        int force = 12;
        int score = 0;
        int obstacleSpeed = 10;
        Random rand = new Random();
        int position;
        bool isGameOver = false;

        public Game()
        {
            InitializeComponent();
            GameReset();
        }

        private void Game_Load(object sender, EventArgs e)
        {
        }

        private void label1_Click(object sender, EventArgs e)
        {
        }

        private void MainGameTimerEvent(object sender, EventArgs e)
        {
            Trex.Top += jumpSpeed;
            txtScore.Text = "Score: " + score;

            if (jumping == true && force < 0)
            {
                jumping = false;
            }

            if (jumping == true)
            {
                jumpSpeed = -12;
                force -= 1;
            }
            else
            {
                jumpSpeed = 12;
            }

            if (Trex.Top > 366 && jumping == false)
            {
                force = 12;
                Trex.Top = 367;
                jumpSpeed = 0;
            }

            foreach (Control x in this.Controls)
            {
                if (x is PictureBox && (string)x.Tag == "obstacle")
                {
                    x.Left -= obstacleSpeed;

                    if (x.Left < -100)
                    {
                        x.Left = this.ClientSize.Width + rand.Next(200, 500) + (x.Width * 15);
                        score++;
                    }

                    if (Trex.Bounds.IntersectsWith(x.Bounds))
                    {
                        gameTimer.Stop();
                        // ПОПРАВЕНО: Преименувано от dead[1] на dead_1_
                        Trex.Image = Properties.Resources.running_1_;   
                        txtScore.Text += " Press R to Restart the game!";
                        isGameOver = true;
                    }
                }
            }

            if (score > 5)
            {
                obstacleSpeed = 15;
            }
        }

        // ПОПРАВЕНО: Променено от EventArgs на KeyEventArgs
        private void keyisdown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Space && jumping == false)
            {
                jumping = true;
            }
        }

        // ПОПРАВЕНО: Променено от EventArgs на KeyEventArgs
        private void keyisup(object sender, KeyEventArgs e)
        {
            if (jumping == true)
            {
                jumping = false;
            }

            if (e.KeyCode == Keys.R && isGameOver == true)
            {
                GameReset();
            }
        }

        private void GameReset()
        {
            force = 12;
            jumpSpeed = 0;
            jumping = false;
            score = 0;
            obstacleSpeed = 10;
            txtScore.Text = "Score: " + score;

            // Ако и тук ресурсът даде грешка, го промени на running_1_
            Trex.Image = Properties.Resources.running_1_;
            isGameOver = false;
            Trex.Top = 367;

            foreach (Control x in this.Controls)
            {
                if (x is PictureBox && (string)x.Tag == "obstacle")
                {
                    position = this.ClientSize.Width + rand.Next(500, 800) + (x.Width * 10);
                    x.Left = position;
                }
            }

            gameTimer.Start();
        }
    }
}