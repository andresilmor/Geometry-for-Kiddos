using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FacesManager : MonoBehaviour
{
    [SerializeField] FaceHandler[] _list;


    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public FaceHandler[] List() {
        return _list;
    }

    public void PaintAllFaces(Color color) {
        foreach (FaceHandler surface in _list)
            surface.PaintFace(color);
    }

}
