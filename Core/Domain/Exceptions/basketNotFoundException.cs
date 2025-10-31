using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Exceptions
{
    public class basketNotFoundException(string Id) : NotFoundException($"Basket with Id {Id} is not Found!")
    {
    }
}
