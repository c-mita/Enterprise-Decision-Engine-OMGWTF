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
            for (int i=0; i <= 1; i++) {
                if (Outcomes[i].Sign) {
                    outcome = Outcomes[i];
                }
            }
            //determine symbol type and display result - Mark
            HideAll();
            switch (outcome.Symbol) {
                case "0":
                case "1":
                    lblCentre.Visible = true;
                    if (outcome.Sign) {
                        lblCentre.Text = "1";
                    } else {
                        lblCentre.Text = "0";
                    }
                    break;
                default:
                    break;
            }

        }

        private void HideAll()
        {
            lblCentre.Visible = false;
            lblLeft.Visible = false;
            lblRight.Visible = false;
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
