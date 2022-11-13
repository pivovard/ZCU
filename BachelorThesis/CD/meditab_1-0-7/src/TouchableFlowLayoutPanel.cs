using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Drawing;

namespace MediTab
{
    public partial class TouchableFlowLayoutPanel : FlowLayoutPanel
    {
        private bool _doTouchScroll;
        private Point _mouseStartPoint = Point.Empty;
        private Point _PanelStartPoint = Point.Empty;

        public TouchableFlowLayoutPanel()
        {
            InitializeComponent();

            Program.mouseFilter.MouseFilterDown += mouseFilter_MouseFilterDown;
            Program.mouseFilter.MouseFilterMove += mouseFilter_MouseFilterMove;
            Program.mouseFilter.MouseFilterUp += mouseFilter_MouseFilterUp;
        }

        public TouchableFlowLayoutPanel(IContainer container)
        {
            container.Add(this);

            InitializeComponent();

            Program.mouseFilter.MouseFilterDown += mouseFilter_MouseFilterDown;
            Program.mouseFilter.MouseFilterMove += mouseFilter_MouseFilterMove;
            Program.mouseFilter.MouseFilterUp += mouseFilter_MouseFilterUp;
        }

        /// <summary>
        ///     Handles the MouseFilterDown event of the mudmFilter control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">
        ///     The <see cref="MouseFilterEventArgs" /> instance containing the event data.
        /// </param>
        private void mouseFilter_MouseFilterDown(object sender, MouseFilterEventArgs e)
        {
            if (!_doTouchScroll && e.Button == MouseButtons.Left)
            {
                _mouseStartPoint = new Point(e.X, e.Y);
                _PanelStartPoint = new Point(-this.AutoScrollPosition.X,
                                                     -this.AutoScrollPosition.Y);
            }
        }

        /// <summary>
        ///     Handles the MouseFilterMove event of the mudmFilter control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">
        ///     The <see cref="MouseFilterEventArgs" /> instance containing the event data.
        /// </param>
        private void mouseFilter_MouseFilterMove(object sender, MouseFilterEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                if (!_mouseStartPoint.Equals(Point.Empty))
                {
                    int dx = (e.X - _mouseStartPoint.X);
                    int dy = (e.Y - _mouseStartPoint.Y);

                    if (_doTouchScroll)
                    {
                        this.AutoScrollPosition = new Point(_PanelStartPoint.X - dx,
                                                                     _PanelStartPoint.Y - dy);
                    }
                    else if (Math.Abs(dx) > 10 || Math.Abs(dy) > 10)
                    {
                        _doTouchScroll = true;
                    }
                }
            }
        }

        /// <summary>
        ///     Handles the MouseFilterUp event of the mudmFilter control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">
        ///     The <see cref="MouseFilterEventArgs" /> instance containing the event data.
        /// </param>
        private void mouseFilter_MouseFilterUp(object sender, MouseFilterEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                if (_doTouchScroll && !this.AutoScrollPosition.Equals(_PanelStartPoint) &&
                    !_PanelStartPoint.Equals(Point.Empty))
                {
                    // dont fire Click-Event
                    e.Handled = true;
                }
                _doTouchScroll = false;
                _mouseStartPoint = Point.Empty;
                _PanelStartPoint = Point.Empty;
            }
        }
    }
}
