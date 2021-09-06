﻿using System.Collections.Generic;

namespace GameX.Game.Types
{
    public class Character
    {
        public string Name { get; set; }
        public int Value { get; set; }

        public List<Costume> Costumes { get; set; }

        public override string ToString()
        {
            return Name;
        }
    }

    public class Costume
    {
        public string Name { get; set; }
        public string Portrait { get; set; }
        public string File { get; set; }
        public int Value { get; set; }

        public override string ToString()
        {
            return Name;
        }
    }
}