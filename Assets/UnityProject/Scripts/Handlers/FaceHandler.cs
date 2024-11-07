using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FaceHandler : SolidComponent
{
    Color _latestColor;
    Color _currentColor;
    MeshRenderer _meshRenderer;

    void Start() {
        _meshRenderer = GetComponent<MeshRenderer>();
        _latestColor = _meshRenderer.material.color;
        _currentColor = _meshRenderer.material.color;

    }

    public void PaintFace(Color newColor) {
        _latestColor = _currentColor;
        _currentColor = newColor;
        _meshRenderer.material.color = newColor;

    }

    public void SetVisibility(bool isVisible) {
        _meshRenderer.enabled = isVisible;  

    }

    private void OnTriggerEnter(Collider other) {
        switch (GameManager.ApplicationMode) {
            case ApplicationMode.Edit: {
                    switch (GameManager.Instance.EditPolyhedronMenu.EditMethod) {
                        case EditMethod.Paint:
                            GameManager.Instance.EditPolyhedronMenu.selectedComponentToPaint = this;
                            break;

                    }

                    break;
                }

        }
    }

}
