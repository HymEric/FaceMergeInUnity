
using UnityEngine;
using TencentYoutuYun.SDK.Csharp;
using UnityEngine.UI;
using System.Collections.Generic;

using System.IO;
using System.Collections;

public class MyFacialRec : MonoBehaviour
{
    private string imgName = "crop1.jpg";
    private static MyFacialRec instance;
    public static MyFacialRec Instance
    {
        get
        {
            return instance;
        }
    }

    public RawImage rawImage;
    public string template = "cf_yuren_cungu";
 
    private void Awake()
    {
        instance = this;
    }

    void Start()
    {
        // 设置为你自己的密钥对
        string appid = "10143323";
        string secretId = "AKIDsswvy7sjL1nXUXco3Mp8b9nWJAZFs7X4";
        string secretKey = "q9aAFRyusujBlO5tnxy6qddho0mhks5O";
        string userid = "951523291";

        Conf.Instance().setAppInfo(appid, secretId, secretKey, userid, Conf.Instance().YOUTU_END_POINT);

       
    }

    public void faceMerg()
    {
        string path =
#if UNITY_ANDROID
            "file:///mnt/sdcard/Android/data/com.erichym.myunitylib/files/" + imgName;
#else
        "file:///"+Application.streamingAssetsPath+ "/test.jpg";
#endif
        if (Application.platform == RuntimePlatform.WindowsEditor)
        {
            //YoutuFunc(Application.streamingAssetsPath + "/test.jpg");
            YoutuFunc(Application.streamingAssetsPath + "/test.jpg");
        }
        else if (Application.platform == RuntimePlatform.Android)
        {
            YoutuFunc(path);
        }
    }
    byte[] mybytes;
    IEnumerator loadWWW(string url)
    {

        WWW www = new WWW(url);

        yield return www;

        if (www.isDone)
        {
            Debug.Log("下载完成");
            mybytes = www.bytes;
            Debug.Log("**");
            Debug.Log(mybytes.ToString());
        }
    }

    public void YoutuFunc(string path)
    {

        //// 人脸对比
        //result = Youtu.facecompare(path, path2);
        //print(result);

        //// 人脸关键点定位 调用demo
        //result = Youtu.faceshape(path);
        //print(result);

        //result = Youtu.getpersonids("group");
        //print(result);

        //// 名片OCR
        //path = System.IO.Directory.GetCurrentDirectory() + "\\ocr_card_01.jpg";
        //result = Youtu.bcocr(path);
        //print(result);

        //// 通用OCR
        //result = Youtu.generalocrurl("http://open.youtu.qq.com/app/img/experience/char_general/ocr_common01.jpg");
        //print(result);

        //// 行驶证OCR
        //path = System.IO.Directory.GetCurrentDirectory() + "\\ocr_xsz_01.jpg";
        //result = Youtu.driverlicenseocr(path, 0);
        //print(result);

        ////多人脸检索
        //List<string> group_ids = new List<string>();
        //result = Youtu.multifaceidentifyurl("http://open.youtu.qq.com/app/img/experience/face_img/face_05.jpg?v=1.0", "test", group_ids, 5, 40);
        //print(result);

        /////识别一个图像是否为暴恐图像
        //result = Youtu.imageterrorismurl("http://open.youtu.qq.com/app/img/experience/terror/img_terror01.jpg");
        //print(result);

        /////自动地检测图片车身以及识别车辆属性
        //result = Youtu.carcalssifyurl("http://open.youtu.qq.com/app/img/experience/car/car_01.jpg");
        //print(result);

        /////银行卡OCR识别，根据用户上传的银行卡图像，返回识别出的银行卡字段信息。
        //result = Youtu.creditcardocrurl("http://open.youtu.qq.com/app/img/experience/char_general/ocr_card_1.jpg");
        //print(result);

        /////营业执照OCR 识别，根据用户上传的营业执照图像，返回识别出的注册号、公司名称、地址字段信息
        //result = Youtu.bizlicenseocrurl("http://open.youtu.qq.com/app/img/experience/char_general/ocr_yyzz_01.jpg");
        //print(result);

        /// 车牌OCR识别，根据用户上传的图像，返回识别出的图片中的车牌号。
        //result = Youtu.plateocrurl("http://open.youtu.qq.com/app/img/experience/char_general/ocr_license_1.jpg");
        //print(result);

        /// 人脸融合，根据用户上传的图像，返回融合后的图像。
        string result = string.Empty;
        try
        {
            Debug.Log("hym");
            StartCoroutine(loadWWW(path));
            Debug.Log(mybytes.ToString());
            //result = Youtu.faceMerge(path, "base64", "[{\"cmd\":\"doFaceMerge\",\"params\":{\"model_id\":\"" + template + "\"}}]");
            result = Youtu.faceMergeFrombytes(mybytes, "base64", "[{\"cmd\":\"doFaceMerge\",\"params\":{\"model_id\":\"" + template + "\"}}]");
            ShowRawimage(result, rawImage);
        }
        catch (System.Exception e)
        {
            
        }

        //List<string> group_ids = new List<string>();
        //result = Youtu.multifaceidentify(path, "test", group_ids, 5, 40);

        //JsonParse.Multifaceidentify mu = JsonParse.Multifaceidentify.ParseMultifaceidentify(result);

    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            Application.Quit();
        }
    }

    void ShowRawimage(string json, RawImage raw)
    {
        JsonParse.FaceMerge faceMerge = JsonParse.FaceMerge.ParseJsonFaceMerge(json);
        if (faceMerge.img_base64 != null)
        {
            raw.texture = SCY.Utility.Base64Img(faceMerge.img_base64);
            raw.SetNativeSize();
        }
        else if (faceMerge.img_url != null)
        {
            Debug.Log("图片地址");
            Application.OpenURL(faceMerge.img_url);
        }
    }


    void LoadTextFromBase64(string base64, RawImage rawImage)
    {

        Texture2D pic = new Texture2D(500, 500);
        byte[] data = System.Convert.FromBase64String(base64);
        pic.LoadImage(data);

        rawImage.texture = pic;
        rawImage.SetNativeSize();
        rawImage.transform.localScale = Vector3.one * 0.3f;
    }

  
}
