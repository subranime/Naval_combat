using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Naval_combat.Entities;

namespace Naval_combat.GameLogic
{
    internal class GameEngine
    {
        private readonly List<Brush> enemyShipBrushes;
        private readonly PictureBox pictureBox;
        private Bitmap bufferBitmap;
        private readonly object lockObject = new object();
        public GameEngine(PictureBox pictureBox)
        {
            this.pictureBox = pictureBox;
            enemyShipBrushes = new List<Brush>
            {
                Brushes.Red,
                Brushes.Blue,
                Brushes.Green,
                Brushes.Yellow,
                Brushes.Orange,
                Brushes.Purple,
                Brushes.Pink,
                Brushes.Brown,
                Brushes.Cyan,
                Brushes.Magenta
            };

            // Создаем буферное изображение
            bufferBitmap = new Bitmap(pictureBox.Width, pictureBox.Height);

            // Рисуем игрока
            using (Graphics g = Graphics.FromImage(bufferBitmap))
            {
                Image submarineImage = Properties.Resources.sub50x50;
                g.DrawImage(submarineImage, new Point(300, 450));
            }

            // Отображаем буферное изображение в PictureBox
            pictureBox.Image = bufferBitmap;
        }

        public void UpdateUI(Game game)
        {
            lock (lockObject)
            {
                // Создаем Graphics для буферного изображения
                using (Graphics g = Graphics.FromImage(bufferBitmap))
                {
                    // Очищаем предыдущие рисунки на буферном изображении
                    g.Clear(Color.Transparent);

                    // Пример: рисуем вражеские корабли с разными цветами
                    for (int i = 0; i < game.Field.EnemyShips.Count; i++)
                    {
                        EnemyShip enemyShip = game.Field.EnemyShips[i];
                        Rectangle enemyShipRect = new Rectangle(enemyShip.Coordinates.X, enemyShip.Coordinates.Y, enemyShip.Width, enemyShip.Height);

                        // Используем кисть с соответствующим индексом
                        Brush enemyShipBrush = enemyShipBrushes[i % enemyShipBrushes.Count];
                        g.FillRectangle(enemyShipBrush, enemyShipRect);
                    }
                }

                // Перерисовываем PictureBox с обновленным буферным изображением
                pictureBox.Invalidate();
            }
        }
    }
}
