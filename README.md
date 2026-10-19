# Reto 1 – Algoritmos y Lógica de Control: El Motor de la Aplicación

Aplicación de consola en C# (.NET 10) que hace tres cosas desde un menú:

1. **Calcular IVA** de una operación (general 21 %, reducido 10 %, superreducido 4 %, exento 0 %).
2. **Validar un usuario**: nombre de usuario, contraseña, DNI (con letra de control) y edad.
3. **Simular un préstamo** y mostrar la tabla de amortización (sistema francés, cuota fija).

## Cómo ejecutarlo

Requiere el SDK de .NET 10.

- **Visual Studio 2026**: abrir `MotorFinanciero.sln` y pulsar F5.
- **Visual Studio Code**: abrir la carpeta `MotorFinanciero` y ejecutar con Code Runner
  (la configuración está en `.vscode/settings.json`), o usar el terminal.
- **Terminal**: desde esta carpeta, `dotnet run --project MotorFinanciero`.

## Estructura

| Archivo | Qué contiene |
|---|---|
| `Program.cs` | Método `Main`: menú principal (bucle `while` + `switch`). |
| `Constantes.cs` | Tipos de IVA y límites de validación (`const`). |
| `EntradaDatos.cs` | Lectura segura por consola con `try-catch` (no se cae con letras, vacíos, etc.) y la excepción propia `EntradaCerradaException`. |
| `CalculadoraImpuestos.cs` | Cálculo del IVA con `switch` y `Math.Round`. |
| `ValidadorUsuario.cs` | Validaciones con `if / else if / else`, `do-while`, `for` y una consulta LINQ. |
| `CalculadoraPrestamo.cs` | Cuota con `Math.Pow` y tabla de amortización con `for` / `while`. |

## Fórmula de la cuota (sistema francés)

```
cuota = C · i / (1 − (1 + i)^(−n))
```

- `C` = capital prestado
- `i` = interés mensual en tanto por uno (interés anual % / 100 / 12)
- `n` = número de cuotas (meses)

Cada mes: intereses = saldo pendiente · i; amortización = cuota − intereses; nuevo saldo = saldo − amortización.
La última cuota se ajusta para que el saldo termine exactamente en 0 (absorbe los céntimos del redondeo).
Si el interés es 0 %, la cuota es simplemente C / n.

## Relación con los resultados de aprendizaje

- **RA 1 (sintaxis y tipos)**: variables `double`, `int`, `string`, `char`, `bool`; constantes `const`; conversiones con `int.Parse`/`double.Parse` y casting explícito `(int)`. Sintaxis actual de C#: namespaces de archivo (`namespace MotorFinanciero;`) y `?? throw`.
- **RA 2 (entrada y librerías)**: `Console` para entrada/salida, `Math.Pow`, `Math.Round`, `Math.Ceiling` y LINQ (`Where`, `Select`, `FirstOrDefault`) para buscar caracteres no permitidos.
- **RA 3 (control de flujo)**: `if / else if / else`, `switch`, bucles `for`, `while` y `do-while`.
- **Errores**: `try-catch` con `FormatException` y `OverflowException`; el programa vuelve a pedir el dato en lugar de cerrarse. Si se cierra la entrada (Ctrl+Z), sale de forma ordenada.
