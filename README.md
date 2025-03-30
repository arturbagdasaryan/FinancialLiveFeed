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

- **Backend**: .NET Core, SignalR, WebSocket for real-time data
- **Frontend**: SignalR client for receiving updates
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

## Project Structure
- **Program.cs:** Main entry point of the application, configures services, and sets up endpoints.
- **WebSocketConnectionManager.cs:** Manages WebSocket connections and broadcasting messages.
- **PriceStore.cs:** Stores and updates the latest prices.
- **Controllers/PricesController.cs:** Handles REST API requests for financial instrument prices.
- **Services/BinanceWebSocketService.cs:** Background service that connects to Binance WebSocket and updates prices.
- **Hubs/PriceHub.cs:** Defines the SignalR hub that clients can connect to for receiving real-time updates.
- **wwwroot/:** Contains static files for the project, including a sample client (index.html) to test SignalR connections.
## Logging
The application logs important events and errors to the console. Ensure you check the console output for real-time logging information.

## Notes
- The application subscribes to the Binance WebSocket stream for BTCUSD price updates. You can modify the code to subscribe to additional instruments if needed.
- The application is designed to handle over 1,000 WebSocket subscribers efficiently.

## Troubleshooting
If you encounter any issues, please check the following:
- Ensure you have .NET 8.0 SDK installed and properly set up on your machine.
- Check the console logs for any error messages.
- Ensure you have a stable internet connection to connect to Binance WebSocket.
