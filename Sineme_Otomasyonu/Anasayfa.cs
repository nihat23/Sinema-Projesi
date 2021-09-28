using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

using System.Data.OleDb;//ktuphanemizi ekledik..

namespace Sineme_Otomasyonu
{
    public partial class Anasayfa : Form
    {
        public Anasayfa()
        {
            InitializeComponent();
        }




        OleDbConnection baglan = new OleDbConnection("Provider=Microsoft.ACE.OLEDB.12.0;Data Source=SinemaBilgiler.accdb");
        OleDbDataAdapter liste_oku;
        OleDbCommand komutver;
        OleDbDataReader oku;
        DataTable tablo;


        void listele()
        {
            baglan.Open();
            liste_oku = new OleDbDataAdapter("select SatisId as[Sıra],KoltukNo as [Koltuk No],SalonAdi as [Salon Adı],FilmSaati as [Film Saati],FilmAdi as [Film Adı],Tarih as [Tarih],AdiSoyadi as [Adı Soyadı],Ucret as [Ücret] from Satıs ", baglan);
            tablo = new DataTable();
            liste_oku.Fill(tablo);
            dataGridView1.DataSource = tablo;
            baglan.Close();
        }
        public void combocek()
        {
            baglan.Open();
            komutver = new OleDbCommand("select * from Film", baglan);
            oku = komutver.ExecuteReader();
            while (oku.Read())
            {
                comboBox1F_Adi.Items.Add(oku["FilmAdi"].ToString());
            }
            baglan.Close();

        }
        int sayac = 0;
        void salonlar()
        {

            for (int i = 0; i <= 14; i++)
            {
                sayac++;
                comboBox2S_Adi.Items.Add("Salon " + sayac);
            }
        }

        int sayi = 0;
        void tuslar()
        {

            for (int i = 0; i < 8; i++)
            {
                for (int j = 0; j < 8; j++)
                {

                    Button Btn = new Button();
                    Btn.Size = new Size(40, 40);
                    Btn.BackColor = Color.White;
                    Btn.Location = new Point(j * 40 + 10, i * 40 + 10);
                    if (j == 4)
                    {
                        continue;
                    }
                    sayi++;
                    Btn.Name = sayi.ToString();
                    Btn.Text = sayi.ToString();
                    panel1.Controls.Add(Btn);
                    Btn.Click += Btn_Click;
                }
            }
        }

        private void Btn_Click(object sender, EventArgs e)
        {
            Button Btn = (Button)sender;

            textBox1K_No.Text = ((Button)sender).Text;
            

        }

       

        private void Form1_Load(object sender, EventArgs e)
        {
            listele();
            combocek();
            salonlar();
            tuslar();



        }
        private void button2FilmEkle_Click(object sender, EventArgs e)
        {
            Film film = new Film();
            film.ShowDialog();
        }
        private void button2BiletSat_Click(object sender, EventArgs e)
        {
            try
            {
                baglan.Open();
                komutver = new OleDbCommand("insert into Satıs(KoltukNo,SalonAdi,FilmSaati,FilmAdi,Tarih,AdiSoyadi,Ucret) values(@p1,@p2,@p3,@p4,@p5,@p6,@p7)", baglan);
                komutver.Parameters.AddWithValue("@p1", int.Parse(textBox1K_No.Text));
                komutver.Parameters.AddWithValue("@p2", comboBox2S_Adi.SelectedItem);
                komutver.Parameters.AddWithValue("@p3", comboBox3F_Saati.Text);
                komutver.Parameters.AddWithValue("@p4", comboBox1F_Adi.SelectedItem);
                komutver.Parameters.AddWithValue("@p5", dateTimePicker1.Text);
                komutver.Parameters.AddWithValue("@p6", textBox2A_Soyadi.Text);
                komutver.Parameters.AddWithValue("@p7", double.Parse(textBox3Ücret.Text));
                komutver.ExecuteNonQuery();
                baglan.Close();
                MessageBox.Show("Bilgiler Başarıyla Kaydedildi..", "Bilgilendirme", MessageBoxButtons.OK, MessageBoxIcon.Information);
                listele();

                foreach (Control item in Controls)
                {
                    if (item is TextBox || item is ComboBox)
                    {
                        item.Text = "";
                    }
                }
            }
            catch (Exception)
            {

                MessageBox.Show("Lütfen Dogru Deger girdiginizden Emin olun ..", "Uyarı", MessageBoxButtons.OK, MessageBoxIcon.Hand);
            }


        }

        private void button1BiletIptal_Click(object sender, EventArgs e)
        {
            if (int.Parse(label10.Text) > 0)
            {
                DialogResult yanit = MessageBox.Show("Filmi Silmek istediginizden Eminmisiniz..", "Uyarı", MessageBoxButtons.YesNo, MessageBoxIcon.Question);

                if (yanit == DialogResult.Yes)
                {
                    baglan.Open();
                    komutver = new OleDbCommand("delete from Satıs where SatisId=@id", baglan);
                    komutver.Parameters.AddWithValue("@id", dataGridView1.CurrentRow.Cells[0].Value);
                    komutver.ExecuteNonQuery();
                    baglan.Close();
                    MessageBox.Show("Film Başarıyla Silindi..", "Uyarı", MessageBoxButtons.OK, MessageBoxIcon.Hand);
                    listele();
                    label10.Text = "0";


                }
                else if (yanit == DialogResult.No)
                {
                    listele();
                    baglan.Close();
                    label10.Text = "0";
                }
            }
            else
            {
                MessageBox.Show("Silinecek Satırı Seçiniz..", "Uyarı", MessageBoxButtons.OK, MessageBoxIcon.Hand);
            }

        }

        private void dataGridView1_CellClick(object sender, DataGridViewCellEventArgs e)
        {

            label10.Text = dataGridView1.CurrentRow.Cells[0].Value.ToString();

        }

        private void button1Yenile_Click(object sender, EventArgs e)
        {
            Application.Restart();

        }
    }
}

