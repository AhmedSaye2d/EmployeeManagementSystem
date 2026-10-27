using BaseLibrary.Entities;
using BaseLibrary.Respones;
using HostLibrary.Data;
using HostLibrary.Repository.Contracts;
using Microsoft.EntityFrameworkCore;
namespace HostLibrary.Repository.Implemntation
{
    public class DepartmentRepository : IGenericRepositoryInterface<Department>
    {
        private readonly AppDbContext _context;
        public DepartmentRepository(AppDbContext context)
        {
            _context = context;
        }
        public async Task<GeneralResponse> DeleteById(int id)
        {
            var dept = await _context.Departments.FindAsync(id);

            if (dept is null) return NotFound();

            _context.Departments.Remove(dept);
            await Commite();
            return Success();
        }

        public async Task<List<Department>> GetAll()
        {
            return await _context.Departments.ToListAsync();
        }

        public async Task<Department> GetById(int id)
        {
            return await _context.Departments.FindAsync(id);
        }

        public async Task<GeneralResponse> Insert(Department item)
        {
            if (!await CheckName(item.Name!)) return new GeneralResponse(false, "Department already Add");
            await _context.Departments.AddAsync(item);
            await Commite();
            return Success();
        }

        public async Task<GeneralResponse> Update(Department item)
        {
            var dept = await _context.GeneralDepartments.FindAsync(item.Id);
            if (dept == null)
            {
                return NotFound();
            }
            dept.Name = item.Name;
            await Commite();
            return Success();
        }
        private static GeneralResponse NotFound() => new(false, "Sorry department not found");
        private static GeneralResponse Success() => new GeneralResponse(true, "Process completed");
        public async Task Commite() => await _context.SaveChangesAsync();

        private async Task<bool> CheckName(string name)
        {
            var item = await _context.Departments.FirstOrDefaultAsync(c => c.Name!.ToLower().Equals(name.ToLower()));
            return item is null;
        }
    }
}

