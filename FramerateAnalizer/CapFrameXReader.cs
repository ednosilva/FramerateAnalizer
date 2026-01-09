using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace FramerateAnalizer
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

            if (streams == null || !streams.Any())
                throw new ArgumentException(
                    "Nenhum arquivo foi informado.",
                    nameof(streams));

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
        //                var data = ReadFile(file);
        //                if (data != null)
        //                {
        //                    allData.Add(data);
        //                    progress?.Report($"✓ {Path.GetFileName(file)} - {data.Info?.GameName ?? "Unknown"}");
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

        //    return allData;
        //}
        
        //private string GetDirectoryDisplay(string path, int depth)
        //{
        //    var directoryName = Path.GetFileName(path);
        //    if (string.IsNullOrEmpty(directoryName))
        //        directoryName = path;
            
        //    return depth == 0 ? directoryName : $"{new string(' ', depth * 2)}└─ {directoryName}";
        //}
        
        //public async Task<List<CapFrameXData>> ReadDirectoryAsync(string directoryPath,
        //    IProgress<string> progress = null)
        //{
        //    return await Task.Run(() => ReadDirectory(directoryPath, progress));
        //}
        
        public List<FramerateCapture> GetFramerateCaptures(List<CapFrameXData> capFrameXData)
        {
            var captures = new List<FramerateCapture>();
            
            foreach (CapFrameXData data in capFrameXData)
            {
                IList<RunFrameCapture>? runFrameCaptures = data.Runs == null ? [] :
                    data.Runs.Select(r => new RunFrameCapture(r.CaptureData.TimeInSeconds)).ToList();

                var capture = new FramerateCapture(data.FilePath, data.Info?.Processor ?? "Unknown",
                    data.Info?.GPU ?? "Unknown", data.Info?.SystemRam ?? "Unknown", data.Info?.GameName ?? "Unknown",
                    data.Info?.Comment ?? "Unknown", data.Info?.CreationDate ?? DateTime.MinValue,
                    runFrameCaptures);
                
                captures.Add(capture);
            }
            
            return captures;
        }
    }
}