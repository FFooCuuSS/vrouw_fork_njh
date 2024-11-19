using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using System.Net;
using Unity.VisualScripting;
using UnityEditor.Experimental;
using UnityEditor.Experimental.GraphView;
using UnityEditor.Tilemaps;
using UnityEngine;
using UnityEngine.UI;

public class PlayerMove : MonoBehaviour
{
    //�÷��̾� �¿� �̵�
    [SerializeField] private float speed = 8f;//�÷��̾� ���ǵ�
    private float moveInput = 0f;//�÷��̾� �¿��̵� input
    private bool isFacingRight = true;//�¿� ó�ٺ��°�

    //�÷��̾� ����
    private float jumpingPower = 16f;//���� ����

    //�÷��̾� ���� �̵�
    private HingeJoint2D joint;
    private bool isOnRope = false;
    HingeJoint2D linkedHinge;
    [SerializeField] private float ropeForce = 15f;
    float ropeCooltime = 0.1f;
    bool ableRope = false;

    //�÷��̾� �뽬
    private bool isDash = false;
    private bool canDash = true;
    [SerializeField] private float dashDuration = 0.2f;//�뽬 ���ӽð�
    [SerializeField] private float dashCoolTime = 2.0f;//�뽬 ��Ÿ��
    [SerializeField] private float dashSpeed = 10.0f;//�뽬 �ӵ�

    //�׿�
    [SerializeField] private Rigidbody2D rb;
    [SerializeField] private Transform groundCheck;
    [SerializeField] private LayerMask groundLayer;

    //���� ü��
    [SerializeField] private float curHealth;
    //�ִ� ü��
    [SerializeField] public float maxHealth;
    //HP ����
    public Slider HpBarSlider;
    Rigidbody2D rigid;

    private SpriteRenderer spriteRenderer;

    private void Start()
    {
        joint = GetComponent<HingeJoint2D>();
        spriteRenderer = GetComponent<SpriteRenderer>(); // SpriteRenderer �ʱ�ȭ
        rigid = GetComponent<Rigidbody2D>(); // Rigidbody2D �ʱ�ȭ
    }

    // Update is called once per frame
    void Update()
    {

        if (Input.GetKey(KeyCode.LeftArrow))
        {
            moveInput = -1f;
        }
        else if (Input.GetKey(KeyCode.RightArrow))
        {
            moveInput = 1f;
        }
        else
        {
            moveInput = 0f;
        }
        if (Input.GetKeyDown(KeyCode.UpArrow) && IsGrounded())
        {
            rb.velocity = new Vector2(rb.velocity.x, jumpingPower);
        }
        if (Input.GetKey(KeyCode.UpArrow) && isOnRope)
        {
            if (!ableRope)
            {
                StartCoroutine(UpRope());
            }
        }
        if (Input.GetKey(KeyCode.DownArrow) && isOnRope)
        {
            if (!ableRope)
            {
                StartCoroutine(DownRope());
            }
        }
        if (Input.GetKeyUp(KeyCode.UpArrow) && rb.velocity.y > 0f)
        {
            rb.velocity = new Vector2(rb.velocity.x, rb.velocity.y * 0.5f);
        }
        if (Input.GetKeyDown(KeyCode.LeftShift) && canDash && !isDash)
        {
            StartCoroutine(dash());
        }
        if (Input.GetKeyDown(KeyCode.Space) && isOnRope)
        {
            isOnRope = false;
            joint.enabled = false;
            //rb.velocity+=new Vector2(rb.velocity.x, rb.velocity.y);
            rb.velocity += rb.velocity.normalized * rb.velocity.magnitude * 1.5f;//1.5f�� �ݵ� ���
        }

        Flip();
    }

