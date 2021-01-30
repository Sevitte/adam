using ClassLibrary2.Products;
using System;
using System.Collections.Generic;
using System.Text;

namespace ClassLibrary2
{
    public class ProductOrder
    {
        public ProductOrder(IProduct product, decimal profitMargin)
        {
            Product = product ?? throw new ArgumentNullException(nameof(product));
            if (profitMargin < 0)
                throw new ArgumentException("Profit margin can't be less than zero.", nameof(profitMargin));
            ProfitMargin = profitMargin;
        }

        public IProduct Product { get; }
        public decimal ProfitMargin { get; set; }

        public decimal TotalValue => checked(Product.Value + (Product.Value * ProfitMargin));
    }
}
