using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

namespace Max_Almog.MyCompany.MyGame
{
    public class MultiplayerCam : MonoBehaviour
    {
        public static CinemachineVirtualCamera followCamera;

        private void Awake()
        {
            followCamera = GetComponent<CinemachineVirtualCamera>();
        }
    }
}
