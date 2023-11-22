using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PhysicsHandler : MonoBehaviour {

    [SerializeField] GameObject[] _occlusionBounds;

    BoxCollider _collider;
    Rigidbody _rigidbody;

    public GameObject[] occlusionBounds { get { return _occlusionBounds; } }
    public Rigidbody rigidbody { get { return _rigidbody; } }
    public BoxCollider collider { get { return _collider; } }

    // Start is called before the first frame update
    void Start() {
        _collider = GetComponent<BoxCollider>();
        _rigidbody = GetComponent<Rigidbody>();

        _rigidbody.isKinematic = true;

    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
