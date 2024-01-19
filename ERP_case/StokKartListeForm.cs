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

        private void listBox1_SelectedIndexChanged(object sender, EventArgs e) //listeden girdi seçilmesi
        {
            if (listBox1.SelectedItem != null)
            {
                selectedBookName = listBox1.SelectedItem.ToString();
                string[] lines = File.ReadAllLines(filePath);

                foreach (string line in lines)
                {
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

        /// <summary>
        /// Stok girdilerini açar. Bütün satırları aynı dosyaya yeniden yazar. 
        /// Eğer yeni girdideki kitap ismiyle stok girdilerindeki bir kitap ismi aynı ise
        /// o girdinin satırını atlar ve yeni girdiyi ekler
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void button1_Click(object sender, EventArgs e) //girdi kaydet
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

                string[] lines = File.ReadAllLines(filePath);
                List<string> updatedLines = new List<string>();
                bool entryUpdated = false;

                foreach (string line in lines)
                {
                    string[] values = line.Split();
                    string bookName = values[1].Trim();
           
                    if (listBox1.SelectedItem != null && bookName == listBox1.SelectedItem.ToString())
                    {
                        updatedLines.Add($"{txtStokKodu.Text},{txtStokAdı.Text},{numBirimFiyat.Value}");
                        entryUpdated = true;
                    }
                    else
                    {
                        updatedLines.Add(line);
                    }
                }

                if (!entryUpdated)
                {
                    updatedLines.Add($"{txtStokKodu.Text},{txtStokAdı.Text},{numBirimFiyat.Value}");
                }

                File.WriteAllLines(filePath, updatedLines.ToArray());

                MessageBox.Show("Kayıt kaydedildi!", "Başarı", MessageBoxButtons.OK, MessageBoxIcon.Information);
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
            listBox1.ClearSelected();
            ClearForm();
        }
    }
}
