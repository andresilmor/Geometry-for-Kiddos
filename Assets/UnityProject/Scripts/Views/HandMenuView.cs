using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class HandMenuView : MonoBehaviour
{
    [Header("Text References:")]
    [SerializeField] TextMeshPro _currentMode;

    public void SetApplicationMode(int mode) {
        GameManager.ApplicationMode = (ApplicationMode)mode;
        _currentMode.text = GameManager.ApplicationMode.ToString();
        Debug.Log("Setting to " + mode + "(" + GameManager.ApplicationMode.ToString() + ")");

    }

}
