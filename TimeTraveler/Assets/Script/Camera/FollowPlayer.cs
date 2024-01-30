using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowPlayer : MonoBehaviour
{
    [SerializeField]
    public Transform playerTransform; // 플레이어의 Transform을 저장할 변수
    //public float smoothSpeed = 0.125f; // 카메라 이동의 부드러움 정도

    void LateUpdate()
    {
        if (playerTransform != null)
        {
            // 플레이어의 위치를 가져와서 카메라를 부드럽게 따라가게 함
            // Vector3 desiredPosition = new Vector3(playerTransform.position.x, playerTransform.position.y+10, playerTransform.position.z);
            // Vector3 smoothedPosition = Vector3.Lerp(transform.position, desiredPosition, smoothSpeed);
            // transform.position = smoothedPosition;

            transform.position = new Vector3(playerTransform.position.x, playerTransform.position.y+8, playerTransform.position.z);
            // 플레이어를 항상 바라보도록 함
            transform.LookAt(playerTransform);
        }
    }
}
