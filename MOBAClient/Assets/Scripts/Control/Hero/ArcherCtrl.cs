using Common.Config;
using UnityEngine;

public class ArcherCtrl : HeroCtrl, IResourceListener
{
    [SerializeField]
    private Transform HitPoint;

    protected override void Start()
    {
        base.Start();

        // 加载音效
        ResourcesManager.Instance.Load(Paths.SOUND_ARCHER_ATTACK, typeof(AudioClip), this);
        ResourcesManager.Instance.Load(Paths.SOUND_ARCHER_DEATH, typeof(AudioClip), this);
    }

    public override void AttackRequest()
    {
        // 创建攻击特效
        GameObject go = PoolManager.Instance.GetObject("ArcherArrow");
        go.transform.position = HitPoint.position;
        if (Target == null)
            return;

        go.GetComponent<FlightProps>().Init(Target.transform, ServerConfig.SkillId, Model.Id, 
            Target.Model.Id, GameData.HeroData.Id == Model.Id);
    }

    public void OnLoaded(string assetName, object asset)
    {
        switch (assetName)
        {
            case Paths.SOUND_ARCHER_ATTACK:
                m_ClipDict.Add("attack", asset as AudioClip);
                break;
            case Paths.SOUND_ARCHER_DEATH:
                m_ClipDict.Add("death", asset as AudioClip);
                break;
        }
    }
}
