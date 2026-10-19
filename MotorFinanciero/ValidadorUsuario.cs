using System;
using System.Linq;

namespace MotorFinanciero;

/// <summary>
/// Opción 2 del menú: alta y validación de un cliente.
/// Comprueba nombre de usuario, contraseña, DNI y edad.
/// </summary>
public static class ValidadorUsuario
{
    public static void Ejecutar()
    {
        Console.WriteLine();
        Console.WriteLine("=== VALIDACIÓN DE USUARIO ===");

        // Cada dato se pide en bucle hasta que sea válido.
        string usuario;
        do
        {
            usuario = EntradaDatos.LeerTexto(
                $"Nombre de usuario ({Constantes.LONGITUD_MIN_USUARIO}-{Constantes.LONGITUD_MAX_USUARIO} letras o números): ");
        } while (!EsUsuarioValido(usuario));

        string password;
        do
        {
            password = EntradaDatos.LeerTexto(
                $"Contraseña (mín. {Constantes.LONGITUD_MIN_PASSWORD} caracteres, una mayúscula, una minúscula y un número): ");
        } while (!EsPasswordValida(password));

        string dni;
        do
        {
            dni = EntradaDatos.LeerTexto("DNI (8 números y letra, ej. 12345678Z): ").ToUpper();
        } while (!EsDniValido(dni));

        int edad = EntradaDatos.LeerEntero("Edad: ", 0, Constantes.EDAD_MAXIMA);

        Console.WriteLine();
        EntradaDatos.MostrarOk($"Usuario '{usuario}' validado correctamente (DNI {dni}).");

        // Condicional final: solo los mayores de edad pueden pedir un préstamo.
        if (edad >= Constantes.EDAD_MINIMA)
        {
            EntradaDatos.MostrarOk("El cliente es mayor de edad: puede solicitar préstamos.");
        }
        else
        {
            EntradaDatos.MostrarError($"El cliente tiene {edad} años: no puede solicitar préstamos hasta los {Constantes.EDAD_MINIMA}.");
        }
    }

    /// <summary>
    /// El usuario debe tener la longitud permitida y solo letras o números.
    /// </summary>
    public static bool EsUsuarioValido(string usuario)
    {
        if (usuario.Length < Constantes.LONGITUD_MIN_USUARIO || usuario.Length > Constantes.LONGITUD_MAX_USUARIO)
        {
            EntradaDatos.MostrarError("Longitud de usuario no válida.");
            return false;
        }

        // Busca el primer carácter que no sea letra ni número.
        // Si no hay ninguno, FirstOrDefault devuelve '\0' (el valor por defecto de char).
        char caracterNoPermitido = usuario.FirstOrDefault(c => !char.IsLetterOrDigit(c));

        if (caracterNoPermitido != '\0')
        {
            EntradaDatos.MostrarError($"El carácter '{caracterNoPermitido}' no está permitido en el nombre de usuario.");
            return false;
        }
        return true;
    }

    /// <summary>
    /// La contraseña debe tener longitud mínima, al menos una mayúscula,
    /// una minúscula y un dígito. Se recorre carácter a carácter con un for.
    /// </summary>
    public static bool EsPasswordValida(string password)
    {
        bool tieneMayuscula = false;
        bool tieneMinuscula = false;
        bool tieneNumero = false;

        for (int i = 0; i < password.Length; i++)
        {
            char c = password[i];
            if (char.IsUpper(c)) tieneMayuscula = true;
            else if (char.IsLower(c)) tieneMinuscula = true;
            else if (char.IsDigit(c)) tieneNumero = true;
        }

        if (password.Length < Constantes.LONGITUD_MIN_PASSWORD)
        {
            EntradaDatos.MostrarError($"La contraseña debe tener al menos {Constantes.LONGITUD_MIN_PASSWORD} caracteres.");
            return false;
        }
        else if (!tieneMayuscula || !tieneMinuscula || !tieneNumero)
        {
            EntradaDatos.MostrarError("La contraseña necesita al menos una mayúscula, una minúscula y un número.");
            return false;
        }
        return true;
    }

    /// <summary>
    /// Valida un DNI español: 8 dígitos + letra de control.
    /// La letra correcta es LETRAS_DNI[número % 23].
    /// </summary>
    public static bool EsDniValido(string dni)
    {
        if (dni.Length != 9)
        {
            EntradaDatos.MostrarError("El DNI debe tener 9 caracteres (8 números y una letra).");
            return false;
        }

        string parteNumerica = dni.Substring(0, 8);
        char letra = dni[8];

        try
        {
            // Conversión string -> int. Si hay letras en la parte numérica, salta FormatException.
            int numero = int.Parse(parteNumerica);
            char letraCorrecta = Constantes.LETRAS_DNI[numero % 23];

            if (letra != letraCorrecta)
            {
                EntradaDatos.MostrarError($"La letra del DNI no es correcta.");
                return false;
            }
            return true;
        }
        catch (FormatException)
        {
            EntradaDatos.MostrarError("Los 8 primeros caracteres del DNI deben ser números.");
            return false;
        }
    }
}
