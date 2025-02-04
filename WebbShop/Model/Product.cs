﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace WebbShop.Model
{
    internal class Product
    {
        public int Id { get; set; }

        public string ProductName { get; set; }

        public string Category {  get; set; }

        public int Price { get; set; }

        public string Gender { get; set; }

        public int[] ColorId { get; set; }

        public string Size { get; set; }

        public string Description { get; set; }

        public int Brand { get; set; }

        public bool EnviromentFriendly { get; set; }

        public int ProductGroup { get; set; }

        public bool CanBeBought { get; set; } = true;

        public  int CompanyBuyInPrice { get; set; }

    }        

}
