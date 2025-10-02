namespace PlantillaAlmacen.Security
{
    public class SecurityOptions
    {
        public string Pepper { get; set; } = "";
        /// <summary>
        /// Iteraciones por defecto para PBKDF2 (puedes subirlo con el tiempo).
        /// </summary>
        public int DefaultIterations { get; set; } = 210_000;
        /// <summary>
        /// Tamaño del salt en bytes.
        /// </summary>
        public int SaltSize { get; set; } = 16;
        /// <summary>
        /// Tamaño del hash en bytes.
        /// </summary>
        public int HashSize { get; set; } = 32;
    }
}
