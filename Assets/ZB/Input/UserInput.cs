using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace ZB.Input
{
    public delegate void Vector2Delegate(Vector2 vector2);
    public class UserInput : MonoBehaviour
    {
        [SerializeField] private KeyInput[] keyInputs;
        [SerializeField] private Direction2DInput[] direction2DInputs;

        public void OnUpdate()
        {
            for (int i = 0; i < keyInputs.Length; i++)
            {
                keyInputs[i].OnUpdate();
            }
            for (int i = 0; i < direction2DInputs.Length; i++)
            {
                direction2DInputs[i].OnUpdate();
            }
        }
        public KeyInput FindKeyInput(string name)
        {
            for (int i = 0; i < keyInputs.Length; i++)
            {
                if (keyInputs[i].Comment == name)
                    return keyInputs[i];
            }
            return null;
        }
        public Direction2DInput FindDirection2DInput(string name)
        {
            for (int i = 0; i < direction2DInputs.Length; i++)
            {
                if (direction2DInputs[i].Comment == name)
                    return direction2DInputs[i];
            }
            return null;
        }

        [System.Serializable]
        public class KeyInput
        {
            public string Comment;
            public KeyCode KeyCode;
            public UnityEvent DownEvent;
            public UnityEvent UpEvent;
            public UnityEvent StayEvent;
            public void OnUpdate()
            {
                if (UnityEngine.Input.GetKeyDown(KeyCode) &&
                    DownEvent != null)
                {
                    DownEvent.Invoke();
                }
                if (UnityEngine.Input.GetKeyUp(KeyCode) &&
                    UpEvent != null)
                {
                    UpEvent.Invoke();
                }
                if (UnityEngine.Input.GetKey(KeyCode) &&
                    StayEvent != null)
                {
                    StayEvent.Invoke();
                }
            }
        }

        [System.Serializable]
        public class Direction2DInput
        {
            public string Comment;
            public KeyCode UpKey;
            public KeyCode DownKey;
            public KeyCode LeftKey;
            public KeyCode RightKey;
            public Vector2 Direction;
            public Vector2Delegate StayEvent;
            [SerializeField] private bool left = false;
            [SerializeField] private bool right = false;
            [SerializeField] private bool up = false;
            [SerializeField] private bool down = false;
            public void OnUpdate()
            {
                if (UnityEngine.Input.GetKey(UpKey))
                    up = true;
                else
                    up = false;
                if (UnityEngine.Input.GetKey(DownKey))
                    down = true;
                else
                    down = false;
                if (UnityEngine.Input.GetKey(LeftKey))
                    left = true;
                else
                    left = false;
                if (UnityEngine.Input.GetKey(RightKey))
                    right = true;
                else
                    right = false;

                int x = 0;
                int y = 0;
                if (up && !down) y = 1;
                else if (!up && down) y = -1; 
                else y = 0;
                if (right && !left) x = 1;
                else if(!right && left) x = -1;
                else x = 0;

                Direction = new Vector2(x, y);

                StayEvent.Invoke(Direction);
            }
        }
    }
}