using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParticleCollisionHandler : MonoBehaviour
{
    private new ParticleSystem particleSystem;
    private List<ParticleCollisionEvent> collisionEvents = new List<ParticleCollisionEvent>();

    void Start()
    {
        particleSystem = GetComponent<ParticleSystem>();
    }

    void OnParticleCollision(GameObject other)
    {
        if (!other.CompareTag("Player"))
        {
            return;
        }

        // 获取碰撞事件数量
        int numCollisionEvents = particleSystem.GetCollisionEvents(other, collisionEvents);

        // 遍历所有碰撞事件
        for (int i = 0; i < numCollisionEvents; i++)
        {
            ParticleCollisionEvent collisionEvent = collisionEvents[i];

            HandleParticleCollision(other, i);
        }
    }

    void HandleParticleCollision(GameObject other, int particleIndex)
    {
        //Debug.Log($"粒子 {particleIndex} 与 {other.name} 碰撞");

        GlobalData.hurt();
    }
}
