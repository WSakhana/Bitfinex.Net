﻿using Bitfinex.Net.Converters;
using Bitfinex.Net.Interfaces;
using Bitfinex.Net.Objects;
using Bitfinex.Net.Objects.RestV1Objects;
using CryptoExchange.Net;
using CryptoExchange.Net.Authentication;
using CryptoExchange.Net.Converters;
using CryptoExchange.Net.Objects;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Threading.Tasks;

namespace Bitfinex.Net
{
    public class BitfinexClient : RestClient, IBitfinexClient
    {
        #region fields

        private static BitfinexClientOptions defaultOptions = new BitfinexClientOptions();
        

        private static BitfinexClientOptions DefaultOptions => defaultOptions.Copy<BitfinexClientOptions>();

        private const string ApiVersion1 = "1";
        private const string ApiVersion2 = "2";

        private const string StatusEndpoint = "platform/status";
        private const string SymbolsEndpoint = "symbols";
        private const string SymbolDetailsEndpoint = "symbols_details";
        private const string TickersEndpoint = "tickers";
        private const string TradesEndpoint = "trades/{}/hist";
        
        private const string OrderBookEndpoint = "book/{}/{}";
        private const string StatsEndpoint = "stats1/{}:1m:{}:{}/{}";
        private const string LastCandleEndpoint = "candles/trade:{}:{}/last";
        private const string CandlesEndpoint = "candles/trade:{}:{}/hist";
        private const string MarketAverageEndpoint = "calc/trade/avg";

        private const string WalletsEndpoint = "auth/r/wallets";
        private const string CalcAvailableBalanceEndpoint = "auth/calc/order/avail";
        private const string OpenOrdersEndpoint = "auth/r/orders";
        
        private const string OrderHistoryEndpoint = "auth/r/orders/{}/hist";
        private const string OrderTradesEndpoint = "auth/r/order/{}:{}/trades";
        private const string MyTradesEndpoint = "auth/r/trades/{}/hist";
        private const string UserInfoEndpoint = "auth/r/info/user";

        private const string ActivePositionsEndpoint = "auth/r/positions";
        private const string ActiveFundingOffersEndpoint = "auth/r/funding/offers/{}";
        private const string FundingOfferHistoryEndpoint = "auth/r/funding/offers/{}/hist";
        private const string FundingLoansEndpoint = "auth/r/funding/loans/{}";
        private const string FundingLoansHistoryEndpoint = "auth/r/funding/loans/{}/hist";
        private const string FundingCreditsEndpoint = "auth/r/funding/credits/{}";
        private const string FundingCreditsHistoryEndpoint = "auth/r/funding/credits/{}/hist";
        private const string FundingTradesEndpoint = "auth/r/funding/trades/{}/hist";
        private const string MarginInfoBaseEndpoint = "auth/r/info/margin/base";
        private const string MarginInfoSymbolEndpoint = "auth/r/info/margin/{}";
        private const string FundingInfoEndpoint = "auth/r/info/funding/{}";

        private const string MovementsEndpoint = "auth/r/movements/{}/hist";
        private const string DailyPerformanceEndpoint = "auth/r/stats/perf:1D/hist";

        private const string AlertListEndpoint = "auth/r/alerts";
        private const string SetAlertEndpoint = "auth/w/alert/set";
        private const string DeleteAlertEndpoint = "auth/w/alert/price:{}:{}/del";

        private const string AccountInfoEndpoint = "account_infos";
        private const string WithdrawalFeeEndpoint = "account_fees";
        private const string PlaceOrderEndpoint = "order/new";
        private const string CancelOrderEndpoint = "order/cancel";
        private const string CancelAllOrderEndpoint = "order/cancel/all";
        private const string OrderStatusEndpoint = "order/status";
        private const string OrdersHistoryEndpoint = "orders/hist";
        private const string WithdrawEndpoint = "withdraw";
        #endregion

        #region constructor/destructor
        /// <summary>
        /// Create a new instance of BitfinexClient using the default options
        /// </summary>
        public BitfinexClient() : this(DefaultOptions)
        {
        }

        /// <summary>
        /// Create a new instance of BitfinexClient using provided options
        /// </summary>
        /// <param name="options">The options to use for this client</param>
        public BitfinexClient(BitfinexClientOptions options) : base(options, options.ApiCredentials == null ? null : new BitfinexAuthenticationProvider(options.ApiCredentials))
        {
            Configure(options);
        }
        #endregion

        #region methods
        /// <summary>
        /// Sets the default options to use for new clients
        /// </summary>
        /// <param name="options">The options to use for new clients</param>
        public static void SetDefaultOptions(BitfinexClientOptions options)
        {
            defaultOptions = options;
        }

        /// <summary>
        /// Set the API key and secret
        /// </summary>
        /// <param name="apiKey">The api key</param>
        /// <param name="apiSecret">The api secret</param>
        public void SetApiCredentials(string apiKey, string apiSecret)
        {
            SetAuthenticationProvider(new BitfinexAuthenticationProvider(new ApiCredentials(apiKey, apiSecret)));
        }

        #region Version2
        /// <summary>
        /// Gets the platform status
        /// </summary>
        /// <returns>Whether Bitfinex platform is running normally or not</returns>
        public CallResult<BitfinexPlatformStatus> GetPlatformStatus() => GetPlatformStatusAsync().Result;

        /// <summary>
        /// Gets the platform status
        /// </summary>
        /// <returns>Whether Bitfinex platform is running normally or not</returns>
        public async Task<CallResult<BitfinexPlatformStatus>> GetPlatformStatusAsync()
        {
            return await ExecuteRequest<BitfinexPlatformStatus>(GetUrl(StatusEndpoint, ApiVersion2)).ConfigureAwait(false);
        }

        /// <summary>
        /// Gets a list of all symbols
        /// </summary>
        /// <returns></returns>
        public CallResult<string[]> GetSymbols() => GetSymbolsAsync().Result;

        /// <summary>
        /// Gets a list of all symbols
        /// </summary>
        /// <returns></returns>
        public async Task<CallResult<string[]>> GetSymbolsAsync()
        {
            return await ExecuteRequest<string[]>(GetUrl(SymbolsEndpoint, ApiVersion1)).ConfigureAwait(false);
        }

        /// <summary>
        /// Gets details of all symbols
        /// </summary>
        /// <returns></returns>
        public CallResult<BitfinexSymbolDetails[]> GetSymbolDetails() => GetSymbolDetailsAsync().Result;

        /// <summary>
        /// Gets details of all symbols
        /// </summary>
        /// <returns></returns>
        public async Task<CallResult<BitfinexSymbolDetails[]>> GetSymbolDetailsAsync()
        {
            return await ExecuteRequest<BitfinexSymbolDetails[]>(GetUrl(SymbolDetailsEndpoint, ApiVersion1)).ConfigureAwait(false);
        }

        /// <summary>
        /// Returns basic market data for the provided symbols
        /// </summary>
        /// <param name="symbols">The symbols to get data for</param>
        /// <returns>Market data</returns>
        public CallResult<BitfinexMarketOverviewRest[]> GetTicker(params string[] symbols) => GetTickerAsync(symbols).Result;

