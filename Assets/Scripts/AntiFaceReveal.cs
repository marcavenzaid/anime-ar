using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AntiFaceReveal : MonoBehaviour
{
    public bool disableAntiFaceReveal = true;
    public GameObject faceCover;
    public FaceAugmentationChecker faceAugmentationChecker;

    void Start()
    {
        // Make sure that it is active at the very start to avoid showing the face.
        faceCover.SetActive(true);
    }

    void Update()
    {
        CoverScreen();
    }

    private void CoverScreen()
    {
        // this is temporary
        if (disableAntiFaceReveal) {
            faceCover.SetActive(false);
            return;
        }

        if (faceAugmentationChecker.IsFaceAugmented()) {
            faceCover.SetActive(false);
        } else {
            faceCover.SetActive(true);
        }
    }
}
