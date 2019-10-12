package com.wearnotch.notchdemo.visualiser;

import android.annotation.SuppressLint;
import android.app.Activity;
import android.content.ComponentName;
import android.content.Context;
import android.content.DialogInterface;
import android.content.Intent;
import android.content.ServiceConnection;
import android.net.Uri;
import android.opengl.GLES20;
import android.os.AsyncTask;
import android.os.Bundle;
import android.os.Environment;
import android.os.IBinder;
import android.os.Parcelable;
import android.support.v7.app.AlertDialog;
import android.support.v7.app.AppCompatActivity;
import android.util.Log;
import android.view.Menu;
import android.view.MenuItem;
import android.view.View;
import android.widget.ImageButton;
import android.widget.ProgressBar;
import android.widget.SeekBar;
import android.widget.TextView;
import android.widget.Toast;

import com.unity3d.AndroidGameUsingNotches.UnityPlayerActivity;
import com.wearnotch.framework.Bone;
import com.wearnotch.framework.Measurement;
import com.wearnotch.framework.Skeleton;
import com.wearnotch.framework.visualiser.VisualiserData;
import com.wearnotch.notchdemo.BaseActivity;
import com.wearnotch.notchdemo.MainActivity;
import com.wearnotch.notchdemo.MainFragment;
import com.wearnotch.notchdemo.NotchServiceConnection;
import com.wearnotch.notchdemo.R;
import com.wearnotch.notchdemo.util.Util;
import com.wearnotch.notchmaths.fvec3;
import com.wearnotch.service.NotchAndroidService;
import com.wearnotch.service.common.Cancellable;
import com.wearnotch.service.common.NotchCallback;
import com.wearnotch.service.common.NotchError;
import com.wearnotch.service.common.NotchProgress;
import com.wearnotch.service.network.NotchService;
import com.wearnotch.visualiser.NotchSkeletonRenderer;
import com.wearnotch.visualiser.RigidBody;
import com.wearnotch.visualiser.shader.ColorShader;

import org.greenrobot.eventbus.EventBus;
import org.greenrobot.eventbus.Subscribe;

import java.io.BufferedWriter;
import java.io.File;
import java.io.FileInputStream;
import java.io.FileWriter;
import java.text.DecimalFormat;
import java.text.DecimalFormatSymbols;
import java.util.ArrayList;
import java.util.Arrays;
import java.util.List;
import java.util.Locale;

import javax.microedition.khronos.egl.EGL10;
import javax.microedition.khronos.egl.EGL11;
import javax.microedition.khronos.egl.EGLConfig;
import javax.microedition.khronos.egl.EGLDisplay;
import javax.microedition.khronos.opengles.GL10;

import butterknife.ButterKnife;
import butterknife.BindView;
import butterknife.OnClick;

public class VisualiserActivity extends AppCompatActivity implements SeekBar.OnSeekBarChangeListener, ServiceConnection, NotchServiceConnection {
    private static final String LOGTAG = "NotchPosition";
    private static final String TAG = "Visualiser";

    private static final float SECONDARY_TRANSPARENCY = 0.8f;

    private static final int REQUEST_OPEN = 1;

    private static final String NOTCH_DIR = "notch_tutorial";

    public static final String PARAM_INPUT_ZIP = "INPUT_ZIP";
    public static final String PARAM_INPUT_DATA = "INPUT_DATA";
    public static final String PARAM_REALTIME = "REALTIME";

    public static final String OBJ_ASSET = "droid_obj.dat";
    public static final String MTL_ASSET = "droid_mtl.dat";



    public static Intent createIntent(Context context, Uri zipUri) {
        Intent i = new Intent(context, VisualiserActivity.class);
        i.putExtra(PARAM_INPUT_ZIP, new Parcelable[] { zipUri });
        return i;
    }

