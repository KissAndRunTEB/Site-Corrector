using Google.Apis.Pagespeedonline.v2;
using Google.Apis.Services;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Site_Corrector.Logika
{
    class PageSpeed : INotifyPropertyChanged
    {
        string url;

        WynikPS desktop;
        WynikPS smartfony;

        public PageSpeed(string url)
        {
            this.Url = url;

            string kluczAPI = "AIzaSyAo_ZuCkRZHhLghE2wFX1VyypMLn4emGHY";

            // Create the service.
            var service = new PagespeedonlineService(new BaseClientService.Initializer
            {
                ApplicationName = "SC",
                ApiKey = kluczAPI
            });

            // Run the request.          
            //var result = service.Pagespeedapi.Runpagespeed(url).ExecuteAsync();

            var request = service.Pagespeedapi.Runpagespeed(url);

            request.Locale = "pl_PL";
            // request.Screenshot = true;

            request.Strategy = Google.Apis.Pagespeedonline.v2.PagespeedapiResource.RunpagespeedRequest.StrategyEnum.Desktop;
            Google.Apis.Pagespeedonline.v2.Data.Result d = request.Execute();

            WynikPS wyniki_d = new WynikPS();

            wyniki_d.Css_rozmiar=(int)Math.Ceiling(d.PageStats.CssResponseBytes.GetValueOrDefault() / 1024f);
            wyniki_d.Flash_rozmiar = (int)Math.Ceiling(d.PageStats.FlashResponseBytes.GetValueOrDefault() / 1024f);
            wyniki_d.Html_rozmiar = (int)Math.Ceiling(d.PageStats.HtmlResponseBytes.GetValueOrDefault() / 1024f);
            wyniki_d.Zdjecia_rozmiar = (int)Math.Ceiling(d.PageStats.ImageResponseBytes.GetValueOrDefault() / 1024f);
            wyniki_d.Js_rozmiar = (int)Math.Ceiling(d.PageStats.JavascriptResponseBytes.GetValueOrDefault() / 1024f);
            wyniki_d.Inne_rozmiar = (int)Math.Ceiling(d.PageStats.OtherResponseBytes.GetValueOrDefault() / 1024f);
            wyniki_d.Text_rozmiar = (int)Math.Ceiling(d.PageStats.TextResponseBytes.GetValueOrDefault() / 1024f);
            wyniki_d.Wszystko_rozmiar = (int)Math.Ceiling(d.PageStats.TotalRequestBytes.GetValueOrDefault() / 1024f);

            wyniki_d.Css_ile = (int)d.PageStats.NumberCssResources.GetValueOrDefault();
            wyniki_d.Js_ile = (int)d.PageStats.NumberJsResources.GetValueOrDefault();
            wyniki_d.Hosty_ile = (int)d.PageStats.NumberHosts.GetValueOrDefault();
            wyniki_d.Zasoby_ile = (int)d.PageStats.NumberResources.GetValueOrDefault();
            wyniki_d.Statyczne_zasoby_ile = (int)d.PageStats.NumberStaticResources.GetValueOrDefault();



            //d.FormattedResults.RuleResults


            this.Desktop = wyniki_d;

            request.Strategy = Google.Apis.Pagespeedonline.v2.PagespeedapiResource.RunpagespeedRequest.StrategyEnum.Mobile;
            Google.Apis.Pagespeedonline.v2.Data.Result s = request.Execute();

            WynikPS wyniki_s = new WynikPS();

            wyniki_s.Css_rozmiar = (int)Math.Ceiling(s.PageStats.CssResponseBytes.GetValueOrDefault() / 1024f);
            wyniki_s.Flash_rozmiar = (int)Math.Ceiling(s.PageStats.FlashResponseBytes.GetValueOrDefault() / 1024f);
            wyniki_s.Html_rozmiar = (int)Math.Ceiling(s.PageStats.HtmlResponseBytes.GetValueOrDefault() / 1024f);
            wyniki_s.Zdjecia_rozmiar = (int)Math.Ceiling(s.PageStats.ImageResponseBytes.GetValueOrDefault() / 1024f);
            wyniki_s.Js_rozmiar = (int)Math.Ceiling(s.PageStats.JavascriptResponseBytes.GetValueOrDefault() / 1024f);
            wyniki_s.Inne_rozmiar = (int)Math.Ceiling(s.PageStats.OtherResponseBytes.GetValueOrDefault() / 1024f);
            wyniki_s.Text_rozmiar = (int)Math.Ceiling(s.PageStats.TextResponseBytes.GetValueOrDefault() / 1024f);
            wyniki_s.Wszystko_rozmiar = (int)Math.Ceiling(s.PageStats.TotalRequestBytes.GetValueOrDefault() / 1024f);

            wyniki_s.Css_ile = (int)s.PageStats.NumberCssResources.GetValueOrDefault();
            wyniki_s.Js_ile = (int)s.PageStats.NumberJsResources.GetValueOrDefault();
            wyniki_s.Hosty_ile = (int)s.PageStats.NumberHosts.GetValueOrDefault();
            wyniki_s.Zasoby_ile = (int)s.PageStats.NumberResources.GetValueOrDefault();
            wyniki_s.Statyczne_zasoby_ile = (int)s.PageStats.NumberStaticResources.GetValueOrDefault();

           // Google.Apis.Pagespeedonline.v2.Data.Result.FormattedResultsData.RuleResultsDataElement z_reguly = s.FormattedResults.RuleResults.First();
            //s.FormattedResults.RuleResults

            this.Smartfony = wyniki_s;
        }

        private string Url
        {
            get
            {
                return url;
            }

            set
            {
                url = value;
                OnPropertyChanged("Url");
            }
        }

        internal WynikPS Desktop
        {
            get
            {
                return desktop;
            }

            set
            {
                desktop = value;
                OnPropertyChanged("Desktop");
            }
        }

        internal WynikPS Smartfony
        {
            get
            {
                return smartfony;
            }

            set
            {
                smartfony = value;
                OnPropertyChanged("Smartfony");
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        // Create the OnPropertyChanged method to raise the event 
        protected void OnPropertyChanged(string name)
        {
            PropertyChangedEventHandler handler = PropertyChanged;
            if (handler != null)
            {
                handler(this, new PropertyChangedEventArgs(name));
            }
        }
    }


    class WynikPS : INotifyPropertyChanged
    {
        int css_rozmiar;
        int js_rozmiar;
        int flash_rozmiar;
        int html_rozmiar;
        int text_rozmiar;
        int zdjecia_rozmiar;
        int inne_rozmiar;
        int wszystko_rozmiar;

        int css_ile;
        int js_ile;
        int hosty_ile;
        int zasoby_ile;
        int statyczne_zasoby_ile;

        public int Css_rozmiar
        {
            get
            {
                return css_rozmiar;
            }

            set
            {
                css_rozmiar = value;
                OnPropertyChanged("Css_rozmiar");
            }
        }

        public int Js_rozmiar
        {
            get
            {
                return js_rozmiar;
            }

            set
            {
                js_rozmiar = value;
                OnPropertyChanged("Js_rozmiar");
            }
        }

        public int Flash_rozmiar
        {
            get
            {
                return flash_rozmiar;
            }

            set
            {
                flash_rozmiar = value;
                OnPropertyChanged("Flash_rozmiar");
            }
        }

        public int Html_rozmiar
        {
            get
            {
                return html_rozmiar;
            }

            set
            {
                html_rozmiar = value;
                OnPropertyChanged("Html_rozmiar");
            }
        }

        public int Text_rozmiar
        {
            get
            {
                return text_rozmiar;
            }

            set
            {
                text_rozmiar = value;
                OnPropertyChanged("Text_rozmiar");
            }
        }

        public int Zdjecia_rozmiar
        {
            get
            {
                return zdjecia_rozmiar;
            }

            set
            {
                zdjecia_rozmiar = value;
                OnPropertyChanged("Zdjecia_rozmiar");
            }
        }

        public int Inne_rozmiar
        {
            get
            {
                return inne_rozmiar;
            }

            set
            {
                inne_rozmiar = value;
                OnPropertyChanged("Inne_rozmiar");
            }
        }

        public int Wszystko_rozmiar
        {
            get
            {
                return wszystko_rozmiar;
            }

            set
            {
                wszystko_rozmiar = value;
                OnPropertyChanged("Wszystko_rozmiar");
            }
        }

        public int Css_ile
        {
            get
            {
                return css_ile;
            }

            set
            {
                css_ile = value;
                OnPropertyChanged("Css_ile");
            }
        }

        public int Js_ile
        {
            get
            {
                return js_ile;
            }

            set
            {
                js_ile = value;
                OnPropertyChanged("Js_ile");
            }
        }

        public int Hosty_ile
        {
            get
            {
                return hosty_ile;
            }

            set
            {
                hosty_ile = value;
                OnPropertyChanged("Hosty_ile");
            }
        }

        public int Zasoby_ile
        {
            get
            {
                return zasoby_ile;
            }

            set
            {
                zasoby_ile = value;
                OnPropertyChanged("Zasoby_ile");
            }
        }

        public int Statyczne_zasoby_ile
        {
            get
            {
                return statyczne_zasoby_ile;
            }

            set
            {
                statyczne_zasoby_ile = value;
                OnPropertyChanged("Statyczne_zasoby_ile");
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        // Create the OnPropertyChanged method to raise the event 
        protected void OnPropertyChanged(string name)
        {
            PropertyChangedEventHandler handler = PropertyChanged;
            if (handler != null)
            {
                handler(this, new PropertyChangedEventArgs(name));
            }
        }
    }

}
