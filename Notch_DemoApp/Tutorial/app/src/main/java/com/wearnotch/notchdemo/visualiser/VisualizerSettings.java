package com.wearnotch.notchdemo.visualiser;

import android.content.Context;
import android.content.SharedPreferences;

public class VisualizerSettings {
    private static final String VISUALIZER_SETTINGS_NAME = "notch_visualizer_settings";

    private static final String SHOW_FULL_BODY = "show_full_body";
    private static final String PIN_TO_CENTER = "pin_to_center";

    private static VisualizerSettings sInstance;

    public static void init(Context context) {
        sInstance = new VisualizerSettings(context);
    }

    public static VisualizerSettings getInstance() {
        return sInstance;
    }

    private SharedPreferences mPrefs;

    private VisualizerSettings(Context context) {
        mPrefs = context.getSharedPreferences(VISUALIZER_SETTINGS_NAME, Context.MODE_PRIVATE);
    }

    public void putVisualizerShowFullBody(boolean isShowing) {
        SharedPreferences.Editor editor = mPrefs.edit();

        editor.putBoolean(SHOW_FULL_BODY, isShowing);
        editor.apply();
    }

    public boolean isVisualizerShowFullBody() {
        return mPrefs.getBoolean(SHOW_FULL_BODY, false);
    }

    public void putVisualizerPinToCenter(boolean isPinned) {
        SharedPreferences.Editor editor = mPrefs.edit();

        editor.putBoolean(PIN_TO_CENTER, isPinned);
        editor.apply();
    }

    public boolean isVisualizerPinToCentre() {
        return mPrefs.getBoolean(PIN_TO_CENTER, false);
    }

}
