using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Controller : MonoBehaviour {
    public static Controller Instance = null; 

    public static List<Action<ApplicationMode>> OnApplicationModeChange = new List<Action<ApplicationMode>>();
    private static ApplicationMode _applicationMode;
    public static ApplicationMode ApplicationMode {
        get { return _applicationMode; }
        set {
            _applicationMode = value;
            foreach (Action<ApplicationMode> action in OnApplicationModeChange)
                action?.Invoke(_applicationMode);

        }
    }

    [Header("Screens:")]
    public HandMenuView handMenu;
    public EditSolidView editSolid;

    [Header("Materials:")]
    public Material defaultMaterial;
    public Material outlineMaterial;
    public Material transparentMaterial;
    public Material defaultEdgeLineMaterial;
    public Material dashedEdgeLineMaterial;

    [Header("Support:")]
    public GameObject playground;
    public LayerMask occlusionColliderLayer;

    [Header("Info:")]
    public Dictionary<Guid, SolidHandler> solidsActives = new Dictionary<Guid, SolidHandler>();


    private void Awake() {
        if (Instance != null)
            Destroy(this);
        else
            Instance = this;

    }

    // Start is called before the first frame update
    void Start() {
        handMenu.SetApplicationMode((int)ApplicationMode.Manipulate);

    }

    // Update is called once per frame
    void Update() {

    }

}
