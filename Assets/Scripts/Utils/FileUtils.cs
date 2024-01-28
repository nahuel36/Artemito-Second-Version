using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
public static class FileUtils {

    public static List<string> GetDirList(string path)
    {
        List<string> dirs = new List<string>();

        foreach (string dir in Directory.GetDirectories(path))
        {
            DirectoryInfo info = new DirectoryInfo(dir);
            dirs.Add(info.Name);
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
