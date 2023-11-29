using Abot.Crawler;
using Abot.Poco;
using Site_Corrector.Logika.Modele;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace Site_Corrector.Logika
{
    class Linki: INotifyPropertyChanged
    {
        Uri www;// = new Uri();

        List<Link> linki_ok = new List<Link>();
        List<Link> linki_puste = new List<Link>();
        List<Link> linki_blad = new List<Link>();


        public Linki(string adres_www)
        {
            this.www = new Uri(adres_www);

            IWebCrawler crawler;
            crawler = GetManuallyConfiguredWebCrawler();

            this.Linki_ok.Add(new Link(this.www.AbsoluteUri, 0));

            crawler.PageCrawlCompletedAsync += crawler_ProcessPageCrawlCompleted;

            //Start the crawl
            //This is a synchronous call
            CrawlResult result = crawler.Crawl(this.www);
        }


        void crawler_ProcessPageCrawlCompleted(object sender, PageCrawlCompletedArgs e)
        {
            CrawledPage crawledPage = e.CrawledPage;

            string poczatek = this.www.AbsoluteUri;
            if(poczatek.EndsWith("/"))
            {
                poczatek=poczatek.Remove(poczatek.Count()-1, 1);
            }


            if (crawledPage.Uri.AbsolutePath != "/" && crawledPage.Uri.AbsolutePath != "/index.html" && crawledPage.Uri.AbsolutePath != "/index.php" && crawledPage.Uri.AbsolutePath != "/index.aspx")
            {
                if (crawledPage.WebException != null || crawledPage.HttpWebResponse.StatusCode != HttpStatusCode.OK)
                {
                    this.Linki_blad.Add(new Link(poczatek + crawledPage.Uri.AbsolutePath, crawledPage.CrawlDepth));
                }
                else
                {
                    this.Linki_ok.Add(new Link(poczatek + crawledPage.Uri.AbsolutePath, crawledPage.CrawlDepth));
                }

                if (string.IsNullOrEmpty(crawledPage.Content.Text))
                {
                    this.linki_puste.Add(new Link(poczatek + crawledPage.Uri.AbsolutePath, crawledPage.CrawlDepth));
                }

            }
        }

        private static IWebCrawler GetManuallyConfiguredWebCrawler()
        {
            //Create a config object manually
            CrawlConfiguration config = new CrawlConfiguration();
            config.CrawlTimeoutSeconds = 0;
            config.DownloadableContentTypes = "text/html, text/plain";

            config.IsExternalPageCrawlingEnabled = false;
            config.IsExternalPageLinksCrawlingEnabled = false;
            config.IsRespectRobotsDotTextEnabled = false;
            config.IsUriRecrawlingEnabled = false;
            config.MaxConcurrentThreads = 10;

            config.MaxCrawlDepth = 50;
            config.MaxPagesToCrawl = 10000;
            config.MaxPagesToCrawlPerDomain = 0;
            config.MinCrawlDelayPerDomainMilliSeconds = 1000;

            //Add you own values without modifying Abot's source code.
            //These are accessible in CrawlContext.CrawlConfuration.ConfigurationException object throughout the crawl
            // config.ConfigurationExtensions.Add("Somekey1", "SomeValue1");
            // config.ConfigurationExtensions.Add("Somekey2", "SomeValue2");

            //Initialize the crawler with custom configuration created above.
            //This override the app.config file values
            return new PoliteWebCrawler(config, null, null, null, null, null, null, null, null);
        }


        public List<Link> Linki_ok
        {
            get
            {
                return linki_ok;
            }

            set
            {
                linki_ok = value;
                OnPropertyChanged("Linki_ok");
            }
        }

        public List<Link> Linki_puste
        {
            get
            {
                return linki_puste;
            }

            set
            {
                linki_puste = value;
                OnPropertyChanged("Linki_puste");
            }
        }

        public List<Link> Linki_blad
        {
            get
            {
                return linki_blad;
            }

            set
            {
                linki_blad = value;
                OnPropertyChanged("Linki_blad");
            }
        }

        public Uri Www
        {
            get
            {
                return www;
            }

            set
            {
                www = value;
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged(string name)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        }
    }
}
