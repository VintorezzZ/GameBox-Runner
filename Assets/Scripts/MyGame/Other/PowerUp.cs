using System;
using UnityEngine;
using UnityEngine.UI;

namespace MyGame.Other
{
    public class PowerUp : MonoBehaviour
    {
        public PowerUpType type;
        public float duration;
        public Sprite image;
        public Sprite background;
    }
}

public enum PowerUpType
{
    None,
    Rpg
}