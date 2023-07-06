using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RestfulApi.DTOs;
using RestfulApi.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RestfulApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class GenresController : ControllerBase
    {
        private readonly ApplicationDbContext _context;
        private readonly IMapper _mapper;

        public GenresController(ApplicationDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        [HttpGet]
        public async Task<ActionResult<List<GenreDTO>>> Get()
        {
            var genres = await _context.Genres.ToListAsync();

            var genresDTO = _mapper.Map<List<GenreDTO>>(genres);

            return genresDTO;
        }

        [HttpGet("{Id}", Name = "getGenre")]
        public async Task<ActionResult<GenreDTO>> Get(int Id)
        {
            var genre = await _context.Genres.FirstOrDefaultAsync(x => x.Id == Id);

            if (genre == null)
                return NotFound();
            var genreDTO = _mapper.Map<GenreDTO>(genre);

            return genreDTO;
        }

        [HttpPost]
        public async Task<ActionResult> Post([FromBody] GenreCreationDTO genreCreation)
        {
            var genre = _mapper.Map<Genre>(genreCreation);
            _context.Add(genre);
            await _context.SaveChangesAsync();

            var genreDTO = _mapper.Map<GenreDTO>(genre);

            return new CreatedAtRouteResult("getGenre", new { Id = genreDTO.Id }, genreDTO);
        }

        [HttpPut("{Id}")]
        public async Task<ActionResult> Put(int Id, [FromBody] GenreCreationDTO genreCreation)
        {
            var genre = _mapper.Map<Genre>(genreCreation);
            genre.Id = Id;
            _context.Entry(genre).State = EntityState.Modified;

            await _context.SaveChangesAsync();

            return NoContent();
        }

        [HttpDelete("{Id}")]
        public async Task<ActionResult> Delete(int Id)
        {
            var genre = _context.Genres.FirstOrDefault(x => x.Id == Id);

            if (genre == null)
                return NotFound();

            _context.Genres.Remove(genre);

            await _context.SaveChangesAsync();

            return NoContent();
        }
    }
}
