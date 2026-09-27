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
        [Range(0.01, 1000)]
        [Display(Name = "Tick Value ($)", Description = "Dollar value of one tick for one contract", Order = 2, GroupName = "Parameters")]
        public double TickValue { get; set; }

        [NinjaScriptProperty]
        [Range(1, 1000)]
        [Display(Name = "Contracts", Description = "Position size the drawdown buffer is spread across", Order = 3, GroupName = "Parameters")]
        public int Contracts { get; set; }

        protected override void OnStateChange()
        {
            if (State == State.SetDefaults)
            {
                Description = "Price-based trailing drawdown floor: tracks the chart high-water mark and plots the floor a fixed dollar distance below it.";
                Name = "TradeGuardTrailingFloor";
                Calculate = Calculate.OnPriceChange;
                IsOverlay = true;
                DisplayInDataBox = true;

                MaxTrailingDrawdown = 2000;
                TickValue = 5.0; // NQ: $5 per 0.25-point tick ($20 per point)
                Contracts = 1;

                AddPlot(Brushes.Crimson, "TrailingFloor");
                AddPlot(Brushes.RoyalBlue, "HighWaterMark");
            }
        }

        protected override void OnBarUpdate()
        {
            if (CurrentBar < 1)
            {
                highWaterMark = Close[0];
                trailingFloor = highWaterMark - (MaxTrailingDrawdown / (TickValue * Contracts) * TickSize);
                Values[0][0] = trailingFloor;
                Values[1][0] = highWaterMark;
                return;
            }

            if (High[0] > highWaterMark)
            {
                highWaterMark = High[0];
            }

            double calculatedFloor = highWaterMark - (MaxTrailingDrawdown / (TickValue * Contracts) * TickSize);
            if (calculatedFloor > trailingFloor)
            {
                trailingFloor = calculatedFloor;
            }

            Values[0][0] = trailingFloor;
            Values[1][0] = highWaterMark;
        }
    }
}
