package com.unity3d.AndroidGameUsingNotches;

import android.app.Activity;
import android.content.Intent;
import android.content.res.Configuration;
import android.os.Bundle;
import android.util.Log;
import android.view.KeyEvent;
import android.view.MotionEvent;
import android.view.Window;
import android.view.WindowManager;

import com.unity3d.player.UnityPlayer;

public class UnityPlayerActivity extends Activity
{
    protected UnityPlayer mUnityPlayer; // don't change the name of this variable; referenced from native code
    protected Bundle bundle;

    // Override this in your custom UnityPlayerActivity to tweak the command line arguments passed to the Unity Android Player
    // The command line arguments are passed as a string, separated by spaces
    // UnityPlayerActivity calls this from 'onCreate'
    // Supported: -force-gles20, -force-gles30, -force-gles31, -force-gles31aep, -force-gles32, -force-gles, -force-vulkan
    // See https://docs.unity3d.com/Manual/CommandLineArguments.html
    // @param cmdLine the current command line arguments, may be null
    // @return the modified command line string or null
    protected String updateUnityCommandLineArguments(String cmdLine)
    {
        return cmdLine;
    }

    private static final String LOGTAG = "NotchPosition";


    // Setup activity layout
    @Override protected void onCreate(Bundle savedInstanceState)
    {
        Log.i(LOGTAG, "UnityPlayerActivity onCreate");
        requestWindowFeature(Window.FEATURE_NO_TITLE);
        super.onCreate(savedInstanceState);

        String cmdLine = updateUnityCommandLineArguments(getIntent().getStringExtra("unity"));
        getIntent().putExtra("unity", cmdLine);


        mUnityPlayer = new UnityPlayer(this);
        //getWindow().addFlags(WindowManager.LayoutParams.FLAG_KEEP_SCREEN_ON);
        setContentView(mUnityPlayer);
        mUnityPlayer.requestFocus();
    }

    //private int i;

    public void javaTestFunc(String strFromUnity){
        Bundle bundle = getIntent().getExtras();
        //i++;
        //String message = i + "" + bundle.getString("message");
        String message = bundle.getString("message");
        //mData = (VisualiserData) getIntent().getSerializableExtra(PARAM_INPUT_DATA);

        //UnityPlayer.UnitySendMessage("PluginScript", "SetText", message);
        //UnityPlayer.UnitySendMessage("PluginScript", "SetJavaLog", strFromUnity + "HelloWorld");
        UnityPlayer.UnitySendMessage("PluginScript", "SetJavaLog", message);
        //UnityPlayer.UnitySendMessage("MenuPluginScript", "SetJavaLog", message);
        //Log.i(LOGTAG, strFromUnity + "javaTestFunc called");
    }


