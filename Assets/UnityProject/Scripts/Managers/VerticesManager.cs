using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VerticesManager : MonoBehaviour
{
    [SerializeField] VerticeHandler[] _list;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public VerticeHandler[] List() {
        return _list;
    }

    public void BindSolid(PolyhedronHandler solid) {
        foreach (VerticeHandler vertice in _list)
            vertice.BindSolid(solid);
    }

}
