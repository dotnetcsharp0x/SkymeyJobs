﻿using MongoDB.Driver;
using SkymeyJobsLibs.Data;
using SkymeyJobsLibs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tinkoff.InvestApi;
using Google.Protobuf.WellKnownTypes;
using MongoDB.Bson;
using SkymeyJobsLibs.Models.Tickers.Tinkoff;

namespace SkymeyTinkoffSharesList.Actions.GetShares.Tinkoff
{
    public class GetShares
    {
        private static InvestApiClient? investApiClient = InvestApiClientFactory.Create(Config.TinkoffAPI);
        private static MongoClient _mongoClient = new MongoClient(Config.MongoClientConnection);
        private static ApplicationContext _db = ApplicationContext.Create(_mongoClient.GetDatabase(Config.MongoDbDatabase));
        public static async Task GetSharesFromTinkoff()
        {
            var response = investApiClient.Instruments.Shares();
            var ticker_finds = (from i in _db.Bonds select i);
            foreach (var item in response.Instruments)
            {
                //Console.WriteLine(item.Ticker);
                var ticker_find = (from i in ticker_finds where i.ticker == item.Ticker && i.figi == item.Figi select i).FirstOrDefault();
                if (ticker_find == null)
                {
                    TinkoffSharesInstrument tsi = new TinkoffSharesInstrument();
                    tsi._id = ObjectId.GenerateNewId();
                    tsi.figi = item.Figi;
                    if (tsi.figi == null) tsi.figi = "";
                    tsi.ticker = item.Ticker;
                    if (tsi.ticker == null) tsi.ticker = "";
                    tsi.classCode = item.ClassCode;
                    if (tsi.classCode == null) tsi.classCode = "";
                    tsi.isin = item.Isin;
                    if (tsi.isin == null) tsi.isin = "";
                    tsi.lot = item.Lot;
                    if (tsi.lot == null) tsi.lot = 0;
                    tsi.currency = item.Currency;
                    if (tsi.currency == null) tsi.currency = "";
                    if (item.Klong != null)
                    {
                        TinkoffSharesKlong tskl = new TinkoffSharesKlong();
                        tskl.units = item.Klong.Units;
                        tskl.nano = item.Klong.Nano;
                        tsi.klong = tskl;
                    }
                    else
                    {
                        tsi.klong = new TinkoffSharesKlong();
                    }
                    if (item.Kshort != null)
                    {
                        TinkoffSharesKshort tsks = new TinkoffSharesKshort();
                        tsks.units = item.Kshort.Units;
                        tsks.nano = item.Kshort.Nano;
                        tsi.kshort = tsks;
                    }
                    else
                    {
                        tsi.kshort = new TinkoffSharesKshort();
                    }
                    if (item.Dlong != null)
                    {
                        TinkoffSharesDlong tsdl = new TinkoffSharesDlong();
                        tsdl.units = item.Dlong.Units;
                        tsdl.nano = item.Dlong.Nano;
                        tsi.dlong = tsdl;
                    }
                    else
                    {
                        tsi.dlong = new TinkoffSharesDlong();
                    }
                    if (item.Dshort != null)
                    {
                        TinkoffSharesDshort tsds = new TinkoffSharesDshort();
                        tsds.units = item.Dshort.Units;
                        tsds.nano = item.Dshort.Nano;
                        tsi.dshort = tsds;
                    }
                    else
                    {
                        tsi.dshort = new TinkoffSharesDshort();
                    }
                    if (item.DlongMin != null)
                    {
                        TinkoffSharesDlongMin tsdlm = new TinkoffSharesDlongMin();
                        tsdlm.units = item.DlongMin.Units;
                        tsdlm.nano = item.DlongMin.Nano;
                        tsi.dlongMin = tsdlm;
                    }
                    else
                    {
                        tsi.dlongMin = new TinkoffSharesDlongMin();
                    }
                    if (item.DshortMin != null)
                    {
                        TinkoffSharesDshortMin tsdsm = new TinkoffSharesDshortMin();
                        tsdsm.units = item.DshortMin.Units;
                        tsdsm.nano = item.DshortMin.Nano;
                        tsi.dshortMin = tsdsm;
                    }
                    else
                    {
                        tsi.dshortMin = new TinkoffSharesDshortMin();
                    }
                    tsi.shortEnabledFlag = item.ShortEnabledFlag;
                    if (tsi.shortEnabledFlag == null) tsi.shortEnabledFlag = false;
                    tsi.name = item.Name;
                    if (tsi.name == null) tsi.name = "";
                    tsi.exchange = item.Exchange;
                    if (tsi.exchange == null) tsi.exchange = "";
                    tsi.issueSize = item.IssueSize;
                    if (tsi.issueSize == null) tsi.issueSize = 0;
                    tsi.countryOfRisk = item.CountryOfRisk;
                    if (tsi.countryOfRisk == null) tsi.countryOfRisk = "";
                    tsi.countryOfRiskName = item.CountryOfRiskName;
                    if (tsi.countryOfRiskName == null) tsi.countryOfRiskName = "";
                    tsi.sector = item.Sector;
                    if (tsi.sector == null) tsi.sector = "";
                    tsi.issueSizePlan = item.IssueSizePlan;
                    if (tsi.issueSizePlan == null) tsi.issueSizePlan = 0;
                    if (item.Nominal != null)
                    {
                        TinkoffSharesNominal tsn = new TinkoffSharesNominal();
                        tsn.currency = item.Nominal.Currency;
                        tsn.units = item.Nominal.Units;
                        tsn.nano = item.Nominal.Nano;
                        tsi.nominal = tsn;
                    }
                    else
                    {
                        tsi.nominal = new TinkoffSharesNominal();
                    }
                    tsi.tradingStatus = item.TradingStatus.ToString();
                    if (tsi.tradingStatus == null) tsi.tradingStatus = "";
                    tsi.otcFlag = item.OtcFlag;
                    if (tsi.otcFlag == null) tsi.otcFlag = false;
                    tsi.buyAvailableFlag = item.BuyAvailableFlag;
                    if (tsi.buyAvailableFlag == null) tsi.buyAvailableFlag = false;
                    tsi.sellAvailableFlag = item.SellAvailableFlag;
                    if (tsi.sellAvailableFlag == null) tsi.sellAvailableFlag = false;
                    tsi.divYieldFlag = item.DivYieldFlag;
                    if (tsi.divYieldFlag == null) { tsi.divYieldFlag = false; }
                    tsi.shareType = item.ShareType.ToString();
                    if (tsi.shareType == null) tsi.shareType = "";
                    if (item.MinPriceIncrement != null)
                    {
                        TinkoffSharesMinPriceIncrement tsmpi = new TinkoffSharesMinPriceIncrement();
                        tsmpi.units = item.MinPriceIncrement.Units;
                        tsmpi.nano = item.MinPriceIncrement.Nano;
                        tsi.minPriceIncrement = tsmpi;
                    }
                    else
                    {
                        tsi.minPriceIncrement = new TinkoffSharesMinPriceIncrement();
                    }
                    tsi.apiTradeAvailableFlag = item.ApiTradeAvailableFlag;
                    if (tsi.apiTradeAvailableFlag == null) tsi.apiTradeAvailableFlag = false;
                    tsi.uid = item.Uid;
                    if (tsi.uid == null) { tsi.uid = ""; }
                    tsi.realExchange = item.RealExchange.ToString();
                    if (tsi.realExchange == null) { tsi.realExchange = ""; }
                    tsi.positionUid = item.PositionUid;
                    if (tsi.positionUid == null) { tsi.positionUid = ""; }
                    tsi.forIisFlag = item.ForIisFlag;
                    if (tsi.forIisFlag == null) tsi.forIisFlag = false;
                    tsi.forQualInvestorFlag = item.ForQualInvestorFlag;
                    if (tsi.forQualInvestorFlag == null) tsi.forQualInvestorFlag = false;
                    tsi.weekendFlag = item.WeekendFlag;
                    if (tsi.weekendFlag == null) { tsi.weekendFlag = false; }
                    tsi.blockedTcaFlag = item.BlockedTcaFlag;
                    if (tsi.blockedTcaFlag == null) { tsi.blockedTcaFlag = false; }
                    tsi.liquidityFlag = item.LiquidityFlag;
                    if (tsi.liquidityFlag == null) { tsi.liquidityFlag = false; }
                    tsi.first1minCandleDate = item.First1MinCandleDate;
                    if (tsi.first1minCandleDate == null) { tsi.first1minCandleDate = Timestamp.FromDateTime(DateTime.UtcNow); }
                    tsi.first1dayCandleDate = item.First1DayCandleDate;
                    if (tsi.first1dayCandleDate == null) { tsi.first1dayCandleDate = Timestamp.FromDateTime(DateTime.UtcNow); }
                    tsi.ipoDate = item.IpoDate;
                    if (tsi.ipoDate == null) tsi.ipoDate = Timestamp.FromDateTime(DateTime.UtcNow);
                    tsi.Update = DateTime.UtcNow;
                    _db.Shares.Add(tsi);
                }
            }
            _db.SaveChanges();
        }
    }
}
