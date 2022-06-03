using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using LitJson;
using System.IO;

public class GameDataManager : MonoBehaviour
{
    public static void LoadGameData()
    {
		string filePath = Application.dataPath + "/Data/test.json";
		if (File.Exists(filePath))
		{
			StreamReader sr = new StreamReader(filePath);
			string json = sr.ReadToEnd();
			sr.Close();

			Dictionary<string, Dictionary<string, string>> data
				= JsonMapper.ToObject<Dictionary<string, Dictionary<string, string>>>(json);
			if (data == null) return;
			foreach (var i in data)
			{
				long key = long.Parse(i.Key);
				ModifyBlock.change.Add(key, new Dictionary<int, string>());
				foreach (var j in i.Value)
					ModifyBlock.change[key].Add(int.Parse(j.Key), j.Value);
			}
		}
    }
    public static void SaveGameData()
    {
		Dictionary<string, Dictionary<string, string>> data
			= new Dictionary<string, Dictionary<string, string>>();
		foreach (var i in ModifyBlock.change)
		{
			string key = i.Key.ToString();
			data.Add(key, new Dictionary<string, string>());
			foreach (var j in i.Value)
				data[key].Add(j.Key.ToString(), j.Value);
		}

		string filePath = Application.dataPath + "/Data/test.json";
		string json = JsonMapper.ToJson(data);
		StreamWriter sw = new StreamWriter(filePath);
		sw.Write(json);
		sw.Close();
    }
}
