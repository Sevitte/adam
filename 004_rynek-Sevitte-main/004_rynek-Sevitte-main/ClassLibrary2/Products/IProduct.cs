using System;
using System.Collections.Generic;
using System.Text;

namespace ClassLibrary2.Products
{
    public interface IProduct
    {
        public Guid Id { get; }
        public string Name { get; }
        public decimal Value { get; }
    }
}
