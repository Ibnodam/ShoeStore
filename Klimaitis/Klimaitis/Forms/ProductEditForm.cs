using Klimaitis.Models;
using ShoeStore.Repositories;
using System;
using System.Drawing;
using System.IO;
using System.Windows.Forms;

namespace ShoeStore.Forms
{
    public partial class ProductEditForm : Form
    {
        private TextBox txtArticle;
        private TextBox txtName;
        private ComboBox cmbCategory;
        private TextBox txtDescription;
        private ComboBox cmbManufacturer;
        private ComboBox cmbSupplier;
        private TextBox txtPrice;
        private TextBox txtUnit;
        private TextBox txtQuantity;
        private TextBox txtDiscount;
        private PictureBox pbPhoto;
        private Button btnSelectPhoto;
        private Button btnSave;
        private Button btnCancel;
        private Label lblId;

        private Product? _product;
        private ProductRepository _productRepo;
        private string _selectedPhotoPath;
        private bool _isEditMode;

        public ProductEditForm(Product? product)
        {
            _product = product;
            _isEditMode = product != null;
            _productRepo = new ProductRepository();
            InitializeComponent();
            SetupForm();
            LoadComboBoxes();
            if (_isEditMode)
                LoadProductData();
        }

        private void SetupForm()
        {
            this.Text = _isEditMode ? "Редактирование товара" : "Добавление товара";
            this.Size = new Size(500, 600);
            this.StartPosition = FormStartPosition.CenterParent;
            this.FormBorderStyle = FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;

            int y = 20;
            int labelX = 20;
            int fieldX = 150;
            int fieldWidth = 250;

            if (_isEditMode)
            {
                Label lblIdText = new Label
                {
                    Text = "ID:",
                    Location = new Point(labelX, y),
                    Size = new Size(100, 25)
                };

                lblId = new Label
                {
                    Location = new Point(fieldX, y),
                    Size = new Size(fieldWidth, 25),
                    Text = _product?.ProductId.ToString()
                };

                this.Controls.Add(lblIdText);
                this.Controls.Add(lblId);
                y += 30;
            }

            Label lblArticle = new Label { Text = "Артикул:", Location = new Point(labelX, y), Size = new Size(100, 25) };
            txtArticle = new TextBox { Location = new Point(fieldX, y), Size = new Size(fieldWidth, 25) };
            this.Controls.Add(lblArticle);
            this.Controls.Add(txtArticle);
            y += 30;

            Label lblName = new Label { Text = "Наименование:", Location = new Point(labelX, y), Size = new Size(100, 25) };
            txtName = new TextBox { Location = new Point(fieldX, y), Size = new Size(fieldWidth, 25) };
            this.Controls.Add(lblName);
            this.Controls.Add(txtName);
            y += 30;

            Label lblCategory = new Label { Text = "Категория:", Location = new Point(labelX, y), Size = new Size(100, 25) };
            cmbCategory = new ComboBox
            {
                Location = new Point(fieldX, y),
                Size = new Size(fieldWidth, 25),
                DropDownStyle = ComboBoxStyle.DropDownList
            };
            this.Controls.Add(lblCategory);
            this.Controls.Add(cmbCategory);
            y += 30;

            Label lblManufacturer = new Label { Text = "Производитель:", Location = new Point(labelX, y), Size = new Size(100, 25) };
            cmbManufacturer = new ComboBox
            {
                Location = new Point(fieldX, y),
                Size = new Size(fieldWidth, 25),
                DropDownStyle = ComboBoxStyle.DropDownList
            };
            this.Controls.Add(lblManufacturer);
            this.Controls.Add(cmbManufacturer);
            y += 30;

            Label lblSupplier = new Label { Text = "Поставщик:", Location = new Point(labelX, y), Size = new Size(100, 25) };
            cmbSupplier = new ComboBox
            {
                Location = new Point(fieldX, y),
                Size = new Size(fieldWidth, 25),
                DropDownStyle = ComboBoxStyle.DropDownList
            };
            this.Controls.Add(lblSupplier);
            this.Controls.Add(cmbSupplier);
            y += 30;

            Label lblPrice = new Label { Text = "Цена:", Location = new Point(labelX, y), Size = new Size(100, 25) };
            txtPrice = new TextBox { Location = new Point(fieldX, y), Size = new Size(fieldWidth, 25) };
            this.Controls.Add(lblPrice);
            this.Controls.Add(txtPrice);
            y += 30;

            Label lblUnit = new Label { Text = "Ед. измерения:", Location = new Point(labelX, y), Size = new Size(100, 25) };
            txtUnit = new TextBox { Location = new Point(fieldX, y), Size = new Size(fieldWidth, 25), Text = "шт." };
            this.Controls.Add(lblUnit);
            this.Controls.Add(txtUnit);
            y += 30;

            Label lblQuantity = new Label { Text = "Количество:", Location = new Point(labelX, y), Size = new Size(100, 25) };
            txtQuantity = new TextBox { Location = new Point(fieldX, y), Size = new Size(fieldWidth, 25) };
            this.Controls.Add(lblQuantity);
            this.Controls.Add(txtQuantity);
            y += 30;

            Label lblDiscount = new Label { Text = "Скидка %:", Location = new Point(labelX, y), Size = new Size(100, 25) };
            txtDiscount = new TextBox { Location = new Point(fieldX, y), Size = new Size(fieldWidth, 25), Text = "0" };
            this.Controls.Add(lblDiscount);
            this.Controls.Add(txtDiscount);
            y += 30;

            Label lblDescription = new Label { Text = "Описание:", Location = new Point(labelX, y), Size = new Size(100, 25) };
            txtDescription = new TextBox
            {
                Location = new Point(fieldX, y),
                Size = new Size(fieldWidth, 60),
                Multiline = true
            };
            this.Controls.Add(lblDescription);
            this.Controls.Add(txtDescription);
            y += 70;

            Label lblPhoto = new Label { Text = "Фото:", Location = new Point(labelX, y), Size = new Size(100, 25) };
            pbPhoto = new PictureBox
            {
                Location = new Point(fieldX, y),
                Size = new Size(100, 100),
                SizeMode = PictureBoxSizeMode.Zoom,
                BorderStyle = BorderStyle.FixedSingle,
                BackColor = Color.White
            };

            btnSelectPhoto = new Button
            {
                Text = "Выбрать фото",
                Location = new Point(fieldX + 110, y + 35),
                Size = new Size(130, 30)
            };
            btnSelectPhoto.Click += BtnSelectPhoto_Click;

            this.Controls.Add(lblPhoto);
            this.Controls.Add(pbPhoto);
            this.Controls.Add(btnSelectPhoto);
            y += 110;

            btnSave = new Button
            {
                Text = "Сохранить",
                Location = new Point(150, y),
                Size = new Size(100, 35),
                BackColor = Color.FromArgb(161, 99, 245),
                ForeColor = Color.White,
                FlatStyle = FlatStyle.Flat
            };
            btnSave.Click += BtnSave_Click;

            btnCancel = new Button
            {
                Text = "Отмена",
                Location = new Point(260, y),
                Size = new Size(100, 35)
            };
            btnCancel.Click += (s, e) => this.Close();

            this.Controls.Add(btnSave);
            this.Controls.Add(btnCancel);
        }




