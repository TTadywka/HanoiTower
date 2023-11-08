using SourceChord.FluentWPF;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;

namespace Hanoi_Towers
{
    /// <summary>
    /// Логика взаимодействия для AutomaticGame.xaml
    /// </summary>
    public partial class AutomaticGame
    {
        /// <summary>
        /// Настройки игры
        /// </summary>
        GameSettings Settings;
        /// <summary>
        /// Стандартная скорость анимации (мс). Обновится при перемещении слайдера
        /// </summary>
        int RingMoveTime = 500;
        /// <summary>
        /// Хранит набор перемещений, необходимых для решения головоломки
        /// </summary>
        List<Tuple<int, int>> Movements = new List<Tuple<int, int>>();
        /// <summary>
        /// Индикатор завершения игры, для очистки поля при нажатии кнопки "Старт"
        /// </summary>
        bool GameFinished = false;

        public void InitField()
        {
            //Очистка поля
            column0.Children.Clear();
            column1.Children.Clear();
            column2.Children.Clear();
            //Создание и установка колец на первый колышек
            int RingWidth = GameSettings.FirstRingWidth;
            for (int i = 0; i < Settings.ringsCount; i++)
            {
                Rectangle rect = new Rectangle();
                rect.Width = RingWidth;
                rect.Height = GameSettings.ringHeight;

                Canvas.SetBottom(rect, column0.Children.Count * GameSettings.ringHeight);
                Canvas.SetLeft(rect, 100 - RingWidth / 2);

                rect.Fill = GameSettings.GetColorFromRGBA(GameSettings.Colors.RingColors[i]);
                rect.Stroke = GameSettings.GetColorFromRGBA(GameSettings.Colors.RingColors[i]);
                rect.StrokeThickness = 1;

                column0.Children.Add(rect);
                RingWidth -= Settings.ringWidthFall * 2;
            }
        }

        public AutomaticGame(GameSettings gameSettings)
        {
            InitializeComponent();
            Settings = gameSettings;
            InitField();
        }

        public void MoveRing(int from, int to)
        {
            try
            {
                //Определение исходного колышка и колышка назначения
                Canvas fromColumn = new Canvas();
                Canvas toColumn = new Canvas();

                switch (from)
                {
                    case 0:
                        fromColumn = column0;
                        break;
                    case 1:
                        fromColumn = column1;
                        break;
                    case 2:
                        fromColumn = column2;
                        break;
                    default:
                        break;
                }
                switch (to)
                {
                    case 0:
                        toColumn = column0;
                        break;
                    case 1:
                        toColumn = column1;
                        break;
                    case 2:
                        toColumn = column2;
                        break;
                    default:
                        break;
                }

                if (fromColumn.Children.Count == 0)
                {
                    return;
                }   
                //Подъем верхнего колышка исходного столбика
                Rectangle rect = (Rectangle)fromColumn.Children[fromColumn.Children.Count - 1];

                //Подсчет абслютных координат изначального расположения кольца для анимации перемещения
                Point CalculatedOrigin = new Point((int)Canvas.GetLeft(rect) + (int)Canvas.GetLeft(fromColumn), (int)Canvas.GetBottom(rect) + (int)Canvas.GetBottom(fromColumn));

                //Удаление кольца с колышка
                fromColumn.Children.Remove(rect);
                Canvas.SetBottom(rect, toColumn.Children.Count * GameSettings.ringHeight);

                //Подсчет абсолютных координат конечного расположения кольца для анимации перемещения
                Point CalculatedDestination = new Point((int)Canvas.GetLeft(rect) + (int)Canvas.GetLeft(toColumn), (int)Canvas.GetBottom(rect) + (int)Canvas.GetBottom(toColumn));

                //Запуск анимации перемещения кольца
                AnimateRingMovement(rect, CalculatedOrigin, CalculatedDestination);

                

                //Добавление кольца на новый колышек
                toColumn.Children.Add(rect);
            }
            catch(Exception err)
            {
                Debug.WriteLine(err.Message);
            }
        }

