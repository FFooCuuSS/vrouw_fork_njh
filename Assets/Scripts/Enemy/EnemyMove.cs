using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyMove : MonoBehaviour
{
    [SerializeField] protected Rigidbody2D rigid;
    [SerializeField] protected SpriteRenderer render;

    [SerializeField] protected float speed = 2.5f;
    [SerializeField] protected float followDistance = 5f;    // �߰� ���� �Ÿ�
    [SerializeField] protected float stopChaseRange = 2f;    // ���� ���� �Ÿ� (���� �غ� �Ÿ�)
    [SerializeField] protected float Hp;

    protected Transform player;
    protected bool isPlayerOnSamePlatform;
    protected bool isChasing;
    protected int nextMove;


    void Awake()
    {
        rigid = GetComponent<Rigidbody2D>();
        player = GameObject.FindWithTag("Player").transform;
        Think(); // �ʱ� �̵� ���� ����
    }

    void FixedUpdate()
    {
        CheckPlatform(); // �÷��̾�� ���� �÷����� �ִ��� Ȯ��

        if (isPlayerOnSamePlatform && Vector2.Distance(transform.position, player.position) <= followDistance)
        {
            // �÷��̾ ���� ���� ���� ������
            if (Vector2.Distance(transform.position, player.position) > stopChaseRange)
            {
                // ���� �Ÿ� �̻��� �� �߰�
                ChasePlayer();
            }
            else
            {
                // ���� ���� ���� �����ϸ� ����
                StopAndPrepareAttack();
            }
        }
        else if (isChasing && Vector2.Distance(transform.position, player.position) > followDistance)
        {
            // ���� ����
            StopChasing();
        }
        else if (!isChasing)
        {
            // ���� ����
            Patrol();
        }
    }

    private void Patrol()
    {
        rigid.velocity = new Vector2(nextMove * speed, rigid.velocity.y);
        Vector2 frontVector = new Vector2(rigid.position.x + nextMove * 0.5f, rigid.position.y);
        Debug.DrawRay(frontVector, Vector3.down, Color.green);

        RaycastHit2D rayHit = Physics2D.Raycast(frontVector, Vector3.down, 2f, LayerMask.GetMask("Ground"));

        if (rayHit.collider == null)
        {
            Turn();
        }
    }

    private void ChasePlayer()
    {
        isChasing = true;
        Vector2 direction = (player.position - transform.position).normalized;
        rigid.velocity = new Vector2(direction.x * speed, rigid.velocity.y);
        render.flipX = direction.x < 0;
    }

    private void StopChasing()
    {
        isChasing = false;
        Think(); // ���� ���·� ��ȯ
    }

    private void StopAndPrepareAttack()
    {
        rigid.velocity = Vector2.zero; // �� ����
        Debug.Log("�� �÷��̾� ����");

        // �÷��̾ ��ó�� �ִ��� Ȯ��
        if (player != null)
        {
            PlayerHP playerScript = player.GetComponent<PlayerHP>();
            if (playerScript != null)
                if (playerScript != null)
                {
                    playerScript.TakeDamage(1, this.transform.position);
                }
        }
    }

    private void Think()
    {
        nextMove = Random.Range(-1, 2);
        render.flipX = nextMove == -1;
        float nextThinkTime = Random.Range(2f, 5f);
        Invoke("Think", nextThinkTime); // ���� �ð� �� ���� ����
    }

    private void Turn()
    {
        nextMove *= -1;
        render.flipX = nextMove == -1;
    }

    private void CheckPlatform()
    {
        isPlayerOnSamePlatform = Mathf.Abs(player.position.y - transform.position.y) < 0.5f;
    }

    // 2024/11/14 ������ �� hp �� ��� �߰�

    // �浹 ����
    // �̰� istrigger�� ���� ��Ÿ��� trigger�� �÷��̾ �� ��Ÿ��� ������ ���� �׾������
    // collision���� ���� �Ѿ˰� ���� �� �� trigger ���� ���� ü���� ���� �ʴ´�


    // ������ ���
    public void TakeDamage(int damage)
    {
        Debug.Log("�ƾ�");
        Hp -= damage;
        if (Hp <= 0)
            Destroy(this.gameObject);
    }

    // ���ο�
    public IEnumerator Slow()
    {
        speed -= 1.5f;
        yield return new WaitForSeconds(1.5f);
        speed += 1.5f;
    }
}