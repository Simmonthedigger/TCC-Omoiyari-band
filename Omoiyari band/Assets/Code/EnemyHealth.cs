using UnityEngine;

public class EnemyHealth : MonoBehaviour
{
    public float vida = 100;

    public void TakeDamage(float quantidade)
    {
        vida -= quantidade;
        Debug.Log($"{gameObject.name} recebeu dano. Vida restante: {vida}");

        if (vida <= 0) Destroy(gameObject);
    }
}