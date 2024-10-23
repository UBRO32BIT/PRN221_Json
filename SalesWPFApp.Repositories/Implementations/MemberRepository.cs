using BusinessObject.Entities;
using SalesWPFApp.DAOs;
using SalesWPFApp.Repositories.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SalesWPFApp.Repositories.Implementations
{
    public class MemberRepository : IMemberRepository
    {
        public Member? GetMemberById(int id) => MemberDAO.Instance.GetMemberById(id);
        public Member? GetMemberByEmail(string email) => MemberDAO.Instance.GetMemberByEmail(email);
        public Member? GetMemberByEmailAndPassword(string email, string password) => MemberDAO.Instance.GetMemberByEmailAndPassword(email, password);
    }
}
