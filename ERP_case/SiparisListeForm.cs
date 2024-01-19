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
    public partial class SiparisListeForm : Form
    {
        private int siraCounter = 1;
        private int columnWidth = 155;
        private string stockDetailsFilePath = "Stock Details.txt";
        private string ordersFilePath = "Orders.txt";
        private SiparisKayıtlarıForm siparisKayıtlarıForm;

        private void SiparisListeForm_Load(object sender, EventArgs e)
        {
        }

        private void label3_Click(object sender, EventArgs e)
        {
        }

        private void label7_Click(object sender, EventArgs e)
        {
        }

        public SiparisListeForm(string evrakNo, SiparisKayıtlarıForm _siparisKayıtlarıForm)
        {
            InitializeComponent();
            siparisKayıtlarıForm = _siparisKayıtlarıForm;
            dataGridView1.AllowUserToAddRows = false;
            dataGridView1.CellValueChanged += DataGridView1_CellValueChanged;
            dataGridView1.Columns.Add("ColumnSıra", "Sıra");
            dataGridView1.Columns.Add("ColumnStokAdı", "Stok Adı");
            dataGridView1.Columns.Add("ColumnStokKodu", "Stok Kodu");
            dataGridView1.Columns.Add("ColumnBirimFiyat", "Birim Fiyat");
            dataGridView1.Columns.Add("ColumnMiktar", "Miktar");
            dataGridView1.Columns.Add("ColumnAraToplam", "Ara Toplam");
            dataGridView1.Columns["ColumnSıra"].Width = columnWidth;
            dataGridView1.Columns["ColumnStokAdı"].Width = columnWidth;
            dataGridView1.Columns["ColumnStokKodu"].Width = columnWidth;
            dataGridView1.Columns["ColumnBirimFiyat"].Width = columnWidth;
            dataGridView1.Columns["ColumnMiktar"].Width = columnWidth;
            dataGridView1.Columns["ColumnAraToplam"].Width = columnWidth;
            foreach (DataGridViewColumn column in dataGridView1.Columns)
            {
                if (column.Name != "ColumnStokKodu" && column.Name != "ColumnMiktar")
                {
                    column.ReadOnly = true;
                }
            }
            if (evrakNo != "")
            {
                txtEvrakNo.Text = evrakNo;
                FillDataGridViewFromOrderFile($"Order{evrakNo}.txt");
            }
        }

        private void FillDataGridViewFromOrderFile(string orderFilePath)
        {
            if (File.Exists(orderFilePath))
            {
                string[] orderLines = File.ReadAllLines(orderFilePath);
                int siraCounter = 1;
                foreach (string orderDetailLine in orderLines)
                {
                    string[] orderDetailValues = orderDetailLine.Split(',');
                    string stokAdi = orderDetailValues[0].Split(':')[1].Trim();
                    string stokKodu = orderDetailValues[1].Split(':')[1].Trim();
                    string birimFiyat = orderDetailValues[2].Split(':')[1].Trim();
                    string miktar = orderDetailValues[3].Split(':')[1].Trim();
                    dataGridView1.Rows.Add(
                        siraCounter,
                        stokAdi,
                        stokKodu,
                        birimFiyat,
                        miktar,
                        0
                    );
                    siraCounter++;
                }
                UpdateAraToplam();
            }
        }

        private void DataGridView1_CellValueChanged(object sender, DataGridViewCellEventArgs e)
        {
            if (e.ColumnIndex == dataGridView1.Columns["ColumnStokKodu"].Index && e.RowIndex >= 0)
            {
                string enteredStockCode = dataGridView1.Rows[e.RowIndex].Cells["ColumnStokKodu"].Value?.ToString();
                if (!string.IsNullOrEmpty(enteredStockCode))
                {
                    SearchStockDetails(enteredStockCode);
                }
            }
            string enteredAmount = dataGridView1.Rows[e.RowIndex].Cells["ColumnMiktar"].Value?.ToString();
            if (!string.IsNullOrEmpty(enteredAmount))
                UpdateAraToplam();
        }

        private void SearchStockDetails(string stockCode)
        {
            if (File.Exists(stockDetailsFilePath))
            {
                string[] stockDetailsLines = File.ReadAllLines(stockDetailsFilePath);
                foreach (string stockDetailLine in stockDetailsLines)
                {
                    string[] stockDetailValues = stockDetailLine.Split(',');
                    if (stockDetailValues.Length >= 3 && stockDetailValues[0].Trim() == stockCode)
                    {
                        dataGridView1.Rows[dataGridView1.CurrentRow.Index].Cells["ColumnStokKodu"].Value = stockDetailValues[0].Trim();
                        dataGridView1.Rows[dataGridView1.CurrentRow.Index].Cells["ColumnStokAdı"].Value = stockDetailValues[1].Trim();
                        dataGridView1.Rows[dataGridView1.CurrentRow.Index].Cells["ColumnBirimFiyat"].Value = decimal.Parse(stockDetailValues[2].Trim());
                        break;
                    }
                }
            }
        }

        private void UpdateAraToplam()
        {
            decimal toplam = 0;
            foreach (DataGridViewRow row in dataGridView1.Rows)
            {
                if (row.Cells["ColumnBirimFiyat"].Value != null && row.Cells["ColumnMiktar"].Value != null)
                {
                    decimal birimFiyat = Convert.ToDecimal(row.Cells["ColumnBirimFiyat"].Value);
                    int miktar = Convert.ToInt32(row.Cells["ColumnMiktar"].Value);
                    decimal araToplam = birimFiyat * miktar;
                    row.Cells["ColumnAraToplam"].Value = araToplam;
                    toplam += araToplam;
                }
            }
            labelToplam.Text = $"{toplam:C}";
        }

        private void btnYeniSatır_Click(object sender, EventArgs e)
        {
            int rowIndex = dataGridView1.Rows.Add("", "", "", "", "", 0);
            dataGridView1.Rows[rowIndex].Cells["ColumnSıra"].Value = siraCounter++;
        }

        private void btnKaydet_Click(object sender, EventArgs e)
        {
            if (!string.IsNullOrEmpty(txtEvrakNo.Text))
            {
                if (dateTimePicker1.Value != DateTime.MinValue)
                {
                    SaveOrderDetails();
                    MessageBox.Show("Sipariş kaydedildi!", "Başarı", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    siparisKayıtlarıForm.PopulateOrdersDataGridView();
                }
                else
                {
                    MessageBox.Show("Lütfen bir tarih seçin!", "Hata", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            else
            {
                MessageBox.Show("Lütfen bir Evrak No girin!", "Hata", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void SaveOrderDetails()
        {
            string orderFileName = $"Order{txtEvrakNo.Text}.txt";
            int evrakNo = int.Parse(txtEvrakNo.Text);
            if (File.Exists(orderFileName))
            {
                File.Delete(orderFileName);
            }
            using (StreamWriter orderWriter = new StreamWriter(orderFileName, true))
            {
                foreach (DataGridViewRow row in dataGridView1.Rows)
                {
                    orderWriter.WriteLine(
                        $"Stok Adı:{row.Cells["ColumnStokAdı"].Value}, " +
                        $"Stok Kodu:{row.Cells["ColumnStokKodu"].Value}, " +
                        $"Birim Fiyat:{row.Cells["ColumnBirimFiyat"].Value}, " +
                        $"Miktar:{row.Cells["ColumnMiktar"].Value}"
                    );
                }
            }
            if (File.Exists(ordersFilePath))
            {
                string[] orderLines = File.ReadAllLines(ordersFilePath);
                List<string> updatedOrderLines = new List<string>();
                //orders dosyasını okuyup yeniden yaratır. aynı evrak no'ya sahip bir girdi varsa o girdiyi
                //atlar. Yeni order'ı en sona ekler
                foreach (string line in orderLines)
                {                   
                    if (line.Contains($"Evrak No:{txtEvrakNo.Text}"))
                    {
                        continue;
                    }
                    updatedOrderLines.Add(line);
                }
                File.WriteAllLines(ordersFilePath, updatedOrderLines.ToArray());
            }
            using (StreamWriter ordersWriter = new StreamWriter(ordersFilePath, true))
            {
                ordersWriter.WriteLine(
                    $"Evrak No:{txtEvrakNo.Text}, " +
                    $"Tarih:{dateTimePicker1.Value.ToShortDateString()}, " +
                    $"Toplam:{labelToplam.Text}"
                );
            }
        }

        private void txtEvrakNo_TextChanged(object sender, EventArgs e)
        {
        }

        private void dateTimePicker1_ValueChanged(object sender, EventArgs e)
        {
        }

        private void labelToplam_Click(object sender, EventArgs e)
        {
        }

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
        }
    }
}
