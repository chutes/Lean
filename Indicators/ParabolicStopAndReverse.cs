using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using QuantConnect.Data.Market;

namespace QuantConnect.Indicators
{
    /// <summary>
    /// 
    /// </summary>
    public class ParabolicStopAndReverse : WindowIndicator<TradeBar>
    {
        private TradeBar _previousInput;

        public IndicatorBase<TradeBar> _psarInitial { get; private set; }

        public IndicatorBase<TradeBar> _psar { get; private set; }
        public IndicatorBase<TradeBar> _extremePoint { get; private set; }
        public IndicatorBase<TradeBar> _psarEpAcc { get; private set; }

        public IndicatorBase<TradeBar> _previousPsar { get; private set; }

        private decimal highPoint2;
        private decimal lowPoint2;
        public IndicatorBase<TradeBar> _previousExtremePoint { get; private set; }
        public IndicatorBase<TradeBar> _acceleration { get; private set; }

        private readonly decimal _accelerationMax;
        private readonly decimal _accelerationMin;
        private static bool _previousTrend;
        public bool _trend;

        /// <summary>
        ///     Initializes a new instance of the LogReturn class with the specified name and period
        /// </summary>
        /// <param name="name">The name of this indicator</param>
        /// <param name="period">The period of the LOGR</param>
        public ParabolicStopAndReverse(string name, int period, decimal accelerationLow = 0.02m, decimal accelerationHigh = 0.2m, bool initialTrend = false)
            : base(name, period)
        {
            _accelerationMax = accelerationHigh;
            _accelerationMin = accelerationLow;
            _acceleration = new FunctionalIndicator<TradeBar>(name + "_acceleration",
                currentBar =>
                {
                    var value = ComputeAcceleration(currentBar);
                    return value;
                },
                isReady => _previousInput != null
                ); ;
            _trend = initialTrend;
            _previousTrend = initialTrend;

            // Psar Initial Indicator
            _psarInitial = new FunctionalIndicator<TradeBar>(name + "_psar_initial",
                currentBar =>
                {
                    var value = ComputePsarInitial(currentBar);
                    return value;
                },
                isReady => _previousInput != null
                );

            // Psar Indicator
            _psar = new FunctionalIndicator<TradeBar>(name + "_psar",
                currentBar =>
                {
                    var value = ComputePsar(currentBar);
                    return value;
                },
                isReady => _previousInput != null
                );

            // Extreme Point Indicator
            _extremePoint = new FunctionalIndicator<TradeBar>(name + "_extreme_point",
                currentBar =>
                {
                    var value = ComputeExtremePoint(currentBar);
                    return value;
                },
                isReady => _previousInput != null
                );

            // (Psar - Exteme Point) * Acceleration Indicator
            _psarEpAcc = new FunctionalIndicator<TradeBar>(name + "_psar_ep_acc",
                currentBar =>
                {
                    var value = ComputePsarEpAcc(currentBar);
                    return value;
                },
                isReady => _previousInput != null
                );

            // Previous Psar Indcator
            _previousPsar = new FunctionalIndicator<TradeBar>(name + "_psar_previous",
                currentBar =>
                {
                    var value = ComputePsarPrevious(currentBar);
                    return value;
                },
                isReady => _previousInput != null
                );

            _previousExtremePoint = new FunctionalIndicator<TradeBar>(name + "_previous_extreme_point",
                currentBar =>
                {
                    var value = ComputePreviousExtremePoint(currentBar);
                    return value;
                },
                isReady => _previousInput != null
                );
        }

