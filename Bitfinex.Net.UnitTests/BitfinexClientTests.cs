using System;
using System.Collections.Generic;
using Bitfinex.Net.Objects;
using Bitfinex.Net.Objects.RestV1Objects;
using CryptoExchange.Net.Authentication;
using CryptoExchange.Net;
using NUnit.Framework;
using System.Linq;
using Bitfinex.Net.UnitTests.TestImplementations;

namespace Bitfinex.Net.UnitTests
{
    [TestFixture()]
    public class BitfinexClientTests
    {
        [TestCase]
        public void GetPlatformStatus_Should_RespondWithPlatformStatus()
        {
            // arrange
            var expected = new BitfinexPlatformStatus()
            {
                Status = PlatformStatus.Operative
            };

            var client = TestHelpers.CreateResponseClient(expected);

            // act
            var result = client.GetPlatformStatus();

            // assert
            Assert.AreEqual(true, result.Success);
            Assert.IsTrue(TestHelpers.AreEqual(expected, result.Data));
        }

        [TestCase]
        public void GetTicker_Should_RespondWithPrices()
        {
            // arrange
            var expected = new[]
            {
                new BitfinexMarketOverviewRest()
                {
                    Ask = 0.1m,
                    AskSize = 0.2m,
                    Bid = 0.3m,
                    BidSize = 0.4m,
                    DailyChangePercentage = 0.5m,
                    DailyChange = 0.6m,
                    High = 0.7m,
                    LastPrice = 0.8m,
                    Low = 0.9m,
                    Volume = 1.1m,
                    Symbol = "Test"
                }
            };

            var client = TestHelpers.CreateResponseClient(expected);

            // act
            var result = client.GetTicker("Test");

            // assert
            Assert.AreEqual(true, result.Success);
            Assert.IsTrue(TestHelpers.AreEqual(expected[0], result.Data[0]));
        }

        [TestCase]
        public void GetTrades_Should_RespondWithPrices()
        {
            // arrange
            var expected = new[]
            {
                new BitfinexTradeSimple()
                {
                    Amount = 0.1m,
                    Id = 1,
                    Price = 0.2m,
                    Timestamp = new DateTime(2017, 1, 1)
                },
                new BitfinexTradeSimple()
                {
                    Amount = 0.3m,
                    Id = 2,
                    Price = 0.4m,
                    Timestamp = new DateTime(2016, 1, 1)
                }
            };

            var client = TestHelpers.CreateResponseClient(expected);

            // act
            var result = client.GetTrades("Test");

            // assert
            Assert.AreEqual(true, result.Success);
            Assert.IsTrue(TestHelpers.AreEqual(expected[0], result.Data[0]));
            Assert.IsTrue(TestHelpers.AreEqual(expected[1], result.Data[1]));
        }

        [TestCase]
        public void GetOrderbook_Should_RespondWithOrderbook()
        {
            // arrange
            var expected = new[]
            {
                new BitfinexOrderBookEntry()
                {
                    Amount = 0.1m,
                    Price = 0.2m,
                    Count = 1
                },
                new BitfinexOrderBookEntry()
                {
                    Amount = 0.3m,
                    Price = 0.4m,
                    Count = 2
                }
            };

            var client = TestHelpers.CreateResponseClient(expected);

            // act
            var result = client.GetOrderBook("Test", Precision.PrecisionLevel0);

            // assert
            Assert.AreEqual(true, result.Success);
            Assert.IsTrue(TestHelpers.AreEqual(expected[0], result.Data[0]));
            Assert.IsTrue(TestHelpers.AreEqual(expected[1], result.Data[1]));
        }

        [TestCase]
        public void GetStats_Should_RespondWithStats()
        {
            // arrange
            var expected =
                new BitfinexStats()
                {
                    Timestamp = new DateTime(2017, 1, 1),
                    Value = 0.1m
                };

            var client = TestHelpers.CreateResponseClient(expected);

            // act
            var result = client.GetStats("test", StatKey.ActiveFundingInPositions, StatSide.Long, StatSection.History);

            // assert
            Assert.AreEqual(true, result.Success);
            Assert.IsTrue(TestHelpers.AreEqual(expected, result.Data));
        }

