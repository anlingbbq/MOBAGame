using UnityEngine;

public class bl_TestMouseEvent : MonoBehaviour
{
    public bl_HUDText HUDRoot;
    public bool enemy;
    public bool friendly;
    public bool player;
    public bool Other;
    public bool Other2;

    private string[] Text = new string[] { "Floating Text", "Awasome", "But you say", "Nice", "Beatiful", "Surprising", "Impossible" };

    private void OnMouseDown()
    {
        int i = Random.Range(0, Text.Length);
        if (this.friendly)
        {
            HUDRoot.NewText(Text[i], base.transform);
         
        }
        else if (this.enemy)
        {
            if (Random.Range(0, 2) == 1)
            {
                HUDRoot.NewText("- " + Random.Range(50, 100).ToString(), base.transform, Color.red, 8, 20f, -1f, 2.2f, bl_Guidance.RightDown);
            }
            else
            {
                HUDRoot.NewText("- " + Random.Range(50, 100).ToString(), base.transform, Color.red, 8, 20f, -1f, 2.2f, bl_Guidance.LeftDown);
            }
        }
        else if (this.player)
        {
            if (Random.Range(0, 2) == 1)
            {
                if (Random.Range(0, 2) == 1)
                {
                    HUDRoot.NewText("+ " + Random.Range(50, 100).ToString(), base.transform, Color.green, 8, 20f, -1f, 2.2f, bl_Guidance.RightDown);
                }
                else
                {
                    HUDRoot.NewText("+ " + Random.Range(50, 100).ToString(), base.transform, Color.green, 8, 20f, -1f, 2.2f, bl_Guidance.LeftDown);
                }
            }
            else if (Random.Range(0, 2) == 1)
            {
                HUDRoot.NewText("- " + Random.Range(50, 100).ToString(), base.transform, Color.red, 8, 20f, -1f, 2.2f, bl_Guidance.RightDown);
            }
            else
            {
                HUDRoot.NewText("- " + Random.Range(50, 100).ToString(), base.transform, Color.red, 8, 20f, -1f, 2.2f, bl_Guidance.LeftDown);
            }
        }
        else if (Other)
        {
            HUDRoot.NewText("- " + Random.Range(50, 100).ToString(), base.transform, Color.red, 8, 20f, -1f, 2.2f, bl_Guidance.Down);
        }
        else if (Other2)
        {
            HUDRoot.NewText("- " + Random.Range(50, 100).ToString(), base.transform, Color.green, 8, 20f, -1f, 2.2f, bl_Guidance.Up);
        }
    }
}