        /// <summary>
        /// Returns basic market data for the provided symbols
        /// </summary>
        /// <param name="symbols">The symbols to get data for</param>
        /// <returns>Market data</returns>
        public async Task<CallResult<BitfinexMarketOverviewRest[]>> GetTickerAsync(params string[] symbols)
        {
            var parameters = new Dictionary<string, object>()
            {
                {"symbols", string.Join(",", symbols)}
            };

            return await ExecuteRequest<BitfinexMarketOverviewRest[]>(GetUrl(TickersEndpoint, ApiVersion2), Constants.GetMethod, parameters).ConfigureAwait(false);
        }

        /// <summary>
        /// Get recent trades for a symbol
        /// </summary>
        /// <param name="symbol">The symbol to get trades for</param>
        /// <param name="limit">The amount of results</param>
        /// <param name="startTime">The start time to return trades for</param>
        /// <param name="endTime">The end time to return trades for</param>
        /// <param name="sorting">The way the result is sorted</param>
        /// <returns>Trades for the symbol</returns>
        public CallResult<BitfinexTradeSimple[]> GetTrades(string symbol, int? limit = null, DateTime? startTime = null, DateTime? endTime = null, Sorting? sorting = null) => GetTradesAsync(symbol, limit, startTime, endTime, sorting).Result;

        /// <summary>
        /// Get recent trades for a symbol
        /// </summary>
        /// <param name="symbol">The symbol to get trades for</param>
        /// <param name="limit">The amount of results</param>
        /// <param name="startTime">The start time to return trades for</param>
        /// <param name="endTime">The end time to return trades for</param>
        /// <param name="sorting">The way the result is sorted</param>
        /// <returns>Trades for the symbol</returns>
        public async Task<CallResult<BitfinexTradeSimple[]>> GetTradesAsync(string symbol, int? limit = null, DateTime? startTime = null, DateTime? endTime = null, Sorting? sorting = null)
        {
            // Only accepts tBTCUSD format
            if (symbol.Length == 6)
            {
                symbol = "t" + symbol.ToUpper();
            }

            var parameters = new Dictionary<string, object>();
            parameters.AddOptionalParameter("limit", limit?.ToString());
            parameters.AddOptionalParameter("start", startTime != null ? JsonConvert.SerializeObject(startTime, new TimestampConverter()) : null);
            parameters.AddOptionalParameter("end", endTime != null ? JsonConvert.SerializeObject(endTime, new TimestampConverter()) : null);
            parameters.AddOptionalParameter("sort", sorting != null ? JsonConvert.SerializeObject(sorting, new SortingConverter(false)) : null);

            return await ExecuteRequest<BitfinexTradeSimple[]>(GetUrl(FillPathParameter(TradesEndpoint, symbol), ApiVersion2), Constants.GetMethod, parameters).ConfigureAwait(false);
        }

        /// <summary>
        /// Gets the order book for a symbol
        /// </summary>
        /// <param name="symbol">The symbol to get the order book for</param>
        /// <param name="precision">The precision of the data</param>
        /// <param name="limit">The amount of results in the book</param>
        /// <returns>The order book for the symbol</returns>
        public CallResult<BitfinexOrderBookEntry[]> GetOrderBook(string symbol, Precision precision, int? limit = null) => GetOrderBookAsync(symbol, precision, limit).Result;

        /// <summary>
        /// Gets the order book for a symbol
        /// </summary>
        /// <param name="symbol">The symbol to get the order book for</param>
        /// <param name="precision">The precision of the data</param>
        /// <param name="limit">The amount of results in the book</param>
        /// <returns>The order book for the symbol</returns>
        public async Task<CallResult<BitfinexOrderBookEntry[]>> GetOrderBookAsync(string symbol, Precision precision, int? limit = null)
        {
            if (limit != null && (limit != 25 && limit != 100))
            {
                return new CallResult<BitfinexOrderBookEntry[]>(null, new ArgumentError("Limit should be either 25 or 100"));
            }

            var parameters = new Dictionary<string, object>();
            parameters.AddOptionalParameter("len", limit?.ToString());
            var prec = JsonConvert.SerializeObject(precision, new PrecisionConverter(false));

            return await ExecuteRequest<BitfinexOrderBookEntry[]>(GetUrl(FillPathParameter(OrderBookEndpoint, symbol, prec), ApiVersion2), Constants.GetMethod, parameters).ConfigureAwait(false);
        }

        /// <summary>
        /// Get various stats for the symbol
        /// </summary>
        /// <param name="symbol">The symbol to request stats for</param>
        /// <param name="key">The type of stats</param>
        /// <param name="side">Side of the stats</param>
        /// <param name="section">Section of the stats</param>
        /// <param name="sorting">The way the result should be sorted</param>
        /// <returns></returns>
        public CallResult<BitfinexStats> GetStats(string symbol, StatKey key, StatSide side, StatSection section, Sorting? sorting = null) => GetStatsAsync(symbol, key, side, section, sorting).Result;

        /// <summary>
        /// Get various stats for the symbol
        /// </summary>
        /// <param name="symbol">The symbol to request stats for</param>
        /// <param name="key">The type of stats</param>
        /// <param name="side">Side of the stats</param>
        /// <param name="section">Section of the stats</param>
        /// <param name="sorting">The way the result should be sorted</param>
        /// <returns></returns>
        public async Task<CallResult<BitfinexStats>> GetStatsAsync(string symbol, StatKey key, StatSide side, StatSection section, Sorting? sorting)
        {
            var parameters = new Dictionary<string, object>();
            parameters.AddOptionalParameter("sort", sorting != null ? JsonConvert.SerializeObject(sorting, new SortingConverter(false)) : null);

            var endpoint = FillPathParameter(StatsEndpoint,
                JsonConvert.SerializeObject(key, new StatKeyConverter(false)),
                symbol,
                JsonConvert.SerializeObject(side, new StatSideConverter(false)),
                JsonConvert.SerializeObject(section, new StatSectionConverter(false)));

            return await ExecuteRequest<BitfinexStats>(GetUrl(endpoint, ApiVersion2), Constants.GetMethod, parameters).ConfigureAwait(false);
        }

        /// <summary>
        /// Get the last candle for a symbol
        /// </summary>
        /// <param name="timeFrame">The time frame of the candle</param>
        /// <param name="symbol">The symbol to get the candle for</param>
        /// <returns>The last candle for the symbol</returns>
        public CallResult<BitfinexCandle> GetLastCandle(TimeFrame timeFrame, string symbol)
            => GetLastCandleAsync(timeFrame, symbol).Result;

        /// <summary>
        /// Get the last candle for a symbol
        /// </summary>
        /// <param name="timeFrame">The time frame of the candle</param>
        /// <param name="symbol">The symbol to get the candle for</param>
        /// <returns>The last candle for the symbol</returns>
        public async Task<CallResult<BitfinexCandle>> GetLastCandleAsync(TimeFrame timeFrame, string symbol)
        {
            var endpoint = FillPathParameter(LastCandleEndpoint, JsonConvert.SerializeObject(timeFrame, new TimeFrameConverter(false)), symbol);

            return await ExecuteRequest<BitfinexCandle>(GetUrl(endpoint, ApiVersion2)).ConfigureAwait(false);
        }