        [TestCase]
        public void GetLastCandle_Should_RespondWithCandle()
        {
            // arrange
            var expected =
                new BitfinexCandle()
                {
                    Timestamp = new DateTime(2017, 1, 1),
                    Volume = 0.1m,
                    Low = 0.2m,
                    High = 0.3m,
                    Close = 0.4m,
                    Open = 0.5m
                };

            var client = TestHelpers.CreateResponseClient(expected);

            // act
            var result = client.GetLastCandle(TimeFrame.FiveMinute, "test");

            // assert
            Assert.AreEqual(true, result.Success);
            Assert.IsTrue(TestHelpers.AreEqual(expected, result.Data));
        }

        [TestCase]
        public void GetCandles_Should_RespondWithCandles()
        {
            // arrange
            var expected = new[]
            {
                new BitfinexCandle()
                {
                    Timestamp = new DateTime(2017, 1, 1),
                    Volume = 0.1m,
                    Low = 0.2m,
                    High = 0.3m,
                    Close = 0.4m,
                    Open = 0.5m
                },
                new BitfinexCandle()
                {
                    Timestamp = new DateTime(2016, 1, 1),
                    Volume = 0.6m,
                    Low = 0.7m,
                    High = 0.8m,
                    Close = 0.9m,
                    Open = 1.1m
                }
            };
            var client = TestHelpers.CreateResponseClient(expected);

            // act
            var result = client.GetCandles(TimeFrame.FiveMinute, "test");

            // assert
            Assert.AreEqual(true, result.Success);
            Assert.IsTrue(TestHelpers.AreEqual(expected[0], result.Data[0]));
            Assert.IsTrue(TestHelpers.AreEqual(expected[1], result.Data[1]));
        }

        [TestCase]
        public void GetMarketAveragePrice_Should_RespondWithAveragePrice()
        {
            // arrange
            var expected =
                new BitfinexMarketAveragePrice()
                {
                    Amount = 0.1m,
                    AverageRate = 0.2m
                };

            var client = TestHelpers.CreateResponseClient(expected);

            // act
            var result = client.GetMarketAveragePrice("test", 0.1m, 0.2m);

            // assert
            Assert.AreEqual(true, result.Success);
            Assert.IsTrue(TestHelpers.AreEqual(expected, result.Data));
        }

        [TestCase]
        public void GetWallets_Should_RespondWithWallets()
        {
            // arrange
            var expected = new[]
            {
                new BitfinexWallet()
                {
                    Balance = 0.1m,
                    BalanceAvailable = 0.2m,
                    Currency = "test",
                    Type = WalletType.Exchange,
                    UnsettledInterest = 0.3m
                },
                new BitfinexWallet()
                {
                    Balance = 0.4m,
                    BalanceAvailable = 0.5m,
                    Currency = "test2",
                    Type = WalletType.Funding,
                    UnsettledInterest = 0.6m
                }
            };
            var client = TestHelpers.CreateAuthenticatedResponseClient(expected);

            // act
            var result = client.GetWallets();

            // assert
            Assert.AreEqual(true, result.Success);
            Assert.IsTrue(TestHelpers.AreEqual(expected[0], result.Data[0]));
            Assert.IsTrue(TestHelpers.AreEqual(expected[1], result.Data[1]));
        }

