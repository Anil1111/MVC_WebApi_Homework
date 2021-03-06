﻿using Microsoft.AspNetCore.Mvc;
using Products.App.Models.ViewModels;
using Products.App.Models.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Products.App.DB;
using System.ComponentModel;

namespace Products.App.Controllers
{
    public class ProductController : Controller
    {
        
        //Creating product should have same route for Get and Post HttpMethod

        [HttpGet("CreateProduct")]
        public IActionResult CreateProduct(string error)
        {
            ViewBag.Error = error;
            return View();
        }

        //When the user creates product should be redirected to see the products list and see an appropriate message if the product is created or not

        [HttpPost("CreateProduct")]
        public IActionResult CreateProduct(CreateProductListVM createProduct)
        {
            
            var product = new Product()
            {
                Id = DataBase.Products.Count + 1,
                Name = createProduct.Name,
                Price = createProduct.Price,
                Description = createProduct.Description,
                Category = createProduct.Category

            };

            if (string.IsNullOrWhiteSpace(product.Name))
            {
                return RedirectToAction("CreateProduct", new { error = "Product name is required!" });
            }

            if (string.IsNullOrWhiteSpace(product.Description))
            {
                return RedirectToAction("CreateProduct", new { error = "Description is required!" });
            }
           
            if (product.Price == 0)
            {
                return RedirectToAction("CreateProduct", new { error = "Price is required!" });
            }

            DataBase.Products.Add(product);
            return RedirectToAction("Index", "Home", new { message = "Product was created!" });
            
        }

        //The user should be able to see more details about a product

        [HttpGet("ProductDetails/{id:int}")]
        public IActionResult ProductDetails(int id)
        {

            var productVM = DataBase.Products.Select(p => new ProductVM { Id = p.Id, 
                                                                         Name = p.Name, 
                                                                         Description = p.Description, 
                                                                         Price = p.Price, 
                                                                         Category = p.Category});

            ProductVM productVM2 = productVM.SingleOrDefault(p => p.Id == id);
            
            return View("_PartialProductDetails", productVM2);
        }


    }
}
