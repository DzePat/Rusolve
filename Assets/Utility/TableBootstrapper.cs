using System.IO;
using System.Collections;
using UnityEngine;
using UnityEngine.Networking;


public class TableBootstrapper : MonoBehaviour
{
    public static bool TablesCopied { get; private set; } = false;

    /// <summary>
    /// Copies all tables to a path where it can be used by sytem.io filereader
    /// </summary>
    IEnumerator CopyAllTables()
    {
        string[] tableFiles = {
        //other folder
        "KociembaSolver/tables/other/combi",
        "KociembaSolver/tables/other/ep2_merge",
        //Move folder
        "KociembaSolver/tables/move/co_move",
        "KociembaSolver/tables/move/cp_move",
        "KociembaSolver/tables/move/ds_move",
        "KociembaSolver/tables/move/eo_move",
        "KociembaSolver/tables/move/ep2_move",
        "KociembaSolver/tables/move/ud_move",
        "KociembaSolver/tables/move/uds_move",
        "KociembaSolver/tables/move/us_move",
        //prune folder
        "KociembaSolver/tables/prune/co_ud_prun",
        "KociembaSolver/tables/prune/cp_ud2_prun",
        "KociembaSolver/tables/prune/eo_ud_prun",
        "KociembaSolver/tables/prune/ep2_ud2_prun",
        };

        foreach (var file in tableFiles)
        {
            yield return FileHelper.CopyFileToPersistentDataPath(file);
        }

        TablesCopied = true;
        Debug.Log("All tables copied");
    }

    private void Start()
    {
        StartCoroutine(CopyAllTables());
    }


    /// <summary>
    /// Copie a table file to persistentDataPath
    /// </summary>
    public static class FileHelper
    {
        public static IEnumerator CopyFileToPersistentDataPath(string relativePath)
        {

            string sourcePath = Path.Combine(Application.streamingAssetsPath, relativePath);
            string destPath = Path.Combine(Application.persistentDataPath, relativePath);

            // Create directory if not exists
            string destDir = Path.GetDirectoryName(destPath);
            if (!Directory.Exists(destDir)) 
            { 
                Directory.CreateDirectory(destDir);
            }

            // If file already exists, skip copying
            if (File.Exists(destPath))
                yield break;

            UnityWebRequest www = UnityWebRequest.Get(sourcePath);
            yield return www.SendWebRequest();

            if (www.result == UnityWebRequest.Result.Success)
            {
                File.WriteAllBytes(destPath, www.downloadHandler.data);
                Debug.Log($"Copied {relativePath} to persistentDataPath.");
            }
            else
            {
                Debug.LogError($"Failed to copy {relativePath}: {www.error}");
            }
        }
    }
}
