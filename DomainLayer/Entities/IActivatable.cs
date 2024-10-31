using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DomainLayer.Entities
{
    public interface IActivatable
    {
        int Id { get; set; }
        bool IsActive { get; set; }
        bool IsDeleted { get; set; }
    }
}