    public static Intent createIntent(Context context, VisualiserData data, boolean realtime) {
        Intent i = new Intent(context, VisualiserActivity.class);

        Intent UnityIntent = new Intent(context, UnityPlayerActivity.class);
        //UnityIntent.putExtra()
        i.putExtra(PARAM_INPUT_DATA, data);
        i.putExtra(PARAM_REALTIME,realtime);
        //i.putExtra(UNITY_ACTIVITY, new Intent(this, UnityPlayerActivity.class));
        return i;
    }

    private Context mApplicationContext;
    private Parcelable[] mZipUri;
    private VisualiserData mData;
    private Skeleton mSkeleton;

    private volatile boolean mPaused;
    private volatile int mFrameIndex;
    private int mFrameCount;

    private volatile float mSpeed = 1f;
    private volatile boolean mSeeking;
    private volatile boolean mShowPath, mPinToCentre, mHighlightBones;
    private volatile List<Bone> mBonesToShow = new ArrayList<>();
    private String[] mBoneNames;
    private volatile boolean[] mCheckedBones;
    private boolean mRealTime, isGroundDrawn, mShowAngles;
    private float mFrequency;

    private AlertDialog mBoneSelectorDialog;

    @BindView(R.id.progress_animation)
    protected ProgressBar mProgress;

    @BindView(R.id.surface_view)
    protected TouchGLView mSurfaceView;

    private ExtendedRenderer mRenderer;

    @BindView(R.id.seekbar)
    protected SeekBar mSeekBar;

    @BindView(R.id.button_play_pause)
    protected ImageButton mPlayPause;

    @BindView(R.id.speed_text)
    protected TextView mSpeedText;

    @BindView(R.id.angles_text)
    protected TextView mAnglesText;

    @BindView(R.id.elapsed_time_txt)
    protected TextView mElapsedTimeText;

    DecimalFormat decimalFormat;

    @Override
    protected void onPostResume() {
        super.onPostResume();
        EventBus.getDefault().register(this);
    }

    @Override
    protected void onPause() {
        EventBus.getDefault().unregister(this);
        super.onPause();
    }

    @Override
    protected void onCreate(Bundle savedInstanceState) {
        super.onCreate(savedInstanceState);
        mApplicationContext = getApplicationContext();
        setContentView(R.layout.activity_visualiser);
        ButterKnife.bind(this);

        mZipUri = getIntent().getParcelableArrayExtra(PARAM_INPUT_ZIP);
        mData = (VisualiserData) getIntent().getSerializableExtra(PARAM_INPUT_DATA);
        mRealTime = getIntent().getBooleanExtra(PARAM_REALTIME,false);
        mSeekBar.setOnSeekBarChangeListener(this);
        initRenderer();
        mShowAngles = true;

        DecimalFormatSymbols otherSymbols = new DecimalFormatSymbols(Locale.US);
        decimalFormat = new DecimalFormat("0.000", otherSymbols);

        bindNotchService();

    }

    @Subscribe
    public void onDataUpdate(VisualiserData data) {
        if (data != null && mRenderer != null) {
            mData = data;
            NotchSkeletonRenderer.RendererContext rc = mRenderer.getRendererContext(0);
            if (rc == null) {
                rc = mRenderer.createRendererContext(data);
                mRenderer.setRendererContext(0, rc);
            } else {
                rc.setData(data);
            }
        }
    }

    @Override
    public boolean onCreateOptionsMenu(Menu menu) {
        super.onCreateOptionsMenu(menu);
        getMenuInflater().inflate(R.menu.visualiser, menu);

        VisualizerSettings settings = VisualizerSettings.getInstance();

        if (settings != null) {
            MenuItem highlightBones = menu.findItem(R.id.highlight_bones);
            mHighlightBones = settings.isVisualizerShowFullBody();
            highlightBones.setChecked(mHighlightBones);

            MenuItem pinToCenter = menu.findItem(R.id.pin_to_center);
            mPinToCentre = settings.isVisualizerPinToCentre();
            pinToCenter.setChecked(mPinToCentre);
        }

        return true;
    }


