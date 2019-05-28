using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Refracto.Data
{
    class FileStore : IStore
    {
        public FileStore(string basePath)
        {
            if (!Directory.Exists(basePath))
            {
                throw new ArgumentException("Directory not found", nameof(basePath));
            }
            BasePath = basePath;
        }

        public string BasePath { get; }

        private string GetFilePath(string id) => Path.Combine(BasePath, id + ".csv");

        public bool Create(Timeline timeline)
        {
            var filePath = GetFilePath(timeline.Id);
            if (File.Exists(filePath))
            {
                return false;
            }
            File.WriteAllLines(filePath, ReadoutSerializer.Serialize(timeline.Data));
            return true;
        }

        public IEnumerable<Timeline> ReadAll()
        {
            return Directory.EnumerateFiles(BasePath, "*.csv").Select(filePath => new Timeline(Path.GetFileNameWithoutExtension(filePath)));
        }

        public void ReadData(Timeline timeline)
        {
            var filePath = GetFilePath(timeline.Id);
            if (!File.Exists(filePath))
            {
                throw new ArgumentException("File not found", nameof(timeline));
            }
            timeline.Data.Clear();
            timeline.Data.AddRange(ReadoutSerializer.Deserialize(File.ReadAllLines(filePath)));
        }

        public void Update(Timeline timeline)
        {
            var filePath = GetFilePath(timeline.Id);
            if (!File.Exists(filePath))
            {
                throw new ArgumentException("File not found", nameof(timeline));
            }
            File.WriteAllLines(filePath, ReadoutSerializer.Serialize(timeline.Data));
        }

        public void Delete(string id)
        {
            var filePath = GetFilePath(id);
            if (!File.Exists(filePath))
            {
                throw new ArgumentException("File not found", nameof(id));
            }
            File.Delete(filePath);
        }
    }
}
