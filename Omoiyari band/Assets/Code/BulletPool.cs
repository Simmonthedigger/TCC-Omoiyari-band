using System.Collections.Generic;
using UnityEngine;

public class BulletPool : MonoBehaviour
{
    public static BulletPool Instance;

    [Header("Configurações do Pool")]
    [SerializeField] private GameObject bulletPrefab;
    [SerializeField] private int poolSize = 1000;

    private List<GameObject> pooledBullets;

    void Awake()
    {
        Instance = this;
        InitializePool();
    }

    void InitializePool()
    {
        pooledBullets = new List<GameObject>();

        for (int i = 0; i < poolSize; i++)
        {
            GameObject bullet = Instantiate(bulletPrefab, transform);
            bullet.SetActive(false);
            pooledBullets.Add(bullet);
        }
    }

    // Pega uma bala desativada da "piscina"
    public GameObject GetBullet()
    {
        for (int i = 0; i < pooledBullets.Count; i++)
        {
            if (!pooledBullets[i].activeInHierarchy)
            {
                return pooledBullets[i];
            }
        }

        // Se o pool acabar, cria uma nova para não quebrar o jogo (segurança)
        GameObject bullet = Instantiate(bulletPrefab, transform);
        bullet.SetActive(false);
        pooledBullets.Add(bullet);
        return bullet;
    }
}