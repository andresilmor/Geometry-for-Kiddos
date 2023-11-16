using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class GeometryCalculus
{
    /* Bullshit
   public static bool IsVerticeVisible(Vector3 verticePosition, Vector3 viewerPosition, Transform cubeTransform, System.Action<bool> action = null) {

       Vector3 viewVector =  verticePosition - viewerPosition;
       Vector3 faceNormal = cubeTransform.rotation * GetLocalNormal(verticePosition, cubeTransform);

       float dotProduct = Vector3.Dot(viewVector.normalized, faceNormal.normalized);

       if (dotProduct < 0) {
           Debug.Log("Vertex at " + verticePosition + " is on the hidden side of the cube.");
           action?.Invoke(false);
           return false;

       } else {
           Debug.Log("Vertex at " + verticePosition + " is visible.");
           action?.Invoke(true);
           return true;
       }

   }
   */

    /* Bullshit: The Second Wave
    public static bool IsVerticeVisible(Vector3 verticePosition, Camera camera) {
        Bounds bounds = new Bounds(verticePosition, Vector3.one * 0.1f);
        Plane[] planes = GeometryUtility.CalculateFrustumPlanes(camera);
        bool isVisible = GeometryUtility.TestPlanesAABB(planes, bounds);

        if (isVisible)
            Debug.Log("Visible");
        else
            Debug.Log("Nop");

        return isVisible;

    }*/

    public static bool IsVerticeVisible(Vector3 verticePos, Transform cameraTransform, LayerMask layerMask, GameObject[] bounds = null) {
        RaycastHit hit;
        if (bounds == null) { 
            if(Physics.Raycast(verticePos, cameraTransform.position - verticePos, Mathf.Infinity, layerMask)) { 
                return false;
            }
            return true;
        }

        if (Physics.Raycast(verticePos, cameraTransform.position - verticePos, out hit, Mathf.Infinity, layerMask)) {
            foreach (GameObject bound in bounds)
                if (hit.collider.gameObject == bound) 
                    return false;
            return true;

        }

        return true;
    }


    public static Vector3 GetLocalNormal(Vector3 vertexPosition, Transform cubeTransform) {

        Vector3 localNormal = Vector3.zero;

        if (Mathf.Approximately(vertexPosition.x, cubeTransform.position.x)) {
            // Vertex is on the left or right face
            localNormal = (vertexPosition.x < cubeTransform.position.x) ? Vector3.left : Vector3.right;
        } else if (Mathf.Approximately(vertexPosition.y, cubeTransform.position.y)) {
            // Vertex is on the bottom or top face
            localNormal = (vertexPosition.y < cubeTransform.position.y) ? Vector3.down : Vector3.up;
        } else if (Mathf.Approximately(vertexPosition.z, cubeTransform.position.z)) {
            // Vertex is on the back or front face
            localNormal = (vertexPosition.z < cubeTransform.position.z) ? Vector3.back : Vector3.forward;
        }
        return localNormal;
    }

}



