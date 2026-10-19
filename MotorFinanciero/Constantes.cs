namespace MotorFinanciero;

/// <summary>
/// Valores fijos de la aplicación. Se declaran como constantes (const)
/// para que no puedan modificarse durante la ejecución (RA 1).
/// </summary>
public static class Constantes
{
    // ---- Tipos de IVA vigentes en España (expresados en tanto por uno) ----
    public const double IVA_GENERAL = 0.21;        // 21 %: la mayoría de bienes y servicios
    public const double IVA_REDUCIDO = 0.10;       // 10 %: hostelería, transporte de viajeros...
    public const double IVA_SUPERREDUCIDO = 0.04;  //  4 %: pan, leche, libros, medicamentos...
    public const double IVA_EXENTO = 0.0;          //  0 %: sanidad, educación, seguros...

    // ---- Límites para la validación de usuarios ----
    public const int EDAD_MINIMA = 18;
    public const int EDAD_MAXIMA = 120;
    public const int LONGITUD_MIN_USUARIO = 4;
    public const int LONGITUD_MAX_USUARIO = 15;
    public const int LONGITUD_MIN_PASSWORD = 8;

    // Letras de control del DNI. La posición de la letra es (número DNI % 23).
    public const string LETRAS_DNI = "TRWAGMYFPDXBNJZSQVHLCKE";

    // ---- Límites para el simulador de préstamos ----
    public const double IMPORTE_MINIMO_PRESTAMO = 100.0;
    public const double IMPORTE_MAXIMO_PRESTAMO = 1000000.0;
    public const double INTERES_MAXIMO_ANUAL = 30.0;  // en porcentaje
    public const int PLAZO_MAXIMO_MESES = 480;         // 40 años
    public const int MESES_POR_ANIO = 12;
}
