namespace MotorFinanciero.Tests;

public class ValidadorUsuarioTests
{
    // ---------- EsUsuarioValido ----------

    [Theory]
    [InlineData("jaime")]           // solo letras
    [InlineData("usuario123")]      // letras y números
    [InlineData("ABC12")]           // mayúsculas permitidas
    public void EsUsuarioValido_ConUsuariosCorrectos_DevuelveTrue(string usuario)
    {
        Assert.True(ValidadorUsuario.EsUsuarioValido(usuario));
    }

    [Theory]
    [InlineData("ab")]                                // demasiado corto
    [InlineData("usuario_largo_que_pasa_del_limite")] // demasiado largo
    [InlineData("hola mundo")]                        // contiene espacio
    [InlineData("user@name")]                         // carácter no permitido
    public void EsUsuarioValido_ConUsuariosIncorrectos_DevuelveFalse(string usuario)
    {
        Assert.False(ValidadorUsuario.EsUsuarioValido(usuario));
    }

    // ---------- EsPasswordValida ----------

    [Theory]
    [InlineData("Abcdef12")] // justo 8 caracteres: mínimo permitido
    [InlineData("MiPass123")]
    public void EsPasswordValida_ConPasswordsCorrectas_DevuelveTrue(string password)
    {
        Assert.True(ValidadorUsuario.EsPasswordValida(password));
    }

    [Theory]
    [InlineData("Ab1")]      // demasiado corta
    [InlineData("abcdefg1")] // sin mayúscula
    [InlineData("ABCDEFG1")] // sin minúscula
    [InlineData("Abcdefgh")] // sin número
    public void EsPasswordValida_ConPasswordsIncorrectas_DevuelveFalse(string password)
    {
        Assert.False(ValidadorUsuario.EsPasswordValida(password));
    }

    // ---------- EsDniValido ----------

    [Theory]
    [InlineData("12345678Z")] // 12345678 % 23 = 14 → 'Z'
    [InlineData("00000000T")] // 0 % 23 = 0 → 'T'
    public void EsDniValido_ConDniCorrecto_DevuelveTrue(string dni)
    {
        Assert.True(ValidadorUsuario.EsDniValido(dni));
    }

    [Theory]
    [InlineData("12345678A")]  // letra incorrecta
    [InlineData("1234567Z")]   // solo 8 caracteres
    [InlineData("123456789Z")] // 10 caracteres
    [InlineData("1234567AZ")]  // letra en zona numérica
    public void EsDniValido_ConDniIncorrecto_DevuelveFalse(string dni)
    {
        Assert.False(ValidadorUsuario.EsDniValido(dni));
    }
}
