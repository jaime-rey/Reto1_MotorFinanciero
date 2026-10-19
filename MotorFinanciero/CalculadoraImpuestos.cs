using System;

namespace MotorFinanciero;

/// <summary>
/// Opción 1 del menú: cálculo del IVA de una operación.
/// </summary>
public static class CalculadoraImpuestos
{
    public static void Ejecutar()
    {
        Console.WriteLine();
        Console.WriteLine("=== CÁLCULO DE IVA ===");

        string concepto = EntradaDatos.LeerTexto("Concepto de la operación: ");
        double precioUnitario = EntradaDatos.LeerDouble("Precio unitario sin IVA (€): ", 0.01, 1000000);
        int unidades = EntradaDatos.LeerEntero("Número de unidades: ", 1, 100000);

        Console.WriteLine("Tipo de IVA:");
        Console.WriteLine("  1) General       (21 %)");
        Console.WriteLine("  2) Reducido      (10 %)");
        Console.WriteLine("  3) Superreducido  (4 %)");
        Console.WriteLine("  4) Exento         (0 %)");
        int opcionIva = EntradaDatos.LeerEntero("Elige el tipo (1-4): ", 1, 4);

        // Elegimos el porcentaje según la opción con un switch (RA 3)
        double tipoIva;
        string nombreTipo;
        switch (opcionIva)
        {
            case 1:
                tipoIva = Constantes.IVA_GENERAL;
                nombreTipo = "General";
                break;
            case 2:
                tipoIva = Constantes.IVA_REDUCIDO;
                nombreTipo = "Reducido";
                break;
            case 3:
                tipoIva = Constantes.IVA_SUPERREDUCIDO;
                nombreTipo = "Superreducido";
                break;
            default:
                tipoIva = Constantes.IVA_EXENTO;
                nombreTipo = "Exento";
                break;
        }

        // Conversión implícita: el int "unidades" se convierte a double al multiplicar.
        double baseImponible = precioUnitario * unidades;
        double cuotaIva = CalcularIva(baseImponible, tipoIva);
        double total = baseImponible + cuotaIva;

        // Casting explícito (double -> int) para mostrar el porcentaje como número entero.
        int porcentajeEntero = (int)Math.Round(tipoIva * 100);

        string etiquetaIva = $"IVA {nombreTipo} ({porcentajeEntero} %):";

        Console.WriteLine();
        Console.WriteLine($"  {"Concepto:",-26}{concepto}");
        Console.WriteLine($"  {"Base imponible:",-26}{baseImponible,15:N2} €");
        Console.WriteLine($"  {etiquetaIva,-26}{cuotaIva,15:N2} €");
        Console.WriteLine($"  {"TOTAL A PAGAR:",-26}{total,15:N2} €");
    }

    /// <summary>
    /// Devuelve la cuota de IVA redondeada a céntimos.
    /// </summary>
    public static double CalcularIva(double baseImponible, double tipoIva)
    {
        return Math.Round(baseImponible * tipoIva, 2, MidpointRounding.AwayFromZero);
    }
}
