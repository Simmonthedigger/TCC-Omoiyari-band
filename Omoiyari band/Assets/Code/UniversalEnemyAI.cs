using UnityEngine;

public class EnemyTwoPhaseMovement : MonoBehaviour
{
    [System.Serializable]
    public struct MovementPattern
    {
        public Vector2 direction;
        public float speed;
        public float waveAmplitude;
        public float waveFrequency;
        public bool waveOnX;
        public bool useCircular;
        public float circleRadius;
        public float circleSpeed;
        public float duration; // Quanto tempo essa fase dura
    }

    [Header("Fase 1: Movimento Inicial")]
    public MovementPattern phase1;

    [Header("Fase 2: Movimento Posterior")]
    public MovementPattern phase2;

    private float timer;
    private bool isPhase2 = false;
    private Vector3 startPos;
    private Vector3 phaseTransitionPos; // Posição onde a fase 1 terminou

    void Start()
    {
        startPos = transform.position;
        phaseTransitionPos = transform.position;
    }

    void Update()
    {
        timer += Time.deltaTime;

        // FASE 1 -> FASE 2
        if (!isPhase2 && timer >= phase1.duration)
        {
            isPhase2 = true;
            timer = 0; // Reseta para a fase 2
            phaseTransitionPos = transform.position; // Nova base para a fase 2
        }
        // FASE 2 -> FASE 1 (O LOOP ACONTECE AQUI)
        else if (isPhase2 && timer >= phase2.duration)
        {
            isPhase2 = false;
            timer = 0; // Reseta para a fase 1 voltar do início
            phaseTransitionPos = transform.position; // Nova base para a fase 1 reiniciar de onde o inimigo está
        }

        // Sempre usa a 'phaseTransitionPos' como ponto de partida atualizado
        MovementPattern current = isPhase2 ? phase2 : phase1;
        Vector3 move = CalculatePattern(current, timer);

        // Corrigido: Se p.useCircular estiver ativo, o Cosseno/Seno no tempo 0 não é (0,0).
        // Para evitar que o inimigo dê um pequeno "pulo/teletransporte" no frame 0 de cada fase,
        // subtraímos o deslocamento inicial do círculo.
        if (current.useCircular)
        {
            move.x -= Mathf.Cos(0) * current.circleRadius;
            move.y -= Mathf.Sin(0) * current.circleRadius;
        }

        transform.position = phaseTransitionPos + move;
    }

    Vector3 CalculatePattern(MovementPattern p, float t)
    {
        // 1. Linear
        Vector3 pos = (Vector3)p.direction.normalized * p.speed * t;

        // 2. Onda
        if (p.waveAmplitude != 0)
        {
            float wave = Mathf.Sin(t * p.waveFrequency) * p.waveAmplitude;
            if (p.waveOnX) pos.x += wave;
            else pos.y += wave;
        }

        // 3. Círculo
        if (p.useCircular)
        {
            pos.x += Mathf.Cos(t * p.circleSpeed) * p.circleRadius;
            pos.y += Mathf.Sin(t * p.circleSpeed) * p.circleRadius;
        }

        return pos;
    }
}