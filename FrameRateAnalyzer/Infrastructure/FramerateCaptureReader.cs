using FramerateAnalyzer.Domain;
using System.Text.Json;
using System.Text.Json.Serialization;
using static System.Runtime.InteropServices.JavaScript.JSType;

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

            var groupedCaptures = capFrameXData
                .GroupBy(c => new
                {
                    c.Info.Processor,
                    c.Info.GPU,
                    c.Info.SystemRam,
                    c.Info.GameName,
                    c.Info.ResolutionInfo,
                    c.Info.Comment
                });

            foreach (var sameSettingsCaptures in groupedCaptures)
            {
                string cpu = sameSettingsCaptures.Key.Processor
                    .Replace("AMD", string.Empty)
                    .Replace("Intel", string.Empty)
                    .Trim() ?? "Unknown";

                string gpu = sameSettingsCaptures.Key.GPU
                    .Replace("NVIDIA", string.Empty)
                    .Replace("NVidia", string.Empty)
                    .Replace("GeForce", string.Empty)
                    .Replace("AMD", string.Empty)
                    .Replace("Radeon", string.Empty)
                    .Trim() ?? "Unknown";

                string memory = sameSettingsCaptures.Key.SystemRam ?? "Unknown";

                string gameName = sameSettingsCaptures.Key.GameName ?? "Unknown";

                string captureDetails = sameSettingsCaptures.Key.Comment ?? "Unknown";

                var gameAndSettingsCaptureData = sameSettingsCaptures.ToList();

                IList<FrameCaptureRun> runs = gameAndSettingsCaptureData
                    .SelectMany(data => data.Runs ?? [])
                    .Select(r => new FrameCaptureRun(r.Hash, r.CaptureData.MsBetweenPresents))
                    .ToList();

                var creationDate = gameAndSettingsCaptureData.OrderBy(d => d.Info.CreationDate).Last().Info.CreationDate;

                var capture = new FramerateCapture(cpu, gpu, memory, gameName, captureDetails, runs, creationDate);

                captures.Add(capture);
            }
            
            return captures;
        }
    }
}