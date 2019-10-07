package com.wearnotch.notchdemo.util;

import android.bluetooth.BluetoothDevice;
import android.util.Log;
import android.widget.Toast;

import com.wearnotch.framework.Bone;
import com.wearnotch.notchdemo.NotchApplication;
import com.wearnotch.service.common.NotchError;

import java.io.FileOutputStream;
import java.io.IOException;
import java.io.InputStream;
import java.util.List;
import java.util.Map;

public final class Util {

    private static String TAG = "NotchUtil";

    public static void copyFile(InputStream in, FileOutputStream out) throws IOException {
        byte[] buff = new byte[1024];
        int read;

        try {
            while ((read = in.read(buff)) > 0) {
                out.write(buff, 0, read);
            }
        } finally {
            in.close();
            out.close();
        }
    }

    @SuppressWarnings("unchecked")
    public static String getNotchErrorStr(NotchError error) {
        StringBuilder result = new StringBuilder();
        Map<String, List<Bone>> status = error.getStatus();
        if (result.length() > 0) result.append("\n\n");
        for (String type : status.keySet()) {
            result.append(type);
            if (status.get(type).size() > 0) {
                result.append(" (").append(getBoneList(status.get(type))).append(")");
            }
        }

        return result.toString();
    }

    public static String getMeasurementStatusString(Map<String, List<Bone>> status) {
        StringBuilder result = new StringBuilder();

        if (result.length() > 0) result.append("\n\n");
        for (String type : status.keySet()) {
            result.append(type);
            if (status.get(type).size() > 0) {
                result.append(" (").append(getBoneList(status.get(type))).append(")");
            }
        }

        return result.toString();
    }

    private static String getBoneList(List<Bone> bones) {
        StringBuilder result = new StringBuilder();

        for (Bone b : bones) {
            if (result.length() > 0) result.append(", ");
            result.append(b.getName());
        }

        return result.toString();
    }

    public static void showNotchError(final NotchError error) {
        try {
            String errorStr = getNotchErrorStr(error);
            Log.e(TAG, errorStr);
            Toast.makeText(NotchApplication.getInst(), errorStr, Toast.LENGTH_LONG).show();
        } catch (Exception e) {
            Log.e(TAG, "Toast exception", e);
        }
    }

    public static void showNotification(final int stringId) {
        try {
            Toast.makeText(NotchApplication.getInst(), stringId, Toast.LENGTH_SHORT).show();
        } catch (Exception e) {
            Log.e(TAG, "Toast exception", e);
        }
    }

    public static void showNotification(final String msg) {
        try {
            Toast.makeText(NotchApplication.getInst(), msg, Toast.LENGTH_SHORT).show();
        } catch (Exception e) {
            Log.e(TAG, "Toast exception", e);
        }
    }

    /**
     * Get the Notch ID of a Bluetooth device.
     */
    public static String getNotchId(BluetoothDevice device) {
        String[] macParts = device.getAddress().split(":");
        return macParts[macParts.length - 1];
    }

    private Util() {}
}
