using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class InteractPolyhedronMenuView : MonoBehaviour
{

    [Header("Menu Config:")]
    [SerializeField] TextMeshPro _menuTitle;
    [SerializeField] float _menuOffsetX = 0.33f;

    [Serializable]
    public struct SubPanel {
        public SolidEditMode EditMode;
        public GameObject Panel;
    }

    [SerializeField] SubPanel[] SubPanels;
    Dictionary<SolidEditMode, GameObject> _subPanels = new Dictionary<SolidEditMode, GameObject>();


    GameObject _currentSubPanel = null;

    PolyhedronHandler _bindedSolid;
    public PolyhedronHandler BindedSolid { get { return _bindedSolid; } }

    void Start() {
        foreach (SubPanel subPanel in SubPanels) {
            _subPanels.Add(subPanel.EditMode, subPanel.Panel);
            subPanel.Panel.SetActive(false);
        }

    }

    void LateUpdate() {
        transform.LookAt(gameObject.transform.position - (Camera.main.transform.position - gameObject.transform.position));

    }


}
