using System;

namespace MotorFinanciero;

/// <summary>
/// Opción 3 del menú: simulador de préstamo con tabla de amortización
/// por el sistema francés (cuota mensual constante).
/// </summary>
public static class CalculadoraPrestamo
{
    public static void Ejecutar()
    {
        Console.WriteLine();
        Console.WriteLine("=== SIMULADOR DE PRÉSTAMO (sistema francés) ===");

        double capital = EntradaDatos.LeerDouble("Importe del préstamo (€): ",
            Constantes.IMPORTE_MINIMO_PRESTAMO, Constantes.IMPORTE_MAXIMO_PRESTAMO);
        double interesAnual = EntradaDatos.LeerDouble("Tipo de interés anual (%, ej. 4,5): ",
            0, Constantes.INTERES_MAXIMO_ANUAL);
        double anios = EntradaDatos.LeerDouble("Plazo en años (se admiten decimales, ej. 2,5): ",
            1.0 / Constantes.MESES_POR_ANIO, (double)Constantes.PLAZO_MAXIMO_MESES / Constantes.MESES_POR_ANIO);

        // Casting explícito double -> int: el plazo en años se pasa a un número entero de meses.
        int numeroCuotas = (int)Math.Round(anios * Constantes.MESES_POR_ANIO);

        // El interés anual en % se pasa a interés mensual en tanto por uno.
        double interesMensual = interesAnual / 100 / Constantes.MESES_POR_ANIO;

        double cuota = CalcularCuota(capital, interesMensual, numeroCuotas);

        Console.WriteLine();
        Console.WriteLine($"  Capital: {capital:N2} €   Interés: {interesAnual:N2} % anual   Plazo: {numeroCuotas} meses");
        Console.WriteLine($"  Cuota mensual: {cuota:N2} €");
        Console.WriteLine();

        if (numeroCuotas > 24 && !EntradaDatos.LeerSiNo($"La tabla tiene {numeroCuotas} filas. ¿Mostrarla completa?"))
        {
            MostrarResumen(capital, interesMensual, numeroCuotas, cuota);
        }
        else
        {
            MostrarTabla(capital, interesMensual, numeroCuotas, cuota);
        }
    }

    /// <summary>
    /// Cuota mensual del sistema francés:
    ///
    ///                 C · i
    ///   cuota = ─────────────────
    ///            1 − (1 + i)^(−n)
    ///
    /// C = capital prestado, i = interés mensual (tanto por uno), n = número de cuotas.
    /// Si el interés es 0 la fórmula divide entre 0, así que se reparte el capital
    /// a partes iguales.
    /// </summary>
    public static double CalcularCuota(double capital, double interesMensual, int numeroCuotas)
    {
        double cuota;
        if (interesMensual == 0)
        {
            cuota = capital / numeroCuotas;
        }
        else
        {
            cuota = capital * interesMensual / (1 - Math.Pow(1 + interesMensual, -numeroCuotas));
        }
        return Redondear(cuota);
    }

    /// <summary>
    /// Recorre todos los meses con un bucle for y muestra la tabla de amortización.
    /// En cada mes:
    ///   intereses    = saldo pendiente · interés mensual
    ///   amortización = cuota − intereses   (lo que reduce la deuda)
    ///   saldo nuevo  = saldo pendiente − amortización
    /// </summary>
    private static void MostrarTabla(double capital, double interesMensual, int numeroCuotas, double cuota)
    {
        Console.WriteLine($"{"Mes",5} | {"Cuota",12} | {"Intereses",12} | {"Amortización",12} | {"Pendiente",14}");
        Console.WriteLine(new string('-', 68));

        double saldo = capital;
        double totalIntereses = 0;
        double totalPagado = 0;

        for (int mes = 1; mes <= numeroCuotas; mes++)
        {
            double intereses = Redondear(saldo * interesMensual);
            double amortizacion;
            double cuotaMes;

            if (mes == numeroCuotas)
            {
                // Última cuota: se amortiza exactamente lo que queda para
                // absorber los céntimos de diferencia por redondeo.
                amortizacion = saldo;
                cuotaMes = Redondear(amortizacion + intereses);
            }
            else
            {
                amortizacion = Redondear(cuota - intereses);
                cuotaMes = cuota;
            }

            saldo = Redondear(saldo - amortizacion);
            totalIntereses += intereses;
            totalPagado += cuotaMes;

            Console.WriteLine($"{mes,5} | {cuotaMes,12:N2} | {intereses,12:N2} | {amortizacion,12:N2} | {saldo,14:N2}");
        }

        Console.WriteLine(new string('-', 68));
        Console.WriteLine($"  Total intereses: {totalIntereses:N2} €   Total pagado: {totalPagado:N2} €");
    }

    /// <summary>
    /// Versión corta para préstamos largos: calcula todo con un bucle while
    /// pero solo imprime el resumen de cada año.
    /// </summary>
    private static void MostrarResumen(double capital, double interesMensual, int numeroCuotas, double cuota)
    {
        Console.WriteLine($"{"Año",5} | {"Intereses año",14} | {"Amortizado año",14} | {"Pendiente",14}");
        Console.WriteLine(new string('-', 58));

        double saldo = capital;
        double interesesAnio = 0, amortizadoAnio = 0, totalIntereses = 0, totalPagado = 0;
        int mes = 1;

        while (mes <= numeroCuotas)
        {
            double intereses = Redondear(saldo * interesMensual);
            double amortizacion = (mes == numeroCuotas) ? saldo : Redondear(cuota - intereses);

            saldo = Redondear(saldo - amortizacion);
            interesesAnio += intereses;
            amortizadoAnio += amortizacion;
            totalIntereses += intereses;
            totalPagado += amortizacion + intereses;

            // Al cerrar cada año (o en la última cuota) se imprime una fila
            if (mes % Constantes.MESES_POR_ANIO == 0 || mes == numeroCuotas)
            {
                int anio = (int)Math.Ceiling((double)mes / Constantes.MESES_POR_ANIO);
                Console.WriteLine($"{anio,5} | {interesesAnio,14:N2} | {amortizadoAnio,14:N2} | {saldo,14:N2}");
                interesesAnio = 0;
                amortizadoAnio = 0;
            }
            mes++;
        }

        Console.WriteLine(new string('-', 58));
        Console.WriteLine($"  Total intereses: {totalIntereses:N2} €   Total pagado: {totalPagado:N2} €");
    }

    /// <summary>
    /// Redondea a céntimos (2 decimales).
    /// </summary>
    private static double Redondear(double valor)
    {
        return Math.Round(valor, 2, MidpointRounding.AwayFromZero);
    }
}
