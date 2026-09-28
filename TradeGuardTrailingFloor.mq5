//+------------------------------------------------------------------+
//|                                       TradeGuardTrailingFloor.mq5 |
//|                  TradeGuard Systems - https://tradeguardsystems.com |
//+------------------------------------------------------------------+
#property copyright   "Copyright 2026, Sovereign Holding Co."
#property link        "https://tradeguardsystems.com"
#property version     "1.00"
#property description "Price-based trailing drawdown floor: tracks the chart's high-water mark and plots the floor a fixed dollar distance below it."
#property indicator_chart_window
#property indicator_buffers 2
#property indicator_plots   2

#property indicator_label1  "TrailingFloor"
#property indicator_type1   DRAW_LINE
#property indicator_color1  clrCrimson
#property indicator_width1  2

#property indicator_label2  "HighWaterMark"
#property indicator_type2   DRAW_LINE
#property indicator_color2  clrRoyalBlue
#property indicator_width2  1

input double InpMaxTrailingDrawdown = 2000.0; // Trailing Drawdown ($)
input double InpLots                = 1.0;    // Position Size (lots)
input double InpTickValue           = 0.0;    // Tick Value per 1 lot ($), 0 = read from symbol

double FloorBuffer[];
double HwmBuffer[];

int OnInit()
  {
   if(InpMaxTrailingDrawdown <= 0 || InpLots <= 0 || InpTickValue < 0)
      return(INIT_PARAMETERS_INCORRECT);

   SetIndexBuffer(0, FloorBuffer, INDICATOR_DATA);
   SetIndexBuffer(1, HwmBuffer, INDICATOR_DATA);
   IndicatorSetString(INDICATOR_SHORTNAME, "TradeGuard Trailing Floor");
   IndicatorSetInteger(INDICATOR_DIGITS, _Digits);
   return(INIT_SUCCEEDED);
  }

// Converts the dollar drawdown into a price distance for the configured position size.
double BufferInPrice()
  {
   double tickSize  = SymbolInfoDouble(_Symbol, SYMBOL_TRADE_TICK_SIZE);
   double tickValue = InpTickValue > 0 ? InpTickValue : SymbolInfoDouble(_Symbol, SYMBOL_TRADE_TICK_VALUE);
   if(tickSize <= 0 || tickValue <= 0)
      return(0);
   return(InpMaxTrailingDrawdown / (tickValue * InpLots) * tickSize);
  }

int OnCalculate(const int rates_total,
                const int prev_calculated,
                const datetime &time[],
                const double &open[],
                const double &high[],
                const double &low[],
                const double &close[],
                const long &tick_volume[],
                const long &volume[],
                const int &spread[])
  {
   if(rates_total < 1)
      return(0);

   double buffer = BufferInPrice();
   if(buffer <= 0)
      return(0);

   if(prev_calculated == 0)
     {
      HwmBuffer[0]   = high[0];
      FloorBuffer[0] = high[0] - buffer;
     }

   int start = MathMax(prev_calculated - 1, 1);
   for(int i = start; i < rates_total; i++)
     {
      double hwm     = MathMax(HwmBuffer[i - 1], high[i]);
      HwmBuffer[i]   = hwm;
      FloorBuffer[i] = MathMax(FloorBuffer[i - 1], hwm - buffer);
     }

   return(rates_total);
  }
//+------------------------------------------------------------------+
