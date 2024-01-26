using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShowPlayer : MonoBehaviour
{
    [SerializeField]
    GameObject _player;

    private void OnDrawGizmos()
    {
        /*
        if(Physics.BoxCastAll(transform.position, _player.transform.lossyScale / 2, dir, 35f,
            1 << LayerMask.NameToLayer("Object")))
        */
    }

    private void LateUpdate()
    {
        Vector3 dir = (_player.transform.position - transform.position).normalized;
        /*
        RaycastHit[] hits = Physics.RaycastAll(transform.position, dir, 35f,
            1 << LayerMask.NameToLayer("Object"));

        Debug.DrawRay(transform.position, dir * 35f, Color.red);
        for (int i = 0; i < hits.Length; i++)
        {
            TransparentObject[] obj = hits[i].transform.GetComponentsInChildren<TransparentObject>();
            for(int j = 0; j < obj.Length; j++)
            {
                obj[j]?.BecomeTransparent();
            }
        }
        */
    }
}
