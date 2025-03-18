using System;
using System.Data;
using System.Diagnostics;
using System.Numerics;
namespace MohawkGame2D;

public class Game
{
    SafeZone[] safeZones; // Safezone Encapsulation
    
    float flowerSpawnTime = 0;  // Starts flower spawn timer
    float flowerSpawnInterval = Random.Float(2f, 6f); // Adjust to control spawn frequency

    float _timeofday;  // Day - Night cycle main variable
    
    int Score = 0;  // Score
    
    int FlowerCount = 10; // Max number of flowers on screen
    int[] FlowerPositionX; // Flower position array

    // Bools
    bool isPlaying = false;
    bool isPlaying2 = false;
    bool isDay = true;
    bool isNight = false;
    bool isDead = false;
    bool isVictory = false;

    // Texture Assets
    Texture2D Background = Graphics.LoadTexture("../../../Assets/BackgroundForest.png");
    Texture2D Trees = Graphics.LoadTexture("../../../Assets/HidingTrees.png");
    Texture2D Player = Graphics.LoadTexture("../../../Assets/PlayerCharacter.png");
    Texture2D Flower = Graphics.LoadTexture("../../../Assets/Flower.png");
    Texture2D Grass = Graphics.LoadTexture("../../../Assets/Grass.png");
    Texture2D VictoryScreen = Graphics.LoadTexture("../../../Assets/VictoryScreen.png");
    Texture2D DeathScreen = Graphics.LoadTexture("../../../Assets/DeathScreen.png");
    
    // Audio Assets
    Sound WalkingFX = Audio.LoadSound("../../../Assets/Audio/Movement.wav");
    Sound DayFX = Audio.LoadSound("../../../Assets/Audio/DayFX.wav");
    Sound NightFX = Audio.LoadSound("../../../Assets/Audio/NightFX.wav");
    Sound Day1FX = Audio.LoadSound("../../../Assets/Audio/Day1FX.wav");
    Sound FlowerPickFX = Audio.LoadSound("../../../Assets/Audio/FlowerPick.wav");
    Sound FlowerSpawnFX = Audio.LoadSound("../../../Assets/Audio/FlowerSpawn.wav");

    // Vectors
    Vector2 position1 = new Vector2(0, 0);

    // Player Movement
    float PlayerMovementX = 300f;
    float PlayerSpeed = 3f;
    
    public void Setup()
    {
        Window.SetTitle("Dark Forest Quest");
        Window.SetSize(800, 600);

        Draw.LineSize = 1;
        
        FlowerPositionX = new int[FlowerCount];  // Flower Array Setup
        for (int i = 0; i < FlowerCount; i++)
        {
            FlowerPositionX[i] = -1; // Marks as empty
        }

        Audio.Play(Day1FX); // Intro Audio

        // Safezones Setup
        
        safeZones = new SafeZone[2];  
        safeZones[0] = new SafeZone(70, 80);
        safeZones[1] = new SafeZone(650, 80);
    }

