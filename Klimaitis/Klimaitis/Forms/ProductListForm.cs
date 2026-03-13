using Klimaitis.Models;
using ShoeStore.Repositories;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;

namespace ShoeStore.Forms
{
    public partial class ProductListForm : Form
    {
        private DataGridView dgvProducts;
        private Panel topPanel;
        private Button btnBack;
        private Label lblUserInfo;
        private ComboBox cmbFilterSupplier;
        private TextBox txtSearch;
        private Button btnAdd;
        private Button btnEdit;
        private Button btnDelete;
        private Button btnOrders;
        private User? _currentUser;
        private ProductRepository _productRepo;
        private List<Product> _allProducts;

        public ProductListForm(User? user)
        {
            _currentUser = user;
            _productRepo = new ProductRepository();
            InitializeComponent();
            SetupForm();
            LoadData();
        }

        private void SetupForm()
        {
            this.Text = "Каталог товаров - Магазин обуви";
            this.Size = new Size(1200, 700);
            this.StartPosition = FormStartPosition.CenterScreen;
            this.BackColor = Color.White;

            // Верхняя панель
            topPanel = new Panel
            {
                Dock = DockStyle.Top,
                Height = 100,
                BackColor = Color.FromArgb(211, 211, 211)
            };

            // Информация о пользователе
            string userInfo = _currentUser == null
                ? "Гость"
                : $"{_currentUser.Surname} {_currentUser.Name} {_currentUser.Patronymic} ({_currentUser.Role?.RoleName})";
            lblUserInfo = new Label
            {
                Text = $"Пользователь: {userInfo}",
                Font = new Font("Courier New", 10),
                Location = new Point(20, 15),
                Size = new Size(400, 25)
            };

            btnBack = new Button
            {
                Text = "Назад",
                Location = new Point(1050, 15),
                Size = new Size(100, 30),
                Font = new Font("Courier New", 10),
                BackColor = Color.White,
                FlatStyle = FlatStyle.Flat
            };
            btnBack.Click += (s, e) => this.Close();

            topPanel.Controls.Add(lblUserInfo);
            topPanel.Controls.Add(btnBack);

            // Фильтры (только для менеджера и админа)
            if (_currentUser != null && (_currentUser.Role?.RoleName == "Менеджер" || _currentUser.Role?.RoleName == "Администратор"))
            {
                Label lblSearch = new Label
                {
                    Text = "Поиск:",
                    Font = new Font("Courier New", 10),
                    Location = new Point(20, 50),
                    Size = new Size(60, 25)
                };
                txtSearch = new TextBox
                {
                    Location = new Point(90, 50),
                    Size = new Size(200, 25),
                    Font = new Font("Courier New", 10)
                };
                txtSearch.TextChanged += (s, e) => ApplyFilter();

                Label lblFilter = new Label
                {
                    Text = "Поставщик:",
                    Font = new Font("Courier New", 10),
                    Location = new Point(310, 50),
                    Size = new Size(90, 25)
                };
                cmbFilterSupplier = new ComboBox
                {
                    Location = new Point(410, 50),
                    Size = new Size(150, 25),
                    Font = new Font("Courier New", 10),
                    DropDownStyle = ComboBoxStyle.DropDownList
                };
                cmbFilterSupplier.SelectedIndexChanged += (s, e) => ApplyFilter();

                topPanel.Controls.Add(lblSearch);
                topPanel.Controls.Add(txtSearch);
                topPanel.Controls.Add(lblFilter);
                topPanel.Controls.Add(cmbFilterSupplier);
            }

            // Кнопки для админа
            if (_currentUser != null && _currentUser.Role?.RoleName == "Администратор")
            {
                btnAdd = new Button
                {
                    Text = "Добавить",
                    Location = new Point(600, 15),
                    Size = new Size(100, 30),
                    Font = new Font("Courier New", 10),
                    BackColor = Color.FromArgb(161, 99, 245),
                    ForeColor = Color.White,
                    FlatStyle = FlatStyle.Flat
                };
                // ✅ ИСПРАВЛЕНИЕ: подключаем к реальному методу
                btnAdd.Click += BtnAdd_Click;

                btnEdit = new Button
                {
                    Text = "Изменить",
                    Location = new Point(710, 15),
                    Size = new Size(100, 30),
                    Font = new Font("Courier New", 10),
                    BackColor = Color.White,
                    FlatStyle = FlatStyle.Flat
                };
                // ✅ ИСПРАВЛЕНИЕ: подключаем к реальному методу
                btnEdit.Click += BtnEdit_Click;

                btnDelete = new Button
                {
                    Text = "Удалить",
                    Location = new Point(820, 15),
                    Size = new Size(100, 30),
                    Font = new Font("Courier New", 10),
                    BackColor = Color.White,
                    FlatStyle = FlatStyle.Flat
                };
                // ✅ ИСПРАВЛЕНИЕ: подключаем к реальному методу
                btnDelete.Click += BtnDelete_Click;

                topPanel.Controls.Add(btnAdd);
                topPanel.Controls.Add(btnEdit);
                topPanel.Controls.Add(btnDelete);
            }

            // Кнопка Заказы для менеджера и админа
            if (_currentUser != null && (_currentUser.Role?.RoleName == "Менеджер" || _currentUser.Role?.RoleName == "Администратор"))
            {
                btnOrders = new Button
                {
                    Text = "Заказы",
                    Location = new Point(940, 15),
                    Size = new Size(100, 30),
                    Font = new Font("Courier New", 10),
                    BackColor = Color.FromArgb(161, 99, 245),
                    ForeColor = Color.White,
                    FlatStyle = FlatStyle.Flat
                };
                btnOrders.Click += BtnOrders_Click;
                topPanel.Controls.Add(btnOrders);
            }

            // Таблица товаров
            dgvProducts = new DataGridView
            {
                Dock = DockStyle.Fill,
                AllowUserToAddRows = false,
                AllowUserToDeleteRows = false,
                ReadOnly = true,
                SelectionMode = DataGridViewSelectionMode.FullRowSelect,
                AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill,
                Font = new Font("Courier New", 9),
                RowTemplate = { Height = 60 }
            };

            // Добавляем колонку для фото
            DataGridViewImageColumn photoColumn = new DataGridViewImageColumn
            {
                Name = "Photo",
                HeaderText = "Фото",
                Width = 80
            };
            dgvProducts.Columns.Add(photoColumn);

            dgvProducts.Columns.Add("Name", "Наименование");
            dgvProducts.Columns.Add("Category", "Категория");
            dgvProducts.Columns.Add("Price", "Цена");
            dgvProducts.Columns.Add("Quantity", "Кол-во");
            dgvProducts.Columns.Add("Discount", "Скидка %");
            dgvProducts.Columns.Add("FinalPrice", "Цена со скидкой");

            // Двойной клик для редактирования
            dgvProducts.CellDoubleClick += (s, e) =>
            {
                if (_currentUser?.Role?.RoleName == "Администратор" && dgvProducts.SelectedRows.Count > 0)
                {
                    BtnEdit_Click(null, null);
                }
            };

            this.Controls.Add(dgvProducts);
            this.Controls.Add(topPanel);
        }

