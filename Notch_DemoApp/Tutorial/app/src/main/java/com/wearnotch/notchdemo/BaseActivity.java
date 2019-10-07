package com.wearnotch.notchdemo;

import android.content.ComponentName;
import android.content.Intent;
import android.content.ServiceConnection;
import android.os.Bundle;
import android.os.IBinder;
import android.support.v4.app.Fragment;
import android.support.v7.app.AppCompatActivity;
import android.view.MenuItem;

import com.wearnotch.service.NotchAndroidService;
import com.wearnotch.service.network.NotchService;

import java.util.ArrayList;
import java.util.List;

/**
 * Base class for all activities.
 */
public class BaseActivity extends AppCompatActivity implements ServiceConnection {

    private final List<NotchServiceConnection> mNotchServiceConnections =
            new ArrayList<NotchServiceConnection>();

    private boolean mServiceBound;
    private ComponentName mNotchServiceComponent;
    protected NotchService mNotchService;

    public void addNotchServiceConnection(NotchServiceConnection c) {
        if (!mServiceBound) {
            mServiceBound = true;
            Intent controlServiceIntent = new Intent(this, NotchAndroidService.class);
            bindService(controlServiceIntent, this, BIND_AUTO_CREATE);
        }

        if (!mNotchServiceConnections.contains(c)) {
            mNotchServiceConnections.add(c);
        }

        if (mNotchService != null) {
            c.onServiceConnected(mNotchService);
        }
    }

    @Override
    protected void onCreate(Bundle savedInstanceState) {
        super.onCreate(savedInstanceState);
        getSupportActionBar().setHomeButtonEnabled(true);
        getSupportActionBar().setDisplayHomeAsUpEnabled(true);
    }

    @Override
    protected void onDestroy() {
        if (mServiceBound) {
            mServiceBound = false;
            unbindService(this);
        }

        super.onDestroy();
    }

    @Override
    public void onBackPressed() {
        boolean processed = false;
        List<Fragment> fragments = getSupportFragmentManager().getFragments();
        if (fragments != null) {
            for (Fragment f : fragments) {
                if (f instanceof BaseFragment && f.isVisible()) {
                    processed = ((BaseFragment) f).onBackPressed();
                }

                if (processed)
                    break;
            }
        }

        if (!processed) {
            super.onBackPressed();
        }
    }

    @Override
    public boolean onOptionsItemSelected(MenuItem item) {
        if (item.getItemId() == android.R.id.home) {
            onBackPressed();
            return true;
        } else {
            return super.onOptionsItemSelected(item);
        }
    }

    @Override
    public void onServiceConnected(ComponentName name, IBinder service) {
        if (service instanceof NotchService) {
            mNotchServiceComponent = name;
            mNotchService = (NotchService) service;
            fireNotchServiceChange();
        }
    }

    @Override
    public void onServiceDisconnected(ComponentName name) {
        if (name.equals(mNotchServiceComponent)) {
            mNotchServiceComponent = null;
            mNotchService = null;
            fireNotchServiceChange();
        }
    }

    private void fireNotchServiceChange() {
        for (NotchServiceConnection c : mNotchServiceConnections) {
            if (mNotchService != null) {
                c.onServiceConnected(mNotchService);
            } else {
                c.onServiceDisconnected();
            }
        }
    }
}
