using Microsoft.EntityFrameworkCore;
using SocialCrap.Data;
using SocialCrap.DTOs;
using SocialCrap.Models;

namespace SocialCrap.Service
{
    // Regras de negocio para Noticia.
    public class NoticiaService : INoticiaService
    {
        private readonly AppDbContext _context;

        public NoticiaService(AppDbContext context)
        {
            _context = context;
        }

        public async Task<List<NoticiaResponse>> GetAllAsync()
        {
            // Lista com nome do autor para o front.
            return await _context.Noticias
                .Include(n => n.Autor)
                .Select(n => new NoticiaResponse
                {
                    Id = n.Id,
                    Titulo = n.Titulo,
                    Conteudo = n.Conteudo,
                    Data = n.Data,
                    AutorId = n.AutorId,
                    AutorNome = n.Autor.Nome
                })
                .ToListAsync();
        }

        public async Task<OperationResult<NoticiaResponse>> GetByIdAsync(int id)
        {
            var noticia = await _context.Noticias
                .Include(n => n.Autor)
                .Where(n => n.Id == id)
                .Select(n => new NoticiaResponse
                {
                    Id = n.Id,
                    Titulo = n.Titulo,
                    Conteudo = n.Conteudo,
                    Data = n.Data,
                    AutorId = n.AutorId,
                    AutorNome = n.Autor.Nome
                })
                .FirstOrDefaultAsync();

            if (noticia == null)
                return OperationResult<NoticiaResponse>.NotFoundResult();

            return OperationResult<NoticiaResponse>.Ok(noticia);
        }

        public async Task<OperationResult<NoticiaResponse>> CreateAsync(NoticiaCreateRequest request)
        {
            // Valida campos obrigatorios.
            if (string.IsNullOrWhiteSpace(request.Titulo) || string.IsNullOrWhiteSpace(request.Conteudo))
                return OperationResult<NoticiaResponse>.Fail("Titulo e conteudo sao obrigatorios.");

            if (request.AutorId <= 0)
                return OperationResult<NoticiaResponse>.Fail("AutorId e obrigatorio.");

            // Garante que o autor existe.
            var autor = await _context.Usuarios.FindAsync(request.AutorId);
            if (autor == null)
                return OperationResult<NoticiaResponse>.Fail("Autor nao encontrado.");

            var noticia = new Noticia(request.Titulo.Trim(), request.Conteudo.Trim(), request.AutorId);

            _context.Noticias.Add(noticia);
            await _context.SaveChangesAsync();

            var response = new NoticiaResponse
            {
                Id = noticia.Id,
                Titulo = noticia.Titulo,
                Conteudo = noticia.Conteudo,
                Data = noticia.Data,
                AutorId = noticia.AutorId,
                AutorNome = autor.Nome
            };

            return OperationResult<NoticiaResponse>.Ok(response);
        }

        public async Task<OperationResult<Noticia>> UpdateAsync(int id, NoticiaUpdateRequest request)
        {
            // Valida conteudo antes de buscar.
            if (string.IsNullOrWhiteSpace(request.Titulo) || string.IsNullOrWhiteSpace(request.Conteudo))
                return OperationResult<Noticia>.Fail("Titulo e conteudo sao obrigatorios.");

            var noticia = await _context.Noticias.FindAsync(id);
            if (noticia == null)
                return OperationResult<Noticia>.NotFoundResult();

            var atualizado = noticia.Atualizar(request.Titulo.Trim(), request.Conteudo.Trim());
            if (!atualizado)
                return OperationResult<Noticia>.Fail("Titulo e conteudo invalidos.");

            await _context.SaveChangesAsync();
            return OperationResult<Noticia>.Ok(noticia);
        }

        public async Task<OperationResult> DeleteAsync(int id)
        {
            var noticia = await _context.Noticias.FindAsync(id);
            if (noticia == null)
                return OperationResult.NotFoundResult();

            _context.Noticias.Remove(noticia);
            await _context.SaveChangesAsync();

            return OperationResult.Ok();
        }
    }
}
