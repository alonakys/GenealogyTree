using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace GenealogyTree
{
    public partial class Visualization : Form
    {
        private const int NODE_HEIGHT = 80;
        private const int NODE_WIDTH = 120;
        private const int NODE_MARGIN_X = 50;
        private const int NODE_MARGIN_Y = 40;
        private static Pen NODE_PEN = Pens.Gray;
        public List<SampleDataModel> _data;
        public TreeNodeModel<SampleDataModel> _tree;
        public Visualization(List<SampleDataModel> sampleTree)
        {
            InitializeComponent();
                _data = sampleTree;
                _tree = GetSampleTree(_data);
                TreeHelpers<SampleDataModel>.CalculateNodePositions(_tree);
                CalculateControlSize();
                DoubleBuffered = true;
                treePanel.Paint += treePanel_Paint;
        }
        public TreeNodeModel<SampleDataModel> GetSampleTree(List<SampleDataModel> data)
        {
            var root = data.FirstOrDefault(p => p.ParentId == string.Empty);
            var rootTreeNode = new TreeNodeModel<SampleDataModel>(root, null);

            rootTreeNode.Children = GetChildNodes(data, rootTreeNode);

            return rootTreeNode;
        }
        private static List<TreeNodeModel<SampleDataModel>> GetChildNodes(List<SampleDataModel> data, TreeNodeModel<SampleDataModel> parent)
        {
            var nodes = new List<TreeNodeModel<SampleDataModel>>();

            foreach (var item in data.Where(p => p.ParentId == parent.Item.Id))
            {
                var treeNode = new TreeNodeModel<SampleDataModel>(item, parent);
                treeNode.Children = GetChildNodes(data, treeNode);
                nodes.Add(treeNode);
            }

            return nodes;
        }
        private void CalculateControlSize()
        {
            // tree sizes are 0-based, so add 1
            var treeWidth = _tree.Width + 1;
            var treeHeight = _tree.Height + 1;


            treePanel.Size = new Size(
                Convert.ToInt32((treeWidth * NODE_WIDTH) + ((treeWidth + 1) * NODE_MARGIN_X)),
                (treeHeight * NODE_HEIGHT) + ((treeHeight + 1) * NODE_MARGIN_Y));
        }
        private void treePanel_Paint(object sender, PaintEventArgs e)
        {
            e.Graphics.Clear(Color.White);
            DrawNode(_tree, e.Graphics);
        }
        private void DrawNode(TreeNodeModel<SampleDataModel> node, Graphics g)
        {
            // rectangle where node will be positioned
            var nodeRect = new Rectangle(
                Convert.ToInt32(NODE_MARGIN_X + (node.X * (NODE_WIDTH + NODE_MARGIN_X))),
                NODE_MARGIN_Y + (node.Y * (NODE_HEIGHT + NODE_MARGIN_Y))
                , NODE_WIDTH, NODE_HEIGHT);

            // draw box
            g.DrawRectangle(NODE_PEN, nodeRect);

            // draw content
            g.DrawString(node.ToString(), this.Font, Brushes.Black, nodeRect.X + 10, nodeRect.Y + 10);

            // draw line to parent
            if (node.Parent != null)
            {
                var nodeTopMiddle = new Point(nodeRect.X + (nodeRect.Width / 2), nodeRect.Y);
                g.DrawLine(NODE_PEN, nodeTopMiddle, new Point(nodeTopMiddle.X, nodeTopMiddle.Y - (NODE_MARGIN_Y / 2)));
            }

            // draw line to children
            if (node.Children.Count > 0)
            {
                var nodeBottomMiddle = new Point(nodeRect.X + (nodeRect.Width / 2), nodeRect.Y + nodeRect.Height);
                g.DrawLine(NODE_PEN, nodeBottomMiddle, new Point(nodeBottomMiddle.X, nodeBottomMiddle.Y + (NODE_MARGIN_Y / 2)));


                // draw line over children
                if (node.Children.Count > 1)
                {
                    var childrenLineStart = new Point(
                        Convert.ToInt32(NODE_MARGIN_X + (node.GetRightMostChild().X * (NODE_WIDTH + NODE_MARGIN_X)) + (NODE_WIDTH / 2)),
                        nodeBottomMiddle.Y + (NODE_MARGIN_Y / 2));
                    var childrenLineEnd = new Point(
                        Convert.ToInt32(NODE_MARGIN_X + (node.GetLeftMostChild().X * (NODE_WIDTH + NODE_MARGIN_X)) + (NODE_WIDTH / 2)),
                        nodeBottomMiddle.Y + (NODE_MARGIN_Y / 2));

                    g.DrawLine(NODE_PEN, childrenLineStart, childrenLineEnd);
                }
            }


            foreach (var item in node.Children)
            {
                DrawNode(item, g);
            }
        }
    }
}
