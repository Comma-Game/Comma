using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    StageController _stageController;
    void Start()
    {
        _stageController = StageController.Instance;
    }

    void Update()
    {
        
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Portal")) triggerPortal();
        else if (other.gameObject.CompareTag("Ground")) triggerGround();
    }

    void triggerPortal()
    {
        _stageController.Destroy_Stage();
    }

    void triggerGround()
    {
        Destroy(transform.gameObject);
        _stageController.Game_Over();
    }
}
