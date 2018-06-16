using System;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using System.Security.Cryptography;
using System.Text;
using System.Security;

namespace Remontinka.Server.Crypto
{
    public class RSAManager : MarshalByRefObject, IRSAManager
    {
        #region Constants

        private const string HashAlgorithmName = "SHA1";

        private const int KeyLen = 1024;

        private const int MaxRSABlockSize = KeyLen / 8 - 11 - 30 - 1;

        #endregion

        #region Static Fields

        private readonly static RSAFactory rsaFactory = new RSAFactory();

        #endregion

        #region Private Fields

        private RSACryptoServiceProvider rsaCryptoServiceProvider = null;

        private Encoding _encoding;

        #endregion

        #region Overrides

        public override object InitializeLifetimeService()
        {
            return null;
        }

        #endregion

        #region Constructors

        private RSAManager(RSACryptoServiceProvider rsaProvider, Encoding dataEncoding)
        {
            rsaCryptoServiceProvider = rsaProvider;
            _encoding = dataEncoding;
        }

        #endregion

        #region IRSAManager Members

        public void SignFile(string inFile, string signatureFile)
        {
            FileStream inFileStream = null;
            FileStream signatureFileStream = null;
            try
            {
                inFileStream = File.Open(inFile, FileMode.Open, FileAccess.Read);
                signatureFileStream = File.Open(signatureFile, FileMode.Create, FileAccess.Write);
                StreamReader reader = new StreamReader(inFileStream, _encoding);
                string pemData = reader.ReadToEnd();
                string sign = Sign(pemData);
                StreamWriter writer = new StreamWriter(signatureFileStream, _encoding);
                writer.Write(sign);
            }
            finally
            {
                if (inFileStream != null)
                {
                    inFileStream.Close();
                }
                if (signatureFileStream != null)
                {
                    signatureFileStream.Close();
                }
            }
        }

        public string Sign(string data)
        {
            string result = string.Empty;
            if (!HasPrivateKey)
                throw new RSAException("The private key is not availiable.");
            //создаем хеш
            HashAlgorithm ha = (HashAlgorithm)new SHA1CryptoServiceProvider();
            ha.Initialize();
            byte[] buf = _encoding.GetBytes(data);
            byte[] hash = ha.ComputeHash(buf);
            AsymmetricSignatureFormatter sf =
                (AsymmetricSignatureFormatter)new RSAPKCS1SignatureFormatter(rsaCryptoServiceProvider);
            sf.SetHashAlgorithm(HashAlgorithmName);
            //создаем подпись
            byte[] sign = sf.CreateSignature(ha.Hash);
            result = Convert.ToBase64String(sign);
            return result;
        }

        public bool VerifySign(string data, string sign)
        {
            HashAlgorithm ha = (HashAlgorithm)new SHA1CryptoServiceProvider();
            ha.Initialize();
            byte[] buf = Convert.FromBase64String(sign);
            byte[] hash = ha.ComputeHash(_encoding.GetBytes(data));
            AsymmetricSignatureDeformatter df =
                (AsymmetricSignatureDeformatter)new RSAPKCS1SignatureDeformatter(rsaCryptoServiceProvider);
            df.SetHashAlgorithm(HashAlgorithmName);
            return df.VerifySignature(hash, buf);
        }

        public void EncryptFile(string inFile, string ecryptedFile)
        {
            FileStream inputFileStream = null;
            FileStream outputFileStream = null;
            try
            {
                inputFileStream = new FileStream(inFile, FileMode.Open, FileAccess.Read);
                outputFileStream = new FileStream(ecryptedFile, FileMode.Create, FileAccess.Write);
                EncryptData(inputFileStream, outputFileStream);
            }
            finally
            {
                if (inputFileStream != null)
                {
                    inputFileStream.Close();
                }
                if (outputFileStream != null)
                {
                    outputFileStream.Close();
                }
            }
        }

        public string Encrypt(string data)
        {
            string result = string.Empty;
            MemoryStream inMemoryStream = new MemoryStream();
            MemoryStream outMemoryStream = new MemoryStream();
            try
            {
                BinaryWriter bw = new BinaryWriter(inMemoryStream, Encoding.UTF8);
                bw.Write(_encoding.GetBytes(data));
                inMemoryStream.Position = 0;
                EncryptData(inMemoryStream, outMemoryStream);
                byte[] buf = new byte[outMemoryStream.Length];
                outMemoryStream.Position = 0;
                outMemoryStream.Read(buf, 0, (int)outMemoryStream.Length);
                result = Convert.ToBase64String(buf);
            }
            finally
            {
                inMemoryStream.Close();
                outMemoryStream.Close();
            }
            return result;
        }

        public string Decrypt(string data)
        {
            string result = "";
            byte[] buf = Convert.FromBase64String(data);
            MemoryStream inputMemoryStream = new MemoryStream(buf);
            MemoryStream outputMemoryStream = new MemoryStream();
            try
            {
                DecryptData(inputMemoryStream, outputMemoryStream);
                outputMemoryStream.Position = 0;
                BinaryReader br = new BinaryReader(outputMemoryStream, Encoding.UTF8);
                result = _encoding.GetString(br.ReadBytes((int)br.BaseStream.Length));
            }
            finally
            {
                inputMemoryStream.Close();
                outputMemoryStream.Close();
            }
            return result;
        }

