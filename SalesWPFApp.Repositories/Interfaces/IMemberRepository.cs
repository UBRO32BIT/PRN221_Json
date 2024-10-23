using BusinessObject.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SalesWPFApp.Repositories.Interfaces
{
    public interface IMemberRepository
    {
        public Member GetMemberById(int id);
        public Member GetMemberByEmail(string email);
        public Member GetMemberByEmailAndPassword(string email, string password);
    }
}