    @Override
    public boolean onOptionsItemSelected(MenuItem item) {
        switch (item.getItemId()) {
            case R.id.action_open:
                openMeasurement();
                return true;
            case R.id.highlight_bones:
                mHighlightBones = !mHighlightBones;
                mRenderer.setHighlightBones(mHighlightBones);
                item.setChecked(mHighlightBones);
                VisualizerSettings.getInstance().putVisualizerShowFullBody(mHighlightBones);
                refreshUI();
                return true;
            case R.id.pin_to_center:
                mPinToCentre = !mPinToCentre;
                mRenderer.setRootMovement(!mPinToCentre);
                item.setChecked(mPinToCentre);
                VisualizerSettings.getInstance().putVisualizerPinToCenter(mPinToCentre);
                refreshUI();
                return true;
            case R.id.show_angles:
                mShowAngles = !mShowAngles;
                if (mShowAngles) mAnglesText.setVisibility(View.VISIBLE);
                else mAnglesText.setVisibility(View.GONE);
                return true;
            case R.id.export_angles:
                exportAngles();
                return true;
            default:
                return super.onOptionsItemSelected(item);
        }
    }

    @Override
    protected void onActivityResult(int requestCode, int resultCode, Intent data) {
        switch (requestCode) {
            case REQUEST_OPEN:
                if (resultCode == Activity.RESULT_OK && data != null) {
                    setData(data.getData());
                }
                break;

            default:
                super.onActivityResult(requestCode, resultCode, data);
        }
    }

    @SuppressLint("StaticFieldLeak")
    private void initRenderer() {
        new AsyncTask<Void, Void, ExtendedRenderer>() {
            @Override
            protected ExtendedRenderer doInBackground(Void... params) {
                try {
                    ExtendedRenderer renderer = new ExtendedRenderer(getApplicationContext());
                    renderer.setAutoPlayback(true);

                    if (mZipUri != null) {
                        for (Parcelable uri : mZipUri) {
                            VisualiserData data = VisualiserData.fromStream(new FileInputStream(new File(((Uri) uri).getPath())));
                            int dataIndex = renderer.addRendererContext(renderer.createRendererContext(data));
                            if (dataIndex > 0) {
                                renderer.setAlpha(dataIndex, SECONDARY_TRANSPARENCY);
                            }
                        }
                    } else if (mData != null) {
                        int dataIndex = renderer.addRendererContext(renderer.createRendererContext(mData));
                        if (dataIndex > 0) {
                            renderer.setAlpha(dataIndex, SECONDARY_TRANSPARENCY);
                        }
                    }

                    return renderer;
                } catch (Exception e) {
                    Log.e(TAG, "NotchSkeletonRenderer exception", e);
                    return null;
                }
            }

            @Override
            protected void onPostExecute(ExtendedRenderer renderer) {
                super.onPostExecute(renderer);
                if (renderer != null) {
                    mRenderer = renderer;
                    mSurfaceView.setRenderer(renderer);
                    mProgress.setVisibility(View.GONE);
                    mSurfaceView.setVisibility(View.VISIBLE);
                    mShowPath = true;

                    mRenderer.setHighlightBones(mHighlightBones);
                    mRenderer.setRootMovement(!mPinToCentre);

                    if (mData == null){
                        mData = renderer.getRendererContext(0).getData();
                    }
                    mFrameCount = mData.getFrameCount();
                    mFrequency = mData.getFrequency();
                    mSeekBar.setMax(mFrameCount);
                    if (mRealTime) {
                        mSeekBar.setMax(0);
                        mSeekBar.setEnabled(false);
                        mPlayPause.setEnabled(false);
                    }
                    mRenderer.setRealTime(mRealTime);
                    mSkeleton = mData.getSkeleton();

                    mBoneNames = new String[mSkeleton.getBoneOrder().size()];
                    mCheckedBones = new boolean[mSkeleton.getBoneOrder().size()];
                    int i = 0;
                    for (Bone b : mSkeleton.getBoneOrder()) {
                        mBoneNames[i] = b.getName();
                        if (b.getName().equals("RightHand")) {
                            mCheckedBones[i] = true;
                            mBonesToShow.add(b);
                        } else {
                            mCheckedBones[i] = false;
                        }
                        i++;
                    }

                    buildBoneSelectorDialog();

                } else {
                    showNotification(R.string.error_invalid_measurement);
                    finish();
                }
            }
        }.execute();
    }

