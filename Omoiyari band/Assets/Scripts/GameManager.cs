using UnityEngine;
using UnityEngine.InputSystem; // Necessário caso use o Input System novo

public class GameManager : MonoBehaviour
{
    // Singleton para facilitar o acesso de qualquer outro script
    public static GameManager Instance { get; private set; }

    [Header("Configurações de Pontuação")]
    [Tooltip("Pontuação com a qual o jogador inicia a fase.")]
    public float currentScore = 0f;

    [Tooltip("Pontuação mínima necessária para VENCER ao final do tempo.")]
    public float targetScoreToWin = 50f;

    [Tooltip("Se a pontuação cair abaixo deste valor, é GAMEOVER imediato.")]
    public float gameOverScoreThreshold = -70f;

    [Header("Configurações de Tempo / Música")]
    [Tooltip("Duração total da fase/música em segundos.")]
    public float stageDuration = 180f;

    private float timer;
    private bool isGameActive = true;

    [Header("Controles de Teste Manual")]
    [Tooltip("Habilita o ganho/perda de pontos via teclado.")]
    public bool enableTestInputs = true;

    [Tooltip("Quantidade de pontos a somar ao pressionar a tecla de aumento.")]
    public float pointsPerTestPress = 10f;

    private void Awake()
    {
        // Padrão Singleton simples
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void Start()
    {
        timer = stageDuration;
    }

    private void Update()
    {
        if (!isGameActive) return;

        // --- TESTES MANUAIS DE TECLADO ---
        if (enableTestInputs)
        {
            HandleTestInputs();
        }

        // Contagem regressiva do tempo da fase
        timer -= Time.deltaTime;

        if (timer <= 0)
        {
            EndStage();
        }
    }

    /// <summary>
    /// Método de teste para alterar o score via teclado.
    /// Funciona tanto com o Input Antigo quanto com o Novo Input System (via Keyboard.current).
    /// </summary>
    private void HandleTestInputs()
    {
        // Teste usando o Novo Input System (já que seu PlayerController usa)
        var keyboard = Keyboard.current;
        if (keyboard != null)
        {
            // Tecla 'E' Ganha Pontos
            if (keyboard.eKey.wasPressedThisFrame)
            {
                AddScore(pointsPerTestPress);
                Debug.Log($"<color=green>[TESTE] +{pointsPerTestPress} Pontos!</color> Total: {currentScore}");
            }

            // Tecla 'Q' Perde Pontos (Dano)
            if (keyboard.qKey.wasPressedThisFrame)
            {
                ApplyDamage(pointsPerTestPress);
                Debug.Log($"<color=red>[TESTE] -{pointsPerTestPress} Pontos!</color> Total: {currentScore}");
            }
        }
    }

    /// <summary>
    /// Altera a pontuação (valores positivos aumentam a barra, valores negativos causam dano).
    /// </summary>
    public void AddScore(float amount)
    {
        if (!isGameActive) return;

        currentScore += amount;
        Debug.Log($"Pontuação Atual: {currentScore}");

        // Checa condição de GameOver imediato por pontuação muito baixa
        if (currentScore <= gameOverScoreThreshold)
        {
            TriggerGameOver();
        }
    }

    /// <summary>
    /// Aplica dano diretamente na pontuação.
    /// </summary>
    public void ApplyDamage(float damageAmount)
    {
        // Como é dano, subtraímos do score (ou somamos o valor negativo)
        AddScore(-Mathf.Abs(damageAmount));
    }

    private void EndStage()
    {
        isGameActive = false;

        if (currentScore >= targetScoreToWin)
        {
            Debug.Log("<color=cyan>VITÓRIA!</color> Você manteve a pontuação acima da meta ao final da música.");
            // Chame aqui sua lógica de próxima fase / tela de vitória
        }
        else
        {
            Debug.Log("<color=red>DERROTA!</color> O tempo acabou e você não atingiu a meta de pontos.");
            TriggerGameOver();
        }
    }

    private void TriggerGameOver()
    {
        isGameActive = false;
        Debug.Log("<color=red>GAME OVER!</color>");
        // Chame aqui sua lógica de reiniciar a fase ou abrir menu de GameOver
    }

    // Getters úteis para a UI (Barra de Vida/Pontuação e Timer)
    public float GetCurrentScore() => currentScore;
    public float GetRemainingTime() => Mathf.Max(0, timer);
}