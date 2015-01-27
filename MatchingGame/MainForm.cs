using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace MatchingGame
{

    public partial class MatchingGame : Form
    {
        // RNG
        private Random randomizer = new Random();

        // Preset letters from the Webdings font.  Each of these represents an interesting image.
        private List<string> icons = new List<string>()
        {
            "!", "!", "N", "N", ",", ",", "k", "k",
            "b", "b", "v", "v", "w", "w", "z", "z"
        };

        // Track which labels were selected, and in which order.
        Label firstClicked = null;
        Label secondClicked = null;

        // Forms used by this app.
        ConfigForm config = new ConfigForm();

        public MatchingGame()
        {
            InitializeComponent();

            AssignIconsToSquares();
        }

        /// <summary>
        /// Assigns Icons to squares.  Pulls random icons from the icons list.
        /// </summary>
        public void AssignIconsToSquares()
        {
            // Use a temporary list while generating the puzzle.
            List<string> tempList = new List<string>();

            foreach(string icon in icons)
            {
                tempList.Add(icon);
            }

            foreach(Control control in tableLayoutPanel1.Controls)
            {
                Label iconLabel = control as Label;
                if (iconLabel != null)
                {
                    int randomNumber = randomizer.Next(tempList.Count);
                    iconLabel.Text = tempList[randomNumber];
                    iconLabel.ForeColor = iconLabel.BackColor;
                    tempList.RemoveAt(randomNumber);
                }
            }
        }

        /// <summary>
        /// Traps mouse clicks to each of the labels.
        /// </summary>
        /// <param name="sender">The label that was clicked</param>
        /// <param name="e">Unused</param>
        private void label_Click(object sender, EventArgs e)
        {
            if( gameTimer.Enabled == true)
            {
                return;
            }

            Label clicked = sender as Label;
            if (clicked != null)
            {
                if(clicked.ForeColor == Color.Black)
                {
                    return;
                }

                /*
                 * Sees if this is the first label clicked.
                 */
                if (firstClicked == null)
                {
                    firstClicked = clicked;
                    firstClicked.ForeColor = Color.Black;
                    return;
                }

                // Second label got clicked.
                secondClicked = clicked;
                secondClicked.ForeColor = Color.Black;

                CheckForWinner();

                if (firstClicked.Text == secondClicked.Text)
                {
                    firstClicked = null;
                    secondClicked = null;
                    return;
                }

                gameTimer.Start();
            }
        }

        /// <summary>
        /// Fires when the timer expires.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void gameClock_tick(object sender, EventArgs e)
        {
            // Stops the clock.
            gameTimer.Stop();

            firstClicked.ForeColor = firstClicked.BackColor;
            secondClicked.ForeColor = secondClicked.BackColor;

            firstClicked = null;
            secondClicked = null;
        }

        private void CheckForWinner()
        {
            foreach( Control control in tableLayoutPanel1.Controls)
            {
                Label iconLabel = control as Label;
                if(iconLabel != null)
                {
                    if (iconLabel.ForeColor == iconLabel.BackColor)
                    {
                        return;
                    }
                }
            }

            // This is a winner!
            MessageBox.Show("You've won the game", "Yay!");
            Close();
        }

        private void menuClose_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void menuConfig_Click(object sender, EventArgs e)
        {
            config.Show();
        }

        private void mainMenuReset_Click(object sender, EventArgs e)
        {
            /*
             * Stop the timers.
             */
            if (gameTimer.Enabled == true)
            {
                gameTimer.Stop();
            }

            /*
             * Reset state.
             */
            firstClicked = null;
            secondClicked = null;

            foreach( Control control in tableLayoutPanel1.Controls)
            {
                Label iconLabel = control as Label;

                if(iconLabel != null)
                {
                    iconLabel.ForeColor = iconLabel.BackColor;
                }
            }

            // Generate a new puzzle.
            AssignIconsToSquares();
        }
    }
}
