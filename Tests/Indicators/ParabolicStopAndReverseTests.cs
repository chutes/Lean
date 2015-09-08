using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NUnit.Framework;
using QuantConnect.Data.Market;
using QuantConnect.Indicators;

namespace QuantConnect.Tests.Indicators
{
    [TestFixture]
    public class ParabolicStopAndReverseTests
    {
        [Test]
        public void PSARComputesCorrectly()
        {
            const int period = 5;
            TradeBar tradebar1 = new TradeBar(DateTime.UtcNow.AddSeconds(1.0), "SPY", 3755.74m, 3836.86m, 3643.25m, 3790.55m, (long) Math.Pow(1.51 * 10.0, 9.0));
            TradeBar tradebar2 = new TradeBar(DateTime.UtcNow.AddSeconds(1.0), "SPY", 3766.57m, 3766.57m, 3542.73m, 3546.2m, (long)Math.Pow(1.51 * 10.0, 9.0));
            TradeBar tradebar3 = new TradeBar(DateTime.UtcNow.AddSeconds(1.0), "SPY", 3543.13m, 3576.17m, 3371.75m, 3507.31m, (long)Math.Pow(1.74 * 10.0, 9.0));
            TradeBar tradebar4 = new TradeBar(DateTime.UtcNow.AddSeconds(1.0), "SPY", 3488.31m, 3513.55m, 3334.02m, 3340.81m, (long)Math.Pow(1.6 * 10.0, 9.0));
            TradeBar tradebar5 = new TradeBar(DateTime.UtcNow.AddSeconds(1.0), "SPY", 3337.26m, 3529.75m, 3314.75m, 3529.6m, (long)Math.Pow(1.63 * 10.0, 9.0));
            TradeBar tradebar6 = new TradeBar(DateTime.UtcNow.AddSeconds(1.0), "SPY", 3558.31m, 3756.17m, 3558.21m, 3717.41m, (long)Math.Pow(1.69 * 10.0, 9.0));
            TradeBar tradebar7 = new TradeBar(DateTime.UtcNow.AddSeconds(1.0), "SPY", 3715.22m, 3717.17m, 3517.79m, 3544.35m, (long)Math.Pow(1.69 * 10.0, 9.0));
            TradeBar tradebar8 = new TradeBar(DateTime.UtcNow.AddSeconds(1.0), "SPY", 3548.77m, 3572.62m, 3447.9m, 3478.14m, (long) Math.Pow(1.53 * 10.0, 9.0));

            var psar14 = new ParabolicStopAndReverse(period);
            psar14.Update(tradebar1);
            Assert.AreEqual(3643.25m, psar14._extremePoint);
            Assert.AreEqual(0.02m, psar14._acceleration);
            Assert.AreEqual(3.8722m, psar14._psarEpAcc);
            Assert.AreEqual(0m, psar14._psarInitial);
            Assert.AreEqual(3836.86m, psar14._psar);
            Assert.AreEqual(false, psar14._trend);

            psar14.Update(tradebar2);
            Assert.AreEqual(3542.73m, psar14._extremePoint);
            Assert.AreEqual(0.04m, psar14._acceleration);
            Assert.AreEqual(11.7652m, psar14._psarEpAcc);
            Assert.AreEqual(3836.86m, psar14._psarInitial);
            Assert.AreEqual(3836.86m, psar14._psar);
            Assert.AreEqual(false, psar14._trend);

            psar14.Update(tradebar3);
            Assert.AreEqual(3371.75m, psar14._extremePoint);
            Assert.AreEqual(0.06m, psar14._acceleration);
            Assert.AreEqual(27.9066m, psar14._psarEpAcc);
            Assert.AreEqual(3836.86m, psar14._psarInitial);
            Assert.AreEqual(3836.86m, psar14._psar);
            Assert.AreEqual(false, psar14._trend);

            psar14.Update(tradebar4);
            Assert.AreEqual(3334.02m, psar14._extremePoint);
            Assert.AreEqual(0.08m, psar14._acceleration);
            Assert.AreEqual(37.994672m, psar14._psarEpAcc);
            Assert.AreEqual(3808.9534m, psar14._psarInitial);
            Assert.AreEqual(3808.9534m, psar14._psar);
            Assert.AreEqual(false, psar14._trend);

            psar14.Update(tradebar5);
            Assert.AreEqual(3314.75m, psar14._extremePoint);
            Assert.AreEqual(0.1m, psar14._acceleration);
            Assert.AreEqual(45.6208728m, psar14._psarEpAcc);
            Assert.AreEqual(3770.958728m, psar14._psarInitial);
            Assert.AreEqual(3770.958728m, psar14._psar);
            Assert.AreEqual(false, psar14._trend);
            
            psar14.Update(tradebar6);
            Assert.AreEqual(3756.17m, psar14._extremePoint);
            Assert.AreEqual(0.02m, psar14._acceleration);
            Assert.AreEqual(-8.8284m, psar14._psarEpAcc);
            Assert.AreEqual(3725.3378552m, psar14._psarInitial);
            Assert.AreEqual(3314.75m, psar14._psar);
            Assert.AreEqual(true, psar14._trend);

            psar14.Update(tradebar7);
            Assert.AreEqual(3756.17m, psar14._extremePoint);
            Assert.AreEqual(0.02m, psar14._acceleration);
            Assert.AreEqual(-8.8284m, psar14._psarEpAcc);
            Assert.AreEqual(3314.75m, psar14._psarInitial);
            Assert.AreEqual(3314.75m, psar14._psar);
            Assert.AreEqual(true, psar14._trend);

            psar14.Update(tradebar8);
            Assert.AreEqual(3756.17m, psar14._extremePoint);
            Assert.AreEqual(0.02m, psar14._acceleration);
            Assert.AreEqual(-8.651832m, psar14._psarEpAcc);
            Assert.AreEqual(3323.5784m, psar14._psarInitial);
            Assert.AreEqual(3323.5784m, psar14._psar);
            Assert.AreEqual(true, psar14._trend);
        }

