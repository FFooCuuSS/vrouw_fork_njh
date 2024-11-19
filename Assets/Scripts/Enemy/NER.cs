using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NER : EnemyMove
{
    public GameObject attackRange;
    public GameObject player;
    private CircleCollider2D rangeCollider;

    private bool canAttack;

    //��Ÿ� �ޱ�
    void Start()
    {
        rangeCollider = attackRange.GetComponent<CircleCollider2D>();
        rangeCollider.isTrigger = true;
    }

    //�÷��̾� ����
    private void OnTriggerEnter2D(Collider2D collision)
    {
        canAttack = false;
        if (collision.tag == "Player")
        {
            Debug.Log("�÷��̾� ����");
            StartCoroutine("Aim");
        }
    }
    //�÷��̾� ��ħ
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.tag == "Player")
        {
            Debug.Log("�÷��̾� ��ħ");
            StopCoroutine("Aim");
        }
    }
    //���� �ð�
    private IEnumerator Aim()
    {
        yield return new WaitForSeconds(1f);
        canAttack = true;
    }
    private void Update()
    {
        if (canAttack)
            Debug.Log("�� �߻�");
    }
}
