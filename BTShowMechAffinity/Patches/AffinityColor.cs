using BattleTech.UI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace BTShowMechAffinity.Patches
{

    /// <summary>
    /// The color for the mech warrior's deployment count. 
    /// 
    /// </summary>
    public class AffinityColor
    {
        /// <summary>
        /// The color.  Expected to be in HTML format.
        /// </summary>
        public string Color { get; set; }

        /// <summary>
        /// The maximum deployment count for the color, inclusive.
        /// </summary>
        public int DeploymentCountMax { get; set; }

        /// <summary>
        /// The color object for this color
        /// </summary>
        public Color UnityColor { get; set; }


        /// <summary>
        /// Sets the UnityColor from this object.
        /// </summary>
        /// <exception cref="ApplicationException"></exception>
        public void Init()
        {
            if(UnityEngine.ColorUtility.TryParseHtmlString(Color, out UnityEngine.Color ueColor) == false)
            {
                throw new ApplicationException($"Unable to parse HTML color to unity color. Value {Color}");
            }

            UnityColor = ueColor;
        }
    }
}
