/*************************************************************************
 *****开发人员     :     #朝阳# 
 *****修改日期     :     #DATE# 
 *****描述信息     :     
*************************************************************************/

using UnityEngine;
using TencentYoutuYun.SDK.Csharp;
using UnityEngine.UI;
using System.Collections.Generic;

public class FacialRecognition : MonoBehaviour {

    private static FacialRecognition instance;
    public static FacialRecognition Instance {
        get {
            return instance;
        } 
    }

    public Text textLog;
    public RawImage rawImage;
    public string template= "cf_yuren_cungu";

    private void Awake()
    {
        instance=this;
    }

    void Start () {
        // 设置为你自己的密钥对
        string appid = "10126488";
        string secretId = "AKID1qSt9Cce18zbyAHnFRN4ZppNjrk0hsTu";
        string secretKey = "PceU9anlV1JNErGRN6btwmNktnTxDDzW";
        string userid = "1218207883";

        Conf.Instance().setAppInfo(appid, secretId, secretKey, userid, Conf.Instance().YOUTU_END_POINT);



        string path =
#if UNITY_ANDROID
            Application.streamingAssetsPath + "/test.jpg";
#else
        "file:///"+Application.streamingAssetsPath+ "/test.jpg";
#endif
        if (Application.platform == RuntimePlatform .WindowsEditor)
        {
        YoutuFunc(Application.streamingAssetsPath + "/test.jpg");
        }
    }

    public void YoutuFunc(string path) {

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
            result = Youtu.faceMerge(path, "base64", "[{\"cmd\":\"doFaceMerge\",\"params\":{\"model_id\":\"" + template + "\"}}]");

            ShowRawimage(result, rawImage);
        }
        catch (System.Exception e)
        {
            textLog.text = e.Message;
        }

        List<string> group_ids = new List<string>();
        result = Youtu.multifaceidentify(path, "test", group_ids, 5, 40);

        JsonParse.Multifaceidentify mu=JsonParse.Multifaceidentify.ParseMultifaceidentify(result);

        textLog.text += "\n面部坐标:"+ mu.results[0].face_rect.x.ToString()+";"+ mu.results[0].face_rect.y;
        textLog.text += "\n面部宽高:"+ mu.results[0].face_rect.width.ToString()+";"+ mu.results[0].face_rect.height;
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            Application.Quit();
        }
    }

    void ShowRawimage(string json,RawImage raw) {
        JsonParse.FaceMerge faceMerge = JsonParse.FaceMerge.ParseJsonFaceMerge(json);
        if (faceMerge.img_base64!=null)
        {
            raw.texture = SCY.Utility.Base64Img(faceMerge.img_base64);
            raw.SetNativeSize();
        }
        else if (faceMerge.img_url!=null)
        {
            Debug.Log("图片地址");
            Application.OpenURL(faceMerge.img_url);
        }
        textLog.text += faceMerge.msg;
    }


    void LoadTextFromBase64(string base64,RawImage rawImage) {

        Texture2D pic = new Texture2D(500, 500);
        byte[] data = System.Convert.FromBase64String(base64);
        pic.LoadImage(data);

        rawImage.texture = pic;
        rawImage.SetNativeSize();
        rawImage.transform.localScale = Vector3.one * 0.3f;
    }
}
