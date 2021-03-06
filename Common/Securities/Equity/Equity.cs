﻿/*
 * QUANTCONNECT.COM - Democratizing Finance, Empowering Individuals.
 * Lean Algorithmic Trading Engine v2.0. Copyright 2014 QuantConnect Corporation.
 * 
 * Licensed under the Apache License, Version 2.0 (the "License"); 
 * you may not use this file except in compliance with the License.
 * You may obtain a copy of the License at http://www.apache.org/licenses/LICENSE-2.0
 * 
 * Unless required by applicable law or agreed to in writing, software
 * distributed under the License is distributed on an "AS IS" BASIS,
 * WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
 * See the License for the specific language governing permissions and
 * limitations under the License.
*/

using QuantConnect.Data;

namespace QuantConnect.Securities.Equity 
{
    /// <summary>
    /// Equity Security Type : Extension of the underlying Security class for equity specific behaviours.
    /// </summary>
    /// <seealso cref="Security"/>
    public class Equity : Security
    {
        /// <summary>
        /// Construct the Equity Object
        /// </summary>
        public Equity(SubscriptionDataConfig config, decimal leverage, bool isDynamicallyLoadedData = false)
            : this(SecurityExchangeHoursProvider.FromDataFolder().GetExchangeHours(config), config, leverage, isDynamicallyLoadedData)
        {
            // this constructor is provided for backward compatibility

            // should we even keep this?
        }

        /// <summary>
        /// Construct the Equity Object
        /// </summary>
        public Equity(SecurityExchangeHours exchangeHours, SubscriptionDataConfig config, decimal leverage, bool isDynamicallyLoadedData = false) 
            : base(exchangeHours, config, leverage, isDynamicallyLoadedData) 
        {
            //Holdings for new Vehicle:
            Cache = new EquityCache();
            Exchange = new EquityExchange(exchangeHours);
            DataFilter = new EquityDataFilter();
            //Set the Equity Transaction Model
            TransactionModel = new EquityTransactionModel();
            PortfolioModel = new EquityPortfolioModel();
            MarginModel = new EquityMarginModel(leverage);
            Holdings = new EquityHolding(this, TransactionModel, MarginModel);
        }
    }
}
