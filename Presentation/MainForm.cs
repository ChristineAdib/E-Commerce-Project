using Application.DTOs.OrderDTOs;
using Application.DTOs.ProductDTOs;
using Application.DTOs.UserDTOs;
using Application.Interfaces.Repository.Cart_Repo;
using Application.Interfaces.Repository.Ctegory_Repo;
using Application.Interfaces.Repository.Product_Repo;
using Application.Interfaces.Repository.User_Repo;
using Application.Interfaces.Services.Cart_services;
using Application.Interfaces.Services.Category_servises;
using Application.Interfaces.Services.Order_servises;
using Application.Interfaces.Services.User_services;
using Application.Services;
using Domain.Entities;
using Infrastructure.Data;
using Infrastructure.Repositories;
using Microsoft.Web.WebView2.Core;
using Microsoft.Web.WebView2.WinForms;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Presentation
{
    public partial class MainForm : Form
    {
        private WebView2 webView;
        private readonly ProductService _productService;
        private readonly ICategoryService _categoryService;
        private readonly ICartRepository _cartRepo;
        private readonly IUserServices _userService;
        private readonly ICartServices _cartService;
        private readonly IOrderService _orderService;
        private bool _isNavigationComplete = false;
        
        // Session Management
        private int? _currentUserId = null;
        private bool _isAdmin = false;

        public MainForm()
        {
            InitializeComponent();
            
            // Initialize Dependencies
            var context = new ApplicationDbContext();
            var repo = new ProductRepository(context);
            _productService = new ProductService(repo);
            
            var categoryRepo = new Infrastructure.Repositories.CategoryRepository(context);
            _categoryService = new CategoryService(categoryRepo);

            _cartRepo = new Infrastructure.Repositories.CartRepository(context);
            var cartItemRepo = new Infrastructure.Repositories.CartItemRepository(context);
            var userRepo = new Infrastructure.Repositories.UserRepository(context);
            _cartService = new CartService(_cartRepo, cartItemRepo, userRepo, repo);
            var orderRepo = new Infrastructure.Repositories.OrderRepository(context);
            _orderService = new OrderService(orderRepo, repo);
            _userService = new UserService(userRepo, _cartRepo);

            InitializeWebView();
        }

        private async void InitializeWebView()
        {
            webView = new WebView2
            {
                Dock = DockStyle.Fill
            };
            this.Controls.Add(webView);

            await webView.EnsureCoreWebView2Async(null);
            
            webView.CoreWebView2.WebMessageReceived += CoreWebView2_WebMessageReceived;
            webView.CoreWebView2.NavigationStarting += CoreWebView2_NavigationStarting;
            webView.CoreWebView2.NavigationCompleted += (s, e) => _isNavigationComplete = true;
            
            // Set up path to local HTML files
            string baseDir = AppDomain.CurrentDomain.BaseDirectory;
            string htmlPath = Path.Combine(baseDir, "WebUI", "login.html");
            
            if (!File.Exists(htmlPath))
            {
                string? current = baseDir;
                while (current != null && !Directory.Exists(Path.Combine(current, "WebUI")))
                {
                    current = Directory.GetParent(current)?.FullName;
                }
                if (current != null)
                {
                    htmlPath = Path.Combine(current, "WebUI", "login.html");
                }
            }

            if (File.Exists(htmlPath))
            {
                webView.Source = new Uri(htmlPath);
            }
        }

        private async void CoreWebView2_WebMessageReceived(object sender, CoreWebView2WebMessageReceivedEventArgs e)
        {
            try
            {
                var jsonOptions = new JsonSerializerOptions { PropertyNameCaseInsensitive = true };
                var message = JsonSerializer.Deserialize<WebMessage>(e.WebMessageAsJson, jsonOptions);
                if (message == null) return;

                switch (message.Type)
                {
                    case "GET_ALL_PRODUCTS":
                        var products = await _productService.GetAllProductsAsync();
                        SendToJS("RECEIVE_ALL_PRODUCTS", products);
                        break;

                    case "GET_PRODUCT_DETAILS":
                        if (message.Data is JsonElement element && element.ValueKind != JsonValueKind.Null)
                        {
                            int id = int.Parse(element.ToString());
                            var product = await _productService.GetProductByIdAsync(id);
                            SendToJS("RECEIVE_PRODUCT_DETAILS", product);
                        }
                        break;

                    case "DELETE_PRODUCT":
                        if (message.Data is JsonElement delElement && delElement.ValueKind != JsonValueKind.Null)
                        {
                            int id = int.Parse(delElement.ToString());
                            await _productService.DeleteProductAsync(id);
                            SendToJS("PRODUCT_DELETED", new { Success = true, Id = id });
                        }
                        break;

                    case "CREATE_PRODUCT":
                        if (message.Data is JsonElement createEl)
                        {
                            var createDto = JsonSerializer.Deserialize<CreateProductDto>(createEl.GetRawText(), jsonOptions);
                            if (createDto != null)
                            {
                                await _productService.CreateProductAsync(createDto);
                                SendToJS("PRODUCT_CREATED", new { Success = true });
                            }
                        }
                        break;
                    case "PLACE_ORDER":
                        if (_currentUserId == null || _isAdmin) return;

                        var cart = await _cartRepo.GetCartByUserIdAsync(_currentUserId.Value);
                        if (cart == null || cart.CartItems == null || !cart.CartItems.Any()) return;

                        var createOrderDto = new CreateOrderDto
                        {
                            UserId = _currentUserId.Value,
                            Items = cart.CartItems.Select(ci => new CreateOrderItemDto
                            {
                                ProductId = ci.ProductId,
                                Quantity = ci.Quantity
                            }).ToList()
                        };

                        await _orderService.CreateOrderAsync(createOrderDto);

                        await _cartService.ClearCartAsync(_currentUserId.Value);
                        await SendUpdatedCartToFrontend();

                        break;

                    case "UPDATE_PRODUCT":
                        if (message.Data is JsonElement updateEl)
                        {
                            var updateDto = JsonSerializer.Deserialize<UpdateProductDto>(updateEl.GetRawText(), jsonOptions);
                            if (updateDto != null)
                            {
                                await _productService.UpdateProductAsync(updateDto);
                                SendToJS("PRODUCT_UPDATED", new { Success = true });
                            }
                        }
                        break;

                    case "PICK_IMAGE":
                        var relativePath = HandlePickImage();
                        if (relativePath != null)
                        {
                            SendToJS("RECEIVE_IMAGE_PATH", new { Path = relativePath });
                        }
                        break;
                    case "GET_USER_ORDERS":
                        if (_currentUserId == null) return;

                        var orders = await _orderService.GetOrdersByUserAsync(_currentUserId.Value);
                        SendToJS("RECEIVE_USER_ORDERS", orders);
                        break;

                    case "GET_ALL_CATEGORIES":
                        if (!_isNavigationComplete) return;
                        var categories = _categoryService.GetAll();
                        var categoryList = categories.Select(c => new { id = c.Id, name = c.CategoryName }).ToList();
                        string categoriesJson = JsonSerializer.Serialize(categoryList);
                        webView.CoreWebView2.ExecuteScriptAsync($"window.receiveCategories && window.receiveCategories({categoriesJson});");
                        break;

                    case "GET_CART":
                        if (_currentUserId == null) return;
                        await SendUpdatedCartToFrontend();
                        break;

                    case "ADD_TO_CART":
                        if (_currentUserId == null || _isAdmin) return;
                        if (message.Data is JsonElement addElement)
                        {
                            var productId = addElement.GetProperty("productId").GetInt32();
                            var quantity = addElement.GetProperty("quantity").GetInt32();
                            await _cartService.AddToCartAsync(_currentUserId.Value, productId, quantity);
                            await SendUpdatedCartToFrontend();
                        }
                        break;

                    case "UPDATE_CART_ITEM":
                        if (_currentUserId == null || _isAdmin) return;
                        if (message.Data is JsonElement updateElement)
                        {
                            var productId = updateElement.GetProperty("productId").GetInt32();
                            var quantity = updateElement.GetProperty("quantity").GetInt32();
                            await _cartService.UpdateQuantityAsync(_currentUserId.Value, productId, quantity);
                            await SendUpdatedCartToFrontend();
                        }
                        break;

                    case "REMOVE_FROM_CART":
                        if (_currentUserId == null || _isAdmin) return;
                        if (message.Data is JsonElement removeElement)
                        {
                            var productId = removeElement.GetProperty("productId").GetInt32();
                            await _cartService.RemoveFromCartAsync(_currentUserId.Value, productId);
                            await SendUpdatedCartToFrontend();
                        }
                        break;

                

                    case "LOGIN":
                        if (message.Data is JsonElement loginEl)
                        {
                            var loginData = JsonSerializer.Deserialize<Dictionary<string, string>>(loginEl.GetRawText());
                            if (loginData != null && loginData.TryGetValue("username", out var userN) && loginData.TryGetValue("password", out var pass))
                            {
                                var user = await _userService.Login(userN, pass);
                                if (user != null)
                                {
                                    _currentUserId = user.Id;
                                    _isAdmin = user.IsAdmin;
                                    string target = _isAdmin ? "admin.html" : "products.html";
                                    NavigateTo(target);
                                }
                                else
                                {
                                    SendToJS("LOGIN_ERROR", new { Message = "Invalid username or password" });
                                }
                            }
                        }
                        break;

                    case "REGISTER":
                        if (message.Data is JsonElement regEl)
                        {
                            var regDto = JsonSerializer.Deserialize<AddUserDto>(regEl.GetRawText(), jsonOptions);
                            if (regDto != null)
                            {
                                await _userService.Register(regDto);
                                // Auto login
                                var user = await _userService.Login(regDto.UserName, regDto.Password);
                                if (user != null)
                                {
                                    _currentUserId = user.Id;
                                    _isAdmin = user.IsAdmin;
                                    NavigateTo("products.html");
                                }
                            }
                        }
                        break;

                    case "LOGOUT":
                        _currentUserId = null;
                        _isAdmin = false;
                        NavigateTo("login.html");
                        break;
                }
            }
            catch (Exception ex)
            {
                SendToJS("ERROR", new { Message = ex.Message });
            }
        }

        private void SendToJS(string type, object data)
        {
            var response = new { Type = type, Data = data };
            string json = JsonSerializer.Serialize(response);
            if (webView.CoreWebView2 != null)
            {
                webView.CoreWebView2.ExecuteScriptAsync($"if (window.onMessageReceived) window.onMessageReceived({json});");
            }
        }

        private void NavigateTo(string pageName)
        {
            string baseDir = AppDomain.CurrentDomain.BaseDirectory;
            string htmlPath = Path.Combine(baseDir, "WebUI", pageName);

            if (!File.Exists(htmlPath))
            {
                string? current = baseDir;
                while (current != null && !Directory.Exists(Path.Combine(current, "WebUI")))
                {
                    current = Directory.GetParent(current)?.FullName;
                }
                if (current != null)
                {
                    htmlPath = Path.Combine(current, "WebUI", pageName);
                }
            }

            if (File.Exists(htmlPath))
            {
                webView.CoreWebView2.Navigate(new Uri(htmlPath).ToString());
            }
        }

        private void CoreWebView2_NavigationStarting(object sender, CoreWebView2NavigationStartingEventArgs e)
        {
            string uri = e.Uri.ToLower();
            
            // Allow login and register always
            if (uri.Contains("login.html") || uri.Contains("register.html")) return;

            // Security: Not logged in
            if (_currentUserId == null)
            {
                e.Cancel = true;
                NavigateTo("login.html");
                return;
            }

            // Role Security
            if (_isAdmin)
            {
                // Admin trying to open customer pages
                if (uri.Contains("cart.html") || uri.Contains("checkout.html"))
                {
                    e.Cancel = true;
                    NavigateTo("admin.html");
                }
            }
            else
            {
                // Customer trying to open admin pages
                if (uri.Contains("admin.html") || uri.Contains("admin-product-form.html"))
                {
                    e.Cancel = true;
                    NavigateTo("products.html");
                }
            }
        }

        private string HandlePickImage()
        {
            using (OpenFileDialog openFileDialog = new OpenFileDialog())
            {
                openFileDialog.Filter = "Image Files|*.jpg;*.jpeg;*.png;*.webp";
                openFileDialog.Title = "Select a Product Image";

                if (openFileDialog.ShowDialog() == DialogResult.OK)
                {
                    try
                    {
                        string sourceFile = openFileDialog.FileName;
                        string fileName = Path.GetFileName(sourceFile);
                        
                        // Find WebUI directory
                        string baseDir = AppDomain.CurrentDomain.BaseDirectory;
                        string webUIDir = Path.Combine(baseDir, "WebUI");
                        
                        if (!Directory.Exists(webUIDir))
                        {
                            string? current = baseDir;
                            while (current != null && !Directory.Exists(Path.Combine(current, "WebUI")))
                            {
                                current = Directory.GetParent(current)?.FullName;
                            }
                            if (current != null)
                            {
                                webUIDir = Path.Combine(current, "WebUI");
                            }
                        }

                        string imagesDir = Path.Combine(webUIDir, "images");
                        if (!Directory.Exists(imagesDir))
                        {
                            Directory.CreateDirectory(imagesDir);
                        }

                        string targetPath = Path.Combine(imagesDir, fileName);
                        
                        // To handle duplicate filenames, we could add a timestamp or guid if needed, 
                        // but requirements just say copy to directory.
                        File.Copy(sourceFile, targetPath, true);

                        return $"images/{fileName}";
                    }
                    catch (Exception ex)
                    {
                        SendToJS("ERROR", new { Message = "Failed to copy image: " + ex.Message });
                    }
                }
            }
            return null;
        }

        private async Task SendUpdatedCartToFrontend()
        {
            try
            {
                if (_currentUserId == null) return;
                var cart = await _cartRepo.GetCartByUserIdAsync(_currentUserId.Value);
                if (cart == null) return;

                var items = cart.CartItems ?? new List<CartItem>();
                var projectedItems = items.Select(item => new
                {
                    productId = item.ProductId,
                    name = item.Product?.ProductName ?? "Unknown Product",
                    price = item.Product?.Price ?? 0,
                    quantity = item.Quantity,
                    imageUrl = item.Product?.ImageUrl,
                    subtotal = (item.Product?.Price ?? 0) * item.Quantity
                }).ToList();

                var response = new
                {
                    items = projectedItems,
                    totalQuantity = projectedItems.Count,
                    totalPrice = items.Sum(item => (item.Product?.Price ?? 0) * item.Quantity)
                };

                string json = JsonSerializer.Serialize(response);
                await webView.CoreWebView2.ExecuteScriptAsync($"if (window.receiveCart) window.receiveCart({json});");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error sending cart to JS: {ex.Message}");
            }
        }
    }

    public class WebMessage
    {
        public string Type { get; set; }
        public object Data { get; set; }
    }
}
