using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EdgesManager : MonoBehaviour
{
    [SerializeField] EdgeHandler[] _list;

    public bool EnabledGlobalOcclusion = false;
    bool _localOcclusion = false;
    public bool EnabledOcclusion {
        get { return _localOcclusion; }
        set {
            if (!value)
                ResetEdges();
            _localOcclusion = value;
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {

    }

    public EdgeHandler[] List() {
        return _list;
    }

    public void BindSolid(SolidHandler solid) {
        foreach (EdgeHandler edge in _list)
            edge.BindSolid(solid);
    }

    public void ResetEdges() {
        foreach (EdgeHandler edgeHandler in _list) {
            edgeHandler.ResetLineMaterial();
            edgeHandler.ResetOcclusionedVariables();

        }

    }

}