        private void LoadComboBoxes()
        {
            cmbCategory.DataSource = _productRepo.GetAllCategories();
            cmbCategory.DisplayMember = "CategoryName";
            cmbCategory.ValueMember = "CategoryId";

            cmbManufacturer.DataSource = _productRepo.GetAllManufacturers();
            cmbManufacturer.DisplayMember = "ManufacturerName";
            cmbManufacturer.ValueMember = "ManufacturerId";

            cmbSupplier.DataSource = _productRepo.GetAllSuppliers();
            cmbSupplier.DisplayMember = "SupplierName";
            cmbSupplier.ValueMember = "SupplierId";
        }

        private void LoadProductData()
        {
            if (_product == null) return;

            txtArticle.Text = _product.Article;
            txtName.Text = _product.ProductName;
            cmbCategory.SelectedValue = _product.CategoryId;
            cmbManufacturer.SelectedValue = _product.ManufacturerId;
            cmbSupplier.SelectedValue = _product.SupplierId;
            txtPrice.Text = _product.Price.ToString();
            txtUnit.Text = _product.Unit;
            txtQuantity.Text = _product.QuantityInStock.ToString();
            txtDiscount.Text = _product.Discount.ToString();
            txtDescription.Text = _product.Description;

            if (!string.IsNullOrEmpty(_product.PhotoPath))
            {
                string fullPath = Path.Combine(Application.StartupPath, _product.PhotoPath);
                if (File.Exists(fullPath))
                {
                    pbPhoto.Image = Image.FromFile(fullPath);
                }
            }
        }

