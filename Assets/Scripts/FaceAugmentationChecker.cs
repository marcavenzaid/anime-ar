using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.ARFoundation;

[RequireComponent(typeof(ARFaceManager))]
public class FaceAugmentationChecker : MonoBehaviour
{
    private ARFaceManager arFaceManager;

    void Start()
    {
        arFaceManager = GetComponent<ARFaceManager>();
        arFaceManager.facesChanged += OnFacesChanged;
    }

    void OnDestroy() {
        arFaceManager.facesChanged -= OnFacesChanged;
    }

    private void OnFacesChanged(ARFacesChangedEventArgs eventArgs) {
        if (eventArgs.added.Count > 0 || eventArgs.updated.Count > 0) {
            //Debug.Log("Face is augmented.");
        } else if (eventArgs.removed.Count > 0) {
            //Debug.Log("Face is not augmented.");
        }
    }

    public bool IsFaceAugmented() {
        return arFaceManager.trackables.count > 0;
    }

}
