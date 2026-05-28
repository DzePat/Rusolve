using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEditor.PackageManager.Requests;

namespace Assets.Scripts.Utility
{
    public class ColorCount
    {
        public int white, green, yellow, blue, red, orange;

        public ColorCount()
        {
            Reset();
        }

        public void Add(string color)
        {
            switch (color)
            {
                case "white": white++; break;
                case "green": green++; break;
                case "yellow": yellow++; break;
                case "blue": blue++; break;
                case "red": red++; break;
                case "orange": orange++; break;
            }
        }

        public void Sub(string color)
        {
            switch (color)
            {
                case "white": white--; break;
                case "green": green--; break;
                case "yellow": yellow--; break;
                case "blue": blue--; break;
                case "red": red--; break;
                case "orange": orange--; break;
            }
        }

        public void Reset()
        {
            this.white = 9;
            this.green = 9;
            this.yellow = 9;
            this.blue = 9;
            this.red = 9;
            this.orange = 9;
        }

        public bool AllColorsEqual()
        {
            if(white != 9) { return false; }
            else if(green != 9) { return false; }
            else if (yellow != 9) {return false; }
            else if(blue != 9) {return false; }
            else if(red != 9) {return false; }
            else if(orange != 9) {return false; }
            return true;
        }
    }
}
