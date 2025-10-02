using Microsoft.EntityFrameworkCore;

namespace PlantillaAlmacen
{
    public class Paginacion<T> : List<T>
    {
        public int IndicePagina { get; set; }
        public int TotalPaginas { get; set; }

        public Paginacion(List<T> elementos, int contador, int indicePagina, int tamanoPagina)
        {
            IndicePagina = indicePagina;
            TotalPaginas = (int)Math.Ceiling(contador / (double)tamanoPagina);
            this.AddRange(elementos);
        }

        public bool TienePaginaAnterior
        {
            get
            {
                return (IndicePagina > 1);
            }
        }

        public bool TienePaginaSiguiente
        {
            get
            {
                return (IndicePagina < TotalPaginas);
            }
        }

        public static async Task<Paginacion<T>> CreateAsync(IQueryable<T> origen, int indicePagina, int tamanoPagina)
        {
            var contador = await origen.CountAsync();

            var elementos = await origen.Skip((indicePagina - 1) * tamanoPagina).Take(tamanoPagina).ToListAsync();
            return new Paginacion<T>(elementos, contador, indicePagina, tamanoPagina);
        }
    }
}
