using System;
using System.Collections.Generic;
using System.Text;

namespace ClassLibrary2.Products
{
    public abstract class Product : IProduct
    {
        protected Product(Guid id, string name, decimal value)
        {
            Id = id;
            Name = name;
            Value = value;
        }

        public Guid Id { get; }
        public string Name { get; }
        public decimal Value { get; }
    }
}
