using MixedReality.Toolkit;
using MixedReality.Toolkit.UX;
using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class EditSolidView : MonoBehaviour {
    
    [Header("Panel Config:")]
    [SerializeField] TextMeshPro panelTitle;
    [SerializeField] float panelOffsetX = 0.33f;

    public SolidEditMode EditMode = SolidEditMode.None;

    [Serializable]
    public struct SubPanel {
        public SolidEditMode EditMode;
        public GameObject Panel;
    }

    [SerializeField] SubPanel[] SubPanels;
    Dictionary<SolidEditMode, GameObject> _subPanels = new Dictionary<SolidEditMode, GameObject>();

    [Header("Components References:")]
    [SerializeField] PressableButton _hideSolidCheckbox;

    GameObject _currentSubPanel = null;
    SolidHandler _bindedSolid;

    #region Personalization
    [Serializable]
    public class Personalization {
        public EditSolidOption Value;
        public PressableButton Checkbox;
        public PressableButton GetCheckbox() { return Checkbox; }
        public EditSolidOption GetValue() { return Value; }

    }

    [SerializeField] Personalization[] SolidPersonalization;

    #endregion


    // Start is called before the first frame update
    void Start() {

        foreach (SubPanel subPanel in SubPanels) { 
            _subPanels.Add(subPanel.EditMode, subPanel.Panel);
            subPanel.Panel.SetActive(false);
        }

    }

    void LateUpdate() {
        transform.LookAt(gameObject.transform.position - (Camera.main.transform.position - gameObject.transform.position));

    }

    public void OpenEditModePanel(int editMode) {
        if (_currentSubPanel != null)
            _currentSubPanel.SetActive(false);

        if (_subPanels.ContainsKey((SolidEditMode) editMode)) { 
            _subPanels[(SolidEditMode)editMode].SetActive(true);
            _currentSubPanel = _subPanels[(SolidEditMode)editMode];

            EditMode = (SolidEditMode)editMode;

        }

    }

    public void OnClosePanel() {
        Controller.Instance.HandMenu.SetApplicationMode(0);

    }

    public void BindSolid(SolidHandler solid) {
        _bindedSolid = solid;
        Debug.Log("Binding " + SolidPersonalization.Length);
        panelTitle.text = "Editar " + _bindedSolid.SolidDesignation;
        Debug.Log("Binding " + SolidPersonalization.Length);
        
        foreach (Personalization personalization in SolidPersonalization) {
            Debug.Log("EditSolidScreen Personalization");
            personalization.GetCheckbox().ForceSetToggled(_bindedSolid.SolidPersonalization[personalization.GetValue()].IsToggled());

        }

    }

    #region Edit Edges

    public void ToggleEdgeLetters(bool toShow) {
        foreach (EdgeHandler edge in _bindedSolid.Edges.List())
            edge.Letter.gameObject.SetActive(toShow);

    }

    public void ToggleEdgeMarkers(bool toShow) {
        foreach (EdgeHandler edge in _bindedSolid.Edges.List())
            edge.Mesh.gameObject.SetActive(toShow);

    }

    public void ToggleOcclusion(bool toEnable) {
        _bindedSolid.Edges.EnabledOcclusion = toEnable;

    }

    public void ToggleGlobalOcclusion(bool toEnable) {
        _bindedSolid.Edges.EnabledGlobalOcclusion = toEnable;

    }

    public void ToggleSolidVisibility(bool isVisible) {
        //_bindedSolid.SetColor(_bindedSolid.SolidMeshRenderer.material.color);

        foreach (MeshRenderer mesh in _bindedSolid.Surfaces.List()) {
            mesh.enabled = isVisible;

        }

        _bindedSolid.IsSolidColor = isVisible;
        _bindedSolid.Mesh.enabled = !isVisible;

        if (!isVisible) {
            _bindedSolid.Mesh.material = Controller.Instance.TransparentMaterial;
            return;

        }

        _bindedSolid.Mesh.material = Controller.Instance.DefaultMaterial;



    }

    public void ToggleOutline(bool toEnable) {
        _bindedSolid.Mesh.enabled = toEnable;
        foreach (MeshRenderer meshRenderer in _bindedSolid.Surfaces.List())
            meshRenderer.enabled = !toEnable;

        if (toEnable) {
            _bindedSolid.Mesh.material = Controller.Instance.OutlineMaterial;
            return;

        }

        _bindedSolid.Mesh.material = Controller.Instance.DefaultMaterial;
        //foreach (EdgeHandler edge in _bindedSolid.SolidEdges) {
         //   edge.ResetLineMaterial();
           // edge.ResetOcclusionedVariables();

 //       }

    }

    #endregion

    #region Edit Vertices

    public void ToggleVerticeLetters(bool toShow) {
        foreach (VerticeHandler vertice in _bindedSolid.Vertices.List())
            vertice.Letter.gameObject.SetActive(toShow);

    }

    public void ToggleVerticeMarkers(bool toShow) {
        foreach (VerticeHandler vertice in _bindedSolid.Vertices.List())
            vertice.Mesh.gameObject.SetActive(toShow);

    }

    #endregion

    #region Edit Physics

    public void ToggleGravity(bool toEnable) {
        _bindedSolid.Physics.Rigidbody.isKinematic = !toEnable;

    }

    public void ToggleCollision(bool toEnable) {
        if (toEnable) {
            _bindedSolid.Physics.Rigidbody.gameObject.layer = LayerMask.NameToLayer("Default");
            _bindedSolid.Physics.Rigidbody.isKinematic = false;
            _bindedSolid.Physics.Rigidbody.useGravity = false;
            _bindedSolid.Physics.Rigidbody.velocity = Vector3.zero;

        } else { 
            _bindedSolid.Physics.Rigidbody.gameObject.layer = LayerMask.NameToLayer("Ignore Rigidbody");
            _bindedSolid.Physics.Rigidbody.isKinematic = true;
            _bindedSolid.Physics.Rigidbody.useGravity = true;
            _bindedSolid.Physics.Rigidbody.velocity = Vector3.one;

        }

    }

    #endregion

    #region Color

    UnityEngine.Color? _pickedColor = null;
    bool _individualPaint = false;
    public bool IndividualPaint { get { return _individualPaint; } }

    public void ToggleIndividualPaint(bool toEnable) {
        Debug.Log("Value (" + toEnable + ") Before: " + _bindedSolid.SolidPersonalization[EditSolidOption.IndividualPaint].IsToggled());
        _bindedSolid.SolidPersonalization[EditSolidOption.IndividualPaint].SetToggle(toEnable);
        Debug.Log("Value (" + toEnable + ") After: " + _bindedSolid.SolidPersonalization[EditSolidOption.IndividualPaint].IsToggled());
        _individualPaint = toEnable;
    }

    public void PickColor(string hexColor) {
        _pickedColor = Parser.HexToColor(hexColor);

    }

    public void SetSolidColor(string hexColor) {
        _bindedSolid.SetColor(Parser.HexToColor(hexColor));
        _bindedSolid.ResetEdges();
        _hideSolidCheckbox.ForceSetToggled(false);

    }

    #endregion

    private void OnEnable() {
        if (_bindedSolid != null) {
            _hideSolidCheckbox.ForceSetToggled(!_bindedSolid.IsSolidColor);

        }

    }

    private void OnDisable() {
        foreach (SubPanel subPanel in SubPanels)
            subPanel.Panel.SetActive(false);

        EditMode = SolidEditMode.None;


    }

}
