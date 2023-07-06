using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RestfulApi.DTOs;
using RestfulApi.Entities;
using RestfulApi.Services;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace RestfulApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MoviesController : ControllerBase
    {
        private readonly ApplicationDbContext _context;
        private readonly IMapper _mapper;
        private readonly IFileStorageService _fileStorageService;
        private readonly string containerName = "movies";

        public MoviesController(ApplicationDbContext context, IMapper mapper
            , IFileStorageService fileStorageService)
        {
            _context = context;
            _mapper = mapper;
            _fileStorageService = fileStorageService;
        }

        [HttpGet]
        public async Task<ActionResult<List<MovieDTO>>> Get()
        {
            var movie = _context.Movies.ToListAsync();
            return _mapper.Map<List<MovieDTO>>(movie);
        }

        [HttpGet("{Id}", Name ="getMovie")]
        public async Task<ActionResult<MovieDTO>> Get(int Id)
        {
            var movie = await _context.Movies.FirstOrDefaultAsync(x => x.Id == Id);

            if (movie == null)
                return NotFound();

            var movieDTO = _mapper.Map<MovieDTO>(movie);

            return movieDTO;
        }

        [HttpPost]
        public async Task<ActionResult> Post([FromForm] MovieCreationDTO movieCreationDTO)
        {
            var movie = _mapper.Map<Movie>(movieCreationDTO);

            if(movieCreationDTO.Poster != null)
            {
                using (var memoryStream = new MemoryStream())
                {
                    await movieCreationDTO.Poster.CopyToAsync(memoryStream);
                    var content = memoryStream.ToArray();
                    var extension = Path.GetExtension(movieCreationDTO.Poster.FileName);
                    movie.Poster = await _fileStorageService.SaveFile(content, extension, containerName, movieCreationDTO.Poster.ContentType);
                }
            }

            _context.Add(movie);
            await _context.SaveChangesAsync();
            var movieDTO = _mapper.Map<MovieDTO>(movie);

            return new CreatedAtRouteResult("getMovie", new { Id = movieDTO.Id}, movieDTO);
        }

        [HttpPut("Id")]
        public async Task<ActionResult> Put(int Id, MovieCreationDTO movieCreationDTO)
        {
            var movieDB = await _context.Movies.FirstOrDefaultAsync(x => x.Id == Id);

            if(movieDB == null)
            {
                return NotFound();
            }

            movieDB = _mapper.Map(movieCreationDTO, movieDB);

            if(movieCreationDTO.Poster != null)
            {
                using (var memoryStream = new MemoryStream())
                {
                    await movieCreationDTO.Poster.CopyToAsync(memoryStream);
                    var content = memoryStream.ToArray();
                    var extension = Path.GetExtension(movieCreationDTO.Poster.FileName);
                    movieDB.Poster = await _fileStorageService.EditFile(content, extension, containerName
                                                                            , movieDB.Poster, movieCreationDTO.Poster.ContentType);
                }
            }

            await _context.SaveChangesAsync();
            return NoContent();
        }

        [HttpPatch("Id")]
        public async Task<ActionResult> Patch(int Id, [FromBody] JsonPatchDocument<MoviePatchDTO> patchDocument)
        {
            if(patchDocument == null)
            {
                return BadRequest();
            }

            var entityFromDB = await _context.Movies.FirstOrDefaultAsync(x => x.Id == Id);

            if (entityFromDB == null)
            {
                return NotFound();
            }

            var patchDTO = _mapper.Map<MoviePatchDTO>(entityFromDB);

            patchDocument.ApplyTo(patchDTO, ModelState);

            var isValid = TryValidateModel(patchDTO);

            if (!isValid)
            {
                return BadRequest(ModelState);
            }

            _mapper.Map(patchDTO, entityFromDB);

            await _context.SaveChangesAsync();

            return NoContent();
        }

        [HttpDelete("{Id}")]
        public async Task<ActionResult> Delete(int Id)
        {
            var exists = await _context.Movies.AnyAsync(x => x.Id == Id);

            if(!exists)
            {
                return NotFound();
            }

            _context.Remove(new Movie() { Id = Id});

            await _context.SaveChangesAsync();

            return NoContent();
        }
    }
}
