using Newtonsoft.Json;
using System.IO;
using UnityEngine;

public class JsonSaveService : ISaveService
{
    public void Save(string fileName, object data)
    {
        string path = BuildFilePath(fileName);
        string json = JsonConvert.SerializeObject(data, Formatting.Indented);
        using(StreamWriter streamWriter = new StreamWriter(path)) 
        {
            streamWriter.Write(json);
        }
    }

    public T Load<T>(string fileName)
    {
        string path = BuildFilePath(fileName);
        try
        {
            using (StreamReader streamReader = new StreamReader(path))
            {
                string json = streamReader.ReadToEnd();
                return JsonConvert.DeserializeObject<T>(json);
            }
        }
        catch(FileNotFoundException e)
        {
            Debug.LogError(e.Message);
        }
        return default;
    }

    private string BuildFilePath(string fileName)
    {
        return Path.Combine(Application.dataPath, fileName);
    }
}