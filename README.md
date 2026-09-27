# TradeGuard Trailing Drawdown Floor & Account Shield

Dynamic intraday high-water mark trailing drawdown overlay for NinjaTrader 8 (`.cs`) and MetaTrader 5 (`.mq5`). 

Built for evaluation traders (FundedNext, Topstep, Apex, Hola Prime) to visually track peak equity pullbacks and protect trailing loss buffers in real time.

---

### Master Calculation Matrix

To calculate your allowable contracts and lot sizing backward against active trailing floors before entering a trade, download the complete 1-page printable reference sheet:

👉 **[Download the Trailing Drawdown Defense Matrix (PDF)](https://tradeguardsystems.com)**

---

### Key Capabilities

Most prop evaluations fail because intraday unrealized profit wicks ratchet the trailing floor upward tick-by-tick. When the market retraces, the allowable risk buffer has already shrunk.

- **NinjaTrader 8:** Plots the dynamic Crimson Floor line and Blue High-Water Mark directly on futures charts (`TradeGuardTrailingFloor.cs`).
- **MetaTrader 5:** Renders real-time pip and dollar drawdown thresholds on FX and index charts (`TradeGuardTrailingFloor.mq5`).

---

### Installation

#### NinjaTrader 8:
1. Download `TradeGuardTrailingFloor.cs`.
2. Open NinjaTrader 8 > Tools > NinjaScript Editor.
3. Import or paste the script into the `Indicators` directory and press `F5` to compile.

#### MetaTrader 5:
1. Download `TradeGuardTrailingFloor.mq5`.
2. Open MT5 > File > Open Data Folder > `MQL5` > `Indicators`.
3. Paste the file, restart or refresh the Navigator panel in MT5, and drag it onto your chart.

---

### License & Support

Free and open-source utility provided by **TradeGuard Systems** (`tradeguardsystems.com`).  
Direct inquiries: `director@tradeguardsystems.com`
