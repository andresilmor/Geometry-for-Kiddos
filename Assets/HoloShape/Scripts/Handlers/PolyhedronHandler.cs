using MixedReality.Toolkit;
using MixedReality.Toolkit.SpatialManipulation;
using MixedReality.Toolkit.UX;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using UnityEngine;

public class PolyhedronHandler : MonoBehaviour {
    public string solidDesignation = "Cube";
    Guid _identifier;
    public Guid identifier { get { return _identifier; } }  

    [SerializeField] GameObject _solid;
    [SerializeField] EdgesManager _edges;
    [SerializeField] VerticesManager _vertices;
    [SerializeField] SurfacesManager _surfaces;
    [SerializeField] PhysicsHandler _physics;
    [SerializeField] MeshRenderer _mesh;

    ObjectManipulator _solidManipulator;
    BoundsControl _solidBounds;

    public GameObject solid { get { return _solid; } }
    public EdgesManager edges { get { return _edges; } }
    public VerticesManager vertices { get { return _vertices; } }
    public SurfacesManager surfaces { get { return _surfaces; } }
    public PhysicsHandler physics { get { return _physics; } }
    public MeshRenderer mesh { get { return _mesh; } }

    public ObjectManipulator objectManipulator { get { return _solidManipulator; } }

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

        _solidManipulator = solid.GetComponent<ObjectManipulator>();
        _solidBounds = solid.GetComponent<BoundsControl>();


        _edges.BindSolid(this);
        _vertices.BindSolid(this);

        switch (GameManager.ApplicationMode) {
            case ApplicationMode.Manipulate:
                objectManipulator.AllowedManipulations = TransformFlags.Move | TransformFlags.Rotate | TransformFlags.Scale;

                _solidBounds.HandlesActive = true;
                break;

            case ApplicationMode.Edit:
                objectManipulator.AllowedManipulations = TransformFlags.None;
                _solidBounds.HandlesActive = false;
                break;

        }

        GameManager.OnApplicationModeChange.Add((ApplicationMode mode) => {
            switch (mode) {
                case ApplicationMode.Manipulate:
                    objectManipulator.AllowedManipulations = TransformFlags.Move | TransformFlags.Rotate | TransformFlags.Scale;
                    _solidBounds.HandlesActive = true;
                    break;

                case ApplicationMode.Edit:
                    Debug.Log("EditMode");
                    objectManipulator.AllowedManipulations = TransformFlags.None;
                    _solidBounds.HandlesActive = false;
                    GameManager.Instance.editPolyhedronMenu?.gameObject.SetActive(false);
                    break;

            }

        });

    }

    void Update() {

    }

    public void ResetEdges() {
        _edges.ResetEdges();
    }

    public void DisplayEditPanel() {
        GameManager.Instance.editPolyhedronMenu.gameObject.SetActive(true);
        Vector3 position = Camera.main.transform.position + Camera.main.transform.forward * 0.88f;
        position.y = 1.6f;
        GameManager.Instance.editPolyhedronMenu.gameObject.transform.position = position;

    }

    public void OnClick() {
        Debug.Log(GameManager.ApplicationMode);
        switch (GameManager.ApplicationMode) {
            case ApplicationMode.Edit:
                if (GameManager.Instance.editPolyhedronMenu.bindedSolid != this) {
                    Debug.Log("Binding Solid");
                    GameManager.Instance.editPolyhedronMenu.BindSolid(this);
                    DisplayEditPanel();

                } else {
                    if (!GameManager.Instance.editPolyhedronMenu.gameObject.activeSelf)
                        DisplayEditPanel();

                    switch (GameManager.Instance.editPolyhedronMenu.editMethod) {
                        case EditMethod.Paint: {
                                if (GameManager.Instance.editPolyhedronMenu.individualPaint) {
                                    try {
                                        Debug.Log(GameManager.Instance.editPolyhedronMenu.pickedColor);

                                        (GameManager.Instance.editPolyhedronMenu.selectedComponentToPaint as SurfaceHandler).PaintSurface(GameManager.Instance.editPolyhedronMenu.pickedColor);

                                    } catch (Exception e) {
                                        Debug.Log(e.Message);
                                    }
                                } else {
                                    isSolidColor = GameManager.Instance.editPolyhedronMenu.pickedColor.a >= 0.9f;
                                    _surfaces.PaintAllSurfaces(GameManager.Instance.editPolyhedronMenu.pickedColor);

                                }

                                break;
                            }

                    }


                }
                break;

        }

    }

    void OnDisable() {
        if (GameManager.Instance.editPolyhedronMenu != null)
            GameManager.Instance.editPolyhedronMenu.gameObject.gameObject.SetActive(false);


    }

}
