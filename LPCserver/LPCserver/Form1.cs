using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Net.Sockets;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Timers;


namespace LPCserver
{
    public partial class Form1 : Form
    {
        Socket socket;
        NetworkStream stream;
        TcpListener listener;
        string kullaniciAdi;
        int süre = 180;
        bool sıraSende = false;
        bool sıraOnda =true;
        bool oyunBitti = false;
       


        public Form1()
        {
            InitializeComponent();
            Control.CheckForIllegalCrossThreadCalls = false;
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            timer1.Start();
        }

        private void startBtn_Click(object sender, EventArgs e)
        {
            Thread serversibaslat = new Thread(fonkserveribaslat);
            serversibaslat.Start();
            listBox1.Items.Add("Oyuncu Bekleniyor...");
        }

        void fonkserveribaslat()
        {
            kullaniciAdi = kullaniciAditext.Text;
            listener = new TcpListener(500);
            listener.Start();

            socket = listener.AcceptSocket();
            stream = new NetworkStream(socket);

            Thread thlistener = new Thread(fonklistener);
            thlistener.Start();

                
                
        }
        BinaryFormatter bf = new BinaryFormatter();
        void fonklistener()
        {
            
            while (true)
            {


                string mesaj = bf.Deserialize(stream).ToString();

                Console.WriteLine(mesaj);

                if (mesaj.Substring(0, 3) == "100")//mesaj
                {
                    listBox1.Items.Add(mesaj.Substring(3));
                }
                else if (mesaj.Substring(0, 3) == "101")//deneme
                {
                    Console.WriteLine(mesaj.Substring(3));
                }
                else if (mesaj.Substring(0, 3) == "102")//sıra
                {
                    rakibinSurelbl.Text=mesaj.Substring(3).ToString();
                    sıraSende = true;
                }
                else if (mesaj.Substring(0, 3) == "103")
                {
                  string gelen = mesaj.Substring(3).ToString();
                   // int [] x = gelen.Split('=');
                    //int[] y = gelen.Split('=');

                    

                   // movPicture1.Location = new Point(x[0], y[0]);
                }



            }
        }
      
        private void gonderBtn_click(object sender, EventArgs e)
        {
            listBox1.Items.Add(kullaniciAdi + ":" + mesajtextBox1.Text);
            bf.Serialize(stream, "100" +kullaniciAdi+":"+ mesajtextBox1.Text);
            stream.Flush();
            mesajtextBox1.Text = "";
        }

        private void oynadimBtn_Click(object sender, EventArgs e)
        {
            sıraSende = false;
            bf.Serialize(stream, "101" + "b4 to f8");
            bf.Serialize(stream, "102" + süre.ToString());//süre gönder


            stream.Flush();

        }

       

        private void timer1_Tick(object sender, EventArgs e)
        {
            if (sıraSende){
                süre--;
                seninSurenlbl.Text = süre.ToString();
                if (süre == 0)
                {
                    timer1.Stop();
                    MessageBox.Show("Oyun bitti rakip kazandı...");
                    oyunBitti = true;
                }
            }
           
        }
    }
}
