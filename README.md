# TradeGuard Trailing Drawdown Floor & Account Shield

**TradeGuard Trailing Floor is a free, open-source (MIT) chart overlay for NinjaTrader 8, MetaTrader 5 and TradingView that plots a trailing drawdown floor under the price high-water mark, so prop-firm evaluation traders can see how much room is left before the trailing limit.**

Made by **TradeGuard Systems** ([tradeguardsystems.com](https://tradeguardsystems.com)). Not affiliated with other products named "TradeGuard".

| Platform | File | Language |
|---|---|---|
| NinjaTrader 8 | [`TradeGuardTrailingFloor.cs`](TradeGuardTrailingFloor.cs) | NinjaScript (C#) |
| MetaTrader 5 | [`TradeGuardTrailingFloor.mq5`](TradeGuardTrailingFloor.mq5) | MQL5 |
| TradingView | [`TradeGuardTrailingFloor.pine`](TradeGuardTrailingFloor.pine) | Pine Script v5 |

---

## What it plots

- **High-water mark (blue):** the highest price reached on the chart since the indicator started (or since the session began, on TradingView with daily reset on).
- **Trailing floor (red):** the high-water mark minus your drawdown allowance, converted from dollars into price. It only moves up, never down.
- **Breach shading and alert (TradingView):** the background turns red while price closes below the floor, and a "Price crossed below TradeGuard floor" alert condition is available. On non-futures charts a reminder asks you to set the dollar value per point for that symbol.

### What it does *not* do

The indicator is **price-based**. It follows the chart's high, not your broker account's equity, fills or realized P&L. It matches your account's trailing drawdown only when you hold a single **long** position of the configured size from the start of the tracking period. For short positions, multiple entries or account-level tracking, use your platform's or prop firm's account metrics.

---

## How the floor is calculated

```
price buffer    = drawdown ($) / (dollar value per point × contracts)
high-water mark = highest high so far
floor           = max(previous floor, high-water mark − price buffer)
```

**Worked example (NQ, 1 contract, $2,000 trailing drawdown):**
NQ is worth $20 per point, so the buffer is 2,000 / (20 × 1) = **100 points**. If NQ peaks at 20,150, the floor is 20,050. With 2 contracts the buffer halves to 50 points.

### Sizing against the remaining buffer

```
max contracts = floor(remaining buffer ($) / (stop distance in points × dollar value per point))
```

| Remaining buffer | Instrument ($/pt) | Stop | Risk per contract | Max contracts |
|---|---|---|---|---|
| $1,500 | NQ ($20) | 20 pts | $400 | 3 |
| $1,500 | MNQ ($2) | 20 pts | $40 | 37 |
| $1,000 | ES ($50) | 8 pts | $400 | 2 |
| $1,000 | MES ($5) | 8 pts | $40 | 25 |

### Intraday vs. end-of-day trailing

Prop firms differ in *when* the floor trails. Some update it intraday, tick by tick and including unrealized profit. Others update it only at the end-of-day balance. Rules also change over time, so check your firm's current rules page. This indicator trails intraday on every new high, which is the stricter case.

---

## Installation

### NinjaTrader 8
1. Download `TradeGuardTrailingFloor.cs`.
2. Open NinjaTrader 8 > Tools > NinjaScript Editor.
3. Place the file in the `Indicators` folder and press `F5` to compile.
4. Add it to a chart and set **Trailing Drawdown ($)**, **Tick Value ($)** (NQ default: $5 per 0.25 tick) and **Contracts**.

### MetaTrader 5
1. Download `TradeGuardTrailingFloor.mq5`.
2. Open MT5 > File > Open Data Folder > `MQL5` > `Indicators` and paste the file.
3. Open it in MetaEditor and compile (`F7`), or right-click Navigator > Refresh.
4. Attach it to a chart and set **Trailing Drawdown ($)** and **Position Size (lots)**. **Tick Value** defaults to 0, which reads the symbol's own tick value from the broker.

### TradingView (Pine Script v5)
1. Open `TradeGuardTrailingFloor.pine` and copy the script.
2. In TradingView, open **Pine Editor** (`Alt + E`).
3. Paste the code and click **Add to chart**.
4. Set **Dollar Value per 1.0 Price Point** for your instrument (NQ = 20, MNQ = 2, ES = 50, MES = 5) and your position size.

---

## License & support

Released under the [MIT License](LICENSE). Provided as is. It is a visual aid, not a guarantee against breaching any firm's rules.

Guides and resources: [tradeguardsystems.com](https://tradeguardsystems.com)
Contact: `director@tradeguardsystems.com`
