﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TOJAM
{
    public static class Constants 
    {
        //ENUMS
        public enum GameState { none, menu, game, gameOver, paused, launching}
        public enum PlatformDifficulty { none, easy, medium, hard, expert, end, runway}
        public enum ObstacleType { none, rockSmall, rockMed, rockLarge, trafficCone, oil, goat, car, barrier, runway}
        public enum MenuType { none, main, results}

        //SCENES
        public const string SCENE_GAMEPLAY = "Gameplay";
        public const string SCENE_PRELOADER = "Preloader";

        //GAMEPLAY VARIABLES
        public const float PLAYER_BASE_SPEED = 0f;
        public const int GAME_END_NUM_PLATFORMS = 100;

        //TAGS
        public const string TAG_PLAYER_CART = "PlayerCart";
        public const string TAG_PLAYER_PERSON = "PlayerPerson";
        public const string TAG_GROUND = "Ground";
        public const string TAG_OBSTACLE = "Obstacle";

        //MENU
        public const string ANIMATOR_SHOW_TRIGGER = "show";

        //PLAYER PREFS
        public const string PLAYER_PREFS_BEST = "Best";


        public static float GetSpeedForObstacle (ObstacleType type)
        {
            switch(type)
            {
                case ObstacleType.barrier:
                    return -100f;
                case ObstacleType.car:
                    return -100f;
                case ObstacleType.goat:
                    return -25f;
                case ObstacleType.none:
                    return 0f;
                case ObstacleType.oil:
                    return 10f;
                case ObstacleType.rockLarge:
                    return -15f;
                case ObstacleType.rockMed:
                    return -10f;
                case ObstacleType.rockSmall:
                    return -5f;
                case ObstacleType.trafficCone:
                    return -15f;
                default:
                    return 0f;
            }
        }
    }
}

