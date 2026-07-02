using UnityEngine;

public class Bullet : MonoBehaviour
{
    private float speed;
    private float angle;
    private bool isPlayerTargeted;

    // Configurações para padrões complexos
    private float acceleration;
    private float lifetime;

    public void Setup(float startAngle, float startSpeed, float startAcceleration = 0)
    {
        angle = startAngle;
        speed = startSpeed;
        acceleration = startAcceleration;
        lifetime = 0f;

        // Opcional: Rotacionar o sprite da bala para apontar na direção do movimento
        transform.rotation = Quaternion.Euler(0, 0, angle - 90f);
    }

    void Update()
    {
        lifetime += Time.deltaTime;

        // Aplica aceleração/desaceleração se houver
        speed += acceleration * Time.deltaTime;

        // 1. O Segredo: Conversão de Polares para Cartesianas
        float angleRad = angle * Mathf.Deg2Rad;
        float moveX = Mathf.Cos(angleRad);
        float moveY = Mathf.Sin(angleRad);

        Vector3 movement = new Vector3(moveX, moveY, 0) * speed * Time.deltaTime;
        transform.position += movement;

        // Desativa a bala se ela for longe demais (ajuste o valor conforme sua câmera)
        if (transform.position.magnitude > 25f)
        {
            gameObject.SetActive(false);
        }
    }
}