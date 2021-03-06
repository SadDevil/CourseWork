using System;
using System.Drawing;
using System.Net.Sockets;
using System.Windows.Forms;

namespace warships
{
    public partial class GameForm : Form
    {
        private const int SizeWidth = 25;
        private const int SizeHeight = 25;
        public readonly TcpWorker Worker = new TcpWorker();

        public GameForm()
        {
            InitializeComponent();

            #region events bound

            CurrentBackground.Paint += CurrentBackgroundPaint;
            EnemyBackground.Paint += EnemyBackgroundPaint;
            CurrentBackground.MouseClick += CurrentBackgroundMouseClick;
            EnemyBackground.MouseClick += EnemyBackgroundMouseClick;
            Worker.RecieveMessagesEvent += WorkerRecieveMessagesEvent;
            this.Closing += GameFormClosing;
            #endregion

            ResetGame();

        }

        void GameFormClosing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            if (Model.Status == GameStatus.InGameTurn || Model.Status == GameStatus.InGameWait)
            {
                Worker.SendMessage("Закрити");
            }

        }

        private void EnemyBackgroundMouseClick(object sender, MouseEventArgs e)
        {
            Point coord = Get2DByGround(new Point(e.X, e.Y));
            if (Model.Enemy[coord.X][coord.Y] == Cell.Empty &&
                Model.Status == GameStatus.InGameTurn)
            {
                SetGameStatus(GameStatus.InGameWait);
                Model.LastCurrentCoords = new Point(coord.X, coord.Y);
                Worker.SendMessage(string.Format("{0}_{1}", coord.X + 1, coord.Y + 1));
            }
        }

        private void CurrentBackgroundMouseClick(object sender, MouseEventArgs e)
        {
            if (Model.Status == GameStatus.Ready)
            {
                Point coord = Get2DByGround(new Point(e.X, e.Y));

                if (e.Button == MouseButtons.Left)
                {
                    TrySetupCell(Model.Current, coord.X, coord.Y);
                }
                if (e.Button == MouseButtons.Right)
                    Model.Current[coord.X][coord.Y] = Cell.Empty;

                RecountReadyShips(Model.Current, out Model.CurrentShips);
                UpdateShipsLabels(Model.CurrentShips);
                CurrentBackground.Invalidate();
            }
        }


        private void UpdateShipsLabels(Ships ships)
        {
            CurrentShip1Label.Text = ships.LengthCountShipsArray[4].ToString();
            CurrentShip2Label.Text = ships.LengthCountShipsArray[3].ToString();
            CurrentShip3Label.Text = ships.LengthCountShipsArray[2].ToString();
            CurrentShip4Label.Text = ships.LengthCountShipsArray[1].ToString();
        }

        protected Point Get2DByGround(Point backgroundPoint)
        {
            int x = backgroundPoint.X / SizeWidth;
            int y = backgroundPoint.Y / SizeHeight;
            return new Point(x, y);
        }

        private void EnemyBackgroundPaint(object sender, PaintEventArgs e)
        {
            PaintGround(Model.Enemy, e);
        }

        private void CurrentBackgroundPaint(object sender, PaintEventArgs e)
        {
            PaintGround(Model.Current, e);
        }

        private void PaintGround(Cell[][] cells, PaintEventArgs e)
        {
            for (int i = 0; i < cells.Length; i++)
            {
                for (int j = 0; j < cells[0].Length; j++)
                {
                    Graphics g = e.Graphics;
                    var coord = new Point(i, j);
                    coord.X *= SizeWidth;
                    coord.Y *= SizeHeight;

                    var rect = new Rectangle(coord, new Size(SizeWidth, SizeHeight));
                    g.DrawRectangle(Pens.Black, rect);

                    Cell cell = cells[i][j];
                    switch (cell)
                    {
                        case Cell.Dead:
                            coord = new Point(coord.X + SizeWidth / 2, coord.Y + SizeHeight / 2);
                            var tl = new Point(coord.X - SizeWidth / 2, coord.Y - SizeHeight / 2);
                            var tr = new Point(coord.X - SizeWidth / 2, coord.Y + SizeHeight / 2);
                            var bl = new Point(coord.X + SizeWidth / 2, coord.Y - SizeHeight / 2);
                            var br = new Point(coord.X + SizeWidth / 2, coord.Y + SizeHeight / 2);
                            g.DrawLine(new Pen(Color.Brown, 2), tl, br);
                            g.DrawLine(new Pen(Color.Brown, 2), tr, bl);
                            break;
                        case Cell.Life:
                            var lifeRect = new Rectangle(new Point(coord.X + 2, coord.Y + 2),
                                                         new Size(SizeWidth - 3, SizeHeight - 3));
                            g.FillRectangle(new SolidBrush(Color.CornflowerBlue), lifeRect);
                            break;
                        case Cell.Miss:
                            g.DrawEllipse(new Pen(Color.Black, 2), coord.X + SizeWidth / 2, coord.Y + SizeHeight / 2, 2, 2);
                            break;
                    }
                }
            }
        }