        /// <summary>
        ///     Initializes a new instance of the LogReturn class with the default name and period
        /// </summary>
        /// <param name="period">The period of the SMA</param>
        public ParabolicStopAndReverse(int period, decimal accelerationLow = 0.02m, decimal accelerationHigh = 0.2m, bool initialTrend = false)
            : base("PSAR" + period, period)
        {
            string name = "PSAR";
            _accelerationMax = accelerationHigh;
            _accelerationMin = accelerationLow;
            _acceleration = new FunctionalIndicator<TradeBar>(name + "_acceleration",
                currentBar =>
                {
                    var value = ComputeAcceleration(currentBar);
                    return value;
                },
                isReady => _previousInput != null
                ); ;
            _trend = initialTrend;
            _previousTrend = initialTrend;

            // Psar Initial Indicator
            _psarInitial = new FunctionalIndicator<TradeBar>(name + "_psar_initial",
                currentBar =>
                {
                    var value = ComputePsarInitial(currentBar);
                    return value;
                },
                isReady => _previousInput != null
                );

            // Psar Indicator
            _psar = new FunctionalIndicator<TradeBar>(name + "_psar",
                currentBar =>
                {
                    var value = ComputePsar(currentBar);
                    return value;
                },
                isReady => _previousInput != null
                );

            // Extreme Point Indicator
            _extremePoint = new FunctionalIndicator<TradeBar>(name + "_extreme_point",
                currentBar =>
                {
                    var value = ComputeExtremePoint(currentBar);
                    return value;
                },
                isReady => _previousInput != null
                );

            // (Psar - Exteme Point) * Acceleration Indicator
            _psarEpAcc = new FunctionalIndicator<TradeBar>(name + "_psar_ep_acc",
                currentBar =>
                {
                    var value = ComputePsarEpAcc(currentBar);
                    return value;
                },
                isReady => _previousInput != null
                );

            // Previous Psar Indcator
            _previousPsar = new FunctionalIndicator<TradeBar>(name + "_psar_previous",
                currentBar =>
                {
                    var value = ComputePsarPrevious(currentBar);
                    return value;
                },
                isReady => _previousInput != null
                );

            _previousExtremePoint = new FunctionalIndicator<TradeBar>(name + "_previous_extreme_point",
                currentBar =>
                {
                    var value = ComputePreviousExtremePoint(currentBar);
                    return value;
                },
                isReady => _previousInput != null
                );
        }

        protected override decimal ComputeNextValue(IReadOnlyWindow<TradeBar> window, TradeBar input)
        {
            //////////////////////////////////////////////////////////////////////////////////////////
            if (this.Samples == 1)
            {
                _previousInput = input;
                _extremePoint.Update(input);
                _psar.Update(input);

                //trend is set
                _acceleration.Update(input);
                _psarEpAcc.Update(input);

                _psarInitial.Update(input);
                _previousExtremePoint.Update(input);

                // set psar - ep * acc
                decimal psar = ComputePsar(input);
                _previousPsar.Current.Value = psar;

                highPoint2 = input.High;
                lowPoint2 = input.Low;
                return psar;
            }
            //_psar.Current.

            // update initial psar
            _psarInitial.Update(input);

            // update psar
            _psar.Update(input);

            // set previous trend
            _previousTrend = _trend;

            // update trend
            _trend = _psar.Current.Value < _previousInput.Close;

            // update extreme point
            _extremePoint.Update(input);

            // update acceleration
            _acceleration.Update(input);

            // update (psar - ep) * ac
            _psarEpAcc.Update(input);

            // set highPoint2, lowPoint2, and previous input.
            highPoint2 = _previousInput.High;
            lowPoint2 = _previousInput.Low;
            _previousInput = new TradeBar(input);

            // set previous psar
            _previousPsar.Current.Value = _psar.Current.Value;

            // update previous extreme point
            _previousExtremePoint.Current.Value = _extremePoint.Current.Value;

            return _psar.Current.Value;
        }

        private decimal ComputeAcceleration(TradeBar input)
        {
            if (Samples == 1)
            {
                return _accelerationMin;
            }

            decimal acceleration = _acceleration.Current.Value;
            // acceleration can not be higher than the price
            if (_trend == true && _previousTrend == true)
            {
                // 
                if (_extremePoint.Current.Value > _previousExtremePoint.Current.Value && _acceleration < _accelerationMax)
                {
                    acceleration += _accelerationMin;
                }
                else if (_extremePoint.Current.Value == _previousExtremePoint.Current.Value)
                {
                }
                else
                {
                    acceleration = _accelerationMax;
                }
            }
            // when the trend changes, reset _acceleration to _accelerationMin
            else if (_trend == false && _previousTrend == false)
            {
                if (_extremePoint.Current.Value < _previousExtremePoint.Current.Value && _acceleration < _accelerationMax)
                {
                    acceleration += _accelerationMin;
                }
                else if (_extremePoint.Current.Value == _previousExtremePoint.Current.Value)
                {
                }    
                else
                {
                    acceleration = _accelerationMax;
                }
            }
            else if (_previousTrend != _trend)
            {
                acceleration = _accelerationMin;
            }

            //if (acceleration > _accelerationMax)
            //{
            //    acceleration = _accelerationMax;
            //}
            return acceleration;
        }

