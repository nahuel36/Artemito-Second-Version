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

}