        [TestCase]
        public void GetActiveOrders_Should_RespondWithActiveOrders()
        {
            // arrange
            var expected = new[]
            {
                new BitfinexOrder()
                {
                    Amount = 0.1m,
                    Price = 0.2m,
                    Id = 1,
                    StatusString = "ACTIVE",
                    Type = OrderType.ExchangeFillOrKill,
                    Symbol = "test",
                    AmountOriginal = 0.3m,
                    ClientOrderId = 2,
                    Flags = 0,
                    GroupId = null,
                    Hidden = false,
                    Notify = false,
                    PlacedId = 3,
                    PriceAuxilliaryLimit = 0,
                    PriceAverage = 0.4m,
                    PriceTrailing = 0.5m,
                    TimestampCreated = new DateTime(2017,1,1),
                    TimestampUpdated = new DateTime(2017,1,1),
                    TypePrevious = null
                },
                new BitfinexOrder()
                {
                    Amount = 0.6m,
                    Price = 0.7m,
                    Id = 4,
                    StatusString = "ACTIVE",
                    Type = OrderType.Limit,
                    Symbol = "test",
                    AmountOriginal = 0.8m,
                    ClientOrderId = 5,
                    Flags = 0,
                    GroupId = null,
                    Hidden = false,
                    Notify = false,
                    PlacedId = 6,
                    PriceAuxilliaryLimit = 0,
                    PriceAverage = 0.9m,
                    PriceTrailing = 1.1m,
                    TimestampCreated = new DateTime(2016,1,1),
                    TimestampUpdated = new DateTime(2016,1,1),
                    TypePrevious = null
                }
            };

            var client = TestHelpers.CreateAuthenticatedResponseClient(expected);

            // act
            var result = client.GetActiveOrders();

            // assert
            Assert.AreEqual(true, result.Success);
            Assert.IsTrue(TestHelpers.AreEqual(expected[0], result.Data[0]));
            Assert.IsTrue(TestHelpers.AreEqual(expected[1], result.Data[1]));
        }

        [TestCase]
        public void GetTradesForOrder_Should_RespondWithTrades()
        {
            // arrange
            var expected = new[]
            {
                new BitfinexTradeDetails()
                {
                    Id = 1,
                    TimestampCreated = new DateTime(2017,1,1),
                    ExecutedAmount = 0.1m,
                    ExecutedPrice = 0.2m,
                    Fee = 0.3m,
                    FeeCurrency = "Test",
                    Maker = false,
                    OrderId = 2,
                    OrderPrice = 0.4m,
                    OrderType = OrderType.Limit,
                    Pair = "TEST"
                },
                new BitfinexTradeDetails()
                {
                    Id = 3,
                    TimestampCreated = new DateTime(2016,1,1),
                    ExecutedAmount = 0.5m,
                    ExecutedPrice = 0.6m,
                    Fee = 0.7m,
                    FeeCurrency = "Test",
                    Maker = false,
                    OrderId = 4,
                    OrderPrice = 0.8m,
                    OrderType = OrderType.Market,
                    Pair = "TEST"
                }
            };
            var client = TestHelpers.CreateAuthenticatedResponseClient(expected);

            // act
            var result = client.GetTradesForOrder("TEST", 1);

            // assert
            Assert.AreEqual(true, result.Success);
            Assert.IsTrue(TestHelpers.AreEqual(expected[0], result.Data[0]));
            Assert.IsTrue(TestHelpers.AreEqual(expected[1], result.Data[1]));
        }

        [TestCase]
        public void GetActivePositions_Should_RespondWithPositions()
        {
            // arrange
            var expected = new[]
            {
                new BitfinexPosition()
                {
                    Amount = 0.1m,
                    Symbol = "Test",
                    Status = PositionStatus.Active,
                    BasePrice = 0.2m,
                    Leverage = 0.3m,
                    LiquidationPrice = 0.4m,
                    MarginFunding = 0.5m,
                    MarginFundingType = MarginFundingType.Daily,
                    ProfitLoss = 0.6m,
                    ProfitLossPercentage = 0.7m
                },
                new BitfinexPosition()
                {
                    Amount = 0.8m,
                    Symbol = "Test2",
                    Status = PositionStatus.Active,
                    BasePrice = 0.9m,
                    Leverage = 1.1m,
                    LiquidationPrice = 1.2m,
                    MarginFunding = 1.3m,
                    MarginFundingType = MarginFundingType.Daily,
                    ProfitLoss = 1.4m,
                    ProfitLossPercentage = 1.5m
                }
            };
            var client = TestHelpers.CreateAuthenticatedResponseClient(expected);

            // act
            var result = client.GetActivePositions();

            // assert
            Assert.AreEqual(true, result.Success);
            Assert.IsTrue(TestHelpers.AreEqual(expected[0], result.Data[0]));
            Assert.IsTrue(TestHelpers.AreEqual(expected[1], result.Data[1]));
        }