        private void BtnSelectPhoto_Click(object? sender, EventArgs e)
        {
            using (OpenFileDialog dialog = new OpenFileDialog())
            {
                dialog.Filter = "Image files|*.jpg;*.jpeg;*.png;*.bmp";
                dialog.FilterIndex = 1;

                if (dialog.ShowDialog() == DialogResult.OK)
                {
                    try
                    {
                        string photoDir = Path.Combine(Application.StartupPath, "product_photos");
                        if (!Directory.Exists(photoDir))
                            Directory.CreateDirectory(photoDir);

                        string fileName = Guid.NewGuid().ToString() + Path.GetExtension(dialog.FileName);
                        string destPath = Path.Combine(photoDir, fileName);

                        using (var img = Image.FromFile(dialog.FileName))
                        {
                            var resized = new Bitmap(img, new Size(300, 200));
                            resized.Save(destPath);
                        }

                        _selectedPhotoPath = Path.Combine("product_photos", fileName);
                        pbPhoto.Image = Image.FromFile(destPath);
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show($"Ошибка загрузки фото: {ex.Message}");
                    }
                }
            }
        }

        private void BtnSave_Click(object? sender, EventArgs e)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(txtArticle.Text) ||
                    string.IsNullOrWhiteSpace(txtName.Text) ||
                    string.IsNullOrWhiteSpace(txtPrice.Text))
                {
                    MessageBox.Show("Заполните обязательные поля");
                    return;
                }

                if (!_isEditMode)
                {
                    var existing = _productRepo.GetProductByArticle(txtArticle.Text);
                    if (existing != null)
                    {
                        MessageBox.Show("Товар с таким артикулом уже существует");
                        return;
                    }
                }

                var product = _isEditMode ? _product : new Product();
                if (product == null) return;

                product.Article = txtArticle.Text;
                product.ProductName = txtName.Text;
                product.CategoryId = (int)cmbCategory.SelectedValue;
                product.ManufacturerId = (int)cmbManufacturer.SelectedValue;
                product.SupplierId = (int)cmbSupplier.SelectedValue;
                product.Price = decimal.Parse(txtPrice.Text);
                product.Unit = txtUnit.Text;
                product.QuantityInStock = int.Parse(txtQuantity.Text);
                product.Discount = decimal.Parse(txtDiscount.Text);
                product.Description = txtDescription.Text;

                if (!string.IsNullOrEmpty(_selectedPhotoPath))
                {
                    if (_isEditMode && !string.IsNullOrEmpty(product.PhotoPath))
                    {
                        string oldPath = Path.Combine(Application.StartupPath, product.PhotoPath);
                        if (File.Exists(oldPath))
                            File.Delete(oldPath);
                    }
                    product.PhotoPath = _selectedPhotoPath;
                }

                if (_isEditMode)
                    _productRepo.UpdateProduct(product);
                else
                    _productRepo.AddProduct(product);

                DialogResult = DialogResult.OK;
                this.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка сохранения: {ex.Message}");
            }
        }
    }
}


////////////////////////////////////////////
//using Klimaitis.Models;
//using ShoeStore.Repositories;
//using System;
//using System.Drawing;
//using System.IO;
//using System.Linq;
//using System.Windows.Forms;

//namespace ShoeStore.Forms
//{
//    public partial class ProductEditForm : Form
//    {
//        private TextBox txtArticle;
//        private TextBox txtName;
//        private ComboBox cmbCategory;
//        private TextBox txtDescription;
//        private ComboBox cmbManufacturer;
//        private ComboBox cmbSupplier;
//        private TextBox txtPrice;
//        private TextBox txtUnit;
//        private TextBox txtQuantity;
//        private TextBox txtDiscount;
//        private PictureBox pbPhoto;
//        private Button btnSelectPhoto;
//        private Button btnSave;
//        private Button btnCancel;
//        private Label lblId;

