using MixedReality.Toolkit;
using MixedReality.Toolkit.SpatialManipulation;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static SolidSpawnerManager;


public class SolidHandler : MonoBehaviour {
    public string SolidDesignation = "Cube";
    Guid _identifier;
    public Guid Identifier { get { return _identifier; } }  

    GameObject _solid;
    EditSolidScreen _solidPanel;

    ObjectManipulator _solidManipulator;

    BoxCollider _solidCollider;
    Rigidbody _solidRigidbody;

    public MeshRenderer SolidMeshRenderer;
    public MeshRenderer[] SolidSurfaces;
    public VerticeHandler[] SolidVertices;
    public EdgeHandler[] SolidEdges;
    public GameObject[] CollisionBounds;

    public GameObject Solid { get { return _solid; } }

    public ObjectManipulator ObjectManipulator { get { return _solidManipulator; } }

    public Rigidbody SolidRigidbody { get { return _solidRigidbody; } }
    public BoxCollider SolidCollider { get { return _solidCollider; } }

    public bool IsSolidColor = true;
    public bool GloabalCollision = false;
    public bool GlobalOcclusion = false;

    public Color BackupColor;

    bool _enabledOcclusion = false;
    public bool EnabledOcclusion {
        get { return _enabledOcclusion; }
        set {
            if (!value)
                foreach (EdgeHandler edgeHandler in SolidEdges) { 
                    edgeHandler.ResetLineMaterial();
                    edgeHandler.ResetOcclusionedVariables();

                }
            _enabledOcclusion = value;
        }
    }

    private void Awake() {

    }

    void Start() {
        _identifier = Guid.NewGuid(); 
        Debug.Log("Identifier: " + _identifier.ToString());
        Controller.Instance.SolidsActives.Add(_identifier, this);

        _solid = transform.Find("Solid").gameObject;
        _solidPanel = transform.Find("Panel").gameObject.GetComponent<EditSolidScreen>();

        _solidManipulator = Solid.GetComponent<ObjectManipulator>();

        _solidCollider = Solid.transform.Find("Body").GetComponent<BoxCollider>();
        _solidRigidbody = Solid.transform.Find("Body").GetComponent<Rigidbody>();

        _solidPanel.gameObject.SetActive(false);
        _solidPanel.BindSolid(this);

        _solidRigidbody.isKinematic = true;

        foreach (VerticeHandler vertice in SolidVertices)
            vertice.SetSolid(this);

        foreach (EdgeHandler edge in SolidEdges)
            edge.SetSolid(this);

        Controller.OnApplicationModeChange.Add((ApplicationMode mode) => {
            switch (mode) {
                case ApplicationMode.Manipulate:
                    _solidPanel.ToggleManipulation(true);
                    _solidPanel.gameObject.SetActive(false);
                    break;

                case ApplicationMode.Edit:
                    _solidPanel.ToggleManipulation(false);
                    break;

            }

        });

    }

    void Update() {

    }

    public void SetColor(Color color, GameObject[] surfaces = null) {
        if (surfaces == null) {
            foreach (MeshRenderer surface in SolidSurfaces)
                surface.material.color = color;

            IsSolidColor = color.a >= 0.9f;

        }

        //SolidMeshRenderer.material.color = color;
        //IsSolidColor = color.a >= 0.9f;

    }

    public void OnClick() {
        Debug.Log(Controller.ApplicationMode);
        switch (Controller.ApplicationMode) {
            case ApplicationMode.Edit:
                if (!_solidPanel.gameObject.activeSelf) { 
                    if (Controller.OpenedEditSolidScreen != null && Controller.OpenedEditSolidScreen != _solidPanel)
                        Controller.OpenedEditSolidScreen.gameObject.SetActive(false);

                    Controller.OpenedEditSolidScreen = _solidPanel;
                    _solidPanel.gameObject.SetActive(true);
                }
                break;

        }

    }

    void OnDisable() {
        _solidPanel.gameObject.SetActive(false);


    }

}
