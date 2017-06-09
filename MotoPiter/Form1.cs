using Bike18;
using RacerMotors;
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
using Формирование_ЧПУ;

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
        string boldOpen = "<span style=\"\"font-weight: bold; font-weight: bold; \"\">";
        string boldClose = "</span>";

        List<string> newProduct = new List<string>();

        nethouse nethouse = new nethouse();
        httpRequest webRequest = new httpRequest();
        CHPU chpu = new CHPU();
        FileEdit files = new FileEdit();
        WebClient webClient = new WebClient();

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
            File.Delete("naSite.csv");
            newProduct = newList();
            ControlsFormEnabledFalse();

            #region Моторасходники
            otv = webRequest.getRequest(cookieMotoPiter, "http://www.motopiter.ru/dealer/dealer.asp");

            otv = webRequest.getRequest(cookieMotoPiter, "http://www.motopiter.ru/product/6");

            MatchCollection category = new Regex("(?<=<li class=\"submenu\"><a href=\"/product/6/).*?(?=</a>)").Matches(otv);

            foreach (Match categoryStr in category)
            {
                string str = categoryStr.ToString();
                string subCategoryName = new Regex("(?<=\">).*").Match(str).ToString();
                string subCategoryUrl = new Regex(".*(?=\">)").Match(str).ToString();

                if (subCategoryUrl == "750" || subCategoryUrl == "470" || subCategoryUrl == "40")
                    continue;

                MatchCollection subCategory = new Regex("(?<=<a class=\"nolink small\" href=\"/product/6/" + subCategoryUrl + "/).*?(?=</a>)").Matches(otv);

                foreach (Match subCategoryStr in subCategory)
                {
                    string subStr = subCategoryStr.ToString();
                    string subCategorySmallName = new Regex("(?<=\" title=\").*?(?=\">)").Match(subStr).ToString();
                    string subCategorySmallUrl = new Regex(".*?(?=\" title=\")").Match(subStr).ToString();
                    string url = "http://www.motopiter.ru/product/6/" + subCategoryUrl + "/" + subCategorySmallUrl;

                    UpdateTovars(cookieNethouse, cookieMotoPiter, url);

                    UploadCSVInNethoise(cookieNethouse);
                }
            }

            #endregion

        }

        private void UploadCSVInNethoise(CookieContainer cookieNethouse)
        {
            string[] naSite1 = File.ReadAllLines("naSite.csv", Encoding.GetEncoding(1251));
            if (naSite1.Length > 1)
                nethouse.UploadCSVNethouse(cookieNethouse, "naSite.csv");
        }

        private void UpdateTovars(CookieContainer cookieNethouse, CookieContainer cookieMotoPiter, string url)
        {
            otv = null;
            otv = webRequest.getRequest(cookieMotoPiter, url);

            MatchCollection tovarBox = new Regex("<div class=\"box_grey\".*?</div></div></a></div>").Matches(otv);
            foreach (Match str in tovarBox)
            {
                string strTovarBox = str.ToString();
                string urlTovar = new Regex("(?<=<a href=\").*?(?=\")").Match(strTovarBox).ToString();
                urlTovar = "http://www.motopiter.ru" + urlTovar;
                urlTovar = "http://www.motopiter.ru/20940064";

                List<string> tovarMotoPiter = GetTovarMotoPiter(cookieMotoPiter, urlTovar);

                string resultSearch = SearchInBike18(tovarMotoPiter);
                if (resultSearch == null)
                {
                    WriteTovarInCSV(tovarMotoPiter);
                }
                else
                {
                    //обновить цену
                }
            }
        }

        private List<string> newList()
        {
            List<string> newProduct = new List<string>();
            newProduct.Add("id");                                                                               //id
            newProduct.Add("Артикул *");                                                 //артикул
            newProduct.Add("Название товара *");                                          //название
            newProduct.Add("Стоимость товара *");                                    //стоимость
            newProduct.Add("Стоимость со скидкой");                                       //со скидкой
            newProduct.Add("Раздел товара *");                                         //раздел товара
            newProduct.Add("Товар в наличии *");                                                    //в наличии
            newProduct.Add("Поставка под заказ *");                                                 //поставка
            newProduct.Add("Срок поставки (дни) *");                                           //срок поставки
            newProduct.Add("Краткий текст");                                 //краткий текст
            newProduct.Add("Текст полностью");                                          //полностью текст
            newProduct.Add("Заголовок страницы (title)");                               //заголовок страницы
            newProduct.Add("Описание страницы (description)");                                 //описание
            newProduct.Add("Ключевые слова страницы (keywords)");                                 //ключевые слова
            newProduct.Add("ЧПУ страницы (slug)");                                   //ЧПУ
            newProduct.Add("С этим товаром покупают");                              //с этим товаром покупают
            newProduct.Add("Рекламные метки");
            newProduct.Add("Показывать на сайте *");                                           //показывать
            newProduct.Add("Удалить *");                                    //удалить
            files.fileWriterCSV(newProduct, "naSite");
            return newProduct;
        }

        private void WriteTovarInCSV(List<string> tovarMotoPiter)
        {
            string nameTovar = tovarMotoPiter[0].ToString();
            string fullText = tovarMotoPiter[9].ToString();
            string minitextTemplate = tovarMotoPiter[8].ToString();
            string categoryTovar = tovarMotoPiter[7].ToString();
            string descriptionText = tovarMotoPiter[4].ToString();
            string titleText = tovarMotoPiter[5].ToString();
            string keywordsText = tovarMotoPiter[6].ToString();


            string[] articles = tovarMotoPiter[1].ToString().Split(';');
            string[] prices = tovarMotoPiter[2].ToString().Split(';');
            string[] slugs = tovarMotoPiter[3].ToString().Split(';');            
            string[] miniDescriptions = tovarMotoPiter[10].ToString().Split(';');

            if(articles.Length == prices.Length && prices.Length == slugs.Length && slugs.Length == miniDescriptions.Length)
            {
                for (int i = 0; articles.Length > i; i++)
                {
                    string article = articles[i];
                    string price = prices[i];
                    string minitext = miniDescriptions[i];
                    string slug = slugs[i];

                    if (article == "")
                        continue;

                    minitext = "<p>" + minitext + "</p>" + minitextTemplate;

                    newProduct = new List<string>();
                    newProduct.Add(""); //id
                    newProduct.Add("\"" + article + "\""); //артикул
                    newProduct.Add("\"" + nameTovar + "\"");  //название
                    newProduct.Add("\"" + price + "\""); //стоимость
                    newProduct.Add("\"" + "" + "\""); //со скидкой
                    newProduct.Add("\"" + categoryTovar + "\""); //раздел товара
                    newProduct.Add("\"" + "100" + "\""); //в наличии
                    newProduct.Add("\"" + "0" + "\"");//поставка
                    newProduct.Add("\"" + "1" + "\"");//срок поставки
                    newProduct.Add("\"" + minitext + "\"");//краткий текст
                    newProduct.Add("\"" + fullText + "\"");//полностью текст
                    newProduct.Add("\"" + titleText + "\""); //заголовок страницы
                    newProduct.Add("\"" + descriptionText + "\""); //описание
                    newProduct.Add("\"" + keywordsText + "\"");//ключевые слова
                    newProduct.Add("\"" + slug + "\""); //ЧПУ
                    newProduct.Add(""); //с этим товаром покупают
                    newProduct.Add("");   //рекламные метки
                    newProduct.Add("\"" + "1" + "\"");  //показывать
                    newProduct.Add("\"" + "0" + "\""); //удалить

                    files.fileWriterCSV(newProduct, "naSite");
                }
            }
            else
            {

            }
        }

        private string SearchInBike18(List<string> tovarMotoPiter)
        {
            string urlTovar = "";

            string nameTovar = tovarMotoPiter[0].ToString();
            string articles = tovarMotoPiter[1].ToString();
            string[] article = articles.Split(';');

            urlTovar = nethouse.searchTovar(nameTovar, nameTovar);
            
            foreach(string str in article)
            {
                string search = "";
                if (urlTovar == null)
                {
                    search = nethouse.searchTovar(nameTovar, str);
                    if(search != null)
                    {
                        urlTovar = urlTovar + ";" + search;
                    }
                }
                    
            }
            
            return urlTovar;
        }

        private List<string> GetTovarMotoPiter(CookieContainer cookieMotoPiter, string urlTovar)
        {
            List<string> tovar = new List<string>();
            otv = null;

            otv = webRequest.getRequest(cookieMotoPiter, urlTovar);

            string nameTovar = new Regex("(?<=<h4>).*?(?=</h4><dl)").Match(otv).ToString();

            string panelTovar = new Regex("(?<=id=\"description\")[\\w\\W]*?(?=</div>)").Match(otv).ToString();
            panelTovar = panelTovar.Replace("<p></p>", "");
            string descriptionTovar = new Regex("(?<=<p>)[\\w\\W]*?(?=</p>)").Match(panelTovar).ToString();
            descriptionTovar = DeleteUrlsInText(descriptionTovar);

            string minDescription = "";
            MatchCollection miniDescription = new Regex("(?<=data-TextArt=\").*?(?=\")").Matches(otv);
            foreach (Match str in miniDescription)
            {
                string s = str.ToString();
                s = s.Trim();
                minDescription = minDescription + ";" + s;
            }

            string article = "";
            MatchCollection articles = new Regex("(?<=<strong>Арт.).*?(?=</strong>)").Matches(otv);
            for (int i = 0; articles.Count > i; i++)
            {
                string s = articles[i].ToString();
                s = s.Trim();
                s = s.Replace("/", "_").Replace("-", "_").Replace(" ", "_");
                article = article + ";MP_" + s + "_" + i;
            }

            string price = "";
            MatchCollection prices = new Regex("(?<=<p><small>).*(?=</small></p>)").Matches(otv);
            foreach (Match str in prices)
            {
                string s = str.ToString();
                s = s.Replace("р:&nbsp;", "").Replace("&nbsp;р.", "");
                s = s.Trim();
                price = price + ";" + s;
            }

            string slug = "";
            for (int i = 0; prices.Count > i; i++)
            {
                slug = slug + ";" + chpu.vozvr(nameTovar) + "-" + i;
            }

            string descriptionText = descriptionTextTemplate;
            string titleText = titleTextTemplate;
            string keywordsText = keywordsTextTemplate;

            titleText = ReplaceSEO("title", titleText, nameTovar, article.Replace(";", " "));
            descriptionText = ReplaceSEO("description", descriptionText, nameTovar, article);
            keywordsText = ReplaceSEO("keywords", keywordsText, nameTovar, article);

            string categoryTovar = ReturnCategoryTovar(otv);

            string miniText = minitextTemplate;
            string fullText = fullTextTemplate;

            miniText = Replace(miniText, nameTovar, article);
            miniText = miniText.Remove(miniText.LastIndexOf("<p>"));

            fullText = Replace(fullText, nameTovar, article);
            fullText = fullText.Remove(fullText.LastIndexOf("<p>"));
            fullText = "<p>" + descriptionTovar + "</p><p></p>" + fullText;

            DownloadImages(otv, article);

            tovar.Add(nameTovar);
            tovar.Add(article);
            tovar.Add(price);
            tovar.Add(slug);
            tovar.Add(descriptionText);
            tovar.Add(titleText);
            tovar.Add(keywordsText);
            tovar.Add(categoryTovar);
            tovar.Add(miniText);
            tovar.Add(fullText);
            tovar.Add(minDescription);

            return tovar;
        }

        private void DownloadImages(string otv, string article)
        {
            string urlImage = new Regex("(?<=<img class=\"img-responsive\" src=\").*?(?=\")").Match(otv).ToString();

            string[] articles = article.Split(';');
            articles = article.Split(';');
            foreach (string str in articles)
            {
                if (str == "")
                    continue;
                if (!File.Exists("pic\\" + str + ".jpg"))
                {
                    try
                    {
                        webClient.DownloadFile("http://www.motopiter.ru" + urlImage, "Pic\\" + str + ".jpg");
                    }
                    catch
                    {

                    }
                }
            }
        }

        private string Replace(string text, string nameTovar, string article)
        {
            string discount = Discount();
            string nameText = boldOpen + nameTovar + boldClose;
            text = text.Replace("СКИДКА", discount).Replace("НАЗВАНИЕ", nameText).Replace("АРТИКУЛ", article).Replace("<p><br /></p><p><br /></p><p><br /></p><p>", "<p><br /></p>");
            return text;
        }

        private string ReturnCategoryTovar(string otv)
        {
            string category = "";

            string categoriesStr = new Regex("(?<=breadcrumb\">).*?(?=Следующий)").Match(otv).ToString();
            MatchCollection categories = new Regex("(?<=\">).*?(?=</a>)").Matches(categoriesStr);
            string categoryName = categories[1].ToString();
            category = "Запчасти и расходники => Расходники для японских, европейских, американских мотоциклов => " + categoryName;

            return category;
        }

        private string ReplaceSEO(string nameSEO, string text, string nameTovar, string article)
        {
            text = text.Replace("НАЗВАНИЕ", nameTovar).Replace("АРТИКУЛ", article);

            switch (nameSEO)
            {
                case "title":
                    text = RemoveText(text, 255);
                    break;
                case "description":
                    text = RemoveText(text, 200);
                    break;
                case "keywords":
                    text = RemoveText(text, 100);
                    break;
                default:
                    text = RemoveText(text, 100);
                    break;
            }

            return text;
        }

        private string Discount()
        {
            string discount = "<p style=\"\"text-align: right;\"\"><span style=\"\"font -weight: bold; font-weight: bold;\"\"> Сделай ТРОЙНОЙ удар по нашим ценам! </span></p><p style=\"\"text-align: right;\"\"><span style=\"\"font -weight: bold; font-weight: bold;\"\"> 1. <a target=\"\"_blank\"\" href =\"\"http://bike18.ru/stock\"\"> Скидки за отзывы о товарах!</a> </span></p><p style=\"\"text-align: right;\"\"><span style=\"\"font -weight: bold; font-weight: bold;\"\"> 2. <a target=\"\"_blank\"\" href =\"\"http://bike18.ru/stock\"\"> Друзьям скидки и подарки!</a> </span></p><p style=\"\"text-align: right;\"\"><span style=\"\"font -weight: bold; font-weight: bold;\"\"> 3. <a target=\"\"_blank\"\" href =\"\"http://bike18.ru/stock\"\"> Нашли дешевле!? 110% разницы Ваши!</a></span></p>";
            return discount;
        }

        private string RemoveText(string text, int v)
        {
            if (text.Length > v)
            {
                text = text.Remove(v);
                text = text.Remove(text.LastIndexOf(" "));
            }
            return text;
        }

        private string DeleteUrlsInText(string text)
        {
            MatchCollection urls = new Regex("<a[\\w\\W]*?</a>").Matches(text);
            foreach (Match str in urls)
            {
                string s = str.ToString();
                text = text.Replace(s, "");
            }
            return text;
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
