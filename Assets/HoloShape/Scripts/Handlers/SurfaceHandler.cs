using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SurfaceHandler : SolidComponent
{
    Color _latestColor;
    Color _currentColor;
    MeshRenderer _meshRenderer;

    void Start() {
        _meshRenderer = GetComponent<MeshRenderer>();
        _latestColor = _meshRenderer.material.color;
        _currentColor = _meshRenderer.material.color;

    }

    public void PaintSurface(Color newColor) {
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
                    switch (GameManager.Instance.editPolyhedronMenu.editMethod) {
                        case EditMethod.Paint:
                            GameManager.Instance.editPolyhedronMenu.selectedComponentToPaint = this;
                            break;

                    }

                    break;
                }

        }
    }

}
