using System;
using System.Globalization;

namespace MotorFinanciero;

/// <summary>
/// Métodos para pedir datos al usuario por consola de forma segura.
/// Si el usuario escribe algo no válido (letras en un número, valores fuera
/// de rango...), se captura el error con try-catch y se vuelve a preguntar,
/// de modo que el programa nunca termina de forma inesperada (RA 2).
/// </summary>
public static class EntradaDatos
{
    /// <summary>
    /// Pide un número decimal comprendido entre minimo y maximo.
    /// Acepta tanto coma como punto como separador decimal (1500,50 o 1500.50).
    /// </summary>
    public static double LeerDouble(string mensaje, double minimo, double maximo)
    {
        while (true)
        {
            Console.Write(mensaje);
            string texto = LeerLinea();

            try
            {
                // Normalizamos la coma a punto y convertimos el texto a double.
                // double.Parse lanza FormatException si el texto no es un número.
                string normalizado = texto.Trim().Replace(',', '.');
                double valor = double.Parse(normalizado, NumberStyles.Float, CultureInfo.InvariantCulture);

                if (double.IsNaN(valor) || double.IsInfinity(valor))
                {
                    throw new FormatException();
                }

                if (valor < minimo || valor > maximo)
                {
                    MostrarError($"El valor debe estar entre {minimo:N2} y {maximo:N2}.");
                }
                else
                {
                    return valor;
                }
            }
            catch (FormatException)
            {
                MostrarError("Debes introducir un número (por ejemplo: 1500,50).");
            }
            catch (OverflowException)
            {
                MostrarError("El número es demasiado grande.");
            }
        }
    }

    /// <summary>
    /// Pide un número entero comprendido entre minimo y maximo.
    /// </summary>
    public static int LeerEntero(string mensaje, int minimo, int maximo)
    {
        while (true)
        {
            Console.Write(mensaje);
            string texto = LeerLinea();

            try
            {
                // int.Parse lanza FormatException si hay letras o decimales
                // y OverflowException si el número no cabe en un int.
                int valor = int.Parse(texto.Trim());

                if (valor < minimo || valor > maximo)
                {
                    MostrarError($"El valor debe estar entre {minimo} y {maximo}.");
                }
                else
                {
                    return valor;
                }
            }
            catch (FormatException)
            {
                MostrarError("Debes introducir un número entero, sin decimales ni letras.");
            }
            catch (OverflowException)
            {
                MostrarError("El número es demasiado grande.");
            }
        }
    }

    /// <summary>
    /// Pide un texto que no puede estar vacío.
    /// </summary>
    public static string LeerTexto(string mensaje)
    {
        while (true)
        {
            Console.Write(mensaje);
            string texto = LeerLinea().Trim();

            if (texto.Length > 0)
            {
                return texto;
            }
            MostrarError("El campo no puede estar vacío.");
        }
    }

    /// <summary>
    /// Pregunta algo de sí/no. Devuelve true si la respuesta es "s".
    /// </summary>
    public static bool LeerSiNo(string mensaje)
    {
        while (true)
        {
            Console.Write(mensaje + " (s/n): ");
            string respuesta = LeerLinea().Trim().ToLower();

            if (respuesta == "s" || respuesta == "si" || respuesta == "sí")
            {
                return true;
            }
            else if (respuesta == "n" || respuesta == "no")
            {
                return false;
            }
            MostrarError("Responde 's' o 'n'.");
        }
    }

    /// <summary>
    /// Muestra un mensaje de error en rojo y restaura el color original.
    /// </summary>
    public static void MostrarError(string mensaje)
    {
        ConsoleColor colorOriginal = Console.ForegroundColor;
        Console.ForegroundColor = ConsoleColor.Red;
        Console.WriteLine("  [ERROR] " + mensaje);
        Console.ForegroundColor = colorOriginal;
    }

    /// <summary>
    /// Muestra un mensaje de éxito en verde.
    /// </summary>
    public static void MostrarOk(string mensaje)
    {
        ConsoleColor colorOriginal = Console.ForegroundColor;
        Console.ForegroundColor = ConsoleColor.Green;
        Console.WriteLine("  [OK] " + mensaje);
        Console.ForegroundColor = colorOriginal;
    }

    /// <summary>
    /// Lee una línea de la consola. Si la entrada se cierra (Ctrl+Z / Ctrl+D),
    /// Console.ReadLine devuelve null: lo convertimos en una excepción controlada
    /// que el Main recoge para cerrar el programa de forma ordenada.
    /// </summary>
    private static string LeerLinea()
    {
        return Console.ReadLine() ?? throw new EntradaCerradaException();
    }
}

/// <summary>
/// Excepción propia que indica que ya no hay más entrada por teclado.
/// </summary>
public class EntradaCerradaException : Exception
{
    public EntradaCerradaException() : base("Se ha cerrado la entrada de datos.") { }

    public EntradaCerradaException(string? message) : base(message)
    {
    }

    public EntradaCerradaException(string? message, Exception? innerException) : base(message, innerException)
    {
    }

}
