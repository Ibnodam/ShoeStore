using System;
using System.Drawing;
using System.Windows.Forms;
using ShoeStore.Repositories;

namespace ShoeStore.Forms
{
    public partial class LoginForm : Form
    {
        private TextBox txtLogin;
        private TextBox txtPassword;
        private Button btnLogin;
        private Button btnGuest;
        private Label lblTitle;
        private Label lblLogin;
        private Label lblPassword;

        public LoginForm()
        {
            InitializeComponent();
            SetupForm();
        }

        private void SetupForm()
        {
            this.Text = "Авторизация - Магазин обуви";
            this.Size = new Size(300, 250);
            this.StartPosition = FormStartPosition.CenterScreen;
            this.FormBorderStyle = FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.BackColor = Color.White;

            lblTitle = new Label
            {
                Text = "Вход в систему",
                Font = new Font("Courier New", 12, FontStyle.Bold),
                Location = new Point(80, 20),
                Size = new Size(150, 25),
                TextAlign = ContentAlignment.MiddleCenter
            };

            lblLogin = new Label
            {
                Text = "Логин:",
                Font = new Font("Courier New", 10),
                Location = new Point(30, 60),
                Size = new Size(60, 25),
                TextAlign = ContentAlignment.MiddleRight
            };

            txtLogin = new TextBox
            {
                Location = new Point(100, 60),
                Size = new Size(150, 25),
                Font = new Font("Courier New", 10)
            };

            lblPassword = new Label
            {
                Text = "Пароль:",
                Font = new Font("Courier New", 10),
                Location = new Point(30, 100),
                Size = new Size(60, 25),
                TextAlign = ContentAlignment.MiddleRight
            };

            txtPassword = new TextBox
            {
                Location = new Point(100, 100),
                Size = new Size(150, 25),
                Font = new Font("Courier New", 10),
                PasswordChar = '*'
            };

            btnLogin = new Button
            {
                Text = "Войти",
                Location = new Point(60, 150),
                Size = new Size(80, 30),
                Font = new Font("Courier New", 10),
                BackColor = Color.FromArgb(161, 99, 245),
                ForeColor = Color.White,
                FlatStyle = FlatStyle.Flat
            };
            btnLogin.FlatAppearance.BorderSize = 0;
            btnLogin.Click += BtnLogin_Click;

            btnGuest = new Button
            {
                Text = "Гость",
                Location = new Point(150, 150),
                Size = new Size(80, 30),
                Font = new Font("Courier New", 10),
                BackColor = Color.White,
                FlatStyle = FlatStyle.Flat
            };
            btnGuest.FlatAppearance.BorderSize = 1;
            btnGuest.Click += BtnGuest_Click;

            this.Controls.Add(lblTitle);
            this.Controls.Add(lblLogin);
            this.Controls.Add(txtLogin);
            this.Controls.Add(lblPassword);
            this.Controls.Add(txtPassword);
            this.Controls.Add(btnLogin);
            this.Controls.Add(btnGuest);
        }

