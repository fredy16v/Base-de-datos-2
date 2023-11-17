using Microsoft.AspNetCore.Mvc.Filters;

namespace WebApiAutores.Filters;

public static class FiltroLenguajeOfensivo
{
    private static List<string> PalabrasOfensivas = new List<string>
    {
        "puta",
        "mierda",
    };

    public static bool ContienePalabraOfensiva(string texto)
    {
        var textoEnMinusculas = texto.ToLower();
        return PalabrasOfensivas.Any(palabra => textoEnMinusculas.Contains(palabra));
    }
}
