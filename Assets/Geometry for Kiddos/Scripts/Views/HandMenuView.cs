using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class HandMenuView : MonoBehaviour
{
    [Header("Text References:")]
    [SerializeField] TextMeshPro _currentMode;

    public void SetApplicationMode(int mode) {
        Controller.ApplicationMode = (ApplicationMode)mode;
        _currentMode.text = Controller.ApplicationMode.ToString();

    }

}