        [Test]
        public void PSARComputesCorrectly2()
        {
            //Date,                     Open,   High,   Low,    Close,  Parabolic SAR 0.02 0.20
            //6/23/1998 12:00:00 AM,    111.44, 112.06, 107.5,  111.91, 107.5
            //7/6/1998 12:00:00 AM,     112.16, 116,    111.61, 116,    107.5
            //7/17/1998 12:00:00 AM,    115.98, 119,    115.06, 118.56, 107.5
            //7/30/1998 12:00:00 AM,    118.75, 119.23, 111.88, 114.22, 107.96
            //8/12/1998 12:00:00 AM,    114.34, 114.5,  105.5,  108.69, 119.23
            //8/25/1998 12:00:00 AM,    108.81, 111.25, 105.5,  109.5,  119.23
            //9/4/1998 12:00:00 AM,     108.28, 109.62, 93.62,  97.75,  118.9554
            //9/18/1998 12:00:00 AM,    100.88, 105.25, 96.81,  102.09, 117.942
            const int period = 1;
            TradeBar tradebar1 = new TradeBar(DateTime.UtcNow.AddSeconds(1.0), "SPY", 111.44m, 112.06m, 107.5m, 111.91m, (long)Math.Pow(1.51 * 10.0, 9.0));
            TradeBar tradebar2 = new TradeBar(DateTime.UtcNow.AddSeconds(1.0), "SPY", 112.16m, 116m, 111.61m, 116m, (long)Math.Pow(1.51 * 10.0, 9.0));
            TradeBar tradebar3 = new TradeBar(DateTime.UtcNow.AddSeconds(1.0), "SPY", 115.98m, 119m, 115.06m, 118.56m, (long)Math.Pow(1.74 * 10.0, 9.0));
            TradeBar tradebar4 = new TradeBar(DateTime.UtcNow.AddSeconds(1.0), "SPY", 118.75m, 119.23m, 111.88m, 114.22m, (long)Math.Pow(1.6 * 10.0, 9.0));
            TradeBar tradebar5 = new TradeBar(DateTime.UtcNow.AddSeconds(1.0), "SPY", 114.34m, 114.5m, 105.5m, 108.69m, (long)Math.Pow(1.63 * 10.0, 9.0));
            TradeBar tradebar6 = new TradeBar(DateTime.UtcNow.AddSeconds(1.0), "SPY", 108.81m, 111.25m, 105.5m, 109.5m, (long)Math.Pow(1.69 * 10.0, 9.0));
            TradeBar tradebar7 = new TradeBar(DateTime.UtcNow.AddSeconds(1.0), "SPY", 108.28m, 109.62m, 93.62m, 97.75m, (long)Math.Pow(1.69 * 10.0, 9.0));
            TradeBar tradebar8 = new TradeBar(DateTime.UtcNow.AddSeconds(1.0), "SPY", 100.88m, 105.25m, 96.81m, 102.09m, (long)Math.Pow(1.53 * 10.0, 9.0));

            //Date,                     Open,   High,   Low,    Close,  Parabolic SAR 0.02 0.20
            //10/1/1998 12:00:00 AM,    99.62,  107,    98.09,  98.81,  116.9691
            //10/14/1998 12:00:00 AM,   98.88,  101.81, 92.22,  100.56, 116.0351
            //10/27/1998 12:00:00 AM,   100.12, 108.97, 99.94,  107,    114.6062
            //11/9/1998 12:00:00 AM,    106.56, 114.56, 106.03, 113.31, 92.22
            //11/20/1998 12:00:00 AM,   113,    116.75, 111.69, 116.62, 92.6668
            //12/3/1998 12:00:00 AM,    117.47, 119.72, 115.09, 115.34, 93.63013
            //12/16/1998 12:00:00 AM,   116.62, 119.75, 113.75, 116.53, 95.19552
            //12/29/1998 12:00:00 AM,   117.22, 124.48, 117.03, 124.31, 97.15987
            TradeBar tradebar9 = new TradeBar(DateTime.UtcNow.AddSeconds(1.0), "SPY", 99.62m, 107m, 98.09m, 98.81m, (long)Math.Pow(1.51 * 10.0, 9.0));
            TradeBar tradebar10 = new TradeBar(DateTime.UtcNow.AddSeconds(1.0), "SPY", 98.88m, 101.81m, 92.22m, 100.56m, (long)Math.Pow(1.51 * 10.0, 9.0));
            TradeBar tradebar11 = new TradeBar(DateTime.UtcNow.AddSeconds(1.0), "SPY", 100.12m, 108.97m, 99.94m, 107m, (long)Math.Pow(1.74 * 10.0, 9.0));
            TradeBar tradebar12 = new TradeBar(DateTime.UtcNow.AddSeconds(1.0), "SPY", 106.56m, 114.56m, 106.03m, 113.31m, (long)Math.Pow(1.6 * 10.0, 9.0));
            TradeBar tradebar13 = new TradeBar(DateTime.UtcNow.AddSeconds(1.0), "SPY", 113m, 116.75m, 111.69m, 116.62m, (long)Math.Pow(1.63 * 10.0, 9.0));
            TradeBar tradebar14 = new TradeBar(DateTime.UtcNow.AddSeconds(1.0), "SPY", 117.47m, 119.72m, 115.09m, 115.34m, (long)Math.Pow(1.69 * 10.0, 9.0));
            TradeBar tradebar15 = new TradeBar(DateTime.UtcNow.AddSeconds(1.0), "SPY", 116.62m, 119.75m, 113.75m, 116.53m, (long)Math.Pow(1.69 * 10.0, 9.0));
            TradeBar tradebar16 = new TradeBar(DateTime.UtcNow.AddSeconds(1.0), "SPY", 117.22m, 124.48m, 117.03m, 124.31m, (long)Math.Pow(1.53 * 10.0, 9.0));

            //1/11/1999 12:00:00 AM,    123.94, 128.5,  121.72, 126.53, 99.89188
            //1/22/1999 12:00:00 AM,    126.22, 127.94, 120.38, 122.56, 103.3249
            //2/4/1999 12:00:00 AM,     123.28, 128.69, 121.91, 125.5,  106.3459
            //2/17/1999 12:00:00 AM,    125.66, 126.09, 121.33, 122.75, 109.4741
            //3/2/1999 12:00:00 AM,     123.19, 128.84, 122.22, 122.81, 112.1643
            //3/15/1999 12:00:00 AM,    123.09, 131.25, 121.78, 131.22, 114.8324
            //3/26/1999 12:00:00 AM,    131.12, 132.62, 125.62, 128.56, 117.7876
            //4/8/1999 12:00:00 AM,     129.16, 134.94, 128.12, 134.84, 120.7541
            //4/21/1999 12:00:00 AM,    134.44, 136.47, 128.38, 134.88, 123.5912
            TradeBar tradebar17 = new TradeBar(DateTime.UtcNow.AddSeconds(1.0), "SPY", 123.94m, 128.5m, 121.72m, 126.53m, (long)Math.Pow(1.51 * 10.0, 9.0));
            TradeBar tradebar18 = new TradeBar(DateTime.UtcNow.AddSeconds(1.0), "SPY", 126.22m, 127.94m, 120.38m, 122.56m, (long)Math.Pow(1.51 * 10.0, 9.0));
            TradeBar tradebar19 = new TradeBar(DateTime.UtcNow.AddSeconds(1.0), "SPY", 123.28m, 128.69m, 121.91m, 125.5m, (long)Math.Pow(1.74 * 10.0, 9.0));
            TradeBar tradebar20 = new TradeBar(DateTime.UtcNow.AddSeconds(1.0), "SPY", 125.66m, 126.09m, 121.33m, 122.75m, (long)Math.Pow(1.6 * 10.0, 9.0));
            TradeBar tradebar21 = new TradeBar(DateTime.UtcNow.AddSeconds(1.0), "SPY", 123.19m, 128.84m, 122.22m, 122.81m, (long)Math.Pow(1.63 * 10.0, 9.0));
            TradeBar tradebar22 = new TradeBar(DateTime.UtcNow.AddSeconds(1.0), "SPY", 123.09m, 131.25m, 121.78m, 131.22m, (long)Math.Pow(1.63 * 10.0, 9.0));
            TradeBar tradebar23 = new TradeBar(DateTime.UtcNow.AddSeconds(1.0), "SPY", 131.12m, 132.62m, 125.62m, 128.56m, (long)Math.Pow(1.69 * 10.0, 9.0));
            TradeBar tradebar24 = new TradeBar(DateTime.UtcNow.AddSeconds(1.0), "SPY", 129.16m, 134.94m, 128.12m, 134.84m, (long)Math.Pow(1.69 * 10.0, 9.0));
            TradeBar tradebar25 = new TradeBar(DateTime.UtcNow.AddSeconds(1.0), "SPY", 134.44m, 136.47m, 128.38m, 134.88m, (long)Math.Pow(1.53 * 10.0, 9.0));

            var psar14 = new ParabolicStopAndReverse(period, .02m, .2m, true);
            psar14.Update(tradebar1);
            Assert.AreEqual(107.5m, psar14._psar);
            Assert.AreEqual(true, psar14._trend);

            psar14.Update(tradebar2);
            Assert.AreEqual(107.5m, psar14._psar);
            Assert.AreEqual(true, psar14._trend);

            psar14.Update(tradebar3);
            Assert.AreEqual(107.5m, psar14._psar);
            Assert.AreEqual(true, psar14._trend);

            psar14.Update(tradebar4);
            //107.96
            Assert.AreEqual(108.19m, psar14._psar);
            Assert.AreEqual(true, psar14._trend);

            // begin falling trend
            psar14.Update(tradebar5);
            Assert.AreEqual(119.23m, psar14._psar);
            Assert.AreEqual(false, psar14._trend);

            psar14.Update(tradebar6);
            Assert.AreEqual(false, psar14._trend);
            Assert.AreEqual(119.23m, psar14._psar);

            //118.9554
            psar14.Update(tradebar7);
            Assert.AreEqual(118.9554m, psar14._psar);
            Assert.AreEqual(false, psar14._trend);

            //117.942
            psar14.Update(tradebar8);
            Assert.AreEqual(117.941984m, psar14._psar);
            Assert.AreEqual(false, psar14._trend);

            //Date,                     Open,   High,   Low,    Close,  Parabolic SAR 0.02 0.20
            //10/1/1998 12:00:00 AM,    99.62,  107,    98.09,  98.81,  116.9691
            //10/14/1998 12:00:00 AM,   98.88,  101.81, 92.22,  100.56, 116.0351
            //10/27/1998 12:00:00 AM,   100.12, 108.97, 99.94,  107,    114.6062
            //11/9/1998 12:00:00 AM,    106.56, 114.56, 106.03, 113.31, 92.22
            //11/20/1998 12:00:00 AM,   113,    116.75, 111.69, 116.62, 92.6668
            //12/3/1998 12:00:00 AM,    117.47, 119.72, 115.09, 115.34, 93.63013
            //12/16/1998 12:00:00 AM,   116.62, 119.75, 113.75, 116.53, 95.19552
            //12/29/1998 12:00:00 AM,   117.22, 124.48, 117.03, 124.31, 97.15987

            //116.9691m
            psar14.Update(tradebar9);
            Assert.AreEqual(116.96910464m, psar14._psar);
            Assert.AreEqual(false, psar14._trend);

            //116.0351m
            psar14.Update(tradebar10);
            Assert.AreEqual(116.0351404544m, psar14._psar);
            Assert.AreEqual(false, psar14._trend);

            //114.6062m
            psar14.Update(tradebar11);
            Assert.AreEqual(114.606232027136m, psar14._psar);
            Assert.AreEqual(false, psar14._trend);

            psar14.Update(tradebar12);
            Assert.AreEqual(92.22m, psar14._psar);
            Assert.AreEqual(true, psar14._trend);

            psar14.Update(tradebar13);
            Assert.AreEqual(92.6668m, psar14._psar);
            Assert.AreEqual(true, psar14._trend);

            //93.63013m
            psar14.Update(tradebar14);
            Assert.AreEqual(93.630128m, psar14._psar);
            Assert.AreEqual(true, psar14._trend);

            //95.19552m
            psar14.Update(tradebar15);
            Assert.AreEqual(95.19552032m, psar14._psar);
            Assert.AreEqual(true, psar14._trend);

            // TODO: this value diverges when rounded from expected
            //97.15987m
            psar14.Update(tradebar16);
            Assert.AreEqual(97.1598786944m, psar14._psar);
            Assert.AreEqual(true, psar14._trend);

            //1/11/1999 12:00:00 AM,    123.94, 128.5,  121.72, 126.53, 99.89188
            //1/22/1999 12:00:00 AM,    126.22, 127.94, 120.38, 122.56, 103.3249
            //2/4/1999 12:00:00 AM,     123.28, 128.69, 121.91, 125.5,  106.3459
            //2/17/1999 12:00:00 AM,    125.66, 126.09, 121.33, 122.75, 109.4741
            //3/2/1999 12:00:00 AM,     123.19, 128.84, 122.22, 122.81, 112.1643
            //3/15/1999 12:00:00 AM,    123.09, 131.25, 121.78, 131.22, 114.8324
            //3/26/1999 12:00:00 AM,    131.12, 132.62, 125.62, 128.56, 117.7876
            //4/8/1999 12:00:00 AM,     129.16, 134.94, 128.12, 134.84, 120.7541
            //4/21/1999 12:00:00 AM,    134.44, 136.47, 128.38, 134.88, 123.5912

            //99.89188m
            psar14.Update(tradebar17);
            Assert.AreEqual(99.89189082496m, psar14._psar);
            Assert.AreEqual(true, psar14._trend);

            //103.3249m
            psar14.Update(tradebar18);
            Assert.AreEqual(103.3248639259648m, psar14._psar);
            Assert.AreEqual(true, psar14._trend);

            //106.3459m
            psar14.Update(tradebar19);
            Assert.AreEqual(106.345880254849024m, psar14._psar);
            Assert.AreEqual(true, psar14._trend);

            //109.4741m
            psar14.Update(tradebar20);
            Assert.AreEqual(109.47405701917016064m, psar14._psar);
            Assert.AreEqual(true, psar14._trend);

            //112.1643m
            psar14.Update(tradebar21);
            Assert.AreEqual(112.1642890364863381504m, psar14._psar);
            Assert.AreEqual(true, psar14._trend);

            //114.8324m
            psar14.Update(tradebar22);
            Assert.AreEqual(114.832402790648524046336m, psar14._psar);
            Assert.AreEqual(true, psar14._trend);

            //117.7876m
            psar14.Update(tradebar23);
            Assert.AreEqual(117.78757028833178971799552m, psar14._psar);
            Assert.AreEqual(true, psar14._trend);

            //120.7541m
            psar14.Update(tradebar24);
            Assert.AreEqual(120.754056230665431774396416m, psar14._psar);
            Assert.AreEqual(true, psar14._trend);

            //123.5912m
            psar14.Update(tradebar25);
            Assert.AreEqual(123.5912449845323454195171328m, psar14._psar);
            Assert.AreEqual(true, psar14._trend);
        }

