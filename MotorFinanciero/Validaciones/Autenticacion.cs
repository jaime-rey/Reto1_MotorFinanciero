using System;
using System.Collections.Generic;
using System.IO;
using MotorFinanciero.IO;

namespace MotorFinanciero.Validaciones;

/// <summary>
/// Inicio de sesión de usuarios. Las parejas usuario;contraseña se guardan
/// en un fichero CSV (usuarios.csv) junto al ejecutable.
/// </summary>
public static class Autenticacion
{
    private const int MAX_INTENTOS = 3;
    private const char SEPARADOR = ';';

    private static readonly string rutaCsv = Path.Combine(AppContext.BaseDirectory, "usuarios.csv");

    private static readonly Dictionary<string, string> usuarios = CargarUsuarios();

    /// <summary>
    /// Lee el CSV. Si no existe, lo crea con los usuarios por defecto.
    /// </summary>
    private static Dictionary<string, string> CargarUsuarios()
    {
        var lista = new Dictionary<string, string>();

        try
        {
            if (!File.Exists(rutaCsv))
            {
                File.WriteAllLines(rutaCsv, new[] { "admin;Admin1234", "jaime;Dam12345", "invitado;Invitado1" });
            }

            foreach (var linea in File.ReadAllLines(rutaCsv))
            {
                var partes = linea.Split(SEPARADOR, 2);
                if (partes.Length == 2 && partes[0].Length > 0)
                {
                    lista.TryAdd(partes[0], partes[1]);
                }
            }
        }
        catch (Exception ex) when (ex is IOException || ex is UnauthorizedAccessException)
        {
            EntradaDatos.MostrarError("No se pudo leer usuarios.csv: " + ex.Message);
        }

        return lista;
    }

    /// <summary>
    /// Añade un usuario ya validado a la lista y lo guarda en el CSV.
    /// Devuelve false si el nombre ya existe.
    /// </summary>
    public static bool RegistrarUsuario(string usuario, string password)
    {
        if (!usuarios.TryAdd(usuario, password))
        {
            return false;
        }

        try
        {
            File.AppendAllText(rutaCsv, usuario + SEPARADOR + password + Environment.NewLine);
        }
        catch (Exception ex) when (ex is IOException || ex is UnauthorizedAccessException)
        {
            EntradaDatos.MostrarError("No se pudo guardar en usuarios.csv: " + ex.Message);
        }
        return true;
    }

    /// <summary>
    /// Pide usuario y contraseña (máximo
    /// </summary>
    public static bool IniciarSesion()
    {
        Console.WriteLine();
        Console.WriteLine("------------- INICIO DE SESIÓN -------------");

        for (var intento = 1; intento <= MAX_INTENTOS; intento++)
        {
            var usuario = EntradaDatos.LeerTexto("Usuario (invitado): ");
            var password = EntradaDatos.LeerTexto("Contraseña (Invitado1): ");

            // Antes de comprobar las credenciales se aplican las reglas de ValidadorUsuario
            if (!ValidadorUsuario.EsUsuarioValido(usuario) || !ValidadorUsuario.EsPasswordValida(password))
            {
                EntradaDatos.MostrarError($"Datos no válidos ({intento}/{MAX_INTENTOS}).");
                continue;
            }

            if (usuarios.TryGetValue(usuario, out var passwordCorrecta) && passwordCorrecta == password)
            {
                EntradaDatos.MostrarOk($"Bienvenido, {usuario}.");
                return true;
            }

            EntradaDatos.MostrarError($"Usuario o contraseña incorrectos ({intento}/{MAX_INTENTOS}).");
        }

        return false;
    }
}
