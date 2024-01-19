using System;
using System.IO;
using System.Linq;
using System.Windows.Forms;
using System.Collections.Generic;

namespace ERP_case
{
    public partial class StokKartListeForm : Form
    {
        private string filePath = "Stock Details.txt";
        private string selectedBookName;

        public StokKartListeForm()
        {
            InitializeComponent();
            LoadStockDetails();
        }

        private void StokKartListe_Load(object sender, EventArgs e)
        {

        }

        private void panel1_Paint(object sender, PaintEventArgs e)
        {

        }

        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void label2_Click(object sender, EventArgs e)
        {

        }

        private void label3_Click(object sender, EventArgs e)
        {

        }

        private void txtStokKodu_TextChanged(object sender, EventArgs e)
        {

        }

        private void txtStokAdı_TextChanged(object sender, EventArgs e)
        {

        }

        private void numBirimFiyat_ValueChanged(object sender, EventArgs e)
        {

        }

        private void listBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (listBox1.SelectedItem != null)
            {
                selectedBookName = listBox1.SelectedItem.ToString();
                string[] lines = File.ReadAllLines(filePath);

                foreach (string line in lines)
                {
                    // Skip empty lines
                    if (string.IsNullOrWhiteSpace(line))
                    {
                        continue;
                    }

                    string[] values = line.Split(',');
                    string bookName = values[1].Trim();

                    if (bookName == selectedBookName)
                    {
                        txtStokKodu.Text = values[0].Trim();
                        txtStokAdı.Text = bookName;
                        numBirimFiyat.Value = int.Parse(values[2].Trim());
                        break;
                    }
                }
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (!int.TryParse(txtStokKodu.Text, out _) || string.IsNullOrEmpty(txtStokAdı.Text) || numBirimFiyat.Value <= 0)
            {
                MessageBox.Show("Hatalı girdi!", "Hata", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            else
            {
                if (!File.Exists(filePath))
                {
                    File.Create(filePath).Close();
                }

                // Read all lines from the file
                string[] lines = File.ReadAllLines(filePath);

                // Create a list to store the updated lines
                List<string> updatedLines = new List<string>();

                bool entryUpdated = false;

                // Iterate through each line
                foreach (string line in lines)
                {
                    string[] values = line.Split(',');

                    // Extract the second value (book name) and trim whitespaces
                    string bookName = values[1].Trim();

                    // If the current line corresponds to the selected book name, update the values
                    if (listBox1.SelectedItem != null && bookName == listBox1.SelectedItem.ToString())
                    {
                        // Update the line with the new values
                        updatedLines.Add($"{txtStokKodu.Text},{txtStokAdı.Text},{numBirimFiyat.Value}");
                        entryUpdated = true;
                    }
                    else
                    {
                        // Keep the existing line unchanged
                        updatedLines.Add(line);
                    }
                }

                // If no entry was updated, it means there was no selection, so add the new data
                if (!entryUpdated)
                {
                    updatedLines.Add($"{txtStokKodu.Text},{txtStokAdı.Text},{numBirimFiyat.Value}");
                }

                // Write the updated lines back to the file
                File.WriteAllLines(filePath, updatedLines.ToArray());

                MessageBox.Show("Kayıt kaydedildi!", "Başarı", MessageBoxButtons.OK, MessageBoxIcon.Information);

                // Clear the form after saving
                ClearForm();
                LoadStockDetails();
            }
        }



        private void LoadStockDetails()
        {
            if (!File.Exists(filePath))
            {
                File.Create(filePath).Close();
            }

            listBox1.Items.Clear();
            string[] lines = File.ReadAllLines(filePath);

            foreach (string line in lines)
            {
                string[] values = line.Split(',');

                if (values.Length >= 2)
                {
                    string bookName = values[1].Trim();
                    listBox1.Items.Add(bookName);
                }
            }
        }



        private void ClearForm()
        {
            txtStokKodu.Text = "";
            txtStokAdı.Text = "";
            numBirimFiyat.Value = 0;
        }

        private void button1_Click_1(object sender, EventArgs e)
        {
            // Clear the selection in listBox1
            listBox1.ClearSelected();

            // Clear the form
            ClearForm();          
        }
    }
}
