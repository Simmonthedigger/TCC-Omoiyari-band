using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems; // Necessário para detectar interações de UI

// Este script precisa que o objeto tenha um Animator e um Botão (UI)
[RequireComponent(typeof(Animator))]
[RequireComponent(typeof(Button))]
public class MenuButtonEffects : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerDownHandler, IPointerUpHandler
{
    private Animator animator;
    private Button button;

    // Nomes dos parâmetros exatos que criaremos no Animator Controller
    private string isHoveredParam = "IsHovered";
    private string isClickedParam = "IsClicked";

    private void Awake()
    {
        animator = GetComponent<Animator>();
        button = GetComponent<Button>();
    }

    private void Start()
    {
        // Garante que o botão comece sem o efeito de clique
        animator.SetBool(isClickedParam, false);
    }

    // --- Implementação das Interfaces de Evento ---

    // Chamado quando o mouse ENTRA na área do botão
    public void OnPointerEnter(PointerEventData eventData)
    {
        if (button.interactable)
        {
            animator.SetBool(isHoveredParam, true);
        }
    }

    // Chamado quando o mouse SAI da área do botão
    public void OnPointerExit(PointerEventData eventData)
    {
        animator.SetBool(isHoveredParam, false);
    }

    // Chamado quando o botão do mouse é PRESSIONADO (Inicia o clique)
    public void OnPointerDown(PointerEventData eventData)
    {
        if (button.interactable)
        {
            animator.SetBool(isClickedParam, true);
        }
    }

    // Chamado quando o botão do mouse é SOLTO (Termina o clique)
    public void OnPointerUp(PointerEventData eventData)
    {
        animator.SetBool(isClickedParam, false);
    }
}