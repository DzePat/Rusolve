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
                MoveNext();
            }
            if (button.name == "Previous")
            {
                MovePrevious();
            }
        }
    }

    public void MoveNext()
    {
        if (solutionIndex != solution.Length)
        {
            string step = solution[solutionIndex];
            List<Vector3Int> face = solveController.moveMap[step[0]];
            if (step.Length == 1)
            {
                solveController.cubeController.EnqueueRotation(face, true);
            }
            else
            {
                if (step[1] == '2')
                {
                    solveController.cubeController.EnqueueRotation(face, true);
                    solveController.cubeController.EnqueueRotation(face, true);
                }
                else
                {
                    solveController.cubeController.EnqueueRotation(face, false);
                }
            }
            solutionIndex++;
        }
    }

    public void MovePrevious()
    {
       if(solutionIndex != 0)
        {
            solutionIndex--;
            string step = solution[solutionIndex];
            List<Vector3Int> face = solveController.moveMap[step[0]];
            if (step.Length == 1)
            {
                solveController.cubeController.EnqueueRotation(face, false);
            }
            else
            {
                if (step[1] == '2')
                {
                    solveController.cubeController.EnqueueRotation(face, false);
                    solveController.cubeController.EnqueueRotation(face, false);
                }
                else
                {
                    solveController.cubeController.EnqueueRotation(face, true);
                }
            }
        }
    }

}
