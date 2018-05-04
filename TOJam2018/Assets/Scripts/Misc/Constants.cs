using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TOJAM
{
    public static class Constants 
    {
        //ENUMS
        public enum GameState { none, menu, game, gameOver, paused}

        //GAMEPLAY VARIABLES
        public const float PLAYER_BASE_SPEED = 0f;

        //TAGS
        public const string TAG_PLAYER_CART = "PlayerCart";
        public const string TAG_PLAYER_PERSON = "PlayerPerson";
        public const string TAG_GROUND = "Ground";
        public const string TAG_OBSTACLE = "Obstacle";
    }
}

