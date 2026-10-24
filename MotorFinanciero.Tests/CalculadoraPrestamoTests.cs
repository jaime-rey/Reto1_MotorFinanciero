namespace MotorFinanciero.Tests;

public class CalculadoraPrestamoTests
{
    // ---------- Caso especial: interés 0 → reparto a partes iguales ----------

    [Theory]
    [InlineData(1200, 12, 100.00)]  // 1200 € en 12 meses → 100 €/mes
    [InlineData(1000, 10, 100.00)]
    [InlineData(500, 4, 125.00)]
    public void CalcularCuota_ConInteresCero_DivideCapitalEntreCuotas(
        double capital, int numeroCuotas, double cuotaEsperada)
    {
        double cuota = CalculadoraPrestamo.CalcularCuota(capital, 0, numeroCuotas);

        Assert.Equal(cuotaEsperada, cuota);
    }

    // ---------- Caso general: sistema francés ----------
    // Fórmula: cuota = C·i / (1 − (1+i)^(−n))
    // Ejemplo: 10 000 € al 5 % anual, 24 meses → cuota mensual ≈ 438,71 €

    [Fact]
    public void CalcularCuota_ConInteresPositivo_AplicaFormulaFrancesa()
    {
        double capital = 10_000;
        double interesMensual = 0.05 / 12; // 5 % anual → mensual
        int numeroCuotas = 24;

        double cuota = CalculadoraPrestamo.CalcularCuota(capital, interesMensual, numeroCuotas);

        // Tercer parámetro = precisión en nº de decimales. Equivale al "delta" de JUnit:
        //   assertEquals(expected, actual, delta)
        // Aquí: debe coincidir hasta 2 decimales (céntimos).
        Assert.Equal(438.71, cuota, 2);
    }

    // ---------- La cuota debe ser positiva ----------

    [Fact]
    public void CalcularCuota_DevuelveValorPositivo()
    {
        double cuota = CalculadoraPrestamo.CalcularCuota(5000, 0.03 / 12, 36);

        Assert.True(cuota > 0, $"La cuota debería ser > 0 pero fue {cuota}");
    }

    // ---------- A mayor plazo, menor cuota (misma deuda, mismo interés) ----------

    [Fact]
    public void CalcularCuota_AMayorPlazo_MenorCuota()
    {
        double capital = 10_000;
        double interesMensual = 0.04 / 12;

        double cuotaA12Meses = CalculadoraPrestamo.CalcularCuota(capital, interesMensual, 12);
        double cuotaA60Meses = CalculadoraPrestamo.CalcularCuota(capital, interesMensual, 60);

        Assert.True(cuotaA60Meses < cuotaA12Meses,
            $"Esperado: cuota a 60 meses ({cuotaA60Meses}) < cuota a 12 meses ({cuotaA12Meses})");
    }
}
