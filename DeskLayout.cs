using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

/*
 * How to get better: write more code lol
 * What is it Mark Zuckerberg said "Build fast, break fast, fail fast"
 */

namespace StickClassroom
{
    internal class DeskLayout
    {
        private int DeskRows { get; set; }
        private int DeskCols { get; set; }
        private Desk[,] desks { get; set; }
        private List<GridPoint> GridPoints { get; set; } // maybe turn this into a Dictionary, yup the key is the PosString and the GridPoint obj is the value
        //private Dictionary<string, GridPoint> GridPointDictionary { get; set; }

        private Texture2D DeskTexture;

        public DeskLayout(Texture2D deskTexture, List<Rectangle> collidables, string type) // enum to specify which layout to do, string for now
        {
            this.DeskTexture = deskTexture;
            if (type.Equals("High"))
            {
                this.DeskRows = 4;
                this.DeskCols = 6;
                this.desks = new Desk[DeskRows, DeskCols];
                //this.GridPointDictionary = new Dictionary<string, GridPoint>();
                this.GridPoints = new List<GridPoint>();
                this.HighSchoolLayout(collidables);
            }
            //else if (type.Equals("Middle"))
            //{
            //    this.DeskRows = 4;
            //    this.DeskCols = 6;
            //    this.desks = new Desk[DeskRows, DeskCols];
            //    this.HighSchoolLayout(collidables);
            //}
            //else if (type.Equals("Elem"))
            //{
            //    this.DeskRows = 4;
            //    this.DeskCols = 4;
            //    this.desks = new Desk[DeskRows, DeskCols];
            //    this.PrimarySchoolLayout(collidables);
            //}
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            for (int i = 0; i < DeskRows; i++)
            {
                for (int j = 0; j < DeskCols; j++)
                {
                    desks[i, j].Draw(spriteBatch);
                }
            }

            GridPoints.ForEach(entity => entity.Draw(spriteBatch, DeskTexture, Color.Orange, 5)); //hmmm
        }

