using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

namespace Pluto
{
    public class CameraManager : MonoBehaviour
    {
        public static CameraManager instance;
        [SerializeField] private CinemachineVirtualCamera camForMyPlayer;

        private void Awake()
        {
            instance = this;
        }

        public void CamForMyPlayerFocusThis(Transform transform)
        {
            camForMyPlayer.Follow = transform;
        }
    }
}