using UnityEngine;

public class Projectile : MonoBehaviour
{
    [Header("Configurações do Tiro")]
    public float speed = 10f;
    public float damage = 10f;
    public float destroyTime = 5f; // Destrói o tiro após X segundos para não poluir a cena

    private Vector2 direction = Vector2.down; // Direção padrão: para baixo

    public void Setup(Vector2 newDirection, float newDamage)
    {
        direction = newDirection.normalized;
        damage = newDamage;
    }

    private void Start()
    {
        Destroy(gameObject, destroyTime);
    }

    private void Update()
    {
        // Move o tiro na direção configurada
        transform.position += (Vector3)direction * speed * Time.deltaTime;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        // Verifica se colidiu com o Player
        if (other.CompareTag("Player"))
        {
            if (GameManager.Instance != null)
            {
                GameManager.Instance.ApplyDamage(damage);
            }

            // Destrói o tiro ao acertar o jogador
            Destroy(gameObject);
        }
    }
}