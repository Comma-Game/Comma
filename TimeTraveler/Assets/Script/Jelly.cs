using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Jelly : MonoBehaviour
{
    [SerializeField]
    int _healAmount;

    void Start()
    {
        
    }

    void Update()
    {
        
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("Player"))
        {
            StageController.Instance.ScoreUp(100);
            other.transform.GetComponent<Player>().Heal(_healAmount);
            Destroy(transform.gameObject);
        }
    }
}
