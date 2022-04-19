using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Windows.Forms;

namespace warships
{
    public partial class GameForm : Form
    {

        /// <summary>
        /// Сюди будуть приходити всі повідомлення від 2 гравця
        /// </summary>
        void WorkerRecieveMessagesEvent(string message)
        {

            if (message == "Закрити")
            {
                MessageBox.Show(string.Format("Гра закінчена. {0} переміг!", Model.CurrentNick));
                ResetGame();
                return;
            }
            var messageItems = message.Split('_');
            if (messageItems.Length == 1 && !Utils.IsNum(messageItems[0]))
            {
                SetEnemyNick(message);
                Worker.SendMessage(string.Format("OK_{0}", Model.CurrentNick));
            }
            if (messageItems.Length == 2 && messageItems[0] == "OK")
            {
                SetEnemyNick(messageItems[1]);
                SetGameStatus(GameStatus.InGameTurn);
            }
            if (messageItems.Length == 2 && Utils.IsNum(messageItems[0]) && Utils.IsNum(messageItems[1]))
            {
                int x = Convert.ToInt32(messageItems[0]) - 1;
                int y = Convert.ToInt32(messageItems[1]) - 1;
                ProcessEnemyTurn(x, y);
                CurrentBackground.Invalidate();
            }
            if (messageItems.Length == 1 && Utils.IsNum(messageItems[0])) 
            {
                TurnsResult status = (TurnsResult)Convert.ToInt32(messageItems[0]);
                var lastCurrentCoords = Model.LastCurrentCoords;
                Model.Enemy[lastCurrentCoords.X][lastCurrentCoords.Y] = status == TurnsResult.Miss ? Cell.Miss : Cell.Dead;

                if (status == TurnsResult.Kill || status == TurnsResult.Hit)
                {
                    SetGameStatus(GameStatus.InGameTurn);
                }
                if (status == TurnsResult.Kill)
                {
                    MarkShipAsDead(Model.Enemy, lastCurrentCoords.X, lastCurrentCoords.Y);
                }

                EnemyBackground.Invalidate();

            }
        }


        /// <summary>
        /// Відправляє противнику статус його ходу та оновлює Model.Current відповідно до цього ходу.
        /// Також встановлює чергу ходити поточному гравцю, якщо супротивник промазав.
        /// </summary>
        void ProcessEnemyTurn(int x, int y)
        {
            Contract.Assert(Model.Status == GameStatus.InGameWait);
            Contract.Assert(Model.Current[x][y] != Cell.Miss && Model.Current[x][y] != Cell.Dead);
            var enemyStatus = CalculateTurnResult(Model.Current, x, y);
            Model.Current[x][y] = enemyStatus == TurnsResult.Miss ? Cell.Miss : Cell.Dead;


            if (enemyStatus == TurnsResult.Kill)
            {
                MarkShipAsDead(Model.Current, x, y);
                if (AllShipsDead(Model.Current))
                {
                    Worker.SendMessage("Закрити");
                    MessageBox.Show(string.Format("Гра закінченна. {0} виграв!", Model.EnemyNick));
                    ResetGame();
                    return;

                }
            }

            Worker.SendMessage(((int)enemyStatus).ToString());
            if (enemyStatus == TurnsResult.Miss)
            {
                SetGameStatus(GameStatus.InGameTurn);
            }

        }

        private bool AllShipsDead(Cell[][] cells)
        {
            return cells.All(row => !row.Any(cell => cell == Cell.Life));
        }

        private TurnsResult CalculateTurnResult(Cell[][] cells, int x, int y)
        {
            if (cells[x][y] == Cell.Empty)
                return TurnsResult.Miss;

            var ship = GetShipCells(cells, x, y);

            bool allIsDead = true;
            foreach (Point cellCoord in ship)
            {

                if (!(cellCoord.X == x && cellCoord.Y == y) && cells[cellCoord.X][cellCoord.Y] != Cell.Dead)
                {
                    allIsDead = false;
                    break;
                }
            }
            if (allIsDead)
                return TurnsResult.Kill;

            return TurnsResult.Hit;

        }

        /// <summary>
        /// Повернути кількість клітин осторонь заданої координатами x,y
        /// сторона визначається усуненням delta
        /// </summary>
        int ShipLineLength(Cell[][] cells, int x, int y, int deltaX, int deltaY)
        {
            int result = 0;
            while (true)
            {
                x += deltaX;
                y += deltaY;
                if (x >= cells.Length || x < 0 || y >= cells[0].Length || y < 0 || cells[x][y] == Cell.Empty || cells[x][y] == Cell.Miss)
                    return result;
                result++;
            }
        }

        /// <summary>
        /// Повернути кількість осередків біля корабля з осередком із зазначеними координатами
        /// </summary>
        List<Point> GetShipCells(Cell[][] allCells, int x, int y)
        {
            Contract.Assert(allCells[x][y] != Cell.Empty && allCells[x][y] != Cell.Miss);

            int rightCount = ShipLineLength(allCells, x, y, 1, 0);
            int leftCount = ShipLineLength(allCells, x, y, -1, 0);
            int topCount = ShipLineLength(allCells, x, y, 0, 1);
            int bottomCount = ShipLineLength(allCells, x, y, 0, -1);

            topCount++;
            rightCount++;

            var result = new List<Point>();

            for (int i = y - bottomCount; i < y + topCount; i++)
            {
                for (int j = x - leftCount; j < x + rightCount; j++)
                {
                    result.Add(new Point(j, i));
                }
            }
            return result;

        }

        void MarkShipAsDead(Cell[][] cells, int x, int y)
        {
            var shipCellCoords = GetShipCells(cells, x, y);

            Action<Cell, int, int> setCellIfPossible = (cellValue, xValue, yValue) =>
            {
                if (xValue >= 0 && xValue < cells.Length && yValue >= 0 && yValue < cells[0].Length)
                    cells[xValue][yValue] = cellValue;
            };
            foreach (var c in shipCellCoords)
            {
                setCellIfPossible(Cell.Miss, c.X + 1, c.Y);
                setCellIfPossible(Cell.Miss, c.X + 1, c.Y + 1);
                setCellIfPossible(Cell.Miss, c.X + 1, c.Y - 1);

                setCellIfPossible(Cell.Miss, c.X, c.Y + 1);
                setCellIfPossible(Cell.Miss, c.X, c.Y - 1);

                setCellIfPossible(Cell.Miss, c.X - 1, c.Y);
                setCellIfPossible(Cell.Miss, c.X - 1, c.Y + 1);
                setCellIfPossible(Cell.Miss, c.X - 1, c.Y - 1);
            }

            foreach (var c in shipCellCoords)
            {
                cells[c.X][c.Y] = Cell.Dead;
            }
        }





        void ResetGame()
        {

            SetGameStatus(GameStatus.Ready);

            for (int i = 0; i < Model.Current.Length; i++)
            {
                for (int j = 0; j < Model.Current[0].Length; j++)
                {
                    Model.Current[i][j] = Cell.Empty;
                    Model.Enemy[i][j] = Cell.Empty;
                }
            }
            Worker.Reset();


            RecountReadyShips(Model.Current, out Model.CurrentShips);
            UpdateShipsLabels(Model.CurrentShips);

            CurrentBackground.Invalidate();
            EnemyBackground.Invalidate();
        }

        void SetGameStatus(GameStatus newStatus)
        {
            if (Model.Status != newStatus)
            {
                Model.Status = newStatus;
                UpdateLabelsByStatus(newStatus);
                if (newStatus == GameStatus.Ready)
                    ResetGame();
            }
        }
    }
}
