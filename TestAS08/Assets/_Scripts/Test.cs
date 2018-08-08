using UnityEngine;
using System.Collections;
using System.IO;
using UnityEngine.UI;

public class Test : MonoBehaviour
{
//    public Button loadCameraBtn;
//    public Button loadAlbumBtn;
    private AndroidJavaObject jo = null;
    public Text text;
    public Text text2;
    private string imgName = "crop1.jpg";
    public RawImage rawImage;
    //AndroidJavaObject _ajc;
    void Start()
    {
        //这里是固定的写法
        //AndroidJavaClass jc = new AndroidJavaClass("com.erichym.myunitylib.WebViewActivity");
       // jo = jc.GetStatic<AndroidJavaObject>("currentActivity");//获取当前的Activity对象，即Android中的MainActivity

        //canvasTras = GameObject.Find("Canvas").transform;
        //text = canvasTras.Find("Text").GetComponent<Text>();
        //btn = canvasTras.Find("Button").GetComponent<Button>();
        //btn.onClick.AddListener(Click);//按钮点击事件监听下面的Click()方法

        //通过该API来实例化导入的arr中对应的类
        jo = new AndroidJavaObject("com.erichym.myunitylib.UnityTestActivity");
    }

    void Update()
    {

        if (Input.GetKeyDown(KeyCode.Escape) || Input.GetKeyDown(KeyCode.Home))
        {
            Application.Quit();
        }
    }

    public void ToastBtnClick()
    {
        jo.Call("showToast", "this is unity");
        text2.text = "unity done method in AS";
    }

    public void loadCamera()
    {
        jo.Call("TakePhoto", "takePhoto");
        string path = "/mnt/sdcard/Android/data/com.erichym.myunitylib/files/" + imgName;
        text.text = path;
        text2.text = "/mnt/sdcard/Android/data/com.erichym.myunitylib/files/" + imgName;
    }

    public void loadAlbum()
    {
       jo.Call("TakePhoto", "takeSave");
        //包名是com.YHKJ.HuaFuXiaoDangJia的时候，path具体是file:///mnt/sdcard/Android/data/com.YHKJ.HuaFuXiaoDangJia/files/crop1.jpg
        //string path = "file://" + Application.persistentDataPath + "/" + imgName;
        string path = "file:///mnt/sdcard/Android/data/com.erichym.myunitylib/files/" + imgName;
        text.text = path;
        text2.text = "/mnt/sdcard/Android/data/com.erichym.myunitylib/files/" + imgName;
    }

    public void loadImage1()
    {
        // string path = "file://" + Application.persistentDataPath + "/" + imgName;
        string path = "file:///mnt/sdcard/Android/data/com.erichym.myunitylib/files/" + imgName;
        // string path = "http://e.hiphotos.baidu.com/exp/w=480/sign=44a29625a5efce1bea2bc9c29f50f3e8/fc1f4134970a304ef1527529d3c8a786c8175cf5.jpg";
        WWW www = new WWW(path);
        while (!www.isDone)
        {

        }
        //为贴图赋值
        rawImage.texture = www.texture;
    }

    public void loadImage2()
    {
        string path = "/mnt/sdcard/Android/data/com.erichym.myunitylib/files/" + imgName;
        WWW www = new WWW(path);
        while (!www.isDone)
        {

        }
        //为贴图赋值
        www.ToString();
        rawImage.texture = www.texture;
    }

    //void OnGUI()
    //{


    //    if (texture != null)
    //    {
    //        //注意！ 我们在这里绘制Texture对象，该对象是通过
    //        //我们制作的Android插件得到的，当这个对象不等于空的时候
    //        //直接绘制。
    //        GUI.DrawTexture(new Rect(100, 300, 300, 300), texture);
    //    }
    //}

    //void messgae(string str)
    //{
    //    //在Android插件中通知Unity开始去指定路径中找图片资源
    //    StartCoroutine(LoadTexture(str));

    //}
    ////crop1.jpg
    //IEnumerator LoadTexture(string name)
    //{
    //    //注解1
    //    string path = "file://" + Application.persistentDataPath + "/" + name;

    //    WWW www = new WWW(path);
    //    while (!www.isDone)
    //    {

    //    }
    //    yield return www;
    //    //为贴图赋值
    //    texture = www.texture;
    //}
}