//        private Product? _product;
//        private ProductRepository _productRepo;
//        private string _selectedPhotoPath;
//        private bool _isEditMode;

//        public ProductEditForm(Product? product)
//        {
//            _product = product;
//            _isEditMode = product != null;
//            _productRepo = new ProductRepository();
//            InitializeComponent();
//            SetupForm();
//            LoadComboBoxes();
//            if (_isEditMode)
//                LoadProductData();
//        }

//        //private void InitializeComponent()
//        //{
//        //    this.Text = _isEditMode ? "Редактирование товара" : "Добавление товара";
//        //    this.Size = new Size(500, 600);
//        //    this.StartPosition = FormStartPosition.CenterParent;
//        //    this.FormBorderStyle = FormBorderStyle.FixedDialog;
//        //    this.MaximizeBox = false;
//        //}

//        private void SetupForm()
//        {
//            int y = 20;
//            int labelX = 20;
//            int fieldX = 150;
//            int fieldWidth = 250;

//            // ID (только для чтения при редактировании)
//            if (_isEditMode)
//            {
//                Label lblIdText = new Label
//                {
//                    Text = "ID:",
//                    Location = new Point(labelX, y),
//                    Size = new Size(100, 25)
//                };

//                lblId = new Label
//                {
//                    Location = new Point(fieldX, y),
//                    Size = new Size(fieldWidth, 25),
//                    Text = _product?.ProductId.ToString()
//                };

//                this.Controls.Add(lblIdText);
//                this.Controls.Add(lblId);
//                y += 30;
//            }

//            // Артикул
//            Label lblArticle = new Label { Text = "Артикул:", Location = new Point(labelX, y), Size = new Size(100, 25) };
//            txtArticle = new TextBox { Location = new Point(fieldX, y), Size = new Size(fieldWidth, 25) };
//            this.Controls.Add(lblArticle);
//            this.Controls.Add(txtArticle);
//            y += 30;

//            // Наименование
//            Label lblName = new Label { Text = "Наименование:", Location = new Point(labelX, y), Size = new Size(100, 25) };
//            txtName = new TextBox { Location = new Point(fieldX, y), Size = new Size(fieldWidth, 25) };
//            this.Controls.Add(lblName);
//            this.Controls.Add(txtName);
//            y += 30;

//            // Категория
//            Label lblCategory = new Label { Text = "Категория:", Location = new Point(labelX, y), Size = new Size(100, 25) };
//            cmbCategory = new ComboBox
//            {
//                Location = new Point(fieldX, y),
//                Size = new Size(fieldWidth, 25),
//                DropDownStyle = ComboBoxStyle.DropDownList
//            };
//            this.Controls.Add(lblCategory);
//            this.Controls.Add(cmbCategory);
//            y += 30;

//            // Производитель
//            Label lblManufacturer = new Label { Text = "Производитель:", Location = new Point(labelX, y), Size = new Size(100, 25) };
//            cmbManufacturer = new ComboBox
//            {
//                Location = new Point(fieldX, y),
//                Size = new Size(fieldWidth, 25),
//                DropDownStyle = ComboBoxStyle.DropDownList
//            };
//            this.Controls.Add(lblManufacturer);
//            this.Controls.Add(cmbManufacturer);
//            y += 30;

//            // Поставщик
//            Label lblSupplier = new Label { Text = "Поставщик:", Location = new Point(labelX, y), Size = new Size(100, 25) };
//            cmbSupplier = new ComboBox
//            {
//                Location = new Point(fieldX, y),
//                Size = new Size(fieldWidth, 25),
//                DropDownStyle = ComboBoxStyle.DropDownList
//            };
//            this.Controls.Add(lblSupplier);
//            this.Controls.Add(cmbSupplier);
//            y += 30;

//            // Цена
//            Label lblPrice = new Label { Text = "Цена:", Location = new Point(labelX, y), Size = new Size(100, 25) };
//            txtPrice = new TextBox { Location = new Point(fieldX, y), Size = new Size(fieldWidth, 25) };
//            this.Controls.Add(lblPrice);
//            this.Controls.Add(txtPrice);
//            y += 30;

