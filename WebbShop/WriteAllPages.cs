﻿using Microsoft.EntityFrameworkCore.Metadata.Conventions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebbShop.Model;

namespace WebbShop
{
    internal class WriteAllPages
    {
        static public void WriteOutPages( )  // Classen funkar, skulle vilja fixa mer med visual men men 
        {
            Console.Clear();
            using (var myDb = new MyDbContext())
            {
                List<string> product = new List<string>();

                var groupProduct = myDb.products
                    .GroupBy(p => p.ProductName)
                    .Select(g => new
                    {
                        ProductName = g.Key,
                        Id = g.First().Id
                    })
                    .ToList();

                int pageSize = 6;   // Hur många produkter som ska vissas
                
                // Få Max antal sidor
                int totalProducts = groupProduct.Count;
                int totalPages = (int)Math.Ceiling((double)totalProducts / pageSize);
                int start = 1;
                DataTracker.SetPageNumber(start);
               
                while (DataTracker.GetRunPage()) 
                {
                    Console.Clear();
                    int page = DataTracker.GetPageNumber(); 
                    

                    // få enbart de produkter som ska vissa på sidan
                    var pageProducts = groupProduct
                        .Skip((page - 1) * pageSize)  //skip tar bort de första så vi inte bevarar info från tidigare sidor
                        .Take(pageSize)
                        .ToList();

                    //tömma listan så den blir tom mellan sidorna


                    int[,] positions =
                        {
                            {20, 8, 0,0},   // 1
                            {58, 8,0,0},   // 2
                            {95, 8,0,0},  // 3
                            {20, 14,0,0},  // 4
                            {58, 14,0,0},  // 5
                            {95, 14,0,0}  // 6
                        };
                    for (int i = 0; i < pageProducts.Count(); i++)
                    {
                        var id = pageProducts[i];
                        var selectedProduct = myDb.products.Where(p => p.Id == id.Id).SingleOrDefault();

                        string cash = selectedProduct.Price.ToString();

                        var products = myDb.products.Where(p => p.ProductName == selectedProduct.ProductName).ToList();

                        var brands = myDb.brands;
                        var brandName = (from p in products
                                         join b in brands
                                         on p.Brand equals b.Id
                                         where p.Id == selectedProduct.Id
                                         select b.Name)
                                         .FirstOrDefault();


                        product.Add(selectedProduct.ProductName);
                        product.Add(brandName);
                        product.Add($"{cash}");

                        // position för lådor, 

                        positions[i, 2] = id.Id;
                        positions[i, 3] = selectedProduct.ProductGroup;

                        var productwindow = new Window("Product " + (i + 1), positions[i, 0], positions[i, 1], product);
                        productwindow.Draw();
                        Helpers.TopBarBox();
                        Helpers.WriteCart();
                        Helpers.UserBox();
                        product.Clear();

                        Console.WriteLine($"                           Page {page} of {totalPages}");


                        // Användare input

                        if (i == pageProducts.Count()-1)
                        {
                            
                            if (DataTracker.GetIsAdmin() == true)
                            {
                                if (DataTracker.GetAddProduct() == false)
                                {   
                                    Helpers.OptionsForAdminPages(positions, totalPages);
                                }
                                else if (DataTracker.GetAddProduct() == true) 
                                {
                                    Helpers.OptionsAddProductAdmin(positions,totalPages);
                                }
                            }
                            else if(DataTracker.GetIsAdmin() == false)
                            { Helpers.OptionsForPages( positions, totalPages);}
                            
                        }
                    }
                }
            }
        }
    }
}
