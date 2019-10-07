package com.wearnotch.notchdemo.visualiser;

import android.content.Context;
import android.opengl.GLSurfaceView;
import android.util.AttributeSet;
import android.view.GestureDetector;
import android.view.MotionEvent;
import android.view.ScaleGestureDetector;

import com.wearnotch.visualiser.NotchSkeletonRenderer;

public class TouchGLView extends GLSurfaceView implements GestureDetector.OnGestureListener,
        ScaleGestureDetector.OnScaleGestureListener {

    private NotchSkeletonRenderer mRenderer;
    private GestureDetector mTapDetector;
    private ScaleGestureDetector mScaleDetector;
    private float mLastSpan = 0;
    private long mLastNonTapTouchEventTimeNS = 0;

    public TouchGLView(Context context) {
        super(context);
        init();
    }

    public TouchGLView(Context context, AttributeSet attrs) {
        super(context, attrs);
        init();
    }

    private void init() {
        // Use Android's built-in gesture detectors to detect
        // which touch event the user is doing.
        mTapDetector = new GestureDetector(getContext(), this);
        mTapDetector.setIsLongpressEnabled(false);
        mScaleDetector = new ScaleGestureDetector(getContext(), this);

        // Create an OpenGL ES 2.0 context.
        setEGLContextClientVersion(2);
    }

    @Override
    public void setRenderer(Renderer renderer) {
        if (renderer instanceof NotchSkeletonRenderer) {
            mRenderer = (NotchSkeletonRenderer) renderer;
            super.setRenderer(renderer);
        } else {
            throw new IllegalArgumentException("Invalid renderer!");
        }
    }

    @Override
    public boolean onTouchEvent(final MotionEvent e) {
        // Forward touch events to the gesture detectors.
        mScaleDetector.onTouchEvent(e);
        mTapDetector.onTouchEvent(e);
        return true;
    }

    @Override
    public boolean onScale(ScaleGestureDetector detector) {
        // Forward the scale event to the renderer.
        final float amount = detector.getCurrentSpan() - mLastSpan;
        queueEvent(new Runnable() {
            public void run() {
                // This Runnable will be executed on the render
                // thread.
                // In a real app, you'd want to divide this by
                // the display resolution first.
                mRenderer.zoom(amount);
            }});
        mLastSpan = detector.getCurrentSpan();
        mLastNonTapTouchEventTimeNS = System.nanoTime();
        return true;
    }

    @Override
    public boolean onScaleBegin(ScaleGestureDetector detector) {
        mLastSpan = detector.getCurrentSpan();
        return true;
    }

    @Override
    public void onScaleEnd(ScaleGestureDetector detector) {
    }

    @Override
    public boolean onDown(MotionEvent e) {
        return false;
    }

    @Override
    public boolean onFling(MotionEvent e1, MotionEvent e2, float vx, float vy) {
        return false;
    }

    @Override
    public void onLongPress(MotionEvent e) {
    }

    @Override
    public boolean onScroll(MotionEvent e1, MotionEvent e2,
                            final float dx, final float dy) {
        // Forward the drag event to the renderer.
        queueEvent(new Runnable() {
            public void run() {
                // This Runnable will be executed on the render
                // thread.
                // In a real app, you'd want to divide these by
                // the display resolution first.
                mRenderer.drag(dx, dy);
            }});
        mLastNonTapTouchEventTimeNS = System.nanoTime();
        return true;
    }

    @Override
    public void onShowPress(MotionEvent e) {
    }

    @Override
    public boolean onSingleTapUp(MotionEvent e) {
        // Have a short dead time after rotating and zooming,
        // to make erratic taps less likely.
        final double kDeadTimeS = 0.3;
        if ((System.nanoTime() - mLastNonTapTouchEventTimeNS) / 1e9f < kDeadTimeS)
            return true;

        // Copy x/y into local variables, because |e| is changed and reused for
        // other views after this has been called.
        //final int x = Math.round(e.getX());
        //final int y = Math.round(e.getY());

        // Run something on the render thread...
        queueEvent(new Runnable(){
            public void run() {
                // Here you could call a method on the renderer that
                // checks which object has been tapped.
                // ...once that's done, post the result back to the UI
                // thread:
                getHandler().post(new Runnable() {
                    @Override
                    public void run() {
                        // ...
                    }});
            }});
        return true;
    }
}
