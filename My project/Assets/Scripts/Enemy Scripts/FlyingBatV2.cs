using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlyingBatV2 : MonoBehaviour
{
    private Collision2D col;
    [SerializeField]
    private float minX = -8.3f, maxX = 8.3f, minY = -4.5f, maxY = 4.5f;

    private Vector3 targetPosition;

    [SerializeField]
    private float moveSpeed = 2f;

    private SpriteRenderer sr;

    private float previousX;

    private void Awake()
    {
        sr = GetComponent<SpriteRenderer>();
    }

    private void Update()
    {
        MoveToTargetPosition();
    }

    void MoveToTargetPosition()
    {

        transform.position = Vector3.MoveTowards(transform.position, targetPosition,
            moveSpeed * Time.deltaTime);

        if (Vector3.Distance(transform.position, targetPosition) < 0.1f)
        {
            targetPosition = new Vector3(Random.Range(minX, maxX), Random.Range(minY, maxY), 0f);
            previousX = transform.position.x;
        }

        ChangeFacingDirection();
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        
    }
    void ChangeFacingDirection()
    {

        if (transform.position.x > previousX)
            sr.flipX = false;
        else if (transform.position.x < previousX)
            sr.flipX = true;

    }


} // class





























