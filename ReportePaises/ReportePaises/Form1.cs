using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Microsoft.Reporting.WinForms;
using MySql.Data;
using MySql.Data.MySqlClient;
namespace ReportePaises
{
    /// <summary>
    /// Clase parcial del formulario
    /// </summary>
    public partial class Form1 : Form
    {
        /// <summary>
        /// metodo que inicia los componentes.
        /// </summary>
        public Form1()
        {
            InitializeComponent();
        }
        /// <summary>
        /// metodo que carga el formulario con su compenentes. En este caso rellenar los datos dentro del rdlc.
        /// </summary>
        /// <param name="sender">El objeto que desencadenó el evento de carga del formulario.</param>
        /// <param name="e">Los argumentos del evento de carga del formulario.</param>
        private void Form1_Load(object sender, EventArgs e)
        {
            MySqlConnection conn = getConnection();
            try
            {
                conn.Open();// abre la conexion

                //inserta el select dentro del MySqlCommand.
                String sql = "Select code, name, continent From country";
                MySqlCommand cmd = new MySqlCommand();
                cmd.Connection = conn;
                cmd.CommandText = sql;

                //crea un MySqlReader y crea una lista para guardar los datos del select
                MySqlDataReader rdr = cmd.ExecuteReader();
                List<ReportPaises> lrp = new List<ReportPaises>();

                //crea un bucle while en donde leera cada columna y lo introducira al array o lista
                while (rdr.Read())
                {
                    ReportPaises rp = new ReportPaises();
                    rp.code = rdr[0].ToString();
                    rp.name = rdr[1].ToString();
                    rp.continent = rdr[2].ToString();
                    lrp.Add(rp);
                    rp = null;
                }
                //cierra la conexion
                rdr.Close();

                //creamos un objeto con reportDataSource la cuales contiene las tablas y diseño. Ademas de pasarle tambien los datos.
                ReportDataSource rds = new ReportDataSource("ReportePaises",lrp);

                //insertamos en el reportview el diseño del ReportDataSource y actualizamos.
                this.reportViewer1.LocalReport.ReportEmbeddedResource = "ReportePaises.Report1.rdlc";
                this.reportViewer1.LocalReport.DataSources.Clear();
                this.reportViewer1.LocalReport.DataSources.Add(rds);
                this.reportViewer1.RefreshReport();
            }
            catch (Exception ex)//recoge el error.
            {
                MessageBox.Show(ex.ToString());
            }
            conn.Close();
        }
        /// <summary>
        /// Metodo que manda envia un MySqlConnection con los datos puesto. para evitar ponerlo varias veces
        /// </summary>
        /// <returns> devuelve un MySqlConnection con los datos puesto.</returns>
        private MySqlConnection getConnection()
        {
            String connstr = "server=localhost;user=root;password='';database=world;port=3306";
            MySqlConnection conn = new MySqlConnection(connstr);
            return conn;

        }
    }
}
