using BusinessObject.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace SalesWPFApp.DAOs
{
    public class RoleDAO
    {
        private static RoleDAO? _instance;
        private readonly string _jsonFilePath = "roles.json";

        // Singleton pattern
        public static RoleDAO Instance => _instance ??= new RoleDAO();

        private RoleDAO()
        {
            // Tạo file JSON nếu chưa tồn tại
            if (!File.Exists(_jsonFilePath))
            {
                File.WriteAllText(_jsonFilePath, "[]");
            }
        }

        // Đọc danh sách Roles từ file JSON
        private List<Role> ReadRolesFromFile()
        {
            var jsonData = File.ReadAllText(_jsonFilePath);
            return JsonSerializer.Deserialize<List<Role>>(jsonData) ?? new List<Role>();
        }

        // Ghi danh sách Roles vào file JSON
        private void WriteRolesToFile(List<Role> roles)
        {
            var jsonData = JsonSerializer.Serialize(roles, new JsonSerializerOptions { WriteIndented = true });
            File.WriteAllText(_jsonFilePath, jsonData);
        }

        // Lấy tất cả các Roles
        public List<Role> GetAllRoles()
        {
            return ReadRolesFromFile();
        }

        // Lấy Role theo RoleId
        public Role? GetRoleById(int roleId)
        {
            var roles = ReadRolesFromFile();
            return roles.SingleOrDefault(r => r.RoleId == roleId);
        }

        // Thêm Role mới
        public void AddRole(Role role)
        {
            var roles = ReadRolesFromFile();

            // Kiểm tra trùng lặp RoleId
            if (roles.Any(r => r.RoleId == role.RoleId))
            {
                throw new Exception($"Role với RoleId = {role.RoleId} đã tồn tại.");
            }

            roles.Add(role);
            WriteRolesToFile(roles);
        }

        // Cập nhật Role
        public void UpdateRole(Role updatedRole)
        {
            var roles = ReadRolesFromFile();
            var index = roles.FindIndex(r => r.RoleId == updatedRole.RoleId);

            if (index != -1)
            {
                roles[index].RoleName = updatedRole.RoleName;
                WriteRolesToFile(roles);
            }
            else
            {
                throw new Exception($"Role với RoleId = {updatedRole.RoleId} không tồn tại.");
            }
        }

        // Xóa Role theo RoleId
        public bool DeleteRole(int roleId)
        {
            var roles = ReadRolesFromFile();
            var roleToRemove = roles.SingleOrDefault(r => r.RoleId == roleId);

            if (roleToRemove != null)
            {
                roles.Remove(roleToRemove);
                WriteRolesToFile(roles);
                return true;
            }
            return false;
        }
    }
}
