using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Timers;
using System.Windows.Forms;
namespace LCPclient
{
    public partial class Form1 : Form
    {
        ArrayList tasKonumları = new ArrayList();

        TcpClient tcpClient;
        NetworkStream stream;
        string kullaniciAdı;
        bool sıraSende;
        int süre = 180;
        bool oyunBitti = false;





       




        public Form1()
        {
            Control.CheckForIllegalCrossThreadCalls = false;
            InitializeComponent();
            tasKonumları.Add(movPicture1.Location);
            tasKonumları.Add(movPicture2.Location);
            tasKonumları.Add(movPicture3.Location);
            tasKonumları.Add(movPicture4.Location);
            tasKonumları.Add(movPicture5.Location);
            tasKonumları.Add(movPicture6.Location);
            tasKonumları.Add(movPicture7.Location);
            tasKonumları.Add(movPicture8.Location);
            tasKonumları.Add(movPicture9.Location);
            tasKonumları.Add(movPicture10.Location);
            tasKonumları.Add(movPicture11.Location);
            tasKonumları.Add(movPicture12.Location);
            tasKonumları.Add(movPicture13.Location);
            tasKonumları.Add(movPicture14.Location);
            tasKonumları.Add(movPicture15.Location);
            tasKonumları.Add(movPicture16.Location);
            tasKonumları.Add(movPicture17.Location);
            tasKonumları.Add(movPicture18.Location);
            tasKonumları.Add(movPicture19.Location);
            tasKonumları.Add(movPicture20.Location);
            tasKonumları.Add(movPicture21.Location);
            tasKonumları.Add(movPicture22.Location);
            tasKonumları.Add(movPicture23.Location);
            tasKonumları.Add(movPicture24.Location);
            tasKonumları.Add(movPicture25.Location);
            tasKonumları.Add(movPicture26.Location);
            tasKonumları.Add(movPicture27.Location);
            tasKonumları.Add(movPicture28.Location);
            tasKonumları.Add(movPicture29.Location);
            tasKonumları.Add(movPicture30.Location);
            tasKonumları.Add(movPicture31.Location);
            tasKonumları.Add(movPicture32.Location);
            


        }



        BinaryFormatter bf = new BinaryFormatter();
    



        


        private void baslatBtn_Click(object sender, EventArgs e)
        {
            string bilgisayarAdi = Dns.GetHostName();
            string ipAdresi = Dns.GetHostByName(bilgisayarAdi).AddressList[0].ToString();
          

            kullaniciAdı = kullaniciAditxt.Text;
            tcpClient = new TcpClient(ipAdresi, 500);
            stream = tcpClient.GetStream();
            
            Thread dinleyici = new Thread(fonklistener);
            dinleyici.Start();
            sıraKimdelbl.Text = "Sıra Sende:";
            sıraSende = true;
            timer1.Start();
            
            

              

        }
        void fonklistener()
        {
            while (true)
            {

                string mesaj = bf.Deserialize(stream).ToString();

                Console.WriteLine(mesaj.Substring(0, 3));

                if (mesaj.Substring(0, 3) == "100") //mesaj 
                {
                    listBox1.Items.Add(mesaj.Substring(3));
                }
                else if (mesaj.Substring(0, 3) == "101")
                {
                    Console.WriteLine(mesaj.Substring(3));
                }
                else if (mesaj.Substring(0, 3) == "102") // sure
                {
                    rakibinSurelbl.Text = mesaj.Substring(3);

                    sıraSende = true;
                    
                  

                }
               

            }

        }

       


        private void gonderBtn_Click(object sender, EventArgs e)
        {
            listBox1.Items.Add(kullaniciAdı + ":" + mesajtextBox1.Text);
            bf.Serialize(stream, "100" +kullaniciAdı+":"+ mesajtextBox1.Text);
            stream.Flush();
            mesajtextBox1.Text = "";

        }

        private void oynadimBtn_Click(object sender, EventArgs e)
        {

            sıraSende = false;
            bf.Serialize(stream, "101" + "b4 to f8");
            bf.Serialize(stream, "102" + süre.ToString());
            stream.Flush();
          
           
                if (tasKonumları[1].ToString() != movPicture1.Location.ToString())
                {
                bf.Serialize(stream, "103" + movPicture1.Location.ToString());

            }
          
            
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            if (sıraSende)
            {
                süre--;
                seninSurenlbl.Text = süre.ToString();
                if(süre == 0)
                {
                    oyunBitti = true;

                }
            }
          

        }
    }
}
