using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Media;
using System.Windows.Shapes;

namespace Hanoi_Towers
{

    public class GameSettings
    {
        /// Хранит параметры и определения цветов
        public class Colors
        {
            public static readonly List<String> RingColors = new List<String> { "#ff409f", "#ff79bb", "#ffb2d8", "#ffd8eb" };
            public static readonly String ErrorColor = "#33E23636";
            public static readonly String SuccessColor = "#3361D161";
            public static readonly String Transparent = "#00000000";
        }

        public static readonly string MessageBoxDoneMessage = "Башня собрана";
        public static readonly string MessageBoxDoneCaption = "Welcome to the limit";

        /// Ширина нижнего кольца
        public static readonly int FirstRingWidth = 150;

        /// Высота колец
        public static readonly int ringHeight = 20;

        /// "Потеря ширины" кольца - половина разницы между шириной соседних колец
        public readonly int ringWidthFall = 5;

        /// Количество колец
        public int ringsCount = 4;

        /// Возвращает сплошную кисть на базе определенного RGB или RGBA цвета в HEX формате
        public static SolidColorBrush GetColorFromRGBA(String hex)
        {
            return (SolidColorBrush)new BrushConverter().ConvertFrom(hex);
        }

        /// Создает копию кольца
        public static Rectangle GetCopy(Rectangle origin)
        {
            Rectangle temp = new Rectangle();
            temp.Height = origin.Height;
            temp.Width = origin.Width;
            temp.Fill = origin.Fill;
            temp.Stroke = origin.Stroke;
            temp.StrokeThickness = origin.StrokeThickness;
            return temp;
        }

    }

    
}
