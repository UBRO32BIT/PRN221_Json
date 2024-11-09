using BusinessObject.Entities;
using Microsoft.EntityFrameworkCore;
using BCrypt.Net;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Text.Json;

namespace SalesWPFApp.DAOs
{
    public class MemberDAO
    {
        private static MemberDAO? _instance;
        private readonly string _jsonFilePath = "members.json";

        public static MemberDAO Instance => _instance ??= new MemberDAO();

        private MemberDAO()
        {
            try
            {
                if (!File.Exists(_jsonFilePath))
                {
                    File.WriteAllText(_jsonFilePath, "[]");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error initializing file: {ex.Message}");
                throw;
            }
        }

        private List<Member> ReadMembersFromFile()
        {
            try
            {
                var jsonData = File.ReadAllText(_jsonFilePath);
                var members = JsonSerializer.Deserialize<List<Member>>(jsonData) ?? new List<Member>();

                var roles = RoleDAO.Instance.GetAllRoles();
                foreach (var member in members)
                {
                    member.Role = roles.SingleOrDefault(r => r.RoleId == member.RoleId);
                }

                return members;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error reading members from file: {ex.Message}");
                return new List<Member>();
            }
        }

        private void WriteMembersToFile(List<Member> members)
        {
            try
            {
                var jsonData = JsonSerializer.Serialize(members, new JsonSerializerOptions { WriteIndented = true });
                File.WriteAllText(_jsonFilePath, jsonData);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error writing members to file: {ex.Message}");
            }
        }

        public Member? GetMemberById(int id)
        {
            try
            {
                return ReadMembersFromFile().SingleOrDefault(m => m.MemberId == id);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error retrieving member by ID: {ex.Message}");
                return null;
            }
        }

        public Member? GetMemberByEmail(string email)
        {
            try
            {
                return ReadMembersFromFile().SingleOrDefault(m => m.Email.Equals(email, StringComparison.OrdinalIgnoreCase));
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error retrieving member by email: {ex.Message}");
                return null;
            }
        }

        public Member? GetMemberByEmailAndPassword(string email, string password)
        {
            try
            {
                var member = ReadMembersFromFile().SingleOrDefault(m => m.Email.Equals(email, StringComparison.OrdinalIgnoreCase));

                if (member != null && BCrypt.Net.BCrypt.Verify(password, member.Password))
                {
                    return member;
                }

                return null;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error retrieving member by email and password: {ex.Message}");
                return null;
            }
        }

        public List<Member> GetMembers()
        {
            try
            {
                return ReadMembersFromFile();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error retrieving members: {ex.Message}");
                return new List<Member>();
            }
        }

        public bool AddMember(Member member)
        {
            try
            {
                var members = ReadMembersFromFile();

                if (members.Any(m => m.Email.Equals(member.Email, StringComparison.OrdinalIgnoreCase)))
                {
                    return false;
                }

                member.MemberId = members.Any() ? members.Max(m => m.MemberId) + 1 : 1;

                member.Password = BCrypt.Net.BCrypt.HashPassword(member.Password);

                members.Add(member);
                WriteMembersToFile(members);
                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error adding member: {ex.Message}");
                return false;
            }
        }

        public bool UpdateMember(Member member)
        {
            try
            {
                var members = ReadMembersFromFile();
                var index = members.FindIndex(m => m.MemberId == member.MemberId);

                if (index != -1)
                {
                    members[index] = member;
                    WriteMembersToFile(members);
                    return true;
                }

                return false;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error updating member: {ex.Message}");
                return false;
            }
        }

        public bool DeleteMember(int id)
        {
            try
            {
                var members = ReadMembersFromFile();
                var memberToRemove = members.SingleOrDefault(m => m.MemberId == id);

                if (memberToRemove != null)
                {
                    members.Remove(memberToRemove);
                    WriteMembersToFile(members);
                    return true;
                }

                return false;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error deleting member: {ex.Message}");
                return false;
            }
        }
    }
}
