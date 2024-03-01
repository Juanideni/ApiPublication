using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ChallengeTecnicoLubee.Context;
using ChallengeTecnicoLubee.Models;

namespace ChallengeTecnicoLubee.Controllers
{
    [Route("api/imagePublication")]
    [ApiController]
    public class ImagePublicationController : ControllerBase
    {
        private readonly AppDbContext _context;

        public ImagePublicationController(AppDbContext context)
        {
            _context = context;
        }

        // GET: api/ImagePublication
        [HttpGet]
        public async Task<ActionResult<IEnumerable<ImagePublication>>> GetImagePublications()
        {
            return await _context.ImagePublications.ToListAsync();
        }

        // GET: api/ImagePublication/5
        [HttpGet("/by-url-publicationId/{id}")]
        public async Task<ActionResult<ImagePublication>> GetImageByURLImageAndPublicationId(string URLImage, int publicationId)
        {
            var imagePublication =  _context.ImagePublications.Where(x => x.URLImage == URLImage && x.PublicationId == publicationId).FirstOrDefault();

            if (imagePublication == null)
            {
                return NotFound();
            }

            return imagePublication;
        }

        // GET: api/ImagePublication/by-publicationId/5
        [HttpGet("/by-publicationId/{publicationId}")]
        public async Task<ActionResult<List<ImagePublication>>> GetImageListByPublicationId(int publicationId)
        {
            List<ImagePublication> listImages = new List<ImagePublication>();
            var imagePublication = _context.ImagePublications.Where(x => x.PublicationId == publicationId).ToList();

            listImages = imagePublication.ToList();

            return listImages;
        }


        // POST: api/ImagePublication
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<ImagePublication>> PostImagePublication(ImagePublication imagePublication)
        {
            _context.ImagePublications.Add(imagePublication);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetImagePublication", new { id = imagePublication.Id }, imagePublication);
        }

        // DELETE: api/ImagePublication/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteImagePublication(int id)
        {
            var imagePublication = await _context.ImagePublications.FindAsync(id);
            if (imagePublication == null)
            {
                return NotFound();
            }

            _context.ImagePublications.Remove(imagePublication);
            await _context.SaveChangesAsync();

            return NoContent();
        }

    }
}