        /// <summary>
        /// Gets candles for a symbol
        /// </summary>
        /// <param name="timeFrame">The time frame of the candles</param>
        /// <param name="symbol">The symbol to get the candles for</param>
        /// <param name="limit">The amount of results</param>
        /// <param name="startTime">The start time of the candles</param>
        /// <param name="endTime">The end time of the candles</param>
        /// <param name="sorting">The way the result is sorted</param>
        /// <returns></returns>
        public CallResult<BitfinexCandle[]> GetCandles(TimeFrame timeFrame, string symbol, int? limit = null, DateTime? startTime = null, DateTime? endTime = null, Sorting? sorting = null)
            => GetCandlesAsync(timeFrame, symbol, limit, startTime, endTime, sorting).Result;

        /// <summary>
        /// Gets candles for a symbol
        /// </summary>
        /// <param name="timeFrame">The time frame of the candles</param>
        /// <param name="symbol">The symbol to get the candles for</param>
        /// <param name="limit">The amount of results</param>
        /// <param name="startTime">The start time of the candles</param>
        /// <param name="endTime">The end time of the candles</param>
        /// <param name="sorting">The way the result is sorted</param>
        /// <returns></returns>
        public async Task<CallResult<BitfinexCandle[]>> GetCandlesAsync(TimeFrame timeFrame, string symbol, int? limit = null, DateTime? startTime = null, DateTime? endTime = null, Sorting? sorting = null)
        {
            var parameters = new Dictionary<string, object>();
            parameters.AddOptionalParameter("limit", limit?.ToString());
            parameters.AddOptionalParameter("start", startTime != null ? JsonConvert.SerializeObject(startTime, new TimestampConverter()) : null);
            parameters.AddOptionalParameter("end", endTime != null ? JsonConvert.SerializeObject(endTime, new TimestampConverter()) : null);
            parameters.AddOptionalParameter("sort", sorting != null ? JsonConvert.SerializeObject(sorting, new SortingConverter(false)) : null);

            var endpoint = FillPathParameter(CandlesEndpoint,
                JsonConvert.SerializeObject(timeFrame, new TimeFrameConverter(false)),
                symbol);

            return await ExecuteRequest<BitfinexCandle[]>(GetUrl(endpoint, ApiVersion2), Constants.GetMethod, parameters).ConfigureAwait(false);
        }

        /// <summary>
        /// Calculate the average execution price
        /// </summary>
        /// <param name="symbol">The symbol to calculate for</param>
        /// <param name="amount">The amount to execute</param>
        /// <param name="rateLimit">Limit to price</param>
        /// <param name="period">Maximum period for margin funding</param>
        /// <returns>The average price at which the execution would happen</returns>
        public CallResult<BitfinexMarketAveragePrice> GetMarketAveragePrice(string symbol, decimal amount, decimal rateLimit, int? period = null) => GetMarketAveragePriceAsync(symbol, amount, rateLimit, period).Result;

        /// <summary>
        /// Calculate the average execution price
        /// </summary>
        /// <param name="symbol">The symbol to calculate for</param>
        /// <param name="amount">The amount to execute</param>
        /// <param name="rateLimit">Limit to price</param>
        /// <param name="period">Maximum period for margin funding</param>
        /// <returns>The average price at which the execution would happen</returns>
        public async Task<CallResult<BitfinexMarketAveragePrice>> GetMarketAveragePriceAsync(string symbol, decimal amount, decimal? rateLimit = null, int? period = null)
        {
            var parameters = new Dictionary<string, object>
            {
                { "symbol", symbol },
                { "amount", amount.ToString(CultureInfo.InvariantCulture) },
            };
            parameters.AddOptionalParameter("period", period?.ToString());
            parameters.AddOptionalParameter("rate_limit", rateLimit?.ToString(CultureInfo.InvariantCulture));

            return await ExecuteRequest<BitfinexMarketAveragePrice>(GetUrl(MarketAverageEndpoint, ApiVersion2), Constants.PostMethod, parameters).ConfigureAwait(false);
        }

        /// <summary>
        /// Get all funds
        /// </summary>
        /// <returns></returns>
        public CallResult<BitfinexWallet[]> GetWallets() => GetWalletsAsync().Result;

        /// <summary>
        /// Get all funds
        /// </summary>
        /// <returns></returns>
        public async Task<CallResult<BitfinexWallet[]>> GetWalletsAsync()
        {
            return await ExecuteRequest<BitfinexWallet[]>(GetUrl(WalletsEndpoint, ApiVersion2), Constants.PostMethod, null, true).ConfigureAwait(false);
        }

        /// <summary>
        /// Get the active orders
        /// </summary>
        /// <returns></returns>
        public CallResult<BitfinexOrder[]> GetActiveOrders() => GetActiveOrdersAsync().Result;

        /// <summary>
        /// Get the active orders
        /// </summary>
        /// <returns></returns>
        public async Task<CallResult<BitfinexOrder[]>> GetActiveOrdersAsync()
        {
            return await ExecuteRequest<BitfinexOrder[]>(GetUrl(OpenOrdersEndpoint, ApiVersion2), Constants.PostMethod, null, true).ConfigureAwait(false);
        }


        /// <summary>
        /// Get the active ordersView your latest inactive orders.
        /// Limited to last 3 days and 1 request per minute.
        /// </summary>
        /// <returns></returns>
        public CallResult<BitfinexPlacedOrder[]> GetOrdersHistory(int? limit) => GetOrdersHistoryAsync(limit).Result;

        /// <summary>
        /// Get the active ordersView your latest inactive orders.
        /// Limited to last 3 days and 1 request per minute.
        /// </summary>
        /// <returns></returns>
        public async Task<CallResult<BitfinexPlacedOrder[]>> GetOrdersHistoryAsync(int? limit)
        {
            var parameters = new Dictionary<string, object>();
            parameters.AddOptionalParameter("limit", limit?.ToString());

            return await ExecuteRequest<BitfinexPlacedOrder[]>(GetUrl(OrdersHistoryEndpoint, ApiVersion1), Constants.PostMethod, parameters, true)
                .ConfigureAwait(false);
        }

        /// <summary>
        /// Get the order history for a symbol for this account
        /// </summary>
        /// <param name="symbol">The symbol to get the history for</param>
        /// <param name="startTime">Start time of the data to return</param>
        /// <param name="endTime">End time of the data to return</param>
        /// <param name="limit">Max amount of results</param>
        /// <returns></returns>
        public CallResult<BitfinexOrder[]> GetOrderHistory(string symbol, DateTime? startTime = null, DateTime? endTime = null, int? limit = null) => GetOrderHistoryAsync(symbol, startTime, endTime, limit).Result;

        /// <summary>
        /// Get the order history for a symbol for this account
        /// </summary>
        /// <param name="symbol">The symbol to get the history for</param>
        /// <param name="startTime">Start time of the data to return</param>
        /// <param name="endTime">End time of the data to return</param>
        /// <param name="limit">Max amount of results</param>
        /// <returns></returns>
        public async Task<CallResult<BitfinexOrder[]>> GetOrderHistoryAsync(string symbol, DateTime? startTime = null, DateTime? endTime = null, int? limit = null)
        {
            var parameters = new Dictionary<string, object>();
            parameters.AddOptionalParameter("limit", limit?.ToString());
            parameters.AddOptionalParameter("start", startTime != null ? JsonConvert.SerializeObject(startTime, new TimestampConverter()) : null);
            parameters.AddOptionalParameter("end", endTime != null ? JsonConvert.SerializeObject(endTime, new TimestampConverter()) : null);

            return await ExecuteRequest<BitfinexOrder[]>(GetUrl(FillPathParameter(OrderHistoryEndpoint, symbol), ApiVersion2), Constants.PostMethod, parameters, true).ConfigureAwait(false);
        }

