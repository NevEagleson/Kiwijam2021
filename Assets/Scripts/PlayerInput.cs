using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class PlayerInput
{
    private static bool m_fireDown;
    private static bool m_jumpDown;
    private static float m_horizontal;
    private static float m_vertical;

    public static void Update()
    {
    }

    public static float Horizontal => Input.GetAxis("Horizontal");
    public static float Vertical => Input.GetAxis("Vertical");
    public static bool JumpDown => Input.GetButton("Jump");
    public static bool FireDown => Input.GetButton("Fire1");
}
