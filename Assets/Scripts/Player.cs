//Copyright 2021 Andrew Young
using System;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Game/Player")]
public class Player : ScriptableObject
{
    [SerializeField]
    private Number m_health;
    [SerializeField]
    private Element m_startElement;
    [SerializeField]
    private int m_adaptAtElementCount = 5;

    public Player Instance => s_instance;
    public Number Health => m_health;

    public Element AdaptedElement { get; private set; } = null;
    public Element EnvironmentalElement { get; private set; } = null;
    public Element CollectionElement { get; private set; } = null;
    public bool IsSafe { get; private set; } = true;

    public event Action<Element> CollectionElementChanged;
    public event Action<Element> AdaptedElementChanged;
    public event Action<Element> EnvironmentElementChanged;
    public event Action<bool> SafeChanged;

    private static Player s_instance = null;

    private Dictionary<Element, int> m_elementZoneCount = new Dictionary<Element, int>();

    void OnEnable()
    {
        s_instance = this;
        if (m_startElement != null)
        {
            Respawn();
        }
    }
    private void OnDisable()
    {
        if (CollectionElement != null)
        {
            CollectionElement.NumberCollected.ValueChanged -= OnElementCollected;
        }
    }

    public void Respawn()
    {
        Health.Reset();
        SetEnvironmentalElement(m_startElement);
        SetCollectionElement(m_startElement);
        SetAdaptedElement(m_startElement);
        m_elementZoneCount.Clear();
    }

    public void SetCollectionElement(Element e)
    {
        if (CollectionElement != e)
        {
            if (CollectionElement != null)
            {
                CollectionElement.NumberCollected.ValueChanged -= OnElementCollected;
                CollectionElement.DropAll();
            }
            CollectionElement = e;
            CollectionElement.NumberCollected.ValueChanged += OnElementCollected;
            CollectionElementChanged?.Invoke(CollectionElement);
            return;
        }
    }
    void OnElementCollected(int value)
    {
        if (CollectionElement != AdaptedElement && value >= m_adaptAtElementCount)
        {
            SetAdaptedElement(CollectionElement);
        }
        if (CollectionElement == AdaptedElement && Health.Value < Health.InitialValue)
        {
            Health.Value += 1;
        }
    }

    public void EnvironmentElementEntered(Element e)
    {
        //add 1 to element count
        int count = 0;
        m_elementZoneCount.TryGetValue(e, out count);
        m_elementZoneCount[e] = count + 1;

        foreach (var element in m_elementZoneCount)
        {
            if (element.Key != e && element.Value > 0) return; 
        }

        //if this is the only element left
        SetEnvironmentalElement(e);
    }
    public void EnvironmentElementExited(Element e)
    {
        //suptract 1 from element count
        int count = 0;
        m_elementZoneCount.TryGetValue(e, out count);
        if (count > 0)
        {
            m_elementZoneCount[e] = count - 1;
        }
        //if count is now zero check remaining elements
        if (count <= 1)
        {
            count = 0;
            foreach (var element in m_elementZoneCount)
            {
                if (element.Value > 0)
                {
                    count++;
                    e = element.Key;
                }
            }
            if (count == 1) //apply the only element remaining
            {
                SetEnvironmentalElement(e);
            }
        }
    }
    void SetEnvironmentalElement(Element e)
    {
        if (EnvironmentalElement != e)
        {
            EnvironmentalElement = e;
            EnvironmentElementChanged?.Invoke(EnvironmentalElement);
            Debug.LogFormat("Element is now {0}", e.name);
            CheckIsSafe();
        }
    }

    void SetAdaptedElement(Element e)
    {
        AdaptedElement = e;
        AdaptedElementChanged?.Invoke(AdaptedElement);
        m_health.Reset();
        CheckIsSafe();
    }

    void CheckIsSafe()
    {
        IsSafe = EnvironmentalElement == AdaptedElement;
        SafeChanged?.Invoke(IsSafe);
    }
}
