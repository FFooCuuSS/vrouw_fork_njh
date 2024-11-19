using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NER : EnemyMove
{
    public GameObject attackRange;
    public GameObject player;
    private CircleCollider2D rangeCollider;

    private bool canAttack;

    //사거리 받기
    void Start()
    {
        rangeCollider = attackRange.GetComponent<CircleCollider2D>();
        rangeCollider.isTrigger = true;
    }

    //플레이어 감지
    private void OnTriggerEnter2D(Collider2D collision)
    {
        canAttack = false;
        if (collision.tag == "Player")
        {
            Debug.Log("플레이어 감지");
            StartCoroutine("Aim");
        }
    }
    //플레이어 놓침
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.tag == "Player")
        {
            Debug.Log("플레이어 놓침");
            StopCoroutine("Aim");
        }
    }
    //조준 시간
    private IEnumerator Aim()
    {
        yield return new WaitForSeconds(1f);
        canAttack = true;
    }
    private void Update()
    {
        if (canAttack)
            Debug.Log("적 발사");
    }
}
