using System.IO;
using System.Text;
using UnityEngine;

public class DataManager : MonoBehaviour
{
    public static string ObjectToJson(object obj)
    {
        return JsonUtility.ToJson(obj); 
    }

    public static T JsonToObject<T>(string jsonData)
    {
        return JsonUtility.FromJson<T>(jsonData);
    }

    public static void CreateJsonFile(string createPath, string fileName, string path, string jsonData)
    {
        FileStream fileStream = new FileStream(string.Format("{0}" + path + "/{1}.json", createPath, fileName), FileMode.Create);
        byte[] data = Encoding.UTF8.GetBytes(jsonData);
        fileStream.Write(data, 0, data.Length);
        fileStream.Close();
    }

    public static T LoadJsonFile<T>(string loadPath, string fileName, string path)
    {
        FileStream fileStream = new FileStream(string.Format("{0}" + path + "/{1}.json", loadPath, fileName), FileMode.Open);
        byte[] data = new byte[fileStream.Length];
        fileStream.Read(data, 0, data.Length);
        fileStream.Close();
        string jsonData = Encoding.UTF8.GetString(data);
        return JsonUtility.FromJson<T>(jsonData);
    }
}