        [TestCase]
        public void GetActiveFundingOffers_Should_RespondWithFundingOffers()
        {
            // arrange
            var expected = new[]
            {
                new BitfinexFundingOffer()
                {
                    Amount = 0.1m,
                    Symbol = "Test",
                    Id = 1,
                    Status = OrderStatus.Active,
                    StatusString = "ACTIVE",
                    TimestampCreated = new DateTime(2017,1,1),
                    TimestampUpdated = new DateTime(2017,1,1),
                    Flags = 0,
                    AmountOriginal = 0.2m,
                    Notify = false,
                    Hidden = false,
                    FundingType = FundingType.Lend,
                    Period = 1,
                    Rate = 0.3m,
                    RateReal = 0.4m,
                    Renew = false
                },
                new BitfinexFundingOffer()
                {
                    Amount = 0.5m,
                    Symbol = "Test",
                    Id = 2,
                    Status = OrderStatus.PartiallyFilled,
                    StatusString = "INSUFFICIENT BALANCE (G1) was: ACTIVE (note:POSCLOSE), PARTIALLY FILLED @ 6123.96104092(-0.07139999)",
                    TimestampCreated = new DateTime(2016,1,1),
                    TimestampUpdated = new DateTime(2016,1,1),
                    Flags = 0,
                    AmountOriginal = 0.6m,
                    Notify = true,
                    Hidden = true,
                    FundingType = FundingType.Loan,
                    Period = 2,
                    Rate = 0.7m,
                    RateReal = 0.8m,
                    Renew = true
                }
            };
            var client = TestHelpers.CreateAuthenticatedResponseClient(expected);

            // act
            var result = client.GetActiveFundingOffers("Test");

            // assert
            result.Data[1].Status = OrderStatus.PartiallyFilled;
            result.Data[1].StatusString = "INSUFFICIENT BALANCE (G1) was: ACTIVE (note:POSCLOSE), PARTIALLY FILLED @ 6123.96104092(-0.07139999)";
            Assert.AreEqual(true, result.Success);
            Assert.IsTrue(TestHelpers.AreEqual(expected[0], result.Data[0]));
            Assert.IsTrue(TestHelpers.AreEqual(expected[1], result.Data[1]));
        }

        [TestCase]
        public void GetFundingLoans_Should_RespondWithFundingLoans()
        {
            // arrange
            var expected = new[]
            {
                new BitfinexFundingLoan()
                {
                    Amount = 0.1m,
                    Symbol = "Test",
                    Id = 1,
                    Status = OrderStatus.Active,
                    StatusString = "ACTIVE",
                    TimestampCreated = new DateTime(2017,1,1),
                    TimestampUpdated = new DateTime(2017,1,1),
                    Flags = 0,
                    Notify = false,
                    Hidden = false,
                    Period = 1,
                    Rate = 0.3m,
                    RateReal = 0.4m,
                    Renew = false,
                    NoClose = false,
                    Side = FundingType.Lend,
                    TimestampLastPayout = new DateTime(2017,1,1),
                    TimestampOpened = new DateTime(2017,1,1)
                },
                new BitfinexFundingLoan()
                {
                    Amount = 0.5m,
                    Symbol = "Test",
                    Id = 2,
                    Status = OrderStatus.Canceled,
                    StatusString = "CANCELED",
                    TimestampCreated = new DateTime(2016,1,1),
                    TimestampUpdated = new DateTime(2016,1,1),
                    Flags = 0,
                    Notify = true,
                    Hidden = true,
                    Period = 2,
                    Rate = 0.7m,
                    RateReal = 0.8m,
                    Renew = true,
                    NoClose = true,
                    Side = FundingType.Loan,
                    TimestampLastPayout = new DateTime(2016,1,1),
                    TimestampOpened = new DateTime(2016,1,1)
                }
            };
            var client = TestHelpers.CreateAuthenticatedResponseClient(expected);

            // act
            var result = client.GetFundingLoans("Test");

            // assert
            Assert.AreEqual(true, result.Success);
            Assert.IsTrue(TestHelpers.AreEqual(expected[0], result.Data[0]));
            Assert.IsTrue(TestHelpers.AreEqual(expected[1], result.Data[1]));
        }