        [Test]
        public void PSARComputesCorrectly3()
        {

            //Date,                     Open,   High,   Low,    Close,  Parabolic SAR 0.02 0.20
            //1/16/2013 12:00:00 AM,    146.77, 147.28, 146.61, 147.05, 146.61
            //1/17/2013 12:00:00 AM,    147.7,  148.42, 147.43, 148,    146.61
            //1/18/2013 12:00:00 AM,    147.97, 148.49, 147.43, 148.33, 146.61
            //1/22/2013 12:00:00 AM,    148.33, 149.13, 147.98, 149.13, 146.6852
            //1/23/2013 12:00:00 AM,    149.13, 149.5,  148.86, 149.37, 146.8319
            //1/24/2013 12:00:00 AM,    149.15, 150.14, 149.01, 149.41, 147.0453
            //1/25/2013 12:00:00 AM,    149.88, 150.25, 149.37, 150.25, 147.3548
            //1/28/2013 12:00:00 AM,    150.29, 150.33, 149.51, 150.07, 147.7022
            //1/29/2013 12:00:00 AM,    149.77, 150.85, 149.67, 150.66, 148.0701
            //1/30/2013 12:00:00 AM,    150.64, 150.94, 149.93, 150.07, 148.5149
            //1/31/2013 12:00:00 AM,    149.89, 150.38, 149.6,  149.7,  148.9514
            TradeBar tradebar1 = new TradeBar(DateTime.UtcNow.AddSeconds(1.0), "SPY", 146.77m, 147.28m, 146.61m, 147.05m, (long)Math.Pow(1.51 * 10.0, 9.0));
            TradeBar tradebar2 = new TradeBar(DateTime.UtcNow.AddSeconds(1.0), "SPY", 146.7m, 148.42m, 147.43m, 148m, (long)Math.Pow(1.51 * 10.0, 9.0));
            TradeBar tradebar3 = new TradeBar(DateTime.UtcNow.AddSeconds(1.0), "SPY", 147.97m, 148.49m, 147.43m, 148.33m, (long)Math.Pow(1.74 * 10.0, 9.0));
            TradeBar tradebar4 = new TradeBar(DateTime.UtcNow.AddSeconds(1.0), "SPY", 148.33m, 149.13m, 147.98m, 149.13m, (long)Math.Pow(1.6 * 10.0, 9.0));
            TradeBar tradebar5 = new TradeBar(DateTime.UtcNow.AddSeconds(1.0), "SPY", 149.13m, 149.5m, 148.86m, 149.37m, (long)Math.Pow(1.63 * 10.0, 9.0));
            TradeBar tradebar6 = new TradeBar(DateTime.UtcNow.AddSeconds(1.0), "SPY", 149.15m, 150.14m, 149.01m, 149.41m, (long)Math.Pow(1.69 * 10.0, 9.0));
            TradeBar tradebar7 = new TradeBar(DateTime.UtcNow.AddSeconds(1.0), "SPY", 149.88m, 150.25m, 149.37m, 150.25m, (long)Math.Pow(1.69 * 10.0, 9.0));
            TradeBar tradebar8 = new TradeBar(DateTime.UtcNow.AddSeconds(1.0), "SPY", 150.29m, 150.33m, 149.51m, 150.07m, (long)Math.Pow(1.53 * 10.0, 9.0));
            TradeBar tradebar9 = new TradeBar(DateTime.UtcNow.AddSeconds(1.0), "SPY", 149.77m, 150.85m, 149.67m, 150.66m, (long)Math.Pow(1.53 * 10.0, 9.0));
            TradeBar tradebar10 = new TradeBar(DateTime.UtcNow.AddSeconds(1.0), "SPY", 150.64m, 150.94m, 149.93m, 150.07m, (long)Math.Pow(1.53 * 10.0, 9.0));
            TradeBar tradebar11 = new TradeBar(DateTime.UtcNow.AddSeconds(1.0), "SPY", 149.89m, 150.38m, 149.6m, 149.7m, (long)Math.Pow(1.53 * 10.0, 9.0));

            //Date,                     Open,   High,   Low,    Close,  Parabolic SAR 0.02 0.20
            //2/1/2013 12:00:00 AM,     150.65, 151.42, 150.39, 151.24, 149.3094
            //2/4/2013 12:00:00 AM,     150.32, 150.58, 149.43, 149.54, 151.42
            //2/5/2013 12:00:00 AM,     150.35, 151.48, 150.29, 151.05, 149.43
            //2/6/2013 12:00:00 AM,     150.52, 151.26, 150.41, 151.16, 149.43
            //2/7/2013 12:00:00 AM,     151.21, 151.35, 149.86, 150.96, 149.471
            //2/8/2013 12:00:00 AM,     151.23, 151.89, 151.22, 151.8,  149.5112
            //2/11/2013 12:00:00 AM,    151.75, 151.9,  151.39, 151.77, 149.6063
            //2/12/2013 12:00:00 AM,    151.78, 152.3,  151.61, 152.02, 149.7439
            //2/13/2013 12:00:00 AM,    152.33, 152.61, 151.72, 152.15, 149.9484
            //2/14/2013 12:00:00 AM,    151.69, 152.47, 151.52, 152.29, 150.2146
            //2/15/2013 12:00:00 AM,    152.43, 152.59, 151.55, 152.11, 150.4541
            //2/19/2013 12:00:00 AM,    152.37, 153.28, 152.36, 153.25, 150.6697
            //2/20/2013 12:00:00 AM,    153.14, 153.19, 151.3,  151.34, 150.983
            //2/21/2013 12:00:00 AM,    150.92, 151.42, 149.94, 150.42, 153.28
            //2/22/2013 12:00:00 AM,    151.16, 151.89, 150.49, 151.89, 153.2132
            //2/25/2013 12:00:00 AM,    152.63, 152.86, 149,    149,    153.1477
            //2/26/2013 12:00:00 AM,    149.72, 150.2,  148.73, 150.02, 152.9818
            //2/27/2013 12:00:00 AM,    149.89, 152.33, 149.76, 151.91, 152.86
            //2/28/2013 12:00:00 AM,    151.9,  152.87, 151.41, 151.61, 148.73
            TradeBar tradebar12 = new TradeBar(DateTime.UtcNow.AddSeconds(1.0), "SPY", 150.65m, 151.42m, 150.39m, 151.24m, (long)Math.Pow(1.51 * 10.0, 9.0));
            TradeBar tradebar13 = new TradeBar(DateTime.UtcNow.AddSeconds(1.0), "SPY", 150.32m, 150.58m, 149.43m, 149.54m, (long)Math.Pow(1.51 * 10.0, 9.0));
            TradeBar tradebar14 = new TradeBar(DateTime.UtcNow.AddSeconds(1.0), "SPY", 150.35m, 151.48m, 150.29m, 151.05m, (long)Math.Pow(1.74 * 10.0, 9.0));
            TradeBar tradebar15 = new TradeBar(DateTime.UtcNow.AddSeconds(1.0), "SPY", 150.52m, 151.26m, 150.41m, 151.16m, (long)Math.Pow(1.6 * 10.0, 9.0));
            TradeBar tradebar16 = new TradeBar(DateTime.UtcNow.AddSeconds(1.0), "SPY", 151.21m, 151.35m, 149.86m, 150.96m, (long)Math.Pow(1.63 * 10.0, 9.0));
            TradeBar tradebar17 = new TradeBar(DateTime.UtcNow.AddSeconds(1.0), "SPY", 151.23m, 151.89m, 151.22m, 151.8m, (long)Math.Pow(1.69 * 10.0, 9.0));
            TradeBar tradebar18 = new TradeBar(DateTime.UtcNow.AddSeconds(1.0), "SPY", 151.75m, 151.9m, 151.39m, 151.77m, (long)Math.Pow(1.69 * 10.0, 9.0));
            TradeBar tradebar19 = new TradeBar(DateTime.UtcNow.AddSeconds(1.0), "SPY", 151.78m, 152.3m, 151.61m, 152.02m, (long)Math.Pow(1.53 * 10.0, 9.0));
            TradeBar tradebar20 = new TradeBar(DateTime.UtcNow.AddSeconds(1.0), "SPY", 152.33m, 152.61m, 151.72m, 152.15m, (long)Math.Pow(1.53 * 10.0, 9.0));
            TradeBar tradebar21 = new TradeBar(DateTime.UtcNow.AddSeconds(1.0), "SPY", 151.69m, 152.47m, 151.52m, 152.29m, (long)Math.Pow(1.53 * 10.0, 9.0));
            TradeBar tradebar22 = new TradeBar(DateTime.UtcNow.AddSeconds(1.0), "SPY", 152.43m, 152.59m, 151.55m, 152.11m, (long)Math.Pow(1.53 * 10.0, 9.0));
            TradeBar tradebar23 = new TradeBar(DateTime.UtcNow.AddSeconds(1.0), "SPY", 152.37m, 153.28m, 152.36m, 153.25m, (long)Math.Pow(1.51 * 10.0, 9.0));
            TradeBar tradebar24 = new TradeBar(DateTime.UtcNow.AddSeconds(1.0), "SPY", 153.14m, 153.19m, 151.3m, 151.34m, (long)Math.Pow(1.51 * 10.0, 9.0));
            TradeBar tradebar25 = new TradeBar(DateTime.UtcNow.AddSeconds(1.0), "SPY", 150.92m, 151.42m, 149.94m, 150.42m, (long)Math.Pow(1.74 * 10.0, 9.0));
            TradeBar tradebar26 = new TradeBar(DateTime.UtcNow.AddSeconds(1.0), "SPY", 151.16m, 151.89m, 150.49m, 151.89m, (long)Math.Pow(1.6 * 10.0, 9.0));
            TradeBar tradebar27 = new TradeBar(DateTime.UtcNow.AddSeconds(1.0), "SPY", 152.63m, 152.86m, 149m, 149m, (long)Math.Pow(1.63 * 10.0, 9.0));
            TradeBar tradebar28 = new TradeBar(DateTime.UtcNow.AddSeconds(1.0), "SPY", 149.72m, 150.2m, 148.73m, 150.02m, (long)Math.Pow(1.69 * 10.0, 9.0));
            TradeBar tradebar29 = new TradeBar(DateTime.UtcNow.AddSeconds(1.0), "SPY", 149.89m, 152.33m, 149.76m, 151.91m, (long)Math.Pow(1.69 * 10.0, 9.0));
            TradeBar tradebar30 = new TradeBar(DateTime.UtcNow.AddSeconds(1.0), "SPY", 151.9m, 152.87m, 151.41m, 151.61m, (long)Math.Pow(1.53 * 10.0, 9.0));

            //Date,                     Open,   High,   Low,    Close,  Parabolic SAR 0.02 0.20
            //3/1/2013 12:00:00 AM,     151.09, 152.34, 150.41, 152.11, 148.8128
            //3/4/2013 12:00:00 AM,     151.76, 152.92, 151.52, 152.92, 148.8939
            //3/5/2013 12:00:00 AM,     153.66, 154.7,  153.64, 154.29, 149.055
            //3/6/2013 12:00:00 AM,     154.84, 154.92, 154.16, 154.5,  149.3937
            //3/7/2013 12:00:00 AM,     154.71, 154.98, 154.52, 154.78, 149.8358
            //3/8/2013 12:00:00 AM,     155.46, 155.65, 154.66, 155.44, 150.3502
            //3/11/2013 12:00:00 AM,    155.33, 156.04, 155.13, 156.03, 150.9862
            //3/12/2013 12:00:00 AM,    155.92, 156.1,  155.21, 155.68, 151.6937
            //3/13/2013 12:00:00 AM,    155.76, 156.12, 155.23, 155.9,  152.3987
            //3/14/2013 12:00:00 AM,    156.31, 156.8,  156.22, 156.73, 153.0685
            //3/15/2013 12:00:00 AM,    155.85, 156.04, 155.31, 155.83, 153.8148
            //3/18/2013 12:00:00 AM,    154.34, 155.64, 154.2,  154.97, 156.8
            //3/19/2013 12:00:00 AM,    155.3,  155.51, 153.59, 154.61, 156.748
            //3/20/2013 12:00:00 AM,    155.52, 155.95, 155.26, 155.69, 156.6217
            //3/21/2013 12:00:00 AM,    154.75, 155.64, 154.1,  154.36, 156.5004
            //3/22/2013 12:00:00 AM,    154.84, 155.6,  154.73, 155.6,  156.384
            //3/25/2013 12:00:00 AM,    155.99, 156.27, 154.35, 154.95, 156.2722
            //3/26/2013 12:00:00 AM,    155.59, 156.23, 155.42, 156.19, 156.27
            //3/27/2013 12:00:00 AM,    155.26, 156.24, 155,    156.19, 156.27
            //3/28/2013 12:00:00 AM,    156.09, 156.85, 155.75, 156.67, 153.59
            TradeBar tradebar31 = new TradeBar(DateTime.UtcNow.AddSeconds(1.0), "SPY", 151.09m, 152.34m, 150.41m, 152.11m, (long)Math.Pow(1.51 * 10.0, 9.0));
            TradeBar tradebar32 = new TradeBar(DateTime.UtcNow.AddSeconds(1.0), "SPY", 151.76m, 152.92m, 151.52m, 152.92m, (long)Math.Pow(1.51 * 10.0, 9.0));
            TradeBar tradebar33 = new TradeBar(DateTime.UtcNow.AddSeconds(1.0), "SPY", 153.66m, 154.7m, 153.64m, 154.29m, (long)Math.Pow(1.74 * 10.0, 9.0));
            TradeBar tradebar34 = new TradeBar(DateTime.UtcNow.AddSeconds(1.0), "SPY", 154.84m, 154.92m, 154.16m, 154.5m, (long)Math.Pow(1.6 * 10.0, 9.0));
            TradeBar tradebar35 = new TradeBar(DateTime.UtcNow.AddSeconds(1.0), "SPY", 154.71m, 154.98m, 154.52m, 154.78m, (long)Math.Pow(1.63 * 10.0, 9.0));
            TradeBar tradebar36 = new TradeBar(DateTime.UtcNow.AddSeconds(1.0), "SPY", 155.46m, 155.65m, 154.66m, 155.44m, (long)Math.Pow(1.69 * 10.0, 9.0));
            TradeBar tradebar37 = new TradeBar(DateTime.UtcNow.AddSeconds(1.0), "SPY", 155.33m, 156.04m, 155.13m, 156.03m, (long)Math.Pow(1.69 * 10.0, 9.0));
            TradeBar tradebar38 = new TradeBar(DateTime.UtcNow.AddSeconds(1.0), "SPY", 155.92m, 156.1m, 155.21m, 155.68m, (long)Math.Pow(1.53 * 10.0, 9.0));
            TradeBar tradebar39 = new TradeBar(DateTime.UtcNow.AddSeconds(1.0), "SPY", 155.76m, 156.12m, 155.23m, 155.9m, (long)Math.Pow(1.53 * 10.0, 9.0));
            TradeBar tradebar40 = new TradeBar(DateTime.UtcNow.AddSeconds(1.0), "SPY", 156.31m, 156.8m, 156.22m, 156.73m, (long)Math.Pow(1.53 * 10.0, 9.0));
            TradeBar tradebar41 = new TradeBar(DateTime.UtcNow.AddSeconds(1.0), "SPY", 155.85m, 156.04m, 155.31m, 155.83m, (long)Math.Pow(1.53 * 10.0, 9.0));
            TradeBar tradebar42 = new TradeBar(DateTime.UtcNow.AddSeconds(1.0), "SPY", 154.34m, 155.64m, 154.2m, 154.97m, (long)Math.Pow(1.51 * 10.0, 9.0));
            TradeBar tradebar43 = new TradeBar(DateTime.UtcNow.AddSeconds(1.0), "SPY", 155.3m, 155.51m, 153.59m, 154.61m, (long)Math.Pow(1.51 * 10.0, 9.0));
            TradeBar tradebar44 = new TradeBar(DateTime.UtcNow.AddSeconds(1.0), "SPY", 155.52m, 155.95m, 155.26m, 155.69m, (long)Math.Pow(1.74 * 10.0, 9.0));
            TradeBar tradebar45 = new TradeBar(DateTime.UtcNow.AddSeconds(1.0), "SPY", 154.75m, 155.64m, 154.1m, 154.36m, (long)Math.Pow(1.6 * 10.0, 9.0));
            TradeBar tradebar46 = new TradeBar(DateTime.UtcNow.AddSeconds(1.0), "SPY", 154.85m, 155.6m, 154.73m, 155.6m, (long)Math.Pow(1.63 * 10.0, 9.0));
            TradeBar tradebar47 = new TradeBar(DateTime.UtcNow.AddSeconds(1.0), "SPY", 155.99m, 156.27m, 154.35m, 154.95m, (long)Math.Pow(1.69 * 10.0, 9.0));
            TradeBar tradebar48 = new TradeBar(DateTime.UtcNow.AddSeconds(1.0), "SPY", 155.49m, 156.23m, 155.42m, 156.19m, (long)Math.Pow(1.69 * 10.0, 9.0));
            TradeBar tradebar49 = new TradeBar(DateTime.UtcNow.AddSeconds(1.0), "SPY", 155.26m, 156.24m, 155m, 156.19m, (long)Math.Pow(1.53 * 10.0, 9.0));
            TradeBar tradebar50 = new TradeBar(DateTime.UtcNow.AddSeconds(1.0), "SPY", 156.09m, 156.85m, 155.75m, 156.67m, (long)Math.Pow(1.53 * 10.0, 9.0));

            //Date,                     Open,   High,   Low,    Close,  Parabolic SAR 0.02 0.20
            //4/1/2013 12:00:00 AM,     156.59, 156.91, 155.67, 156.05, 153.6552
            //4/2/2013 12:00:00 AM,     156.61, 157.21, 156.37, 156.82, 153.7854
            //4/3/2013 12:00:00 AM,     156.91, 157.03, 154.82, 155.23, 153.9909
            //4/4/2013 12:00:00 AM,     155.43, 156.17, 155.09, 155.86, 154.184
            //4/5/2013 12:00:00 AM,     153.95, 155.35, 153.77, 155.16, 157.21
            //4/8/2013 12:00:00 AM,     155.27, 156.22, 154.75, 156.21, 157.1412
            //4/9/2013 12:00:00 AM,     156.5,  157.32, 155.98, 156.75, 153.77
            //4/10/2013 12:00:00 AM,    157.17, 158.87, 157.13, 158.67, 153.841
            //4/11/2013 12:00:00 AM,    158.7,  159.71, 158.54, 159.19, 154.0422
            //4/12/2013 12:00:00 AM,    158.68, 159.04, 157.92, 158.8,  154.3822
            //4/15/2013 12:00:00 AM,    158,    158.13, 155.1,  155.12, 154.7019
            //4/16/2013 12:00:00 AM,    156.29, 157.49, 155.91, 157.41, 155.0024
            //4/17/2013 12:00:00 AM,    156.29, 156.32, 154.28, 155.11, 159.71
            //4/18/2013 12:00:00 AM,    155.37, 155.41, 153.55, 154.14, 159.6014
            //4/19/2013 12:00:00 AM,    154.5,  155.55, 154.12, 155.48, 159.3594
            //4/22/2013 12:00:00 AM,    155.78, 156.54, 154.75, 156.17, 159.127
            //4/23/2013 12:00:00 AM,    156.95, 157.93, 156.17, 157.78, 158.9039
            //4/24/2013 12:00:00 AM,    157.83, 158.3,  157.54, 157.88, 158.6897
            //4/25/2013 12:00:00 AM,    158.25, 159.27, 158.1,  158.52, 153.55
            //4/26/2013 12:00:00 AM,    158.32, 158.6,  157.73, 158.24, 153.6644
            //4/29/2013 12:00:00 AM,    158.67, 159.65, 158.42, 159.3,  153.7765
            //4/30/2013 12:00:00 AM,    159.27, 159.72, 158.61, 159.68, 154.0114

            TradeBar tradebar51 = new TradeBar(DateTime.UtcNow.AddSeconds(1.0), "SPY", 156.59m, 156.91m, 155.67m, 156.05m, (long)Math.Pow(1.51 * 10.0, 9.0));
            TradeBar tradebar52 = new TradeBar(DateTime.UtcNow.AddSeconds(1.0), "SPY", 156.61m, 157.21m, 156.37m, 156.82m, (long)Math.Pow(1.51 * 10.0, 9.0));
            TradeBar tradebar53 = new TradeBar(DateTime.UtcNow.AddSeconds(1.0), "SPY", 156.91m, 157.03m, 154.82m, 155.23m, (long)Math.Pow(1.74 * 10.0, 9.0));
            TradeBar tradebar54 = new TradeBar(DateTime.UtcNow.AddSeconds(1.0), "SPY", 155.43m, 156.17m, 155.09m, 155.86m, (long)Math.Pow(1.6 * 10.0, 9.0));
            TradeBar tradebar55 = new TradeBar(DateTime.UtcNow.AddSeconds(1.0), "SPY", 153.95m, 155.35m, 153.77m, 155.16m, (long)Math.Pow(1.63 * 10.0, 9.0));
            TradeBar tradebar56 = new TradeBar(DateTime.UtcNow.AddSeconds(1.0), "SPY", 155.27m, 156.22m, 154.75m, 156.21m, (long)Math.Pow(1.69 * 10.0, 9.0));
            TradeBar tradebar57 = new TradeBar(DateTime.UtcNow.AddSeconds(1.0), "SPY", 156.5m, 157.32m, 155.98m, 156.75m, (long)Math.Pow(1.69 * 10.0, 9.0));
            TradeBar tradebar58 = new TradeBar(DateTime.UtcNow.AddSeconds(1.0), "SPY", 157.17m, 158.87m, 157.13m, 158.67m, (long)Math.Pow(1.53 * 10.0, 9.0));
            TradeBar tradebar59 = new TradeBar(DateTime.UtcNow.AddSeconds(1.0), "SPY", 158.7m, 159.71m, 158.54m, 159.19m, (long)Math.Pow(1.53 * 10.0, 9.0));
            TradeBar tradebar60 = new TradeBar(DateTime.UtcNow.AddSeconds(1.0), "SPY", 158.68m, 159.04m, 157.92m, 158.8m, (long)Math.Pow(1.53 * 10.0, 9.0));
            TradeBar tradebar61 = new TradeBar(DateTime.UtcNow.AddSeconds(1.0), "SPY", 158m, 158.13m, 155.1m, 155.12m, (long)Math.Pow(1.53 * 10.0, 9.0));
            TradeBar tradebar62 = new TradeBar(DateTime.UtcNow.AddSeconds(1.0), "SPY", 156.29m, 157.49m, 155.91m, 157.41m, (long)Math.Pow(1.51 * 10.0, 9.0));
            TradeBar tradebar63 = new TradeBar(DateTime.UtcNow.AddSeconds(1.0), "SPY", 156.29m, 156.32m, 154.28m, 155.11m, (long)Math.Pow(1.51 * 10.0, 9.0));
            TradeBar tradebar64 = new TradeBar(DateTime.UtcNow.AddSeconds(1.0), "SPY", 155.37m, 155.41m, 153.55m, 154.14m, (long)Math.Pow(1.74 * 10.0, 9.0));
            TradeBar tradebar65 = new TradeBar(DateTime.UtcNow.AddSeconds(1.0), "SPY", 154.5m, 155.55m, 154.12m, 155.48m, (long)Math.Pow(1.6 * 10.0, 9.0));
            TradeBar tradebar66 = new TradeBar(DateTime.UtcNow.AddSeconds(1.0), "SPY", 155.78m, 156.54m, 154.75m, 156.17m, (long)Math.Pow(1.63 * 10.0, 9.0));
            TradeBar tradebar67 = new TradeBar(DateTime.UtcNow.AddSeconds(1.0), "SPY", 156.95m, 157.93m, 156.17m, 157.78m, (long)Math.Pow(1.69 * 10.0, 9.0));
            TradeBar tradebar68 = new TradeBar(DateTime.UtcNow.AddSeconds(1.0), "SPY", 157.83m, 158.3m, 157.54m, 157.88m, (long)Math.Pow(1.69 * 10.0, 9.0));
            TradeBar tradebar69 = new TradeBar(DateTime.UtcNow.AddSeconds(1.0), "SPY", 158.25m, 159.27m, 158.1m, 158.52m, (long)Math.Pow(1.53 * 10.0, 9.0));
            TradeBar tradebar70 = new TradeBar(DateTime.UtcNow.AddSeconds(1.0), "SPY", 158.32m, 158.6m, 157.73m, 158.24m, (long)Math.Pow(1.53 * 10.0, 9.0));
            TradeBar tradebar71 = new TradeBar(DateTime.UtcNow.AddSeconds(1.0), "SPY", 158.67m, 159.65m, 158.42m, 159.3m, (long)Math.Pow(1.69 * 10.0, 9.0));
            TradeBar tradebar72 = new TradeBar(DateTime.UtcNow.AddSeconds(1.0), "SPY", 159.27m, 159.72m, 158.61m, 159.68m, (long)Math.Pow(1.53 * 10.0, 9.0));

            //Date,                     Open,   High,   Low,    Close,  Parabolic SAR 0.02 0.20
            //5/1/2013 12:00:00 AM,     159.33, 159.41, 158.1,  158.28, 154.354
            //5/2/2013 12:00:00 AM,     158.68, 159.89, 158.53, 159.75, 154.6759
            //5/3/2013 12:00:00 AM,     161.14, 161.88, 161.04, 161.37, 155.093
            //5/6/2013 12:00:00 AM,     161.49, 162.01, 161.42, 161.78, 155.7717
            //5/7/2013 12:00:00 AM,     162.11, 162.65, 161.67, 162.6,  156.5203
            //5/8/2013 12:00:00 AM,     162.42, 163.39, 162.33, 163.34, 157.3785
            //5/9/2013 12:00:00 AM,     163.27, 163.7,  162.47, 162.88, 158.3403
            //5/10/2013 12:00:00 AM,    163,    163.55, 162.51, 163.41, 159.3051
            //5/13/2013 12:00:00 AM,    163,    163.81, 162.82, 163.54, 160.0962
            //5/14/2013 12:00:00 AM,    163.67, 165.35, 163.67, 165.23, 160.8389
            //5/15/2013 12:00:00 AM,    164.98, 166.45, 164.91, 166.12, 161.7411
            //5/16/2013 12:00:00 AM,    165.76, 166.36, 165.09, 165.34, 162.6829
            //5/17/2013 12:00:00 AM,    165.98, 167.04, 165.73, 166.94, 163.4363
            //5/20/2013 12:00:00 AM,    166.78, 167.58, 166.61, 166.93, 164.1571
            //5/21/2013 12:00:00 AM,    167.07, 167.8,  166.5,  167.17, 164.8417
            //5/22/2013 12:00:00 AM,    167.34, 169.07, 165.17, 165.93, 169.07
            //5/23/2013 12:00:00 AM,    164.2,  165.91, 163.94, 165.45, 169.07
            //5/24/2013 12:00:00 AM,    164.44, 165.38, 163.98, 165.31, 169.07
            //5/28/2013 12:00:00 AM,    167.04, 167.78, 165.81, 166.3,  168.8648
            //5/29/2013 12:00:00 AM,    165.41, 165.8,  164.34, 165.22, 168.6678
            //5/30/2013 12:00:00 AM,    165.35, 166.59, 165.22, 165.83, 168.4787
            //5/31/2013 12:00:00 AM,    165.36, 166.31, 163.13, 163.45, 168.2971

            TradeBar tradebar73 = new TradeBar(DateTime.UtcNow.AddSeconds(1.0), "SPY", 159.33m, 159.41m, 158.1m, 158.28m, (long)Math.Pow(1.74 * 10.0, 9.0));
            TradeBar tradebar74 = new TradeBar(DateTime.UtcNow.AddSeconds(1.0), "SPY", 158.68m, 159.89m, 158.53m, 159.75m, (long)Math.Pow(1.6 * 10.0, 9.0));
            TradeBar tradebar75 = new TradeBar(DateTime.UtcNow.AddSeconds(1.0), "SPY", 161.14m, 161.88m, 161.04m, 161.37m, (long)Math.Pow(1.63 * 10.0, 9.0));
            TradeBar tradebar76 = new TradeBar(DateTime.UtcNow.AddSeconds(1.0), "SPY", 161.49m, 162.01m, 161.42m, 161.78m, (long)Math.Pow(1.69 * 10.0, 9.0));
            TradeBar tradebar77 = new TradeBar(DateTime.UtcNow.AddSeconds(1.0), "SPY", 162.11m, 162.65m, 161.67m, 162.6m, (long)Math.Pow(1.69 * 10.0, 9.0));
            TradeBar tradebar78 = new TradeBar(DateTime.UtcNow.AddSeconds(1.0), "SPY", 162.42m, 163.39m, 162.33m, 163.34m, (long)Math.Pow(1.53 * 10.0, 9.0));
            TradeBar tradebar79 = new TradeBar(DateTime.UtcNow.AddSeconds(1.0), "SPY", 163.27m, 163.7m, 162.47m, 162.88m, (long)Math.Pow(1.53 * 10.0, 9.0));
            TradeBar tradebar80 = new TradeBar(DateTime.UtcNow.AddSeconds(1.0), "SPY", 163m, 163.55m, 162.51m, 163.41m, (long)Math.Pow(1.53 * 10.0, 9.0));
            TradeBar tradebar81 = new TradeBar(DateTime.UtcNow.AddSeconds(1.0), "SPY", 163m, 163.81m, 162.82m, 163.54m, (long)Math.Pow(1.53 * 10.0, 9.0));
            TradeBar tradebar82 = new TradeBar(DateTime.UtcNow.AddSeconds(1.0), "SPY", 153.67m, 165.35m, 163.67m, 165.23m, (long)Math.Pow(1.51 * 10.0, 9.0));
            TradeBar tradebar83 = new TradeBar(DateTime.UtcNow.AddSeconds(1.0), "SPY", 164.98m, 166.45m, 164.91m, 166.12m, (long)Math.Pow(1.51 * 10.0, 9.0));
            TradeBar tradebar84 = new TradeBar(DateTime.UtcNow.AddSeconds(1.0), "SPY", 165.76m, 166.36m, 165.09m, 165.34m, (long)Math.Pow(1.74 * 10.0, 9.0));
            TradeBar tradebar85 = new TradeBar(DateTime.UtcNow.AddSeconds(1.0), "SPY", 165.98m, 167.04m, 165.73m, m, (long)Math.Pow(1.6 * 10.0, 9.0));
            TradeBar tradebar86 = new TradeBar(DateTime.UtcNow.AddSeconds(1.0), "SPY", 166.78m, 167.58m, 166.61m, 156.17m, (long)Math.Pow(1.63 * 10.0, 9.0));
            TradeBar tradebar87 = new TradeBar(DateTime.UtcNow.AddSeconds(1.0), "SPY", 167.07m, 167.8m, 166.5m, 157.78m, (long)Math.Pow(1.69 * 10.0, 9.0));
            TradeBar tradebar88 = new TradeBar(DateTime.UtcNow.AddSeconds(1.0), "SPY", 167.34m, 169.07m, 165.17m, 157.88m, (long)Math.Pow(1.69 * 10.0, 9.0));
            TradeBar tradebar89 = new TradeBar(DateTime.UtcNow.AddSeconds(1.0), "SPY", 164.2m, 165.91m, 163.94m, 158.52m, (long)Math.Pow(1.53 * 10.0, 9.0));
            TradeBar tradebar90 = new TradeBar(DateTime.UtcNow.AddSeconds(1.0), "SPY", 164.44m, 165.38m, 163.98m, 158.24m, (long)Math.Pow(1.53 * 10.0, 9.0));
            TradeBar tradebar91 = new TradeBar(DateTime.UtcNow.AddSeconds(1.0), "SPY", 167.04m, 167.78m, 165.81m, 159.3m, (long)Math.Pow(1.69 * 10.0, 9.0));
            TradeBar tradebar92 = new TradeBar(DateTime.UtcNow.AddSeconds(1.0), "SPY", 165.41m, 165.8m, 164.34m, 159.68m, (long)Math.Pow(1.53 * 10.0, 9.0));
            TradeBar tradebar93 = new TradeBar(DateTime.UtcNow.AddSeconds(1.0), "SPY", 165.35m, 166.59m, 165.22m, 156.05m, (long)Math.Pow(1.51 * 10.0, 9.0));
            TradeBar tradebar94 = new TradeBar(DateTime.UtcNow.AddSeconds(1.0), "SPY", 165.36m, 166.31m, 163.13m, 156.82m, (long)Math.Pow(1.51 * 10.0, 9.0));
            
            int period = 1;
            var psar14 = new ParabolicStopAndReverse(period, .02m, .2m, true);
            psar14.Update(tradebar31);
            Assert.AreEqual(107.5m, psar14._psar);
            Assert.AreEqual(true, psar14._trend);

            psar14.Update(tradebar32);
            Assert.AreEqual(107.5m, psar14._psar);
            Assert.AreEqual(true, psar14._trend);

            psar14.Update(tradebar33);
            Assert.AreEqual(107.5m, psar14._psar);
            Assert.AreEqual(true, psar14._trend);

            psar14.Update(tradebar34);
            //107.96
            Assert.AreEqual(108.19m, psar14._psar);
            Assert.AreEqual(true, psar14._trend);

            // begin falling trend
            psar14.Update(tradebar35);
            Assert.AreEqual(119.23m, psar14._psar);
            Assert.AreEqual(false, psar14._trend);

            psar14.Update(tradebar36);
            Assert.AreEqual(false, psar14._trend);
            Assert.AreEqual(119.23m, psar14._psar);

            psar14.Update(tradebar37);
            Assert.AreEqual(118.9554m, psar14._psar);
            Assert.AreEqual(false, psar14._trend);

            psar14.Update(tradebar38);
            Assert.AreEqual(117.941984m, psar14._psar);
            Assert.AreEqual(false, psar14._trend);

            psar14.Update(tradebar39);
            Assert.AreEqual(false, psar14._trend);
            Assert.AreEqual(119.23m, psar14._psar);

            psar14.Update(tradebar40);
            Assert.AreEqual(118.9554m, psar14._psar);
            Assert.AreEqual(false, psar14._trend);

            psar14.Update(tradebar41);
            Assert.AreEqual(117.941984m, psar14._psar);
            Assert.AreEqual(false, psar14._trend);

            psar14.Update(tradebar42);
            Assert.AreEqual(119.23m, psar14._psar);
            Assert.AreEqual(false, psar14._trend);

            psar14.Update(tradebar43);
            Assert.AreEqual(false, psar14._trend);
            Assert.AreEqual(119.23m, psar14._psar);

            psar14.Update(tradebar44);
            Assert.AreEqual(118.9554m, psar14._psar);
            Assert.AreEqual(false, psar14._trend);

            psar14.Update(tradebar45);
            Assert.AreEqual(117.941984m, psar14._psar);
            Assert.AreEqual(false, psar14._trend);

            psar14.Update(tradebar46);
            Assert.AreEqual(false, psar14._trend);
            Assert.AreEqual(119.23m, psar14._psar);

            psar14.Update(tradebar47);
            Assert.AreEqual(118.9554m, psar14._psar);
            Assert.AreEqual(false, psar14._trend);

            psar14.Update(tradebar48);
            Assert.AreEqual(117.941984m, psar14._psar);
            Assert.AreEqual(false, psar14._trend);

            psar14.Update(tradebar49);
            Assert.AreEqual(118.9554m, psar14._psar);
            Assert.AreEqual(false, psar14._trend);

            psar14.Update(tradebar50);
            Assert.AreEqual(117.941984m, psar14._psar);
            Assert.AreEqual(false, psar14._trend);
        }

        [Test]
        public void ComparesAgainstExternalData()
        {
            var psar = new ParabolicStopAndReverse(1);
            double epsilon = 1e-3;
            TestHelper.TestIndicator(psar, "spy_parabolic_SAR.txt", "Parabolic SAR 0.02 0.20", (ind, expected) => Assert.AreEqual(expected, (double)ind.Current.Value, epsilon));
        }
    }
}
