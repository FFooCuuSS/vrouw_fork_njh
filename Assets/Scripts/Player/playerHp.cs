
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerHP : MonoBehaviour
{
    [SerializeField] private Slider HpBarSlider; // UI 슬라이더
    public float maxHP = 100;     // 최대 HP
    private float currentHP;      // 현재 HP

    PlayerMove Playerscript;
    void Start()
    {
        // 초기 HP 설정
        currentHP = maxHP;

        HpBarSlider.minValue = 0;
        HpBarSlider.maxValue = maxHP;

        // 슬라이더의 초기 값 설정
        HpBarSlider.value = currentHP;

        Playerscript = GetComponent<PlayerMove>();
    }

    // HP를 감소시키는 함수
    /*void DecreaseHP(int amount)
    {
        // HP 감소
        currentHP -= amount;

        // HP가 0 미만으로 떨어지지 않도록 처리
        if (currentHP < 0)
        {
            currentHP = 0;
        }

        // 슬라이더 값 갱신
        HpBarSlider.value = currentHP;
    }*/

    private void HpBar()
    {
        // 플레이어의 HP 값을 초기 
        currentHP = maxHP;
        HpBarSlider.maxValue = maxHP;
        HpBarSlider.value = currentHP;
    }
    public void TakeDamage(float damage, Vector2 targetpos)
    {
        currentHP -= damage;
        // 체력이 0 이하인지 확인
        if (currentHP <= 0)
        {
            currentHP = 0;
            Die(); // 사망 처리
        }
        CheckHp(); // 체력 UI 갱신
        
    }

    private void Die()
    {
        Debug.Log("플레이어 사망!");
        // 사망 처리 로직 추가 (예: 게임 오버 화면)
    }
    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Enemy"))
        {
            //DecreaseHP(1);  // 충돌 시 HP 1 감소*
        }
    }

    public void CheckHp() //hp 상시 업데이트
    {
        if (HpBarSlider != null)
            HpBarSlider.value = (int)(currentHP / maxHP);
    }

    /*public void Damage(float damage) //데미지 받음
    {
        if (maxHP == 0 || currentHP <= 0) //체력 0이하 패스
            return;
        currentHP -= damage;
        CheckHp();
        if (currentHP <= 0)
        {
            //체력 0 플레이어 사망
        }
    }*/

}