        [TestCase]
        public void GetFundingCredits_Should_RespondWithFundingCredits()
        {
            // arrange
            var expected = new[]
            {
                new BitfinexFundingCredit()
                {
                    Amount = 0.1m,
                    Symbol = "Test",
                    Id = 1,
                    StatusString = "ACTIVE",
                    Status = OrderStatus.Active,
                    TimestampCreated = new DateTime(2017,1,1),
                    TimestampUpdated = new DateTime(2017,1,1),
                    Flags = 0,
                    Notify = false,
                    Hidden = false,
                    Period = 1,
                    Rate = 0.3m,
                    RateReal = 0.4m,
                    Renew = false,
                    NoClose = false,
                    Side = FundingType.Lend,
                    TimestampLastPayout = new DateTime(2017,1,1),
                    TimestampOpened = new DateTime(2017,1,1),
                    PositionPair = "Test"
                },
                new BitfinexFundingCredit()
                {
                    Amount = 0.5m,
                    Symbol = "Test",
                    Id = 2,
                    StatusString = "CANCELED",
                    Status = OrderStatus.Canceled,
                    TimestampCreated = new DateTime(2016,1,1),
                    TimestampUpdated = new DateTime(2016,1,1),
                    Flags = 0,
                    Notify = true,
                    Hidden = true,
                    Period = 2,
                    Rate = 0.7m,
                    RateReal = 0.8m,
                    Renew = true,
                    NoClose = true,
                    Side = FundingType.Loan,
                    TimestampLastPayout = new DateTime(2016,1,1),
                    TimestampOpened = new DateTime(2016,1,1),
                    PositionPair = "Test"
                }
            };
            var client = TestHelpers.CreateAuthenticatedResponseClient(expected);

            // act
            var result = client.GetFundingCredits("Test");

            // assert
            Assert.AreEqual(true, result.Success);
            Assert.IsTrue(TestHelpers.AreEqual(expected[0], result.Data[0]));
            Assert.IsTrue(TestHelpers.AreEqual(expected[1], result.Data[1]));
        }

        [TestCase]
        public void GetFundingTradesHistory_Should_RespondWithFundingTrades()
        {
            // arrange
            var expected = new[]
            {
                new BitfinexFundingTrade()
                {
                    Id = 1,
                    Amount = 0.1m,
                    Timestamp = new DateTime(2017,1,1),
                    Rate = 0.2m,
                    Period = 2,
                    Currency = "Test",
                    Maker = false,
                    OfferId = 3
                },
                new BitfinexFundingTrade()
                {
                    Id = 4,
                    Amount = 0.3m,
                    Timestamp = new DateTime(2016,1,1),
                    Rate = 0.4m,
                    Period = 5,
                    Currency = "Test",
                    Maker = true,
                    OfferId = 6
                }
            };
            var client = TestHelpers.CreateAuthenticatedResponseClient(expected);

            // act
            var result = client.GetFundingTradesHistory("Test");

            // assert
            Assert.AreEqual(true, result.Success);
            Assert.IsTrue(TestHelpers.AreEqual(expected[0], result.Data[0]));
            Assert.IsTrue(TestHelpers.AreEqual(expected[1], result.Data[1]));
        }

        [TestCase]
        public void GetBaseMargin_Should_RespondWithMargin()
        {
            // arrange
            var expected = new BitfinexMarginBase()
            {
                Type = "base",
                Data = new BitfinexMarginBaseInfo()
                {
                    MarginBalance = 0.1m,
                    MarginNet = 0.2m,
                    UserProfitLoss = 0.3m,
                    UserSwapsAmount = 0.4m
                }
            };
            var client = TestHelpers.CreateAuthenticatedResponseClient(expected);

            // act
            var result = client.GetBaseMarginInfo();

            // assert
            Assert.AreEqual(true, result.Success);
            Assert.IsTrue(TestHelpers.AreEqual(expected, result.Data));
        }

