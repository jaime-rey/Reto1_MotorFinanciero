namespace MotorFinanciero.Tests;

public class CalculadoraImpuestosTests
{
    [Fact]
    public void CalcularIva_ConBase100YTipo21_Devuelve21()
    {
        //Arrange: se preparan los datos
        const double baseImponible = 100.0;
        const double tipoIva = 0.21;
        
        // Act: se ejecuta lo que se pretende probar
        var resultado = CalculadoraImpuestos.CalcularIva(baseImponible, tipoIva);
        
        // Assert: se comprueba el resultado
        Assert.Equal(21.0, resultado);
        
    } 
    [Theory]                                                                                                                                                                                                                                    
    [InlineData(100, 0.21, 21.00)]   // IVA general                                                                                                                                                                                             
    [InlineData(100, 0.10, 10.00)]   // IVA reducido                                                                                                                                                                                            
    [InlineData(100, 0.04, 4.00)]    // IVA superreducido                                                                                                                                                                                       
    [InlineData(100, 0.00, 0.00)]    // IVA exento                                                                                                                                                                                              
    [InlineData(0,   0.21, 0.00)]    // base cero                                                                                                                                                                                               
    [InlineData(33.33, 0.21, 7.00)]  // redondeo a céntimos (33.33 * 0.21 = 6.9993 → 7.00)                                                                                                                                                      
    public void CalcularIva_DevuelveCuotaCorrecta(double baseImponible, double tipoIva, double esperado)                                                                                                                                        
    {                                                                                                                                                                                                                                           
        double resultado = CalculadoraImpuestos.CalcularIva(baseImponible, tipoIva);
        
        Assert.Equal(esperado, resultado);                                                                                                                                                                                                      
    }  
}