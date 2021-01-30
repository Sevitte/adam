using System;
using System.Collections.Generic;
using System.Text;

namespace ClassLibrary2.Products
{
    public class Banana : Product
    {
        public Banana(Guid id, string name, decimal value) : base(id, name, value)
        {
        }
    }
}
