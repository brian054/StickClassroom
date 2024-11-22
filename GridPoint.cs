using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

/*
 * Defines a GridPoint, which is a point in the
 * teacher grid path that has certain properties.
 * These points will create a GridMatrix and
 * GridPaths can be created for each level
 */

/*
 * Ummmmm okay so we create GridPoints in DeskLayout which is now more of a LevelBuilder,
 * setting the positions via the desks.
 * 
 * I'm unsure of these GridPoints having action. Again stop thinking so much and just throw
 * the fucking code at the screen this isn't 'contemplate for an hour and then do nothing' 
 * this is 'try some shit, see what works and doesn't, repeat until finished lol'
 * 
 */

namespace StickClassroom
{
    internal class GridPoint
    {
        private Vector2 Position { get; set; }
        private float WaitTime { get; set; }

        private string Action { get; set; }

        private string PositionString { get; set; }  // a two char string that indicates the position of the gridPoint


        public GridPoint(Vector2 position, float waitTime, string action, string posString)
        {
            Position = position;
            WaitTime = waitTime;
            Action = action;
            PositionString = posString;
        }

        public void Draw(SpriteBatch spriteBatch, Texture2D texture, Color color, int size = 5)
        {
            Rectangle pointRect = new((int)Position.X, (int)Position.Y, size, size);

            spriteBatch.Draw(texture, pointRect, color);
        }
    }
}


// From ChatGPT, modify for yo self
/*
public class LevelGrid
{
    public List<GridPoint> ElementaryGrid { get; private set; }
    public List<GridPoint> MiddleSchoolGrid { get; private set; }
    public List<GridPoint> HighSchoolGrid { get; private set; }

    public LevelGrid()
    {
        // Define positions for each grid type
        ElementaryGrid = new List<GridPoint>
        {
            new GridPoint(new Vector2(100, 100), 1.0f, "LookAround"),
            new GridPoint(new Vector2(150, 100), 2.0f, "CheckStudent"),
            // Add more points as needed
        };

        MiddleSchoolGrid = new List<GridPoint>
        {
            new GridPoint(new Vector2(200, 200), 1.5f, "LookAround"),
            new GridPoint(new Vector2(250, 200), 1.0f, "CheckStudent"),
            // Add more points as needed
        };

        HighSchoolGrid = new List<GridPoint>
        {
            new GridPoint(new Vector2(300, 300), 2.0f, "LookAround"),
            new GridPoint(new Vector2(350, 300), 1.5f, "CheckStudent"),
            // Add more points as needed
        };
    }

    public List<GridPoint> GetGridForLevelType(string levelType)
    {
        return levelType switch
        {
            "Elementary" => ElementaryGrid,
            "MiddleSchool" => MiddleSchoolGrid,
            "HighSchool" => HighSchoolGrid,
            _ => throw new ArgumentException("Invalid level type")
        };
    }
}

public class TeacherPath
{
    public List<GridPoint> Points { get; set; }
    public int CurrentPointIndex { get; private set; }
    private float elapsedTime;

    public TeacherPath(List<GridPoint> points)
    {
        Points = points;
        CurrentPointIndex = 0;
        elapsedTime = 0;
    }

    public GridPoint GetCurrentPoint()
    {
        return Points[CurrentPointIndex];
    }

    public void UpdatePath(float deltaTime)
    {
        elapsedTime += deltaTime;

        if (elapsedTime >= GetCurrentPoint().WaitTime)
        {
            NextPoint();
            elapsedTime = 0;
        }
    }

    private void NextPoint()
    {
        CurrentPointIndex = (CurrentPointIndex + 1) % Points.Count; // Loop back to start if needed
    }
}




*/
