using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

namespace Pluto
{
    public class PlayerNameShow : MonoBehaviour
    {
        [SerializeField] private TextMeshPro nameText;
        [SerializeField] private Color color_isMine;
        [SerializeField] private Color color_notMine;

        public void SetName(string text, bool isMine)
        {
            nameText.text = text;
            nameText.color = isMine ? color_isMine : color_notMine;
        }
    }
}