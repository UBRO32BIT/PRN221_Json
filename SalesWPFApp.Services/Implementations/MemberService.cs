using BusinessObject.Entities;
using SalesWPFApp.DAOs;
using SalesWPFApp.Repositories.Implementations;
using SalesWPFApp.Repositories.Interfaces;
using SalesWPFApp.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SalesWPFApp.Services.Implementations
{
    public class MemberService : IMemberService
    {
        private readonly IMemberRepository _memberRepository;
        public MemberService()
        {
            _memberRepository = new MemberRepository();
        }
        public Member? GetMemberById(int id) => _memberRepository.GetMemberById(id);
        public Member? GetMemberByEmail(string email) => _memberRepository.GetMemberByEmail(email);
        public Member? GetMemberByEmailAndPassword(string email, string password) => _memberRepository.GetMemberByEmailAndPassword(email, password);
    }
}
