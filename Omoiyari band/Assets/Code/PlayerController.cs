using UnityEngine;
using UnityEngine.InputSystem; // Importante para o novo sistema

public class PlayerController : MonoBehaviour
{
    [Header("Configurações de Velocidade")]
    public float moveSpeed = 8f;
    public float focusSpeed = 3.5f;

    [Header("Prefabs de Tiro")]
    public GameObject prefabM;
    public GameObject prefabK;
    public GameObject prefabO;
    public GameObject prefabP;

    public Transform firePoint;
    public float fireRate = 0.1f;
    private float nextFireTime = 0f;

    void Update()
    {
        Mover();
        Atirar();
    }

    void Mover()
    {
        // Pega o teclado atual
        var keyboard = Keyboard.current;
        if (keyboard == null) return;

        Vector2 direction = Vector2.zero;

        // Movimentação WASD/Setas
        if (keyboard.wKey.isPressed || keyboard.upArrowKey.isPressed) direction.y += 1;
        if (keyboard.sKey.isPressed || keyboard.downArrowKey.isPressed) direction.y -= 1;
        if (keyboard.aKey.isPressed || keyboard.leftArrowKey.isPressed) direction.x -= 1;
        if (keyboard.dKey.isPressed || keyboard.rightArrowKey.isPressed) direction.x += 1;

        // Lógica de Foco (Shift Esquerdo)
        float currentSpeed = keyboard.leftShiftKey.isPressed ? focusSpeed : moveSpeed;

        transform.position += (Vector3)direction.normalized * currentSpeed * Time.deltaTime;
    }

    void Atirar()
    {
        if (Time.time < nextFireTime) return;

        var keyboard = Keyboard.current;

        if (keyboard.mKey.isPressed) Shoot(prefabM);
        else if (keyboard.kKey.isPressed) Shoot(prefabK);
        else if (keyboard.oKey.isPressed) Shoot(prefabO);
        else if (keyboard.pKey.isPressed) Shoot(prefabP);
    }

    void Shoot(GameObject bullet)
    {
        if (bullet != null && firePoint != null)
        {
            Instantiate(bullet, firePoint.position, Quaternion.identity);
            nextFireTime = Time.time + fireRate;
        }
    }
}