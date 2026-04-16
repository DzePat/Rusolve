using NUnit.Framework;
using UnityEngine;


public class Cube
{
    private CubeManager CreateManager(out GameObject obj)
    {
        obj = new GameObject();
        return obj.AddComponent<CubeManager>();
    }

    [Test]
    public void CreateCubeManagerTest()
    {
        GameObject obj = new GameObject();
        CubeManager manager = obj.AddComponent<CubeManager>();
        Assert.IsNotNull(manager, "CubeManager is null");
        Object.DestroyImmediate(obj);
    }

    [Test]
    public void CreateCubeletTest()
    {
        GameObject obj = new GameObject();
        CubeManager manager = obj.AddComponent<CubeManager>();
        //Create white corner cubelet where x,y,z = 1,1,1
        GameObject cubelet = manager.CreateCubelet(1, 1, 1);
        Assert.IsNotNull(cubelet, "Cubelet GameObject is missing");

        //Check if coordinates match
        Assert.AreEqual(new Vector3(1, 1, 1), cubelet.transform.position);

        Transform white_sticker = cubelet.transform.Find("Sticker_white");
        Assert.IsNotNull(white_sticker, "White sticker not found");
        Color colorone = white_sticker.GetComponent<Renderer>().sharedMaterial.color;
        //cubelet has a white sticker
        Assert.AreEqual(Color.white, colorone);
        
        Transform red_sticker = cubelet.transform.Find("Sticker_red");
        Assert.IsNotNull(red_sticker, "Red sticker not found");
        Color colortwo = red_sticker.GetComponent<Renderer>().sharedMaterial.color;
        //cubelet has a red sticker
        Assert.AreEqual(Color.red, colortwo);
        
        Transform blue_sticker = cubelet.transform.Find("Sticker_blue");
        Assert.IsNotNull(blue_sticker, "Blue sticker not found");
        Color colorthree = blue_sticker.GetComponent<Renderer>().sharedMaterial.color;
        //cubelet has a blue sticker
        Assert.AreEqual(Color.blue, colorthree);
        Object.DestroyImmediate(obj);
    }

    [Test]
    public void CreateCubeTest()
    {
        GameObject obj = new GameObject();
        CubeManager manager = obj.AddComponent<CubeManager>();
        manager.BuildCube();
        Assert.IsNotEmpty(manager.cubeletMap, "Cubelet list is empty");
        //all 27 cubelets have been created and added to the list
        Assert.AreEqual(27, manager.cubeletMap.Count);
        Object.DestroyImmediate(obj);
    }

}
