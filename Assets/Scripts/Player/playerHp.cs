
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerHP : MonoBehaviour
{
    [SerializeField] private Slider HpBarSlider; // UI �����̴�
    public float maxHP = 100;     // �ִ� HP
    private float currentHP;      // ���� HP

    PlayerMove Playerscript;
    void Start()
    {
        // �ʱ� HP ����
        currentHP = maxHP;

        HpBarSlider.minValue = 0;
        HpBarSlider.maxValue = maxHP;

        // �����̴��� �ʱ� �� ����
        HpBarSlider.value = currentHP;

        Playerscript = GetComponent<PlayerMove>();
    }

    // HP�� ���ҽ�Ű�� �Լ�
    /*void DecreaseHP(int amount)
    {
        // HP ����
        currentHP -= amount;

        // HP�� 0 �̸����� �������� �ʵ��� ó��
        if (currentHP < 0)
        {
            currentHP = 0;
        }

        // �����̴� �� ����
        HpBarSlider.value = currentHP;
    }*/

    private void HpBar()
    {
        // �÷��̾��� HP ���� �ʱ� 
        currentHP = maxHP;
        HpBarSlider.maxValue = maxHP;
        HpBarSlider.value = currentHP;
    }
    public void TakeDamage(float damage, Vector2 targetpos)
    {
        currentHP -= damage;
        // ü���� 0 �������� Ȯ��
        if (currentHP <= 0)
        {
            currentHP = 0;
            Die(); // ��� ó��
        }
        CheckHp(); // ü�� UI ����
        
    }

    private void Die()
    {
        Debug.Log("�÷��̾� ���!");
        // ��� ó�� ���� �߰� (��: ���� ���� ȭ��)
    }
    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Enemy"))
        {
            //DecreaseHP(1);  // �浹 �� HP 1 ����*
        }
    }

    public void CheckHp() //hp ��� ������Ʈ
    {
        if (HpBarSlider != null)
            HpBarSlider.value = (int)(currentHP / maxHP);
    }

    /*public void Damage(float damage) //������ ����
    {
        if (maxHP == 0 || currentHP <= 0) //ü�� 0���� �н�
            return;
        currentHP -= damage;
        CheckHp();
        if (currentHP <= 0)
        {
            //ü�� 0 �÷��̾� ���
        }
    }*/

}
