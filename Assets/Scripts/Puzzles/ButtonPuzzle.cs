using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class ButtonPuzzle : MonoBehaviour
{
    [Header("Botones del puzzle")]
    [Tooltip("Botones que forman parte de este puzzle")]
    [SerializeField] private PressureButton[] buttons;
    [Tooltip("Cuántos botones deben estar presionados al mismo tiempo")]
    [SerializeField, Min(1)] private int requiredPressedCount = 2;
    [Tooltip("Si está activo, el puzzle queda resuelto aunque se suelten los botones")]
    [SerializeField] private bool solveOnce = true;

    [Header("Eventos")]
    [Tooltip("Acciones que se ejecutan al resolver el puzzle (por ejemplo, abrir una puerta)")]
    [SerializeField] private UnityEvent onSolved = new UnityEvent();
    [Tooltip("Acciones que se ejecutan si un puzzle repetible deja de estar resuelto")]
    [SerializeField] private UnityEvent onUnsolved = new UnityEvent();

    private readonly HashSet<PressureButton> subscribedButtons = new HashSet<PressureButton>();
    private readonly HashSet<PressureButton> pressedButtons = new HashSet<PressureButton>();

    public bool IsSolved { get; private set; }

    private void OnEnable()
    {
        subscribedButtons.Clear();
        pressedButtons.Clear();

        if (buttons != null)
        {
            foreach (PressureButton button in buttons)
            {
                if (button == null || !subscribedButtons.Add(button))
                    continue;

                button.StateChanged += OnButtonStateChanged;
                if (button.IsPressed)
                    pressedButtons.Add(button);
            }
        }

        UpdatePuzzleState();
    }

    private void OnDisable()
    {
        foreach (PressureButton button in subscribedButtons)
        {
            if (button != null)
                button.StateChanged -= OnButtonStateChanged;
        }

        subscribedButtons.Clear();
    }

    private void OnButtonStateChanged(PressureButton button, bool isPressed)
    {
        if (isPressed)
            pressedButtons.Add(button);
        else
            pressedButtons.Remove(button);

        UpdatePuzzleState();
    }

    private void UpdatePuzzleState()
    {
        int requiredCount = Mathf.Max(1, requiredPressedCount);
        bool shouldBeSolved = subscribedButtons.Count > 0
            && requiredCount <= subscribedButtons.Count
            && pressedButtons.Count >= requiredCount;

        if (shouldBeSolved == IsSolved || (IsSolved && solveOnce))
            return;

        IsSolved = shouldBeSolved;
        if (IsSolved)
            onSolved?.Invoke();
        else
            onUnsolved?.Invoke();
    }

    private void OnValidate()
    {
        requiredPressedCount = Mathf.Max(1, requiredPressedCount);
    }
}
