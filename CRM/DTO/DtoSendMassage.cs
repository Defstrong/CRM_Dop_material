using Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DTO
{
    public sealed class DtoSendMassage
    {
        public Guid Id { get; set; }
        public string FirstName { get; set; }
        public Roles Role { get; set; }
        public string Text { get; set; }
        public string Theme { get; set; }
        public string AdminOrManager { get; set; }
    }
}
