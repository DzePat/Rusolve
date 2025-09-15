using System.Collections.Generic;
using System.Runtime.InteropServices.WindowsRuntime;
using UnityEngine;

namespace BeginnerSolve
{
    public static class Search
    {
        static Dictionary<string, Vector3> edgeTargetPositions = new () { 
            {"Sticker_green",new Vector3(0,1,-1) },
            {"Sticker_red", new Vector3(1,1,0) },
            {"Sticker_blue", new Vector3(0,1,1)},
            {"Sticker_orange", new Vector3(-1,1,0)},
        };


        static string[] WhiteCross(Dictionary<Vector3Int, GameObject> cube)
        {
            return null;
        }

        static string[] getEdgeRotation(GameObject cubelet)
        {
            if(cubelet == null) return null;
            Vector3 stickerPos = findWhiteStickerPosition(cubelet);
            if(stickerPos.y < 0)
            {

            }
            return null;

        }

        static Vector3 findWhiteStickerPosition(GameObject cubelet)
        {
            foreach (Transform sticker in cubelet.transform)
            {
                if(sticker.name == "Sticker_white")
                {
                    return sticker.position;
                }
            }
            return new(0,0,0);
        }


        public static GameObject findWhiteEdge(Dictionary<Vector3Int, GameObject> cube)
        {
            foreach (GameObject cubelet in cube.Values)
            {
               if(cubelet.transform.childCount == 2)
                {
                    foreach(Transform sticker in cubelet.transform)
                    {
                        if(sticker.name ==  "Sticker_white" && sticker.transform.position.y < 1)
                        {
                            return cubelet;
                        }
                    }
                }
            }
            return null;
        }

    }
}

