using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CameraTest : MonoBehaviour
{

    //摄像头图像类，继承自texture
    WebCamTexture tex;
  //  public Image WebCam;
    public MeshRenderer ma;
    //public Button saveImage;

    void Start()
    {
        //开启协程，获取摄像头图像数据
        StartCoroutine(OpenCamera());
      //  saveImage.onClick.AddListener(SaveImage);
    }

    // Update is called once per frame
    void Update()
    {

    }
    IEnumerator OpenCamera()
    {
        //等待用户允许访问
        yield return Application.RequestUserAuthorization(UserAuthorization.WebCam);
        //如果用户允许访问，开始获取图像        
        if (Application.HasUserAuthorization(UserAuthorization.WebCam))
        {
            //先获取设备
            WebCamDevice[] device = WebCamTexture.devices;

            string deviceName = device[0].name;
            //然后获取图像
            tex = new WebCamTexture(deviceName);
            //将获取的图像赋值
            ma.material.mainTexture = tex;
            //开始实施获取
            tex.Play();

        }
    }

}