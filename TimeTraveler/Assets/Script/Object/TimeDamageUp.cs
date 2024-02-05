using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimeDamageUp : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.transform.CompareTag("Player")) other.transform.GetComponent<Player>().TimeDamageUp(2);
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.transform.CompareTag("Player")) other.transform.GetComponent<Player>().ResetTimeDamage();
    }
}
