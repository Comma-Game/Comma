using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TriggerSpeedUp : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.transform.CompareTag("Player"))
        {
            Debug.Log("π∞ø° ¡¢√À!");
            StageController.Instance.SetVelocity(StageController.Instance.GetStageVelocity() * 1.5f);
        }
    }
}
