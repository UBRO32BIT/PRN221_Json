using BusinessObject.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SalesWPFApp.DAOs
{
    public class MemberDAO
    {
        private FstoreContext _context;
        private static MemberDAO? _instance;
        public MemberDAO()
        {
            _context = new FstoreContext();
        }
        public static MemberDAO Instance
        {
            get => _instance ?? new MemberDAO();
        }

        public Member? GetMemberById(int id)
        {
            return _context.Members.SingleOrDefault(m => m.MemberId == id);
        }

        public Member? GetMemberByEmail(string email)
        {
            return _context.Members.SingleOrDefault(m => m.Email == email);
        }

        public Member? GetMemberByEmailAndPassword(string email, string password)
        {
            return _context.Members
                       .Include(u => u.Role)
                       .SingleOrDefault(u => u.Email == email && u.Password == password);
        }

        public List<Member> GetMembers()
        {
            return _context.Members.ToList();
        }

        public bool AddMember(Member member)
        {
            bool result = false;
            try
            {
                _context.Members.Add(member);
                _context.SaveChanges();
                result = true;
            }
            catch (Exception e)
            {
                
            }
            return result;
        }
    }
}
