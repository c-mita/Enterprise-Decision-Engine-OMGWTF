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
            lblRNG.Text = "";
        }

        private void button1_Click(object sender, EventArgs e)
        {
            Outcomes[0] = new Option(2);
            Outcomes[1] = new Option(3);
            lblRNG.Text = EDE.RNG().ToString();
            Option.ChangeFlags(9, Outcomes);
        }
    }
}
