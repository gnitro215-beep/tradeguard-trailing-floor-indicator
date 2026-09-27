#region Using declarations
using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Windows.Media;
using System.Xml.Serialization;
using NinjaTrader.Cbi;
using NinjaTrader.Gui;
using NinjaTrader.Gui.Chart;
using NinjaTrader.Data;
#endregion

namespace NinjaTrader.NinjaScript.Indicators
{
    public class TradeGuardTrailingFloor : Indicator
    {
        private double highWaterMark = double.MinValue;
        private double trailingFloor = 0;

        [NinjaScriptProperty]
        [Range(100, 10000)]
        [Display(Name = "Trailing Drawdown ($)", Description = "Max trailing drawdown dollar value", Order = 1, GroupName = "Parameters")]
        public double MaxTrailingDrawdown { get; set; }

        [NinjaScriptProperty]
        [Range(1, 100)]
        [Display(Name = "Tick Value ($)", Description = "Dollar value per full tick", Order = 2, GroupName = "Parameters")]
        public double TickValue { get; set; }

        protected override void OnStateChange()
        {
            if (State == State.SetDefaults)
            {
                Description = "Plots the dynamic trailing drawdown floor based on intra-trade peak equity.";
                Name = "TradeGuardTrailingFloor";
                Calculate = Calculate.OnPriceChange;
                IsOverlay = true;
                DisplayInDataBox = true;

                MaxTrailingDrawdown = 2000;
                TickValue = 5.0; // NQ tick value default ($5 per 0.25 pt / $20 per pt)

                AddPlot(Brushes.Crimson, "TrailingFloor");
                AddPlot(Brushes.RoyalBlue, "HighWaterMark");
            }
        }

        protected override void OnBarUpdate()
        {
            if (CurrentBar < 1)
            {
                highWaterMark = Close[0];
                trailingFloor = highWaterMark - (MaxTrailingDrawdown / TickValue * TickSize);
                Values[0][0] = trailingFloor;
                Values[1][0] = highWaterMark;
                return;
            }

            if (High[0] > highWaterMark)
            {
                highWaterMark = High[0];
            }

            double calculatedFloor = highWaterMark - (MaxTrailingDrawdown / TickValue * TickSize);
            if (calculatedFloor > trailingFloor)
            {
                trailingFloor = calculatedFloor;
            }

            Values[0][0] = trailingFloor;
            Values[1][0] = highWaterMark;
        }
    }
}
