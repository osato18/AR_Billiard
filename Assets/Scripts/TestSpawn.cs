using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestSpawn : MonoBehaviour
{
    [SerializeField] private GameObject _spawnObj;
    [SerializeField] private GameObject _offsetObj;
    // Start is called before the first frame update
    void Start()
    {

    }

    public void Spawn()
    {
        Instantiate(_spawnObj, _offsetObj.transform.position, Quaternion.identity);  //オブジェクトの生成
    }
    // Update is called once per frame
    void Update()
    {
    }
}
