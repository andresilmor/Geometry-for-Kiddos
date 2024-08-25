using MixedReality.Toolkit;
using MixedReality.Toolkit.SpatialManipulation;
using MixedReality.Toolkit.UX;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using UnityEngine;

public class SolidHandler : MonoBehaviour {
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
        Controller.Instance.solidsActives.Add(_identifier, this);

        _solidManipulator = solid.GetComponent<ObjectManipulator>();

        _edges.BindSolid(this);
        _vertices.BindSolid(this);

        switch (Controller.ApplicationMode) {
            case ApplicationMode.Manipulate:
                objectManipulator.AllowedManipulations = TransformFlags.Move | TransformFlags.Rotate | TransformFlags.Scale;
                break;

            case ApplicationMode.Edit:
                objectManipulator.AllowedManipulations = TransformFlags.None;
                break;

        }

        Controller.OnApplicationModeChange.Add((ApplicationMode mode) => {
            switch (mode) {
                case ApplicationMode.Manipulate:
                    objectManipulator.AllowedManipulations = TransformFlags.Move | TransformFlags.Rotate | TransformFlags.Scale;
                    break;

                case ApplicationMode.Edit:
                    Controller.Instance.editSolid?.gameObject.SetActive(false);
                    objectManipulator.AllowedManipulations = TransformFlags.None;
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
        Controller.Instance.editSolid.gameObject.SetActive(true);
        Vector3 position = Camera.main.transform.position + Camera.main.transform.forward * 0.85f;
        position.y = 1.6f;
        Controller.Instance.editSolid.gameObject.transform.position = position;

    }

    public void OnClick() {
        Debug.Log(Controller.ApplicationMode);
        switch (Controller.ApplicationMode) {
            case ApplicationMode.Edit:
                if (Controller.Instance.editSolid.bindedSolid != this) {
                    Debug.Log("Binding Solid");
                    DisplayEditPanel();
                    Controller.Instance.editSolid.BindSolid(this);

                } else {
                    if (!Controller.Instance.editSolid.gameObject.activeSelf)
                        DisplayEditPanel();

                    switch (Controller.Instance.editSolid.editMethod) {
                        case EditMethod.Paint: {
                                if (Controller.Instance.editSolid.individualPaint) {
                                    try {
                                        Debug.Log(Controller.Instance.editSolid.pickedColor);

                                        (Controller.Instance.editSolid.selectedComponentToPaint as SurfaceHandler).PaintSurface(Controller.Instance.editSolid.pickedColor);

                                    } catch (Exception e) {
                                        Debug.Log(e.Message);
                                    }
                                } else {
                                    isSolidColor = Controller.Instance.editSolid.pickedColor.a >= 0.9f;
                                    _surfaces.PaintAllSurfaces(Controller.Instance.editSolid.pickedColor);

                                }

                                break;
                            }

                    }


                }
                break;

        }

    }

    void OnDisable() {
        if (Controller.Instance.editSolid != null)
            Controller.Instance.editSolid.gameObject.gameObject.SetActive(false);


    }

}
