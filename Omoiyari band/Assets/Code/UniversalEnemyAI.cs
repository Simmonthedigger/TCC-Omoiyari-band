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

        // Verifica se é hora de trocar de fase
        if (!isPhase2 && timer >= phase1.duration)
        {
            isPhase2 = true;
            timer = 0; // Reseta o timer para a segunda fase começar do zero
            phaseTransitionPos = transform.position; // Define a nova base de cálculo
        }

        // Escolhe qual padrão usar
        MovementPattern current = isPhase2 ? phase2 : phase1;
        Vector3 baseOrigin = isPhase2 ? phaseTransitionPos : startPos;

        // Cálculo do Movimento
        Vector3 move = CalculatePattern(current, timer);

        transform.position = baseOrigin + move;
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