        /// <summary>
        /// Get the individual trades for an order
        /// </summary>
        /// <param name="symbol">The symbol of the order</param>
        /// <param name="orderId">The order Id</param>
        /// <returns></returns>
        public CallResult<BitfinexTradeDetails[]> GetTradesForOrder(string symbol, long orderId) => GetTradesForOrderAsync(symbol, orderId).Result;

        /// <summary>
        /// Get the individual trades for an order
        /// </summary>
        /// <param name="symbol">The symbol of the order</param>
        /// <param name="orderId">The order Id</param>
        /// <returns></returns>
        public async Task<CallResult<BitfinexTradeDetails[]>> GetTradesForOrderAsync(string symbol, long orderId)
        {
            return await ExecuteRequest<BitfinexTradeDetails[]>(GetUrl(FillPathParameter(OrderTradesEndpoint, symbol, orderId.ToString()), ApiVersion2), Constants.PostMethod, null, true).ConfigureAwait(false);
        }

        /// <summary>
        /// Get the trade history for a symbol
        /// </summary>
        /// <param name="symbol">The symbol to get history for</param>
        /// <param name="startTime">Start time of the data to return</param>
        /// <param name="endTime">End time of the data to return</param>
        /// <param name="limit">Max amount of results</param>
        /// <returns></returns>
        public CallResult<BitfinexTradeDetails[]> GetTradeHistory(string symbol, DateTime? startTime = null, DateTime? endTime = null, int? limit = null) => GetTradeHistoryAsync(symbol, startTime, endTime, limit).Result;

        /// <summary>
        /// Get the trade history for a symbol
        /// </summary>
        /// <param name="symbol">The symbol to get history for</param>
        /// <param name="startTime">Start time of the data to return</param>
        /// <param name="endTime">End time of the data to return</param>
        /// <param name="limit">Max amount of results</param>
        /// <returns></returns>
        public async Task<CallResult<BitfinexTradeDetails[]>> GetTradeHistoryAsync(string symbol, DateTime? startTime = null, DateTime? endTime = null, int? limit = null)
        {
            var parameters = new Dictionary<string, object>();
            parameters.AddOptionalParameter("limit", limit?.ToString());
            parameters.AddOptionalParameter("start", startTime != null ? JsonConvert.SerializeObject(startTime, new TimestampConverter()) : null);
            parameters.AddOptionalParameter("end", endTime != null ? JsonConvert.SerializeObject(endTime, new TimestampConverter()) : null);

            return await ExecuteRequest<BitfinexTradeDetails[]>(GetUrl(FillPathParameter(MyTradesEndpoint, symbol), ApiVersion2), Constants.PostMethod, parameters, true).ConfigureAwait(false);
        }

        /// <summary>
        /// Get the active positions
        /// </summary>
        /// <returns></returns>
        public CallResult<BitfinexPosition[]> GetActivePositions() => GetActivePositionsAsync().Result;

        /// <summary>
        /// Get the active positions
        /// </summary>
        /// <returns></returns>
        public async Task<CallResult<BitfinexPosition[]>> GetActivePositionsAsync()
        {
            return await ExecuteRequest<BitfinexPosition[]>(GetUrl(ActivePositionsEndpoint, ApiVersion2), Constants.PostMethod, null, true).ConfigureAwait(false);
        }

        /// <summary>
        /// Get the active funding offers
        /// </summary>
        /// <param name="symbol">The symbol to return the funding offer for</param>
        /// <returns></returns>
        public CallResult<BitfinexFundingOffer[]> GetActiveFundingOffers(string symbol) => GetActiveFundingOffersAsync(symbol).Result;

        /// <summary>
        /// Get the active funding offers
        /// </summary>
        /// <param name="symbol">The symbol to return the funding offer for</param>
        /// <returns></returns>
        public async Task<CallResult<BitfinexFundingOffer[]>> GetActiveFundingOffersAsync(string symbol)
        {
            return await ExecuteRequest<BitfinexFundingOffer[]>(GetUrl(FillPathParameter(ActiveFundingOffersEndpoint, symbol), ApiVersion2), Constants.PostMethod, null, true).ConfigureAwait(false);
        }

        /// <summary>
        /// Get the funding offer history
        /// </summary>
        /// <param name="symbol">The symbol to get history for</param>
        /// <param name="startTime">Start time of the data to return</param>
        /// <param name="endTime">End time of the data to return</param>
        /// <param name="limit">Max amount of results</param>
        /// <returns></returns>
        public CallResult<BitfinexFundingOffer[]> GetFundingOfferHistory(string symbol, DateTime? startTime = null, DateTime? endTime = null, int? limit = null) => GetFundingOfferHistoryAsync(symbol, startTime, endTime, limit).Result;

        /// <summary>
        /// Get the funding offer history
        /// </summary>
        /// <param name="symbol">The symbol to get history for</param>
        /// <param name="startTime">Start time of the data to return</param>
        /// <param name="endTime">End time of the data to return</param>
        /// <param name="limit">Max amount of results</param>
        /// <returns></returns>
        public async Task<CallResult<BitfinexFundingOffer[]>> GetFundingOfferHistoryAsync(string symbol, DateTime? startTime = null, DateTime? endTime = null, int? limit = null)
        {
            var parameters = new Dictionary<string, object>();
            parameters.AddOptionalParameter("limit", limit?.ToString());
            parameters.AddOptionalParameter("start", startTime != null ? JsonConvert.SerializeObject(startTime, new TimestampConverter()) : null);
            parameters.AddOptionalParameter("end", endTime != null ? JsonConvert.SerializeObject(endTime, new TimestampConverter()) : null);

            return await ExecuteRequest<BitfinexFundingOffer[]>(GetUrl(FillPathParameter(FundingOfferHistoryEndpoint, symbol), ApiVersion2), Constants.PostMethod, parameters, true).ConfigureAwait(false);
        }

        /// <summary>
        /// Get the funding loans
        /// </summary>
        /// <param name="symbol">The symbol to get the funding loans for</param>
        /// <returns></returns>
        public CallResult<BitfinexFundingLoan[]> GetFundingLoans(string symbol) => GetFundingLoansAsync(symbol).Result;

        /// <summary>
        /// Get the funding loans
        /// </summary>
        /// <param name="symbol">The symbol to get the funding loans for</param>
        /// <returns></returns>
        public async Task<CallResult<BitfinexFundingLoan[]>> GetFundingLoansAsync(string symbol)
        {
            return await ExecuteRequest<BitfinexFundingLoan[]>(GetUrl(FillPathParameter(FundingLoansEndpoint, symbol), ApiVersion2), Constants.PostMethod, null, true).ConfigureAwait(false);
        }

        /// <summary>
        /// Get the funding loan history
        /// </summary>
        /// <param name="symbol">The symbol to get history for</param>
        /// <param name="startTime">Start time of the data to return</param>
        /// <param name="endTime">End time of the data to return</param>
        /// <param name="limit">Max amount of results</param>
        /// <returns></returns>
        public CallResult<BitfinexFundingLoan[]> GetFundingLoansHistory(string symbol, DateTime? startTime = null, DateTime? endTime = null, int? limit = null) => GetFundingLoansHistoryAsync(symbol, startTime, endTime, limit).Result;

