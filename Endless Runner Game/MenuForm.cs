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
    public partial class MenuForm : Form
    {
        public MenuForm()
        {
            InitializeComponent();

            // Връзваме бутоните към функциите им
            if (this.Controls.ContainsKey("button1")) // PLAY
            {
                this.Controls["button1"].Click += new System.EventHandler(this.btnPlay_Click);
            }
            if (this.Controls.ContainsKey("button2")) // CREDITS
            {
                this.Controls["button2"].Click += new System.EventHandler(this.btnCredits_Click);
            }
            if (this.Controls.ContainsKey("button3")) // EXIT
            {
                this.Controls["button3"].Click += new System.EventHandler(this.btnExit_Click);
            }
        }

        private void label1_Click(object sender, EventArgs e)
        {
        }

        // КЛИКВАНЕ ВЪРХУ БУТОНА "PLAY"
        private void btnPlay_Click(object sender, EventArgs e)
        {
            Game gameWindow = new Game();
            gameWindow.Show();
            this.Hide();

            gameWindow.FormClosed += (s, args) => this.Close();
        }

        // ПОПРАВЕНО: КЛИКВАНЕ ВЪРХУ БУТОНА "CREDITS"
        private void btnCredits_Click(object sender, EventArgs e)
        {
            // Показва изскачащо прозорче с твоето име
            MessageBox.Show("Тази игра е създадена от: Ademm93", "Credits", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        // КЛИКВАНЕ ВЪРХУ БУТОНА "EXIT"
        private void btnExit_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void MenuForm_Load(object sender, EventArgs e)
        {

        }
    }
}