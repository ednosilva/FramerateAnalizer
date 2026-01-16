using FramerateAnalyzer.Domain;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace FramerateAnalyzer.Infrastructure
{
    public class FramerateCaptureReader
    {
        private JsonSerializerOptions jsonOptions;
        
        public FramerateCaptureReader()
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

        public List<FramerateCapture> GetFramerateCaptures(IList<CapFrameXData> capFrameXData)
        {
            var captures = new List<FramerateCapture>();
            
            foreach (CapFrameXData data in capFrameXData)
            {
                IList<FrameCaptureRun>? runFrameCaptures = data.Runs == null ? [] :
                    data.Runs.Select(r => new FrameCaptureRun(r.CaptureData.TimeInSeconds)).ToList();

                string cpu = data.Info?.Processor
                    .Replace("AMD", string.Empty)
                    .Replace("Intel", string.Empty)
                    .Trim() ?? "Unknown";

                string gpu = data.Info?.GPU
                    .Replace("NVIDIA", string.Empty)
                    .Replace("NVidia", string.Empty)
                    .Replace("GeForce", string.Empty)
                    .Replace("AMD", string.Empty)
                    .Replace("Radeon", string.Empty)
                    .Trim() ?? "Unknown";

                var capture = new FramerateCapture(data.FileName, cpu, gpu, data.Info?.SystemRam ?? "Unknown",
                    data.Info?.GameName ?? "Unknown", data.Info?.Comment ?? "Unknown",
                    data.Info?.CreationDate ?? DateTime.MinValue, runFrameCaptures);
                
                captures.Add(capture);
            }
            
            return captures;
        }
    }
}