        /// <summary>
        /// Get the funding loan history
        /// </summary>
        /// <param name="symbol">The symbol to get history for</param>
        /// <param name="startTime">Start time of the data to return</param>
        /// <param name="endTime">End time of the data to return</param>
        /// <param name="limit">Max amount of results</param>
        /// <returns></returns>
        public async Task<CallResult<BitfinexFundingLoan[]>> GetFundingLoansHistoryAsync(string symbol, DateTime? startTime = null, DateTime? endTime = null, int? limit = null)
        {
            var parameters = new Dictionary<string, object>();
            parameters.AddOptionalParameter("limit", limit?.ToString());
            parameters.AddOptionalParameter("start", startTime != null ? JsonConvert.SerializeObject(startTime, new TimestampConverter()) : null);
            parameters.AddOptionalParameter("end", endTime != null ? JsonConvert.SerializeObject(endTime, new TimestampConverter()) : null);

            return await ExecuteRequest<BitfinexFundingLoan[]>(GetUrl(FillPathParameter(FundingLoansHistoryEndpoint, symbol), ApiVersion2), Constants.PostMethod, parameters, true).ConfigureAwait(false);
        }

        /// <summary>
        /// Get the funding credits
        /// </summary>
        /// <param name="symbol">The symbol to get the funding credits for</param>
        /// <returns></returns>
        public CallResult<BitfinexFundingCredit[]> GetFundingCredits(string symbol) => GetFundingCreditsAsync(symbol).Result;

        /// <summary>
        /// Get the funding credits
        /// </summary>
        /// <param name="symbol">The symbol to get the funding credits for</param>
        /// <returns></returns>
        public async Task<CallResult<BitfinexFundingCredit[]>> GetFundingCreditsAsync(string symbol)
        {
            return await ExecuteRequest<BitfinexFundingCredit[]>(GetUrl(FillPathParameter(FundingCreditsEndpoint, symbol), ApiVersion2), Constants.PostMethod, null, true).ConfigureAwait(false);
        }

        /// <summary>
        /// Get the funding credits history
        /// </summary>
        /// <param name="symbol">The symbol to get history for</param>
        /// <param name="startTime">Start time of the data to return</param>
        /// <param name="endTime">End time of the data to return</param>
        /// <param name="limit">Max amount of results</param>
        /// <returns></returns>
        public CallResult<BitfinexFundingCredit[]> GetFundingCreditsHistory(string symbol, DateTime? startTime = null, DateTime? endTime = null, int? limit = null) => GetFundingCreditsHistoryAsyncTask(symbol, startTime, endTime, limit).Result;

        /// <summary>
        /// Get the funding credits history
        /// </summary>
        /// <param name="symbol">The symbol to get history for</param>
        /// <param name="startTime">Start time of the data to return</param>
        /// <param name="endTime">End time of the data to return</param>
        /// <param name="limit">Max amount of results</param>
        /// <returns></returns>
        public async Task<CallResult<BitfinexFundingCredit[]>> GetFundingCreditsHistoryAsyncTask(string symbol, DateTime? startTime = null, DateTime? endTime = null, int? limit = null)
        {
            var parameters = new Dictionary<string, object>();
            parameters.AddOptionalParameter("limit", limit?.ToString());
            parameters.AddOptionalParameter("start", startTime != null ? JsonConvert.SerializeObject(startTime, new TimestampConverter()) : null);
            parameters.AddOptionalParameter("end", endTime != null ? JsonConvert.SerializeObject(endTime, new TimestampConverter()) : null);

            return await ExecuteRequest<BitfinexFundingCredit[]>(GetUrl(FillPathParameter(FundingCreditsHistoryEndpoint, symbol), ApiVersion2), Constants.PostMethod, parameters, true).ConfigureAwait(false);
        }

        /// <summary>
        /// Get the funding trades history
        /// </summary>
        /// <param name="symbol">The symbol to get history for</param>
        /// <param name="startTime">Start time of the data to return</param>
        /// <param name="endTime">End time of the data to return</param>
        /// <param name="limit">Max amount of results</param>
        /// <returns></returns>
        public CallResult<BitfinexFundingTrade[]> GetFundingTradesHistory(string symbol, DateTime? startTime = null, DateTime? endTime = null, int? limit = null) => GetFundingTradesHistoryAsync(symbol, startTime, endTime, limit).Result;

        /// <summary>
        /// Get the funding trades history
        /// </summary>
        /// <param name="symbol">The symbol to get history for</param>
        /// <param name="startTime">Start time of the data to return</param>
        /// <param name="endTime">End time of the data to return</param>
        /// <param name="limit">Max amount of results</param>
        /// <returns></returns>
        public async Task<CallResult<BitfinexFundingTrade[]>> GetFundingTradesHistoryAsync(string symbol, DateTime? startTime = null, DateTime? endTime = null, int? limit = null)
        {
            var parameters = new Dictionary<string, object>();
            parameters.AddOptionalParameter("limit", limit?.ToString());
            parameters.AddOptionalParameter("start", startTime != null ? JsonConvert.SerializeObject(startTime, new TimestampConverter()) : null);
            parameters.AddOptionalParameter("end", endTime != null ? JsonConvert.SerializeObject(endTime, new TimestampConverter()) : null);

            return await ExecuteRequest<BitfinexFundingTrade[]>(GetUrl(FillPathParameter(FundingTradesEndpoint, symbol), ApiVersion2), Constants.PostMethod).ConfigureAwait(false);
        }

        /// <summary>
        /// Get the base margin info
        /// </summary>
        /// <returns></returns>
        public CallResult<BitfinexMarginBase> GetBaseMarginInfo() => GetBaseMarginInfoAsync().Result;

        /// <summary>
        /// Get the base margin info
        /// </summary>
        /// <returns></returns>
        public async Task<CallResult<BitfinexMarginBase>> GetBaseMarginInfoAsync()
        {
            return await ExecuteRequest<BitfinexMarginBase>(GetUrl(MarginInfoBaseEndpoint, ApiVersion2), Constants.PostMethod, null, true).ConfigureAwait(false);
        }

        /// <summary>
        /// Get the margin info for a symbol
        /// </summary>
        /// <param name="symbol">The symbol to get the info for</param>
        /// <returns></returns>
        public CallResult<BitfinexMarginSymbol> GetSymbolMarginInfo(string symbol) => GetSymbolMarginInfoAsync(symbol).Result;

        /// <summary>
        /// Get the margin info for a symbol
        /// </summary>
        /// <param name="symbol">The symbol to get the info for</param>
        /// <returns></returns>
        public async Task<CallResult<BitfinexMarginSymbol>> GetSymbolMarginInfoAsync(string symbol)
        {
            return await ExecuteRequest<BitfinexMarginSymbol>(GetUrl(FillPathParameter(MarginInfoSymbolEndpoint, symbol), ApiVersion2), Constants.PostMethod, null, true).ConfigureAwait(false);
        }

        /// <summary>
        /// Get funding info for a symbol
        /// </summary>
        /// <param name="symbol">The symbol to get the info for</param>
        /// <returns></returns>
        public CallResult<BitfinexFundingInfo> GetFundingInfo(string symbol) => GetFundingInfoAsync(symbol).Result;

        /// <summary>
        /// Get funding info for a symbol
        /// </summary>
        /// <param name="symbol">The symbol to get the info for</param>
        /// <returns></returns>
        public async Task<CallResult<BitfinexFundingInfo>> GetFundingInfoAsync(string symbol)
        {
            return await ExecuteRequest<BitfinexFundingInfo>(GetUrl(FillPathParameter(FundingInfoEndpoint, symbol), ApiVersion2), Constants.PostMethod, null, true).ConfigureAwait(false);
        }

