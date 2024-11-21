using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NER_bullet : MonoBehaviour
{
    [SerializeField] private float bulletSpeed = 20f;
    private Vector3 direction; // 총알의 발사 방향

    // 방향 설정 메소드
    public void SetDirection(Vector3 dir)
    {
        direction = dir.normalized;
    }

    void Update()
    {
        transform.Translate(direction * bulletSpeed * Time.deltaTime);
    }
}