using System;
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
    Texture2D Player = Graphics.LoadTexture("../../../Assets/800x600_Wallpaper_Blue_Sky.png");
    Texture2D Flower = Graphics.LoadTexture("../../../Assets/800x600_Wallpaper_Blue_Sky.png");     
        
    // Vactors
    Vector2 position1 = new Vector2(0, 0);
  
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
        
        _timeofday += Time.DeltaTime;
        
        float TOD = 6 * (1 + (float)Math.Cos(Math.PI * _timeofday / 12));
        
        float TOD2 = 6 * (1 - (float)Math.Cos(Math.PI * _timeofday / 12));
        int FlowerSpawnRate = Random.Integer(0, 30);
            
        // Color Variables
        // Sky
        float r = 0.0f;
        float g = TOD / 18;
        float b = TOD / 12;
        //float c = TOD / 12;

        float r2 = TOD / 24;
        float g2 = TOD / 22 + 0.1f;
        float b2 = TOD / 26 + 0.2f;
        float c2 = 1.0f;
            
        // Background
        ColorF bgColor = new ColorF(r, g, b);
        Window.ClearBackground(bgColor);
        
        // Asset Tint
        ColorF bgTint = new ColorF(r2, g2, b2, c2);
        
        Graphics.Tint = bgTint;
        Graphics.Draw(Background, position1);
        
        
        
        
        Graphics.Draw(Trees, position1);
        
        if (TOD >= 6)
        {
            //if (FlowerSpawnRate >= 29)
            {
                for (int i = 0; i < FlowerCount; i++)
                {
                    Draw.FillColor = Color.Green;
                    Draw.Rectangle(FlowerPositionX[i] - 1, 510, 3, 40);
                    
                    Draw.FillColor = Color.Red;
                    Draw.Circle(FlowerPositionX[i], 510, 15); 
                    
                    Draw.FillColor = Color.Yellow;
                    Draw.Circle(FlowerPositionX[i], 510, 7);
                }
            }
        }
    }
}


