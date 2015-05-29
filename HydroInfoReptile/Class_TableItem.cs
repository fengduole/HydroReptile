using System;
using System.Text;

namespace HydroInfoReptile
{
    public class Class_TableItem
    {
        public int colorState;
        public string[] subitem = new string[11];
        public int _index;

        public Class_TableItem(int index)
        {
            _index = index;
            colorState = 1;
        }
    }
}