        private void AnimateRingMovement(Rectangle origin, Point CalculatedOrigin, Point CalculatedDestination)
        {
            //Создание виртуального кольца на основе верхнего кольца исходного колышка
            Rectangle tempRect = GameSettings.GetCopy(origin);
            //Установка стартового расположения
            Canvas.SetLeft(tempRect, CalculatedOrigin.X);
            Canvas.SetBottom(tempRect, CalculatedOrigin.Y);
            //Добавление анимационного кольца на глобальный канвас
            gameField.Children.Add(tempRect);

            //Перенос сохранение параметров кольца для создания эффекта "будущего расположения"
            Brush originalColor = origin.Fill;
            origin.Fill = GameSettings.GetColorFromRGBA(GameSettings.Colors.Transparent);

            //Параметры анимации по X
            DoubleAnimation animx = new DoubleAnimation();
            animx.From = CalculatedOrigin.X;
            animx.To = CalculatedDestination.X;
            animx.Duration = TimeSpan.FromMilliseconds(RingMoveTime);
            animx.Completed += (sender, e) => RingMoveCompleted(sender, e, origin, tempRect, originalColor);
            //Параметры анимации по Y
            DoubleAnimation animy = new DoubleAnimation();
            animy.From = CalculatedOrigin.Y;
            animy.To = CalculatedDestination.Y;
            animy.Duration = TimeSpan.FromMilliseconds(RingMoveTime);

            //Запуск анимации
            tempRect.BeginAnimation(Canvas.LeftProperty, animx);
            tempRect.BeginAnimation(Canvas.BottomProperty, animy);
        }
        /// <summary>
        /// Событие - завершение анимации перемещения кольца. Реальному кольцу на новом расположении возвращается его цвет
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        /// <param name="rect"></param>
        /// <param name="that"></param>
        /// <param name="fill"></param>
        private void RingMoveCompleted(object sender, EventArgs e, Rectangle rect, Rectangle that, Brush fill)
        {
            gameField.Children.Remove(that);
            rect.Fill = fill;
        }
        /// <summary>
        /// Непосредственно алгоритм решения задачи
        /// </summary>
        /// <param name="n"></param>
        /// <param name="from_rod"></param>
        /// <param name="to_rod"></param>
        /// <param name="aux_rod"></param>
        private void SolutionHanoibns(int n, int from_rod, int to_rod, int aux_rod)
        {
            try
            {
                if (n > 0)
                {
                    SolutionHanoibns(n - 1, from_rod, aux_rod, to_rod);
                    Movements.Add(new Tuple<int, int>(from_rod, to_rod));
                    SolutionHanoibns(n - 1, aux_rod, to_rod, from_rod);
                }
                           
            }catch(Exception err)
            {
                Debug.WriteLine(err.Message);
            }
            
        }
        /// <summary>
        /// Блокировка кнопкок при старте игры
        /// </summary>
        private void GameStart()
        {
            startBtn.IsEnabled = false;
            clearBtn.IsEnabled = false;
            GameFinished = false;
        }
        /// <summary>
        /// Снятие блокировки после окончания игры
        /// </summary>
        private void GameFinish()
        {
            startBtn.IsEnabled = true;
            clearBtn.IsEnabled = true;
            GameFinished = true;
            Movements.Clear();
            MessageBox.Show(this, GameSettings.MessageBoxDoneMessage, GameSettings.MessageBoxDoneCaption);

        }
        /// <summary>
        /// Обработчик нажатия кнопки старта игры. 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private async void startBtn_Click(object sender, RoutedEventArgs e)
        {
            if (GameFinished)
            {
                InitField();
            }
            GameStart();
            //Вычисление перемещений колец
            SolutionHanoibns(Settings.ringsCount, 0, 1, 2);
            foreach(Tuple<int, int> move in Movements)
            {
                MoveRing(move.Item1, move.Item2);
                await Task.Delay(RingMoveTime);
            }
            GameFinish();
        }

        private void Slider_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            RingMoveTime = (int)(speedSlider.Maximum + speedSlider.Minimum) - (int)speedSlider.Value;
        }

        private void ClearBtn_Click(object sender, RoutedEventArgs e)
        {
            InitField();
        }
    }
}
