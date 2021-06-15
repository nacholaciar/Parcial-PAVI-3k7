using ParcialPav.AccesoADatos;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ParcialPav
{
    public partial class AltaEquipo : Form
    {
        public AltaEquipo()
        {
            InitializeComponent();
        }

        private void AltaEquipo_Load(object sender, EventArgs e)
        {
            CargarFecha();
            txtNroNuevoEquipo.Enabled = false;
            tbNombreJugador.Enabled = false;
            ObtenerNumeroEquipo();
            CargarComboCategorias();
            CargarComboPosiciones();

        }

        private void CargarFecha()
        {
            txtFecha.Enabled = false;
            txtFecha.Text = DateTime.Now.ToShortDateString();
        }

        private void ObtenerNumeroEquipo()
        {
            int id = Acceso.NumeroEquipo();
            txtNroNuevoEquipo.Text = (id + 1).ToString();
        }


        private void CargarComboCategorias()
        {
            try
            {
                cmbCategorias.DataSource = Acceso.cargarComboCategorias();
                cmbCategorias.DisplayMember = "Nombre";
                cmbCategorias.ValueMember = "Id";
                cmbCategorias.SelectedIndex = -1;
            }
            catch (Exception)
            {
                MessageBox.Show("error al cargar categorias");
            }

        }

        private void CargarComboPosiciones()
        {
            try
            {
                cmbPosicion.DataSource = Acceso.cargarComboPosiciones();
                cmbPosicion.DisplayMember = "Nombre";
                cmbPosicion.ValueMember = "Id";
                cmbPosicion.SelectedIndex = -1;
            }
            catch (Exception)
            {
                MessageBox.Show("error al cargar posiciones");
            }

        }

        private void btnBuscarJugador_Click(object sender, EventArgs e)
        {
            try
            {
                DataTable tResultado = Acceso.ObtenerNombreJugador(txtNroJugador.Text.Trim());
                if (tResultado.Rows.Count > 0)
                {
                    tbNombreJugador.Text = tResultado.Rows[0][1].ToString();
                }
                else
                {
                    MessageBox.Show("Jugador no encontrado");
                    txtNroJugador.Focus();
                }
            }
            catch (Exception)
            {
                MessageBox.Show("error");
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            dgvEquipos.Rows.Add(txtNroJugador.Text, tbNombreJugador.Text, cmbPosicion.Text); 
        }

        private void button2_Click(object sender, EventArgs e)
        {
            List<int> listJugadores = new List<int>();
            for (int i = 0; i < dgvEquipos.Rows.Count; i++)
            {
                listJugadores.Add(int.Parse(dgvEquipos.Rows[i].Cells[0].Value.ToString()));
            }

            bool resultado = Acceso.AltaNuevoEquipo(int.Parse(txtNroNuevoEquipo.Text), txtNombreDeEquipo.Text, txtFecha.Text.ToString(), listJugadores);

            if (resultado)
            {
                MessageBox.Show("Equipo cargado");
                LimpiarCampos();
            }
            else
            {
                MessageBox.Show("Error al dar alta equipo");
            }
        }

        private void LimpiarCampos()
        {
            txtNroNuevoEquipo.Text = "";
            txtNombreDeEquipo.Text = "";
            txtNroJugador.Text = "";
            tbNombreJugador.Text = "";
            cmbCategorias.SelectedIndex = -1;
            cmbPosicion.SelectedIndex = -1;
        }
    }
}
