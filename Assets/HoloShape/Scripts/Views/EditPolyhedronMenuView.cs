using MixedReality.Toolkit;
using MixedReality.Toolkit.UX;
using System;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;

public class EditPolyhedronMenuView : MonoBehaviour {
    
    [Header("Panel Config:")]
    [SerializeField] TextMeshPro _panelTitle;
    [SerializeField] float _panelOffsetX = 0.33f;

    public SolidEditMode editMode = SolidEditMode.None;
    public EditMethod editMethod = EditMethod.None;

    [Serializable]
    public struct SubPanel {
        public SolidEditMode editMode;
        public GameObject panel;
    }

    [SerializeField] SubPanel[] subPanels;
    Dictionary<SolidEditMode, GameObject> _subPanels = new Dictionary<SolidEditMode, GameObject>();

    [Header("Components References:")]
    [SerializeField] PressableButton _hideSolidCheckbox;
    [SerializeField] TextMeshPro _learnTextBox;

    GameObject _currentSubPanel = null;
    PolyhedronHandler _bindedSolid;

    public PolyhedronHandler bindedSolid { get { return _bindedSolid; } }

    #region Personalization
    [Serializable]
    public class Personalization {
        public EditSolidOption option;
        public PressableButton button;
        public PressableButton GetButton() { return button; }
        public EditSolidOption GetOption() { return option; }

    }

    [SerializeField] Personalization[] solidPersonalization;

    #endregion


    // Start is called before the first frame update
    void Start() {
        foreach (SubPanel subPanel in subPanels) { 
            _subPanels.Add(subPanel.editMode, subPanel.panel);
            subPanel.panel.SetActive(false);
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

            if (this.editMode != SolidEditMode.Learn && (SolidEditMode)editMode == SolidEditMode.Learn)
                OnEnableLearn();

            this.editMode = (SolidEditMode)editMode;
            editMethod = EditMethod.None;

        }

        InvalidateEditMethod(new EditMethod[] { EditMethod.Paint });

    }

    public void OnClosePanel() {
        GameManager.Instance.handMenu.SetApplicationMode(0);
        InvalidateEditMethod(new EditMethod[] { EditMethod.Paint });

    }

    public void BindSolid(PolyhedronHandler solid) {
        _bindedSolid = solid;
        Debug.Log("Binding " + solidPersonalization.Length);
        _panelTitle.text = "Editing " + _bindedSolid.solidDesignation;
        Debug.Log("Binding " + solidPersonalization.Length);
        InvalidateEditMethod(new EditMethod[] { EditMethod.Paint });

        foreach (Personalization personalization in solidPersonalization) {
            Debug.Log("EditSolidScreen Personalization");
            personalization.GetButton().ForceSetToggled(_bindedSolid.solidPersonalization[personalization.GetOption()].GetBool());

        }

    }

    public void InvalidateEditMethod(EditMethod[] editMethodToInvalidate) {
        foreach (EditMethod method in editMethodToInvalidate)
            if (editMethod == method)
                editMethod = EditMethod.None;

    }

    public void DeleteShape() {
        _bindedSolid.gameObject.SetActive(false);
        foreach (KeyValuePair<SolidEditMode, GameObject> subPanel in _subPanels) {
            subPanel.Value.SetActive(false);
        }


    }

    #region Edit Edges

    public void ToggleEdgeLetters(bool toShow) {
        _bindedSolid.solidPersonalization[EditSolidOption.EdgesShowLetters].SetBool(toShow);
        foreach (EdgeHandler edge in _bindedSolid.edges.List())
            edge.letter.gameObject.SetActive(toShow);

    }

    public void ToggleEdgeMarkers(bool toShow) {
        _bindedSolid.solidPersonalization[EditSolidOption.EdgesShowMarkers].SetBool(toShow);
        foreach (EdgeHandler edge in _bindedSolid.edges.List())
            edge.mesh.gameObject.SetActive(toShow);

    }

    public void ToggleOcclusion(bool toEnable) {
        _bindedSolid.solidPersonalization[EditSolidOption.EdgesOcclusion].SetBool(toEnable);
        _bindedSolid.edges.enabledOcclusion = toEnable;

    }

    public void ToggleGlobalOcclusion(bool toEnable) {
        _bindedSolid.solidPersonalization[EditSolidOption.EdgesGlobalOcclusion].SetBool(toEnable);
        _bindedSolid.edges.enabledGlobalOcclusion = toEnable;

    }

