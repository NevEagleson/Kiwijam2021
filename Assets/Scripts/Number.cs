//Copyright 2021 Andrew Young
using System;
using UnityEngine;

[CreateAssetMenu(menuName = "Game/Number")]
public class Number : ScriptableObject
{
    [SerializeField]
    private int m_initialValue;

    public int InitialValue => m_initialValue;

    private int m_value = 0;
    public int Value
    {
        get
        {
#if UNITY_EDITOR
            if (!Application.isPlaying) return m_initialValue;
#endif
            return m_value;
        }
        set
        {
            if (value < 0) return;
            m_value = value;
            ValueChanged?.Invoke(m_value);
            if (m_value == 0)
                ValueZero?.Invoke();
        }
    }

    public event Action<int> ValueChanged;
    public event Action ValueReset;
    public event Action ValueZero;


    public void Reset()
    {
        Value = m_initialValue;
        ValueReset?.Invoke();
    }

    private void OnEnable()
    {
        Reset();
    }

    public static implicit operator int (Number number)
    {
        return number.Value;
    }
}