    @SuppressLint("StaticFieldLeak")
    public void setData(final Uri zipUri) {
        new AsyncTask<Void, Void, VisualiserData>() {

            @Override
            protected VisualiserData doInBackground(Void... params) {
                try {
                    return VisualiserData.fromStream(new FileInputStream(new File(zipUri.getPath())));
                } catch (Exception e) {
                    Log.e(TAG, "NotchSkeletonRenderer exception", e);
                    return null;
                }
            }

            @Override
            protected void onPostExecute(final VisualiserData data) {
                super.onPostExecute(data);
                if (data != null) {
                    mZipUri = Arrays.copyOf(mZipUri, mZipUri.length + 1);
                    mZipUri[mZipUri.length - 1] = zipUri;
                    getIntent().putExtra(PARAM_INPUT_ZIP, mZipUri);

                    mSurfaceView.postOnAnimation(new Runnable() {
                        @Override
                        public void run() {
                            int dataIndex = mRenderer.setRendererContext(mRenderer.createRendererContext(data));
                            mFrameCount = mRenderer.getRendererContext(dataIndex).getFrameCount();
                            mSeekBar.setMax(mFrameCount);
                            mData = data;
                            refreshUI();
                        }
                    });

                } else {
                    showNotification(R.string.error_invalid_measurement);
                }
            }
        }.execute();
    }

    public void refreshUI() {
        mSeekBar.setProgress(mFrameIndex);

        if (mPaused) {
            mPlayPause.setImageResource(R.drawable.ic_play);
        } else {
            mPlayPause.setImageResource(R.drawable.ic_pause);
        }

        refreshAngles();
    }

    private Runnable mRefreshPlayback = new Runnable() {
        @Override
        public void run() {
            if (mRenderer != null) {
                for (int i=0; i<mRenderer.getContextSize();i++) {
                    if (mRenderer.rendererContextIndexExists(i)) mRenderer.setFrameIndex(i,mFrameIndex);
                }

                if (!mSeeking) {
                    mRenderer.setAutoPlayback(!mPaused);
                }
            }
        }
    };

    private Runnable mStartSeeking = new Runnable() {
        @Override
        public void run() {
            if (mRenderer != null && mSeeking) {
                mRenderer.setAutoPlayback(false);
            }
        }
    };

    @Override
    public void onProgressChanged(SeekBar seekBar, int progress, boolean fromUser) {
        if (mRenderer != null && fromUser && mFrameIndex != progress) {
            mFrameIndex = progress;
            mSurfaceView.postOnAnimation(mRefreshPlayback);
        }
    }

    @Override
    public void onStartTrackingTouch(SeekBar seekBar) {
        if (mRenderer != null) {
            mSeeking = true;
            mSurfaceView.postOnAnimation(mStartSeeking);
        }
    }

    @Override
    public void onStopTrackingTouch(SeekBar seekBar) {
        mSeeking = false;
        mSurfaceView.postOnAnimation(mRefreshPlayback);
    }

    private void openMeasurement() {
        Intent chooser = new Intent(Intent.ACTION_GET_CONTENT);
        chooser.setType("application/zip");
        chooser.addCategory(Intent.CATEGORY_OPENABLE);
        startActivityForResult(chooser, REQUEST_OPEN);
    }


    @OnClick(R.id.button_play_pause)
    void onPlayPauseClicked() {
        if (mRenderer != null) {
            if (mPaused && mFrameIndex == mFrameCount - 1) {
                mFrameIndex = 0;
            }
            mPaused = !mPaused;
            refreshUI();
            mSurfaceView.postOnAnimation(mRefreshPlayback);
        }
    }

