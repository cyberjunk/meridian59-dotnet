using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;

using Meridian59.Drawing2D;

namespace Meridian59.BgfEditor.Controls
{
    public class ComboBoxPalette : ComboBox
    {
        public ComboBoxPalette()
            : base()
        {
            DropDownStyle = ComboBoxStyle.DropDownList;

            string prefix;
            for (int i = 0; i < ColorTransformation.Palettes.Length; i++)
            {
                if (i < 10)
                    prefix = "00";
                else if (i < 100)
                    prefix = "0";
                else 
                    prefix = String.Empty;

                Items.Add("(" + prefix + i.ToString() + ") - " + ColorTransformation.GetNameOfPalette((byte)i));
            }

            // add the special legacy vale palette last
            Items.Add("(999) - VALE OF SORROW)");

            SelectedIndex = 0;
        }
    }
}
