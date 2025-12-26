using System.Text;
using Azure.Identity;
using Azure.Security.KeyVault.Keys;
using Azure.Security.KeyVault.Keys.Cryptography;
using Azure.Security.KeyVault.Secrets;


namespace KeyVaule.Secret; // Optional, but good practice




public class KeyVault
{

    static string clientId = "";
    static string tenantId = "";
    static string clientSecret = "";
    static string keyVaultURI = "https://mobeen-az-vault.vault.azure.net/";

    public static void AccessKeyVaultKeys()
    {
        Console.WriteLine("Access Key Vault::");
        string keyName = "test-key-101";

        // We need to use the KeyClient to get a key first

        ClientSecretCredential clientSecretCredential = new ClientSecretCredential(tenantId, clientId, clientSecret);

        KeyClient keyClient = new KeyClient(new Uri(keyVaultURI), clientSecretCredential);

        var key = keyClient.GetKey(keyName);

        Console.WriteLine($"Got a handle to the key {key.Value.Name} , {key.Value.Properties.Version}");



        // Next we need to use the CryptographyClient class to perform cryptographic operations

        string toEncrypt = "This is a string we want to encrypt";
        var cryptoClient = keyClient.GetCryptographyClient(key.Value.Name, key.Value.Properties.Version);

        byte[] plainText = Encoding.UTF8.GetBytes(toEncrypt);

        EncryptResult encryptResult = cryptoClient.Encrypt(EncryptionAlgorithm.RsaOaep, plainText);

        Console.WriteLine($"Encrypted text {Convert.ToBase64String(encryptResult.Ciphertext)}");

        // If you want to decrypt the text

        byte[] ciperToBytes = encryptResult.Ciphertext;
        var decryptedText = cryptoClient.Decrypt(EncryptionAlgorithm.RsaOaep, encryptResult.Ciphertext);

        Console.WriteLine(Encoding.UTF8.GetString(decryptedText.Plaintext));



    }

    public static void AccessKeyVaultSecrets()
    {

        string secretName = "dbpass";

        ClientSecretCredential clientSecretCredential = new ClientSecretCredential(tenantId, clientId, clientSecret);

        SecretClient secretClient = new SecretClient(new Uri(keyVaultURI), clientSecretCredential);

        var secret = secretClient.GetSecret(secretName);

        Console.WriteLine($"Secret Value - {secret.Value.Value}");
    }
}


// void accessKeyVault()
// {


// }