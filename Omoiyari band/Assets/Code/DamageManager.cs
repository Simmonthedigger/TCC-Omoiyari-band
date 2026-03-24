using UnityEngine;
using System.Collections.Generic;
using System.Linq;
using UnityEngine.InputSystem; // Necessário para o novo sistema

public class DamageManager : MonoBehaviour
{
    [Header("Configurações de Combos")]
    public List<AtaqueData> ataquesCadastrados;
    public float tempoLimiteCombo = 1.0f;

    private List<string> entradaAtual = new List<string>();
    private float timerReset;

    void Update()
    {
        var keyboard = Keyboard.current;
        if (keyboard == null) return;

        // Detecta os cliques únicos no novo sistema
        if (keyboard.mKey.wasPressedThisFrame) RegistrarTecla("m");
        if (keyboard.kKey.wasPressedThisFrame) RegistrarTecla("k");
        if (keyboard.oKey.wasPressedThisFrame) RegistrarTecla("o");
        if (keyboard.pKey.wasPressedThisFrame) RegistrarTecla("p");

        // Timer para limpar o combo se o jogador demorar
        if (entradaAtual.Count > 0)
        {
            timerReset += Time.deltaTime;
            if (timerReset > tempoLimiteCombo)
            {
                entradaAtual.Clear();
            }
        }
    }

    void RegistrarTecla(string tecla)
    {
        entradaAtual.Add(tecla);
        timerReset = 0;
        ProcessarSequencia();
    }

    void ProcessarSequencia()
    {
        foreach (var ataque in ataquesCadastrados)
        {
            if (CheckSequence(ataque.sequencia))
            {
                AplicarDanoGlobal(ataque.dano, ataque.nomeAtaque);
                entradaAtual.Clear();
                break;
            }
        }
    }

    bool CheckSequence(List<string> sequenciaAtaque)
    {
        if (entradaAtual.Count < sequenciaAtaque.Count) return false;

        var ultimosInputs = entradaAtual.Skip(entradaAtual.Count - sequenciaAtaque.Count).ToList();

        for (int i = 0; i < sequenciaAtaque.Count; i++)
        {
            if (ultimosInputs[i] != sequenciaAtaque[i].ToLower()) return false;
        }
        return true;
    }

    void AplicarDanoGlobal(float dano, string nome)
    {
        Debug.Log($"<color=red>Combo: {nome}!</color> Dano: {dano}");

        // Busca todos os inimigos com a Tag "Enemy"
        GameObject[] inimigos = GameObject.FindGameObjectsWithTag("Enemy");

        foreach (GameObject inimigo in inimigos)
        {
            if (inimigo.TryGetComponent<EnemyHealth>(out var health))
            {
                health.TakeDamage(dano);
            }
        }
    }
}