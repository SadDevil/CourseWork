using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;


namespace warships
{
    public enum RecountStatus
    {
        Success, MoreThanFourLengthShip
    }

    public partial class GameForm : Form
    {
        //ships logic

        /// <summary>
        /// Метод для підрахунку кількості кораблів у гравця
        /// під час розставляння кораблів перед грою
        /// </summary>
        /// <param name="cells"></param>
        /// <param name="s"></param>
        RecountStatus RecountReadyShips(Cell[][] cells, out Ships s)
        {
            s = new Ships();

            int nextShipLength = 0;

            for (int i = 0; i < cells.Length; i++)
            {
                for (int j = 0; j < cells[0].Length; j++)
                {
                    if (i != 0 && j == 0)
                    {
                        if (nextShipLength > 4)
                            return RecountStatus.MoreThanFourLengthShip;
                        if (nextShipLength > 1)
                            s.LengthCountShipsArray[nextShipLength]--;
                        nextShipLength = 0;
                    }

                    if (cells[i][j] == Cell.Life)
                        nextShipLength++;

                    if ((cells[i][j] != Cell.Life))
                    {
                        if (nextShipLength > 4)
                            return RecountStatus.MoreThanFourLengthShip;
                        if (nextShipLength > 1)
                            s.LengthCountShipsArray[nextShipLength]--;
                        nextShipLength = 0;
                    }
                }


            }
            if (nextShipLength > 1)
                s.LengthCountShipsArray[nextShipLength]--;
            nextShipLength = 0;
            for (int i = 0; i < cells.Length; i++)
            {

                for (int j = 0; j < cells[0].Length; j++)
                {
                    if (i != 0 && j == 0)
                    {
                        if (nextShipLength > 4)
                            return RecountStatus.MoreThanFourLengthShip;
                        if (nextShipLength > 1)
                            s.LengthCountShipsArray[nextShipLength]--;
                        nextShipLength = 0;
                    }

                    if (cells[j][i] == Cell.Life)
                        nextShipLength++;

                    if (cells[j][i] != Cell.Life)
                    {
                        if (nextShipLength > 4)
                            return RecountStatus.MoreThanFourLengthShip;
                        if (nextShipLength > 1)
                            s.LengthCountShipsArray[nextShipLength]--;
                        nextShipLength = 0;
                    }
                }

            }
            if (nextShipLength > 1)
                s.LengthCountShipsArray[nextShipLength]--;

            for (int i = 0; i < cells.Length; i++)
            {
                for (int j = 0; j < cells[0].Length; j++)
                {
                    if (cells[i][j] == Cell.Life && GetShipCells(cells, i, j).Count == 1)
                        s.LengthCountShipsArray[1]--;
                }
            }
            return RecountStatus.Success;

        }


        /// <summary>
        /// Метод для встановлення кораблів під час підготовки до гри
        /// встановлює осередок і повертає true якщо за діагоналями в сусідніх клітинах
        /// немає кораблів і розмір корабля, що створюється не більше 4
        /// </summary>
        private bool TrySetupCell(Cell[][] cells, int x, int y)
        {
            cells[x][y] = Cell.Life;
            Ships outShip;
            bool noLongShips = RecountReadyShips(cells, out outShip) != RecountStatus.MoreThanFourLengthShip;
            bool rightTopEmpty = x == cells.Length - 1 || y == cells[0].Length - 1 || cells[x + 1][y + 1] == Cell.Empty;
            bool leftTopEmpty = x == cells.Length - 1 || y == 0 || cells[x + 1][y - 1] == Cell.Empty;
            bool rightBottomEmpty = x == 0 || y == cells[0].Length - 1 || cells[x - 1][y + 1] == Cell.Empty;
            bool leftBottomEmpty = x == 0 || y == 0 || cells[x - 1][y - 1] == Cell.Empty;
            if (noLongShips && rightBottomEmpty && leftBottomEmpty && rightTopEmpty && leftTopEmpty)
            {
                return true;
            }
            cells[x][y] = Cell.Empty;
            return false;
        }


    }
}
