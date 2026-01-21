using System;
using System.Collections.Generic;
using System.IO;
using Newtonsoft.Json;
using UnityEngine;

[Serializable]
public class OggFileInfo
{
    public string name;
    public long size;
    public long mtime;
    public string relativePath;
}

[Serializable]
public class OggFileList
{
    public List<OggFileInfo> files = new List<OggFileInfo>();
}

public class DirectoryItem
{
    public string name;
    public bool isDir;
    public long? size;
    public long mtime;
    public string relativePath;
    public List<DirectoryItem> children;
}

public class DirectoryStructure
{
    public List<DirectoryItem> items;
}

public class OggFileSerializer : MonoBehaviour
{
    [SerializeField] private string jsonFilePath = "Assets/dir.json";
    [SerializeField] private string outputFilePath = "Assets/ogg_files.json";

    public OggFileList SerializeOggFiles()
    {
        if (!File.Exists(jsonFilePath))
        {
            Debug.LogError($"File not found: {jsonFilePath}");
            return null;
        }

        string jsonContent = File.ReadAllText(jsonFilePath);
        DirectoryStructure dirStructure = JsonConvert.DeserializeObject<DirectoryStructure>(jsonContent);

        if (dirStructure == null || dirStructure.items == null)
        {
            Debug.LogError("Failed to parse directory structure");
            return null;
        }

        OggFileList oggFileList = new OggFileList();

        foreach (var item in dirStructure.items)
        {
            ExtractOggFiles(item, oggFileList.files);
        }

        Debug.Log($"Found {oggFileList.files.Count} ogg files");
        return oggFileList;
    }

    private void ExtractOggFiles(DirectoryItem item, List<OggFileInfo> oggFiles)
    {
        if (item == null) return;

        if (!item.isDir && item.name.EndsWith(".ogg", StringComparison.OrdinalIgnoreCase))
        {
            oggFiles.Add(new OggFileInfo
            {
                name = item.name,
                size = item.size ?? 0,
                mtime = item.mtime,
                relativePath = item.relativePath
            });
        }

        if (item.children != null)
        {
            foreach (var child in item.children)
            {
                ExtractOggFiles(child, oggFiles);
            }
        }
    }

    public void SaveToFile()
    {
        OggFileList oggFileList = SerializeOggFiles();

        if (oggFileList == null)
        {
            return;
        }

        string json = JsonUtility.ToJson(oggFileList, true);
        File.WriteAllText(outputFilePath, json);
        Debug.Log($"Saved ogg files list to: {outputFilePath} oggFileList count = {oggFileList.files.Count}");
    }

    [ContextMenu("Serialize Ogg Files")]
    public void SerializeAndSave()
    {
        SaveToFile();
    }
}
