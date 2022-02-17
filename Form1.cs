using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace UndoCSharp
{
    public partial class Form1 : Form
    {
        class ACTION
        {
            public Action undo;
            public Action redo;
        }

        Stack<ACTION> undo_stack = new Stack<ACTION>();
        Stack<ACTION> redo_stack = new Stack<ACTION>();
        StringBuilder s = new StringBuilder(1000);
        public Form1()
        {
            InitializeComponent();
        }
        void Undo()
        {
            if (undo_stack.Count == 0)
            {
                return;
            }
            ACTION action = undo_stack.Pop();
            redo_stack.Push(action);// add action nay vo redo -> magic

            action.undo.Invoke();
        }
        void Redo()
        {
            if (redo_stack.Count == 0)
            {
                return;
            }
            ACTION action = redo_stack.Pop();
            undo_stack.Push(action);// add action nay vo undo -> magic

            action.redo.Invoke();
        }

        private void add_text(string str, int at)
        {
            ACTION action = new ACTION()
            {
                undo = () =>
                {
                    s.Remove(at, str.Length);
                    label1.Text = s.ToString();
                },
                redo = () =>
                {
                    s.Insert(at, str);
                    label1.Text = s.ToString();
                }
            };

            undo_stack.Push(action);
            action.redo();
        }
        private void remove_text(int idx, string str)
        {
            ACTION action = new ACTION()
            {
                undo = () =>
                {
                    s.Insert(idx, str);
                    label1.Text = s.ToString();
                },
                redo = () =>
                {
                    s.Remove(idx, str.Length);
                    label1.Text = s.ToString();
                }
            };

            undo_stack.Push(action);
            action.redo();
        }

        string[] texts = { "Chuyện hôm qua ", "như nước chảy", " về đông",
" Mãi", " xa ta không", " sao giữ được",
" Hôm nay ", "lại có bao chuyện", " ưu phiền ", "làm rối cả lòng ta",
" Rút dao chém", " xuống nước, ", "nước càng ", "chảy mạnh",
" Nâng chén tiêu sầu,", " càng sầu thêm" };
        int iiiii = 0;
        private void button1_Click(object sender, EventArgs e)
        {
            add_text(texts[iiiii], s.Length);
            redo_stack.Clear();// always clear redo when user do some action

            iiiii++;
            if (iiiii >= texts.Length) iiiii = 0;
        }

        private void button2_Click(object sender, EventArgs e)
        {
            // just find a good string to remove, nothing special
            string aaa = s.ToString().Trim();
            int idx_of_space = aaa.IndexOf(' ');
            if (idx_of_space == -1)
            {
                return;
            }
            int idx_of_space_next = aaa.IndexOf(' ', idx_of_space + 1);
            if (idx_of_space_next == -1)
            {
                return;
            }

            remove_text(idx_of_space, aaa.Substring(idx_of_space, idx_of_space_next - idx_of_space));

            redo_stack.Clear();// always clear redo when user do some action
        }

        private void button3_Click(object sender, EventArgs e)
        {
            Undo();
        }

        private void button4_Click(object sender, EventArgs e)
        {
            Redo();
        }
    }
}