    public void Update()
    {
        bool isMoving = false;  // Movement Variable
        bool isSafe = true;  // Death Variable

        _timeofday += Time.DeltaTime / 1.2f;  // Program's proportionality to Time

        float TOD = 6 * (1 + (float)Math.Cos(Math.PI * _timeofday / 12));  // Oscillating Value between 12-0 (used for day/night cycle)
        
        float TOD2 = 6 * (1 - (float)Math.Cos(Math.PI * _timeofday / 12)); // Oscillating Value between 0-12 (just in case)

        // Player movement
        
        if (Input.IsKeyboardKeyDown(KeyboardInput.D))
        {
            PlayerMovementX += PlayerSpeed; // Move right
            isMoving = true; // Declarations for audio
        }

        if (Input.IsKeyboardKeyDown(KeyboardInput.A))
        {
            PlayerMovementX -= PlayerSpeed; // Move left
            isMoving = true; // Declarations for audio
        }

        // Play the sound only if moving and not already playing
        if (isMoving && !isPlaying)
        {
            Audio.Play(WalkingFX);
            isPlaying = true; // Declarations for audio
        }

        // Stop the sound if no movement keys are pressed
        if (!isMoving && isPlaying)
        {
            Audio.Stop(WalkingFX);
            isPlaying = false; // Declarations for audio
        }

        // Night Cycle FX
        
        if (TOD >= 4)
        {
            Audio.Play(NightFX);
        }

        // Day Cycle FX
        
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
        
        // Remove Tint Color
        ColorF RemoveTint = new ColorF(1f, 1f, 1f);

        // Monster Eyes
        
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
        
        Graphics.Draw(Player, PlayerMovementX, 470); // Player Character

        Graphics.Draw(Trees, position1);  // Forefront Trees
        
        // References for SafeZone Hitboxes
        //Draw.FillColor = Color.Yellow;
        //Draw.Circle(80, 500, 4);
        //Draw.Circle(140, 500, 4);
        //Draw.Circle(640, 500, 4);
        //Draw.Circle(730, 500, 4);

        // Update flower spawn timer
        flowerSpawnTime += Time.DeltaTime;

        if (flowerSpawnTime >= flowerSpawnInterval && TOD >= 2.7)
        {
            // Reset the spawn timer
            flowerSpawnTime = 0;

            // Find an empty spot in the flower array
            for (int i = 0; i < FlowerCount; i++)
            {
                if (FlowerPositionX[i] == -1) // Check for an "empty" slot
                {
                    FlowerPositionX[i] = Random.Integer(210, 560);
                    break;
                }
            }
        }
        
        // Draw the flowers
        for (int i = 0; i < FlowerCount; i++)
        {
            if (FlowerPositionX[i] != -1) // Only draw flowers at valid positions
            {
                Graphics.Draw(Flower, FlowerPositionX[i], 500);
            }
        }

        Graphics.Draw(Grass, position1); // Forefront Grass

        Draw.FillColor = Color.White;  // Score
        Text.Draw("Score: " + Score, 600, 30);
        
        // Check for flower collection
        for (int i = 0; i < FlowerCount; i++)
        {
            if (FlowerPositionX[i] != -1) // Check only active flowers
            {
                // Check if player touches the flower
                if (Math.Abs(PlayerMovementX - FlowerPositionX[i]) < 17) // Adjust to change flower hitbox
                {
                    // Play flower collection sound
                    Audio.Play(FlowerPickFX);
            
                    // Increase score
                    Score++;

                    // Remove flower by marking it as inactive
                    FlowerPositionX[i] = -1;
                    
                    // Check for victory condition
                    if (Score >= 30)
                    {
                        isVictory = true;
                    }
                }
            }
        }
        if (isVictory)  // Victory Screen
        {
            Audio.Stop(DayFX);
            Audio.Stop(NightFX);
            Graphics.Tint = RemoveTint;
            Graphics.Draw(VictoryScreen, position1);
    
            // Restart the game if the player presses 'R'
            if (Input.IsKeyboardKeyDown(KeyboardInput.R))
            {
                isVictory = false;
                Score = 0;
                PlayerMovementX = 100f;  // Reset player position
                _timeofday = 0;          // Reset time of day
                Setup();                 // Reinitialize the game
            }
            return;
        }

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

            if (!isSafe) // Death Check
            {
                isDead = true;
            }

            if (isDead)  // Death Screen
            {
                _timeofday = -10;
                
                Score = 0;
                Audio.Stop(NightFX);
                Graphics.Tint = RemoveTint;
                Graphics.Draw(DeathScreen, position1);

                // Restart the game if the player presses 'R'
                if (Input.IsKeyboardKeyDown(KeyboardInput.R))
                {
                    isDead = false;
                    PlayerMovementX = 100f;  // Reset player position
                    _timeofday = 0;          // Reset time of day
                    Setup();                 // Reinitialize the game
                }
            }
        }
    }
}