    @OnClick(R.id.button_start)
    void onStartClicked() {
        if (mRenderer != null && !mRealTime) {
            mRenderer.setAutoPlayback(false);
            mRenderer.setAllFrameIndex(0);
            mSurfaceView.postOnAnimation(mRefreshPlayback);
            mRenderer.setAutoPlayback(true);

        }
        refreshUI();
    }

    @OnClick(R.id.button_end)
    void onEndClicked() {
        if (mRenderer != null && !mRealTime) {
            mPaused = true;
            mRenderer.setAutoPlayback(false);
            mRenderer.setAllFrameIndex(mFrameCount);
            mSurfaceView.postOnAnimation(mRefreshPlayback);
        }
        refreshUI();
    }

    @OnClick(R.id.button_speed)
    void onSpeedClicked() {
        if (mRenderer != null && !mRealTime) {
            mSpeed = mSpeed * 2f;
            if (mSpeed > 1f) {
                mSpeed = 0.25f;
            }
            if (mSpeed >= 1f) {
                mSpeedText.setText((int)mSpeed + "x");
            }
            else {
                mSpeedText.setText("1/" + (int)(1f/mSpeed) + "x");
            }
            mRenderer.setAllPlaybackSpeed(mSpeed);
        }
        refreshUI();
    }

    @OnClick(R.id.button_front_view)
    void onFrontViewClicked() {
        if (mRenderer != null) {
            mRenderer.setCameraBeta((float) Math.PI/2.0f);
            mRenderer.setCameraAlpha((float) Math.PI / 2.0f);
        }
        refreshUI();
    }

    private UnityPlayerActivity mUnityPlayerActivity;

    /*@OnClick(R.id.button_top_view)
    void onTopViewClicked() {
        if (mRenderer != null) {
            mRenderer.setCameraBeta(0.0f);
            mRenderer.setCameraAlpha((float) Math.PI/2.0f);
        }
        Intent intent = new Intent(this, UnityPlayerActivity.class);
        intent.putExtra("message", RelativeNotchPosition);
        //intent.putExtra("VisualiserData", mData);
        startActivity(intent);
        //refreshUI();
    }*/

    @OnClick(R.id.button_side_view)
    void onSideViewClicked() {
        if (mRenderer != null) {
            mRenderer.setCameraBeta((float) Math.PI/2.0f);
            mRenderer.setCameraAlpha((float) Math.PI);
        }
        refreshUI();
    }

    @OnClick(R.id.button_show_path)
    void onShowPathClicked() {
        mBoneSelectorDialog.show();
    }

    public void showNotification(final String msg) {
        try {
            Toast.makeText(mApplicationContext, msg, Toast.LENGTH_SHORT).show();
        } catch (Exception e) {
            Log.e(TAG, "Toast exception", e);
        }
    }

    // Show angles
    public void refreshAngles() {
        if(mData == null) {
            return;
        }
        this.runOnUiThread(new Runnable() {
            @Override
            public void run() {
                mElapsedTimeText.setText("Elapsed time: " + String.valueOf(decimalFormat.format(mFrameIndex / mFrequency)) + " sec");
                calculateAngles(mFrameIndex);
            }
        });
    }

    private String RelativeNotchPosition;
    private float ChestRotationAngle;
    private float ChestAnteriorTiltAngle;
    private float ChestLateralTiltAngle;

