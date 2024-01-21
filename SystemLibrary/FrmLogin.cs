﻿using System;
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
    public partial class FrmLogin : Form
    {
        public FrmLogin()
        {
            InitializeComponent();
        }

        private void btnCancelar_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void btnEntrar_Click(object sender, EventArgs e)
        {
            if (txtUsuario.Text == "Cleiton" && txtSenha.Text == "123")
            {
                FrmPricipal principal = new FrmPricipal();
                principal.Show();
                this.Visible = false;
                this.Close();
            }
            else
            {
                MessageBox.Show("Usuarío ou Senha Inválidos",
                                "Ocorreu um Erro ao Autenticar",
                                MessageBoxButtons.OK,
                                MessageBoxIcon.Error); 
            }
        }
    }
}