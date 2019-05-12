using System;
using System.Collections.Generic;
using System.Text;

namespace CVApp.Models
{
    public abstract class BaseModel<T>
    {
        public T Id { get; set; }
    }
}
