using System;
using System.Linq;
using System.Security.Cryptography;
using Microsoft.Extensions.Options;

namespace PlantillaAlmacen.Security
{
    public interface IPasswordHasherService
    {
        /// <summary>Genera un hash con formato versionado: v1$iter$saltB64$hashB64</summary>
        string Hash(string password, int? iterations = null);

        /// <summary>Verifica un password plano contra un hash versionado.</summary>
        bool Verify(string password, string hash);
    }

    public sealed class PasswordHasherService : IPasswordHasherService
    {
        private readonly string _pepper;
        private readonly int _defaultIterations;
        private readonly int _saltSize;
        private readonly int _hashSize;

        private const string Version = "v1"; // Si cambias de algoritmo/formato, sube versión (v2, ...)

        public PasswordHasherService(IOptions<SecurityOptions> opts)
        {
            var o = opts.Value ?? new SecurityOptions();
            _pepper = o.Pepper ?? string.Empty;
            _defaultIterations = o.DefaultIterations > 0 ? o.DefaultIterations : 210_000;
            _saltSize = o.SaltSize > 0 ? o.SaltSize : 16;
            _hashSize = o.HashSize > 0 ? o.HashSize : 32;
        }

        public string Hash(string password, int? iterations = null)
        {
            if (string.IsNullOrWhiteSpace(password))
                throw new ArgumentException("Password cannot be empty.", nameof(password));

            // Mezcla password + pepper (pepper sólo en servidor; no se guarda en BD)
            var pwdBytes = System.Text.Encoding.UTF8.GetBytes(password + _pepper);

            // Salt aleatorio
            byte[] salt = RandomNumberGenerator.GetBytes(_saltSize);

            int iter = iterations ?? _defaultIterations;
            // PBKDF2-HMACSHA512
            byte[] derived;
            using (var pbkdf2 = new Rfc2898DeriveBytes(pwdBytes, salt, iter, HashAlgorithmName.SHA512))
            {
                derived = pbkdf2.GetBytes(_hashSize);
            }

            string saltB64 = Convert.ToBase64String(salt);
            string hashB64 = Convert.ToBase64String(derived);

            // Formato versionado: v1$iter$salt$hash
            return $"{Version}${iter}${saltB64}${hashB64}";
        }

        public bool Verify(string password, string hash)
        {
            if (string.IsNullOrWhiteSpace(password) || string.IsNullOrWhiteSpace(hash))
                return false;

            // Esperamos: v1$iter$saltB64$hashB64
            var parts = hash.Split('$');
            if (parts.Length != 4) return false;

            var version = parts[0];
            if (!string.Equals(version, Version, StringComparison.Ordinal))
            {
                // Aquí podrías soportar otras versiones (v2, v3...) si migras de algoritmo
                return false;
            }

            if (!int.TryParse(parts[1], out int iter) || iter <= 0) return false;

            byte[] salt;
            byte[] expected;
            try
            {
                salt = Convert.FromBase64String(parts[2]);
                expected = Convert.FromBase64String(parts[3]);
            }
            catch
            {
                return false;
            }

            var pwdBytes = System.Text.Encoding.UTF8.GetBytes(password + _pepper);
            byte[] computed;
            using (var pbkdf2 = new Rfc2898DeriveBytes(pwdBytes, salt, iter, HashAlgorithmName.SHA512))
            {
                computed = pbkdf2.GetBytes(expected.Length);
            }

            return CryptographicOperations.FixedTimeEquals(computed, expected);
        }
    }
}
