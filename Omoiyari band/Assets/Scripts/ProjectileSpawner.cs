using UnityEngine;

public class ProjectileSpawner : MonoBehaviour
{
    [Header("Prefab do Tiro")]
    [Tooltip("Arraste aqui o Prefab do tiro que contém o script Projectile.")]
    public GameObject projectilePrefab;

    [Header("Configurações do Spawner")]
    [Tooltip("Tempo em segundos entre cada disparo automático.")]
    public float spawnInterval = 1.5f;

    [Tooltip("Dano que o tiro spawnado irá causar ao jogador.")]
    public float projectileDamage = 10f;

    [Tooltip("Direção para onde o tiro vai andar (ex: Vector2.down = para baixo).")]
    public Vector2 shootDirection = Vector2.down;

    [Header("Modo de Teste Manual")]
    [Tooltip("Se marcado, dispara um tiro ao pressionar a tecla ESPAÇO.")]
    public bool allowManualShootWithSpace = true;

    private float timer;

    private void Update()
    {
        // 1. Spawner Automático por Tempo
        timer += Time.deltaTime;
        if (timer >= spawnInterval)
        {
            SpawnProjectile();
            timer = 0f;
        }

        // 2. Disparo Manual para Teste Rápido (Espaço)
        if (allowManualShootWithSpace && Input.GetKeyDown(KeyCode.Space))
        {
            SpawnProjectile();
        }
    }

    /// <summary>
    /// Instancia o tiro e passa a direção e o dano configurados.
    /// </summary>
    public void SpawnProjectile()
    {
        if (projectilePrefab == null)
        {
            Debug.LogWarning("Nenhum Prefab de tiro atribuído no Spawner!");
            return;
        }

        // Cria o tiro na posição atual do Spawner
        GameObject newProjectile = Instantiate(projectilePrefab, transform.position, Quaternion.identity);

        // Configura a direção e dano do tiro
        Projectile projScript = newProjectile.GetComponent<Projectile>();
        if (projScript != null)
        {
            projScript.Setup(shootDirection, projectileDamage);
        }
    }
}