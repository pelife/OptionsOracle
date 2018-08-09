#region License
/*
 * OOServerBovespaB3Provider.Main.cs
 *
 * The MIT License
 *
 * Copyright (c) 2018 Felipe Bahiana Almeida
 *
 * Permission is hereby granted, free of charge, to any person obtaining a copy
 * of this software and associated documentation files (the "Software"), to deal
 * in the Software without restriction, including without limitation the rights
 * to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
 * copies of the Software, and to permit persons to whom the Software is
 * furnished to do so, subject to the following conditions:
 *
 * The above copyright notice and this permission notice shall be included in
 * all copies or substantial portions of the Software.
 *
 * THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
 * IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
 * FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
 * AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
 * LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
 * OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN
 * THE SOFTWARE.
 * 
 * Contributors:
 * - Felipe Bahiana Almeida <felipe.almeida@gmail.com> https://www.linkedin.com/in/felipe-almeida-ba222577
 */
#endregion

namespace OOServerBovespaB3Provider
{
    using B3Provider;
    using OOServerLib.Config;
    using OOServerLib.Global;
    using OOServerLib.Interface;
    using OOServerLib.Web;
    using System;
    using System.Collections;
    using System.Reflection;
    using System.Xml;
    using System.Linq;
    using System.Collections.Generic;
    using B3Provider.Records;

    public class Main : WebSite, IServer
    {
        private B3ProviderConfig _B3Poviderconfig = null;
        private B3ProviderClient _B3PoviderClient = null;
        private Dictionary<string, Quote> _quoteDictionary = null;

        public Main()
        {
            _quoteDictionary = new Dictionary<string, Quote>();
            FeatureList.Add(FeaturesT.SUPPORTS_REALTIME_OPTIONS_CHAIN);
            FeatureList.Add(FeaturesT.SUPPORTS_REALTIME_STOCK_QUOTE);
            FeatureList.Add(FeaturesT.SUPPORTS_STOCK_HISTORICAL_DATA);
            FeatureList.Add(FeaturesT.SUPPORTS_OPTIONS_HISTORICAL_DATA);
            FeatureList.Add(FeaturesT.SUPPORTS_HISTORICAL_VOLATILITY);
            FeatureList.Add(FeaturesT.SUPPORTS_INTEREST_RATE);

            // update server list
            ServerList.Add(Name);
            _B3Poviderconfig = new B3ProviderConfig();
            _B3PoviderClient = new B3ProviderClient(_B3Poviderconfig);
        }

        public ArrayList FeatureList { get; private set; } = new ArrayList();

        public ArrayList ServerList { get; private set; } = new ArrayList();

        public ArrayList ModeList { get; private set; } = new ArrayList();

        public int DisplayAccuracy { get { return 2; } }

        public string Author { get { return "Felipe Bahiana Almeida"; } }

        public string Description { get { return "B3Provider Plugin for B3 former (BM&F e Bovespa) Brazil"; } }

        public string Name { get { return "PlugIn Server B3Provider Brazil former (BM&F e Bovespa) Brazil"; } }

        public string Version { get { return Assembly.GetExecutingAssembly().GetName().Version.ToString(); } }

        public string Username { get; set; }

        public string Password { get; set; }

        public string Configuration { get; set; }

        public string Server { get { return Name; } set { } } // not-supported

        public string Mode { get { return null; } set { } }

        public IServerHost Host { get; set; }

        public bool Connect { get { return true; } set { } }

        public bool LogEnable { get; set; }

        public string DebugLog { get; private set; }

        public string DefaultSymbol { get { return "PETR4"; } }

