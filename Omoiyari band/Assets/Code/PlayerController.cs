using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    [Header("Configurações de Velocidade")]
    public float moveSpeed = 8f;
    public float focusSpeed = 3.5f;

    void Update()
    {
        Mover();
    }

    void Mover()
    {
        var keyboard = Keyboard.current;
        if (keyboard == null) return;

        Vector2 direction = Vector2.zero;

        // Movimentação WASD e Setas
        if (keyboard.wKey.isPressed || keyboard.upArrowKey.isPressed) direction.y += 1;
        if (keyboard.sKey.isPressed || keyboard.downArrowKey.isPressed) direction.y -= 1;
        if (keyboard.aKey.isPressed || keyboard.leftArrowKey.isPressed) direction.x -= 1;
        if (keyboard.dKey.isPressed || keyboard.rightArrowKey.isPressed) direction.x += 1;

        // Lógica de Foco (Segurar Shift para precisão)
        float currentSpeed = keyboard.leftShiftKey.isPressed ? focusSpeed : moveSpeed;

        // Aplica o movimento diretamente no Transform
        if (direction != Vector2.zero)
        {
            transform.position += (Vector3)direction.normalized * currentSpeed * Time.deltaTime;
        }
    }
}