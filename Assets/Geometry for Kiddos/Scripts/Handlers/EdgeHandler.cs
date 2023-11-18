using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem.HID;

public class EdgeHandler : SolidComponent {
    [SerializeField]
    VerticeHandler[] _vertices = null;

    LineRenderer _lineRenderer;

    int _visibleVertices;
    bool _localOcclusioned = false;
    bool _globalOcclusioned = false;

    void Start() {
        base.OnStart();

        _lineRenderer = _mesh.GetComponent<LineRenderer>();
        if (_lineRenderer != null)
            _lineRenderer.positionCount = _vertices.Length;

    }

    void Update() {
        if (_solid == null)
            return;

        if (_letter != null && _letter.gameObject.activeSelf)
            _letter.gameObject.transform.LookAt(gameObject.transform.position - (_camera.transform.position - gameObject.transform.position));

        if (_mesh != null && _mesh.activeSelf) {
            _lineRenderer.SetPosition(0, _vertices[0].Mesh.transform.position);
            _lineRenderer.SetPosition(1, _vertices[1].Mesh.transform.position);

            if (_solid.Edges.EnabledOcclusion && !_solid.IsSolidColor && _vertices != null && _vertices.Length > 0) {
                Debug.Log("Here");
                UpdateLocalOcclusionMaterial();

                if (_solid.Edges.EnabledGlobalOcclusion)
                    UpdateGlobalOcclusionMaterial();

            }

        }

    }

    public void ResetLineMaterial() {
        _lineRenderer.material = Controller.Instance.DefaultEdgeLineMaterial;
    }

    private void UpdateLocalOcclusionMaterial() {
        _visibleVertices = _vertices.Length;

        foreach (VerticeHandler vertice in _vertices) {
            Vector3 verticePos = vertice.Mesh.transform.position;
            Transform cameraTransform = _camera.transform;

            RaycastHit hit;
            if (Physics.Raycast(verticePos, cameraTransform.position - verticePos, out hit, Mathf.Infinity, Controller.Instance.OcclusionColliderLayer)) {
                foreach (GameObject bound in _solid.Physics.OcclusionBounds)
                    if (hit.collider.gameObject == bound)
                        _visibleVertices -= 1;
               

            }

        }

        if (_visibleVertices < _vertices.Length) { 
            _lineRenderer.material = Controller.Instance.DashedEdgeLineMaterial;
            _localOcclusioned = true;

        } else if (_visibleVertices == _vertices.Length && !_globalOcclusioned) { 
            _lineRenderer.material = Controller.Instance.DefaultEdgeLineMaterial;
            _localOcclusioned = false;
        
        }

    }

    public void ResetOcclusionedVariables() {
        _globalOcclusioned = false;
        _localOcclusioned = false;

    }

    private void UpdateGlobalOcclusionMaterial() {
        _visibleVertices = _vertices.Length;


        foreach (VerticeHandler vertice in _vertices) {
            Vector3 verticePos = vertice.Mesh.transform.position;
            Transform cameraTransform = _camera.transform;

            RaycastHit hit;
            int count = 0;
            if (Physics.Raycast(verticePos, cameraTransform.position - verticePos, out hit, Mathf.Infinity, Controller.Instance.OcclusionColliderLayer)) {
                foreach (GameObject bound in _solid.Physics.OcclusionBounds)
                    if (hit.collider.gameObject != bound) { 
                        _visibleVertices -= 1;
                    }


            }

        }

        if (_visibleVertices <= 0) { 
            _lineRenderer.material = Controller.Instance.DashedEdgeLineMaterial;
            _globalOcclusioned = true;
        
        } else if (_visibleVertices > 0 && !_localOcclusioned) { 
            _lineRenderer.material = Controller.Instance.DefaultEdgeLineMaterial;
            _globalOcclusioned = false;

        }

    }

}
