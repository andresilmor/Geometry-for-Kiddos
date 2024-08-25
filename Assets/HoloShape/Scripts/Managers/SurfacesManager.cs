using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SurfacesManager : MonoBehaviour
{
    [SerializeField] SurfaceHandler[] _list;


    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public SurfaceHandler[] List() {
        return _list;
    }

    public void PaintAllSurfaces(Color color) {
        foreach (SurfaceHandler surface in _list)
            surface.PaintSurface(color);
    }

}
