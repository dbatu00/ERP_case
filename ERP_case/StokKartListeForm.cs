using System;
using System.IO;
using System.Linq;
using System.Windows.Forms;

namespace ERP_case
{
    public partial class StokKartListeForm : Form
    {
        private string filePath = "StockDetails.txt";

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
            // Check if any item is selected in the ListBox
            if (listBox1.SelectedItem != null)
            {
                // Get the selected item (book name)
                string selectedBookName = listBox1.SelectedItem.ToString();

                // Read all lines from the file
                string[] lines = File.ReadAllLines(filePath);

                // Iterate through each line
                foreach (string line in lines)
                {
                    // Split the line by commas
                    string[] values = line.Split(',');

                    // Extract the second value (book name) and trim whitespaces
                    string bookName = values[1].Trim();

                    // If the current line corresponds to the selected book name, fill the textboxes
                    if (bookName == selectedBookName)
                    {
                        // Extract and fill the textboxes with the details
                        txtStokKodu.Text = values[0].Trim();
                        txtStokAdı.Text = bookName;

                        // Set the numeric value directly as an integer
                        numBirimFiyat.Value = int.Parse(values[2].Trim());

                        // Break the loop once the details are found
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
                // Append stock details to the text file
                using (StreamWriter writer = new StreamWriter(filePath, true))
                {
                    writer.WriteLine($"{txtStokKodu.Text},{txtStokAdı.Text},{numBirimFiyat.Value}");
                }

                MessageBox.Show("Kayıt kaydedildi!", "Başarı", MessageBoxButtons.OK, MessageBoxIcon.Information);

                // Clear the form after saving
                ClearForm();

                // Reload stock details to update the ListBox
                LoadStockDetails();
            }
        }

        private void LoadStockDetails()
        {
            // Check if the file exists
            if (!File.Exists(filePath))
            {
                // If the file doesn't exist, create it
                File.Create(filePath).Close();
            }

            // Clear the existing items in the ListBox
            listBox1.Items.Clear();

            // Read all lines from the file
            string[] lines = File.ReadAllLines(filePath);
            
            // Iterate through each line
            foreach (string line in lines)
            {
                // Split the line by commas
                string[] values = line.Split(',');

                // Ensure there are at least two values
                if (values.Length >= 2)
                {
                    // Trim leading and trailing whitespaces from the second value
                    string bookName = values[1].Trim();

                    // Add the stock name to the ListBox
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

      

   
    }
}
