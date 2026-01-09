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
        public CapFrameXData ReadFile(string filePath)
        {
            try
            {
                if (!File.Exists(filePath))
                    throw new FileNotFoundException($"Arquivo não encontrado: {filePath}");
                
                string jsonContent = File.ReadAllText(filePath);
                var data = JsonSerializer.Deserialize<CapFrameXData>(jsonContent, jsonOptions);
                
                if (data != null)
                    data.FilePath = filePath;
                
                return data;
            }
            catch (JsonException ex)
            {
                throw new InvalidOperationException($"Erro ao analisar JSON do arquivo {Path.GetFileName(filePath)}: {ex.Message}");
            }
            catch (Exception ex)
            {
                throw new InvalidOperationException($"Erro ao ler arquivo {Path.GetFileName(filePath)}: {ex.Message}");
            }
        }
        
        /// <summary>
        /// Lê todos os arquivos JSON de um diretório e até 2 subdiretórios
        /// </summary>
        public List<CapFrameXData> ReadDirectory(string directoryPath, IProgress<string> progress = null)
        {
            var allData = new List<CapFrameXData>();
            
            if (!Directory.Exists(directoryPath))
                throw new DirectoryNotFoundException($"Diretório não encontrado: {directoryPath}");
            
            // Procura arquivos .json no diretório principal
            SearchAndReadFiles(directoryPath, allData, progress, 0);
            
            // Procura em subdiretórios (até 2 níveis)
            SearchSubdirectories(directoryPath, allData, progress, 1);
            
            return allData;
        }
        
        private void SearchSubdirectories(string directoryPath, List<CapFrameXData> allData,
            IProgress<string> progress, int currentDepth)
        {
            if (currentDepth > 2) return; // Limite de 2 subdiretórios
            
            try
            {
                var subdirectories = Directory.GetDirectories(directoryPath);
                foreach (var subdir in subdirectories)
                {
                    SearchAndReadFiles(subdir, allData, progress, currentDepth);
                    // Recursão para subsubdiretórios
                    SearchSubdirectories(subdir, allData, progress, currentDepth + 1);
                }
            }
            catch (UnauthorizedAccessException)
            {
                progress?.Report($"Acesso negado ao diretório: {directoryPath}");
            }
            catch (Exception ex)
            {
                progress?.Report($"Erro ao acessar subdiretório {directoryPath}: {ex.Message}");
            }
        }
        
        private void SearchAndReadFiles(string directoryPath, List<CapFrameXData> allData, 
            IProgress<string> progress, int depth)
        {
            try
            {
                var files = Directory.GetFiles(directoryPath, "*.json");
                progress?.Report($"Encontrados {files.Length} arquivos em: {GetDirectoryDisplay(directoryPath, depth)}");
                
                foreach (var file in files)
                {
                    try
                    {
                        progress?.Report($"Lendo: {Path.GetFileName(file)}");
                        var data = ReadFile(file);
                        if (data != null)
                        {
                            allData.Add(data);
                            progress?.Report($"✓ {Path.GetFileName(file)} - {data.Info?.GameName ?? "Unknown"}");
                        }
                    }
                    catch (Exception ex)
                    {
                        progress?.Report($"✗ Erro em {Path.GetFileName(file)}: {ex.Message}");
                    }
                }
            }
            catch (UnauthorizedAccessException)
            {
                progress?.Report($"Acesso negado: {directoryPath}");
            }
            catch (Exception ex)
            {
                progress?.Report($"Erro ao ler arquivos de {directoryPath}: {ex.Message}");
            }
        }
        
        private string GetDirectoryDisplay(string path, int depth)
        {
            var directoryName = Path.GetFileName(path);
            if (string.IsNullOrEmpty(directoryName))
                directoryName = path;
            
            return depth == 0 ? directoryName : $"{new string(' ', depth * 2)}└─ {directoryName}";
        }
        
        public async Task<List<CapFrameXData>> ReadDirectoryAsync(string directoryPath,
            IProgress<string> progress = null)
        {
            return await Task.Run(() => ReadDirectory(directoryPath, progress));
        }
        
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