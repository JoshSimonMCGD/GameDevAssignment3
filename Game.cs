// Include the namespaces (code libraries) you need below.
using System;
using System.Numerics;

// The namespace your code is in.
namespace MohawkGame2D;

public class Game
{
    // Place your variables here:
    
    float _timeofday;
        
    // Framecount Variable.
    int frameCount = 0;

    float BGX = 0;
    float BGY = 600;

    /// <summary>
    ///     Setup runs once before the game loop begins.
    /// </summary>
    public void Setup()
    {
            Window.SetTitle("Dark Forest Quest");
            Window.SetSize(800, 600);
            
    }

    /// <summary>
    ///     Update runs every frame.
    /// </summary>
    public void Update()
    {
        Texture2D bg = Graphics.LoadTexture("../../../../Visual Assets/800x600_Wallpaper_Blue_Sky.png");
            
        float BGX = 0;
        float BGY = 600;
        
        _timeofday += Time.DeltaTime;
        
        float TOD = 6 * (1 - (float)Math.Cos(Math.PI * _timeofday / 12));
            
        // Color Variables
        // Sky
        float r = 0.0f;
        float g = TOD / 18;
        float b = TOD / 12;
        //float c = TOD / 12;
            
        // Background
        ColorF bgColor = new ColorF(r, g, b);
        Window.ClearBackground(bgColor);
    }
}


