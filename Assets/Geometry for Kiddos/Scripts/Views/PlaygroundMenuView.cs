using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlaygroundMenuView : MonoBehaviour
{
    void Start() {
        Vector3 position = Camera.main.transform.position + Camera.main.transform.forward * 0.85f;
        position.y += -0.37f;
        gameObject.transform.position = position;
        transform.LookAt(gameObject.transform.position - (Camera.main.transform.position - gameObject.transform.position));


    }

    public void IsGrabbing(bool grabbing) {
        Debug.Log("Is grabbing? " + grabbing);  

    }

    void OnEnable() {
        Vector3 position = Camera.main.transform.position + Camera.main.transform.forward * 0.85f;
        position.y += -0.37f;
    }

}
