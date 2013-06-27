using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace Enterprise_Decision_Engine
{
    public partial class EDEForm : Form
    {
        Option[] Outcomes = new Option[2];

        public EDEForm()
        {
            InitializeComponent();
        }

        private void EDEForm_Load(object sender, EventArgs e)
        {
            rbNumber.Checked = true;
            HideAll();
        }

        private void btnDecide_Click(object sender, EventArgs e)
        {
            int iFlag = 0;
            //checks to see which radiobutton is ticked - Tom
            iFlag = rbNumber.Checked ? 2 : rbTrueFalse.Checked ? 4 : rbCoin.Checked ? 8 : rbHands.Checked ? 16 : 2;
            int iRNG = EDE.RNG() % 2;
            Outcomes[iRNG++] = new Option(iFlag^1);
            Outcomes[iRNG % 2] = new Option(iFlag);
            DisplayResult();
        }

        private void DisplayResult()
        {
            //loop through Outcome object to determine active one - Mark
            Option outcome = null;
            int i = 0;
            for (i=0; i <= 1; i++) {
                if (Outcomes[i].Sign) {
                    outcome = Outcomes[i];
                    break;
                }
            }
            //determine symbol type and display result - Mark
            HideAll();
            switch (outcome.Symbol) {

                case "Heads":
                case "Tails":
                    if (i == 0) {
                        imgHeads.Visible = true;
                    } else {
                        imgTails.Visible = true;
                    }
                    break;
                case "Left":
                case "Right":
                    lblLeft.Visible = true;
                    lblRight.Visible = true;
                    if (i==0) {
                        lblLeft.BackColor = Color.Green;
                        lblRight.BackColor = Color.Red;
                    } else {
                        lblRight.BackColor = Color.Green;
                        lblLeft.BackColor = Color.Red;
                    }
                    break;
                case "True":
                case "False":
                    lblCentre.Visible = true;
                    if (i==0) {
                        lblCentre.Text = "True";
                    } else {
                        lblCentre.Text = "False";
                    }
                    break;
                case "0":
                case "1":
                default:
                    lblCentre.Visible = true;
                    if (i==0) {
                        lblCentre.Text = "1";
                    } else {
                        lblCentre.Text = "0";
                    }
                    break;
            }

        }

        private void HideAll()
        {
            lblCentre.Visible = false;
            lblLeft.Visible = false;
            lblRight.Visible = false;
            imgHeads.Visible = false;
            imgTails.Visible = false;
        }

        private void btnReset_Click(object sender, EventArgs e)
        {
            HideAll();
        }

        #region "RadioButton selections"
        
        //private void rbNumber_CheckedChanged(object sender, EventArgs e)
        //{
        //    Option.ChangeFlags(3, Outcomes);
        //}

        //private void rbTrueFalse_CheckedChanged(object sender, EventArgs e)
        //{
        //    Option.ChangeFlags(5,Outcomes);
        //}

        //private void rbCoin_CheckedChanged(object sender, EventArgs e)
        //{
        //    Option.ChangeFlags(8, Outcomes);
        //}

        //private void rbHands_CheckedChanged(object sender, EventArgs e)
        //{
        //    Option.ChangeFlags(16, Outcomes);
        //}
        #endregion
    }
}
