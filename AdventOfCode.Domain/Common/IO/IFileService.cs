using System;
using System.Collections.Generic;
using System.Text;

namespace AdventOfCode.Domain.Common.IO
{
    public interface IFileService
    {

        public Task CreateFile(string path, string content);
        public Task<string> ReadFile(string path);
        public Task<List<string>> ReadLines(string path);
        public Task UpdateFile(string path, string content);
        public Task DeleteFile(string path);
    }
}