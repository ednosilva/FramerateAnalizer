using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace FPSAnalizer
{
    public class CapFrameXReader
    {
        private JsonSerializerOptions jsonOptions;
        
        public CapFrameXReader()
        {
            jsonOptions = new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true,
                Converters = { new JsonStringEnumConverter() }
            };
        }
        
        /// <summary>
        /// Lê um arquivo JSON do CapFrameX
        /// </summary>
        public async Task<CapFrameXData> ReadFile(Stream stream, string fileName)
        {
            try
            {
                if (stream.CanSeek)
                    stream.Position = 0;

                CapFrameXData data = await JsonSerializer.DeserializeAsync<CapFrameXData>(stream, jsonOptions);

                data.FileName = fileName;
                data.FilePath = fileName;

                return data;
            }
            catch (JsonException ex)
            {
                throw new InvalidOperationException($"Erro ao analisar JSON do arquivo {fileName}: {ex.Message}");
            }
            catch (Exception ex)
            {
                throw new InvalidOperationException($"Erro ao ler arquivo {fileName}: {ex.Message}");
            }
        }
        
        /// <summary>
        /// Lê todos os arquivos JSON de um diretório e até 2 subdiretórios
        /// </summary>
        public async Task<List<CapFrameXData>> ReadDirectory(List<Stream> streams, IProgress<string> progress = null)
        {
            var allData = new List<CapFrameXData>();

            foreach (var stream in streams)
            {
                try
                {
                    if (stream.CanSeek)
                        stream.Position = 0;

                    var data = await JsonSerializer.DeserializeAsync<CapFrameXData>(
                        stream,
                        jsonOptions
                    );

                    if (data != null)
                    {
                        data.FileName = "StreamInput";
                        data.FilePath = "StreamInput";

                        allData.Add(data);
                        progress?.Report($"✓ Loaded - {data.Info?.GameName ?? "Unknown"}");
                    }
                }
                catch (Exception ex)
                {
                    progress?.Report($"✗ Error: {ex.Message}");
                }
            }

            return allData;
        }
        
        //private void SearchSubdirectories(string directoryPath, List<CapFrameXData> allData,
        //    IProgress<string> progress, int currentDepth)
        //{
        //    if (currentDepth > 2) return; // Limite de 2 subdiretórios
            
        //    try
        //    {
        //        var subdirectories = Directory.GetDirectories(directoryPath);
        //        foreach (var subdir in subdirectories)
        //        {
        //            SearchAndReadFiles(subdir, allData, progress, currentDepth);
        //            // Recursão para subsubdiretórios
        //            SearchSubdirectories(subdir, allData, progress, currentDepth + 1);
        //        }
        //    }
        //    catch (UnauthorizedAccessException)
        //    {
        //        progress?.Report($"Acesso negado ao diretório: {directoryPath}");
        //    }
        //    catch (Exception ex)
        //    {
        //        progress?.Report($"Erro ao acessar subdiretório {directoryPath}: {ex.Message}");
        //    }
        //}
        
        //private void SearchAndReadFiles(string directoryPath, List<CapFrameXData> allData, 
        //    IProgress<string> progress, int depth)
        //{
        //    try
        //    {
        //        var files = Directory.GetFiles(directoryPath, "*.json");
        //        progress?.Report($"Encontrados {files.Length} arquivos em: {GetDirectoryDisplay(directoryPath, depth)}");
                
        //        foreach (var file in files)
        //        {
        //            try
        //            {
        //                progress?.Report($"Lendo: {Path.GetFileName(file)}");
        //                var data = ReadFile(file, file);
        //                if (data != null)
        //                {
        //                    allData.Add(data);
        //                    progress?.Report($"✓ {Path.GetFileName(file)} - {data.Info?.GameName ?? "Desconhecido"}");
        //                }
        //            }
        //            catch (Exception ex)
        //            {
        //                progress?.Report($"✗ Erro em {Path.GetFileName(file)}: {ex.Message}");
        //            }
        //        }
        //    }
        //    catch (UnauthorizedAccessException)
        //    {
        //        progress?.Report($"Acesso negado: {directoryPath}");
        //    }
        //    catch (Exception ex)
        //    {
        //        progress?.Report($"Erro ao ler arquivos de {directoryPath}: {ex.Message}");
        //    }
        //}
        
        private string GetDirectoryDisplay(string path, int depth)
        {
            var directoryName = Path.GetFileName(path);
            if (string.IsNullOrEmpty(directoryName))
                directoryName = path;
            
            return depth == 0 ? directoryName : $"{new string(' ', depth * 2)}└─ {directoryName}";
        }
        
        /// <summary>
        /// Lê arquivos em paralelo (mais rápido para muitos arquivos)
        /// </summary>
        public async Task<List<CapFrameXData>> ReadDirectoryAsync(List<Stream> streams,
            IProgress<string> progress = null)
        {
            return await Task.Run(() => ReadDirectory(streams, progress));
        }
        
        /// <summary>
        /// Converte dados para exibição na lista
        /// </summary>
        public List<FramerateCapture> GetFileInfoDisplays(List<CapFrameXData> capFrameXData)
        {
            var displays = new List<FramerateCapture>();
            
            foreach (var data in capFrameXData)
            {
                var capture = new FramerateCapture
                {
                    FileName = data.FileName,
                    GameName = data.Info?.GameName ?? "Desconhecido",
                    Cpu = data.Info?.Processor ?? "Desconhecido",
                    Gpu = data.Info?.GPU ?? "Desconhecido",
                    GameSettings = data.Info?.Comment ?? "Desconhecido",
                    CaptureDate = data.Info?.CreationDate ?? DateTime.MinValue,
                    RunCount = data.Runs?.Count ?? 0,
                    AverageFramerate = data.Runs?.FirstOrDefault()?.CaptureData?.AverageFps ?? 0,
                    FilePath = data.FilePath
                };
                
                // Adicionar as novas métricas se disponíveis
                var firstRun = data.Runs?.FirstOrDefault();
                if (firstRun?.Statistics != null)
                {
                    var stats = firstRun.Statistics;
                    capture.TenPercentLowFramerate = stats.TenPercentLowFramerate;
                    capture.OnePercentLowFramerate = stats.OnePercentLowFramerate;
                    capture.ZeroPointOnePercentLowFramerate = stats.ZeroPointOnePercentLowFramerate;
                }
                
                displays.Add(capture);
            }
            
            return displays;
        }
    }
}