        /// <summary>
        /// пункт меню Create
        /// </summary>
        private void CreateToolStripMenuItemClick(object sender, EventArgs e)
        {
            if (Model.CurrentShips.Filled() && Model.Status == GameStatus.Ready)
            {
                var createForm = new CreateGameForm(GameJoinHandler, GameCreateHandler);
                createForm.ShowDialog();
            }
            else MessageBox.Show("Потрібно виставити кораблі");
        }

        /// <summary>
        /// пункт меню Reset
        /// </summary>
        private void ResetToolStripMenuItemClick(object sender, EventArgs e)
        {
            if (Model.Status == GameStatus.InGameTurn || Model.Status == GameStatus.InGameWait)
            {
                Worker.SendMessage("Закрити");
            }
            ResetGame();
        }

        private void UpdateLabelsByStatus(GameStatus newStatus)
        {

            if (this.InvokeRequired)
                this.Invoke(new Action<GameStatus>(UpdateLabelsByStatus), new object[] { newStatus });
            else
            {
                bool readyLabelsVisible = newStatus == GameStatus.Ready;
                CurrentInfo.Visible = readyLabelsVisible;

                CurrentShip1Label.Visible = readyLabelsVisible;
                CurrentShip2Label.Visible = readyLabelsVisible;
                CurrentShip3Label.Visible = readyLabelsVisible;
                CurrentShip4Label.Visible = readyLabelsVisible;
                CurrentShip1Info.Visible = readyLabelsVisible;
                CurrentShip2Info.Visible = readyLabelsVisible;
                CurrentShip3Info.Visible = readyLabelsVisible;
                CurrentShip4Info.Visible = readyLabelsVisible;

                EnemyNickLabel.Visible = !readyLabelsVisible;
                CurrentNickLabel.Visible = !readyLabelsVisible;

                switch (newStatus)
                {
                    case GameStatus.InGameTurn:
                        TurnAlertLabel.Text = "твоя черга";
                        break;
                    case GameStatus.InGameWait:
                        TurnAlertLabel.Text = "черга супротивника";
                        break;
                    default:
                        TurnAlertLabel.Text = "Готуємось до гри";
                        break;
                }
            }

        }

        void SetCurrentNick(string nick)
        {
            Model.CurrentNick = nick;
            if (this.InvokeRequired)
                this.Invoke(new Action<string>(SetCurrentNick), new object[] { nick });
            else
                CurrentNickLabel.Text = nick;
        }

        void SetEnemyNick(string nick)
        {
            Model.EnemyNick = nick;
            if (this.InvokeRequired)
                this.Invoke(new Action<string>(SetEnemyNick), new object[] { nick });
            else
                EnemyNickLabel.Text = nick;
        }

        /// <summary>
        /// створення гри користувачем
        /// </summary>
        private void GameCreateHandler(string nickname)
        {
            try
            {
                Worker.Listen(3000);

                SetGameStatus(GameStatus.InGameWait);
                SetCurrentNick(nickname);
            }
            catch (SocketException)
            {
                MessageBox.Show("Порт вже прослуховується");
            }
        }

        /// <summary>
        /// Приєднання до гри
        /// </summary>
        private void GameJoinHandler(string ip, string nickname)
        {
            Worker.Join(ip, 3000);

            Worker.SendMessage(nickname);

            SetGameStatus(GameStatus.InGameWait);
            SetCurrentNick(nickname);
        }

    }
}
