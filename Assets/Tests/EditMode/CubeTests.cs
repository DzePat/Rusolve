using NUnit.Framework;
using System.Collections;
using UnityEngine;
using UnityEngine.TestTools;

public class Cube
{
    [Test]
    public void CreateCubeManagerTest()
    {
        GameObject GameObj = new GameObject();
        CubeManager manager = GameObj.AddComponent<CubeManager>();
        Assert.AreNotEqual(manager, null);
    }

    [Test]
    public void CreateCubeletTest()
    {
        GameObject GameObj = new GameObject();
        CubeManager manager = GameObj.AddComponent<CubeManager>();
        Assert.AreNotEqual(manager, null);
        GameObject cubelet = manager.CreateCubelet(1, 1, 1);
        Vector3Int target = new Vector3Int(1,1,1);
        Vector3Int result = Vector3Int.RoundToInt(cubelet.transform.position);
        Assert.AreEqual(target,result);
    }

}
