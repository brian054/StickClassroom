using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

/*
 * So what I'm thinking is you pass in the Level value to the constructor (enum)
 * then it builds the level based on that.
 * 
 * All the levels in a school type will be the same desk layout, the only differences are 
 *  - default nerd position
 *  
 *  
 *  Also, I'm thinking of puting the GridPoints layout in here too, so this would become more of a Level builder class 
 *  so refactor later once you get grid points in here. Idk should we separate it out???? NOOOOO cuz you have to build the 
 *  layout based on the desks positioning, so just build the teacher grid in here.
 *  But then what about the teacher interacting with this environment???  
 *
 */

namespace StickClassroom
{
    internal class DeskLayout
    {
        private int DeskRows { get; set; }
        private int DeskCols { get; set; }
        private Desk[,] desks { get; set; }
        private List<GridPoint> GridPoints { get; set; }

        private Texture2D DeskTexture;

        public DeskLayout(Texture2D deskTexture, List<Rectangle> collidables, string type) // enum to specify which layout to do, string for now
        {
            this.DeskTexture = deskTexture;
            if (type.Equals("High")) {
                this.DeskRows = 4;
                this.DeskCols = 6;
                this.desks = new Desk[DeskRows, DeskCols];
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
            else if (type.Equals("Elem"))
            {
                this.DeskRows = 4;
                this.DeskCols = 4;
                this.desks = new Desk[DeskRows, DeskCols];
                this.PrimarySchoolLayout(collidables);
            }
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

            GridPoints.ForEach(entity => entity.Draw(spriteBatch, DeskTexture, Color.Orange, 5));
        }

        private void HighSchoolLayout(List<Rectangle> collidables)
        {
            for (int i = 0; i < DeskRows; i++)
            {
                for (int j = 0; j < DeskCols; j++)
                {
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

                    // Create GridPoint instances around each desk
                    // Right 
                    GridPoints.Add(new GridPoint(new Vector2(x + deskWidth + 5 + deskWidth / 2, y + deskHeight / 2), 1.0f, "move"));
                    // Left
                    if (i == 0) {
                        GridPoints.Add(new GridPoint(new Vector2(x - deskWidth / 2, y + deskHeight / 2), 1.0f, "move"));
                    }
                    // Front
                    if (j == 0) {
                        GridPoints.Add(new GridPoint(new Vector2(x + deskWidth / 2, y - deskHeight - 10), 0.5f, "turn"));
                    }
                    // Behind
                    GridPoints.Add(new GridPoint(new Vector2(x + deskWidth / 2, y + deskHeight + deskHeight + 10), 0.5f, "turn"));

                }
            }
        }

        // really just a four corners thing (BL = Teacher desk, all other corners = Student desks (4 students each)

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