        /// <summary>
        /// Get the withdrawal/deposit history
        /// </summary>
        /// <param name="symbol">Symbol to get history for</param>
        /// <returns></returns>
        public CallResult<BitfinexMovement[]> GetMovements(string symbol) => GetMovementsAsync(symbol).Result;

        /// <summary>
        /// Get the withdrawal/deposit history
        /// </summary>
        /// <param name="symbol">Symbol to get history for</param>
        /// <returns></returns>
        public async Task<CallResult<BitfinexMovement[]>> GetMovementsAsync(string symbol)
        {
            return await ExecuteRequest<BitfinexMovement[]>(GetUrl(FillPathParameter(MovementsEndpoint, symbol), ApiVersion2), Constants.PostMethod, null, true).ConfigureAwait(false);
        }

        public CallResult<BitfinexPerformance> GetDailyPerformance() => GetDailyPerformanceAsync().Result;
        public async Task<CallResult<BitfinexPerformance>> GetDailyPerformanceAsync()
        {
            // TODO doesn't work?
            return await ExecuteRequest<BitfinexPerformance>(GetUrl(DailyPerformanceEndpoint, ApiVersion2), Constants.PostMethod, null, true).ConfigureAwait(false);
        }

        /// <summary>
        /// Get the list of alerts
        /// </summary>
        /// <returns></returns>
        public CallResult<BitfinexAlert[]> GetAlertList() => GetAlertListAsync().Result;

        /// <summary>
        /// Get the list of alerts
        /// </summary>
        /// <returns></returns>
        public async Task<CallResult<BitfinexAlert[]>> GetAlertListAsync()
        {
            var parameters = new Dictionary<string, object>()
            {
                { "type", "price" }
            };

            return await ExecuteRequest<BitfinexAlert[]>(GetUrl(AlertListEndpoint, ApiVersion2), Constants.PostMethod, parameters, true).ConfigureAwait(false);
        }

        /// <summary>
        /// Set an alert
        /// </summary>
        /// <param name="symbol">The symbol to set the alert for</param>
        /// <param name="price">The price to set the alert for</param>
        /// <returns></returns>
        public CallResult<BitfinexAlert> SetAlert(string symbol, decimal price) => SetAlertAsync(symbol, price).Result;

        /// <summary>
        /// Set an alert
        /// </summary>
        /// <param name="symbol">The symbol to set the alert for</param>
        /// <param name="price">The price to set the alert for</param>
        /// <returns></returns>
        public async Task<CallResult<BitfinexAlert>> SetAlertAsync(string symbol, decimal price)
        {
            var parameters = new Dictionary<string, object>()
            {
                { "type", "price" },
                { "symbol", symbol },
                { "price", price.ToString(CultureInfo.InvariantCulture) }
            };

            return await ExecuteRequest<BitfinexAlert>(GetUrl(SetAlertEndpoint, ApiVersion2), Constants.PostMethod, parameters, true).ConfigureAwait(false);
        }

        /// <summary>
        /// Delete an existing alert
        /// </summary>
        /// <param name="symbol">The symbol of the alert to delete</param>
        /// <param name="price">The price of the alert to delete</param>
        /// <returns></returns>
        public CallResult<BitfinexSuccessResult> DeleteAlert(string symbol, decimal price) => DeleteAlertAsync(symbol, price).Result;

        /// <summary>
        /// Delete an existing alert
        /// </summary>
        /// <param name="symbol">The symbol of the alert to delete</param>
        /// <param name="price">The price of the alert to delete</param>
        /// <returns></returns>
        public async Task<CallResult<BitfinexSuccessResult>> DeleteAlertAsync(string symbol, decimal price)
        {
            return await ExecuteRequest<BitfinexSuccessResult>(GetUrl(FillPathParameter(DeleteAlertEndpoint, symbol, price.ToString(CultureInfo.InvariantCulture)), ApiVersion2), Constants.PostMethod, null, true).ConfigureAwait(false);
        }
        #endregion

        #region Version1
        /// <summary>
        /// Get information about your account
        /// </summary>
        /// <returns></returns>
        public CallResult<BitfinexAccountInfo> GetAccountInfo() => GetAccountInfoAsync().Result;

        /// <summary>
        /// Get information about your account
        /// </summary>
        /// <returns></returns>
        public async Task<CallResult<BitfinexAccountInfo>> GetAccountInfoAsync()
        {
            var result = await ExecuteRequest<BitfinexAccountInfo[]>(GetUrl(AccountInfoEndpoint, ApiVersion1), Constants.PostMethod, null, true).ConfigureAwait(false);
            return result.Success ? new CallResult<BitfinexAccountInfo>(result.Data[0], null) : new CallResult<BitfinexAccountInfo>(null, result.Error);
        }

        /// <summary>
        /// Get withdrawal fees for this account
        /// </summary>
        /// <returns></returns>
        public CallResult<BitfinexWithdrawalFees> GetWithdrawalFees() => GetWithdrawalFeesAsync().Result;

        /// <summary>
        /// Get withdrawal fees for this account
        /// </summary>
        /// <returns></returns>
        public async Task<CallResult<BitfinexWithdrawalFees>> GetWithdrawalFeesAsync()
        {
            return await ExecuteRequest<BitfinexWithdrawalFees>(GetUrl(WithdrawalFeeEndpoint, ApiVersion1), Constants.PostMethod, null, true).ConfigureAwait(false);
        }

        /// <summary>
        /// Place a new order
        /// </summary>
        /// <param name="symbol">Symbol to place order for</param>
        /// <param name="side">Side of the order</param>
        /// <param name="type">Type of the order</param>
        /// <param name="amount">The amount of the order</param>
        /// <param name="price">The price for the order</param>
        /// <param name="hidden">If the order should be placed as hidden</param>
        /// <param name="postOnly">If the only should only be placed if it isn't immediatly filled</param>
        /// <param name="useAllAvailable">If all available funds should be used</param>
        /// <param name="ocoOrder">If the order is a one-cancels-other order</param>
        /// <param name="ocoBuyPrice">The one-cancels-other buy price</param>
        /// <param name="ocoSellPrice">The one-cancels-other sell price</param>
        /// <returns></returns>
        public CallResult<BitfinexPlacedOrder> PlaceOrder(
            string symbol,
            OrderSide side,
            OrderTypeV1 type,
            decimal amount,
            decimal price,
            bool? hidden = null,
            bool? postOnly = null,
            bool? useAllAvailable = null,
            bool? ocoOrder = null,
            decimal? ocoBuyPrice = null,
            decimal? ocoSellPrice = null) => PlaceOrderAsync(symbol, side, type, amount, price, hidden, postOnly, useAllAvailable, ocoOrder, ocoBuyPrice, ocoSellPrice).Result;

