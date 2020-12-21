using System;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using PemUtils;

namespace dotnetcoretest
{
    /*
    openssl genpkey -algorithm RSA -out private_key.pem
    openssl rsa -in private_key.pem -outform PEM -pubout -out public_key.pem
    */
    class Program
    {
        static void Main(string[] args)
        {
            using (RSA rsaPrivate = ReadPrivateKey(".\\keys\\private_key.pem"))
            {
                PrintRsaPrivateKey("private", rsaPrivate);
            }

            using (RSA rsaPublic = ReadPublicKey(".\\keys\\public_key.pem"))
            {
                PrintRsaPublicKey("public", rsaPublic);
            }
        }

        private static RSA ReadPrivateKey(string pemFile)
        {
            RSA rsaPrivate = RSA.Create();
            byte[] privateBytes = ReadKeyBytes(pemFile);
            rsaPrivate.ImportPkcs8PrivateKey(privateBytes, out int _);
            return rsaPrivate;
        }

        private static void PrintRsaPublicKey(string title, RSA rsa)
        {
            Console.WriteLine($"-----Public key-----{Environment.NewLine}{Convert.ToBase64String(rsa.ExportRSAPublicKey())}");
        }

        private static void PrintRsaPrivateKey(string title, RSA rsa)
        {
            Console.WriteLine($"-----Private key-----{Environment.NewLine}{Convert.ToBase64String(rsa.ExportRSAPrivateKey())}{Environment.NewLine}");
        }

        private static byte[] ReadKeyBytes(string pemFile)
            => Convert.FromBase64String(string.Join(Environment.NewLine, File.ReadAllLines(pemFile).Skip(1).SkipLast(1)));

        private static RSA ReadPublicKey(string pemFile)
        {
            using (var stream = File.OpenRead(pemFile))
            {
                using (var reader = new PemReader(stream))
                {
                    var rsaParameters = reader.ReadRsaKey();
                    var rsa = RSA.Create();
                    rsa.ImportParameters(rsaParameters);
                    return rsa;
                }
            }
        }
    }
}
