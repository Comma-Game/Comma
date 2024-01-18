using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwipeMovement : MonoBehaviour
{
    [SerializeField] public Rigidbody rb; 
    [SerializeField] public float swipeSpeed = 5f;
    private Vector2 touchStartPos;
    private bool isSwiping = false;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    void Update()
    {
        // 유니티 테스트 할 때
        if (Input.GetMouseButtonDown(0))
        {
            touchStartPos = Input.mousePosition;
            isSwiping = true;
        }

        if (Input.GetMouseButtonUp(0))
        {
            isSwiping = false;
        }

        if (isSwiping)
        {
            Vector2 swipeDelta = (Vector2)Input.mousePosition - touchStartPos;
            swipeDelta = swipeDelta / 100;

            Vector3 swipeForce = new Vector3(swipeDelta.x * swipeSpeed, 0, swipeDelta.y * swipeSpeed);
            rb.AddForce(-swipeForce);
        }

        // 모바일 테스트 할 때
        if (Input.touchCount > 0)
        {
            Touch touch = Input.GetTouch(0);

            // 터치 시작 지점 저장
            if (touch.phase == TouchPhase.Began)
            {
                touchStartPos = touch.position;
                isSwiping = true;
            }

            // 터치 중일 때 스와이프 감지
            else if (touch.phase == TouchPhase.Moved && isSwiping)
            {
                Vector2 swipeDelta = touch.position - touchStartPos;

                swipeDelta = swipeDelta / 100;

                Vector3 swipeForce = new Vector3(swipeDelta.x * swipeSpeed, 0, swipeDelta.y * swipeSpeed);
                rb.AddForce(swipeForce);

                // 스와이프가 끝났을 때
                if (touch.phase == TouchPhase.Ended || touch.phase == TouchPhase.Canceled)
                {
                    isSwiping = false;
                }
            }
        
        }
    }
}