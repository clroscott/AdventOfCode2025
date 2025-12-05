using AdventOfCode.Domain.Common.IO;
using System;
using System.Collections.Generic;
using System.Text;

namespace AdventOfCode.Infrastructure.Common.IO
{
    public sealed class FileService : IFileService
    {
        public Task CreateFile(string path, string content)
        {
            return File.WriteAllTextAsync(path, content);
        }
        public Task<string> ReadFile(string path)
        {
            return File.ReadAllTextAsync(path);
        }
        public async Task<List<string>> ReadLines(string path)
        {
            var lines = await File.ReadAllLinesAsync(path);
            return new List<string>(lines);
        }
        public Task UpdateFile(string path, string content)
        {
            return File.WriteAllTextAsync(path, content);
        }
        public Task DeleteFile(string path)
        {
            File.Delete(path);
            return Task.CompletedTask;
        }

    }
}