        [TestCase]
        public void GetSymbolMargin_Should_RespondWithMargin()
        {
            // arrange
            var expected = new BitfinexMarginSymbol()
            {
                Symbol = "test",
                Data = new BitfinexMarginSymbolInfo()
                {
                    Buy = 0.1m,
                    GrossBalance = 0.2m,
                    Sell = 0.3m,
                    TradeableBalance = 0.4m
                }
            };
            var client = TestHelpers.CreateAuthenticatedResponseClient(expected);

            // act
            var result = client.GetSymbolMarginInfo("test");

            // assert
            Assert.AreEqual(true, result.Success);
            Assert.IsTrue(TestHelpers.AreEqual(expected, result.Data));
        }

        [TestCase]
        public void GetFundingInfo_Should_RespondWithFundingInfo()
        {
            // arrange
            var expected = new BitfinexFundingInfo()
            {
                Symbol = "test",
                Type = "sym",
                Data = new BitfinexFundingInfoDetails()
                {
                    DurationLend = 0.1m,
                    DurationLoan = 0.2m,
                    YieldLend = 0.3m,
                    YieldLoan = 0.4m
                }
            };
            var client = TestHelpers.CreateAuthenticatedResponseClient(expected);

            // act
            var result = client.GetFundingInfo("test");

            // assert
            Assert.AreEqual(true, result.Success);
            Assert.IsTrue(TestHelpers.AreEqual(expected, result.Data));
        }

        [TestCase]
        public void GetMovements_Should_RespondWithMovements()
        {
            // arrange
            var expected = new[]
            {
                new BitfinexMovement()
                {
                    Id = "test",
                    Amount = 0.1m,
                    Status = "Status",
                    Currency = "Cur",
                    Address = "add",
                    CurrencyName = "curname",
                    Fees = 0.2m,
                    Started = new DateTime(2017, 1, 1),
                    TransactionId = "tx",
                    Updated = new DateTime(2017, 1, 1)
                }
            };
            var client = TestHelpers.CreateAuthenticatedResponseClient(expected);

            // act
            var result = client.GetMovements("test");

            // assert
            Assert.AreEqual(true, result.Success);
            Assert.IsTrue(TestHelpers.AreEqual(expected[0], result.Data[0]));
        }

        [TestCase]
        public void GetAlertList_Should_RespondWithAlerts()
        {
            // arrange
            var expected = new[]
            {
                new BitfinexAlert()
                {
                    AlertKey = "key",
                    AlertType = "type",
                    Price = 0.1m,
                    Symbol = "symbol",
                    T = 0.2m
                }
            };
            var client = TestHelpers.CreateAuthenticatedResponseClient(expected);

            // act
            var result = client.GetAlertList();

            // assert
            Assert.AreEqual(true, result.Success);
            Assert.IsTrue(TestHelpers.AreEqual(expected[0], result.Data[0]));
        }

        [TestCase]
        public void SetAlert_Should_RespondWithAlert()
        {
            // arrange
            var expected =
                new BitfinexAlert()
                {
                    AlertKey = "key",
                    AlertType = "type",
                    Price = 0.1m,
                    Symbol = "symbol",
                    T = 0.2m
                };
            var client = TestHelpers.CreateAuthenticatedResponseClient(expected);

            // act
            var result = client.SetAlert("symbol", 0.1m);

            // assert
            Assert.AreEqual(true, result.Success);
            Assert.IsTrue(TestHelpers.AreEqual(expected, result.Data));
        }

        [TestCase]
        public void DeleteAlert_Should_ReturnSuccess()
        {
            // arrange
            var expected =
                new BitfinexSuccessResult()
                {
                    Success = true
                };
            var client = TestHelpers.CreateAuthenticatedResponseClient(expected);

            // act
            var result = client.DeleteAlert("symbol", 0.1m);

            // assert
            Assert.AreEqual(true, result.Success);
            Assert.IsTrue(TestHelpers.AreEqual(expected, result.Data));
        }

