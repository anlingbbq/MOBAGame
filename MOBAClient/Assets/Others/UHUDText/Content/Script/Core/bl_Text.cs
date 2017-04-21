using System;
using UnityEngine;
using UnityEngine.UI;

public class bl_Text : MonoBehaviour
{
    public CanvasGroup LayoutRoot = null;
    public Text m_Text = null;
    public RectTransform Rect;

   [HideInInspector] public Color m_Color;
   [HideInInspector] public bl_Guidance movement;
   [HideInInspector] public float Xcountervail;
   [HideInInspector] public float Ycountervail;
   [HideInInspector] public int m_Size;
   [HideInInspector] public float m_Speed;
   [HideInInspector] public string m_text;
   [HideInInspector] public Transform m_Transform;
   [HideInInspector] public float Yquickness;
   [HideInInspector] public float YquicknessScaleFactor;
}