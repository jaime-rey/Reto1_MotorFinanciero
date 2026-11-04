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

        var (tipoIva, nombreTipo, _) = PedirTipoIva("Tipo de IVA:");

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

    /// <summary>
    /// Permite cambiar el tipo de IVA de un producto.
    /// </summary>
    public static void CambiarIvaProducto()
    {
        Console.WriteLine();
        Console.WriteLine("=== CAMBIAR IVA DE UN PRODUCTO ===");

        var concepto = EntradaDatos.LeerTexto("Concepto del producto: ");
        var precioUnitario = EntradaDatos.LeerDouble("Precio unitario: ");

        int opcionIvaInicial;
        int opcionIvaFinal;
        string nombreTipoInicial;
        string nombreTipoFinal;
        double tipoIvaInicial;
        double tipoIvaFinal;

        do
        {
            (tipoIvaInicial, nombreTipoInicial, opcionIvaInicial) = PedirTipoIva("Tipo de IVA inicial:");
            (tipoIvaFinal, nombreTipoFinal, opcionIvaFinal) = PedirTipoIva("Tipo de IVA final:");

            if (opcionIvaInicial == opcionIvaFinal)
            {
                Console.WriteLine("Los tipos de IVA inicial y final no pueden ser iguales, por favor, elija un valor distinto de IVA inicial e IVA final.");
            }
        } while (opcionIvaInicial == opcionIvaFinal);
        double precioUnitarioFinal = CalcularPrecioFinal(precioUnitario, tipoIvaInicial, tipoIvaFinal);
        Console.WriteLine($"Se ha cambiado el IVA del producto '{concepto}' con valor {precioUnitario}€\n" +
                          $"con tipo de IVA inicial: {nombreTipoInicial}\na tipo de IVA final: {nombreTipoFinal}\n" +
                          $"Su precio final es: {precioUnitarioFinal:N2}€.");
    }

    private static (double tipoIva, string nombre, int opcion) PedirTipoIva(string titulo)
    {
        Console.WriteLine(titulo);
        Console.WriteLine("  1) General       (21 %)");
        Console.WriteLine("  2) Reducido      (10 %)");
        Console.WriteLine("  3) Superreducido  (4 %)");
        Console.WriteLine("  4) Exento         (0 %)");
        int opcion = EntradaDatos.LeerEntero("Elige el tipo (1-4): ", 1, 4);

        // Elegimos el porcentaje según la opción con un switch (RA 3)
        switch (opcion)
        {
            case 1: return (Constantes.IVA_GENERAL, "General", opcion);
            case 2: return (Constantes.IVA_REDUCIDO, "Reducido", opcion);
            case 3: return (Constantes.IVA_SUPERREDUCIDO, "Superreducido", opcion);
            default: return (Constantes.IVA_EXENTO, "Exento", opcion);
        }
    }

    private static double CalcularPrecioFinal(double precioUnitario, double tipoIvaInicial, double tipoIvaFinal)
    {
        double precioSinIva = precioUnitario / (1 + tipoIvaInicial);
        return precioSinIva * ( 1 + tipoIvaFinal);
    }
}
