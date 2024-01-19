using System;
using System.Windows.Forms;
using System;
using System.IO;
using System.Windows.Forms;

namespace ERP_case
{
    public partial class StokKartListeForm : Form
    {
        public StokKartListeForm()
        {
            InitializeComponent();
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

        private void button1_Click(object sender, EventArgs e) // kaydetme butonu
        {
            if(!int.TryParse(txtStokKodu.Text, out _) || string.IsNullOrEmpty(txtStokAdı.Text) || numBirimFiyat.Value <= 0)
            {
                MessageBox.Show("Hatalı girdi!", "Hata", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            else
            {
                // Specify the path to the text file
                string filePath = "StockDetails.txt";

                // Create or append to the text file and write the stock details
                using (StreamWriter writer = new StreamWriter(filePath, true))
                {
                    writer.WriteLine($"Stok Kodu: {txtStokKodu.Text}, Stock Name: {txtStokAdı.Text}, Unit Price: {numBirimFiyat.Value:C}");
                }

               
                MessageBox.Show("Kayıt kaydedildi!", "Başarı", MessageBoxButtons.OK, MessageBoxIcon.Information);

                txtStokKodu.Text = "";
                txtStokAdı.Text = "";
                numBirimFiyat.Value = 0;

            }

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
    }
}
