using UnityEngine;
using System.Collections.Generic;

[CreateAssetMenu(fileName = "NovoAtaque", menuName = "Jogo/Sequencia de Ataque")]
public class AtaqueData : ScriptableObject
{
    public string nomeAtaque;
    [Tooltip("Escreva as teclas em minúsculo: m, k, o, p")]
    public List<string> sequencia;
    public float dano;
}