        private decimal ComputePsar(TradeBar input)
        {
            if (Samples == 1)
            {
                return !_trend ? input.High : input.Low;
            }

            if (!_trend)
            {
                if (input.High < _psarInitial.Current.Value)
                {
                    return _psarInitial.Current.Value;
                }
                else if (input.High >= _psarInitial.Current.Value)
                {
                    return _previousExtremePoint.Current.Value;
                }
            }
            else if (_trend)
            {
                if(_trend && input.Low > _psarInitial.Current.Value)
                {
                    return _psarInitial.Current.Value;
                }
                else if (_trend && input.Low <= _psarInitial.Current.Value)
                {
                    return _previousExtremePoint.Current.Value;
                }
            }

            ////IF(AND(L5=”falling”,C6<J6),J6,IF(AND(L5=”rising”,D6>J6),J6,IF(AND(L5=”falling”,C6>=J6),G5,IF(AND(L5=”rising”,D6<=J6),G5,””))))
            //// If the trend is falling and the high price is less than the initial psar
            //if (!_trend && input.High < _psarInitial.Current.Value)
            //{
            //    return _psarInitial.Current.Value;
            //}
            //// If the trend is falling and the high price is less than the initial psar
            //else if (_trend && input.Low > _psarInitial.Current.Value)
            //{
            //    return _psarInitial.Current.Value;
            //}
            //else if (!_trend && input.High >= _psarInitial.Current.Value)
            //{
            //    return _previousExtremePoint.Current.Value;
            //}
            //else if (_trend && input.Low <= _psarInitial.Current.Value)
            //{
            //    return _previousExtremePoint.Current.Value;
            //}

            return 0;
        }

        private decimal ComputePsarInitial(TradeBar input)
        {
            // If the previous trend is falling, the initial psar is the max value
            // of the previous psar - the previous psar - ep  * ac, or the 2 previous high prices
            // If the previous trend is rising, the initial psar is the min value
            // of the previous psar - the previous psar - ep * ac, or the 2 previous low prices
            // 
            //IF(L5=”falling”,MAX(K5-I5,C5,C4),IF(L5=”rising”,MIN(K5-I5,D5,D4),””))
            decimal value = 0m;
            if (Samples == 1)
            {
                return value;
            }
            if (!_trend)
            {
                value = Math.Max(_previousPsar.Current.Value - _psarEpAcc.Current.Value, _previousInput.High);
                if (Samples > 1)
                {
                    value = Math.Max(value, highPoint2);
                }
            }
            else if (_trend)
            {
                value = Math.Min(_previousPsar.Current.Value - _psarEpAcc.Current.Value, _previousInput.Low);
                if (Samples > 2)
                {
                    value = Math.Min(value, lowPoint2);
                }
            }

            return value;
        }
        
        private decimal ComputePsarEpAcc(TradeBar input)
        {
            decimal psarEpAcc = (_psar.Current.Value - _extremePoint.Current.Value) * _acceleration.Current.Value;
        	return psarEpAcc;
        }
        
        private decimal ComputePsarPrevious(TradeBar input)
        {
            if (Samples == 1)
            {
                return !_trend ? input.Low : input.High;
            }

            decimal psarPrev = _psar.Current.Value;
        	
        	return psarPrev;
        }

        private decimal ComputeExtremePoint(TradeBar input)
        {
            // If trending 
            decimal ep;
            if (Samples == 1)
            {
                ep = !_trend ? input.Low : input.High;
                return ep;
            }
            
            ep = _trend ? Math.Max(_previousExtremePoint.Current.Value, input.High)
                        : Math.Min(_previousExtremePoint.Current.Value, input.Low);

            return ep;
        }

        private decimal ComputePreviousExtremePoint(TradeBar input)
        {
            decimal pep;
            if (Samples == 1)
            {
                pep = !_trend ? input.Low : input.High;
                return pep;
            }

            pep = _extremePoint.Current.Value;

            return pep;
        }
    }
}
