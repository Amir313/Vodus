using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;

namespace Vodus.Web.Models
{
    public static class Utils
    {
        public static void WriteJsonFile(object obj, string fileName)
        {
            var jsonString = JsonConvert.SerializeObject(obj, Formatting.Indented);
            File.WriteAllText(fileName, jsonString);
        }
        public static MemoryStream DownloadFile(string filename, string uploadPath)
        {
            var path = Path.Combine(Directory.GetCurrentDirectory(), uploadPath, filename);
            var memory = new MemoryStream();
            if (System.IO.File.Exists(path))

            {
                var net = new System.Net.WebClient();
                var data = net.DownloadData(path);
                var content = new System.IO.MemoryStream(data);
                memory = content;
            }
            memory.Position = 0;
            return memory;
        }

        public static List<ImageDetailModel> ReadJsonFile(string filePath)
        {
            List<ImageDetailModel> items = null;
            using (StreamReader r = new StreamReader(filePath))
            {
                string jsonData = r.ReadToEnd();
                items = JsonConvert.DeserializeObject<List<ImageDetailModel>>(jsonData);
            }
            return items.Where(s => s.page != "").OrderBy(c => c.endDate).ToList();
        }
    }
}
