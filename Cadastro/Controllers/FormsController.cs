using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Cadastro.Context;
using Cadastro.Models;

namespace Cadastro.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class FormsController : ControllerBase
    {
        private readonly AppDbContext _appDbContext;

        public FormsController(AppDbContext appDbContext)
        {
            _appDbContext = appDbContext;
        }

        [HttpGet]
        public async Task<IActionResult> GetForms()
        {

            return Ok(new
            {
                success = true,
                data = await _appDbContext.Forms.ToListAsync()
            });
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetFormById(int id)
        {
            var pessoa = await _appDbContext.Forms.FirstOrDefaultAsync(p => p.Id == id);

            if (pessoa == null || pessoa.Inativo == true) { 
                return NotFound(new 
                { 
                    success = false,
                    message = "Pessoa não encontrada." 
                }); 
            }
            return Ok(new 
            { 
                success = true,
                data = pessoa 
            });
        }

        [HttpPost]
        public async Task<IActionResult> CreateForm(Form form)
        {
            _appDbContext.Forms.Add(form);
            await _appDbContext.SaveChangesAsync();

            return Ok(new
            {
                success = true,
                data = form
            });
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> DeletePessoa(int id, [FromBody] Form pessoaAtualizada)
        {
            var pessoa = await _appDbContext.Forms.FirstOrDefaultAsync(p => p.Id == id);

            if (pessoa == null)
            {
                return NotFound(new
                {
                    success = false,
                    message = "Pessoa não encontrada."
                });
            }
            
            // Atualizar os dados
            pessoa.Name = pessoaAtualizada.Name;
            pessoa.DataNasc = pessoaAtualizada.DataNasc;
            pessoa.Inativo = pessoaAtualizada.Inativo;
            pessoa.Nacionalidade = pessoaAtualizada.Nacionalidade;
            pessoa.RG = pessoaAtualizada.RG;
            // Atualize outros campos conforme necessário

            try
            {
                _appDbContext.Forms.Update(pessoa);
                await _appDbContext.SaveChangesAsync();

                return Ok(new
                {
                    success = true,
                    message = "Pessoa atualizada com sucesso.",
                    data = pessoa
                });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new
                {
                    success = false,
                    message = "Erro ao atualizar pessoa.",
                    error = ex.Message
                });
            }
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeletePessoa(int id)
        {
            var pessoa = await _appDbContext.Forms.FirstOrDefaultAsync(p => p.Id == id);

            if (pessoa == null)
            {
                return NotFound(new
                {
                    success = false,
                    message = "Pessoa não encontrada."
                });
            }

            pessoa.Inativo = true;

            try
            {
                _appDbContext.Forms.Update(pessoa);
                await _appDbContext.SaveChangesAsync();

                return Ok(new
                {
                    success = true,
                    message = "Pessoa deletada com sucesso.",
                    data = pessoa
                });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new
                {
                    success = false,
                    message = "Erro ao deletar pessoa.",
                    error = ex.Message
                });
            }
        }
    }
}
