using System;
using System.IO;
using System.Windows.Forms;

namespace ERP_case
{
    public partial class SiparisKayıtlarıForm : Form
    {
        public SiparisKayıtlarıForm()
        {
            InitializeComponent();
            PopulateOrdersDataGridView();
            dataGridView1.AllowUserToAddRows = false;
            dataGridView1.CellDoubleClick += dataGridView1_CellDoubleClick;
        }

        internal void PopulateOrdersDataGridView()
        {
            string[] orderLines = File.ReadAllLines("Orders.txt");

            dataGridView1.Rows.Clear();
            dataGridView1.Columns.Clear();

            dataGridView1.Columns.Add("EvrakNoColumn", "Evrak No");
            dataGridView1.Columns.Add("TarihColumn", "Tarih");
            dataGridView1.Columns.Add("ToplamColumn", "Toplam");

            foreach (string line in orderLines)
            {
                string[] orderDetails = line.Split(',');

                string evrakNo = orderDetails[0].Split(':')[1].Trim();
                string tarih = orderDetails[1].Split(':')[1].Trim();
                string toplam = orderDetails[2].Split(':')[1].Trim();

                dataGridView1.Rows.Add(evrakNo, tarih, toplam);
            }
        }

        private void Form1_Load(object sender, EventArgs e)
        {
        }

        private void button2_Click(object sender, EventArgs e) // stok kart liste
        {
            StokKartListeForm stokKartListeForm = new StokKartListeForm();
            stokKartListeForm.Show();
        }

        private void button1_Click(object sender, EventArgs e) // sipariş liste
        {
            SiparisListeForm siparisListeForm = new SiparisListeForm("", this);
            siparisListeForm.Show();
        }

        private void dataGridView1_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0 && e.ColumnIndex >= 0)
            {
                string evrakNo = dataGridView1.Rows[e.RowIndex].Cells["EvrakNoColumn"].Value?.ToString();

                SiparisListeForm siparisListeForm = new SiparisListeForm(evrakNo, this);
                siparisListeForm.Show();
                
            }
        }
    }
}
