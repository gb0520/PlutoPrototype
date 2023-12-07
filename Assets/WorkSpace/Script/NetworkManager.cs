using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using Photon.Pun;
using Photon.Realtime;

namespace Pluto.Network
{
    public class NetworkManager : MonoBehaviourPunCallbacks
    {
        public static NetworkManager instance;

        [SerializeField] private TMP_InputField inputField_userName;

        [SerializeField] private GameObject window_disconnect;
        [SerializeField] private GameObject window_respawn;

        public void Spawn()
        {
            PhotonNetwork.Instantiate("Pluto/Prefabs/Player", Vector3.zero, Quaternion.identity);
            window_respawn.SetActive(false);
        }
        public void Connet() => PhotonNetwork.ConnectUsingSettings();
        public void RespawnWindowActive() => window_respawn.SetActive(true);
        public override void OnConnectedToMaster()
        {
            PhotonNetwork.LocalPlayer.NickName = inputField_userName.text;
            PhotonNetwork.JoinOrCreateRoom("Room", new RoomOptions { MaxPlayers = 20 }, null);
        }
        public override void OnJoinedRoom()
        {
            window_disconnect.SetActive(false);
            Spawn();
        }
        public override void OnDisconnected(DisconnectCause cause)
        {
            window_disconnect.SetActive(true);
            window_respawn.SetActive(false);
        }

        private void Awake()
        {
            Screen.SetResolution(1280, 720, false);
            PhotonNetwork.SendRate = 60;
            PhotonNetwork.SerializationRate = 30;
            instance = this;
        }
        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.Escape) &&
                PhotonNetwork.IsConnected)
                PhotonNetwork.Disconnect();
        }
    }
}