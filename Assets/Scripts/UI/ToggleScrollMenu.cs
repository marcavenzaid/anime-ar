using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ToggleScrollMenu : MonoBehaviour
{
    public GameObject scrollMenu; // Assign the Scroll View GameObject in the Inspector
    public bool scrollMenuActiveAtStart = false;

    private bool isMenuVisible;

    private void Start() {
        scrollMenu.SetActive(scrollMenuActiveAtStart);

        isMenuVisible = scrollMenu.activeSelf;
    }

    public void ToggleMenu() {
        isMenuVisible = !isMenuVisible;
        scrollMenu.SetActive(isMenuVisible);
    }
}
