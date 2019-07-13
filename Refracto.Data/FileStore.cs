using Refracto.Services;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Refracto.Data
{
    class FileStore : IStore
    {
        readonly ISettings m_Settings;

        public FileStore(ISettings settings)
        {
            m_Settings = settings;
        }

        public string BasePath
        {
            get
            {
                var path = m_Settings.FileStorePath;
                return Directory.Exists(path) ? path : AppDomain.CurrentDomain.BaseDirectory;
            }
        }

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
            return Directory.EnumerateFiles(BasePath, "*.csv").Select(filePath => new Timeline(Path.GetFileNameWithoutExtension(filePath), File.GetCreationTime(filePath)));
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
            File.SetCreationTime(filePath, timeline.Timestamp);
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
