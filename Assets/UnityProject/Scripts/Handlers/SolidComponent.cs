using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

abstract public class SolidComponent : MonoBehaviour {
    [SerializeField] protected string _letterKey = "Big Letter";
    [SerializeField] protected string _meshKey = "Sphere";

    [SerializeField] protected string _designation = "";

    protected PolyhedronHandler _solid;
    protected GameObject _mesh;
    protected GameObject _letter;
    protected GameObject _camera;

    public GameObject mesh { get { return _mesh; } }
    public GameObject letter { get { return _letter; } }

    private void Awake() {
        _camera = GameObject.FindGameObjectWithTag("MainCamera");
        _mesh = this.transform.Find(_meshKey)?.gameObject;
        _letter = this.transform.Find(_letterKey)?.gameObject;

        if (_letter != null )
            _letter.transform.Find("Text").GetComponent<TextMeshPro>().text = _designation;

    }

    protected void OnStart() {
        _mesh?.gameObject.SetActive(false);
        _letter?.gameObject.SetActive(false);

    }

    public void BindSolid(PolyhedronHandler solid) {
        _solid = solid;
    }

    private void OnDisable() {
        _mesh?.gameObject.SetActive(false);
        _letter?.gameObject.SetActive(false);

    }

}
