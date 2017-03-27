using System;

[Serializable]
public class UIPanelInfo {
    public string Name;
    public string Path;

    public UIPanelType PanelType
    {
        get
        {
            // 将字符串转换为枚举
            return (UIPanelType)Enum.Parse(typeof(UIPanelType), Name);
        } 
    }
}
