using System;
using System.Data;
using System.Diagnostics;
using System.Numerics;
namespace MohawkGame2D;

public class Game
{

    SafeZone[] safeZones;

    float _timeofday;

    // Framecount Variable.
    int frameCount = 0;

    float BGX = 0;
    float BGY = 600;

    int FlowerCount = 10;
    int[] FlowerPositionX;

    // Bools
    bool isPlaying = false;
    bool isPlaying2 = false;
    bool isDay = true;
    bool isNight = false;
    bool isDead = false;

    // Texture Assets
    Texture2D Background = Graphics.LoadTexture("../../../Assets/BackgroundForest.png");
    Texture2D Trees = Graphics.LoadTexture("../../../Assets/HidingTrees.png");
    Texture2D Player = Graphics.LoadTexture("../../../Assets/PlayerCharacter.png");
    Texture2D Flower = Graphics.LoadTexture("../../../Assets/Flower.png");
    Texture2D Grass = Graphics.LoadTexture("../../../Assets/Grass.png");

    // Audio Assets
    Sound WalkingFX = Audio.LoadSound("../../../Assets/Audio/Movement.wav");
    Sound DayFX = Audio.LoadSound("../../../Assets/Audio/DayFX.wav");
    Sound NightFX = Audio.LoadSound("../../../Assets/Audio/NightFX.wav");
    Sound Day1FX = Audio.LoadSound("../../../Assets/Audio/Day1FX.wav");
    Sound FlowerPickFX = Audio.LoadSound("../../../Assets/move.wav");

    // Vectors
    Vector2 position1 = new Vector2(0, 0);

    // Floats
    float PlayerMovementX = 100f;
    float PlayerMovementY = 100f;
    float PlayerSpeed = 3.5f;

    public void Setup()
    {
        Window.SetTitle("Dark Forest Quest");
        Window.SetSize(800, 600);

        Draw.LineSize = 1;

        FlowerPositionX = new int[FlowerCount];
        for (int i = 0; i < FlowerCount; i++)
        {
            FlowerPositionX[i] = Random.Integer(210, 560);
        }

        Audio.Play(Day1FX);

        safeZones = new SafeZone[2];

        safeZones[0] = new SafeZone(80, 60);
        safeZones[1] = new SafeZone(650, 80);
    }

    public void Update()
    {
        bool isMoving = false;
        bool isSafe = true;
        float BGX = 0;
        float BGY = 600;

        _timeofday += Time.DeltaTime / 1.2f;

        float TOD = 6 * (1 + (float)Math.Cos(Math.PI * _timeofday / 12));

        float TOD2 = 6 * (1 - (float)Math.Cos(Math.PI * _timeofday / 12));
        int FlowerSpawnRate = Random.Integer(0, 30);

        if (Input.IsKeyboardKeyDown(KeyboardInput.D))
        {
            PlayerMovementX += PlayerSpeed; // Move right
            isMoving = true;
        }

        if (Input.IsKeyboardKeyDown(KeyboardInput.A))
        {
            PlayerMovementX -= PlayerSpeed; // Move left
            isMoving = true;
        }

        // Play the sound only if moving and not already playing
        if (isMoving && !isPlaying)
        {
            Audio.Play(WalkingFX);
            isPlaying = true;
        }

        // Stop the sound if no movement keys are pressed
        if (!isMoving && isPlaying)
        {
            Audio.Stop(WalkingFX);
            isPlaying = false;
        }

        if (TOD >= 4)
        {
            Audio.Play(NightFX);
        }

        if (TOD <= 4)
        {
            Audio.Play(DayFX);
        }

        // Color Variables
        // Sky
        float r = 0.0f;
        float g = TOD / 18;
        float b = TOD / 12;
        //float c = TOD / 12;

        // Asset Tint
        float r2 = TOD / 24;
        float g2 = TOD / 22 + 0.1f;
        float b2 = TOD / 26 + 0.2f;
        float c2 = 1.0f;

        // Monster Eyes
        float r3 = 1.0f;
        float g3 = TOD / 3;
        float b3 = TOD / 12;
        float c3 = 1.0f;

        // Background
        ColorF bgColor = new ColorF(r, g, b);
        Window.ClearBackground(bgColor);

        // Asset Tint
        ColorF bgTint = new ColorF(r2, g2, b2, c2);

        Graphics.Tint = bgTint;
        Graphics.Draw(Background, position1);

        if (TOD <= 2.5)
        {
            Draw.FillColor = new ColorF(r3, g3, b3, c3);
            Draw.Circle(270, 450, 4);
            Draw.Circle(310, 450, 4);
        }

        if (TOD <= 2.3)
        {
            Draw.FillColor = new ColorF(r3, g3, b3, c3);
            Draw.Circle(600, 430, 4);
            Draw.Circle(630, 430, 4);
        }

        if (TOD <= 2)
        {
            Draw.FillColor = new ColorF(r3, g3, b3, c3);
            Draw.Circle(380, 500, 3);
            Draw.Circle(410, 500, 3);
        }

        Graphics.Draw(Player, PlayerMovementX, 470);

        Graphics.Draw(Trees, position1);

        Draw.FillColor = Color.Yellow;
        Draw.Circle(80, 500, 4);
        Draw.Circle(140, 500, 4);
        Draw.Circle(640, 500, 4);
        Draw.Circle(730, 500, 4);

        if (TOD >= 2.7)
        {
            //if (FlowerSpawnRate >= 29)
            {
                for (int i = 0; i < FlowerCount; i++)
                {
                    Graphics.Draw(Flower, FlowerPositionX[i], 500);
                }
            }
        }

        Graphics.Draw(Grass, position1);

        // Check for player safety at night
        if (TOD <= 2.3)
        {
            isSafe = false;
            for (int i = 0; i < safeZones.Length; i++)
            {
                if (safeZones[i] != null && safeZones[i].IsPlayerSafe(PlayerMovementX))
                {
                    isSafe = true;
                    break;
                }
            }

            if (!isSafe)
            {
                isDead = true;
            }

            if (isDead)
            {
                _timeofday = -10;
                Audio.Stop(NightFX);
                Draw.FillColor = Color.Red;
                Draw.Rectangle(0, 0, 800, 600); // Full screen rectangle
                Draw.FillColor = Color.White;
                Text.Draw("You Died! Press R to Restart", 300, 300);

                // Restart the game if the player presses 'R'
                if (Input.IsKeyboardKeyDown(KeyboardInput.R))
                {
                    isDead = false;
                    Setup(); // Restart the game
                }

                return;
            }
        }
    }
}


