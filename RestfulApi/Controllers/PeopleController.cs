using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RestfulApi.DTOs;
using RestfulApi.Entities;
using RestfulApi.Helpers;
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
    public class PeopleController : ControllerBase
    {
        private readonly ApplicationDbContext _context;
        private readonly IMapper _mapper;
        private readonly IFileStorageService _fileStorageService;
        private readonly string containerName = "people";

        public PeopleController(ApplicationDbContext context, IMapper mapper, IFileStorageService fileStorageService)
        {
            _context = context;
            _mapper = mapper;
            _fileStorageService = fileStorageService;
        }

        [HttpGet]
        public async Task<ActionResult<List<PersonDTO>>> Get([FromQuery] PaginationDTO pagination)
        {
            var queryable = _context.People.AsQueryable();

            await HttpContext.InsertPaginationParametersInResponse(queryable, pagination.RecordsPegPage);

            var people = await queryable.Paginate(pagination).ToListAsync();

            var personDTO = _mapper.Map<List<PersonDTO>>(people);

            return personDTO;
        }

        [HttpGet("{Id}", Name = "getPerson")]
        public async Task<ActionResult<PersonDTO>> Get(int Id)
        {
            var person = await _context.People.FirstOrDefaultAsync(x => x.Id == Id);

            if (person == null)
                return NotFound();

            var personDTO = _mapper.Map<PersonDTO>(person);

            return personDTO;
        }

        [HttpPost]
        public async Task<ActionResult> Post([FromForm] PersonCreationDTO personCreationDTO)
        {
            var person = _mapper.Map<Person>(personCreationDTO);

            if(personCreationDTO.Picture != null)
            {
                using (var memoryStream = new MemoryStream())
                {
                    await personCreationDTO.Picture.CopyToAsync(memoryStream);
                    var content = memoryStream.ToArray();
                    var extension = Path.GetExtension(personCreationDTO.Picture.FileName);
                    person.Picture = await _fileStorageService.SaveFile(content, extension, containerName, personCreationDTO.Picture.ContentType);

                }
            }

            _context.Add(person);
            await _context.SaveChangesAsync();
            var personDTO = _mapper.Map<PersonDTO>(person);

            return new CreatedAtRouteResult("getPerson", new { Id = personDTO.Id}, personDTO);
        }

        [HttpPut("{Id}")]
        public async Task<ActionResult> Put(int Id, [FromForm] PersonCreationDTO personCreationDTO)
        {
            var personDB = await _context.People.FirstOrDefaultAsync(x => x.Id == Id);

            if(personDB == null) { return NotFound(); }

            personDB = _mapper.Map(personCreationDTO, personDB);

            if (personCreationDTO.Picture != null)
            {
                using (var memoryStream = new MemoryStream())
                {
                    await personCreationDTO.Picture.CopyToAsync(memoryStream);
                    var content = memoryStream.ToArray();
                    var extension = Path.GetExtension(personCreationDTO.Picture.FileName);
                    personDB.Picture = await _fileStorageService.EditFile(content, extension, containerName
                                                                            , personDB.Picture, personCreationDTO.Picture.ContentType);
                }
            }

            await _context.SaveChangesAsync();
            return NoContent();
        }

        [HttpPatch("{Id}")]
        public async Task<ActionResult> Patch(int Id, [FromBody] JsonPatchDocument<PersonPatchDTO> patchDocument)
        {
            if(patchDocument == null)
            {
                return BadRequest();
            }

            var entityFromDB = await _context.People.FirstOrDefaultAsync(x => x.Id == Id);

            if(entityFromDB == null)
            {
                return NotFound();
            }

            var patchDTO = _mapper.Map<PersonPatchDTO>(entityFromDB);

            patchDocument.ApplyTo(patchDTO, ModelState);

            var isValid = TryValidateModel(patchDTO);

            if(!isValid)
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
            var person = _context.People.FirstOrDefault(x => x.Id == Id);

            if (person == null)
                return NotFound();

            await _fileStorageService.DeleteFile(person.Picture, containerName);

            _context.People.Remove(person);

            await _context.SaveChangesAsync();

            return NoContent();
        }
    }
}
