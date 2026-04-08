# Dice Web3

A provably fair dice gambling game built with Unity, integrated with blockchain payments (Ethereum, BSC, Polygon) and PlayFab as the backend.

## Demo

https://github.com/dawideqqpl/dice-web3/releases/download/v1.0-demo/web3-dice.mp4

## Features

- **Manual and auto-bet** modes with configurable strategies (stop on profit/loss, increase on win/loss)
- **10 supported cryptocurrencies**: ETH, BNB, MATIC, SHIB, LINK, USDT, USDC, FDUSD, CRO, APE
- **Multi-chain support**: Ethereum, Binance Smart Chain, Polygon
- **PlayFab backend**: user authentication (email + Google OAuth), cloud-stored balances, referral system
- **Real-time crypto prices** from CoinGecko API — automatic minimum bet calculation
- **On-chain withdrawals** via Nethereum (ERC-20 and native token transfers)
- **Deposit QR codes** generated on-device with ZXing.Net
- **Encrypted local storage** using OPS AntiCheat ProtectedPlayerPrefs

## Tech Stack

| Layer | Technology |
|---|---|
| Game engine | Unity (C#) |
| Backend / auth | PlayFab |
| Blockchain | Nethereum, Web3 |
| Price feed | CoinGecko public API |
| Email | Elastic Email API |
| Payments | BitPay (invoice API) |
| Encryption | AES-256 (BouncyCastle) |
| QR codes | ZXing.Net |

## Getting Started

### 1. Clone the repository

```bash
git clone https://github.com/YOUR_USERNAME/dice-web3.git
cd dice-web3
```

### 2. Configure secrets

Copy the secrets template and fill in your own API keys:

```bash
cp Assets/Scripts/Config/Secrets.example.cs Assets/Scripts/Config/Secrets.cs
```

Open `Secrets.cs` and replace every `YOUR_*` placeholder:

| Key | Where to get it |
|---|---|
| `InfuraApiKey` | https://infura.io — create a project |
| `ElasticEmailApiKey` | https://elasticemail.com — API keys section |
| `AesEncryptionKey` | Generate: `Convert.ToBase64String(Aes.Create().Key)` |
| `EthPrivateKey` | Your hot-wallet private key (see security note below) |
| `GoogleOAuthClientId` | https://console.cloud.google.com — OAuth 2.0 credentials |
| `BitPayApiKey` | https://bitpay.com — merchant API keys |

> **Security note — ETH private key**: Having a private key in client-side code is an architectural debt.
> The correct production approach is to move transaction signing to a server-side service
> (e.g. PlayFab CloudScript + Azure Function). This is tracked as a future improvement.

### 3. Open in Unity

Open the project in **Unity 2022.3 LTS** or newer. All packages are managed via the Package Manager.

### 4. PlayFab setup

Set your PlayFab Title ID in `Assets/Resources/playfab.json` (or via the PlayFab Unity plugin settings).

## Project Structure

```
Assets/Scripts/
├── Config/           # Secrets, enums, constants (Secrets.cs is gitignored)
├── Auth/             # PlayFabAccountManager, PlayFabGoogleLogin
├── Betting/          # AutoBetManager, BettingManager, BetResultPopup, DiceSimulation
├── Blockchain/       # WalletManager, KeyGenerator, QRCodeGenerator, BitPayTransaction
├── Crypto/           # CryptoPriceReader, CryptoDepositHistory, CryptoWithdrawHistory
├── UI/               # UIManager, DepositScript, WithdrawScript, PopupError
└── Data/             # StatisticsManager, RecentBets, RecentGames
```

## Architecture Notes

- **PlayFabManager** acts as the central game state manager — balances, bets, encryption, referrals.
- **WalletManager** handles all on-chain interactions via Nethereum (ERC-20 transfers, gas estimation, balance checks).
- Crypto prices are fetched from CoinGecko and cached per session to avoid redundant API calls.
- All sensitive local data is stored via OPS AntiCheat `ProtectedPlayerPrefs` (obfuscated storage).
