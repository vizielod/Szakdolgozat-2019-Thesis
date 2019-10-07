package com.wearnotch.notchdemo;

import android.content.Context;
import android.os.Bundle;
import android.support.v4.app.Fragment;

import com.wearnotch.service.network.NotchService;

/**
 * Base class for fragments.
 */
public class BaseFragment extends Fragment implements NotchServiceConnection {

    protected Context mApplicationContext;
    protected NotchService mNotchService;

    protected void bindNotchService() {
        BaseActivity activity = (BaseActivity) getActivity();
        if (activity != null) {
            activity.addNotchServiceConnection(this);
        }
    }

    @Override
    public void onCreate(Bundle savedInstanceState) {
        super.onCreate(savedInstanceState);
        mApplicationContext = getActivity().getApplicationContext();
    }

    public boolean onBackPressed() {
        return false;
    }

    public BaseActivity getBaseActivity() {
        return (BaseActivity) getActivity();
    }

    public void setActionBarTitle(int stringResId) {
        BaseActivity activity = getBaseActivity();
        if (activity != null) {
            activity.getSupportActionBar().setTitle(stringResId);
        }
    }

    /**
     * Override point for subclasses.
     */
    @Override
    public void onServiceConnected(NotchService notchService) {
        mNotchService = notchService;
    }

    /**
     * Override point for subclasses.
     */
    @Override
    public void onServiceDisconnected() {
        mNotchService = null;
    }

    protected void fireInvalidateOptionsMenu() {
        if (isAdded()) {
            getActivity().runOnUiThread(mInvalidateOptionsMenu);
        }
    }

    private Runnable mInvalidateOptionsMenu = new Runnable() {
        @Override
        public void run() {
            if (isAdded()) {
                getActivity().invalidateOptionsMenu();
            }
        }
    };

}
