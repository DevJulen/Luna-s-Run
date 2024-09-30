using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Constants
{
    // --- Controller types --- //
    public const string GAMEPAD = "Gamepad";
    public const string KEYBOARD_MOUSE = "Keyboard&Mouse";

    // --- Tags --- //
    public const string TAG_PLAYER = "Player";
    public const string TAG_BEE = "Bee";
    public const string TAG_SNAIL = "Snail";
    public const string TAG_ENEMY_STOMP = "EnemyStomp";
    public const string TAG_SNAIL_STOMP = "SnailStomp";
    public const string TAG_GHOST = "GhostEnemy";
    public const string TAG_DEATH_LIMIT = "DeathLimit";

    // --- Player Animator --- //
    public const string ANIM_PLAYER_MOVING = "Moving";
    public const string ANIM_PLAYER_GROUNDED = "Grounded";
    public const string ANIM_PLAYER_HANG_TIMING = "HangTiming";
    public const string ANIM_PLAYER_FALLING = "Falling";
    public const string ANIM_PLAYER_VELOCITY = "Velocity";
    public const string ANIM_PLAYER_DASHING = "Dashing";
    public const string ANIM_PLAYER_DEAD = "Dead";
    public const string ANIM_PLAYER_SURPRISED = "Surprised";

    // --- Ghost Enemy Animator --- //
    public const string ANIM_GHOST_MOVING = "Moving";
    public const string ANIM_GHOST_ATTACK = "Attack";
    public const string ANIM_GHOST_VELOCITY = "Velocity";
    public const string ANIM_GHOST_SCREAM = "Scream";

    // --- Scene Names --- //
    public const string TUTORIAL_SCENE = "Tutorial";
    public const string GAME_SCENE = "Game";
    public const string MAIN_MENU_SCENE = "MainMenu";
    public const string TOP_SCORES_SCENE = "TopScores";
    public const string CREDITS_SCENE = "Credits";
    public const string RANKING_SCENE = "Ranking";
    public const string LOAD_SCREEN_SCENE = "LoadScreen";
    public const string ADVICE_SCENE = "AdviceScreen";

    // --- Data Manager Keys --- //
    public const string DATE_FORMAT = "dd MMM yyyy";

    public const string MAX_STAR_COUNT1 = "maxStarCount1";
    public const string MAX_SNAILS_KILLED1 = "maxSnailsKilled1";
    public const string MAX_BEES_KILLED1 = "maxBeesKilled1";
    public const string MAX_SCORE_DATE1 = "maxScoreDate1";

    public const string MAX_STAR_COUNT2 = "maxStarCount2";
    public const string MAX_SNAILS_KILLED2 = "maxSnailsKilled2";
    public const string MAX_BEES_KILLED2 = "maxBeesKilled2";
    public const string MAX_SCORE_DATE2 = "maxScoreDate2";

    public const string MAX_STAR_COUNT3 = "maxStarCount3";
    public const string MAX_SNAILS_KILLED3 = "maxSnailsKilled3";
    public const string MAX_BEES_KILLED3 = "maxBeesKilled3";
    public const string MAX_SCORE_DATE3 = "maxScoreDate3";
}
