using Bike18;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace MotoPiter
{
    public partial class Form1 : Form
    {
        Thread forms;

        string minitextTemplate;
        string fullTextTemplate;
        string keywordsTextTemplate;
        string titleTextTemplate;
        string descriptionTextTemplate;
        string otv;

        nethouse nethouse = new nethouse();
        httpRequest webRequest = new httpRequest();

        public Form1()
        {
            InitializeComponent();
            if (!Directory.Exists("files"))
            {
                Directory.CreateDirectory("files");
            }
            if (!Directory.Exists("pic"))
            {
                Directory.CreateDirectory("pic");
            }

            if (!File.Exists("files\\miniText.txt"))
            {
                File.Create("files\\miniText.txt");
            }

            if (!File.Exists("files\\fullText.txt"))
            {
                File.Create("files\\fullText.txt");
            }

            if (!File.Exists("files\\title.txt"))
            {
                File.Create("files\\title.txt");
            }

            if (!File.Exists("files\\description.txt"))
            {
                File.Create("files\\description.txt");
            }

            if (!File.Exists("files\\keywords.txt"))
            {
                File.Create("files\\keywords.txt");
            }
            StreamReader altText = new StreamReader("files\\miniText.txt", Encoding.GetEncoding("windows-1251"));
            while (!altText.EndOfStream)
            {
                string str = altText.ReadLine();
                rtbMiniText.AppendText(str + "\n");
            }
            altText.Close();

            altText = new StreamReader("files\\fullText.txt", Encoding.GetEncoding("windows-1251"));
            while (!altText.EndOfStream)
            {
                string str = altText.ReadLine();
                rtbFullText.AppendText(str + "\n");
            }
            altText.Close();

            altText = new StreamReader("files\\title.txt", Encoding.GetEncoding("windows-1251"));
            while (!altText.EndOfStream)
            {
                string str = altText.ReadLine();
                tbTitle.AppendText(str + "\n");
            }
            altText.Close();

            altText = new StreamReader("files\\description.txt", Encoding.GetEncoding("windows-1251"));
            while (!altText.EndOfStream)
            {
                string str = altText.ReadLine();
                tbDescription.AppendText(str + "\n");
            }
            altText.Close();

            altText = new StreamReader("files\\keywords.txt", Encoding.GetEncoding("windows-1251"));
            while (!altText.EndOfStream)
            {
                string str = altText.ReadLine();
                tbKeywords.AppendText(str + "\n");
            }
            altText.Close();
        }

        private void btnSaveTemplate_Click(object sender, EventArgs e)
        {
            int count = 0;
            StreamWriter writers = new StreamWriter("files\\miniText.txt", false, Encoding.GetEncoding(1251));
            count = rtbMiniText.Lines.Length;
            for (int i = 0; rtbMiniText.Lines.Length > i; i++)
            {
                if (count - 1 == i)
                {
                    if (rtbFullText.Lines[i] == "")
                        break;
                }
                writers.WriteLine(rtbMiniText.Lines[i].ToString());
            }
            writers.Close();

            writers = new StreamWriter("files\\fullText.txt", false, Encoding.GetEncoding(1251));
            count = rtbFullText.Lines.Length;
            for (int i = 0; count > i; i++)
            {
                if (count - 1 == i)
                {
                    if (rtbFullText.Lines[i] == "")
                        break;
                }
                writers.WriteLine(rtbFullText.Lines[i].ToString());
            }
            writers.Close();

            writers = new StreamWriter("files\\title.txt", false, Encoding.GetEncoding(1251));
            writers.WriteLine(tbTitle.Lines[0]);
            writers.Close();

            writers = new StreamWriter("files\\description.txt", false, Encoding.GetEncoding(1251));
            writers.WriteLine(tbDescription.Lines[0]);
            writers.Close();

            writers = new StreamWriter("files\\keywords.txt", false, Encoding.GetEncoding(1251));
            writers.WriteLine(tbKeywords.Lines[0]);
            writers.Close();

            MessageBox.Show("Сохранено");
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            tbLoginNethouse.Text = Properties.Settings.Default.loginNethouse;
            tbPassNethouse.Text = Properties.Settings.Default.passNethouse;
            tbLoginMotopiter.Text = Properties.Settings.Default.loginMotopiter;
            tbPassMotopiter.Text = Properties.Settings.Default.passMotopiter;
        }

        private void btnActual_Click(object sender, EventArgs e)
        {
            #region Сохранение паролей
            Properties.Settings.Default.loginNethouse = tbLoginNethouse.Text;
            Properties.Settings.Default.passNethouse = tbPassNethouse.Text;
            Properties.Settings.Default.loginMotopiter = tbLoginMotopiter.Text;
            Properties.Settings.Default.passMotopiter = tbPassMotopiter.Text;
            Properties.Settings.Default.Save();
            #endregion

            #region Обработка сайта

            minitextTemplate = MinitextStr();
            fullTextTemplate = FulltextStr();
            keywordsTextTemplate = tbKeywords.Lines[0].ToString();
            titleTextTemplate = tbTitle.Lines[0].ToString();
            descriptionTextTemplate = tbDescription.Lines[0].ToString();

            Thread tabl = new Thread(() => ActualMotoPiter());
            forms = tabl;
            forms.IsBackground = true;
            forms.Start();

            #endregion

        }

        private void ActualMotoPiter()
        {
            CookieContainer cookieNethouse = nethouse.CookieNethouse(tbLoginNethouse.Text, tbPassNethouse.Text);
            if (cookieNethouse.Count == 1)
            {
                MessageBox.Show("Логин или пароль для сайта Nethouse введены не верно", "Ошибка логина/пароля");
                return;
            }

            CookieContainer cookieMotoPiter = CookieMotoPiter(tbLoginMotopiter.Text, tbPassMotopiter.Text);
            if (cookieMotoPiter.Count != 1)
            {
                MessageBox.Show("Логин или пароль для сайта MotoPiter введены не верно", "Ошибка логина/пароля");
                return;
            }

            ControlsFormEnabledFalse();

            #region Моторасходники
            otv = webRequest.getRequest(cookieMotoPiter, "http://www.motopiter.ru/dealer/dealer.asp");

            otv = webRequest.getRequest(cookieMotoPiter, "http://www.motopiter.ru/product/6");

            MatchCollection category = new Regex("(?<=<li class=\"submenu\"><a href=\"/product/6/).*?(?=</a>)").Matches(otv);

            foreach(Match categoryStr in category)
            {
                string str = categoryStr.ToString();
                string subCategoryName = new Regex("(?<=\">).*").Match(str).ToString();
                string subCategoryUrl = new Regex(".*(?=\">)").Match(str).ToString();

                if (subCategoryUrl == "750" || subCategoryUrl == "470" || subCategoryUrl == "40")
                    continue;

                MatchCollection subCategory = new Regex("(?<=<a class=\"nolink small\" href=\"/product/6/" + subCategoryUrl + "/).*?(?=</a>)").Matches(otv);

                foreach(Match subCategoryStr in subCategory)
                {
                    string subStr = subCategoryStr.ToString();
                    string subCategorySmallName = new Regex("(?<=\" title=\").*?(?=\">)").Match(subStr).ToString();
                    string subCategorySmallUrl = new Regex(".*?(?=\" title=\")").Match(subStr).ToString();
                    string url = "http://www.motopiter.ru/product/6/" + subCategoryUrl + "/" + subCategorySmallUrl;

                    UpdateTovars(cookieMotoPiter, url);
                }
            }

            #endregion

        }

        private void UpdateTovars(CookieContainer cookieMotoPiter, string url)
        {
            otv = null;
            otv = webRequest.getRequest(cookieMotoPiter, url);

            MatchCollection tovarBox = new Regex("<div class=\"box_grey\".*?</div></div></a></div>").Matches(otv);
            foreach(Match str in tovarBox)
            {
                string strTovarBox = str.ToString();
                string urlTovar = new Regex("(?<=<a href=\").*?(?=\")").Match(strTovarBox).ToString();
                urlTovar = "http://www.motopiter.ru" + urlTovar;
            }
        }

        private CookieContainer CookieMotoPiter(string login, string password)
        {
            CookieContainer cookie = new CookieContainer();
            HttpWebRequest req = (HttpWebRequest)HttpWebRequest.Create("http://www.motopiter.ru/run");
            req.Accept = "text/html, */*; q=0.01";
            req.UserAgent = "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/58.0.3029.96 Safari/537.36";
            req.Method = "POST";
            req.ContentType = "application/x-www-form-urlencoded; charset=UTF-8";
            req.CookieContainer = cookie;
            byte[] ms = Encoding.ASCII.GetBytes("par=LogonDealer1&email=" + login + "&password=" + password);
            req.ContentLength = ms.Length;
            Stream stre = req.GetRequestStream();
            stre.Write(ms, 0, ms.Length);
            stre.Close();
            HttpWebResponse res = (HttpWebResponse)req.GetResponse();

            return cookie;
        }

        private void ControlsFormEnabledFalse()
        {
            btnActual.Invoke(new Action(() => btnActual.Enabled = false));
            btnImages.Invoke(new Action(() => btnImages.Enabled = false));
            btnSaveTemplate.Invoke(new Action(() => btnSaveTemplate.Enabled = false));
            rtbFullText.Invoke(new Action(() => rtbFullText.Enabled = false));
            rtbMiniText.Invoke(new Action(() => rtbMiniText.Enabled = false));
            tbDescription.Invoke(new Action(() => tbDescription.Enabled = false));
            tbKeywords.Invoke(new Action(() => tbKeywords.Enabled = false));
            tbTitle.Invoke(new Action(() => tbTitle.Enabled = false));
            tbLoginNethouse.Invoke(new Action(() => tbLoginNethouse.Enabled = false));
            tbPassNethouse.Invoke(new Action(() => tbPassNethouse.Enabled = false));
            tbLoginMotopiter.Invoke(new Action(() => tbLoginMotopiter.Enabled = false));
            tbPassMotopiter.Invoke(new Action(() => tbPassMotopiter.Enabled = false));
        }

        private string MinitextStr()
        {
            string minitext = "";
            for (int z = 0; rtbMiniText.Lines.Length > z; z++)
            {
                if (rtbMiniText.Lines[z].ToString() == "")
                {
                    minitext += "<p><br /></p>";
                }
                else
                {
                    minitext += "<p>" + rtbMiniText.Lines[z].ToString() + "</p>";
                }
            }
            return minitext;
        }

        private string FulltextStr()
        {
            string fullText = "";
            for (int z = 0; rtbFullText.Lines.Length > z; z++)
            {
                if (rtbFullText.Lines[z].ToString() == "")
                {
                    fullText += "<p><br /></p>";
                }
                else
                {
                    fullText += "<p>" + rtbFullText.Lines[z].ToString() + "</p>";
                }
            }
            return fullText;
        }
    }
}
