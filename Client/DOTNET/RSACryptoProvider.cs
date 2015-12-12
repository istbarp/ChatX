using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Security.Cryptography;

namespace ConsoleApplication1
{
    class RSACryptoProvider
    {
        private RSACryptoServiceProvider Keys;
        private const string PATH = "C:/keys.xml";

        public RSACryptoProvider()
        {
            if (!(File.Exists(PATH)))
            {
                GenerateKeyPair();
                SaveKeys();
            }
            else
            {
                LoadKeys();
            }
        }

        private void GenerateKeyPair()
        {
            Keys = new RSACryptoServiceProvider(1024);
        }

        public byte[] Encrypt(string text)
	    {
		    return Keys.Encrypt(Encoding.ASCII.GetBytes(text), false);
	    }

        public String Decrypt(byte[] text)
	    {
		    return Encoding.ASCII.GetString(Keys.Decrypt(text, false));
	    }

        private void SaveKeys()
	    {
            File.WriteAllText(PATH, Keys.ToXmlString(true));
	    }
	
	    private void LoadKeys()
	    {
            Keys = new RSACryptoServiceProvider();
            Keys.FromXmlString(File.ReadAllText(PATH));
	    }
    }
}
