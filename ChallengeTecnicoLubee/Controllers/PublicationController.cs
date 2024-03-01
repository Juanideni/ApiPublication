using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ChallengeTecnicoLubee.Context;
using ChallengeTecnicoLubee.Models;
using Microsoft.Extensions.Hosting;
using Microsoft.OpenApi.Writers;

namespace ChallengeTecnicoLubee.Controllers
{
    [Route("api/publication")]
    [ApiController]
    public class PublicationController : ControllerBase
    {
        private readonly AppDbContext _context;
        private readonly IWebHostEnvironment env;

        public PublicationController(AppDbContext context, IWebHostEnvironment env)
        {
            _context = context;
            this.env = env;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Publication>>> GetPublications()
        {
            var publications = await _context.Publications.ToListAsync();

            foreach (var publication in publications)
            {
                // Buscar las imágenes asociadas a esta publicación

                var images = await _context.ImagePublications
                    .Where(ip => ip.PublicationId == publication.Id)
                    .ToListAsync();

                if (images.Any())
                {
                    foreach (var image in images)
                    {
                        // Obtener solo el nombre del archivo de la URL
                        string fileName = Path.GetFileName(image.URLImage);

                        image.URLImage = fileName;

                        // Agregar el nombre del archivo a la lista de imágenes de la publicación
                        publication.ImagesPublication.Add(image);

                       
                    }
                }


            }

            return publications;
        }

        // GET: api/Publication/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Publication>> GetPublication(int id)
        {
            var publication = await _context.Publications.FindAsync(id);

            if (publication == null)
            {
                return NotFound();
            }
            // Buscar las imágenes asociadas a esta publicación

            var images = await _context.ImagePublications
                .Where(ip => ip.PublicationId == id)
                .ToListAsync();

            if (images.Any())
            {
                foreach (var image in images)
                {
                    // Obtener solo el nombre del archivo de la URL
                    string fileName = Path.GetFileName(image.URLImage);

                    image.URLImage = fileName;

                    // Agregar el nombre del archivo a la lista de imágenes de la publicación
                    publication.ImagesPublication.Add(image);

                    
                }
            }

            return publication;
        }

        // PUT: api/Publication/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        [DisableRequestSizeLimit, RequestFormLimits(MultipartBodyLengthLimit = int.MaxValue, ValueLengthLimit = int.MaxValue)]
        public async Task<IActionResult> PutPublication(int id, Publication publication)
        {
            if (id != publication.Id)
            {
                return BadRequest();
            }

            _context.Entry(publication).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();

                var images = publication.Images;

                foreach (var image in images)
                {

                    // Verificar y crear la carpeta "images" si no existe
                    string imagesFolder = Path.Combine(Directory.GetCurrentDirectory(), "images");
                    if (!Directory.Exists(imagesFolder))
                    {
                        Directory.CreateDirectory(imagesFolder);
                    }

                    // Verificar y crear la carpeta "publications" si no existe
                    string publicationsFolder = Path.Combine(Directory.GetCurrentDirectory(), "images", "publications");
                    if (!Directory.Exists(publicationsFolder))
                    {
                        Directory.CreateDirectory(publicationsFolder);
                    }

                    var pathDestino = Path.Combine(Directory.GetCurrentDirectory(), "images\\publications");


                    var fileName = Guid.NewGuid().ToString().Replace("-", "") + Path.GetExtension(image.FileName);

                    var filePath = Path.Combine(publicationsFolder, fileName);

                    using (var fileStream = new FileStream(filePath, FileMode.Create))
                    {
                        image.CopyTo(fileStream);

                        ImagePublication newImagePublication = new ImagePublication()
                        {
                            URLImage = filePath,
                            PublicationId = publication.Id,
                        };

                        _context.ImagePublications.Add(newImagePublication);
                        _context.SaveChanges(); // Guardar los cambios en la base de datos
                    }
                }

            }
            catch (DbUpdateConcurrencyException)
            {
                if (!PublicationExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        // POST: api/Publication
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        [DisableRequestSizeLimit, RequestFormLimits(MultipartBodyLengthLimit = int.MaxValue, ValueLengthLimit = int.MaxValue)]
        public async Task<ActionResult<Publication>> PostPublication(Publication publication)
        {
            _context.Publications.Add(publication);
            var newPublicationId = await _context.SaveChangesAsync();

            var images = publication.Images;

            foreach (var image in images)
            {

                // Verificar y crear la carpeta "images" si no existe
                string imagesFolder = Path.Combine(Directory.GetCurrentDirectory(), "images");
                if (!Directory.Exists(imagesFolder))
                {
                    Directory.CreateDirectory(imagesFolder);
                }

                // Verificar y crear la carpeta "publications" si no existe
                string publicationsFolder = Path.Combine(Directory.GetCurrentDirectory(), "images", "publications");
                if (!Directory.Exists(publicationsFolder))
                {
                    Directory.CreateDirectory(publicationsFolder);
                }

                var pathDestino = Path.Combine(Directory.GetCurrentDirectory(), "images\\publications");


                var fileName = Guid.NewGuid().ToString().Replace("-", "") + Path.GetExtension(image.FileName);

                var filePath = Path.Combine(publicationsFolder, fileName);

                using (var fileStream = new FileStream(filePath, FileMode.Create))
                {
                    image.CopyTo(fileStream);

                    ImagePublication newImagePublication = new ImagePublication()
                    {
                        URLImage = filePath,
                        PublicationId = publication.Id,
                    };

                    _context.ImagePublications.Add(newImagePublication);
                    _context.SaveChanges(); // Guardar los cambios en la base de datos
                }
            }

            return CreatedAtAction("GetPublication", new { id = publication.Id }, publication);
        }

        // DELETE: api/Publication/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeletePublication(int id)
        {
            var publication = await _context.Publications.FindAsync(id);
            if (publication == null)
            {
                return NotFound();
            }

            var publicationImages =  _context.ImagePublications.Where(x => x.PublicationId == id).ToList();

            foreach (var image in publicationImages)
            {
                _context.ImagePublications.Remove(image);
            };

            _context.Publications.Remove(publication);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool PublicationExists(int id)
        {
            return _context.Publications.Any(e => e.Id == id);
        }


    }
}