        public void DecryptFile(string encryptedFile, string decryptedFile)
        {
            FileStream inputFileStream = null;
            FileStream outputFileStream = null;
            try
            {
                inputFileStream = new FileStream(encryptedFile, FileMode.Open, FileAccess.Read);
                outputFileStream = new FileStream(decryptedFile, FileMode.Create, FileAccess.Write);
                DecryptData(inputFileStream, outputFileStream);
            }
            finally
            {
                if (inputFileStream != null)
                {
                    inputFileStream.Close();
                }
                if (outputFileStream != null)
                {
                    outputFileStream.Close();
                }
            }
        }

        #endregion

        #region Private Methods

        private void EncryptData(Stream sourceData, Stream encryptedData)
        {
            if (sourceData == null || encryptedData == null)
                return;
            BinaryFormatter bf = new BinaryFormatter();
            byte[] inbuf = new byte[MaxRSABlockSize];
            byte[] outbuf = new byte[MaxRSABlockSize];
            int len;
            BinaryWriter bw = new BinaryWriter(encryptedData, Encoding.UTF8);
            //записываем длину файла
            bw.Write((int)sourceData.Length);
            while ((len = sourceData.Read(inbuf, 0, MaxRSABlockSize)) == MaxRSABlockSize)
            {
                byte[] enc = rsaCryptoServiceProvider.Encrypt(inbuf, true); //шифруем
                encryptedData.Write(enc, 0, enc.Length);
            }
            outbuf = rsaCryptoServiceProvider.Encrypt(inbuf, true); //шифруем последний кусок
            encryptedData.Write(outbuf, 0, outbuf.Length);
        }

        private void DecryptData(Stream encryptedData, Stream decryptedData)
        {
            if (!HasPrivateKey)
                throw new RSAException("The private key is not availiable.");
            byte[] inbuf = new byte[KeyLen / 8];
            byte[] outbuf = new byte[MaxRSABlockSize];
            //Читаем длину файла
            BinaryReader br = new BinaryReader(encryptedData, Encoding.UTF8);
            long length = br.ReadInt32();
            byte[] dec;
            //long cl = encryptedData.Position;
            while (encryptedData.Position < encryptedData.Length - KeyLen / 8)
            {
                encryptedData.Read(inbuf, 0, KeyLen / 8);
                dec = rsaCryptoServiceProvider.Decrypt(inbuf, true);
                decryptedData.Write(dec, 0, dec.Length);
            }
            //Обрабатываем последний кусок
            encryptedData.Read(inbuf, 0, KeyLen / 8);
            dec = rsaCryptoServiceProvider.Decrypt(inbuf, true);
            decryptedData.Write(dec, 0, (int)length - (int)decryptedData.Position);
        }

        #endregion

        #region Public Propeties

        public static IRSAFactory RsaFactory
        {
            get { return rsaFactory; }
        }

        public bool HasPrivateKey
        {
            get { return !rsaCryptoServiceProvider.PublicOnly; }
        }

        public bool HasPublicKey
        {
            get { return rsaCryptoServiceProvider.PublicOnly; }
        }

        #endregion

        #region Nested Types

        private class RSAFactory : IRSAFactory
        {
            private static readonly Encoding DefaultEncoding = Encoding.UTF8;

            #region IRSAFactory Members

            public IRSAManager CreateManager(string pemData, SecureString password)
            {
                return CreateManager(pemData, password, DefaultEncoding);
            }

            public IRSAManager CreateManager(string pemData, SecureString password,Encoding dataEncoding)
            {
                RSACryptoServiceProvider provider = RSAHelper.CreateProviderFromKey(pemData, password);
                IRSAManager result = new RSAManager(provider,dataEncoding);
                return result;
            }

            public IRSAManager CreateManagerFromFile(string keyFile, SecureString password)
            {
                string pemData = string.Empty;
                FileStream stream = null;
                try
                {
                    stream = new FileStream(keyFile, FileMode.Open, FileAccess.Read);
                    StreamReader reader = new StreamReader(stream, Encoding.ASCII);
                    pemData = reader.ReadToEnd().Trim(new char[] { '\r', '\n' });
                }
                catch (Exception ex)
                {
                    throw new RSAException("Ошибка чтения файла." + Environment.NewLine + ex.Message);
                }
                finally
                {
                    if (stream != null)
                    {
                        stream.Close();
                    }
                }
                RSACryptoServiceProvider provider = RSAHelper.CreateProviderFromKey(pemData, password);
                if (provider == null)
                {
                    throw new RSAException("Error create RSAManager.");
                }
                IRSAManager result = new RSAManager(provider, DefaultEncoding);
                return result;
            }

            #endregion
        }

        #endregion

        /// <summary>
        /// Выполняет определяемые приложением задачи, связанные с удалением, высвобождением или сбросом неуправляемых ресурсов.
        /// </summary>
        /// <filterpriority>2</filterpriority>
        public void Dispose()
        {
            rsaCryptoServiceProvider.Clear();
        }
    }
}