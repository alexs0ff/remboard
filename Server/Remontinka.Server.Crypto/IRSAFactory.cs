using System.Security;
using System.Text;

namespace Remontinka.Server.Crypto
{
    public interface IRSAFactory
    {
        IRSAManager CreateManager( string pemData, SecureString password );

        IRSAManager CreateManager( string pemData, SecureString password,Encoding dataEncoding );

        IRSAManager CreateManagerFromFile( string keyFile, SecureString password );
    }
}
