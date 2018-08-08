package com.erichym.myunitylib;

import android.Manifest;
import android.app.Activity;
import android.content.ContentValues;
import android.content.Intent;
import android.content.pm.PackageManager;
import android.database.Cursor;
import android.graphics.Bitmap;
import android.net.Uri;
import android.os.Bundle;
import android.os.Environment;
import android.provider.MediaStore;
import android.support.annotation.NonNull;
import android.support.v4.app.ActivityCompat;
import android.support.v4.content.ContextCompat;
import android.util.Log;
import android.widget.ImageView;
import android.widget.Toast;

import java.io.File;
import java.io.IOException;
import java.text.SimpleDateFormat;
import java.util.Date;

/**
 * Author: erichym
 * Date: 2018/8/8
 */
public class WebViewActivity extends Activity {
    public static final int CAMERA_RESULT_CODE = 1;   //照相机请求代码
    public static final int CROP_RESULT_CODE=2;  //裁剪图片请求代码
    public static final int ALBUM_RESULT_CODE=3; //打开相册请求码
    //适配6.0以上权限请求码
    public static final int REQUEST_PERMISSIONS=4; //打开相机权限请求码

    ImageView imageView = null;
    private File file;
    private Uri imgUriOri;
    private String imgPathOri;
    private String ImgMergeUri;
    private String imgName;

    @Override
    protected void onCreate(Bundle savedInstanceState) {

        super.onCreate(savedInstanceState);

        setContentView(R.layout.main);

        imageView = (ImageView) this.findViewById(R.id.imageID);

        String type = this.getIntent().getStringExtra("type");

        //在这里判断是打开本地相册还是直接照相
        if(type.equals("takePhoto"))
        {
            openSysCamera();
        }else
        {
            openSysAlbum();
        }

    }

    /**
     * 打开系统相机
     */
    public void openSysCamera(){
        Intent cameraIntent = new Intent(MediaStore.ACTION_IMAGE_CAPTURE);
        cameraIntent.putExtra(MediaStore.EXTRA_OUTPUT, Uri.fromFile(
                new File(Environment.getExternalStorageDirectory(), imgName)));
        startActivityForResult(cameraIntent, CAMERA_RESULT_CODE);
//        File file = new File(Environment.getExternalStorageDirectory(), imgName);
//        try {
//            file = createOriImageFile();
//        } catch (IOException e) {
//            e.printStackTrace();
//        }
//
//        if (file != null) {
//            if (Build.VERSION.SDK_INT < Build.VERSION_CODES.N) {
//                imgUriOri = Uri.fromFile(file);
//            } else {
//                imgUriOri = FileProvider.getUriForFile(this, getPackageName() + ".provider", file);
//            }
//            cameraIntent.putExtra(MediaStore.EXTRA_OUTPUT, imgUriOri);
//            startActivityForResult(cameraIntent, CAMERA_RESULT_CODE);
//        }
    }

    /**
     * 打开系统相册
     */
    private void openSysAlbum() {
        Intent albumIntent = new Intent(Intent.ACTION_PICK);
        albumIntent.setDataAndType(MediaStore.Images.Media.EXTERNAL_CONTENT_URI, "image/*");
        startActivityForResult(albumIntent, ALBUM_RESULT_CODE);
    }

    /**
     * 通过返回的值来设置图片
     */
    @Override
    protected void onActivityResult(int requestCode, int resultCode, Intent data) {
        super.onActivityResult(requestCode,resultCode,data);

        switch (requestCode){
            case CAMERA_RESULT_CODE:
                File tempFile = new File(Environment.getExternalStorageDirectory(), imgName);
                cropPic(Uri.fromFile(tempFile));
                //适配android7.0+
                //cropPic(getImageContentUri(file));
                break;
            case CROP_RESULT_CODE:
                // 裁剪时,这样设置 cropIntent.putExtra("return-data", true); 处理方案如下
                if (data != null) {
                    Bundle bundle = data.getExtras();
                    if (bundle != null) {
                        Bitmap bitmap = bundle.getParcelable("data");
                        imageView.setImageBitmap(bitmap);
                        // 把裁剪后的图片保存至本地 返回路径
                        String urlpath = FileUtilcll.saveFile(this,"/mnt/sdcard/Android/data/com.erichym.myunitylib/files", "crop1.jpg", bitmap);
                        ImgMergeUri=urlpath;   //全局变量记录图片的路径
                        Log.e("裁剪图片地址->",urlpath);
                        // Log.e("裁剪图片地址->" + urlpath);
                    }
                }
                else{
//                    Intent intent=new Intent("com.android.camera.action.CROP",Second.class);
//                    mActivity.startActivity(intent);
                }

                // 裁剪时,这样设置 cropIntent.putExtra("return-data", false); 处理方案如下
//                try {
//                    ivHead.setImageBitmap(BitmapFactory.decodeStream(
// getActivity().getContentResolver().openInputStream(imageUri)));
//                } catch (FileNotFoundException e) {
//                    e.printStackTrace();
//                }
                break;
            case ALBUM_RESULT_CODE:
                // 相册
                cropPic(data.getData());
                break;
        }
    }