    // Calculate angles
    private void calculateAngles(int frameIndex) {
        Bone chest = mSkeleton.getBone("ChestTop");
        Bone root = mSkeleton.getRoot();
        Bone foreArm = mSkeleton.getBone("RightForeArm");
        Bone upperArm = mSkeleton.getBone("RightUpperArm");

        fvec3 chestAngles = new fvec3();
        fvec3 elbowAngles = new fvec3();

        // Calculate forearm angles with respect to upper arm (determine elbow joint angles).
        // Angles correspond to rotations around X,Y and Z axis of the paren bone's coordinate system, respectively.
        // The coordinate system is X-left, Y-up, Z-front aligned.
        // Default orientations are defined in the steady pose (in the skeleton file)
        // Usage: calculateRelativeAngle(Bone child, Bone parent, int frameIndex, fvec3 output)
        mData.calculateRelativeAngle(foreArm, upperArm, frameIndex, elbowAngles);

        // Calculate chest angles with respect root, i.e. absolute angles
        // The root orientation is the always the same as in the steady pose.
        mData.calculateRelativeAngle(chest, root, frameIndex, chestAngles);
        //Log.i(LOGTAG, chestAngles.toString());
        RelativeNotchPosition = chestAngles.toString();
        ChestAnteriorTiltAngle = chestAngles.get(0);
        ChestRotationAngle = chestAngles.get(1);
        ChestLateralTiltAngle = chestAngles.get(2);


        // Show angles
        StringBuilder sb = new StringBuilder();
        sb.append("Elbow angles:\n")
                // Extension/flexion is rotation around the upperarm's X-axis
                .append("Extension(+)/flexion(-): ").append((int)elbowAngles.get(0)).append("°\n")
                // Supination/pronation is rotation around the upperarm's Y-axis
                .append("Supination(+)/pronation(-): ").append((int)elbowAngles.get(1)).append("°\n")
                .append("\nChest angles:\n")
                // Anterior/posterior tilt (forward/backward bend) is rotation around global X axis
                .append("Anterior(+)/posterior(-) tilt: ").append((int)chestAngles.get(0)).append("°\n")
                // Rotation to left/right is rotation around the global Y axis
                .append("Rotation left(+)/right(-): ").append((int)chestAngles.get(1)).append("°\n")
                // Lateral tilt (side bend) is rotation around global Z axis
                .append("Lateral tilt left(-)/right(+): ").append((int)chestAngles.get(2)).append("°\n");

        mAnglesText.setText(sb.toString());
    }

    public void showNotification(final int stringId) {
        try {
            Toast.makeText(mApplicationContext, stringId, Toast.LENGTH_SHORT).show();
        } catch (Exception e) {
            Log.e(TAG, "Toast exception", e);
        }
    }

    class ExtendedRenderer extends NotchSkeletonRenderer {
        private final float cameraBetaMax = (float) Math.PI - 0.01f;
        private final float cameraBetaMin = 0.01f;
        private final float cameraDistanceMin = 0.5f;

        ColorShader mVisualisationShader;

        public ExtendedRenderer(Context context) {
            super(context);
            mContext = context;
        }

        @Override
        protected void createAdditionalShaders() {
            super.createAdditionalShaders();
            mVisualisationShader = new ColorShader(mApplicationContext);
            addShader(mVisualisationShader);
        }

        @Override
        protected void onSurfaceCreatedGL(GL10 unused, EGLConfig config) {
            super.onSurfaceCreatedGL(unused, config);
            GLES20.glClearColor(0.87f, 0.87f, 0.87f, 0.35f);
        }

        public RendererContext createRendererContext(VisualiserData data) {
            return createRendererContext(data, OBJ_ASSET, MTL_ASSET, null);
        }

        public RendererContext createRendererContext(VisualiserData data, String obj, String mtl, RigidBody.StickCustomizer stick) {
            final PlotDemo plotDemo = new PlotDemo(data, isGroundDrawn);
            if (!isGroundDrawn) isGroundDrawn = true;
            plotDemo.init();

            return new RendererContext(data, obj, mtl, stick) {

                @Override
                public void prepare() {
                    plotDemo.prepare(mVisualisationShader);
                    super.prepare();
                }

                @Override
                public void draw() {
                    super.draw();
                    plotDemo.setShowPath(mShowPath ? mBonesToShow : null);
                    plotDemo.draw(getFrameIndex());
                }
            };
        }