        [TestCase]
        public void GetAccountInfo_Should_ReturnAccountInfo()
        {
            // arrange
            var expected =
                new BitfinexAccountInfo()
                {
                    MakerFee = 0.1m,
                    TakerFee = 0.2m,
                    Fees = new[]
                    {
                        new BitfinexFee
                        {
                            TakerFee = 0.3m,
                            MakerFee = 0.4m,
                            Pairs = "BTC"
                        }
                    }
                };
            var client = TestHelpers.CreateAuthenticatedResponseClient(new [] {expected});

            // act
            var result = client.GetAccountInfo();

            // assert
            Assert.AreEqual(true, result.Success);
            Assert.IsTrue(TestHelpers.AreEqual(expected, result.Data, "Fees"));
            Assert.IsTrue(TestHelpers.AreEqual(expected.Fees[0], result.Data.Fees[0]));
        }

        [TestCase]
        public void GetWithdrawalFees_Should_ReturnFees()
        {
            // arrange
            var expected =
                new BitfinexWithdrawalFees()
                {
                    Withdraw = new Dictionary<string, decimal>
                    {
                        { "BTC", 0.1m },
                        { "ETH", 0.2m },
                    }
                };
            var client = TestHelpers.CreateAuthenticatedResponseClient(expected);

            // act
            var result = client.GetWithdrawalFees();

            // assert
            Assert.AreEqual(true, result.Success);
            Assert.AreEqual(expected.Withdraw["BTC"], result.Data.Withdraw["BTC"]);
            Assert.AreEqual(expected.Withdraw["ETH"], result.Data.Withdraw["ETH"]);
        }

        [Test]
        public void ProvidingApiCredentials_Should_SaveApiCredentials()
        {
            // arrange
            // act
            var authProvider = new BitfinexAuthenticationProvider(new ApiCredentials("TestKey", "TestSecret"));

            // assert
            Assert.AreEqual(authProvider.Credentials.Key.GetString(), "TestKey");
            Assert.AreEqual(authProvider.Credentials.Secret.GetString(), "TestSecret");
        }

        [Test]
        public void SigningString_Should_ReturnCorrectString()
        {
            // arrange
            var authProvider = new BitfinexAuthenticationProvider(new ApiCredentials("TestKey", "TestSecret"));

            // act
            string signed = authProvider.Sign("SomeTestString");

            // assert
            Assert.AreEqual(signed, "9052C73092B21B945BC5859CADBA6A5658E142F021FCB092A72F68E8A0D5E6351CFEBAE52DB9067D4360F796CB520960");
        }

        [Test]
        public void MakingAuthv2Call_Should_SendAuthHeaders()
        {
            // arrange
            var client = TestHelpers.CreateClient(new BitfinexClientOptions(){ ApiCredentials = new ApiCredentials("TestKey", "t")});
            var request = TestHelpers.SetResponse((RestClient)client, "{}");

            // act
            client.GetActiveOrders();

            // assert
            Assert.IsTrue(request.Headers.AllKeys.Contains("bfx-nonce"));
            Assert.IsTrue(request.Headers.AllKeys.Contains("bfx-signature"));
            Assert.IsTrue(request.Headers["bfx-apikey"] == "TestKey");
        }

        [Test]
        public void MakingAuthv1Call_Should_SendAuthHeaders()
        {
            // arrange
            var client = TestHelpers.CreateClient(new BitfinexClientOptions() { ApiCredentials = new ApiCredentials("TestKey", "t") });
            var request = TestHelpers.SetResponse((RestClient)client, "{}");

            // act
            client.GetAccountInfo();

            // assert
            Assert.IsTrue(request.Headers.AllKeys.Contains("X-BFX-SIGNATURE"));
            Assert.IsTrue(request.Headers.AllKeys.Contains("X-BFX-PAYLOAD"));
            Assert.IsTrue(request.Headers["X-BFX-APIKEY"] == "TestKey");
        }
    }
}
