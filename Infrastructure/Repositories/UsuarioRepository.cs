using APIUsuarios.Application.Interfaces;
using APIUsuarios.Domain.Entities;
using APIUsuarios.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace APIUsuarios.Infrastructure.Repositories
{
    public class UsuarioRepository : IUsuarioRepository
    {
        private readonly AppDbContext _context;

        public UsuarioRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Usuario>> GetAllAsync(CancellationToken ct = default)
        {
            return await _context.Usuarios
                .AsNoTracking()
                .ToListAsync(ct);
        }

        public async Task<Usuario?> GetByIdAsync(int id, CancellationToken ct = default)
        {
            return await _context.Usuarios
                .FirstOrDefaultAsync(u => u.Id == id, ct);
        }

        public async Task<Usuario?> GetByEmailAsync(string email, CancellationToken ct = default)
        {
            return await _context.Usuarios
                .FirstOrDefaultAsync(u => u.Email == email.ToLower(), ct);
        }

        public async Task AddAsync(Usuario usuario, CancellationToken ct = default)
        {
            await _context.Usuarios.AddAsync(usuario, ct);
            await SaveChangesAsync(ct);
        }

        public async Task UpdateAsync(Usuario usuario, CancellationToken ct = default)
        {
            _context.Usuarios.Update(usuario);
            await SaveChangesAsync(ct);
        }

        public async Task RemoveAsync(Usuario usuario, CancellationToken ct = default)
        {
            _context.Usuarios.Remove(usuario);
            await SaveChangesAsync(ct);
        }

        public async Task<bool> EmailExistsAsync(string email, CancellationToken ct = default)
        {
            return await _context.Usuarios
                .AnyAsync(u => u.Email == email.ToLower(), ct);
        }

        public async Task<int> SaveChangesAsync(CancellationToken ct = default)
        {
            return await _context.SaveChangesAsync(ct);
        }
    }
}