        @Override
        protected void onFrameIndexChanged(int dataIndex, int frameIndex) {
            super.onFrameIndexChanged(dataIndex, frameIndex);
            mFrameIndex = frameIndex;
            mSeekBar.setProgress(mFrameIndex);
            refreshAngles();
        }

        @Override
        protected void onPlaybackFinished(int dataIndex, int frameIndex) {
            super.onPlaybackFinished(dataIndex, frameIndex);
            mPaused = true;
            runOnUiThread(new Runnable() {
                @Override
                public void run() {
                    refreshUI();
                }
            });
            mSurfaceView.postOnAnimation(mRefreshPlayback);
        }


        @Override
        public void setCameraBeta(float cameraBeta) {
            if (cameraBeta > cameraBetaMax) {
                cameraBeta = cameraBetaMax;
            } else if (cameraBeta < cameraBetaMin) {
                cameraBeta = cameraBetaMin;
            }

            super.setCameraBeta(cameraBeta);
        }

        @Override
        public void setCameraDistance(float cameraDistance) {
            if (cameraDistance < cameraDistanceMin) {
                cameraDistance = cameraDistanceMin;
            }

            super.setCameraDistance(cameraDistance);
        }
    }


    private void buildBoneSelectorDialog() {
        AlertDialog.Builder builder = new AlertDialog.Builder(this);
        builder.setTitle("Select bones!");

        builder.setMultiChoiceItems(mBoneNames, mCheckedBones, new DialogInterface.OnMultiChoiceClickListener() {
            @Override
            public void onClick(DialogInterface dialog, int which, boolean isChecked) {
                Bone b = mSkeleton.getBone(mBoneNames[which]);
                mCheckedBones[which] = isChecked;
                if (isChecked) {
                    if (!mBonesToShow.contains(b)) mBonesToShow.add(b);
                } else {
                    if (mBonesToShow.contains(b)) mBonesToShow.remove(b);
                }
            }
        });

        builder.setPositiveButton("OK", new DialogInterface.OnClickListener() {
            @Override
            public void onClick(DialogInterface dialog, int which) {

            }
        });
        builder.setNegativeButton("Clear all", new DialogInterface.OnClickListener() {
            @Override
            public void onClick(DialogInterface dialog, int which) {
                mBonesToShow.removeAll(mBonesToShow);
                for (int i=0; i<mCheckedBones.length; i++) {
                    Bone b = mSkeleton.getBone(mBoneNames[i]);
                    mBonesToShow.remove(b);
                    mCheckedBones[i]= false;
                    ((AlertDialog) dialog).getListView().setItemChecked(i, false);
                }
            }
        });

        mBoneSelectorDialog = builder.create();
    }

    private void exportAngles() {
        File directory = new File(Environment.getExternalStoragePublicDirectory(NOTCH_DIR), "exported_angles");
        if (!directory.isDirectory()) directory.mkdirs();
        BufferedWriter writer = null;

        Bone chest = mSkeleton.getBone("ChestTop");
        Bone root = mSkeleton.getRoot();
        Bone foreArm = mSkeleton.getBone("RightForeArm");
        Bone upperArm = mSkeleton.getBone("RightUpperArm");

        fvec3 angles = new fvec3();

        String[] mNames = {"Elbow", "Chest"};

        for (int i = 0; i < mNames.length; i++) {
            String mName = mNames[i];

            File file = new File(directory, mName.toLowerCase() + ".csv");
            try {
                writer = new BufferedWriter(new FileWriter(file));
                StringBuilder sb = new StringBuilder();

                for (int frame = 0; frame < mFrameCount; frame++) {
                    // Header
                    if (frame == 0) {
                        if (i==0) {
                            sb.append("Time [sec], Extension(+)/flexion(-), Supination(+)/pronation(-),\n");
                        } else {
                            sb.append("Time [sec], Anterior(+)/posterior(-) tilt, Rotation left(+)/right(-), Lateral tilt left(-)/right(+),\n");
                        }
                    }
                    // Angles
                    if (i==0) {
                        mData.calculateRelativeAngle(foreArm, upperArm, frame, angles);
                        sb.append(String.valueOf(decimalFormat.format(frame / mFrequency))).append(",")
                                .append(angles.get(0)).append(",")
                                .append(angles.get(1)).append(",")
                                .append("\n");
                    } else {
                        mData.calculateRelativeAngle(chest, root, frame, angles);
                        sb.append(String.valueOf(decimalFormat.format(frame / mFrequency))).append(",")
                                .append(angles.get(0)).append(",")
                                .append(angles.get(1)).append(",")
                                .append(angles.get(2)).append(",")
                                .append("\n");
                    }
                }
                writer.write(sb.toString());

            } catch (Exception e) {
                e.printStackTrace();
            } finally {
                try {
                    if (writer != null) {
                        writer.close();
                    }
                    Util.showNotification("Angles are exported to : '" + directory);

                } catch (Exception ignored) {
                }
            }
        }
    }

