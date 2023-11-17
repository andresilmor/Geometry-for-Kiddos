using MixedReality.Toolkit;
using MixedReality.Toolkit.SpatialManipulation;
using MixedReality.Toolkit.UX;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using TMPro.EditorUtilities;
using UnityEngine;

public class SolidHandler : MonoBehaviour {
    public string SolidDesignation = "Cube";
    Guid _identifier;
    public Guid Identifier { get { return _identifier; } }  

    [SerializeField] GameObject _solid;
    [SerializeField] EdgesManager _edges;
    [SerializeField] VerticesManager _vertices;
    [SerializeField] SurfacesManager _surfaces;
    [SerializeField] PhysicsHandler _physics;
    [SerializeField] MeshRenderer _mesh;

    ObjectManipulator _solidManipulator;

    public GameObject Solid { get { return _solid; } }
    public EdgesManager Edges { get { return _edges; } }
    public VerticesManager Vertices { get { return _vertices; } }
    public SurfacesManager Surfaces { get { return _surfaces; } }
    public PhysicsHandler Physics { get { return _physics; } }
    public MeshRenderer Mesh { get { return _mesh; } }

    public ObjectManipulator ObjectManipulator { get { return _solidManipulator; } }

    public bool IsSolidColor = true;
    public UnityEngine.Color BackupColor;

    #region Personalization
    [Serializable]
    public class Personalization {
        public EditSolidOption Option;
        public bool Bool;
        public string String;
        public void SetBool(bool value) { Bool = value; }
        public bool GetBool() { return Bool; }
        public void SetString(string value) { String = value; }
        public bool GetString() { return Bool; }
        public EditSolidOption GetOption() { return Option; }
    }

    [SerializeField] Personalization[] PersonalizationValues;
    public Dictionary<EditSolidOption, Personalization> SolidPersonalization = new Dictionary<EditSolidOption, Personalization>();
   

    #endregion

    private void Awake() {

    }

    void Start() {
        foreach (Personalization personalization in PersonalizationValues)
            SolidPersonalization.Add(personalization.GetOption(), personalization);
     
        _identifier = Guid.NewGuid(); 
        Controller.Instance.SolidsActives.Add(_identifier, this);

        _solidManipulator = Solid.GetComponent<ObjectManipulator>();

        _edges.BindSolid(this);
        _vertices.BindSolid(this);

        Controller.OnApplicationModeChange.Add((ApplicationMode mode) => {
            switch (mode) {
                case ApplicationMode.Manipulate:
                    ObjectManipulator.AllowedManipulations = TransformFlags.Move | TransformFlags.Rotate | TransformFlags.Scale;
                    break;

                case ApplicationMode.Edit:
                    Controller.Instance.EditSolid?.gameObject.SetActive(false);
                    ObjectManipulator.AllowedManipulations = TransformFlags.None;
                    break;

            }

        });

    }

    void Update() {

    }

    public void SetColor(UnityEngine.Color color, GameObject[] surfaces = null) {
        if (surfaces == null) {
            

            IsSolidColor = color.a >= 0.9f;

        }

        //SolidMeshRenderer.material.color = color;
        //IsSolidColor = color.a >= 0.9f;

    }

    public void ResetEdges() {
        _edges.ResetEdges();
    }

    public void OnClick() {
        Debug.Log(Controller.ApplicationMode);
        switch (Controller.ApplicationMode) {
            case ApplicationMode.Edit:
                if (Controller.Instance.EditSolid.BindedSolid != this) {
                    Debug.Log("Binding Solid");
                    Controller.Instance.EditSolid?.gameObject.SetActive(true);
                    Controller.Instance.EditSolid?.BindSolid(this);
                    Vector3 position = Camera.main.transform.position + Camera.main.transform.forward * 0.85f;
                    position.y += -0.37f;
                    if (Controller.Instance.EditSolid != null) 
                        Controller.Instance.EditSolid.gameObject.transform.position = position;

                } else {
                    Debug.Log("Solid Already binded");
                    switch (Controller.Instance.EditSolid.EditMethod) {
                        case EditMethod.Paint: {
                                Debug.Log("Gonna Paint");
                                Debug.Log("Picked color " + Controller.Instance.EditSolid.PickedColor);
                                if (Controller.Instance.EditSolid.IndividualPaint) {
                                    Debug.Log("Indivdual");
                                } else {
                                    Debug.Log("Full");
                                    IsSolidColor = Controller.Instance.EditSolid.PickedColor.a >= 0.9f;
                                    _surfaces.PaintAllSurfaces(Controller.Instance.EditSolid.PickedColor);

                                }

                                break;
                            }

                    }


                }
                break;

        }

    }

    void OnDisable() {
        Controller.Instance.EditSolid.gameObject.gameObject.SetActive(false);


    }

}
