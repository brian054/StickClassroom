﻿using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

/*
 * So what I'm thinking is you pass in the Level value to the constructor (enum)
 * then it builds the level based on that.
 * 
 * All the levels in a school type will be the same desk layout, the only difference is where
 * the nerd is.
 * 
 * 
 */

namespace StickClassroom
{
    internal class DeskLayout
    {
        public int DeskRows { get; set; }
        public int DeskCols { get; set; }
        public Desk[,] desks { get; set; }
        private Texture2D DeskTexture;

        public DeskLayout(Texture2D deskTexture, List<Rectangle> collidables, string type) // enum to specify which layout to do, string for now
        {
            this.DeskTexture = deskTexture;
            if (type.Equals("High")) {
                this.DeskRows = 4;
                this.DeskCols = 6;
                this.desks = new Desk[DeskRows, DeskCols];
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

                    // Create and place a new desk at the calculated position
                    desks[i, j] = new Desk(x, y, DeskTexture);

                    // Store in collidables List
                    collidables.Add(desks[i, j].DeskRect); // WHY NULL WTF
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
