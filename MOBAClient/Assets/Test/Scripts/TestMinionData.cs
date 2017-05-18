using Common.Dto;
using UnityEngine;

public class TestMinionData : MonoBehaviour
{
    public int TeamId = 2;
    public int Hp = 220;
    public int Attack = 30;
    public int Defense = 20;
    public Transform End;

    void Start()
    {
        DtoMinion data = new DtoMinion(0, 0, TeamId, Hp, Attack, Defense, 3, 1.5, 3, "Minion");
        MinionCtrl ctrl = GetComponent<MinionCtrl>();
        ctrl.Model = data;
        ctrl.Init(data, false);
        ctrl.EndPoint = End.position;

        ctrl.ChangeState(AIStateEnum.IDLE);
    }
}
