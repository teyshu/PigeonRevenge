using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PoorSpawn : MonoBehaviour
{
    [SerializeField] GameObject _poorPrefab;
    [SerializeField] Transform _poorPosition;
    private void Start()
    {
        /*_poorPosition = GameObject.FindGameObjectWithTag("SpawnPos").transform;
        Instantiate(_poorPrefab, _poorPosition);*/
        GameObject.FindGameObjectWithTag("SpawnPos").transform.GetChild(0).gameObject.SetActive(true);
    }
}
