using FizzBuzz.Domain.Exceptions;
using FizzBuzz.Infrastructure.Interface;
using Microsoft.Extensions.Logging;
using System.Collections.Concurrent;
using System.Runtime.CompilerServices;

namespace FizzBuzz.Infrastructure.Implementation
{
    public class FileWriter : IFileWriter
    {
        private readonly string _filePath = "fizzbuzz_log.txt";
        private readonly ConcurrentQueue<string> _queue = new ConcurrentQueue<string>();
        private readonly SemaphoreSlim _semaphore = new SemaphoreSlim(1, 1);
        private readonly ILogger<FileWriter> _logger;

        public FileWriter(ILogger<FileWriter> logger)
        {
            // Iniciar un proceso en segundo plano para escribir en el archivo
            Task.Run(WriteToFileBackground);
            _logger = logger;
        }

        public void WriteToFile(List<string> data)
        {
            _queue.Enqueue(DateTime.UtcNow + ": " + string.Join(", ", data));
        }

        private async Task WriteToFileBackground()
        {
            while (true)
            {
                try
                {             
                    if (_queue.TryDequeue(out var line))
                    {
                        await _semaphore.WaitAsync();
                        try
                        {
                            await File.AppendAllLinesAsync(_filePath, new[] { line });
                        }
                        finally
                        {
                            _semaphore.Release();
                        }
                    }
                    else
                    {
                        await Task.Delay(100); // Esperar antes de revisar la cola nuevamente
                    }
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "Error writing to file.");
                    throw new FileWriteException("Error writing to file", ex);
                }
            }
        }
    }
}