        // ✅ Метод добавления
        private void BtnAdd_Click(object? sender, EventArgs e)
        {
            using (var form = new ProductEditForm(null))
            {
                if (form.ShowDialog() == DialogResult.OK)
                {
                    LoadData();
                }
            }
        }

        // ✅ Метод редактирования
        private void BtnEdit_Click(object? sender, EventArgs e)
        {
            if (dgvProducts.SelectedRows.Count == 0)
            {
                MessageBox.Show("Выберите товар для редактирования", "Внимание",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            var selectedRow = dgvProducts.SelectedRows[0];
            // ✅ Получаем Product из Tag строки
            if (selectedRow.Tag is Product product)
            {
                using (var form = new ProductEditForm(product))
                {
                    if (form.ShowDialog() == DialogResult.OK)
                    {
                        LoadData();
                    }
                }
            }
            else
            {
                MessageBox.Show("Ошибка: не удалось получить данные товара", "Ошибка",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        // ✅ Метод удаления
        private void BtnDelete_Click(object? sender, EventArgs e)
        {
            if (dgvProducts.SelectedRows.Count == 0)
            {
                MessageBox.Show("Выберите товар для удаления", "Внимание",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            var selectedRow = dgvProducts.SelectedRows[0];
            // ✅ Получаем Product из Tag строки
            if (selectedRow.Tag is Product product)
            {
                var result = MessageBox.Show($"Удалить товар \"{product.ProductName}\"?",
                    "Подтверждение удаления",
                    MessageBoxButtons.YesNo,
                    MessageBoxIcon.Question);

                if (result == DialogResult.Yes)
                {
                    if (_productRepo.DeleteProduct(product.ProductId))
                    {
                        LoadData();
                        MessageBox.Show("Товар удален", "Успех",
                            MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                    else
                    {
                        MessageBox.Show("Нельзя удалить товар, который есть в заказах", "Ошибка",
                            MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    }
                }
            }
            else
            {
                MessageBox.Show("Ошибка: не удалось получить данные товара", "Ошибка",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void BtnOrders_Click(object? sender, EventArgs e)
        {
            MessageBox.Show("Раздел заказов будет позже", "Информация",
                MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private void LoadData()
        {
            try
            {
                _allProducts = _productRepo.GetAllProducts();
                LoadFilterComboBox();
                DisplayProducts(_allProducts);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка: {ex.Message}");
            }
        }

        private void LoadFilterComboBox()
        {
            if (cmbFilterSupplier == null) return;
            var suppliers = _productRepo.GetAllSuppliers();
            cmbFilterSupplier.Items.Clear();
            cmbFilterSupplier.Items.Add("Все");
            foreach (var s in suppliers)
                cmbFilterSupplier.Items.Add(s.SupplierName);
            cmbFilterSupplier.SelectedIndex = 0;
        }

        private void ApplyFilter()
        {
            if (_allProducts == null) return;
            var filtered = _allProducts;

            if (cmbFilterSupplier != null && cmbFilterSupplier.SelectedIndex > 0)
            {
                string sel = cmbFilterSupplier.SelectedItem?.ToString() ?? "";
                filtered = filtered.FindAll(p => p.Supplier?.SupplierName == sel);
            }

            if (txtSearch != null && !string.IsNullOrWhiteSpace(txtSearch.Text))
            {
                string search = txtSearch.Text.ToLower();
                filtered = filtered.FindAll(p =>
                    (p.ProductName != null && p.ProductName.ToLower().Contains(search)) ||
                    (p.Description != null && p.Description.ToLower().Contains(search)) ||
                    (p.Article != null && p.Article.ToLower().Contains(search))
                );
            }

            DisplayProducts(filtered);
        }

        private void DisplayProducts(List<Product> products)
        {
            dgvProducts.Rows.Clear();
            foreach (var p in products)
            {
                decimal finalPrice = p.Price;
                if (p.Discount > 0)
                    finalPrice = p.Price * (1 - p.Discount / 100m);

                // Загрузка фото
                Image? photo = null;
                if (!string.IsNullOrEmpty(p.PhotoPath))
                {
                    string fullPath = Path.Combine(Application.StartupPath, p.PhotoPath);
                    if (File.Exists(fullPath))
                        photo = Image.FromFile(fullPath);
                }

                int row = dgvProducts.Rows.Add(
                    photo ?? null,
                    p.ProductName,
                    p.Category?.CategoryName,
                    p.Price.ToString("N2") + " ₽",
                    p.QuantityInStock,
                    p.Discount.ToString("N0"),
                    finalPrice.ToString("N2") + " ₽"
                );

                // ✅ ИСПРАВЛЕНИЕ: Сохраняем объект Product в Tag строки
                dgvProducts.Rows[row].Tag = p;

                // Цветовая индикация
                if (p.QuantityInStock == 0)
                    dgvProducts.Rows[row].DefaultCellStyle.BackColor = Color.LightBlue;
                if (p.Discount > 15)
                    dgvProducts.Rows[row].DefaultCellStyle.BackColor = Color.FromArgb(46, 139, 87);
            }
        }
    }
}





//using Klimaitis.Models;
//using ShoeStore.Repositories;
//using System;
//using System.Collections.Generic;
//using System.Drawing;
//using System.Windows.Forms;

//namespace ShoeStore.Forms
//{
//    public partial class ProductListForm : Form
//    {
//        private DataGridView dgvProducts;
//        private Panel topPanel;
//        private Button btnBack;
//        private Label lblUserInfo;
//        private ComboBox cmbFilterSupplier;
//        private TextBox txtSearch;
//        private Button btnAdd;
//        private Button btnEdit;
//        private Button btnDelete;
//        private Button btnOrders;

//        private User? _currentUser;
//        private ProductRepository _productRepo;
//        private List<Product> _allProducts;

//        public ProductListForm(User? user)
//        {
//            _currentUser = user;
//            _productRepo = new ProductRepository();
//            InitializeComponent();
//            SetupForm();
//            LoadData();
//        }

//        private void SetupForm()
//        {
//            this.Text = "Каталог товаров - Магазин обуви";
//            this.Size = new Size(1200, 700);
//            this.StartPosition = FormStartPosition.CenterScreen;
//            this.BackColor = Color.White;

//            // Верхняя панель
//            topPanel = new Panel
//            {
//                Dock = DockStyle.Top,
//                Height = 100,
//                BackColor = Color.FromArgb(211, 211, 211)
//            };

//            // Информация о пользователе
//            string userInfo = _currentUser == null
//                ? "Гость"
//                : $"{_currentUser.Surname} {_currentUser.Name} {_currentUser.Patronymic} ({_currentUser.Role?.RoleName})";

//            lblUserInfo = new Label
//            {
//                Text = $"Пользователь: {userInfo}",
//                Font = new Font("Courier New", 10),
//                Location = new Point(20, 15),
//                Size = new Size(400, 25)
//            };

//            btnBack = new Button
//            {
//                Text = "Назад",
//                Location = new Point(1050, 15),
//                Size = new Size(100, 30),
//                Font = new Font("Courier New", 10),
//                BackColor = Color.White,
//                FlatStyle = FlatStyle.Flat
//            };
//            btnBack.Click += (s, e) => this.Close();

//            topPanel.Controls.Add(lblUserInfo);
//            topPanel.Controls.Add(btnBack);

//            // Фильтры (только для менеджера и админа)
//            if (_currentUser != null && (_currentUser.Role?.RoleName == "Менеджер" || _currentUser.Role?.RoleName == "Администратор"))
//            {
//                Label lblSearch = new Label
//                {
//                    Text = "Поиск:",
//                    Font = new Font("Courier New", 10),
//                    Location = new Point(20, 50),
//                    Size = new Size(60, 25)
//                };

//                txtSearch = new TextBox
//                {
//                    Location = new Point(90, 50),
//                    Size = new Size(200, 25),
//                    Font = new Font("Courier New", 10)
//                };
//                txtSearch.TextChanged += (s, e) => ApplyFilter();

//                Label lblFilter = new Label
//                {
//                    Text = "Поставщик:",
//                    Font = new Font("Courier New", 10),
//                    Location = new Point(310, 50),
//                    Size = new Size(90, 25)
//                };

//                cmbFilterSupplier = new ComboBox
//                {
//                    Location = new Point(410, 50),
//                    Size = new Size(150, 25),
//                    Font = new Font("Courier New", 10),
//                    DropDownStyle = ComboBoxStyle.DropDownList
//                };
//                cmbFilterSupplier.SelectedIndexChanged += (s, e) => ApplyFilter();

//                topPanel.Controls.Add(lblSearch);
//                topPanel.Controls.Add(txtSearch);
//                topPanel.Controls.Add(lblFilter);
//                topPanel.Controls.Add(cmbFilterSupplier);
//            }

//            // Кнопки для админа
//            if (_currentUser != null && _currentUser.Role?.RoleName == "Администратор")
//            {
//                btnAdd = new Button
//                {
//                    Text = "Добавить",
//                    Location = new Point(600, 15),
//                    Size = new Size(100, 30),
//                    Font = new Font("Courier New", 10),
//                    BackColor = Color.FromArgb(161, 99, 245),
//                    ForeColor = Color.White,
//                    FlatStyle = FlatStyle.Flat
//                };
//                //btnAdd.Click += (s, e) => MessageBox.Show("Добавление товара");
//                btnAdd.Click += BtnAdd_Click;

//                btnEdit = new Button
//                {
//                    Text = "Изменить",
//                    Location = new Point(710, 15),
//                    Size = new Size(100, 30),
//                    Font = new Font("Courier New", 10),
//                    BackColor = Color.White,
//                    FlatStyle = FlatStyle.Flat
//                };
//                //btnEdit.Click += (s, e) => MessageBox.Show("Редактирование товара");
//                btnEdit.Click += BtnEdit_Click;

//                btnDelete = new Button
//                {
//                    Text = "Удалить",
//                    Location = new Point(820, 15),
//                    Size = new Size(100, 30),
//                    Font = new Font("Courier New", 10),
//                    BackColor = Color.White,
//                    FlatStyle = FlatStyle.Flat
//                };
//                //btnDelete.Click += (s, e) => MessageBox.Show("Удаление товара");
//                btnDelete.Click += BtnDelete_Click;

//                topPanel.Controls.Add(btnAdd);
//                topPanel.Controls.Add(btnEdit);
//                topPanel.Controls.Add(btnDelete);
//            }

//            // Кнопка Заказы для менеджера и админа
//            if (_currentUser != null && (_currentUser.Role?.RoleName == "Менеджер" || _currentUser.Role?.RoleName == "Администратор"))
//            {
//                btnOrders = new Button
//                {
//                    Text = "Заказы",
//                    Location = new Point(940, 15),
//                    Size = new Size(100, 30),
//                    Font = new Font("Courier New", 10),
//                    BackColor = Color.FromArgb(161, 99, 245),
//                    ForeColor = Color.White,
//                    FlatStyle = FlatStyle.Flat
//                };
//                btnOrders.Click += (s, e) => MessageBox.Show("Список заказов");
//                topPanel.Controls.Add(btnOrders);
//            }

//            // Таблица товаров
//            dgvProducts = new DataGridView
//            {
//                Dock = DockStyle.Fill,
//                AllowUserToAddRows = false,
//                AllowUserToDeleteRows = false,
//                ReadOnly = true,
//                SelectionMode = DataGridViewSelectionMode.FullRowSelect,
//                AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill,
//                Font = new Font("Courier New", 9),
//                RowTemplate = { Height = 60 }
//            };

//            dgvProducts.Columns.Add("Name", "Наименование");
//            dgvProducts.Columns.Add("Category", "Категория");
//            dgvProducts.Columns.Add("Price", "Цена");
//            dgvProducts.Columns.Add("Quantity", "Кол-во");
//            dgvProducts.Columns.Add("Discount", "Скидка %");
//            dgvProducts.Columns.Add("FinalPrice", "Цена со скидкой");

//            this.Controls.Add(dgvProducts);
//            this.Controls.Add(topPanel);
//        }


//        private void BtnAdd_Click(object? sender, EventArgs e)
//        {
//            using (var form = new ProductEditForm(null))
//            {
//                if (form.ShowDialog() == DialogResult.OK)
//                {
//                    LoadData();
//                }
//            }
//        }

//        private void BtnEdit_Click(object? sender, EventArgs e)
//        {
//            if (dgvProducts.SelectedRows.Count == 0)
//            {
//                MessageBox.Show("Выберите товар для редактирования", "Внимание",
//                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
//                return;
//            }

//            var selectedRow = dgvProducts.SelectedRows[0];
//            if (selectedRow.Tag is Product product)
//            {
//                using (var form = new ProductEditForm(product))
//                {
//                    if (form.ShowDialog() == DialogResult.OK)
//                    {
//                        LoadData();
//                    }
//                }
//            }
//        }

//        private void BtnDelete_Click(object? sender, EventArgs e)
//        {
//            if (dgvProducts.SelectedRows.Count == 0)
//            {
//                MessageBox.Show("Выберите товар для удаления", "Внимание",
//                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
//                return;
//            }

//            var selectedRow = dgvProducts.SelectedRows[0];
//            if (selectedRow.Tag is Product product)
//            {
//                var result = MessageBox.Show($"Удалить товар \"{product.ProductName}\"?",
//                    "Подтверждение удаления",
//                    MessageBoxButtons.YesNo,
//                    MessageBoxIcon.Question);

//                if (result == DialogResult.Yes)
//                {
//                    if (_productRepo.DeleteProduct(product.ProductId))
//                    {
//                        LoadData();
//                        MessageBox.Show("Товар удален", "Успех",
//                            MessageBoxButtons.OK, MessageBoxIcon.Information);
//                    }
//                    else
//                    {
//                        MessageBox.Show("Нельзя удалить товар, который есть в заказах", "Ошибка",
//                            MessageBoxButtons.OK, MessageBoxIcon.Warning);
//                    }
//                }
//            }
//        }

//        private void BtnOrders_Click(object? sender, EventArgs e)
//        {
//            MessageBox.Show("Раздел заказов будет позже", "Информация",
//                MessageBoxButtons.OK, MessageBoxIcon.Information);
//        }


//        private void LoadData()
//        {
//            try
//            {
//                _allProducts = _productRepo.GetAllProducts();
//                LoadFilterComboBox();
//                DisplayProducts(_allProducts);
//            }
//            catch (Exception ex)
//            {
//                MessageBox.Show($"Ошибка: {ex.Message}");
//            }
//        }

//        private void LoadFilterComboBox()
//        {
//            if (cmbFilterSupplier == null) return;

//            var suppliers = _productRepo.GetAllSuppliers();
//            cmbFilterSupplier.Items.Clear();
//            cmbFilterSupplier.Items.Add("Все");
//            foreach (var s in suppliers)
//                cmbFilterSupplier.Items.Add(s.SupplierName);
//            cmbFilterSupplier.SelectedIndex = 0;
//        }

//        private void ApplyFilter()
//        {
//            if (_allProducts == null) return;

//            var filtered = _allProducts;

//            if (cmbFilterSupplier != null && cmbFilterSupplier.SelectedIndex > 0)
//            {
//                string sel = cmbFilterSupplier.SelectedItem.ToString();
//                filtered = filtered.FindAll(p => p.Supplier?.SupplierName == sel);
//            }

//            if (txtSearch != null && !string.IsNullOrWhiteSpace(txtSearch.Text))
//            {
//                string search = txtSearch.Text.ToLower();
//                filtered = filtered.FindAll(p =>
//                    (p.ProductName != null && p.ProductName.ToLower().Contains(search)) ||
//                    (p.Description != null && p.Description.ToLower().Contains(search)) ||
//                    (p.Article != null && p.Article.ToLower().Contains(search))
//                );
//            }

//            DisplayProducts(filtered);
//        }

//        private void DisplayProducts(List<Product> products)
//        {
//            dgvProducts.Rows.Clear();

//            foreach (var p in products)
//            {
//                decimal finalPrice = p.Price;
//                if (p.Discount > 0)
//                    finalPrice = p.Price * (1 - p.Discount / 100m);

//                int row = dgvProducts.Rows.Add(
//                    p.ProductName,
//                    p.Category?.CategoryName,
//                    p.Price.ToString("N2") + " ₽",
//                    p.QuantityInStock,
//                    p.Discount.ToString("N0"),
//                    finalPrice.ToString("N2") + " ₽"
//                );

//                if (p.QuantityInStock == 0)
//                    dgvProducts.Rows[row].DefaultCellStyle.BackColor = Color.LightBlue;

//                if (p.Discount > 15)
//                    dgvProducts.Rows[row].DefaultCellStyle.BackColor = Color.FromArgb(46, 139, 87);
//            }
//        }
//    }
//}









////using Klimaitis.Models;
////using ShoeStore.Repositories;
////using System;
////using System.Collections.Generic;
////using System.Drawing;
////using System.IO;
////using System.Windows.Forms;

////namespace ShoeStore.Forms
////{
////    public partial class ProductListForm : Form
////    {
////        private DataGridView dgvProducts;
////        private Panel topPanel;
////        private Button btnBack;
////        private Label lblUserInfo;
////        private ComboBox cmbFilterSupplier;
////        private TextBox txtSearch;
////        private Button btnAdd;
////        private Button btnEdit;
////        private Button btnDelete;
////        private Button btnOrders;

////        private User? _currentUser;
////        private ProductRepository _productRepo;
////        private List<Product> _allProducts;

////        public ProductListForm(User? user)
////        {
////            _currentUser = user;
////            _productRepo = new ProductRepository();
////            InitializeComponent();
////            LoadData();
////        }

////        private void InitializeComponent()
////        {
////            this.Text = "Каталог товаров - Магазин обуви";
////            this.Size = new Size(1200, 700);
////            this.StartPosition = FormStartPosition.CenterScreen;
////            this.BackColor = Color.White;

////            // Верхняя панель
////            topPanel = new Panel
////            {
////                Dock = DockStyle.Top,
////                Height = 100,
////                BackColor = Color.FromArgb(211, 211, 211)
////            };

////            // Информация о пользователе
////            string userInfo = _currentUser == null
////                ? "Гость"
////                : $"{_currentUser.Surname} {_currentUser.Name} {_currentUser.Patronymic} ({_currentUser.Role?.RoleName})";

////            lblUserInfo = new Label
////            {
////                Text = $"Пользователь: {userInfo}",
////                Font = new Font("Courier New", 10),
////                Location = new Point(20, 15),
////                Size = new Size(400, 25)
////            };

////            btnBack = new Button
////            {
////                Text = "Назад",
////                Location = new Point(1050, 15),
////                Size = new Size(100, 30),
////                Font = new Font("Courier New", 10),
////                BackColor = Color.White,
////                FlatStyle = FlatStyle.Flat
////            };
////            btnBack.Click += (s, e) => this.Close();

////            topPanel.Controls.Add(lblUserInfo);
////            topPanel.Controls.Add(btnBack);

////            // Фильтры (только для менеджера и админа)
////            if (_currentUser != null && (_currentUser.Role?.RoleName == "Менеджер" || _currentUser.Role?.RoleName == "Администратор"))
////            {
////                Label lblSearch = new Label
////                {
////                    Text = "Поиск:",
////                    Font = new Font("Courier New", 10),
////                    Location = new Point(20, 50),
////                    Size = new Size(60, 25)
////                };

////                txtSearch = new TextBox
////                {
////                    Location = new Point(90, 50),
////                    Size = new Size(200, 25),
////                    Font = new Font("Courier New", 10)
////                };
////                txtSearch.TextChanged += (s, e) => ApplyFilter();

////                Label lblFilter = new Label
////                {
////                    Text = "Поставщик:",
////                    Font = new Font("Courier New", 10),
////                    Location = new Point(310, 50),
////                    Size = new Size(90, 25)
////                };

////                cmbFilterSupplier = new ComboBox
////                {
////                    Location = new Point(410, 50),
////                    Size = new Size(150, 25),
////                    Font = new Font("Courier New", 10),
////                    DropDownStyle = ComboBoxStyle.DropDownList
////                };
////                cmbFilterSupplier.SelectedIndexChanged += (s, e) => ApplyFilter();

////                topPanel.Controls.Add(lblSearch);
////                topPanel.Controls.Add(txtSearch);
////                topPanel.Controls.Add(lblFilter);
////                topPanel.Controls.Add(cmbFilterSupplier);
////            }

////            // Кнопки для админа
////            if (_currentUser != null && _currentUser.Role?.RoleName == "Администратор")
////            {
////                btnAdd = new Button
////                {
////                    Text = "Добавить",
////                    Location = new Point(600, 15),
////                    Size = new Size(100, 30),
////                    Font = new Font("Courier New", 10),
////                    BackColor = Color.FromArgb(161, 99, 245),
////                    ForeColor = Color.White,
////                    FlatStyle = FlatStyle.Flat
////                };
////                btnAdd.Click += (s, e) => MessageBox.Show("Добавление товара");

////                btnEdit = new Button
////                {
////                    Text = "Изменить",
////                    Location = new Point(710, 15),
////                    Size = new Size(100, 30),
////                    Font = new Font("Courier New", 10),
////                    BackColor = Color.White,
////                    FlatStyle = FlatStyle.Flat
////                };
////                btnEdit.Click += (s, e) => MessageBox.Show("Редактирование товара");

////                btnDelete = new Button
////                {
////                    Text = "Удалить",
////                    Location = new Point(820, 15),
////                    Size = new Size(100, 30),
////                    Font = new Font("Courier New", 10),
////                    BackColor = Color.White,
////                    FlatStyle = FlatStyle.Flat
////                };
////                btnDelete.Click += (s, e) => MessageBox.Show("Удаление товара");

////                topPanel.Controls.Add(btnAdd);
////                topPanel.Controls.Add(btnEdit);
////                topPanel.Controls.Add(btnDelete);
////            }

////            // Кнопка Заказы для менеджера и админа
////            if (_currentUser != null && (_currentUser.Role?.RoleName == "Менеджер" || _currentUser.Role?.RoleName == "Администратор"))
////            {
////                btnOrders = new Button
////                {
////                    Text = "Заказы",
////                    Location = new Point(940, 15),
////                    Size = new Size(100, 30),
////                    Font = new Font("Courier New", 10),
////                    BackColor = Color.FromArgb(161, 99, 245),
////                    ForeColor = Color.White,
////                    FlatStyle = FlatStyle.Flat
////                };
////                btnOrders.Click += (s, e) => MessageBox.Show("Список заказов");
////                topPanel.Controls.Add(btnOrders);
////            }

////            // Таблица товаров
////            dgvProducts = new DataGridView
////            {
////                Dock = DockStyle.Fill,
////                AllowUserToAddRows = false,
////                AllowUserToDeleteRows = false,
////                ReadOnly = true,
////                SelectionMode = DataGridViewSelectionMode.FullRowSelect,
////                AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill,
////                Font = new Font("Courier New", 9),
////                RowTemplate = { Height = 60 }
////            };

////            dgvProducts.Columns.Add("Name", "Наименование");
////            dgvProducts.Columns.Add("Category", "Категория");
////            dgvProducts.Columns.Add("Price", "Цена");
////            dgvProducts.Columns.Add("Quantity", "Кол-во");
////            dgvProducts.Columns.Add("Discount", "Скидка %");
////            dgvProducts.Columns.Add("FinalPrice", "Цена со скидкой");

////            this.Controls.Add(dgvProducts);
////            this.Controls.Add(topPanel);
////        }

////        private void LoadData()
////        {
////            try
////            {
////                _allProducts = _productRepo.GetAllProducts();
////                LoadFilterComboBox();
////                DisplayProducts(_allProducts);
////            }
////            catch (Exception ex)
////            {
////                MessageBox.Show($"Ошибка: {ex.Message}");
////            }
////        }

////        private void LoadFilterComboBox()
////        {
////            if (cmbFilterSupplier == null) return;

////            var suppliers = _productRepo.GetAllSuppliers();
////            cmbFilterSupplier.Items.Clear();
////            cmbFilterSupplier.Items.Add("Все");
////            foreach (var s in suppliers)
////                cmbFilterSupplier.Items.Add(s.SupplierName);
////            cmbFilterSupplier.SelectedIndex = 0;
////        }

////        private void ApplyFilter()
////        {
////            if (_allProducts == null) return;

////            var filtered = _allProducts;

////            if (cmbFilterSupplier != null && cmbFilterSupplier.SelectedIndex > 0)
////            {
////                string sel = cmbFilterSupplier.SelectedItem.ToString();
////                filtered = filtered.FindAll(p => p.Supplier?.SupplierName == sel);
////            }

////            if (txtSearch != null && !string.IsNullOrWhiteSpace(txtSearch.Text))
////            {
////                string search = txtSearch.Text.ToLower();
////                filtered = filtered.FindAll(p =>
////                    (p.ProductName != null && p.ProductName.ToLower().Contains(search)) ||
////                    (p.Description != null && p.Description.ToLower().Contains(search)) ||
////                    (p.Article != null && p.Article.ToLower().Contains(search))
////                );
////            }

////            DisplayProducts(filtered);
////        }



////        private void DisplayProducts(List<Product> products)
////        {
////            dgvProducts.Rows.Clear();

////            foreach (var p in products)
////            {
////                decimal finalPrice = p.Price;
////                if (p.Discount > 0)
////                    finalPrice = p.Price * (1 - p.Discount / 100m);

////                int row = dgvProducts.Rows.Add(
////                    p.ProductName,
////                    p.Category?.CategoryName,
////                    p.Price.ToString("N2") + " ₽",
////                    p.QuantityInStock,
////                    p.Discount.ToString("N0"),
////                    finalPrice.ToString("N2") + " ₽"
////                );

////                if (p.QuantityInStock == 0)
////                    dgvProducts.Rows[row].DefaultCellStyle.BackColor = Color.LightBlue;

////                if (p.Discount > 15)
////                    dgvProducts.Rows[row].DefaultCellStyle.BackColor = Color.FromArgb(46, 139, 87);
////            }
////        }


////    }
////}