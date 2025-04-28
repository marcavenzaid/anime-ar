using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Unity.Collections;
using Unity.Collections.LowLevel.Unsafe;
using UnityEngine.XR.ARFoundation;
using UnityEngine.XR.ARSubsystems;

[RequireComponent(typeof(ARFace))]
public class FaceMovementTracker : MonoBehaviour
{
    private ARFace arFace;
    private GameController gameController;

    private float eyebrowRaisedHeight;
    private float mouthVerticalOpenness;
    private float mouthHorizontalOpenness;

    void Awake() {
        arFace = GetComponent<ARFace>();
    }

    void Start() {
        gameController = GameObject.FindGameObjectWithTag("GameController").GetComponent<GameController>();
    }

    void Update() {
        bool isTracking = (arFace.trackingState == TrackingState.Tracking) && (ARSession.state > ARSessionState.Ready);

        if (isTracking) {
            Vector3 leftEyebrowVertex = arFace.vertices[296];
            Vector3 leftCheeksVertex = arFace.vertices[451];

            Vector3 mouthMiddleTopVertex = arFace.vertices[13];
            Vector3 mouthMiddleBottomVertex = arFace.vertices[14];
            Vector3 mouthMiddleLeftVertex = arFace.vertices[76];
            Vector3 mouthMiddleRightVertex = arFace.vertices[306];

            eyebrowRaisedHeight = Vector3.Distance(leftEyebrowVertex, leftCheeksVertex);

            mouthVerticalOpenness = Vector3.Distance(mouthMiddleTopVertex, mouthMiddleBottomVertex);
            mouthHorizontalOpenness = Vector3.Distance(mouthMiddleRightVertex, mouthMiddleLeftVertex);

            gameController.SetEyebrowRaisedHeight(eyebrowRaisedHeight);
            gameController.SetMouthHorizontalOpenness(mouthVerticalOpenness);
            gameController.SetMouthVerticalOpenness(mouthHorizontalOpenness);
        }
    }

    public float GetEyebrowRaisedHeight() {
        return eyebrowRaisedHeight;
    }

    public float GetMouthVerticalOpenness() {
        return mouthVerticalOpenness;
    }

    public float GetMouthHorizontalOpenness() {
        return mouthHorizontalOpenness;
    }
}
