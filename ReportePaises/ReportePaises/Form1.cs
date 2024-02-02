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
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            String connstr = "server=localhost;user=root;password='';database=world;port=3306";
            MySqlConnection conn = new MySqlConnection(connstr);
            try
            {
                conn.Open();
                String sql = "Select code, name, continent From country";
                MySqlCommand cmd = new MySqlCommand();
                cmd.Connection = conn;
                cmd.CommandText = sql;
                MySqlDataReader rdr = cmd.ExecuteReader();
                List<ReportPaises> lrp = new List<ReportPaises>();
                while (rdr.Read())
                {
                    ReportPaises rp = new ReportPaises();
                    rp.code = rdr[0].ToString();
                    rp.name = rdr[1].ToString();
                    rp.continent = rdr[2].ToString();
                    lrp.Add(rp);
                    rp = null;
                }
                rdr.Close();
                ReportDataSource rds = new ReportDataSource("ReportePaises",lrp);
                this.reportViewer1.LocalReport.ReportEmbeddedResource = "ReportePaises.Report1.rdlc";
                this.reportViewer1.LocalReport.DataSources.Clear();
                this.reportViewer1.LocalReport.DataSources.Add(rds);
                this.reportViewer1.RefreshReport();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
            conn.Close();
        }
    }
}