//            // Единица измерения
//            Label lblUnit = new Label { Text = "Ед. измерения:", Location = new Point(labelX, y), Size = new Size(100, 25) };
//            txtUnit = new TextBox { Location = new Point(fieldX, y), Size = new Size(fieldWidth, 25), Text = "шт." };
//            this.Controls.Add(lblUnit);
//            this.Controls.Add(txtUnit);
//            y += 30;

//            // Количество
//            Label lblQuantity = new Label { Text = "Количество:", Location = new Point(labelX, y), Size = new Size(100, 25) };
//            txtQuantity = new TextBox { Location = new Point(fieldX, y), Size = new Size(fieldWidth, 25) };
//            this.Controls.Add(lblQuantity);
//            this.Controls.Add(txtQuantity);
//            y += 30;

//            // Скидка
//            Label lblDiscount = new Label { Text = "Скидка %:", Location = new Point(labelX, y), Size = new Size(100, 25) };
//            txtDiscount = new TextBox { Location = new Point(fieldX, y), Size = new Size(fieldWidth, 25), Text = "0" };
//            this.Controls.Add(lblDiscount);
//            this.Controls.Add(txtDiscount);
//            y += 30;

//            // Описание
//            Label lblDescription = new Label { Text = "Описание:", Location = new Point(labelX, y), Size = new Size(100, 25) };
//            txtDescription = new TextBox
//            {
//                Location = new Point(fieldX, y),
//                Size = new Size(fieldWidth, 60),
//                Multiline = true
//            };
//            this.Controls.Add(lblDescription);
//            this.Controls.Add(txtDescription);
//            y += 70;

//            // Фото
//            Label lblPhoto = new Label { Text = "Фото:", Location = new Point(labelX, y), Size = new Size(100, 25) };
//            pbPhoto = new PictureBox
//            {
//                Location = new Point(fieldX, y),
//                Size = new Size(100, 100),
//                SizeMode = PictureBoxSizeMode.Zoom,
//                BorderStyle = BorderStyle.FixedSingle,
//                BackColor = Color.White
//            };

//            btnSelectPhoto = new Button
//            {
//                Text = "Выбрать фото",
//                Location = new Point(fieldX + 110, y + 35),
//                Size = new Size(130, 30)
//            };
//            btnSelectPhoto.Click += BtnSelectPhoto_Click;

//            this.Controls.Add(lblPhoto);
//            this.Controls.Add(pbPhoto);
//            this.Controls.Add(btnSelectPhoto);
//            y += 110;

//            // Кнопки
//            btnSave = new Button
//            {
//                Text = "Сохранить",
//                Location = new Point(150, y),
//                Size = new Size(100, 35),
//                BackColor = Color.FromArgb(161, 99, 245),
//                ForeColor = Color.White,
//                FlatStyle = FlatStyle.Flat
//            };
//            btnSave.Click += BtnSave_Click;

//            btnCancel = new Button
//            {
//                Text = "Отмена",
//                Location = new Point(260, y),
//                Size = new Size(100, 35)
//            };
//            btnCancel.Click += (s, e) => this.Close();

//            this.Controls.Add(btnSave);
//            this.Controls.Add(btnCancel);



//        }

//        private void LoadComboBoxes()
//        {
//            cmbCategory.DataSource = _productRepo.GetAllCategories();
//            cmbCategory.DisplayMember = "CategoryName";
//            cmbCategory.ValueMember = "CategoryId";

//            cmbManufacturer.DataSource = _productRepo.GetAllManufacturers();
//            cmbManufacturer.DisplayMember = "ManufacturerName";
//            cmbManufacturer.ValueMember = "ManufacturerId";

//            cmbSupplier.DataSource = _productRepo.GetAllSuppliers();
//            cmbSupplier.DisplayMember = "SupplierName";
//            cmbSupplier.ValueMember = "SupplierId";
//        }

//        private void LoadProductData()
//        {
//            if (_product == null) return;

//            txtArticle.Text = _product.Article;
//            txtName.Text = _product.ProductName;
//            cmbCategory.SelectedValue = _product.CategoryId;
//            cmbManufacturer.SelectedValue = _product.ManufacturerId;
//            cmbSupplier.SelectedValue = _product.SupplierId;
//            txtPrice.Text = _product.Price.ToString();
//            txtUnit.Text = _product.Unit;
//            txtQuantity.Text = _product.QuantityInStock.ToString();
//            txtDiscount.Text = _product.Discount.ToString();
//            txtDescription.Text = _product.Description;