    IEnumerator UpRope()
    {
        if (Rope.FindHead(linkedHinge) != linkedHinge.connectedBody)
        {
            ableRope = true;
            Rigidbody2D connectedRigidbody = linkedHinge.connectedBody;
            //���� ����Ǿ��ִ� ������Ʈ(1)���� ������Ʈ(1)�� ����ִ� ������Ʈ(2)�� ����
            joint.connectedBody = connectedRigidbody;//������Ʈ(2)�� �÷��̾ ����

            joint.anchor = new Vector2(0, 0.5f);//�÷��̾��� anchor�� ������Ʈ�� �Ʒ��κ����� ����
            joint.connectedAnchor = new Vector2(0, -0.5f);
            linkedHinge = connectedRigidbody.GetComponent<HingeJoint2D>();
            //���� ���� �� ������Ʈ(2)�� ������Ʈ(1)�� �ִ� ������ �����
        }
        yield return new WaitForSeconds(ropeCooltime);
        ableRope = false;
    }
    IEnumerator DownRope()
    {
        ableRope = true;
        Rigidbody2D connectedRigidbody = Rope.FindBefore(linkedHinge);
        //����Ǿ��ִ� ������Ʈ(1)�� ����ִ� ������Ʈ(0)�� ����
        joint.connectedBody = connectedRigidbody;//������Ʈ(0)�� �÷��̾ ����

        joint.anchor = new Vector2(0, 0.5f);//�÷��̾��� anchor�� ������Ʈ�� �Ʒ��κ����� ����
        joint.connectedAnchor = new Vector2(0, -0.5f);
        linkedHinge = connectedRigidbody.GetComponent<HingeJoint2D>();
        //���� ���� �� ������Ʈ(0)�� ������Ʈ(1)�� �ִ� ������ �����
        yield return new WaitForSeconds(ropeCooltime);
        ableRope = false;
    }
    IEnumerator dash()
    {
        isDash = true;
        canDash = false;

        Debug.Log("Dash!");

        //rb.AddForce(new Vector2(horizontal* dashSpeed, 1f), ForceMode2D.Impulse);  // ��� �� ���ϱ�

        float dashDirection = transform.localScale.x > 0 ? 1 : -1;
        rb.velocity = new Vector2(dashDirection * dashSpeed, rb.velocity.y);

        yield return new WaitForSeconds(dashDuration);
        isDash = false;
        yield return new WaitForSeconds(dashCoolTime);
        canDash = true;
    }
    private void FixedUpdate()
    {

        if (isOnRope)
        {
            rb.AddForce(new Vector2(ropeForce * moveInput, 0f));
        }
        else
        {
            rb.velocity = new Vector2(moveInput * speed, rb.velocity.y);
        }

    }

    private bool IsGrounded()
    {
        return Physics2D.OverlapCircle(groundCheck.position, 0.2f, groundLayer);
    }

    private void Flip()
    {
        if (isFacingRight && moveInput < 0f || !isFacingRight && moveInput > 0f)
        {
            isFacingRight = !isFacingRight;
            Vector3 localScale = transform.localScale;
            localScale.x *= -1f;
            transform.localScale = localScale;

        }
    }

    private void OnTriggerEnter2D(Collider2D coll)
    {
        if (coll.CompareTag("Rope") && !isOnRope && Input.GetKey(KeyCode.UpArrow))
        {
            joint.enabled = true;
            Rigidbody2D ropeRb = coll.GetComponent<Rigidbody2D>();
            joint.connectedBody = ropeRb;

            joint.anchor = new Vector2(0, 0.5f);
            joint.connectedAnchor = new Vector2(0, -0.5f);

            isOnRope = true;
            linkedHinge = coll.GetComponent<HingeJoint2D>();


        }
    }

    public void Awake()
    {
        rigid = GetComponent<Rigidbody2D>();
    }
    public void SetUp(float amount)
    {
        maxHealth = amount;
        curHealth = maxHealth;
    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Enemy")
            return;
        //   OnDamaged(collision.transform.position);
    }
    public void OnDamaged(Vector2 targetPos)
    {
        //PlayerDamaged 10���̾�
        gameObject.layer = 10;
        //�÷��̾� ��������
        spriteRenderer.color = new Color(1, 1, 1, 0.3f);
        //�ǰ� �� �ڷ� ������
        int dirc = transform.position.x - targetPos.x > 0 ? 1 : -1;
        rigid.AddForce(new Vector2(dirc, 1) * 3, ForceMode2D.Impulse);

        Invoke("OffDamaged", 0.5f);

    }


    void OffDamaged()
    {
        //�������� Ǯ��
        gameObject.layer = 11;
        spriteRenderer.color = new Color(1, 1, 1, 1);
    }

}