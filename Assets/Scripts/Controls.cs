using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Controls : MonoBehaviour
{

    [SerializeField]
    private bool hasGlasses = false;
    [SerializeField]
    private bool trackerStatsToggleController = true;

    private enum CharacterState
    {
        Normal,
        Glasses,
    }

    private List<GameObject> glasses;
    private CharacterState currentState;

    private Text trackerStatsText;

    private void Start() {
        glasses = FindChildrenWithTag(gameObject, "Glasses");
        if (glasses == null) {
            Debug.LogError("Glasses not found.");
        }

        currentState = CharacterState.Normal;
        ChangeState(CharacterState.Normal);

        trackerStatsText = GameObject.Find("Tracker Stats").GetComponent<Text>();
        trackerStatsText.enabled = false;
    }

    void Update() {
        if (Input.touchCount > 0) {
            Touch touch = Input.GetTouch(0);

            if (Input.touchCount == 1) {
                if (touch.phase == TouchPhase.Ended) {
                    if (hasGlasses) {
                        ToggleGlasses();
                    }
                }
            }
            
            if (Input.touchCount == 4) {
                if (touch.phase == TouchPhase.Ended) {
                    if (trackerStatsToggleController) {
                        ToggleTextOverlay();
                    }
                }
            }
        }
    }

    private void ToggleGlasses() {
        if (currentState == CharacterState.Normal) {
            ChangeState(CharacterState.Glasses);
        } else if (currentState == CharacterState.Glasses) {
            ChangeState(CharacterState.Normal);
        }
    }

    private void ChangeState(CharacterState state) {
        foreach (GameObject g in glasses) {
            if (state == CharacterState.Normal) {
                g.SetActive(true);
            } else if (state == CharacterState.Glasses) {
                g.SetActive(false);
            }
        }

        currentState = state;
    }

    private void ToggleTextOverlay() {
        if (trackerStatsText.isActiveAndEnabled) {
            trackerStatsText.enabled = false;
        } else {
            trackerStatsText.enabled = true;
        }
    }

    private List<GameObject> FindChildrenWithTag(GameObject parent, string tag) {
        List<GameObject> children = new List<GameObject>();

        foreach (Transform t in parent.transform) {
            if (t.CompareTag(tag)) {
                children.Add(t.gameObject);
            }
        }

        return children;
    }
}
