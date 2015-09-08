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
    public class ParabolicStopAndReverseComposite : WindowIndicator<TradeBar>
    {
        private TradeBar _previousInput;

        private IndicatorBase<IndicatorDataPoint> _psar { get; set; }
        private IndicatorBase<IndicatorDataPoint> _psarInitial { get; set; }
        private IndicatorBase<IndicatorDataPoint> _previousPsar { get; set; }
        private IndicatorBase<IndicatorDataPoint> _extremePoint { get; set; }
        private IndicatorBase<IndicatorDataPoint> _previousExtremePoint { get; set; }
        private IndicatorBase<IndicatorDataPoint> _psarEpAcc { get; set; }

        private readonly decimal _accelerationMax;
        private readonly decimal _accelerationMin;
        private decimal _acceleration;
        private static bool _previousTrend;
        private bool _trend;

        /// <summary>
        ///     Initializes a new instance of the LogReturn class with the specified name and period
        /// </summary>
        /// <param name="name">The name of this indicator</param>
        /// <param name="period">The period of the LOGR</param>
        public ParabolicStopAndReverseComposite(string name, int period, decimal accelerationLow = 0.02m, decimal accelerationHigh = 0.2m, bool initialTrend = false)
            : base(name, period)
        {
            _accelerationMax = accelerationHigh;
            _accelerationMin = accelerationLow;
            _acceleration = accelerationLow;
            _trend = initialTrend;
            _previousTrend = initialTrend;

            var left = new FunctionalIndicator<TradeBar>(name + "_psarPrevious",
                currentBar =>
                {
                    var value = UpdatePsar(currentBar);
                    return value;
                },
                isReady => _previousInput != null
                );

            var right = new FunctionalIndicator<TradeBar>(name + "_psar",
                currentBar =>
                {
                    var value = UpdatePsar(currentBar);
                    return value;
                },
                isReady => _previousInput != null
                );

            //var composite = new CompositeIndicator<IndicatorDataPoint>(left, right, (l, r) => l + r);
            //_psar = new CompositeIndicator<TradeBar>();
            //_extremePoint.Update(input.Time, input.High);
            //_previousExtremePoint.Update(input.Time, input.High);
        }

        private decimal UpdatePsar(TradeBar input)
        {
            return input.Close;
        }

        /// <summary>
        ///     Initializes a new instance of the LogReturn class with the default name and period
        /// </summary>
        /// <param name="period">The period of the SMA</param>
        public ParabolicStopAndReverseComposite(int period, decimal accelerationLow = 0.02m, decimal accelerationHigh = 0.2m, bool initialTrend = false)
            : base("LOGR" + period, period)
        {
            _accelerationMax = accelerationHigh;
            _accelerationMin = accelerationLow;
            _acceleration = accelerationLow;
            _trend = initialTrend;
            _previousTrend = initialTrend;
        }

        protected override decimal ComputeNextValue(IReadOnlyWindow<TradeBar> window, TradeBar input)
        {
            if (this.Samples == 1)
            {
                _previousInput = input;
                _psar.Update(input.Time, input.Low);
                _extremePoint.Update(input.Time, input.High);
                _previousExtremePoint.Update(input.Time, input.High);

                // set psar - ep * acc
                _psarEpAcc.Update(input.Time, (Current.Value - _extremePoint.Current.Value) * _acceleration);
                return CalculatePSAR(input, _previousPsar.Current.Value, _previousExtremePoint.Current.Value);
            }

            if (!_previousTrend)
            {
                decimal max = Math.Max(Current.Value - _psarEpAcc.Current.Value, input.High);
                max = Math.Max(max, _previousInput.High);
                _psarInitial.Update(input.Time, max);
            }
            else if (_previousTrend)
            {
                decimal min = Math.Min(Current.Value - _psarEpAcc.Current.Value, input.Low);
                min = Math.Min(min, _previousInput.Low);
                _psarInitial.Update(input.Time, min);
            }

            decimal psar = CalculatePSAR(input, _previousPsar.Current.Value, _previousExtremePoint.Current.Value);

            // set new trend
            _previousTrend = _trend;
            _trend = Current.Value > _previousPsar.Current.Value;

            // Set extreme point
            _previousExtremePoint = _extremePoint;
            _extremePoint.Update(input.Time,
                                 _trend
                                     ? Math.Max(_previousExtremePoint.Current.Value, input.High)
                                     : Math.Min(_previousExtremePoint.Current.Value, input.Low));

            // acceleration can not be higher than the price
            if (_trend == true && _previousTrend == true)
            {
                // 
                if (_extremePoint.Current.Value > _previousExtremePoint.Current.Value && _acceleration <= _accelerationMax)
                {
                    _acceleration += _accelerationMin;
                }
                else
                {
                    _acceleration = _accelerationMax;
                }
            }
            // when the trend changes, reset _acceleration to _accelerationMin
            else if (_trend == false && _previousTrend == false)
            {
                if (_extremePoint.Current.Value < _previousExtremePoint.Current.Value && _acceleration <= _accelerationMax)
                {
                    _acceleration += _accelerationMin;
                }
                else
                {
                    _acceleration = _accelerationMax;
                }
            }
            else if (_trend == true && _previousTrend == false)
            {
                _acceleration = _accelerationMin;
            }

            _previousInput = input;

            return psar;
        }

        private decimal CalculatePSAR(TradeBar input, decimal value, decimal previousEpValue)
        {
            if (!_previousTrend && input.High < value)
            {
                return value;
            }
            else if (_previousTrend && input.Close > value)
            {
                return value;
            }
            else if (!_previousTrend && input.High >= value)
            {
                return previousEpValue;
            }
            else if (_previousTrend && input.Low <= value)
            {
                return previousEpValue;
            }

            return 0;
        }
    }
}
