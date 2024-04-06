﻿using Microsoft.EntityFrameworkCore;
using MongoDB.Driver;
using MongoDB.EntityFrameworkCore.Extensions;
using SkymeyJobsLibs.Models.ActualPrices;
using SkymeyJobsLibs.Models.ActualPrices.Binance;
using SkymeyJobsLibs.Models.Tickers;
using SkymeyJobsLibs.Models.Tickers.Tinkoff;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Emit;
using System.Text;
using System.Threading.Tasks;

namespace SkymeyJobsLibs.Data
{
    public class ApplicationContext : DbContext
    {
        #region CRYPTO
        public DbSet<BinanceCurrentPrice> BinanceCurrentPrices { get; init; }
        public DbSet<CurrentPrices> CurrentPrices { get; init; }
        #endregion

        #region STOCKS
        public DbSet<TickerList> TickerList { get; init; }
        public DbSet<TinkoffSharesInstrument> Shares { get; init; }
        #endregion

        public static ApplicationContext Create(IMongoDatabase database) =>
            new(new DbContextOptionsBuilder<ApplicationContext>()
                .UseMongoDB(database.Client, database.DatabaseNamespace.DatabaseName)
                .Options);
        public ApplicationContext(DbContextOptions options)
            : base(options)
        {
        }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.Entity<BinanceCurrentPrice>().ToCollection("crypto_current_binance_prices");
            modelBuilder.Entity<CurrentPrices>().ToCollection("crypto_current_prices");
            modelBuilder.Entity<TickerList>().ToCollection("stock_tickerlist");
            modelBuilder.Entity<TinkoffSharesInstrument>().ToCollection("stock_shareslist");
        }
    }
}