//            if (!string.IsNullOrEmpty(_product.PhotoPath))
//            {
//                string fullPath = Path.Combine(Application.StartupPath, _product.PhotoPath);
//                if (File.Exists(fullPath))
//                {
//                    pbPhoto.Image = Image.FromFile(fullPath);
//                }
//            }
//        }

//        private void BtnSelectPhoto_Click(object? sender, EventArgs e)
//        {
//            using (OpenFileDialog dialog = new OpenFileDialog())
//            {
//                dialog.Filter = "Image files|*.jpg;*.jpeg;*.png;*.bmp";
//                dialog.FilterIndex = 1;

//                if (dialog.ShowDialog() == DialogResult.OK)
//                {
//                    try
//                    {
//                        // Создаем папку для фото если её нет
//                        string photoDir = Path.Combine(Application.StartupPath, "product_photos");
//                        if (!Directory.Exists(photoDir))
//                            Directory.CreateDirectory(photoDir);

//                        // Генерируем имя файла
//                        string fileName = Guid.NewGuid().ToString() + Path.GetExtension(dialog.FileName);
//                        string destPath = Path.Combine(photoDir, fileName);

//                        // Копируем и изменяем размер
//                        using (var img = Image.FromFile(dialog.FileName))
//                        {
//                            var resized = new Bitmap(img, new Size(300, 200));
//                            resized.Save(destPath);
//                        }

//                        _selectedPhotoPath = Path.Combine("product_photos", fileName);
//                        pbPhoto.Image = Image.FromFile(destPath);
//                    }
//                    catch (Exception ex)
//                    {
//                        MessageBox.Show($"Ошибка загрузки фото: {ex.Message}");
//                    }
//                }
//            }
//        }

//        private void BtnSave_Click(object? sender, EventArgs e)
//        {
//            try
//            {
//                // Проверка обязательных полей
//                if (string.IsNullOrWhiteSpace(txtArticle.Text) ||
//                    string.IsNullOrWhiteSpace(txtName.Text) ||
//                    string.IsNullOrWhiteSpace(txtPrice.Text))
//                {
//                    MessageBox.Show("Заполните обязательные поля");
//                    return;
//                }

//                // Проверка артикула на уникальность
//                if (!_isEditMode)
//                {
//                    var existing = _productRepo.GetProductByArticle(txtArticle.Text);
//                    if (existing != null)
//                    {
//                        MessageBox.Show("Товар с таким артикулом уже существует");
//                        return;
//                    }
//                }

//                var product = _isEditMode ? _product : new Product();

//                if (product == null) return;

//                product.Article = txtArticle.Text;
//                product.ProductName = txtName.Text;
//                product.CategoryId = (int)cmbCategory.SelectedValue;
//                product.ManufacturerId = (int)cmbManufacturer.SelectedValue;
//                product.SupplierId = (int)cmbSupplier.SelectedValue;
//                product.Price = decimal.Parse(txtPrice.Text);
//                product.Unit = txtUnit.Text;
//                product.QuantityInStock = int.Parse(txtQuantity.Text);
//                product.Discount = decimal.Parse(txtDiscount.Text);
//                product.Description = txtDescription.Text;

//                if (!string.IsNullOrEmpty(_selectedPhotoPath))
//                {
//                    // Удаляем старое фото если есть
//                    if (_isEditMode && !string.IsNullOrEmpty(product.PhotoPath))
//                    {
//                        string oldPath = Path.Combine(Application.StartupPath, product.PhotoPath);
//                        if (File.Exists(oldPath))
//                            File.Delete(oldPath);
//                    }
//                    product.PhotoPath = _selectedPhotoPath;
//                }

//                if (_isEditMode)
//                    _productRepo.UpdateProduct(product);
//                else
//                    _productRepo.AddProduct(product);

//                DialogResult = DialogResult.OK;
//                this.Close();
//            }
//            catch (Exception ex)
//            {
//                MessageBox.Show($"Ошибка сохранения: {ex.Message}");
//            }
//        }
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
////    public partial class ProductEditForm : Form
////    {
////        public ProductEditForm()
////        {
////            InitializeComponent();
////        }
////    }
////}
