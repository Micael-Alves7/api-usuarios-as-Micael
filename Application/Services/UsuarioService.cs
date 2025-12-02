using APIUsuarios.Application.DTOs;
using APIUsuarios.Application.Interfaces;
using APIUsuarios.Domain.Entities;

namespace APIUsuarios.Application.Services;

public class UsuarioService : IUsuarioService
{
    private readonly IUsuarioRepository _repo;

    public UsuarioService(IUsuarioRepository repo)
    {
        _repo = repo;
    }

    public async Task<IEnumerable<UsuarioReadDto>> ListarAsync(CancellationToken ct = default)
    {
        var usuarios = await _repo.GetAllAsync(ct);
        return usuarios.Select(u => new UsuarioReadDto(
            u.Id, u.Nome, u.Email, u.DataNascimento, u.Telefone, u.Ativo, u.DataCriacao
        ));
    }

    public async Task<UsuarioReadDto?> ObterAsync(int id, CancellationToken ct = default)
    {
        if (id <= 0)
            return null;

        var usuario = await _repo.GetByIdAsync(id, ct);
        if (usuario == null)
            return null;

        return new UsuarioReadDto(
            usuario.Id, usuario.Nome, usuario.Email, usuario.DataNascimento,
            usuario.Telefone, usuario.Ativo, usuario.DataCriacao
        );
    }

    public async Task<UsuarioReadDto> CriarAsync(UsuarioCreateDto dto, CancellationToken ct = default)
    {
        var usuario = new Usuario
        {
            Nome = dto.Nome,
            Email = dto.Email.ToLower(),
            Senha = dto.Senha,
            DataNascimento = dto.DataNascimento,
            Telefone = dto.Telefone,
            Ativo = true,
            DataCriacao = DateTime.UtcNow
        };

        await _repo.AddAsync(usuario, ct);

        return new UsuarioReadDto(
            usuario.Id, usuario.Nome, usuario.Email, usuario.DataNascimento,
            usuario.Telefone, usuario.Ativo, usuario.DataCriacao
        );
    }

    public async Task<UsuarioReadDto> AtualizarAsync(int id, UsuarioUpdateDto dto, CancellationToken ct = default)
    {
        var usuario = await _repo.GetByIdAsync(id, ct);
        if (usuario == null)
            return null!;

        usuario.Nome = dto.Nome;
        usuario.Email = dto.Email.ToLower();
        usuario.DataNascimento = dto.DataNascimento;
        usuario.Telefone = dto.Telefone;
        usuario.Ativo = dto.Ativo;
        usuario.DataAtualizacao = DateTime.UtcNow;

        await _repo.UpdateAsync(usuario, ct);

        return new UsuarioReadDto(
            usuario.Id, usuario.Nome, usuario.Email, usuario.DataNascimento,
            usuario.Telefone, usuario.Ativo, usuario.DataCriacao
        );
    }

    public async Task<bool> RemoverAsync(int id, CancellationToken ct = default)
    {
        if (id <= 0)
            return false;

        var usuario = await _repo.GetByIdAsync(id, ct);
        if (usuario == null)
            return false;

        usuario.Ativo = false;
        usuario.DataAtualizacao = DateTime.UtcNow;
        await _repo.UpdateAsync(usuario, ct);

        return true;
    }

    public async Task<bool> EmailJaCadastradoAsync(string email, CancellationToken ct = default)
    {
        return await _repo.EmailExistsAsync(email.ToLower(), ct);
    }

    public async Task<bool> EmailJaCadastradoParaOutroAsync(string email, int id, CancellationToken ct = default)
    {
        var usuario = await _repo.GetByEmailAsync(email.ToLower(), ct);
        return usuario != null && usuario.Id != id;
    }
}
