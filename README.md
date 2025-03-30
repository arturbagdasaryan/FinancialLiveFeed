# Financial Live Feed Service

This project offers REST API and WebSocket endpoints for real-time financial instrument pricing, utilizing data from Binance. It is designed to handle over 1,000 concurrent subscribers with high efficiency.

## Prerequisites

- [.NET 8.0 SDK](https://dotnet.microsoft.com/download/dotnet/8.0)
- A code editor (e.g., [Visual Studio Code](https://code.visualstudio.com/))

## Features

- **REST API**: Fetch the list of available instruments and get the latest price for a specific instrument.
- **WebSocket**: Real-time price updates via WebSocket subscription to multiple instruments.
- **Data Source**: Live data from Binance's WebSocket API.
- **Scalable**: Designed to efficiently handle 1,000+ concurrent WebSocket subscribers.

## Technology Stack

- **Backend**: .NET Core, WebSocket for real-time data
- **External API**: Binance WebSocket API for instrument prices
## Setup and Run

1. **Clone the repository:**

   ```sh
   git clone https://github.com/arturbagdasaryan/FinancialLiveFeed.git
   cd FinancialLiveFeed
   ```

2. **Restore the dependencies:**

   ```sh
   dotnet restore
   ```

3. **Build the project:**

   ```sh
   dotnet build
   ```

4. **Run the application:**

   ```sh
   dotnet run
   ```

## Endpoints

### REST API

- **Get List of Instruments:**

  `GET /api/prices/instruments`

  Returns the list of available financial instruments.

  Example response:

  ```json
  ["BTCUSD", "ETHUSD", "EURUSD"]
  ```

- **Get Current Price:**

  `GET /api/prices/currentprice/{instrument}`

  Returns the current price of the specified financial instrument.

  Example response:

  ```json
  {
  	"Instrument": "BTCUSD",
  	"Price": 66315.15
  }
  ```

## WebSocket
- **Subscribe to Live Price Updates:**

Connect to `ws://localhost:5273/ws` using a WebSocket client to receive live updates for BTCUSD from Binance.

## Troubleshooting
If you run into any issues, consider the following:
- Confirm that the .NET 8.0 SDK is installed and properly configured on your system.
- Verify that your internet connection is stable for connecting to the Binance WebSocket.
