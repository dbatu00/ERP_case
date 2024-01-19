using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;

namespace ERP_case
{
    public partial class SiparisKayıtlarıForm : Form
    {

        public SiparisKayıtlarıForm()
        {
            InitializeComponent();
            // Populate DataGridView with orders from Orders.txt
            PopulateOrdersDataGridView();
            dataGridView1.AllowUserToAddRows = false;
            dataGridView1.CellDoubleClick += dataGridView1_CellDoubleClick;
        }

        internal void PopulateOrdersDataGridView()
        {
            // Read all lines from the Orders.txt file
            string[] orderLines = File.ReadAllLines("Orders.txt");

            // Clear existing rows and columns in the DataGridView
            dataGridView1.Rows.Clear();
            dataGridView1.Columns.Clear();

            // Add headers to the DataGridView
            dataGridView1.Columns.Add("EvrakNoColumn", "Evrak No");
            dataGridView1.Columns.Add("TarihColumn", "Tarih");
            dataGridView1.Columns.Add("ToplamColumn", "Toplam");

            // Iterate through each line
            foreach (string line in orderLines)
            {
                // Split the line based on comma
                string[] orderDetails = line.Split(',');

                // Extract Evrak No, Tarih, and Toplam values
                string evrakNo = orderDetails[0].Split(':')[1].Trim();
                string tarih = orderDetails[1].Split(':')[1].Trim();
                string toplam = orderDetails[2].Split(':')[1].Trim();

                // Add a new row to the DataGridView
                dataGridView1.Rows.Add(evrakNo, tarih, toplam);
            }
        }


        private void Form1_Load(object sender, EventArgs e)
        {

        }

        private void button2_Click(object sender, EventArgs e) // stok kart liste
        {
            // Create an instance of the StokKartListeForm
            StokKartListeForm stokKartListeForm = new StokKartListeForm();

            // Show the new form
            stokKartListeForm.Show();
        }

        private void button1_Click(object sender, EventArgs e) // yeni sipariş ekle
        {
            // Create an instance of the StokKartListeForm

            SiparisListeForm siparisListeForm = new SiparisListeForm("", this);

            // Show the new form
            siparisListeForm.Show();
        }

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }

        private void dataGridView1_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            // Check if a valid cell is double-clicked
            if (e.RowIndex >= 0 && e.ColumnIndex >= 0)
            {
                // Get the Evrak No value from the clicked row
                string evrakNo = dataGridView1.Rows[e.RowIndex].Cells["EvrakNoColumn"].Value?.ToString();

                // If Evrak No is not null or empty, open SiparisListeForm with the corresponding Evrak No
                if (!string.IsNullOrEmpty(evrakNo))
                {
                    SiparisListeForm siparisListeForm = new SiparisListeForm(evrakNo, this);
                    siparisListeForm.Show();
                }
            }
        }

    }
}
