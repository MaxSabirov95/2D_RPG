using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

namespace Max_Almog.MyCompany.MyGame
{
    public class MultiplayerCam : MonoBehaviour
    {
        CinemachineVirtualCamera thisCamera;

        // Start is called before the first frame update
        void Start()
        {
            thisCamera = GetComponent<CinemachineVirtualCamera>();
            thisCamera.Follow = PlayerMovement.LocalPlayerInstance.transform;
        }
    }
}
