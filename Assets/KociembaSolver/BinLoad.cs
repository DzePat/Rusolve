using System;
using System.IO;
using UnityEngine;
using UnityEngine.Networking;

namespace TwoPhaseSolver
{
    static class BinLoad
    {
        static string streamingAssetsPath = Application.streamingAssetsPath;
        public static string GetFilePath(string relativePath)
        {
            #if UNITY_ANDROID && !UNITY_EDITOR
                // On Android device, read from persistentDataPath
                return Path.Combine(Application.persistentDataPath, relativePath);
            #else
                // On PC and editor, read directly from StreamingAssets folder
                return Path.Combine(Application.streamingAssetsPath, relativePath);
            #endif
        }

        public static byte[][] getUdToPerm(string path)
        {
            var fullPath = GetFilePath(path);
            var raw = File.OpenRead(fullPath);
            byte[][] values = new byte[Constants.N_UD][];
            byte[] c;

            for (var i = 0; i < Constants.N_UD; i++)
            {
                c = new byte[2];
                raw.Read(c, 0, 2);
                
                values[i] = new byte[4]
                {
                    (byte)((c[0] & 0xf0) >> 4),
                    (byte)(c[0] & 0x0f),
                    (byte)((c[1] & 0xf0) >> 4),
                    (byte)(c[1] & 0x0f)
                };
            }

            raw.Close();
            return values;
        }

        public static ushort[,] loadShortTable2D(string path, int chunksize = 18)
        {
            var fullPath = GetFilePath(path);
            var bytes = File.ReadAllBytes(fullPath);
            int len1d = bytes.Length / chunksize / 2;
            ushort[,] values = new ushort[len1d, chunksize];
            int i, j;
            
            for (i = 0; i < len1d; i++)
            {
                for (j = 0; j < chunksize; j++)
                {
                    values[i, j] = (ushort)(
                        (bytes[(chunksize * i + j) * 2] << 8) + 
                        bytes[(chunksize * i + j) * 2 + 1]
                    );
                }
            }

            return values;
        }

        public static PruneTable loadPruneTable(string path)
        {
            var fullPath = GetFilePath(path);
            return new PruneTable(File.ReadAllBytes(fullPath));
        }
    }
}
