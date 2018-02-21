using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace YazilimSinamaOdev2
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        bool fareKoyulsunmu = false, peynirKoyulsunmu = false, fareKilit = false, peynirKilit = false,deger = false, kilit = false,peynirBulundumu=false;
        int fareBaslangicX, fareBaslangicY,i,j,yolHucreSayisi=0,zaman= 0,mevcutX, mevcutY;

        ArrayTypedStack st = new ArrayTypedStack(100);
        int[,] genelYol = new int[13, 20];
        int[,] gidilenYol = new int[13, 20];

        private void Form1_Load(object sender, EventArgs e)
        {         
            dataGridView1.ColumnCount = 20;//20 sutun oluşturuldu.
            dataGridView1.RowCount = 13;//13 satır oluşturuldu.
            foreach (DataGridViewRow row in dataGridView1.Rows)
            {
                row.Height = 40;
            }
            for (int i = 0; i < 13; i++)
            {
                for (int j = 0; j < 20; j++)
                {
                    if(i==0 || j==0 || i==12 || j==19)
                    {
                        dataGridView1.Rows[i].Cells[j].Value = Properties.Resources.cit;//köşelere çitler eklendi.
                        dataGridView1.Rows[i].Cells[j].Style.BackColor = Color.White;//çit olacak hücrelerin rengi beyaz olarak belirlendi.

                    }
                    else
                    {
                        dataGridView1.Rows[i].Cells[j].Value = Properties.Resources.duvar;//köşelerde olmayan kalan hücrelere duvar resmi eklendi.
                    }
                }
            }

            for ( i = 0; i < 13; i++)
            {
                for ( j = 0; j < 20; j++)
                {
                    genelYol[i,j] = 1;//Oluşturulan satır sutun sayısına göre dizi oluşturuldu ve tüm hücre değerleri 1 yapıldı.(Hangi hücrelerin yol hangi hücrelerin duvar olduğu bilgisini tutan dizi)
                }
            }

            for (i = 0; i < 13; i++)
            {
                for (j = 0; j < 20; j++)
                {
                    gidilenYol[i, j] = 1;//Oluşturulan satır sutun sayısına göre dizi oluşturuldu ve tüm hücre değerleri 1 yapıldı.(Gidilen yolların işaretleneceği dizi)
                }
            }
        }


        private void dataGridView1_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            int xkoordinat = dataGridView1.CurrentCellAddress.X;
            int ykoordinat = dataGridView1.CurrentCellAddress.Y;
            if (fareKilit == false && fareKoyulsunmu == true)//farekoy butonu basılmışsa ve henüz  fare konulmamışsa 
            {
                if (dataGridView1.Rows[ykoordinat].Cells[xkoordinat].Style.BackColor == Color.Gray)//seçilen hücre yolun üzerindeyse,
                {
                    dataGridView1.Rows[ykoordinat].Cells[xkoordinat].Value = Properties.Resources.fare;
                    fareKilit = true;
                    fareBaslangicX = xkoordinat;//hareket ederken başlayacağı hücrenin x koordinatı
                    fareBaslangicY = ykoordinat;//hareket ederken başlayacağı hücrenin y koordinatı
                }
                else//yolun üzerinde değilse
                {
                    MessageBox.Show("Lütfen fareyi labirent üstüne yerleştiriniz!");
                }
            }
            else if (fareKilit == false && dataGridView1.Rows[ykoordinat].Cells[xkoordinat].Style.BackColor != Color.White && peynirKoyulsunmu == false)//Fare ve peynir koyulmadıysa ve köşelerdeki çitler seçilmediyse  yol eklenebilir(Köşelerdeki çitlerin arka plan rengi beyaz)
            {
                dataGridView1.Rows[ykoordinat].Cells[xkoordinat].Value = Properties.Resources.yol;
                dataGridView1.Rows[ykoordinat].Cells[xkoordinat].Style.BackColor = Color.Gray;//yol olacak hücrelerin rengi gri olarak belirlendi.
                genelYol[ykoordinat, xkoordinat] = 0;//yol olan hücre genelYol dizisinde  olarak işaretlendi.(duvarlar 1 yollar 0)
                gidilenYol[ykoordinat, xkoordinat] = 0;//yol olan hücre gidilenYol dizisinde  olarak işaretlendi.(duvarlar 1 yollar 0)
                yolHucreSayisi++;

            }
            else if (peynirKoyulsunmu == true && peynirKilit == false && dataGridView1.Rows[ykoordinat].Cells[xkoordinat].Style.BackColor != Color.White)//peynir koy butonuna basılmışsa ve çitler seçilmemişse çalışır.
            {
                if (dataGridView1.Rows[ykoordinat].Cells[xkoordinat].Style.BackColor == Color.Gray)//seçilen hücre yolun üzerindeyse,
                {
                    dataGridView1.Rows[ykoordinat].Cells[xkoordinat].Value = Properties.Resources.peynir;//peynir eklenir.
                    peynirKilit = true;
                    dataGridView1.Rows[ykoordinat].Cells[xkoordinat].Style.BackColor = Color.Pink;//peynir olan hücrenin arka plan rengi değişir.
                }
                else//yolun üzerinde değilse
                {
                    MessageBox.Show("Lütfen peyniri labirent üstüne yerleştiriniz!");
                }
            }
            else if (dataGridView1.Rows[ykoordinat].Cells[xkoordinat].Style.BackColor == Color.White)
                MessageBox.Show("Çitlere yol çizemezsiniz!!");
            else
                MessageBox.Show("Peynir veya fare koyduktan sonra yol çizemezsiniz!!");

        }

        private void btnYeniYol_Click(object sender, EventArgs e)
        {
            Application.Restart();
        }

        private void timer2_Tick(object sender, EventArgs e)
        {
            zaman++;
            lblZaman.Text = zaman.ToString() + " sn";
        }

        private void timer1_Tick(object sender, EventArgs e)
        {

            dataGridView1.Rows[fareBaslangicY].Cells[fareBaslangicX].Value = Properties.Resources.fare;//farenin hareketini sağlar.
            if (kilit)
                dataGridView1.Rows[mevcutY].Cells[mevcutX].Value = Properties.Resources.yol;
            int[] stackDizi = new int[2];
            mevcutX = fareBaslangicX;
            mevcutY = fareBaslangicY;
            kilit = true;
            if (genelYol[fareBaslangicY, fareBaslangicX + 1] == 0 && gidilenYol[fareBaslangicY, fareBaslangicX + 1] == 0)//seçilen hücrenin sağı yolsa ve henüz işaretlenmemişse sağına git.
            {
                fareBaslangicX++;
                deger = true;
            }

            else if (genelYol[fareBaslangicY, fareBaslangicX - 1] == 0 && gidilenYol[fareBaslangicY, fareBaslangicX - 1] == 0)//seçilen hücrenin solu yolsa ve henüz işaretlenmemişse soluna git.
            {
                fareBaslangicX--;
                deger = true;
            }
            else if (genelYol[fareBaslangicY + 1, fareBaslangicX] == 0 && gidilenYol[fareBaslangicY + 1, fareBaslangicX] == 0)//seçilen hücrenin aşağısı yolsa ve henüz işaretlenmemişse aşağısına git.
            {
                fareBaslangicY++;
                deger = true;
            }
            else if (genelYol[fareBaslangicY - 1, fareBaslangicX] == 0 && gidilenYol[fareBaslangicY - 1, fareBaslangicX] == 0)//seçilen hücrenin yukarısı yolsa ve henüz işaretlenmemişse yukarısına git.
            {
                fareBaslangicY--;
                deger = true;
            }
            else//hiçbiri değilse sağında solunda aşağısında ve yukarısında gidilecek yol kalmamıştır.Son girilen yol ayrımını stack içinden pop et.gidilenYol dizisinde o hücreye gidildiği için hücreyi işaretle(değeri 0 idi 2 oldu).
            {
                gidilenYol[mevcutY, mevcutX] = 2;
                int[] popDizi = new int[2];
                popDizi = (int[])st.Pop();
                fareBaslangicY = popDizi[0];
                fareBaslangicX = popDizi[1];
                deger = false;
            }

            if (deger)//sağı solu aşağısı veya yukarısı herhangi biri varsa hücreyi stack içine push et.gidilenYol dizisinde o hücreye gidildiği için hücreyi işaretle(değeri 0 idi 1 oldu).
            {
                stackDizi[0] = mevcutY;
                stackDizi[1] = mevcutX;
                st.Push(stackDizi);
                gidilenYol[mevcutY, mevcutX] = 1;
            }


            if (dataGridView1.Rows[mevcutY].Cells[mevcutX].Style.BackColor == Color.Pink)
            {
                timer1.Enabled = false;
                timer2.Enabled = false;
                dataGridView1.Rows[mevcutY].Cells[mevcutX].Value = Properties.Resources.peynirBulundu;
                MessageBox.Show(zaman + " SANİYEDE PEYNİR BULUNDU.");
                peynirBulundumu = true;

            }


        }

       

        private void btnFareYerlestir_Click(object sender, EventArgs e)
        {
            if(yolHucreSayisi>1)
            {
                if(peynirBulundumu==false)
                {
                    fareKoyulsunmu = true;
                    MessageBox.Show("Lütfen Fareyi Yerleştireceğiniz Yeri Seçin.");
                }

               else
                    MessageBox.Show("Aynı yola tekrar fare yerleştiremezsiniz!Lütfen yeni yol çiziniz.");


            }

            else
            {
                MessageBox.Show("Labirent oluşturmadan fare ekleyemezsiniz!Lütfen en az iki hücre seçerek labirent oluşturun!");
            }
   
        }

        private void btnPeynirYerlestir_Click(object sender, EventArgs e)
        {
            if(yolHucreSayisi>1)
            {
                if(fareKilit)
                {
                    if(peynirBulundumu==false)
                    {
                        peynirKoyulsunmu = true;
                        MessageBox.Show("Lütfen Peyniri Yerleştireceğiniz Yeri Seçin.");
                    }
                    else
                        MessageBox.Show("Aynı yola tekrar peynir yerleştiremezsiniz!Lütfen yeni yol çiziniz.");

                }
                else
                    MessageBox.Show("Fareyi yerleştirmeden peyniri yerleştiremezsiniz.");


            }
            else
            {
                MessageBox.Show("Labirent oluşturmadan peynir ekleyemezsiniz!Lütfen en az iki hücre seçerek labirent oluşturun!");
            }
         
        }
    

        private void btnBul_Click(object sender, EventArgs e)
        {
            if (yolHucreSayisi > 1)
            {
                if (peynirKilit && fareKilit)
                {
                    if(peynirBulundumu==false)
                    {
                        timer1.Enabled = true;
                        timer2.Enabled = true;
                    }
                    else
                    MessageBox.Show("Peynir zaten bulundu!");


                }
                else
                    MessageBox.Show("Fare ve peynir yerleştirmeden peyniri bulamazsınız!");
         
            }
            else
                MessageBox.Show("Labirent oluşturmadan peyniri bulamazsınız!Lütfen en az iki hücre seçerek labirent oluşturun!");
           
        }
       
          
           
    }
}
