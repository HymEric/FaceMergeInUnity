using UnityEngine;
using System.Collections;
using System.IO;
using UnityEngine.UI;

public class FaceScreenShot : MonoBehaviour
{
    //public Text showTip;
    // public GameObject screenShotImage;
    public string id;
    bool saved = false;
    //public new AudioSource audio;
    public GameObject UICanvas;

    void Start()
    {
        FaceScreenshotManager.ScreenshotFinishedSaving += ScreenshotSaved;
    }
    public void TakeScreenShot()
    {
        //stateCanvas = State.Normal;
        UICanvas.SetActive(false);
        StartCoroutine(FaceScreenshotManager.Save(id, "facemerer", true)); //保存到相册
       // plane.SetActive(false)
        //audio.Play();
        StartCoroutine(WaitForOneSeconds());
    }

    void ScreenshotSaved()
    {
        Debug.Log("screenshot finished saving");
        saved = true;
    }
    IEnumerator WaitForOneSeconds()
    {
        yield return new WaitForSeconds(0.5f);

        UICanvas.SetActive(true);
        //if (saved)
        //{
           // screenShotImage.SetActive(true);
            //showTip.text = "图片保存成功";
        //}
        yield return new WaitForSeconds(1.5f);
       //showTip.text = "";
       // screenShotImage.SetActive(false);
    }

   
}