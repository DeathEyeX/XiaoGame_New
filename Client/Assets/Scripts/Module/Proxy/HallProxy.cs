using UnityEngine;
using System.Collections;
using Message;
using System;

namespace RedStone
{
    public class HallProxy : HallProxyBase
    {
        public Data.PlayerData playerData { get; private set; }
        public BattleServerInfo battleServerInfo { get; private set; }


        public bool isLogin { get; private set; }
        public bool isConnected { get; private set; }

        public HallProxy()
        {
            playerData = new Data.PlayerData();
        }

        public override void OnInit()
        {
            isConnected = false;
            isLogin = false;

            network.socket.onConnected = () =>
            {
                isConnected = true;
                Debug.Log("Network Connect Success (Main Server).");
                Login();
            };

            network.RegisterNetwork<CMMatchSuccess>(OnMatchSuccess);
        }

        public void Connect(int timeOut, Action timeOutCallback)
        {
            network.socket.Connect();
            Task.WaitFor(timeOut, () =>
             {
                 if (!isConnected)
                 {
                     timeOutCallback.Invoke();
                 }
             });
        }

        public void Login()
        {
            CMLoginRequest req = new CMLoginRequest();
            req.DeviceID = DeviceID.UUID;
            network.Send<CMLoginRequest, CMLoginReply>(req
            , (reply) =>
             {
                 isLogin = true;
                 playerData.SetData(reply.PlayerInfo);
                 GF.ChangeState<LoadingState>();
             });
        }

        public void BeginMatch(int gameID, int gameMode)
        {
            CMMatchRequest msg = new CMMatchRequest();
            msg.GameID = gameID;
            msg.GameMode = gameMode;
            msg.GroupID = -1;
            SendMessage<CMMatchRequest, CMMatchReply>(msg
            , (rep) =>
            {
                Debug.Log(rep.Status);
                SendEvent(EventDef.MatchBegin);
            });
        }

        public void CancelMatch()
        {
            CMMatchCancel msg = new CMMatchCancel();
            SendMessage(msg);
            SendEvent(EventDef.MatchCancel);
        }

        void OnMatchSuccess(CMMatchSuccess msg)
        {
            Debug.Log(msg.BattleServerInfo.Address);
        }
    }
}