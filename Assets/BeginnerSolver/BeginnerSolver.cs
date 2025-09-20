using System;
using System.Collections.Generic;
using System.Runtime.InteropServices.WindowsRuntime;
using UnityEditor.Animations;
using UnityEngine;

namespace BeginnerSolve
{
    public static class Search
    {
        public static string StartSearch(Dictionary<Vector3Int, GameObject> cubeState)
        {
            if (isWhiteCrossSolved(cubeState))
            {
                return "";
            }
            else
            {
                GameObject cubelet = findWhiteEdge(cubeState);
                return getEdgeRotation(cubelet);
            }
        }

        static Dictionary<string, Vector3> edgeTargetPositions = new () { 
            {"Sticker_green",new Vector3(0,1,-1) },
            {"Sticker_orange", new Vector3(-1,1,0)},
            {"Sticker_blue", new Vector3(0,1,1)},
            {"Sticker_red", new Vector3(1,1,0) },
        };

        /// <summary>
        /// Takes a cubelet and returns rotations needed for it to get to the right position and orientation
        /// </summary>
        /// <param name="cubelet"></param>
        /// <returns></returns>
        static string getEdgeRotation(GameObject cubelet)
        {
            string topRotation = "";
            string rotations = "";
            foreach (Transform sticker in cubelet.transform) {
                if (sticker.name == "Sticker_red") topRotation = "U";
                else if (sticker.name == "Sticker_blue") topRotation = "U U";
                else if (sticker.name == "Sticker_orange") topRotation = "U U U";
            }
            Vector3 stickerPos = findWhiteStickerPosition(cubelet);
            int posX = Mathf.RoundToInt(stickerPos.x);
            int posY = Mathf.RoundToInt(stickerPos.y);
            int posZ = Mathf.RoundToInt(stickerPos.z);
            //white edge in top layer side position (wrong orientation)
            if (posY == 1)
            {
                if (posZ == 2) rotations += "U U F U' R U ";
                else if (posZ == 0) rotations += "U F U' R U ";
                else rotations += "F U' R U ";
                rotations += topRotation;
            }
            //White edge in middle layer
            else if (posY == 0)
            {
                rotations += topRotation;
                if (posZ == 2) rotations += " B D' D' B F' U' R U ";
                else if (posZ == 1 && posX > 0) rotations += " B D' D' B F' U' R U F U' R U ";
                else if (posZ == 1 && posX < 0) rotations += " B' D' D' B F' U' R U F U' R U "; 
                else if (posZ == -1 && posX > 0) rotations += " U' R U F U' R U "; 
                else if (posZ == -1 && posX < 0) rotations += " U L' U F U' R U "; 
                else if (posZ == -2 && posX > 0) rotations += " U' R U "; 
                else if (posZ == -2 && posX < 0) rotations += " U L' U "; 
                rotations += topRotation;
            }
            // white edge bottom layer side position
            else if (posY == -1)
            {
                rotations += topRotation;
                if (posZ == 2) rotations += "D' D' F' U' R U ";
                else if (posZ == 0 && posX > 0) rotations += "D' F' U' R U F U' R U ";
                else if (posZ == 0 && posX < 0) rotations += "D F' U' R U F U' R U ";
                else { rotations += " F' U' R U F U' R U "; }
                rotations += topRotation;
            }
            // white edge bottom layer bottom side
            else if (posY == -2)
            {
                rotations += topRotation;
                if (posZ == 1) rotations += " D' D' F' U' R U F U' R U ";
                else if (posZ == 0 && posX > 0) rotations += " D' F' U' R U F U' R U ";
                else if (posZ == 0 && posX < 0) rotations +=  " D F' U' R U F U' R U ";
                else rotations += " F' U' R U F U' R U ";
                rotations += topRotation;
            }
            return rotations;
        }

        public static bool isWhiteCrossSolved(Dictionary<Vector3Int, GameObject> cubeState)
        {
            bool control = true;
            foreach(string edgeSticker in edgeTargetPositions.Keys)
            {
                GameObject cubelet = cubeState[Vector3Int.RoundToInt(edgeTargetPositions[edgeSticker])];
                if(cubelet.transform.childCount == 2)
                {
                    foreach(Transform sticker in cubelet.transform)
                    {
                        if(sticker.name != edgeSticker && sticker.name != "Sticker_white")
                        {
                            control = false;
                        }
                    }
                }
            }
            return control;
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


        public static GameObject findWhiteEdge(Dictionary<Vector3Int, GameObject> cubeState)
        {
            foreach (string edgeSticker in edgeTargetPositions.Keys)
            {
                foreach (GameObject cubelet in cubeState.Values)
                {
                    if (cubelet.transform.childCount == 2)
                    {
                        bool hasWhiteSticker = false;
                        bool hasChoiceSticker = false;
                        foreach (Transform sticker in cubelet.transform)
                        {
                            if (sticker.name == "Sticker_white" && sticker.transform.position.y <= 1)
                            {
                                hasWhiteSticker = true;
                            }else if(sticker.name == edgeSticker){
                                hasChoiceSticker= true;
                            }
                            if (hasWhiteSticker && hasChoiceSticker)
                            {
                                return cubelet;
                            }
                        }
                    }
                }
            }
            return null;
        }

    }
}

