package com.erichym.myunitylib;

import android.app.Activity;
import android.content.Context;
import android.content.Intent;
import android.widget.Toast;

/**
 * Author: erichym
 * Date: 2018/8/8
 */
public class UnityTestActivity extends Activity{
    /**
     * unity项目启动时的的上下文
     */
    private Activity _unityActivity;
    private Context _unittyContext;
    /**
     * 获取unity项目的上下文
     * @return
     */
    Activity getActivity(){
        if(null == _unityActivity) {
            try {
                Class<?> classtype = Class.forName("com.unity3d.player.UnityPlayer");
                Activity activity = (Activity) classtype.getDeclaredField("currentActivity").get(classtype);
                _unityActivity = activity;
                _unittyContext=activity;
            } catch (ClassNotFoundException e) {

            } catch (IllegalAccessException e) {

            } catch (NoSuchFieldException e) {

            }
        }
        return _unityActivity;
    }

    public void showToast(String content){
        Toast.makeText(getActivity(),content,Toast.LENGTH_SHORT).show();
        //这里是主动调用Unity中的方法，该方法之后unity部分会讲到
        //callUnity("Main Camera","FromAndroid", "hello unity i'm android");
    }

    //Unity中会调用这个方法，用于区分打开摄像机 开始本地相册s
    public void TakePhoto(String str)
    {
        Intent intent = new Intent(getActivity(),WebViewActivity.class);
        intent.putExtra("type", str);
        getActivity().startActivity(intent);
        //_unittyContext.startActivity(intent);
    }
}
