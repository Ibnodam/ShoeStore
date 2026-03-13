using Klimaitis.Models;
using System;
using System.Drawing;
using System.Windows.Forms;

namespace ShoeStore.Forms
{
    public partial class MainForm : Form
    {
        private User? _currentUser;

        public MainForm(User? user)
        {
            _currentUser = user;
            InitializeComponent();
            SetupForm();
        }

        private void SetupForm()
        {
            this.Text = "Главная - Магазин обуви";
            this.Size = new Size(400, 200);
            this.StartPosition = FormStartPosition.CenterScreen;
            this.BackColor = Color.White;

            string userInfo = _currentUser == null
                ? "Гость"
                : $"{_currentUser.Surname} {_currentUser.Name} {_currentUser.Patronymic} ({_currentUser.Role?.RoleName})";

            Label lblUser = new Label
            {
                Text = $"Пользователь: {userInfo}",
                Font = new Font("Courier New", 10),
                Location = new Point(20, 20),
                Size = new Size(300, 25)
            };

            Button btnCatalog = new Button
            {
                Text = "Каталог товаров",
                Location = new Point(100, 70),
                Size = new Size(200, 40),
                Font = new Font("Courier New", 12),
                BackColor = Color.FromArgb(161, 99, 245),
                ForeColor = Color.White,
                FlatStyle = FlatStyle.Flat
            };
            btnCatalog.Click += (s, e) =>
            {
                var form = new ProductListForm(_currentUser);
                form.ShowDialog();
            };

            Button btnExit = new Button
            {
                Text = "Выход",
                Location = new Point(150, 120),
                Size = new Size(100, 30),
                Font = new Font("Courier New", 10)
            };
            btnExit.Click += (s, e) => this.Close();

            this.Controls.Add(lblUser);
            this.Controls.Add(btnCatalog);
            this.Controls.Add(btnExit);
        }
    }
}



//using Klimaitis.Models;
//using System;
//using System.Drawing;
//using System.Windows.Forms;


//namespace ShoeStore.Forms;

//public partial class MainForm : Form
//{
//    private User? _currentUser;

//    public MainForm(User? user)
//    {
//        _currentUser = user;
//        InitializeComponent();
//    }

//    private void InitializeComponent()
//    {
//        this.Text = "Главная - Магазин обуви";
//        this.Size = new Size(400, 200);
//        this.StartPosition = FormStartPosition.CenterScreen;
//        this.BackColor = Color.White;

//        string userInfo = _currentUser == null
//            ? "Гость"
//            : $"{_currentUser.Surname} {_currentUser.Name} {_currentUser.Patronymic} ({_currentUser.Role?.RoleName})";

//        Label lblUser = new Label
//        {
//            Text = $"Пользователь: {userInfo}",
//            Font = new Font("Courier New", 10),
//            Location = new Point(20, 20),
//            Size = new Size(300, 25)
//        };

//        Button btnCatalog = new Button
//        {
//            Text = "Каталог товаров",
//            Location = new Point(100, 70),
//            Size = new Size(200, 40),
//            Font = new Font("Courier New", 12),
//            BackColor = Color.FromArgb(161, 99, 245),
//            ForeColor = Color.White,
//            FlatStyle = FlatStyle.Flat
//        };
//        btnCatalog.Click += (s, e) =>
//        {
//            var form = new ProductListForm(_currentUser);
//            form.ShowDialog();
//        };

//        Button btnExit = new Button
//        {
//            Text = "Выход",
//            Location = new Point(150, 120),
//            Size = new Size(100, 30),
//            Font = new Font("Courier New", 10)
//        };
//        btnExit.Click += (s, e) => this.Close();

//        this.Controls.Add(lblUser);
//        this.Controls.Add(btnCatalog);
//        this.Controls.Add(btnExit);
//    }
//}






////using System;
////using System.Collections.Generic;
////using System.ComponentModel;
////using System.Data;
////using System.Drawing;
////using System.Linq;
////using System.Text;
////using System.Threading.Tasks;
////using System.Windows.Forms;

////namespace Klimaitis.Forms
////{
////    public partial class MainForm : Form
////    {
////        public MainForm()
////        {
////            InitializeComponent();
////        }
////    }
////}
