using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimeDamageUp : MonoBehaviour
{
    Player _player;

    private void OnEnable()
    {
        StopAllCoroutines();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.transform.CompareTag("Player"))
        {
            _player = other.transform.GetComponent<Player>();
            _player.TimeDamageUp();
            StartCoroutine(TimeDamage());
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.transform.CompareTag("Player"))
        {
            _player = other.transform.GetComponent<Player>();
            _player.ResetTimeDamage();
            StopAllCoroutines();
        }
    }

    IEnumerator TimeDamage()
    {
        while (_player.GetHp() > 0)
        {
            _player.TimeDamage();

            yield return new WaitForSeconds(1f);
        }
    }
}
