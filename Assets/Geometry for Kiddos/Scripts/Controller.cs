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
    public static GameObject HandMenu;
    public static EditSolidScreen OpenedEditSolidScreen = null;

    [Header("Materials:")]
    public Material DefaultMaterial;
    public Material OutlineMaterial;
    public Material TransparentMaterial;
    public Material DefaultEdgeLineMaterial;
    public Material DashedEdgeLineMaterial;

    [Header("Support:")]
    public GameObject Playground;
    public LayerMask OcclusionColliderLayer;

    [Header("Info:")]
    public Dictionary<Guid, SolidHandler> SolidsActives = new Dictionary<Guid, SolidHandler>();


    private void Awake() {
        if (Instance != null)
            Destroy(this);
        else
            Instance = this;

    }

    // Start is called before the first frame update
    void Start() {
        ApplicationMode = ApplicationMode.Manipulate;

    }

    // Update is called once per frame
    void Update() {

    }

    public void SetApplicationMode(int mode) {
        ApplicationMode = (ApplicationMode)mode;

    }

}