        private void HighSchoolLayout(List<Rectangle> collidables)
        {
            for (int i = 0; i < DeskRows; i++)
            {
                for (int j = 0; j < DeskCols; j++)
                {
                    // --------------
                    // Desk Creation
                    // --------------
                    // Calculate desk positions
                    int x = i * 180 + 80; // 158
                    int y = j * 120 + 170;

                    // Create and set a new desk at the calculated position
                    desks[i, j] = new Desk(x, y, DeskTexture);

                    // Store in collidables List
                    collidables.Add(desks[i, j].DeskRect);

                    // Calculate points around the desk for navigation
                    int deskWidth = desks[i, j].DeskRect.Width;
                    int deskHeight = desks[i, j].DeskRect.Height;

                    // --------------
                    // Grid Creation
                    //
                    // Problem: We can't do 00 and 01 coordinates 
                    //
                    //
                    // MA = MiddleAisle
                    // 
                    //
                    // --------------

                    /*
                     * So I'm kinda rethinking this...maybe the desk class should generate the points around it. 
                     * Yep doing that instead much much easier!
                     * Remember you will rewrite all the mf time!! Shit is fun af
                     *
                     * Tuesday = rewrite and generate GridPoints in Desk class, 
                     * Then we can access the desk's position in the level as "A1" so first column, first row or something and then to reference that gridpoint
                     * it'd be "TL" or "BottomLeft (BL)" etc.
                     */


                    // Create GridPoint instances around each desk
                    if (i == 0)
                    {
                        // Column 0, so that whole left side

                        // To the left of each desk in first column (so labels = "00, 02, 04, 06, etc.)
                        //GridPoints.Add(new GridPoint(new Vector2(x - deskWidth / 2, y + deskHeight / 2), 1.0f, "move", "00"));

                        // Bottom left of each desk in column 0 (labels = "01, 03, etc")
                        //GridPoints.Add(new GridPoint(new Vector2(x - deskWidth / 2, y + deskHeight + deskHeight + 10), 1.0f, "move", "00"));
                    }
                    else
                    {
                        // Bottom right, or bottom left. In the middle of the aisles.
                        GridPoints.Add(new GridPoint(new Vector2(x - deskWidth / 2 - 15, y + (deskHeight / 2)), 1.0f, "move", "00"));

                        // Mid right
                        //GridPoints.Add(new GridPoint(new Vector2(x - deskWidth / 2 - 15, y + deskHeight + deskHeight + 10), 1.0f, "move", "00"));
                    }
                    if (j == 0)
                    {
                        // Row 0, so in front of the first row of desks
                        GridPoints.Add(new GridPoint(new Vector2(x + deskWidth / 2, y - deskHeight - 10), 0.5f, "turn", "00"));
                        GridPoints.Add(new GridPoint(new Vector2(x + deskWidth + (deskWidth / 2), y - deskHeight - 10), 0.5f, "turn", "00"));
                    }
                    // Behind
                    GridPoints.Add(new GridPoint(new Vector2(x + deskWidth / 2, y + deskHeight + deskHeight + 10), 0.5f, "turn", "00"));

                    // -----------------------------------------
                    // Left side (if not the first column)
                    //if (i > 0)
                    //{
                    //    Vector2 leftPosition = new Vector2(x - deskWidth / 2 - 15, y + deskHeight / 2);
                    //    string positionString = $"{i}{j}_Left";
                    //    GridPoints.Add(new GridPoint(leftPosition, 1.0f, "move", positionString));
                    //}

                    //// Right side (if not the last column)
                    //if (i < DeskRows - 1)
                    //{
                    //    Vector2 rightPosition = new Vector2(x + deskWidth / 2 + 15, y + deskHeight / 2);
                    //    string positionString = $"{i}{j}_Right";
                    //    GridPoints.Add(new GridPoint(rightPosition, 1.0f, "move", positionString));
                    //}

                    //// Front (if not the first row)
                    //if (j > 0)
                    //{
                    //    Vector2 frontPosition = new Vector2(x, y - deskHeight - 10);
                    //    string positionString = $"{i}{j}_Front";
                    //    GridPoints.Add(new GridPoint(frontPosition, 0.5f, "turn", positionString));
                    //}

                    //// Behind (always add)
                    //Vector2 behindPosition = new Vector2(x, y + deskHeight + deskHeight + 10);
                    //string positionStringBehind = $"{i}{j}_Behind";
                    //GridPoints.Add(new GridPoint(behindPosition, 0.5f, "turn", positionStringBehind));
                }
            }
        }

        // really just a four corners thing (BL = Teacher desk, all other corners = Student desks (4 students each)

        // idea: those long tables like in middle school

        //private void MiddleSchoolLayout(List<Rectangle> collidables)
        //{
        //    for (int i = 0; i < DeskRows; i++)
        //    {
        //        for (int j = 0; j < DeskCols; j++)
        //        {
        //            // Calculate desk positions
        //            int x = i * 158 + 80;
        //            int y = j * 120 + 170;

        //            // Create and place a new desk at the calculated position
        //            desks[i, j] = new Desk(x, y, DeskTexture);

        //            // Store in collidables List
        //            collidables.Add(desks[i, j].DeskRect); // WHY NULL WTF
        //        }
        //    }
        //}

        // fine for now
        private void PrimarySchoolLayout(List<Rectangle> collidables)
        {
            for (int i = 0; i < DeskRows; i++)
            {
                for (int j = 0; j < DeskCols; j++)
                {
                    // Calculate desk positions
                    int x = i * 180 + 80;
                    int y = j * 100 + 200;

                    // Create and place a new desk at the calculated position
                    desks[i, j] = new Desk(x, y, DeskTexture);

                    // Store in collidables List
                    collidables.Add(desks[i, j].DeskRect); // WHY NULL WTF
                }
            }
        }

    }
}
