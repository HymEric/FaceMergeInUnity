using UnityEngine;
using System.Collections;
using System.IO;
using UnityEngine.UI;

public class testSin : MonoBehaviour
{
    public GUISkin skin;
    Texture texture;
    public Text text;

    void Update()
    {

        if (Input.GetKeyDown(KeyCode.Escape) || Input.GetKeyDown(KeyCode.Home))
        {
            Application.Quit();
        }
    }

    void OnGUI()
    {
        GUI.skin = skin;

        if (GUILayout.Button("打开手机相册"))
        {
            text.text = "相册";
        }
        if (GUILayout.Button("打开手机摄像机"))
        {

            text.text = "照相机";
        }
       
    }


    IEnumerator LoadTexture(string name)
    {
        //注解1
        string path = "file://" + Application.persistentDataPath + "/" + name;

        WWW www = new WWW(path);
        while (!www.isDone)
        {

        }
        yield return www;
        //为贴图赋值
        texture = www.texture;
    }
}