using System;
using System.Data;
using System.Numerics;
namespace MohawkGame2D;

public class Game
{
    float _timeofday;
        
    // Framecount Variable.
    int frameCount = 0;

    float BGX = 0;
    float BGY = 600;

    int FlowerCount = 10;
    int[] FlowerPositionX;
    
    // Texture Assets
    Texture2D Background = Graphics.LoadTexture("../../../Assets/BackgroundForest.png");
    Texture2D Trees = Graphics.LoadTexture("../../../Assets/HidingTrees.png");
    Texture2D Player = Graphics.LoadTexture("../../../Assets/PlayerCharacter.png");
    Texture2D Flower = Graphics.LoadTexture("../../../Assets/Flower.png");     
    Texture2D Grass = Graphics.LoadTexture("../../../Assets/Grass.png");
    
    // Audio Assets
    Sound WalkingFX = Audio.LoadSound("../../../Assets/move.wav");
    Sound DayFX = Audio.LoadSound("../../../Assets/move.wav");
    Sound NightFX = Audio.LoadSound("../../../Assets/move.wav");
    Sound FlowerSpawnFX = Audio.LoadSound("../../../Assets/move.wav");
    Sound FlowerPickFX = Audio.LoadSound("../../../Assets/move.wav");
    
    // Vactors
    Vector2 position1 = new Vector2(0, 0);
    
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
    }
    public void Update()
    {
        
        float BGX = 0;
        float BGY = 600;
        
        _timeofday += Time.DeltaTime / 1.2f;
        
        float TOD = 6 * (1 + (float)Math.Cos(Math.PI * _timeofday / 12));
        
        float TOD2 = 6 * (1 - (float)Math.Cos(Math.PI * _timeofday / 12));
        int FlowerSpawnRate = Random.Integer(0, 30);

        if (Input.IsKeyboardKeyDown(KeyboardInput.D))
            PlayerMovementX += PlayerSpeed;  // Move right
        if (Input.IsKeyboardKeyDown(KeyboardInput.A))
            PlayerMovementX -= PlayerSpeed;  // Move left
            
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
        
        Graphics.Draw(Player, PlayerMovementX, 0);
        
        Graphics.Draw(Trees, position1);
        
        if (TOD >= 6)
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
    }
}