        /// <summary>
        /// Place a new order
        /// </summary>
        /// <param name="symbol">Symbol to place order for</param>
        /// <param name="side">Side of the order</param>
        /// <param name="type">Type of the order</param>
        /// <param name="amount">The amount of the order</param>
        /// <param name="price">The price for the order</param>
        /// <param name="hidden">If the order should be placed as hidden</param>
        /// <param name="postOnly">If the only should only be placed if it isn't immediatly filled</param>
        /// <param name="useAllAvailable">If all available funds should be used</param>
        /// <param name="ocoOrder">If the order is a one-cancels-other order</param>
        /// <param name="ocoBuyPrice">The one-cancels-other buy price</param>
        /// <param name="ocoSellPrice">The one-cancels-other sell price</param>
        /// <returns></returns>
        public async Task<CallResult<BitfinexPlacedOrder>> PlaceOrderAsync(
            string symbol,
            OrderSide side,
            OrderTypeV1 type,
            decimal amount,
            decimal price,
            bool? hidden = null,
            bool? postOnly = null,
            bool? useAllAvailable = null,
            bool? ocoOrder = null,
            decimal? ocoBuyPrice = null,
            decimal? ocoSellPrice = null)
        {
            var parameters = new Dictionary<string, object>()
            {
                { "symbol", symbol },
                { "amount", amount.ToString(CultureInfo.InvariantCulture) },
                { "price", price.ToString(CultureInfo.InvariantCulture) },
                { "exchange", "bitfinex" },
                { "side", JsonConvert.SerializeObject(side, new OrderSideConverter(false)) },
                { "type", JsonConvert.SerializeObject(type, new OrderTypeV1Converter(false)) },
            };
            parameters.AddOptionalParameter("is_hidden", hidden);
            parameters.AddOptionalParameter("is_postonly", postOnly);
            parameters.AddOptionalParameter("use_all_available", useAllAvailable == true ? "1" : null);
            parameters.AddOptionalParameter("ocoorder", ocoOrder);
            parameters.AddOptionalParameter("buy_price_oco", ocoBuyPrice);
            parameters.AddOptionalParameter("sell_price_oco", ocoSellPrice);

            return await ExecuteRequest<BitfinexPlacedOrder>(GetUrl(PlaceOrderEndpoint, ApiVersion1), Constants.PostMethod, parameters, true).ConfigureAwait(false);
        }

        /// <summary>
        /// Cancel a specific order
        /// </summary>
        /// <param name="orderId">The id of the order to cancel</param>
        /// <returns></returns>
        public CallResult<BitfinexPlacedOrder> CancelOrder(long orderId) => CancelOrderAsync(orderId).Result;

        /// <summary>
        /// Cancel a specific order
        /// </summary>
        /// <param name="orderId">The id of the order to cancel</param>
        /// <returns></returns>
        public async Task<CallResult<BitfinexPlacedOrder>> CancelOrderAsync(long orderId)
        {
            var parameters = new Dictionary<string, object>()
            {
                { "order_id", orderId }
            };

            return await ExecuteRequest<BitfinexPlacedOrder>(GetUrl(CancelOrderEndpoint, ApiVersion1), Constants.PostMethod, parameters, true).ConfigureAwait(false);
        }

        /// <summary>
        /// Cancels all open orders
        /// </summary>
        /// <returns></returns>
        public CallResult<BitfinexResult> CancelAllOrders() => CancelAllOrdersAsync().Result;

        /// <summary>
        /// Cancels all open orders
        /// </summary>
        /// <returns></returns>
        public async Task<CallResult<BitfinexResult>> CancelAllOrdersAsync()
        {
            return await ExecuteRequest<BitfinexResult>(GetUrl(CancelAllOrderEndpoint, ApiVersion1), Constants.PostMethod, null, true).ConfigureAwait(false);
        }

        /// <summary>
        /// Get the status of a specific order
        /// </summary>
        /// <param name="orderId">The order id of the order to get</param>
        /// <returns></returns>
        public CallResult<BitfinexPlacedOrder> GetOrder(long orderId) => GetOrderAsync(orderId).Result;

        /// <summary>
        /// Get the status of a specific order
        /// </summary>
        /// <param name="orderId">The order id of the order to get</param>
        /// <returns></returns>
        public async Task<CallResult<BitfinexPlacedOrder>> GetOrderAsync(long orderId)
        {
            var parameters = new Dictionary<string, object>()
            {
                { "order_id", orderId }
            };

            return await ExecuteRequest<BitfinexPlacedOrder>(GetUrl(OrderStatusEndpoint, ApiVersion1), Constants.PostMethod, parameters, true).ConfigureAwait(false);
        }

        /// <summary>
        /// Calculates the available balance for a symbol at a specific rate
        /// </summary>
        /// <param name="symbol">The symbol</param>
        /// <param name="side">Buy or sell</param>
        /// <param name="rate">The rate/price</param>
        /// <param name="type">The wallet type</param>
        /// <returns></returns>
        public CallResult<BitfinexAvailableBalance> GetAvailableBalance(string symbol, OrderSide side, decimal rate, WalletType type) => GetAvailableBalanceAsync(symbol, side, rate, type).Result;

        /// <summary>
        /// Calculates the available balance for a symbol at a specific rate
        /// </summary>
        /// <param name="symbol">The symbol</param>
        /// <param name="side">Buy or sell</param>
        /// <param name="rate">The rate/price</param>
        /// <param name="type">The wallet type</param>
        /// <returns></returns>
        public async Task<CallResult<BitfinexAvailableBalance>> GetAvailableBalanceAsync(string symbol, OrderSide side, decimal rate, WalletType type)
        {
            var parameters = new Dictionary<string, object>()
            {
                { "symbol", symbol },
                { "dir", side == OrderSide.Buy ? 1: -1 },
                { "rate", rate },
                { "type", JsonConvert.SerializeObject(type, new WalletTypeConverter(false)) }
            };

            return await ExecuteRequest<BitfinexAvailableBalance>(GetUrl(CalcAvailableBalanceEndpoint, ApiVersion2), Constants.PostMethod, parameters, true).ConfigureAwait(false);
        }

        /// <summary>
        /// Withdraw funds from Bitfinex, either to a crypto currency address or a bank account
        /// All withdrawals need the withdrawType, wallet and amount parameters
        /// CryptoCurrency withdrawals need the address parameters, the paymentId can be used for Monero as payment id and for Ripple as tag
        /// Wire withdrawals need the bank parameters. In some cases your bank will require the use of an intermediary bank, if this is the case, please supply those fields as well.
        /// </summary>
        /// <param name="withdrawType">The type of funds to withdraw</param>
        /// <param name="wallet">The wallet to withdraw from</param>
        /// <param name="amount">The amount to withdraw</param>
        /// <param name="address">The destination of the withdrawal</param>
        /// <param name="accountNumber">The account number</param>
        /// <param name="bankSwift">The SWIFT code of the bank</param>
        /// <param name="bankName">The bank name</param>
        /// <param name="bankAddress">The bank address</param>
        /// <param name="bankCity">The bank city</param>
        /// <param name="bankCountry">The bank country</param>
        /// <param name="paymentDetails">Message for the receiver</param>
        /// <param name="expressWire">Whether it is an express wire withdrawal</param>
        /// <param name="intermediaryBankName">Intermediary bank name</param>
        /// <param name="intermediaryBankAddress">Intermediary bank address</param>
        /// <param name="intermediaryBankCity">Intermediary bank city</param>
        /// <param name="intermediaryBankCountry">Intermediary bank country</param>
        /// <param name="intermediaryBankAccount">Intermediary bank account</param>
        /// <param name="intermediaryBankSwift">Intermediary bank SWIFT code</param>
        /// <param name="accountName">The name of the account</param>
        /// <param name="paymentId">Hex string for Monero transaction</param>
        /// <returns></returns>
        public CallResult<BitfinexWithdrawalResult> Withdraw(WithdrawalType withdrawType,
                                                             WithdrawWallet wallet,
                                                             decimal amount,
                                                             string address = null,
                                                             string accountNumber = null,
                                                             string bankSwift = null,
                                                             string bankName = null,
                                                             string bankAddress = null,
                                                             string bankCity = null,
                                                             string bankCountry = null,
                                                             string paymentDetails = null,
                                                             bool? expressWire = null,
                                                             string intermediaryBankName = null,
                                                             string intermediaryBankAddress = null,
                                                             string intermediaryBankCity = null,
                                                             string intermediaryBankCountry = null,
                                                             string intermediaryBankAccount = null,
                                                             string intermediaryBankSwift = null,
                                                             string accountName = null,
                                                             string paymentId = null) =>
                                                             WithdrawAsync(withdrawType, wallet, amount, address, accountNumber, bankSwift, bankName, bankAddress,
                                                                 bankCity, bankCountry, paymentDetails, expressWire, intermediaryBankName, intermediaryBankAddress,
                                                                 intermediaryBankCity, intermediaryBankCountry, intermediaryBankAccount, intermediaryBankSwift,
                                                                 accountName, paymentId).Result;