    public void javaGetCoordFunc(String strFromUnity){
        bundle = getIntent().getExtras();

        /** Chest **/
        float chestX_float = bundle.getFloat("ChestAnteriorTilt");
        float chestY_float = bundle.getFloat("ChestRotation");
        float chestZ_float = bundle.getFloat("ChestLateralTilt");
        String chestX_str = String.valueOf(chestX_float);
        String chestY_str = String.valueOf(chestY_float);
        String chestZ_str = String.valueOf(chestZ_float);
        UnityPlayer.UnitySendMessage("PluginScript", "SetAngleX", chestX_str);
        UnityPlayer.UnitySendMessage("PluginScript", "SetAngleY", chestY_str);
        UnityPlayer.UnitySendMessage("PluginScript", "SetAngleZ", chestZ_str);

        /** Left Upper Arm **/
        float leftUpperArmX_float = bundle.getFloat("LeftUpperArmAnteriorTilt");
        float leftUpperArmY_float = bundle.getFloat("LeftUpperArmRotation");
        float leftUpperArmZ_float = bundle.getFloat("LeftUpperArmLateralTilt");
        String leftUpperArmX_str = String.valueOf(leftUpperArmX_float);
        String leftUpperArmY_str = String.valueOf(leftUpperArmY_float);
        String leftUpperArmZ_str = String.valueOf(leftUpperArmZ_float);
        UnityPlayer.UnitySendMessage("PluginScript", "SetLeftUpperArmAngleX", leftUpperArmX_str);
        UnityPlayer.UnitySendMessage("PluginScript", "SetLeftUpperArmAngleY", leftUpperArmY_str);
        UnityPlayer.UnitySendMessage("PluginScript", "SetLeftUpperArmAngleZ", leftUpperArmZ_str);

        /** Left Forearm **/
        float leftForeArmX_float = bundle.getFloat("LeftForeArmAnteriorTilt");
        float leftForeArmY_float = bundle.getFloat("LeftForeArmRotation");
        float leftForeArmZ_float = bundle.getFloat("LeftForeArmLateralTilt");
        String leftForeArmX_str = String.valueOf(leftForeArmX_float);
        String leftForeArmY_str = String.valueOf(leftForeArmY_float);
        String leftForeArmZ_str = String.valueOf(leftForeArmZ_float);
        UnityPlayer.UnitySendMessage("PluginScript", "SetLeftForeArmAngleX", leftForeArmX_str);
        UnityPlayer.UnitySendMessage("PluginScript", "SetLeftForeArmAngleY", leftForeArmY_str);
        UnityPlayer.UnitySendMessage("PluginScript", "SetLeftForeArmAngleZ", leftForeArmZ_str);

        /** Right Upper Arm **/
        float rightUpperArmX_float = bundle.getFloat("RightUpperArmAnteriorTilt");
        float rightUpperArmY_float = bundle.getFloat("RightUpperArmRotation");
        float rightUpperArmZ_float = bundle.getFloat("RightUpperArmLateralTilt");
        String rightUpperArmX_str = String.valueOf(rightUpperArmX_float);
        String rightUpperArmY_str = String.valueOf(rightUpperArmY_float);
        String rightUpperArmZ_str = String.valueOf(rightUpperArmZ_float);
        UnityPlayer.UnitySendMessage("PluginScript", "SetRightUpperArmAngleX", rightUpperArmX_str);
        UnityPlayer.UnitySendMessage("PluginScript", "SetRightUpperArmAngleY", rightUpperArmY_str);
        UnityPlayer.UnitySendMessage("PluginScript", "SetRightUpperArmAngleZ", rightUpperArmZ_str);

        /** Right Forearm **/
        float rightForeArmX_float = bundle.getFloat("RightForeArmAnteriorTilt");
        float rightForeArmY_float = bundle.getFloat("RightForeArmRotation");
        float rightForeArmZ_float = bundle.getFloat("RightForeArmLateralTilt");
        String rightForeArmX_str = String.valueOf(rightForeArmX_float);
        String rightForeArmY_str = String.valueOf(rightForeArmY_float);
        String rightForeArmZ_str = String.valueOf(rightForeArmZ_float);
        UnityPlayer.UnitySendMessage("PluginScript", "SetRightForeArmAngleX", rightForeArmX_str);
        UnityPlayer.UnitySendMessage("PluginScript", "SetRightForeArmAngleY", rightForeArmY_str);
        UnityPlayer.UnitySendMessage("PluginScript", "SetRightForeArmAngleZ", rightForeArmZ_str);
        //Log.i(LOGTAG, strFromUnity + "javaGetCoordFunc called");
    }


    @Override protected void onNewIntent(Intent intent)
    {
        // To support deep linking, we need to make sure that the client can get access to
        // the last sent intent. The clients access this through a JNI api that allows them
        // to get the intent set on launch. To update that after launch we have to manually
        // replace the intent with the one caught here.
        setIntent(intent);
        mUnityPlayer.newIntent(intent);
    }

