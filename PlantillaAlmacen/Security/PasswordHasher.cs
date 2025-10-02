using System.Security.Cryptography;

namespace PlantillaAlmacen.Security
{
    public static class PasswordHasher
    {
        private const int SaltSize = 32;      
        private const int KeySize = 32;       
        private const int DefaultIterations = 150_000; 
        private const string Prefix = "v1";   

        // Opcional: "pepper" (secreto del servidor). Configúralo vía env var / appsettings.
        // Si no quieres pepper, deja string.Empty.
        private static readonly string Pepper = Environment.GetEnvironmentVariable("APP_PEPPER") ?? string.Empty;

        public static string Hash(string password, int? iterations = null)
        {
            if (string.IsNullOrWhiteSpace(password))
                throw new ArgumentException("Password no puede ser vacío.", nameof(password));

            using var rng = RandomNumberGenerator.Create();
            var salt = new byte[SaltSize];
            rng.GetBytes(salt);

            var iters = iterations ?? DefaultIterations;

            // PBKDF2 con HMAC-SHA256
            using var pbkdf2 = new Rfc2898DeriveBytes(Combine(password, Pepper), salt, iters, HashAlgorithmName.SHA256);
            var key = pbkdf2.GetBytes(KeySize);

            // Formato: v1$iteraciones$saltBase64$hashBase64
            return $"{Prefix}${iters}${Convert.ToBase64String(salt)}${Convert.ToBase64String(key)}";
        }

        public static bool Verify(string password, string hash)
        {
            if (string.IsNullOrWhiteSpace(hash)) return false;

            var parts = hash.Split('$', StringSplitOptions.RemoveEmptyEntries);
            if (parts.Length != 4 || parts[0] != Prefix) return false;

            if (!int.TryParse(parts[1], out var iterations)) return false;

            var salt = Convert.FromBase64String(parts[2]);
            var key = Convert.FromBase64String(parts[3]);

            using var pbkdf2 = new Rfc2898DeriveBytes(Combine(password, Pepper), salt, iterations, HashAlgorithmName.SHA256);
            var keyToCheck = pbkdf2.GetBytes(KeySize);

            return FixedTimeEquals(key, keyToCheck);
        }

        private static string Combine(string password, string pepper)
            => string.IsNullOrEmpty(pepper) ? password : password + pepper;

        // Comparación en tiempo constante para evitar ataques de timing
        private static bool FixedTimeEquals(ReadOnlySpan<byte> a, ReadOnlySpan<byte> b)
        {
            if (a.Length != b.Length) return false;

            var diff = 0;
            for (int i = 0; i < a.Length; i++)
                diff |= a[i] ^ b[i];

            return diff == 0;
        }
    }
}
