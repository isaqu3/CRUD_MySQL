using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using MySql.Data.MySqlClient;

namespace CRUD_MySQL
{
    public partial class Form1 : Form
    {
        // string de conexão com o banco MySQL
        MySqlConnection conexao = new MySqlConnection
                    ("Server = localhost; Database = europa; Uid = root; Pwd = 123@qwe;");

        MySqlCommand comando;
        MySqlDataAdapter da;
        string strSQL;

        public Form1()
        {
            InitializeComponent();
        }

        static void IntNumber(KeyPressEventArgs e)
        {
            // função para permitir somente a entrada de números em algum 
            if (!char.IsDigit(e.KeyChar) && e.KeyChar != 8)
            {
                e.Handled = true;
            }
        }

        private void txtCPF_KeyPress(object sender, KeyPressEventArgs e)
        {
            // função de somente números para o campo CPF
            IntNumber(e);
        }

        private void txtID_KeyPress(object sender, KeyPressEventArgs e)
        {
            // função de somente números para o campo ID

            IntNumber(e);
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            // inicia o form com todos os botões que exigem campos desativados
            btnNovo.Enabled = false;
            btnExcluir.Enabled = false;
            btnEditar.Enabled = false;
            btnConsultar.Enabled = false;
        }

        private void txtID_CPF_TextChanged(object sender, EventArgs e)
        {
            // Habilita o botão Novo somente se os campos Nome e CPF estiverem preenchidos
            btnNovo.Enabled =
                !string.IsNullOrEmpty(txtNome.Text) &&
                !string.IsNullOrEmpty(txtCPF.Text) ? true : false;
        }

        private void NomeCPFID_TextChanged(object sender, EventArgs e)
        {
            // Habilita os botões Editar, Exluir e Consultar somente se o campo 
            // ID estiver preenchido
            btnEditar.Enabled =
                !string.IsNullOrEmpty(txtNome.Text) &&
                !string.IsNullOrEmpty(txtCPF.Text) &&
                !string.IsNullOrEmpty(txtID.Text) ? true : false;

            btnExcluir.Enabled =
                !string.IsNullOrEmpty(txtID.Text) ? true : false;

            btnConsultar.Enabled =
                !string.IsNullOrEmpty(txtID.Text) ? true : false;
        }

        private void btnNovo_Click(object sender, EventArgs e)
        {
            // Funcionalidade do botão NOVO com o comando INSERT
            try
            {
                strSQL =
                    "INSERT INTO tb_clientes (nome, cpf) VALUES (@NOME, @CPF)";


                comando = new MySqlCommand(strSQL, conexao);
                comando.Parameters.AddWithValue("@NOME", txtNome.Text);
                comando.Parameters.AddWithValue("@CPF", txtCPF.Text);

                conexao.Open();

                comando.ExecuteNonQuery();

                MessageBox.Show("Registro inserido com sucesso");


            }
            catch (Exception r)
            {
                MessageBox.Show(r.Message);
            }
            finally
            {
                exibeDB();
                txtNome.Text = null;
                txtCPF.Text = null;
                txtID.Text = null;
            }
        }

        private void btnEditar_Click(object sender, EventArgs e)
        {
            // Funcionalidade do botão EDITAR com o comando UPDATE
            try
            {
                strSQL =
                    "UPDATE tb_clientes SET nome = @NOME, cpf = @CPF WHERE ID = @ID";


                comando = new MySqlCommand(strSQL, conexao);
                comando.Parameters.AddWithValue("@NOME", txtNome.Text);
                comando.Parameters.AddWithValue("@CPF", txtCPF.Text);
                comando.Parameters.AddWithValue("@ID", txtID.Text);

                conexao.Open();

                comando.ExecuteNonQuery();

                MessageBox.Show("Registro atualizado com sucesso");

                
                
            }
            catch (Exception r)
            {
                MessageBox.Show(r.Message);
            }
            finally
            {
                exibeDB();
                txtNome.Text = null;
                txtCPF.Text = null;
                txtID.Text = null;
            }
        }

        private void btnExcluir_Click(object sender, EventArgs e)
        {
            // Funcionalidade do botão EXCLUIR com o comando DELETE
            try
            {
                strSQL =
                    "DELETE FROM tb_clientes WHERE ID = @ID";


                comando = new MySqlCommand(strSQL, conexao);
                comando.Parameters.AddWithValue("@ID", txtID.Text);

                conexao.Open();

                comando.ExecuteNonQuery();

                MessageBox.Show("Registro excluido com sucesso");
            }
            catch (Exception r)
            {
                MessageBox.Show(r.Message);
            }
            finally
            {
                exibeDB();
                txtNome.Text = null;
                txtCPF.Text = null;
                txtID.Text = null;

            }
        }

        private void btnConsultar_Click(object sender, EventArgs e)
        {
            // Funcionalidade do botão CONSULTAR com o comando SELECT
            try
            {
                strSQL =
                    $"SELECT * FROM tb_clientes WHERE ID = {txtID.Text}";

                da = new MySqlDataAdapter(strSQL, conexao);

                var dt = new DataTable();

                da.Fill(dt);

                dgvDados.DataSource = dt;
            }
            catch (Exception r)
            {
                MessageBox.Show(r.Message);
            }
            finally
            {
                conexao.Close();
                strSQL = null;
                comando = null;
            }
        }
        private void btnExibir_Click(object sender, EventArgs e)
        {
            exibeDB();
        }

        private void exibeDB()
        {
            // Funcionalidade de exibir toda a tabela de clientes com o comando SELECT
            // Armazenei em uma função para poder reaproveitar nos outros campos, mostrando
            // a tabela atualizada após criar, editar ou excluir registros
            try
            {
                strSQL =
                    "SELECT * FROM tb_clientes";


                da = new MySqlDataAdapter(strSQL, conexao);

                var dt = new DataTable();

                da.Fill(dt);

                dgvDados.DataSource = dt;
            }
            catch (Exception r)
            {
                MessageBox.Show(r.Message);
            }
            finally
            {
                conexao.Close();
                strSQL = null;
                comando = null;
            }
        }

        private void btnLimpar_Click(object sender, EventArgs e)
        {
            // Funcionalidade do botão LIMPAR com o comando TRUNCATE,
            // possui uma message box com confirmação por segurança
            var limpaSN = MessageBox.Show
                ("Deseja limpar a tabela de clientes?", "ATENÇÃO!", MessageBoxButtons.YesNo);

            if (limpaSN == DialogResult.Yes)
            {
                try
                {
                    strSQL =
                        "TRUNCATE tb_clientes";

                    comando = new MySqlCommand(strSQL, conexao);

                    conexao.Open();

                    comando.ExecuteNonQuery();

                    MessageBox.Show("Tabela limpa com sucesso");
                }
                catch (Exception r)
                {
                    MessageBox.Show(r.Message);
                }
                finally
                {
                    exibeDB();
                }
            }
        }
    }
}

