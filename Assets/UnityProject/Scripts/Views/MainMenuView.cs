using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainMenuView : MonoBehaviour
{
    void Start() {
        Vector3 position = Camera.main.transform.position + Camera.main.transform.forward * 0.85f;
        position.y = 1.6f;
        position.x += 0.12f;
        gameObject.transform.position = position;

        //Transform temp = gameObject.transform.Find("Container").Find("BoundingBoxWithHandles(Clone)");
        //temp.position = new Vector3(0, 0, temp.position.z);

    }

    private void Update() {
        gameObject.transform.LookAt(gameObject.transform.position - (Camera.main.transform.position - gameObject.transform.position));

    }

    public void IsGrabbing(bool grabbing) {
        Debug.Log("Is grabbing? " + grabbing);  

    }

    void OnEnable() {
        Vector3 position = Camera.main.transform.position + Camera.main.transform.forward * 0.85f;
        position.y += -0.37f;
    }

}
