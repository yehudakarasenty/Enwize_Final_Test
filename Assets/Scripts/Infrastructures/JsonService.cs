using Newtonsoft.Json;
using System;
using System.IO;

public static class JsonService
{
    public static T ReadJsonFile<T>(string filePath)
    {
        try
        {
            StreamReader reader = new StreamReader(filePath);
            string json = reader.ReadToEnd();
            reader.Close();
            return JsonToObject<T>(json);
        }
        catch (Exception e)
        {
            throw new Exception($"Failed to read file {filePath}.  {e.Message}");
        }
    }

    public static void WriteJsonFile<T>(T obj, string filePath, bool ignoreNulls = false)
    {
        try
        {
            if (File.Exists(filePath))
                File.Delete(filePath);
            string json = ObjectToJson(obj, ignoreNulls, true);
            StreamWriter writer = new StreamWriter(filePath, true);
            writer.WriteLine(json);
            writer.Close();
        }
        catch (Exception e)
        {
            throw new Exception($"Failed to write file {filePath}.  {e.Message}");
        }
    }

    public static T JsonToObject<T>(string jsonString)
    {
        try
        {
            return JsonConvert.DeserializeObject<T>(jsonString);
        }
        catch (Exception e)
        {
            throw new Exception($"Couldn't convert json to object: {e.Message}.\n{jsonString}");
        }
    }

    public static string ObjectToJson<T>(T obj, bool ignoreNulls = false, bool indented = false)
    {
        JsonSerializerSettings settings = new JsonSerializerSettings()
        {
            NullValueHandling = ignoreNulls ? NullValueHandling.Ignore : NullValueHandling.Include,
            Formatting = indented ? Formatting.Indented : Formatting.None,
            ReferenceLoopHandling = ReferenceLoopHandling.Ignore
        };
        return JsonConvert.SerializeObject(obj, settings);
    }
}

