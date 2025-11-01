using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChildColliderReporter : MonoBehaviour
{
    public BilliardRule parentRule; // 親の参照をInspectorで設定

    private void OnTriggerEnter(Collider other)
    {
        parentRule.OnChildHit(this.gameObject, other);
    }
}