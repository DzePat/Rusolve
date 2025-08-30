using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ButtonController : MonoBehaviour
{
    public ButtonManager buttonManager;
    public SolveController solveController;

    private string[] solution;
    private int solutionIndex = 0;

    public void OnButtonClicked(GameObject buttonObj)
    {
        if (!solveController.cubeController.isRotating)
        {
            Button button = buttonObj.GetComponent<Button>();
            if (button.name == "Solve")
            {
                Destroy(buttonObj);
                solveController.cubeController.DisableStickerClick();
                buttonManager.CreateSolveNavigationButtons();
                solution = solveController.KociembaSolveAlgorithm();
                solveController.populateMoveMap();
            }
            if (button.name == "Solve_With_Kociemba")
            {
                Destroy(buttonObj);
                solveController.cubeController.cubeManager.BuildCube();
                buttonManager.CreateStartSolveButton();
            }
            if (button.name == "Next")
            {
                if (solutionIndex != solution.Length)
                {
                    string step = solution[solutionIndex];
                    Debug.Log($"step: {step} length: {step.Length}");
                    List<Vector3Int> face = solveController.moveMap[step[0]];
                    if (step.Length == 1)
                    {
                        StartCoroutine(solveController.cubeController.RotateFace(face, true));
                    }
                    else
                    {
                        if (step[1] == '2')
                        {
                            StartCoroutine(solveController.cubeController.RotateFace(face, true));
                            StartCoroutine(solveController.cubeController.RotateFace(face, true));
                        }
                        else
                        {
                            StartCoroutine(solveController.cubeController.RotateFace(face, false));
                        }
                    }
                    solutionIndex++;
                }
            }
            if (button.name == "Previous")
            {
                if (solutionIndex > 0) solutionIndex--;

            }
        }
    }

}