    public VisualiserActivity getVisualiserActivity() {
        return this;
    }

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
    protected void onDestroy() {
        if (mServiceBound) {
            mServiceBound = false;
            unbindService(this);
        }

        super.onDestroy();
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
    public void onServiceConnected(NotchService notchService) {
        mNotchService = notchService;
    }

    @Override
    public void onServiceDisconnected(ComponentName name) {
        if (name.equals(mNotchServiceComponent)) {
            mNotchServiceComponent = null;
            mNotchService = null;
            fireNotchServiceChange();
        }
    }

    @Override
    public void onServiceDisconnected() {
        mNotchService = null;
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

    protected void bindNotchService() {
        VisualiserActivity activity = this;
        if (activity != null) {
            activity.addNotchServiceConnection(this);
        }
    }

    Cancellable c;

    void startUnityPlayerActivity() {
        mUnityPlayerActivity = null;
            //inProgress();
            c = mNotchService.capture(new NotchCallback<Void>() {
                @Override
                public void onProgress(NotchProgress progress) {
                    if (progress.getState() == NotchProgress.State.REALTIME_UPDATE) {
                        mData = (VisualiserData) progress.getObject();

                        //Log.i(LOGTAG, mData.toString());
                        refreshAngles();
                        //calculateAngles(mFrameIndex);
                        //Log.i(LOGTAG, RelativeNotchPosition);
                        updateRealTime();
                    }
                }

                @Override
                public void onSuccess(Void nothing) {
                    //clearText();
                }

                @Override
                public void onFailure(NotchError notchError) {
                    Util.showNotification(Util.getNotchErrorStr(notchError));
                    //clearText();
                }

                @Override
                public void onCancelled() {
                    Util.showNotification("Real-time measurement stopped!");
                    //clearText();
                }
            });
    }

    @OnClick(R.id.button_top_view)
    void onTopViewClicked() {
        startUnityPlayerActivity();
    }

    protected Intent unityIntent;

    private void updateRealTime() {
        if (mUnityPlayerActivity == null) {
            unityIntent = new Intent(this, UnityPlayerActivity.class);
            unityIntent.putExtra("message", RelativeNotchPosition);
            unityIntent.putExtra("ChestAnteriorTilt", ChestAnteriorTiltAngle);
            unityIntent.putExtra("ChestRotation", ChestRotationAngle);
            unityIntent.putExtra("ChestLateralTilt", ChestLateralTiltAngle);
            //unityIntent.putExtra("message", mData.toString());
            startActivity(unityIntent);
        }
        else {
            //unityIntent.putExtra("message", mData.toString());
            unityIntent.putExtra("message", RelativeNotchPosition);
            unityIntent.putExtra("ChestAnteriorTilt", ChestAnteriorTiltAngle);
            unityIntent.putExtra("ChestRotation", ChestRotationAngle);
            unityIntent.putExtra("ChestLateralTilt", ChestLateralTiltAngle);
           // EventBus.getDefault().post(mData);
        }
    }

}
