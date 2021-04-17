using GameX.Types;
using System;
using System.Collections.Generic;

namespace GameX.Game.Content
{
    public class Variables
    {
        public static ListItem[] Handness()
        {
            return new ListItem[]
            {
                new ListItem("Default"),
                new ListItem("Right-Handed", 0),
                new ListItem("Left-Handed", 1)
            };
        }

        public static ListItem[] WeaponMode()
        {
            return new ListItem[]
            {
                new ListItem("Default"),
                new ListItem("Male", 0),
                new ListItem("Female", 1)
            };
        }
    }
}
