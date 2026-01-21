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

public class SoundDirFileDeSerializer
{
    public static OggFileList Deserialize(string str)
    {
        OggFileList list = new OggFileList();

        DirectoryStructure dirStructure = JsonConvert.DeserializeObject<DirectoryStructure>(str);
        if (dirStructure == null || dirStructure.items == null)
        {
            Debug.LogError("Failed to parse directory structure");
            return list;
        }

        foreach (var item in dirStructure.items)
        {
            ExtractOggFiles(item, list.files);
        }

        return list;
    }

    private static void ExtractOggFiles(DirectoryItem item, List<OggFileInfo> oggFiles)
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
}