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
        private int siraCounter = 1; // Counter for Sıra value
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

            // Add columns to the DataGridView
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

            // Make all columns read-only except "Stok Kodu" and "Miktar"
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
            // Check if the order file exists
            if (File.Exists(orderFilePath))
            {
                // Read all lines from the file
                string[] orderLines = File.ReadAllLines(orderFilePath);

                // Initialize the Sıra counter
                int siraCounter = 1;

                // Iterate through each line
                foreach (string orderDetailLine in orderLines)
                {
                    // Split the line by commas
                    string[] orderDetailValues = orderDetailLine.Split(',');

                    // Extract Stok Adı, Stok Kodu, Birim Fiyat, and Miktar values
                    string stokAdi = orderDetailValues[0].Split(':')[1].Trim();
                    string stokKodu = orderDetailValues[1].Split(':')[1].Trim();
                    string birimFiyat = orderDetailValues[2].Split(':')[1].Trim();
                    string miktar = orderDetailValues[3].Split(':')[1].Trim();

                    // Add a new row to the DataGridView
                    dataGridView1.Rows.Add(
                        siraCounter,
                        stokAdi,
                        stokKodu,
                        birimFiyat,
                        miktar,
                        0 // Initialize Ara Toplam with 0
                    );

                    // Increment the Sıra counter
                    siraCounter++;
                }

                // Update the "AraToplam" values
                UpdateAraToplam();
            }
        }

  

        private void DataGridView1_CellValueChanged(object sender, DataGridViewCellEventArgs e)
        {
            // Check if the changed cell is in the "ColumnStokKodu" column
            if (e.ColumnIndex == dataGridView1.Columns["ColumnStokKodu"].Index && e.RowIndex >= 0)
            {
                // Get the entered stock code
                string enteredStockCode = dataGridView1.Rows[e.RowIndex].Cells["ColumnStokKodu"].Value?.ToString();

               
                // Check if the entered stock code is not empty
                if (!string.IsNullOrEmpty(enteredStockCode) )
                {
                    // Search for stock details based on the entered stock code
                    SearchStockDetails(enteredStockCode);
               
                }
            }

            // Get the entered amount
            string enteredAmount = dataGridView1.Rows[e.RowIndex].Cells["ColumnMiktar"].Value?.ToString();


            if (!string.IsNullOrEmpty(enteredAmount))
                UpdateAraToplam();

        }

        private void SearchStockDetails(string stockCode)
        {
            // Check if the stock details file exists
            if (File.Exists(stockDetailsFilePath))
            {
                // Read all lines from the file
                string[] stockDetailsLines = File.ReadAllLines(stockDetailsFilePath);

                // Iterate through each line
                foreach (string stockDetailLine in stockDetailsLines)
                {
                    // Split the line by commas
                    string[] stockDetailValues = stockDetailLine.Split(',');

                    // Check if the first value (stock code) matches the entered stock code
                    if (stockDetailValues.Length >= 3 && stockDetailValues[0].Trim() == stockCode)
                    {
                        // Fill the corresponding fields with stock details
                        dataGridView1.Rows[dataGridView1.CurrentRow.Index].Cells["ColumnStokKodu"].Value = stockDetailValues[0].Trim();
                        dataGridView1.Rows[dataGridView1.CurrentRow.Index].Cells["ColumnStokAdı"].Value = stockDetailValues[1].Trim();
                        dataGridView1.Rows[dataGridView1.CurrentRow.Index].Cells["ColumnBirimFiyat"].Value = decimal.Parse(stockDetailValues[2].Trim());

                        // Exit the loop once a matching stock code is found
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

                    // Accumulate Ara Toplam values for the total
                    toplam += araToplam;
                }
            }

            // Update the "Toplam" label
            labelToplam.Text = $"{toplam:C}";
        }




        private void btnYeniSatır_Click(object sender, EventArgs e)
        {
            int rowIndex = dataGridView1.Rows.Add("", "", "", "", "", 0);
            dataGridView1.Rows[rowIndex].Cells["ColumnSıra"].Value = siraCounter++;
        }

        private void btnKaydet_Click(object sender, EventArgs e)
        {
            // Check if the Evrak No is not empty
            if (!string.IsNullOrEmpty(txtEvrakNo.Text))
            {
                // Check if a date is selected
                if (dateTimePicker1.Value != DateTime.MinValue)
                {
                    // Your logic for saving the order goes here

                    SaveOrderDetails();

                    // Show a success message
                    MessageBox.Show("Sipariş kaydedildi!", "Başarı", MessageBoxButtons.OK, MessageBoxIcon.Information);

                    siparisKayıtlarıForm.PopulateOrdersDataGridView();
            
                }
                else
                {
                    // Show an error message for missing date
                    MessageBox.Show("Lütfen bir tarih seçin!", "Hata", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            else
            {
                // Show an error message for missing Evrak No
                MessageBox.Show("Lütfen bir Evrak No girin!", "Hata", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void SaveOrderDetails()
        {
            // Generate the order file name with Evrak No
            string orderFileName = $"Order{txtEvrakNo.Text}.txt";
            int evrakNo = int.Parse(txtEvrakNo.Text);

            // Check if the file exists
            if (File.Exists(orderFileName))
            {
                // If it exists, delete it
                File.Delete(orderFileName);
            }

            // Create or append to the Order file
            using (StreamWriter orderWriter = new StreamWriter(orderFileName, true))
            {
                foreach (DataGridViewRow row in dataGridView1.Rows)
                {
                    // Write details separated by comma
                    orderWriter.WriteLine(
                        $"Stok Adı:{row.Cells["ColumnStokAdı"].Value}, " +
                        $"Stok Kodu:{row.Cells["ColumnStokKodu"].Value}, " +
                        $"Birim Fiyat:{row.Cells["ColumnBirimFiyat"].Value}, " +
                        $"Miktar:{row.Cells["ColumnMiktar"].Value}"
                    );
                }
            }

            // Check if the Orders.txt file exists
            if (File.Exists(ordersFilePath))
            {
                // Read all lines from the Orders.txt file
                string[] orderLines = File.ReadAllLines(ordersFilePath);

                // Create a list to store the updated lines
                List<string> updatedOrderLines = new List<string>();

                // Iterate through each line
                foreach (string line in orderLines)
                {
                    // Check if the line contains the same Evrak No
                    if (line.Contains($"Evrak No:{txtEvrakNo.Text}"))
                    {
                        // Skip the existing order with the same Evrak No
                        continue;
                    }

                    // Add the line to the updated list
                    updatedOrderLines.Add(line);
                }

                // Write the updated lines back to the Orders.txt file
                File.WriteAllLines(ordersFilePath, updatedOrderLines.ToArray());
            }

            // Create or append to the Orders file
            using (StreamWriter ordersWriter = new StreamWriter(ordersFilePath, true))
            {
                // Write details separated by comma
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
