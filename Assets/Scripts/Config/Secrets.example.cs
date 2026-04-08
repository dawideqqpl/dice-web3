// ============================================================
// SECRETS TEMPLATE
// Copy this file to Secrets.cs and fill in your own keys.
// Secrets.cs is listed in .gitignore and must not be committed.
// ============================================================
public static class Secrets
{
    // Infura RPC provider key — https://infura.io
    public const string InfuraApiKey = "YOUR_INFURA_API_KEY";

    // Elastic Email API key — https://elasticemail.com
    public const string ElasticEmailApiKey = "YOUR_ELASTIC_EMAIL_API_KEY";

    // AES-256 symmetric encryption key (Base64)
    // Generate with: Convert.ToBase64String(Aes.Create().Key)
    public const string AesEncryptionKey = "YOUR_AES_ENCRYPTION_KEY_BASE64";

    // Ethereum/Polygon hot-wallet private key used for withdrawals
    // WARNING: Private keys should be handled server-side in production.
    // See: PlayFab CloudScript + Azure Function signing service.
    public const string EthPrivateKey = "YOUR_ETH_PRIVATE_KEY";

    // Google OAuth 2.0 Client ID — https://console.cloud.google.com
    public const string GoogleOAuthClientId = "YOUR_GOOGLE_OAUTH_CLIENT_ID";

    // BitPay API key — https://bitpay.com
    public const string BitPayApiKey = "YOUR_BITPAY_API_KEY";
}
