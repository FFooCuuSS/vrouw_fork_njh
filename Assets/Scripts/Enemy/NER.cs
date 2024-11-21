using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NER : EnemyMove
{
    [SerializeField] float Attackspeed = 2f;

    public GameObject attackRange;
    public GameObject NER_bullet;
    public GameObject target;
    private CircleCollider2D rangeCollider;

    private bool canAttack;
    private bool Wait = true;

    private void Update()
    {
        Debug.Log(canAttack);
        if (canAttack && Wait)
        {
            Attack();
            StartCoroutine("WaitAttack");
        }
    }
    //��Ÿ� �ޱ�
    void Start()
    {
        rangeCollider = attackRange.GetComponent<CircleCollider2D>();
        rangeCollider.isTrigger = true;
    }

    //�÷��̾� ����
    private void OnTriggerEnter2D(Collider2D collision)
    {
        speed = 0; //���Ÿ� ���̴� �������� ��°� ����
        if (collision.tag == "Player")
        {
            Debug.Log("�÷��̾� ����");
            StartCoroutine("Aim");
        }
    }
    //�÷��̾� ��ħ
    private void OnTriggerExit2D(Collider2D collision)
    {
        canAttack = false;
        speed = 2.5f;
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
    //���� ��
    private IEnumerator WaitAttack()
    {
        Wait = false;
        yield return new WaitForSeconds(Attackspeed);
        Wait = true;
    }
    //���� ����
    private void Attack()
    {
        Vector3 directionToPlayer = player.transform.position - transform.position;
        directionToPlayer.z = 0f;

        Debug.Log("�� ����");
        GameObject cpy_bullet = Instantiate(NER_bullet, transform.position, transform.rotation);
        Destroy(cpy_bullet, 5f);

        NER_bullet bulletComponent = cpy_bullet.GetComponent<NER_bullet>();
        bulletComponent.SetDirection(directionToPlayer);
    }
}