        private void BtnLogin_Click(object? sender, EventArgs e)
        {
            string login = txtLogin.Text.Trim();
            string password = txtPassword.Text;

            if (string.IsNullOrEmpty(login) || string.IsNullOrEmpty(password))
            {
                MessageBox.Show("Введите логин и пароль!", "Ошибка",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            try
            {
                var repo = new UserRepository();
                var user = repo.GetUserByLogin(login);

                if (user != null && user.Password == password)
                {
                    this.Hide();
                    var mainForm = new MainForm(user);
                    mainForm.ShowDialog();
                    this.Close();
                }
                else
                {
                    MessageBox.Show("Неверный логин или пароль!", "Ошибка",
                        MessageBoxButtons.OK, MessageBoxIcon.Error);
                    txtPassword.Clear();
                    txtPassword.Focus();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка: {ex.Message}", "Ошибка",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void BtnGuest_Click(object? sender, EventArgs e)
        {
            this.Hide();
            var mainForm = new MainForm(null);
            mainForm.ShowDialog();
            this.Close();
        }
    }
}









//using System;
//using System.Drawing;
//using System.Windows.Forms;
//using ShoeStore.Repositories;

//namespace ShoeStore.Forms
//{
//    public partial class LoginForm : Form
//    {
//        private TextBox txtLogin;
//        private TextBox txtPassword;
//        private Button btnLogin;
//        private Button btnGuest;
//        private Label lblTitle;
//        private Label lblLogin;
//        private Label lblPassword;

//        public LoginForm()
//        {
//            InitializeComponent();
//            SetupSimpleForm();
//        }

//        private void InitializeComponent()
//        {
//            this.Text = "Вход";
//            this.Size = new Size(300, 250);
//        }

//        private void SetupSimpleForm()
//        {
//            this.Text = "Авторизация";
//            this.Size = new Size(300, 250);
//            this.StartPosition = FormStartPosition.CenterScreen;
//            this.FormBorderStyle = FormBorderStyle.FixedDialog;
//            this.MaximizeBox = false;

//            lblTitle = new Label
//            {
//                Text = "Вход в систему",
//                Font = new Font("Arial", 12, FontStyle.Bold),
//                Location = new Point(80, 20),
//                Size = new Size(150, 25)
//            };

//            lblLogin = new Label
//            {
//                Text = "Логин:",
//                Location = new Point(30, 60),
//                Size = new Size(60, 25)
//            };

//            txtLogin = new TextBox
//            {
//                Location = new Point(100, 60),
//                Size = new Size(150, 25)
//            };

//            lblPassword = new Label
//            {
//                Text = "Пароль:",
//                Location = new Point(30, 100),
//                Size = new Size(60, 25)
//            };

//            txtPassword = new TextBox
//            {
//                Location = new Point(100, 100),
//                Size = new Size(150, 25),
//                PasswordChar = '*'
//            };

//            btnLogin = new Button
//            {
//                Text = "Войти",
//                Location = new Point(60, 150),
//                Size = new Size(80, 30)
//            };
//            btnLogin.Click += BtnLogin_Click;

//            btnGuest = new Button
//            {
//                Text = "Гость",
//                Location = new Point(150, 150),
//                Size = new Size(80, 30)
//            };
//            btnGuest.Click += BtnGuest_Click;

//            this.Controls.Add(lblTitle);
//            this.Controls.Add(lblLogin);
//            this.Controls.Add(txtLogin);
//            this.Controls.Add(lblPassword);
//            this.Controls.Add(txtPassword);
//            this.Controls.Add(btnLogin);
//            this.Controls.Add(btnGuest);
//        }

//        private void BtnLogin_Click(object? sender, EventArgs e)
//        {
//            string login = txtLogin.Text.Trim();
//            string password = txtPassword.Text;

//            if (login == "" || password == "")
//            {
//                MessageBox.Show("Введите логин и пароль");
//                return;
//            }

//            try
//            {
//                var repo = new UserRepository();
//                var user = repo.GetUserByLogin(login);

//                if (user != null && user.Password == password)
//                {
//                    this.Hide();
//                    var mainForm = new MainForm(user);
//                    mainForm.ShowDialog();
//                    this.Close();
//                }
//                else
//                {
//                    MessageBox.Show("Неверный логин или пароль");
//                    txtPassword.Clear();
//                }
//            }
//            catch
//            {
//                MessageBox.Show("Ошибка подключения к БД");
//            }
//        }

//        private void BtnGuest_Click(object? sender, EventArgs e)
//        {
//            this.Hide();
//            var mainForm = new MainForm(null);
//            mainForm.ShowDialog();
//            this.Close();
//        }
//    }
//}




//using System;
//using System.Collections.Generic;
//using System.ComponentModel;
//using System.Data;
//using System.Drawing;
//using System.Linq;
//using System.Text;
//using System.Threading.Tasks;
//using System.Windows.Forms;

//namespace Klimaitis.Forms
//{
//    public partial class LoginForm : Form
//    {
//        public LoginForm()
//        {
//            InitializeComponent();
//        }
//    }
//}
