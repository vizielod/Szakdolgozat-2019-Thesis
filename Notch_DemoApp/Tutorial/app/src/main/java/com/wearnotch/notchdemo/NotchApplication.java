package com.wearnotch.notchdemo;

import android.app.Application;

public class NotchApplication extends Application {

    private static NotchApplication mInst;

    public static NotchApplication getInst() {
        return mInst;
    }

    @Override
    public void onCreate() {
        mInst = this;
    }
}
