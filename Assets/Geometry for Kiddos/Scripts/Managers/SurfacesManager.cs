using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SurfacesManager : MonoBehaviour
{
    [SerializeField] MeshRenderer[] _list;


    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public MeshRenderer[] List() {
        return _list;
    }

    public void PaintAllSurfaces(Color color) {
        foreach (MeshRenderer surface in _list)
            surface.material.color = color;
    }

}
