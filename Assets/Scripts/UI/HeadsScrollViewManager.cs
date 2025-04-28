using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.XR.ARFoundation;
using UnityEngine.XR.ARSubsystems;

public class HeadsScrollViewManager : MonoBehaviour
{
    public ARFaceManager aRFaceManager;
    public GameObject headButtonPrefab;
    public Transform contentPanel;
    public TexturePrefabPair[] headTexturePrefabPair;

    private void Start() {
        PopulateScrollView();
    }

    void PopulateScrollView() {
        foreach (TexturePrefabPair pair in headTexturePrefabPair) {
            GameObject newButton = Instantiate(headButtonPrefab, contentPanel);
            newButton.GetComponent<RawImage>().texture = pair.texture;

            // Add click event listener, for using the head.
            newButton.GetComponent<Button>().onClick.AddListener(() => UseHead(pair));
        }
    }

    void UseHead(TexturePrefabPair pair) {
        // Update the face prefab.
        aRFaceManager.facePrefab = pair.prefab;

        StartCoroutine(ResetARFaceManager());

        Debug.Log("Using Head with sprite name: " + pair.texture.name + " and prefab name: " + pair.prefab.name);
    }

    private IEnumerator ResetARFaceManager() {
        aRFaceManager.enabled = false;

        // Wait for a frame to ensure the state changes are processed.
        yield return null;

        aRFaceManager.enabled = true;
    }
}

[System.Serializable]
public class TexturePrefabPair
{
    public Texture2D texture;
    public GameObject prefab;
}
