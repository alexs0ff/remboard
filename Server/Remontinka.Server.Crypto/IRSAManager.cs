using System;

namespace Remontinka.Server.Crypto
{
    public interface IRSAManager : IDisposable 
    {
        #region Methods

        void SignFile( string inFile, string signatureFile );

        string Sign( string data );

        bool VerifySign( string data, string sign );

        void EncryptFile(string inFile, string ecryptedFile);

        string Encrypt( string data );

        string Decrypt( string data );

        void DecryptFile( string encryptedFile, string decryptedFile );

        #endregion

        #region Propeties

        bool HasPrivateKey{ get;}
        bool HasPublicKey{get;}

        #endregion
    }
}
