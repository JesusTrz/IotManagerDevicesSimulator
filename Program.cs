using IotDevicesSimulator.Models;
using Microsoft.AspNetCore.SignalR.Client;

// Dispositivo Inicial
var dispositivo = new DeviceIot
{
    //MacAddress = "11:22:33:44:55:66",
    MacAddress = "AA:22:BB:44:CC:66",
    //MacAddress = "AA:BB:CC:DD:EE:FF", // ID FISICA
    Nombre = "Sensor de Oficina",
    Tipo = "SensorTemperatura",

    Configuracion = new
    {
        IntervaloEnvio = 5,      // Enviar datos cada 5 segundos
        Unidad = "Celsius",      // Grados Celsius
        UmbralAlerta = 30,       // Avisar si pasa de 30 grados
        ModoAhorro = false       // Modo normal
    }
};

Console.WriteLine($"Dispositivo: {dispositivo.Nombre} inicializado.");
Console.WriteLine($"Configuracion base: {dispositivo.Configuracion}");

    // Conexión
var connection = new HubConnectionBuilder()
    .WithUrl($"https://localhost:7291/iotHub?macAddress={dispositivo.MacAddress}") //URL
    .WithAutomaticReconnect() // Si se cae el internet se reconecta solo
    .Build();{

    connection.On<object>("RecibirConfiguracion", (nuevaConfig) =>
    {
        Console.WriteLine($"[ORDEN RECIBIDA] : {nuevaConfig}");
    });

    try
    {
        await connection.StartAsync();
        Console.WriteLine("Dispositivo conectado exitosamente!");

        var random = new Random();

        while (true)
        {
            var datos = new
            {
                Mac = dispositivo.MacAddress,
                Tipo = "Telemetrica",
                Valor = random.Next(20, 35),
                Fecha = DateTime.Now
            };

            if (connection.State == HubConnectionState.Connected)
            {
                await connection.InvokeAsync("ReportarEstado", datos);
                Console.WriteLine($"[ENVIADO] Temp: {datos.Valor}°C");
            }
            dynamic config = dispositivo.Configuracion;
            int segundos = config.IntervaloEnvio;
            await Task.Delay(segundos * 1000);
        }
    }
    catch (Exception ex)
    {
        Console.WriteLine($"Error de conexión: {ex.Message}");
    }

    Console.ReadLine();
}