        public string Authentication(string oo_version, string phrase)
        {
            try
            {
                Crypto crypto = new Crypto(@"<RSAKeyValue><Modulus>otysuKHd8wjQn9Oe9m3zAJ1oXtgs9ukfvBOeEjgM/xIMpAk3pFbyT6lGBjGjBvdMTP4kyMRgBYT1SXUXKU85VulcJjvTVH6kCfq04fktoZrKswahz7XCs5tmt7E1yxnavfZddSdhwOWyjgYyCVjXMpOKIZc04XeSJO6COYptQV8=</Modulus><Exponent>AQAB</Exponent><P>0TRDDBI6gZvxDZokegiocMKejl5RINKSEGc7kHARB3G0MwZ1ZvrOaHMsDeS+feHZlX1MGIJUcP0oM0UdmWXuIw==</P><Q>x0q0fPbhLbM06hNiSCIWDxwC5yNprrLEuyJlqTKQFPTd1xZJ6wLf0c/Zr93KeTaepR7nMBdSsABm16ivo+StlQ==</Q><DP>Rpdd8FrORyG5ix9yI4N8YuAo5F1K/spO4x4SaUCHXn2tknIhd2g18eS6/s0qwgtNgjXPUY3YtG+X+wTdYf+VBQ==</DP><DQ>PxMPyLVCU3pydtsnsfjHzoRpDsqQejAuP6QFVOWh4GAXjimJv42rVPZZyWWC3ZZB47TCKuBW1UlrQzoqTM7leQ==</DQ><InverseQ>Pu9T/OTeCLirNvs/pc4CS3fGfPlNA0K9SpaNyWQMi8FIW9q8ggCCoyVxc3Ij3Ote6cl1xTXa7LRyn3kbtJOiIw==</InverseQ><D>DB1UL8vCodB3DFyGh5g4KkSLPfrgpWFD/g6LhJlsxhCGpjEVVYEuNyTFU7KfiOYeY9/HxrNs3Rw9zsAKAAWnoyQHv/CGwGET1H4xLuTRrykShGACPeu+hsfjj0dHyCjVWmsRiTUdY5IjEsUoniknMd9pm393ZoiINvod0UyPljk=</D></RSAKeyValue>");
                return crypto.Decrypt(phrase);
            }
            catch { return ""; }
        }

        public void Dispose()
        {
            Connect = false;
        }

        public double GetAnnualInterestRate(double duration)
        {
            return 6.5;
            /*
            XmlDocument xml = cap.DownloadXmlPartialWebPage(@"http://www.bcb.gov.br/?english", "<body", "</body>", 1, 1);
            if (xml == null) return double.NaN;

            XmlNode nd = prs.GetXmlNodeByPath(xml.FirstChild, @"body\table\tr\td(3)\table\tr\td(3)\table(2)\tr(5)\td\table\tr(4)\td(2)");
            if (nd == null) return double.NaN;

            try
            {
                return double.Parse(nd.InnerText.Replace("%", "").Trim());
            }
            catch { }

            return double.NaN;
            */
        }

        public ArrayList GetHistoricalData(string ticker, DateTime start, DateTime end)
        {
            // history list
            ArrayList _historyQuotes = new ArrayList();

            if (_B3PoviderClient.HistoricMarketData == null)
                _B3PoviderClient.LoadHistoricQuotes(2018); // vamos pegar os dados de 2017 tambem ?

            var quotes = _B3PoviderClient.HistoricMarketData.Where(hq => hq.Ticker.Equals(ticker, StringComparison.InvariantCultureIgnoreCase)).OrderByDescending(hq => hq.TradeDate).ToList(); // capar pela data.

            if (quotes != null && quotes.Count > 0)
            {
                foreach (B3HistoricMarketDataInfo oneQuote in quotes)
                {

                    // Date, Time, Open, High, Low, Close, Q tt, volume, Business, Status
                    History history = new History();

                    history.stock = ticker;

                    history.date = oneQuote.TradeDate;
                    history.price.open = oneQuote.Opening;
                    history.price.high = oneQuote.Maximum;
                    history.price.low = oneQuote.Minimum;
                    history.price.close = oneQuote.Last;
                    history.price.close_adj = oneQuote.Last;
                    history.volume.total = oneQuote.FinancialVolume;

                    _historyQuotes.Add(history);

                }
            }

            return _historyQuotes;
        }

        public double GetHistoricalVolatility(string ticker, double duration)
        {
            // get historical data
            ArrayList list = GetHistoricalData(ticker, DateTime.Now.AddDays(-duration * 365), DateTime.Now);

            // calculate historical value
            return 100.0 * HistoryVolatility.HighLowParkinson(list);
        }

