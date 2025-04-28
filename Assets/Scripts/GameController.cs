using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.XR.ARFoundation;

public class GameController : MonoBehaviour
{
    [SerializeField]
    private Text trackerStatsText;

    [SerializeField]
    private FaceAugmentationChecker faceAugmentationChecker;

    private float eyebrowRaisedHeight;
    private float mouthVerticalOpenness;
    private float mouthHorizontalOpenness;

    private bool isTrackerStatsTextEnabled = true;

    void Start() {
        trackerStatsText.enabled = false;
    }

    void Update() {
        if (Input.touchCount > 0) {
            Touch touch = Input.GetTouch(0);

            if (Input.touchCount == 2) {
                if (touch.phase == TouchPhase.Ended) {
                    
                }
            }

            if (Input.touchCount == 3) {
                if (touch.phase == TouchPhase.Ended) {
                    if (isTrackerStatsTextEnabled) {
                        ToggleTextOverlay();
                    }
                }
            }
        }

        UpdateTrackerStatsText();
    }

    private void ToggleTextOverlay() {
        trackerStatsText.enabled = !trackerStatsText.enabled;
    }

    private void UpdateTrackerStatsText() {
        trackerStatsText.text = "eyebrowRaisedHeight:     " + eyebrowRaisedHeight + "\n"
                              + "mouthVerticalOpenness:   " + mouthVerticalOpenness + "\n"
                              + "mouthHorizontalOpenness: " + mouthHorizontalOpenness + "\n"
                              + "isFaceAugmented:         " + faceAugmentationChecker.IsFaceAugmented();
    }

    public void SetEyebrowRaisedHeight(float eyebrowRaisedHeight) {
        this.eyebrowRaisedHeight = eyebrowRaisedHeight;
    }

    public void SetMouthVerticalOpenness(float mouthVerticalOpenness) {
        this.mouthVerticalOpenness = mouthVerticalOpenness;
    }

    public void SetMouthHorizontalOpenness(float mouthHorizontalOpenness) {
        this.mouthHorizontalOpenness = mouthHorizontalOpenness;
    }
}
