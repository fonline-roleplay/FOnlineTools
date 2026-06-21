using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;

namespace ObjectEditor
{
    public class ItempiDefinesdParser
    {
        private Dictionary<int, string> _defineByPID = new Dictionary<int, string>();
        public Dictionary<int, string> defineByPID { get { return _defineByPID; } }

        public void ReadLine(string line)
        {
            if (string.IsNullOrEmpty(line) || !line.StartsWith("#define")) return;

            int firstDefineIndex = -1;
            int lastDefineIndex = -1;
            for(int i = 7, len = line.Length; i < len; i++)
            {
                char ch = line[i];
                if (char.IsWhiteSpace(ch)) continue;
                firstDefineIndex = i;
                break;
            }
            for (int i = firstDefineIndex, len = line.Length; i < len; i++)
            {
                char ch = line[i];
                if (!char.IsWhiteSpace(ch) && ch != '\n') continue;
                lastDefineIndex = i;
                break;
            }
            if (lastDefineIndex == -1)
                lastDefineIndex = line.Length - 1;

            string defineName = line.Substring(firstDefineIndex, lastDefineIndex - firstDefineIndex);

            int firstPidIndex = line.IndexOf('(') + 1;
            if (firstPidIndex == -1) return;
            int lastPidIndex = line.IndexOf(')');
            if (lastPidIndex <= firstPidIndex) return;
            string pidStr = line.Substring(firstPidIndex, lastPidIndex - firstPidIndex);
            int pid = int.Parse(pidStr);
            if(pid <= 0) return;
            _defineByPID[pid] = defineName;
        }

        public async Task ReadFromFileAsync(string filePath)
        {
            if (!File.Exists(filePath))
            {
                throw new FileNotFoundException($"Can't find or access file '{filePath}");
            }

            using (StreamReader reader = new StreamReader(filePath))
            {
                string? line;
                while ((line = await reader.ReadLineAsync()) != null)
                {
                    ReadLine(line);
                }
            }
        }
    }
}
