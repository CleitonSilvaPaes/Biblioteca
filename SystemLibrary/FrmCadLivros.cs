using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SystemLibrary
{
    public partial class FrmCadLivros : Form
    {
        public FrmCadLivros()
        {
            InitializeComponent();
            cbStatus.DropDownStyle = ComboBoxStyle.DropDownList;
        }

        private void BtnCancelar_Click(object sender, EventArgs e)
        {
            FrmPricipal frmPricipal = new FrmPricipal();
            frmPricipal.Visible = true;
            this.Close();

        }

        private void Button1_Click(object sender, EventArgs e)
        {
            FrmPricipal frmPricipal = new FrmPricipal();
            frmPricipal.Visible = true;
            this.Close();
        }
    }
}
