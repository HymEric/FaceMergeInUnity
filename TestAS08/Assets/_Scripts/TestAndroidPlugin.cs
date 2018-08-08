using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TestAndroidPlugin : MonoBehaviour {

    public Text outputTextView;
    // Use this for initialization
    void Start () {
        if (Application.platform == RuntimePlatform.Android)
        {
            Debug.Log("Android function is to be called");

            var ajc = new AndroidJavaClass("com.erichym.mylibsforunity.Helper"); //(1)
            var output = ajc.CallStatic<string>("DoSthInAndroid"); //(2)
            outputTextView.text = output;

            Debug.Log(output);
            Debug.Log("Android function is called");
        }

    }
	
	// Update is called once per frame
	void Update () {
		
	}
}
