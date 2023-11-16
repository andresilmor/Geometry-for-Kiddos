using MixedReality.Toolkit;
using MixedReality.Toolkit.UX;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;

public class EditSolidScreen : MonoBehaviour {
    
    [Header("Panel Config:")]
    [SerializeField] TextMeshPro panelTitle;
    [SerializeField] float panelOffsetX = 0.33f;

    [Serializable]
    public struct SubPanel {
        public string Name;
        public GameObject Panel;
    }

    public SubPanel[] SubPanels;
    Dictionary<string, GameObject> _subPanels = new Dictionary<string, GameObject>();

    [Header("Components References:")]
    [SerializeField] PressableButton _hideSolidCheckbox;

    GameObject _currentSubPanel = null;
    SolidHandler _bindedSolid;


    // Start is called before the first frame update
    void Start() {
        foreach (SubPanel subPanel in SubPanels) { 
            _subPanels.Add(subPanel.Name, subPanel.Panel);
            subPanel.Panel.SetActive(false);
        }

    }

    void LateUpdate() {
        transform.LookAt(gameObject.transform.position - (Camera.main.transform.position - gameObject.transform.position));

    }

    public void OpenSubPanel(string key) {
        if (_currentSubPanel != null)
            _currentSubPanel.SetActive(false);

        if (_subPanels.ContainsKey(key)) { 
            _subPanels[key].SetActive(true);
            _currentSubPanel = _subPanels[key];

        }

    }


    public void BindSolid(SolidHandler solid) {
        _bindedSolid = solid;
        panelTitle.text = "Editar " + _bindedSolid.SolidDesignation;

    }

    public void OnClosePanel() {
        Controller.ApplicationMode = ApplicationMode.Manipulate;

    }


    public void ToggleManipulation(bool toAllow) {
        if (toAllow)
            _bindedSolid.ObjectManipulator.AllowedManipulations = TransformFlags.Move | TransformFlags.Rotate | TransformFlags.Scale;
        else
            _bindedSolid.ObjectManipulator.AllowedManipulations = TransformFlags.None;

    }

    public void ToggleSolidVisibility(bool isVisible) {
        //_bindedSolid.SetColor(_bindedSolid.SolidMeshRenderer.material.color);


        foreach (MeshRenderer mesh in _bindedSolid.SolidSurfaces) {
            mesh.enabled = isVisible;

        }

        _bindedSolid.IsSolidColor = isVisible;
        _bindedSolid.SolidMeshRenderer.enabled = !isVisible;

        if (!isVisible) {
            _bindedSolid.SolidMeshRenderer.material = Controller.Instance.TransparentMaterial;
            return;

        }

        _bindedSolid.SolidMeshRenderer.material = Controller.Instance.DefaultMaterial;



    }

    public void ToggleOutline(bool toEnable) {
        _bindedSolid.SolidMeshRenderer.enabled = toEnable;
        foreach (MeshRenderer meshRenderer in _bindedSolid.SolidSurfaces)
            meshRenderer.enabled = !toEnable;

        if (toEnable) {
            _bindedSolid.SolidMeshRenderer.material = Controller.Instance.OutlineMaterial;
            return;

        }

        _bindedSolid.SolidMeshRenderer.material = Controller.Instance.DefaultMaterial;
        //foreach (EdgeHandler edge in _bindedSolid.SolidEdges) {
         //   edge.ResetLineMaterial();
           // edge.ResetOcclusionedVariables();

 //       }

    }

    public void ToggleVerticeLetters(bool toShow) {
        foreach (VerticeHandler vertice in _bindedSolid.SolidVertices)
            vertice.Letter.gameObject.SetActive(toShow);

    }

    public void ToggleVerticeMarkers(bool toShow) {
        foreach (VerticeHandler vertice in _bindedSolid.SolidVertices)
            vertice.Mesh.gameObject.SetActive(toShow);

    }

    public void ToggleEdgeLetters(bool toShow) {
        foreach (EdgeHandler edge in _bindedSolid.SolidEdges)
            edge.Letter.gameObject.SetActive(toShow);

    }

    public void ToggleEdgeMarkers(bool toShow) {
        foreach (EdgeHandler edge in _bindedSolid.SolidEdges)
            edge.Mesh.gameObject.SetActive(toShow);

    }

    public void ToggleOcclusion(bool toEnable) {
        Debug.Log("Occlusion: " + toEnable);
        Debug.Log("Global Occlusion: " + _bindedSolid.GloabalCollision);
        _bindedSolid.EnabledOcclusion = toEnable;

    }

    public void ToggleGlobalOcclusion(bool toEnable) {
        _bindedSolid.GloabalCollision = toEnable;

    }

    public void ToggleGravity(bool toEnable) {
        _bindedSolid.SolidRigidbody.isKinematic = !toEnable;

    }

    public void ToggleCollision(bool toEnable) {
        if (toEnable) {
            _bindedSolid.SolidRigidbody.gameObject.layer = LayerMask.NameToLayer("Default");
            _bindedSolid.SolidRigidbody.isKinematic = false;
            _bindedSolid.SolidRigidbody.useGravity = false;
            _bindedSolid.SolidRigidbody.velocity = Vector3.zero;

        } else { 
            _bindedSolid.SolidRigidbody.gameObject.layer = LayerMask.NameToLayer("Ignore Rigidbody");
            _bindedSolid.SolidRigidbody.isKinematic = true;
            _bindedSolid.SolidRigidbody.useGravity = true;
            _bindedSolid.SolidRigidbody.velocity = Vector3.one;

        }

    }

    public void SetSolidColor(string hexColor) {
        _bindedSolid.SetColor(Parser.HexToColor(hexColor));
        _hideSolidCheckbox.ForceSetToggled(false);

    }

    private void OnEnable() {
        if (_bindedSolid != null) {
            gameObject.transform.position = new Vector3(
                    _bindedSolid.Solid.gameObject.transform.position.x + (_bindedSolid.SolidCollider.size.x / 2),
                    _bindedSolid.Solid.gameObject.transform.position.y,
                    _bindedSolid.Solid.gameObject.transform.position.z
                );

            _hideSolidCheckbox.ForceSetToggled(!_bindedSolid.IsSolidColor);

        }

    }

    private void OnDisable() {
        foreach (SubPanel subPanel in SubPanels)
            subPanel.Panel.SetActive(false);


    }

}