    // Quit Unity
    @Override protected void onDestroy ()
    {
        //mUnityPlayer.quit();
        mUnityPlayer.destroy();
        super.onDestroy();
    }

    @Override public void onUserInteraction(){
        super.onUserInteraction();
        Log.i(LOGTAG, "UnityPlayerActivity onUserInteraction");
    }
    // Pause Unity
    @Override protected void onPause()
    {
        Log.i(LOGTAG, "UnityPlayerActivity onPause");
        super.onPause();
        //mUnityPlayer.pause();
    }

    // Resume Unity
    @Override protected void onResume()
    {
        Log.i(LOGTAG, "UnityPlayerActivity onResume");
        super.onResume();
        mUnityPlayer.resume();

        /*bundle = getIntent().getExtras();
        String message = bundle.getString("message");
        UnityPlayer.UnitySendMessage("PluginScript", "SetText", message);

        javaGetCoordFunc("Something");*/
    }

    @Override protected void onStart()
    {
        Log.i(LOGTAG, "UnityPlayerActivity onStart");
        super.onStart();
        mUnityPlayer.start();
        /*Bundle bundle = getIntent().getExtras();
        String message = bundle.getString("message");
        UnityPlayer.UnitySendMessage("PluginScript", "SetText", message);

        float chestX_float = bundle.getFloat("ChestAnteriorTilt");
        float chestY_float = bundle.getFloat("ChestRotation");
        float chestZ_float = bundle.getFloat("ChestLateralTilt");
        String chestX_str = String.valueOf(chestX_float);
        String chestY_str = String.valueOf(chestY_float);
        String chestZ_str = String.valueOf(chestZ_float);
        UnityPlayer.UnitySendMessage("PluginScript", "SetAngleX", chestX_str);
        UnityPlayer.UnitySendMessage("PluginScript", "SetAngleY", chestY_str);
        UnityPlayer.UnitySendMessage("PluginScript", "SetAngleZ", chestZ_str);*/
    }

    @Override protected void onStop()
    {
        super.onStop();
        mUnityPlayer.stop();
    }

    // Low Memory Unity
    @Override public void onLowMemory()
    {
        super.onLowMemory();
        mUnityPlayer.lowMemory();
    }

    // Trim Memory Unity
    @Override public void onTrimMemory(int level)
    {
        super.onTrimMemory(level);
        if (level == TRIM_MEMORY_RUNNING_CRITICAL)
        {
            mUnityPlayer.lowMemory();
        }
    }

    // This ensures the layout will be correct.
    @Override public void onConfigurationChanged(Configuration newConfig)
    {
        super.onConfigurationChanged(newConfig);
        mUnityPlayer.configurationChanged(newConfig);
    }

    // Notify Unity of the focus change.
    @Override public void onWindowFocusChanged(boolean hasFocus)
    {
        super.onWindowFocusChanged(hasFocus);
        mUnityPlayer.windowFocusChanged(hasFocus);
    }

    // For some reason the multiple keyevent type is not supported by the ndk.
    // Force event injection by overriding dispatchKeyEvent().
    @Override public boolean dispatchKeyEvent(KeyEvent event)
    {
        if (event.getAction() == KeyEvent.ACTION_MULTIPLE)
            return mUnityPlayer.injectEvent(event);
        return super.dispatchKeyEvent(event);
    }

    // Pass any events not handled by (unfocused) views straight to UnityPlayer
    @Override public boolean onKeyUp(int keyCode, KeyEvent event)     { return mUnityPlayer.injectEvent(event); }
    @Override public boolean onKeyDown(int keyCode, KeyEvent event)   { return mUnityPlayer.injectEvent(event); }
    @Override public boolean onTouchEvent(MotionEvent event)          { return mUnityPlayer.injectEvent(event); }
    /*API12*/ public boolean onGenericMotionEvent(MotionEvent event)  { return mUnityPlayer.injectEvent(event); }
}
