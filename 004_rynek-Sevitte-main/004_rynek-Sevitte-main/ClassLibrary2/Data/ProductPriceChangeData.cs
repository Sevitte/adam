using ClassLibrary2.Products;
using System;
using System.Collections.Generic;
using System.Text;

namespace ClassLibrary2.Data
{
    public struct ProductPriceChangeData
    {
        public Guid ProductId;
        public decimal PreviousPrice;
        public decimal NewPrice;
    }
}
