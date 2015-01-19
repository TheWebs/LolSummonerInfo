using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Newtonsoft.Json;
using System.Net;
using Newtonsoft.Json.Linq;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Json;
using System.Text.RegularExpressions;

namespace LoLINFO
{
    public partial class Form1 : Form
    {
        public string TOZE = null;
        public string ID = "";
        public string ICON = "";
        public string IDPLAYER = "";
        public string RANKROMANO = "";
        public string RANKLIGA = "";
        public string TEXTOHTTP = "";
        public Form1()
        {
            InitializeComponent();
        }

        private bool _dragging = false;
        private Point _offset;
        private Point _start_point = new Point(0, 0);

        
        private void button1_Click(object sender, EventArgs e)
        {
            pictureBox1.BackgroundImage = null;
            pictureBox3.BackgroundImage = null;
            ID = textBox1.Text;
            WebClient info = new WebClient();
            try
            {
                TOZE = info.DownloadString("https://euw.api.pvp.net/api/lol/euw/v1.4/summoner/by-name/" + textBox1.Text + "?api_key=067c7765-e085-4a55-83f1-be65b5869416");
            } catch (WebException ex)
                {
                    if(ex.Status == WebExceptionStatus.ProtocolError)
                    {
                        MessageBox.Show("SUMMONER NAO EXISTE!");
                        return;
                    }

                }
            string nomebaixo = textBox1.Text.Replace(" ", "");
            string nomecerto = nomebaixo.ToLower();
            TOZE = TOZE.Replace(nomecerto, "kiko298");
            LerJson();
            ObterIcons();
            ObterLiga();


        }

        public void ObterIcons()
        {
            WebClient joao = new WebClient();
            string text = joao.DownloadString("http://ddragon.leagueoflegends.com/realms/euw.json").ToString();
            string[] URL = text.Split(':');
            // url[8] da me isto "4.21.5","l"298
            string[] bom = URL[8].Split('"');
            // bom[1] da me isto 4.21.5
            WebClient joao2 = new WebClient();
            joao2.DownloadFile("http://ddragon.leagueoflegends.com/cdn/" + bom[1] + "/img/profileicon/" + ICON + ".png ", Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) + "\\Icon.png");
            FileStream fileStream = new FileStream(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) + "\\Icon.png", FileMode.Open, FileAccess.Read);
            pictureBox1.BackgroundImage = Image.FromStream(fileStream);
            fileStream.Close();
            File.Delete(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) + "\\Icon.png");

        }

        public void ObterLiga()
        {
            WebClient web = new WebClient();
            try
            {
                TEXTOHTTP = web.DownloadString("https://euw.api.pvp.net/api/lol/euw/v2.5/league/by-summoner/" + IDPLAYER + "/entry?api_key=067c7765-e085-4a55-83f1-be65b5869416");
            }
            catch (WebException ex)
            {
                if(ex.Status == WebExceptionStatus.ProtocolError)
                {
                    MessageBox.Show("O jogador ainda nao e LvL30!");
                    return;
                }
            }
            string[] textoseparado = TEXTOHTTP.Split(':');
            // apanho isto  "IV", "isInactive"
            string[] ranknumero = textoseparado[8].Split('"');
            //apanho isto IV
            RANKROMANO = ranknumero[1];
            string[] rankliga = textoseparado[3].Split('"');
            RANKLIGA = rankliga[1];
            string before = richTextBox1.Text;
            richTextBox1.Text = before + Environment.NewLine + "Ranked: " + RANKLIGA + " " + RANKROMANO;
            if(RANKROMANO == "V")
            {
                WebClient joao3 = new WebClient();
                joao3.DownloadFile("https://elo-boost.net/images/tiers/" + RANKLIGA.ToLower() + "_5" + ".png ", Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) + "\\Liga.png");
            }
            else if(RANKROMANO == "IV")
            {
                WebClient joao3 = new WebClient();
                joao3.DownloadFile("https://elo-boost.net/images/tiers/" + RANKLIGA.ToLower() + "_4" + ".png ", Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) + "\\Liga.png");
            }
            else if (RANKROMANO == "III")
            {
                WebClient joao3 = new WebClient();
                joao3.DownloadFile("https://elo-boost.net/images/tiers/" + RANKLIGA.ToLower() + "_3" + ".png ", Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) + "\\Liga.png");
            }
            else if (RANKROMANO == "II")
            {
                WebClient joao3 = new WebClient();
                joao3.DownloadFile("https://elo-boost.net/images/tiers/" + RANKLIGA.ToLower() + "_2" + ".png ", Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) + "\\Liga.png");
            }
            else if (RANKROMANO == "I")
            {
                WebClient joao3 = new WebClient();
                joao3.DownloadFile("https://elo-boost.net/images/tiers/" + RANKLIGA.ToLower() + "_1" + ".png ", Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) + "\\Liga.png");
            }
            FileStream fileStream = new FileStream(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) + "\\Liga.png", FileMode.Open, FileAccess.Read);
            pictureBox3.BackgroundImage = Image.FromStream(fileStream);
            fileStream.Close();
            File.Delete(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) + "\\Liga.png");
            //https://elo-boost.net/images/tiers/challenger_1.png



        }


        public void LerJson()
        {
            byte[] data = Encoding.UTF8.GetBytes(TOZE);
            var ms = new MemoryStream(data);
            var dcjs = new DataContractJsonSerializer(typeof(A));
            A a = (A)dcjs.ReadObject(ms);
            richTextBox1.Text = a.Kiko298.ToString();
            ICON = a.Kiko298.ProfileIconId.ToString();
            IDPLAYER = a.Kiko298.Id.ToString();


        }
        [DataContract]
        public class A
        {
            
            [DataMember(Name = "kiko298")]
            public B Kiko298 { get; set; }
        }

        [DataContract]
        public class B
        {
            public B() { }

            [DataMember(Name = "id")]
            public int Id { get; set; }

            [DataMember(Name = "name")]
            public string Name { get; set; }

            [DataMember(Name = "profileIconId")]
            public int ProfileIconId { get; set; }

            [DataMember(Name = "revisionDate")]
            public long RevisionDate { get; set; }

            [DataMember(Name = "summonerLevel")]
            public int SummonerLevel { get; set; }

            public override string ToString()
            {
                return String.Format("Id = {0}\nName = {1}\nProfile Icon Id = {2}\nRevision Date = {3}\nSummoner Level = {4}", Id, Name, ProfileIconId, RevisionDate, SummonerLevel);
            }

        }

        private void pictureBox2_Click(object sender, EventArgs e)
        {
            Environment.Exit(0);
        }

        private void Form1_MouseDown(object sender, MouseEventArgs e)
        {
            _dragging = true;  // _dragging is your variable flag
            _start_point = new Point(e.X, e.Y);
        }

        private void Form1_MouseUp(object sender, MouseEventArgs e)
        {
            _dragging = false; 
        }

        private void Form1_MouseMove(object sender, MouseEventArgs e)
        {
            if (_dragging)
            {
                Point p = PointToScreen(e.Location);
                Location = new Point(p.X - this._start_point.X, p.Y - this._start_point.Y);
            }
        }

    }
}
