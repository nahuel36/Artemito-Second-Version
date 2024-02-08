using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
public static class FileUtils {

    public static List<string> GetDirList(string path, bool excludeSpecialFolders = false)
    {
        List<string> dirs = new List<string>();

        foreach (string dir in Directory.GetDirectories(path))
        {
            DirectoryInfo info = new DirectoryInfo(dir);
            dirs.Add(info.Name);
        }
        if (excludeSpecialFolders)
        {
            dirs.Remove("Editor");
            dirs.Remove("Gizmos");
            dirs.Remove("Plugins");
            dirs.Remove("Resources");
            dirs.Remove("StreamingAssets");
        }

        return dirs;
    }

    public static List<string> GetFilesList(string path)
    {
        List<string> dirs = new List<string>();

        DirectoryInfo info = new DirectoryInfo(path);

        foreach (var file in info.GetFiles())
        {
            dirs.Add(file.Name);
        }

        return dirs;
    }
}
