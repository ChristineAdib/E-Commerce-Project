using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using Microsoft.Web.WebView2.Core;
using Microsoft.Web.WebView2.WinForms;
using Application.Services;
using Application.DTOs.ProductDTOs;
using Infrastructure.Data;
using Infrastructure.Repositories;
using System.Text.Json;
using System.IO;
using Domain.Entities;
using Application.Interfaces.Services.Category_servises;
using Application.Interfaces.Repository.Ctegory_Repo;

namespace Presentation
{
    public partial class MainForm : Form
    {
        private WebView2 webView;
        private readonly ProductService _productService;
        private readonly ICategoryService _categoryService;
        private bool _isNavigationComplete = false;

        public MainForm()
        {
            InitializeComponent();
            
            // Initialize Dependencies
            var context = new ApplicationDbContext();
            var repo = new ProductRepository(context);
            _productService = new ProductService(repo);
            
            var categoryRepo = new Infrastructure.Repositories.CategoryRepository(context);
            _categoryService = new CategoryService(categoryRepo);

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
            webView.CoreWebView2.NavigationCompleted += (s, e) => _isNavigationComplete = true;
            
            // Set up path to local HTML files
            string baseDir = AppDomain.CurrentDomain.BaseDirectory;
            string htmlPath = Path.Combine(baseDir, "WebUI", "products.html");
            
            if (!File.Exists(htmlPath))
            {
                string? current = baseDir;
                while (current != null && !Directory.Exists(Path.Combine(current, "WebUI")))
                {
                    current = Directory.GetParent(current)?.FullName;
                }
                if (current != null)
                {
                    htmlPath = Path.Combine(current, "WebUI", "products.html");
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

                    case "GET_ALL_CATEGORIES":
                        if (!_isNavigationComplete) return;
                        var categories = _categoryService.GetAll();
                        var categoryList = categories.Select(c => new { id = c.Id, name = c.CategoryName }).ToList();
                        string categoriesJson = JsonSerializer.Serialize(categoryList);
                        webView.CoreWebView2.ExecuteScriptAsync($"window.receiveCategories && window.receiveCategories({categoriesJson});");
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
            webView.ExecuteScriptAsync($"if (window.onMessageReceived) window.onMessageReceived({json});");
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
    }

    public class WebMessage
    {
        public string Type { get; set; }
        public object Data { get; set; }
    }
}
