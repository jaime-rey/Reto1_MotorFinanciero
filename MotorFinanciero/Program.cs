using System;
using System.Globalization;
using System.Text;

namespace MotorFinanciero;

/// <summary>
/// Reto 1 - Algoritmos y Lógica de Control: El Motor de la Aplicación.
/// Aplicación de consola que:
///   1) Calcula el IVA de una operación.
///   2) Valida los datos de un usuario.
///   3) Genera la tabla de amortización de un préstamo.
/// </summary>
public static class Program
{
    /// <summary>
    /// Punto de entrada de la aplicación.
    /// </summary>
    public static void Main()
    {
        // UTF-8 para que el símbolo € y las tildes se vean bien en la consola de Windows
        Console.OutputEncoding = Encoding.UTF8;
        // Formato español en los números: 1.234,56
        CultureInfo.CurrentCulture = new CultureInfo("es-ES");

        Console.WriteLine("╔══════════════════════════════════════════════════════╗");
        Console.WriteLine("║   MOTOR FINANCIERO - Reto 1 - PROGRAMACIÓN - 1 DAM   ║");
        Console.WriteLine("╚══════════════════════════════════════════════════════╝");

        var salir = false;

        try
        {
            // Bucle principal: se repite hasta que el usuario elige "Salir"
            while (!salir)
            {
                MostrarMenu();
                var opcion = EntradaDatos.LeerEntero("Elige una opción: ", 0, 4);

                switch (opcion)
                {
                    case 1:
                        CalculadoraImpuestos.Ejecutar();
                        break;
                    case 2:
                        ValidadorUsuario.Ejecutar();
                        break;
                    case 3:
                        CalculadoraPrestamo.Ejecutar();
                        break;
                    case 4:
                        CalculadoraImpuestos.CambiarIvaProducto();
                        break;
                    case 0:
                        salir = true;
                        break;
                }
            }
        }
        catch (EntradaCerradaException)
        {
            // El usuario ha cerrado la entrada (Ctrl+Z / Ctrl+D): salimos sin error.
            Console.WriteLine();
        }
        catch (Exception ex)
        {
            // Red de seguridad: cualquier error imprevisto se muestra en lugar de romper el programa.
            EntradaDatos.MostrarError("Error inesperado: " + ex.Message);
        }

        Console.WriteLine("Gracias por usar el Motor Financiero. ¡Hasta pronto!");
    }

    private static void MostrarMenu()
    {
        Console.WriteLine();
        Console.WriteLine("---------------- MENÚ ----------------");
        Console.WriteLine("  1) Calcular IVA de una operación");
        Console.WriteLine("  2) Validar datos de un usuario");
        Console.WriteLine("  3) Simular préstamo (tabla de amortización)");
        Console.WriteLine("  4) Cambiar IVA de un producto");
        Console.WriteLine("  0) Salir");
        Console.WriteLine("--------------------------------------");
    }
}