        /// <summary>
        /// Withdraw funds from Bitfinex, either to a crypto currency address or a bank account
        /// All withdrawals need the withdrawType, wallet and amount parameters
        /// CryptoCurrency withdrawals need the address parameters, the paymentId can be used for Monero as payment id and for Ripple as tag
        /// Wire withdrawals need the bank parameters. In some cases your bank will require the use of an intermediary bank, if this is the case, please supply those fields as well.
        /// </summary>
        /// <param name="withdrawType">The type of funds to withdraw</param>
        /// <param name="wallet">The wallet to withdraw from</param>
        /// <param name="amount">The amount to withdraw</param>
        /// <param name="address">The destination of the withdrawal</param>
        /// <param name="accountNumber">The account number</param>
        /// <param name="bankSwift">The SWIFT code of the bank</param>
        /// <param name="bankName">The bank name</param>
        /// <param name="bankAddress">The bank address</param>
        /// <param name="bankCity">The bank city</param>
        /// <param name="bankCountry">The bank country</param>
        /// <param name="paymentDetails">Message for the receiver</param>
        /// <param name="expressWire">Whether it is an express wire withdrawal</param>
        /// <param name="intermediaryBankName">Intermediary bank name</param>
        /// <param name="intermediaryBankAddress">Intermediary bank address</param>
        /// <param name="intermediaryBankCity">Intermediary bank city</param>
        /// <param name="intermediaryBankCountry">Intermediary bank country</param>
        /// <param name="intermediaryBankAccount">Intermediary bank account</param>
        /// <param name="intermediaryBankSwift">Intermediary bank SWIFT code</param>
        /// <param name="accountName">The name of the account</param>
        /// <param name="paymentId">Hex string for Monero transaction</param>
        /// <returns></returns>
        public async Task<CallResult<BitfinexWithdrawalResult>> WithdrawAsync(WithdrawalType withdrawType,
                                                                         WithdrawWallet wallet,
                                                                         decimal amount,
                                                                         string address = null,
                                                                         string accountNumber = null,
                                                                         string bankSwift = null,
                                                                         string bankName = null,
                                                                         string bankAddress = null,
                                                                         string bankCity = null,
                                                                         string bankCountry = null,
                                                                         string paymentDetails = null,
                                                                         bool? expressWire = null,
                                                                         string intermediaryBankName = null,
                                                                         string intermediaryBankAddress = null,
                                                                         string intermediaryBankCity = null,
                                                                         string intermediaryBankCountry = null,
                                                                         string intermediaryBankAccount = null,
                                                                         string intermediaryBankSwift = null,
                                                                         string accountName = null,
                                                                         string paymentId = null)
        {
            var parameters = new Dictionary<string, object>()
            {
                { "withdraw_type", JsonConvert.SerializeObject(withdrawType, new WithdrawalTypeConverter(false)) },
                { "walletselected", JsonConvert.SerializeObject(wallet, new WithdrawWalletConverter(false)) },
                { "amount", amount.ToString(CultureInfo.InvariantCulture) }
            };
            parameters.AddOptionalParameter("address", address);
            parameters.AddOptionalParameter("payment_id", paymentId);
            parameters.AddOptionalParameter("account_name", accountName);
            parameters.AddOptionalParameter("account_number", accountNumber);
            parameters.AddOptionalParameter("swift", bankSwift);
            parameters.AddOptionalParameter("bank_name", bankName);
            parameters.AddOptionalParameter("bank_address", bankAddress);
            parameters.AddOptionalParameter("bank_city", bankCity);
            parameters.AddOptionalParameter("bank_country", bankCountry);
            parameters.AddOptionalParameter("detail_payment", paymentDetails);
            parameters.AddOptionalParameter("expressWire", expressWire == null ? null : JsonConvert.SerializeObject(expressWire, new BoolToIntConverter(false)));
            parameters.AddOptionalParameter("intermediary_bank_name", intermediaryBankName);
            parameters.AddOptionalParameter("intermediary_bank_address", intermediaryBankAddress);
            parameters.AddOptionalParameter("intermediary_bank_city", intermediaryBankCity);
            parameters.AddOptionalParameter("intermediary_bank_country", intermediaryBankCountry);
            parameters.AddOptionalParameter("intermediary_bank_account", intermediaryBankAccount);
            parameters.AddOptionalParameter("intermediary_bank_swift", intermediaryBankSwift);

            var result = await ExecuteRequest<BitfinexWithdrawalResult[]>(GetUrl(WithdrawEndpoint, ApiVersion1), Constants.PostMethod, parameters, true).ConfigureAwait(false);
            if (!result.Success)
            {
                return new CallResult<BitfinexWithdrawalResult>(null, result.Error);
            }

            if (result.Data[0].Status == "error")
            {
                return new CallResult<BitfinexWithdrawalResult>(result.Data[0], new ServerError(result.Data[0].Message));
            }

            return new CallResult<BitfinexWithdrawalResult>(result.Data[0], null);
        }

        /// <summary>
        /// Gets information about the user associated with the api key/secret
        /// </summary>
        /// <returns></returns>
        public CallResult<BitfinexUserInfo> GetUserInfo() => GetUserInfoAsync().Result;

        /// <summary>
        /// Gets information about the user associated with the api key/secret
        /// </summary>
        /// <returns></returns>
        public async Task<CallResult<BitfinexUserInfo>> GetUserInfoAsync()
        {
            return await ExecuteRequest<BitfinexUserInfo>(GetUrl(UserInfoEndpoint, ApiVersion2), Constants.PostMethod, signed: true).ConfigureAwait(false);
        }


        #endregion

        #region private methods

        protected override Error ParseErrorResponse(JToken data)
        {
            if (data is JArray)
            {
                var error = data.ToObject<BitfinexError>();
                return new ServerError(error.ErrorCode, error.ErrorMessage);
            }

            return new ServerError(-1, data["message"].ToString());
        }

        private Uri GetUrl(string endpoint, string version)
        {
            var result = $"{BaseAddress}/v{version}/{endpoint}";
            return new Uri(result);
        }
        #endregion
        #endregion
    }
}
