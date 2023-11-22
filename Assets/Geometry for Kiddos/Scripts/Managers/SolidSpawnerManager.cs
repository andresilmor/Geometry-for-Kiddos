using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SolidSpawnerManager : MonoBehaviour
{
    public static SolidSpawnerManager Instance = null;
    [SerializeField] PlaygroundMenuView _createMenuScreen;

    [Serializable]
    public struct SpawnableSolid {
        public string name;
        public GameObject grabSolid;
        [HideInInspector] public Vector3 grabSolidStartPos;
        public GameObject solid;
    }

    public SpawnableSolid[] spawnableSolids;
    Dictionary<string, SpawnableSolid> _spawnableSolids = new Dictionary<string, SpawnableSolid>();

    Collider _collider;

    bool _toInstantiate = true;
    GameObject _newSolid = null;


    void Awake() {
        if (Instance != null)
            Destroy(_createMenuScreen.gameObject);
        else
            Instance = this;

    }

    void Start() {
        _collider = GetComponent<Collider>();

        for (int index = 0; index < spawnableSolids.Length; index++) {
            spawnableSolids[index].grabSolidStartPos = spawnableSolids[index].grabSolid.transform.localPosition;
            spawnableSolids[index].name = spawnableSolids[index].grabSolid.name;
            _spawnableSolids.Add(spawnableSolids[index].name, spawnableSolids[index]);

        }

    }

    public void OnGrab() {
        _toInstantiate = true;
    }

    public void OnRelease(GameObject objectReleased) {
        Debug.Log("Hello There");
        if (_toInstantiate && _spawnableSolids[objectReleased.gameObject.name].solid != null) {
            Debug.Log("Gonna Instantiate");
            _newSolid = Instantiate(_spawnableSolids[objectReleased.gameObject.name].solid,  Vector3.zero, Quaternion.identity, objectReleased.transform.GetChild(0).transform);
            _newSolid.transform.localPosition = new Vector3(0, 0, 0);
            _newSolid.transform.parent = Controller.Instance.playground.transform;
            _newSolid.transform.localScale = Vector3.one;
            //_newSolid.transform.position = new Vector3(_newSolid.transform.position.x, _newSolid.transform.position.y - 1.525811f, _newSolid.transform.position.z - 0.6684352f);
            _newSolid.transform.rotation = objectReleased.transform.rotation;

        }

        objectReleased.transform.localPosition = _spawnableSolids[objectReleased.gameObject.name].grabSolidStartPos;
        _toInstantiate = false;

    }

    void OnTriggerEnter(Collider other) {
        Debug.Log("General Kenobi");
        if (other.gameObject.tag.Equals("OcclusionCollider")) {

            Debug.Log("Master Yoda");
            _newSolid.SetActive(false); // NEED TO SOLVE THIS SHIT
        }

    }

}
