using System;
using System.Collections.Generic;
using Common.Code;
using Common.Config;
using Common.Dto;
using LitJson;
using MOBAServer.Cache;
using MOBAServer.DataBase.Manager;
using MOBAServer.Room;
using Photon.SocketServer;

namespace MOBAServer.Handler
{
    public class BuyItemHandler : BaseHandler
    {
        public override void OnOperationRequest(OperationRequest request, SendParameters sendParameters, MobaPeer peer)
        {
            // 获取道具
            int itemId = (int) request[(byte)ParameterCode.ItemId];
            ItemModel item = ItemData.GetItem(itemId);
            if (item == null)
                return;

            // 获取房间
            int playerId = UserManager.GetPlayer(peer.Username).Identification;
            BattleRoom room = Caches.Battle.GetRoomByPlayerId(playerId);
            if (room == null)
                return;

            // 获取英雄
            DtoHero hero = room.GetDtoHero(playerId);

            // 金币不足
            if (hero.Money < item.Price)
            {
                SendResponse(peer, request.OperationCode, null, ReturnCode.Falied, "金币不足");
                return;
            }
            // 格子不够
            if (hero.Equipments.Length == ServerConfig.ItemMaxCount)
            {
                SendResponse(peer, request.OperationCode, null, ReturnCode.Falied, "装备已满");
                return;
            }

            // 开始购买
            hero.Money -= item.Price;
            hero.AddItem(item);
            // 给所有客户端发送消息 谁买了什么装备
            Dictionary<byte, object> data = new Dictionary<byte, object>();
            data.Add((byte)ParameterCode.DtoHero, JsonMapper.ToJson(hero));
            room.Brocast(OpCode, data);
        }
    }
}
