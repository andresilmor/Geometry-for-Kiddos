using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class VerticeHandler : SolidComponent {
    [SerializeField]
    EdgeHandler[] _connectedEdges = null;

    bool _isVisible = true;
    public bool isVisible { get { return _isVisible; } }

    private void Start() {
        base.OnStart();
    }

    void Update() {
        if (_solid == null)
            return;

        if (_letter != null && _letter.gameObject.activeSelf)
            _letter.gameObject.transform.LookAt(gameObject.transform.position - (_camera.transform.position - gameObject.transform.position));

    }

}