    public void ToggleSolidVisibility(bool isVisible) {
        _bindedSolid.solidPersonalization[EditSolidOption.SurfacesHideSolid].SetBool(isVisible);
        foreach (SurfaceHandler surface in _bindedSolid.surfaces.List()) {
            surface.SetVisibility(isVisible);

        }

        _bindedSolid.isSolidColor = isVisible;
        _bindedSolid.mesh.enabled = !isVisible;

        if (!isVisible) {
            _bindedSolid.mesh.material = GameManager.Instance.transparentMaterial;
            return;

        }

        _bindedSolid.mesh.material = GameManager.Instance.defaultMaterial;



    }

    public void ToggleOutline(bool toEnable) {
        _bindedSolid.solidPersonalization[EditSolidOption.EdgesModeOutline].SetBool(toEnable);
        _bindedSolid.mesh.enabled = toEnable;
        foreach (SurfaceHandler surface in _bindedSolid.surfaces.List())
            surface.SetVisibility(!toEnable);

        if (toEnable) {
            _bindedSolid.mesh.material = GameManager.Instance.outlineMaterial;
            return;

        }

        _bindedSolid.mesh.material = GameManager.Instance.defaultMaterial;
        //foreach (EdgeHandler edge in _bindedSolid.SolidEdges) {
         //   edge.ResetLineMaterial();
           // edge.ResetOcclusionedVariables();

 //       }

    }

    #endregion

    #region Edit Vertices

    public void ToggleVerticeLetters(bool toShow) {
        _bindedSolid.solidPersonalization[EditSolidOption.VerticesShowLetters].SetBool(toShow);
        foreach (VerticeHandler vertice in _bindedSolid.vertices.List())
            vertice.letter.gameObject.SetActive(toShow);

    }

    public void ToggleVerticeMarkers(bool toShow) {
        _bindedSolid.solidPersonalization[EditSolidOption.VerticesShowMarkers].SetBool(toShow);
        foreach (VerticeHandler vertice in _bindedSolid.vertices.List())
            vertice.mesh.gameObject.SetActive(toShow);

    }

    #endregion

    #region Edit Physics

    public void ToggleGravity(bool toEnable) {
        _bindedSolid.solidPersonalization[EditSolidOption.PhysicsGravity].SetBool(toEnable);
        _bindedSolid.physics.rigidbody.isKinematic = !toEnable;

    }

    public void ToggleCollision(bool toEnable) {
        _bindedSolid.solidPersonalization[EditSolidOption.PhysicsCollision].SetBool(toEnable);
        if (toEnable) {
            _bindedSolid.physics.rigidbody.gameObject.layer = LayerMask.NameToLayer("Default");
            _bindedSolid.physics.rigidbody.isKinematic = false;
            _bindedSolid.physics.rigidbody.useGravity = false;
            _bindedSolid.physics.rigidbody.velocity = Vector3.zero;

        } else { 
            _bindedSolid.physics.rigidbody.gameObject.layer = LayerMask.NameToLayer("Ignore Rigidbody");
            _bindedSolid.physics.rigidbody.isKinematic = true;
            _bindedSolid.physics.rigidbody.useGravity = true;
            _bindedSolid.physics.rigidbody.velocity = Vector3.one;

        }

    }

    #endregion

    #region Color

    UnityEngine.Color _pickedColor;
    bool _individualPaint = false;
    public bool individualPaint { get { return _individualPaint; } }
    public UnityEngine.Color pickedColor { get { return _pickedColor; } }
    [HideInInspector] public SolidComponent selectedComponentToPaint;

    public void ToggleIndividualPaint(bool toEnable) {
        _bindedSolid.solidPersonalization[EditSolidOption.ColorIndividualPaint].SetBool(toEnable);
        _individualPaint = toEnable;

        if (editMethod.Equals(EditMethod.Paint))
            editMethod = EditMethod.None;

    }

    public void PickColor(string hexColor) {
        if (_individualPaint) { 
            editMethod = EditMethod.Paint;
            _pickedColor = Parser.HexToColor(hexColor);
            return;
        
        }

        _bindedSolid.surfaces.PaintAllSurfaces(Parser.HexToColor(hexColor));

    }

    #endregion

    private async void OnEnableLearn() {
        _learnTextBox.text = await ChatGPTController.AskChatGPT("With less than 408 characters (counting with spaces), explain, in english and as if talking with a 10 years old student, what a " + _bindedSolid.solidDesignation + " is.");

    }

    private void OnEnable() {

        if (_bindedSolid != null) {
            _hideSolidCheckbox.ForceSetToggled(!_bindedSolid.isSolidColor);

        }

    }

    private void OnDisable() {
        foreach (SubPanel subPanel in subPanels)
            subPanel.panel.SetActive(false);

        InvalidateEditMethod(new EditMethod[] { EditMethod.Paint });

    }

}
