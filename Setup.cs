using System.Collections;
using System.Collections.Generic;
using FubarDev.FtpServer;
using FubarDev.FtpServer.FileSystem.InMemory;
using Microsoft.Extensions.DependencyInjection;
using UnityEngine;
using System;
using System.Threading;
using System.Threading.Tasks;

public class Setup : MonoBehaviour
{
    private IFtpServer _ftpServer;

    async void Start()
    {
        await StartFtpServerAsync();
    }

    private async Task StartFtpServerAsync()
    {
        var services = new ServiceCollection();

        // Adiciona o servidor FTP com sistema de arquivos em memória e autenticação anônima
        services.AddFtpServer(builder => builder
            .UseInMemoryFileSystem() // Usa diretamente o sistema de arquivos em memória
            .EnableAnonymousAuthentication());

        var serviceProvider = services.BuildServiceProvider();

        // Cria e configura o servidor FTP
        _ftpServer = serviceProvider.GetRequiredService<IFtpServer>();

        try
        {
            // Inicia o servidor FTP
            await _ftpServer.StartAsync(CancellationToken.None);
            Debug.Log("Servidor FTP iniciado");
        }
        catch (Exception ex)
        {
            Debug.LogError($"Erro ao iniciar o servidor FTP: {ex.Message}");
        }
    }

    private async void OnApplicationQuit()
    {
        // Para o servidor FTP quando o jogo fecha
        if (_ftpServer != null)
        {
            await _ftpServer.StopAsync(CancellationToken.None);
        }
    }
}
