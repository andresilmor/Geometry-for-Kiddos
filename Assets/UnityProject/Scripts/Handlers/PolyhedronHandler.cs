using MixedReality.Toolkit;
using MixedReality.Toolkit.SpatialManipulation;
using MixedReality.Toolkit.UX;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using UnityEngine;

public class PolyhedronHandler : MonoBehaviour {

    [Header("Basic Info:")]
    public string SolidDesignation = "Cube";
    public ShapeBaseFormat BaseFormat;

    Guid _identifier;
    public Guid Identifier { get { return _identifier; } }

    [Header("Math Formulas:")]



    [Header("References:")]
    [SerializeField] GameObject _solid;
    [SerializeField] EdgesManager _edges;
    [SerializeField] VerticesManager _vertices;
    [SerializeField] FacesManager _surfaces;
    [SerializeField] PhysicsHandler _physics;
    [SerializeField] MeshRenderer _mesh;

    ObjectManipulator _solidManipulator;
    BoundsControl _solidBounds;

    public GameObject Solid { get { return _solid; } }
    public EdgesManager Edges { get { return _edges; } }
    public VerticesManager Vertices { get { return _vertices; } }
    public FacesManager Surfaces { get { return _surfaces; } }
    public PhysicsHandler Physics { get { return _physics; } }
    public MeshRenderer Mesh { get { return _mesh; } }

    public ObjectManipulator ObjectManipulator { get { return _solidManipulator; } }

    [HideInInspector] public bool isSolidColor = true;
    [HideInInspector] public UnityEngine.Color backupColor;

    #region Personalization

    [Serializable]
    public class Personalization {
        public EditSolidOption option;
        public bool boolValue;
        public string stringValue;
        public void SetBool(bool value) { boolValue = value; }
        public bool GetBool() { return boolValue; }
        public void SetString(string value) { stringValue = value; }
        public bool GetString() { return boolValue; }
        public EditSolidOption GetOption() { return option; }
    }


    [Header("Personalization:")]
    [SerializeField] Personalization[] _personalizationValues;
    public Dictionary<EditSolidOption, Personalization> solidPersonalization = new Dictionary<EditSolidOption, Personalization>();
   

    #endregion

    private void Awake() {

    }

    void Start() {
        foreach (Personalization personalization in _personalizationValues)
            solidPersonalization.Add(personalization.GetOption(), personalization);
     
        _identifier = Guid.NewGuid(); 
        GameManager.Instance.solidsActives.Add(_identifier, this);

        _solidManipulator = Solid.GetComponent<ObjectManipulator>();
        _solidBounds = Solid.GetComponent<BoundsControl>();

        _edges.BindSolid(this);
        _vertices.BindSolid(this);

        switch (GameManager.ApplicationMode) {
            case ApplicationMode.Manipulate:
                ObjectManipulator.AllowedManipulations = TransformFlags.Move | TransformFlags.Rotate | TransformFlags.Scale;
                _solidBounds.ToggleHandlesOnClick = true;
                break;

            case ApplicationMode.Edit:
                ObjectManipulator.AllowedManipulations = TransformFlags.None;
                _solidBounds.HandlesActive = false;
                _solidBounds.ToggleHandlesOnClick = false;
                break;

        }

        GameManager.OnApplicationModeChange.Add((ApplicationMode mode) => {
            switch (mode) {
                case ApplicationMode.Manipulate:
                    GameManager.Instance.EditPolyhedronMenu?.gameObject.SetActive(false);
                    ObjectManipulator.AllowedManipulations = TransformFlags.Move | TransformFlags.Rotate | TransformFlags.Scale;
                    _solidBounds.ToggleHandlesOnClick = true;
                    break;

                case ApplicationMode.Edit:  
                    ObjectManipulator.AllowedManipulations = TransformFlags.None;
                    _solidBounds.HandlesActive = false;
                    _solidBounds.ToggleHandlesOnClick = false;
                    GameManager.Instance.EditPolyhedronMenu?.gameObject.SetActive(false);
                    break;

            }

        });

    }

    public void ResetEdges() {
        _edges.ResetEdges();
    }

    public void DisplayEditPanel() {
        GameManager.Instance.EditPolyhedronMenu.gameObject.SetActive(true);



        Vector3 position = Camera.main.transform.position + Camera.main.transform.forward * 0.88f;
        position.y = 1.6f;

        position = Vector3.Lerp(Camera.main.gameObject.transform.position, gameObject.transform.position, 0.5f);
        position.y = 1.6f;

        GameManager.Instance.EditPolyhedronMenu.gameObject.transform.position = position;

    }

    public void OnClick() {
        Debug.Log(GameManager.ApplicationMode);
        switch (GameManager.ApplicationMode) {
            case ApplicationMode.Edit:
                if (GameManager.Instance.EditPolyhedronMenu.BindedSolid != this) {
                    Debug.Log("Binding Solid");
                    GameManager.Instance.EditPolyhedronMenu.BindSolid(this);
                    DisplayEditPanel();

                } else {
                    if (!GameManager.Instance.EditPolyhedronMenu.gameObject.activeSelf)
                        DisplayEditPanel();

                    switch (GameManager.Instance.EditPolyhedronMenu.EditMethod) {
                        case EditMethod.Paint: {
                                if (GameManager.Instance.EditPolyhedronMenu.individualPaint) {
                                    try {
                                        Debug.Log(GameManager.Instance.EditPolyhedronMenu.pickedColor);

                                        (GameManager.Instance.EditPolyhedronMenu.selectedComponentToPaint as FaceHandler).PaintFace(GameManager.Instance.EditPolyhedronMenu.pickedColor);

                                    } catch (Exception e) {
                                        Debug.Log(e.Message);
                                    }
                                } else {
                                    isSolidColor = GameManager.Instance.EditPolyhedronMenu.pickedColor.a >= 0.9f;
                                    _surfaces.PaintAllFaces(GameManager.Instance.EditPolyhedronMenu.pickedColor);

                                }

                                break;
                            }

                    }


                }
                break;

        }

    }

    void OnDisable() {
        if (GameManager.Instance.EditPolyhedronMenu != null)
            GameManager.Instance.EditPolyhedronMenu.gameObject.gameObject.SetActive(false);


    }

}
