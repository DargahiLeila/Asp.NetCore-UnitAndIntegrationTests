using DomainModel.Public;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DomainModel.DTO.UserModel
{
    public class UserSearchModel: PageModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
    }
}

