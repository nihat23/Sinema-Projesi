using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

using System.Data.OleDb;//kutuphanemizi ekleyelim;

namespace Sineme_Otomasyonu
{
    public partial class Film : Form
    {
        public Film()
        {
            InitializeComponent();
        }
        OleDbConnection baglan = new OleDbConnection("Provider=Microsoft.ACE.OLEDB.12.0;Data Source=SinemaBilgiler.accdb");
        OleDbDataReader oku;
        OleDbCommand komutver;

        bool kontrol = false;


        private void button1FilmEkle_Click(object sender, EventArgs e)
        {
            if (pictureBox1.ImageLocation != null && textBox1Adi.Text != "" && textBox2Yonetmen.Text != "" && textBox3Turu.Text != "")
            {
                baglan.Open();
                komutver = new OleDbCommand("select *from Film where FilmAdi='" + textBox1Adi.Text + "'", baglan);
                oku = komutver.ExecuteReader();
                while (oku.Read())
                {
                    kontrol = true;
                    MessageBox.Show(textBox1Adi.Text + " adında film daha önce kaydedilmiş..", "Bilgilendirme", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                if (kontrol == false)
                {
                    komutver = new OleDbCommand("insert into Film(FilmAdi,Yonetmen,FilmTuru,Tarih,Resim) values(@p1,@p2,@p3,@p4,@p5)", baglan);
                    komutver.Parameters.AddWithValue("@p1", textBox1Adi.Text);
                    komutver.Parameters.AddWithValue("@p2", textBox2Yonetmen.Text);
                    komutver.Parameters.AddWithValue("@p3", textBox3Turu.Text);
                    komutver.Parameters.AddWithValue("@p4", DateTime.Now.ToString());
                    komutver.Parameters.AddWithValue("@p5", textBox1Resim.Text);
                    komutver.ExecuteNonQuery();
                    MessageBox.Show("Film Başarıyla Eklendi..", "Bilgilendirme", MessageBoxButtons.OK, MessageBoxIcon.Information);

                    pictureBox1.ImageLocation = null;
                     

                    foreach (Control item in Controls)
                    {
                        if (item is TextBox)
                        {
                            item.Text = "";
                        }
                    }
                }
                baglan.Close();
            }
            else
            {
                MessageBox.Show("Lütfen Alanları Boş Geçmeyiniz...!!", "Uyarı", MessageBoxButtons.OK, MessageBoxIcon.Hand);
            }

        }

        private void button1ResimSec_Click(object sender, EventArgs e)
        {
            OpenFileDialog resim = new OpenFileDialog();
            resim.Filter = "Resim Dosyaları(*.JPG,*PNG,*.BMP,*JPEG)| *.jpg; *.png; *.bmp; *.jpeg";

            if (resim.ShowDialog() == DialogResult.OK)
            {
                pictureBox1.ImageLocation = resim.FileName;
                textBox1Resim.Text = resim.FileName;
            }
        }
    }
}
