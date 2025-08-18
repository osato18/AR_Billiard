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
        Instantiate(_spawnObj, _offsetObj.transform.position, Quaternion.Euler(0,-90,0));  //オブジェクトの生成
    }

        void OnDrawGizmos()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawSphere(_offsetObj.transform.position, 0.1f);
    }
    // Update is called once per frame
    void Update()
    {
    }
}
