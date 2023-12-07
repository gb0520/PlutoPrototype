using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Pluto
{
    public class UserInputSender : MonoBehaviour
    {
        public static UserInputSender instance;

        [SerializeField] private ZB.Input.UserInput userInput;
        [SerializeField] private Player myPlayer;

        public void PlayerConnect(Player player)
        {
            myPlayer = player;
            ZB.Input.UserInput.KeyInput attackInput = userInput.FindKeyInput("Attack");
            ZB.Input.UserInput.KeyInput runInput = userInput.FindKeyInput("Run");
            ZB.Input.UserInput.Direction2DInput moveInput = userInput.FindDirection2DInput("Move");

            if (attackInput.DownEvent == null) attackInput.DownEvent = new UnityEngine.Events.UnityEvent();
            if (attackInput.UpEvent == null) attackInput.UpEvent = new UnityEngine.Events.UnityEvent();
            if (attackInput.StayEvent == null) attackInput.StayEvent = new UnityEngine.Events.UnityEvent();

            attackInput.DownEvent.AddListener(myPlayer.OnInputDown_Atk);
            runInput.DownEvent.AddListener(myPlayer.OnInputDown_Run);
            runInput.UpEvent.AddListener(myPlayer.OnInputUp_Run);
            moveInput.StayEvent += myPlayer.OnDirection2DInput_Run;
        }
        public void PlayerDisconnect()
        {
            ZB.Input.UserInput.KeyInput attackInput = userInput.FindKeyInput("Attack");
            ZB.Input.UserInput.KeyInput runInput = userInput.FindKeyInput("Run");
            ZB.Input.UserInput.Direction2DInput moveInput = userInput.FindDirection2DInput("Move");

            attackInput.DownEvent.RemoveListener(myPlayer.OnInputDown_Atk);
            runInput.DownEvent.RemoveListener(myPlayer.OnInputDown_Run);
            runInput.UpEvent.RemoveListener(myPlayer.OnInputUp_Run);
            moveInput.StayEvent -= myPlayer.OnDirection2DInput_Run;
            myPlayer = null;
        }
        private void Awake()
        {
            instance = this;
        }
        private void Update()
        {
            if (myPlayer != null) 
                userInput.OnUpdate();
        }
    }
}