    /**
     * 裁剪图片
     *
     * @param data
     */
    private void cropPic(Uri data) {
        if (data == null) {
            return;
        }
        Intent cropIntent = new Intent("com.android.camera.action.CROP");
        cropIntent.setDataAndType(data, "image/*");

        // 开启裁剪：打开的Intent所显示的View可裁剪
        cropIntent.putExtra("crop", "true");
        // 裁剪宽高比
        cropIntent.putExtra("aspectX", 1);
        cropIntent.putExtra("aspectY", 1);
        // 裁剪输出大小
        cropIntent.putExtra("outputX", 320);
        cropIntent.putExtra("outputY", 320);
        cropIntent.putExtra("scale", true);
        /**
         * return-data
         * 这个属性决定我们在 onActivityResult 中接收到的是什么数据，
         * 如果设置为true 那么data将会返回一个bitmap
         * 如果设置为false，则会将图片保存到本地并将对应的uri返回，当然这个uri得有我们自己设定。
         * 系统裁剪完成后将会将裁剪完成的图片保存在我们所这设定这个uri地址上。我们只需要在裁剪完成后直接调用该uri来设置图片，就可以了。
         *  // 裁剪属性 cropIntent.putExtra("return-data", false); 时，使用自定义接收图片的Uri
         private static final String IMAGE_FILE_LOCATION = "file:///" + Environment.getExternalStorageDirectory().getPath() + "/temp.jpg";
         private Uri imageUri = Uri.parse(IMAGE_FILE_LOCATION);
         */
        cropIntent.putExtra("return-data", true);
        // 当 return-data 为 false 的时候需要设置这句
//        cropIntent.putExtra(MediaStore.EXTRA_OUTPUT, imageUri);
        // 图片输出格式
//        cropIntent.putExtra("outputFormat", Bitmap.CompressFormat.JPEG.toString());
        // 头像识别 会启动系统的拍照时人脸识别
//        cropIntent.putExtra("noFaceDetection", true);
        startActivityForResult(cropIntent, CROP_RESULT_CODE);
    }

    /**
     * 初始化相机相关权限
     * 适配6.0+手机的运行时权限
     */
    private void initPermission() {
        String[] permissions = new String[]{Manifest.permission.CAMERA,
                Manifest.permission.WRITE_EXTERNAL_STORAGE,
                Manifest.permission.READ_EXTERNAL_STORAGE,Manifest.permission.INTERNET,Manifest.permission.ACCESS_WIFI_STATE,Manifest.permission.ACCESS_NETWORK_STATE};
        //检查权限
        if (ContextCompat.checkSelfPermission(this, Manifest.permission.CAMERA)
                != PackageManager.PERMISSION_GRANTED) {
            // 之前拒绝了权限，但没有点击 不再询问 这个时候让它继续请求权限
            if (ActivityCompat.shouldShowRequestPermissionRationale(this,
                    Manifest.permission.CAMERA)) {
                Toast.makeText(this, "用户曾拒绝打开相机权限", Toast.LENGTH_SHORT).show();
                ActivityCompat.requestPermissions(this, permissions, REQUEST_PERMISSIONS);
            } else {
                //注册相机权限
                ActivityCompat.requestPermissions(this, permissions, REQUEST_PERMISSIONS);
            }
        }
    }
    //回调权限
    @Override
    public void onRequestPermissionsResult(int requestCode, @NonNull String[] permissions, @NonNull int[] grantResults) {
        switch (requestCode) {
            case REQUEST_PERMISSIONS:
                if (grantResults.length > 0 && grantResults[0] == PackageManager.PERMISSION_GRANTED) {
                    //成功
                    Toast.makeText(this, "用户授权相机权限", Toast.LENGTH_SHORT).show();
                } else {
                    // 勾选了不再询问
                    Toast.makeText(this, "用户拒绝相机权限", Toast.LENGTH_SHORT).show();
                    /**
                     * 跳转到 APP 详情的权限设置页,即是手机-》设置-》应用-》权限管理
                     *
                     * 可根据自己的需求定制对话框，点击某个按钮在执行下面的代码
                     */
                    // Intent intent = Util.getAppDetailSettingIntent(PhotoFromSysActivity.this);
                    // startActivity(intent);
                }
                break;
        }
    }

    /**
     * 创建原图像保存的文件
     *
     * @return
     * @throws IOException
     */
    private File createOriImageFile() throws IOException {
        String imgNameOri = "HomePic_" + new SimpleDateFormat(
                "yyyyMMdd_HHmmss").format(new Date());
        File pictureDirOri = new File(getExternalFilesDir(
                Environment.DIRECTORY_PICTURES).getAbsolutePath() + "/OriPicture");
        if (!pictureDirOri.exists()) {
            pictureDirOri.mkdirs();
        }
        File image = File.createTempFile(
                imgNameOri,         /* prefix */
                ".jpg",             /* suffix */
                pictureDirOri       /* directory */
        );
        imgPathOri = image.getAbsolutePath();
        return image;
    }

    /**
     * 7.0以上获取裁剪 Uri
     *
     * @param imageFile
     * @return
     */
    private Uri getImageContentUri(File imageFile) {
        String filePath = imageFile.getAbsolutePath();
        Cursor cursor = getContentResolver().query(
                MediaStore.Images.Media.EXTERNAL_CONTENT_URI,
                new String[]{MediaStore.Images.Media._ID},
                MediaStore.Images.Media.DATA + "=? ",
                new String[]{filePath}, null);

        if (cursor != null && cursor.moveToFirst()) {
            int id = cursor.getInt(cursor
                    .getColumnIndex(MediaStore.MediaColumns._ID));
            Uri baseUri = Uri.parse("content://media/external/images/media");
            return Uri.withAppendedPath(baseUri, "" + id);
        } else {
            if (imageFile.exists()) {
                ContentValues values = new ContentValues();
                values.put(MediaStore.Images.Media.DATA, filePath);
                return getContentResolver().insert(
                        MediaStore.Images.Media.EXTERNAL_CONTENT_URI, values);
            } else {
                return null;
            }
        }
    }

}