using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

public class BossPattern : MonoBehaviour
{
    [Header("Referências")]
    [SerializeField] private Transform playerTransform; // Arraste seu Player aqui

    [Header("Configurações da Espiral")]
    [SerializeField] private float spiralFireRate = 0.05f;
    private float currentSpiralAngle = 0f;

    void Start()
    {
        // Exemplos de ativação de padrões:

        // 1. Atira um anel a cada 2 segundos
        InvokeRepeating(nameof(FireRingExample), 1f, 2f);

        // 2. Começa a atirar a espiral Touhou
        StartCoroutine(FireSpiralCoroutine());
    }

    void Update()
    {
        // 2. Substitua o Input antigo por este formato do Novo Input System:
        var keyboard = Keyboard.current;
        if (keyboard != null && keyboard.spaceKey.wasPressedThisFrame)
        {
            FireAtPlayer(5, 8f);
        }
    }

    // --- PADRÃO 1: ANEL PERFEITO (Ring Shot) ---
    public void FireRing(int bulletCount, float speed)
    {
        float angleStep = 360f / bulletCount;
        float angle = 0f;

        for (int i = 0; i < bulletCount; i++)
        {
            SpawnBulletFromPool(transform.position, angle, speed);
            angle += angleStep;
        }
    }

    private void FireRingExample() => FireRing(24, 5f);


    // --- PADRÃO 2: ESPIRAL DINÂMICA ---
    IEnumerator FireSpiralCoroutine()
    {
        while (true)
        {
            // Atira 4 braços de espiral ao mesmo tempo
            for (int i = 0; i < 4; i++)
            {
                float angle = currentSpiralAngle + (i * 90f);
                SpawnBulletFromPool(transform.position, angle, 6f);
            }

            // O SEGREDO: Soma um ângulo fixo para o próximo frame criar o redemoinho
            currentSpiralAngle += 6f;

            yield return new WaitForSeconds(spiralFireRate);
        }
    }


    // --- PADRÃO 3: DIRECIONADO AO JOGADOR (Aim Shot com Atan2) ---
    public void FireAtPlayer(int bulletCount, float speed)
    {
        if (playerTransform == null) return;

        // Calcula a direção e o ângulo em relação ao jogador usando Atan2
        Vector2 direction = playerTransform.position - transform.position;
        float targetAngle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;

        // Se for apenas 1 bala, vai reto no jogador. Se forem mais, faz um leque (Shotgun)
        if (bulletCount == 1)
        {
            SpawnBulletFromPool(transform.position, targetAngle, speed);
        }
        else
        {
            float spread = 10f; // Distância angular entre as balas do leque
            float startAngle = targetAngle - ((bulletCount - 1) * spread) / 2f;

            for (int i = 0; i < bulletCount; i++)
            {
                float angle = startAngle + (i * spread);
                SpawnBulletFromPool(transform.position, angle, speed);
            }
        }
    }

    // Função auxiliar para encurtar o código de ativação
    private void SpawnBulletFromPool(Vector3 position, float angle, float speed, float accel = 0f)
    {
        GameObject bulletObj = BulletPool.Instance.GetBullet();
        if (bulletObj != null)
        {
            bulletObj.transform.position = position;
            bulletObj.SetActive(true);
            bulletObj.GetComponent<Bullet>().Setup(angle, speed, accel);
        }
    }
}