        public ArrayList GetOptionsChain(string ticker)
        {
            ArrayList _optionsFound = new ArrayList();
            Option _option = null;

            if (_B3PoviderClient.EquityInstruments == null || _B3PoviderClient.OptionInstruments == null)
                _B3PoviderClient.LoadInstruments();

            if (_B3PoviderClient.CurrentMarketData == null)
                _B3PoviderClient.LoadQuotes();

            var equityInstrument = _B3PoviderClient.EquityInstruments.Where(ins => ins.Ticker.Equals(ticker, StringComparison.InvariantCultureIgnoreCase)).FirstOrDefault();
            if (equityInstrument == null)
                throw new ArgumentException("ticker");

            var options = _B3PoviderClient.OptionInstruments.Where(ins =>
                ins.B3IDUnderlying.HasValue &&
                ins.B3IDUnderlying.Value == equityInstrument.B3ID.Value &&
                ins.Expiration.HasValue &&
                ins.Expiration > DateTime.Today).ToList(); // valid options only

            if (options != null && options.Count > 0)
            {
                foreach (B3OptionOnEquityInfo oneOption in options)
                {
                    _option = new Option();
                    var quote = _B3PoviderClient.CurrentMarketData.Where(ins => ins.Ticker.Equals(oneOption.Ticker, StringComparison.InvariantCultureIgnoreCase)).FirstOrDefault();

                    _option.stock = ticker;
                    _option.symbol = oneOption.Ticker;
                    _option.stocks_per_contract = 1;
                    if (oneOption.Type == B3OptionOnEquityTypeInfo.Call)
                        _option.type = "Call";
                    if (oneOption.Type == B3OptionOnEquityTypeInfo.Put)
                        _option.type = "Put";
                    _option.expiration = oneOption.Expiration.Value;
                    _option.strike = oneOption.Strike.Value;

                    if (quote != null)
                    {
                        _option.price.ask = quote.BestAskPrice.HasValue ? quote.BestAskPrice.Value : 0;
                        _option.price.bid = quote.BestBidPrice.HasValue ? quote.BestBidPrice.Value : 0;
                        _option.price.last = quote.LastPrice.HasValue ? quote.LastPrice.Value : 0;
                        _option.price.change = (quote.LastPrice.HasValue && quote.FirstPrice.HasValue) ? quote.LastPrice.Value - quote.FirstPrice.Value : 0.0;
                        _option.volume.total = quote.NationalFinancialVolume.HasValue ? quote.NationalFinancialVolume.Value : 0.0;
                    }

                    _optionsFound.Add(_option);
                }
            }
            return _optionsFound;
        }

        public string GetParameter(string name)
        {
            throw new NotImplementedException();
        }

        public ArrayList GetParameterList(string name)
        {
            throw new NotImplementedException();
        }

        public Quote GetQuote(string ticker)
        {
            Quote _quoteOfTicker = null;

            if (_B3PoviderClient.CurrentMarketData == null)
                _B3PoviderClient.LoadQuotes();

            var quote = _B3PoviderClient.CurrentMarketData.Where(ins => ins.Ticker.Equals(ticker, StringComparison.InvariantCultureIgnoreCase)).FirstOrDefault();

            if (quote != null)
            {
                _quoteOfTicker = new Quote();
                _quoteOfTicker.name = quote.Ticker;
                _quoteOfTicker.stock = quote.Ticker;
                _quoteOfTicker.price.open = quote.FirstPrice.Value;
                _quoteOfTicker.price.last = quote.LastPrice.Value;
                _quoteOfTicker.price.change = quote.LastPrice.Value - quote.FirstPrice.Value;
                _quoteOfTicker.price.high = quote.MaximumPrice.Value;
                _quoteOfTicker.price.low = quote.MinimumPrice.Value;
                _quoteOfTicker.price.ask = quote.BestAskPrice.Value;
                _quoteOfTicker.price.bid = quote.BestBidPrice.Value;
                _quoteOfTicker.update_timestamp = quote.TradeDate.Value;
                _quoteOfTicker.volume.total = (double)quote.NationalFinancialVolume.Value;

                _quoteDictionary[ticker] = _quoteOfTicker;
            }

            return _quoteOfTicker;
        }

        public ArrayList GetStockSymbolLookup(string name)
        {
            throw new NotImplementedException();
        }

        public void Initialize(string config)
        {
            _B3PoviderClient.Setup();
        }

        public void SetParameter(string name, string value)
        {
            throw new NotImplementedException();
        }

        public void SetParameterList(string name, ArrayList value)
        {
            throw new NotImplementedException();
        }

        public void ShowConfigForm(object parent)
        {
            throw new NotImplementedException();
        }
    }
}
